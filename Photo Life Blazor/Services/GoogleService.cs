using System;
using System.Threading.Tasks;
using MetadataExtractor;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.Drive.v3;
using Google.Apis.Download;
using System.Net.Http.Headers;
using Google.Apis.Oauth2.v2;
using Photo_Life_Blazor.Models;
using System.Security.Cryptography.Xml;
using Newtonsoft.Json;
using System.Text;
using System.Linq.Expressions;
using System.IO;
using Google.Apis.Json;
using metadata_extractor.Models;

namespace Photo_Life_Blazor.Services
{
    public class GoogleService
    {
        UserCredential? credential;
        DriveService? driveService;
        string? username;
        string folderID = "default";
        public async Task<string> setUp()
        {

            Console.WriteLine("Setting up");
            credential ??= await GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = Constants.ClientID,
                    ClientSecret = Constants.ClientSecret
                                       
                },
                new[] {
                    DriveService.Scope.Drive,
                    Oauth2Service.Scope.UserinfoEmail,
                    Oauth2Service.Scope.UserinfoProfile,
                    Oauth2Service.Scope.Openid
                },

                "user",
                CancellationToken.None,
                new FileDataStore("PhotoLife")) ;
            driveService ??= new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "PhotoLife"
            });

            if (username == null)
            {
                Oauth2Service userInfoService = new Oauth2Service(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "PhotoLife"
                });
                var userInfo = await userInfoService.Userinfo.Get().ExecuteAsync();
                username = userInfo.Email;
                Console.WriteLine(username);
            }
            if (folderID == "default") {
                List<string> folderId = new List<string>();
                var request = driveService.Files.List();
                request.Q = "mimeType = 'application/vnd.google-apps.folder' and trashed = false and name = 'PhotoLife'";
                var fileList = await request.ExecuteAsync();
                foreach (var file in fileList.Files)
                {
                    folderId.Add(file.Id);
                }
                if (folderId.Any())
                {
                    folderID = folderId[0];
                    Console.WriteLine(folderID);
                }
            }
            return username;
        }
        public async Task<List<string>> getFileIds()
        {
            List<string> fileIds = new List<string>();
            var request = driveService.Files.List();
            var previousIds = await getStoredPhotos(username);
            request.Q = "mimeType contains 'image/' and trashed = false and '" + folderID + "' in parents";
            request.PageSize = 1000;
            var fileList = await request.ExecuteAsync();
            
            foreach (var file in fileList.Files)
            {
                if (!(previousIds.Contains(file.Id)))
                {
                    fileIds.Add(file.Id);
                }
            }
            return fileIds;
        }
        public async Task<List<MemoryStream>> downloadFiles(List<String> ids)
        {
            List<MemoryStream> streams = new List<MemoryStream>();
            List<string> failedIds = new List<string>();
            foreach (string id in ids)
            {
                var request = driveService.Files.Get(id);
                var stream = new MemoryStream();
                request.MediaDownloader.ProgressChanged +=
                    progress =>
                    {
                        switch (progress.Status)
                        {
                            case DownloadStatus.Downloading:
                                {
                                    Console.WriteLine(progress.BytesDownloaded);
                                    break;
                                }
                            case DownloadStatus.Completed:
                                {
                                    Console.WriteLine("Download complete.");
                                    break;
                                }
                            case DownloadStatus.Failed:
                                {
                                    Console.WriteLine("Download failed.");
                                    failedIds.Add(id);
                                    break;
                                }
                        }
                    };
                var range = new RangeHeaderValue(0, 64000);
                await request.DownloadRangeAsync(stream, range);
                stream.Position = 0;
                streams.Add(stream);
            }
            foreach (var id in failedIds)
            {
                ids.Remove(id);
            }
            return streams;
        }
        public async Task<int> writeStreamstoFile(List<MemoryStream> streams, List<string> Ids)
        {
            Console.WriteLine("Writing Files");
            for (int i = 0; i < Ids.Count; i++)
            {
                var path = System.IO.Directory.GetCurrentDirectory();
                FileStream file = File.Create(path + "\\Photos\\" + Ids[i]);
                await streams[i].CopyToAsync(file);
                file.Close();
            }
            return 1;
        }
        
        public async Task<string> sendFilePathstoDB(List<string> Ids, int FilesCreation)
        {
            Console.WriteLine("Sending to DB");
            string base_path  = System.IO.Directory.GetCurrentDirectory() + "\\Photos\\";
            base_path = base_path.Replace(@"\", @"\\");
            var pathStr = "{\"owner\" : \""+username+"\",\"file_paths\" : [";
            foreach (var id in Ids)
            {
                pathStr += ("\""+base_path + id + "\",");
            }
            pathStr = pathStr.TrimEnd(',');
            pathStr += "]}";
            var content = new StringContent(pathStr, Encoding.UTF8, "application/json");
            Console.WriteLine(pathStr);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.PostAsync("https://localhost:7214/api/metadata/InsertToDB", content);
            return await response.Content.ReadAsStringAsync();
        }
        public void deleteFiles(List<string> Ids)
        {
            foreach (var id in Ids)
            {
                var path = System.IO.Directory.GetCurrentDirectory();
                File.Delete(path + "\\Photos\\" + id);
            }
        }

        public async Task<string> createFolder(string folderName)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = folderName,
                MimeType = "application/vnd.google-apps.folder"
            };
            var request = driveService.Files.Create(fileMetadata);
            request.Fields = "id";
            var file = await request.ExecuteAsync();
            // Prints the created folder id.
            Console.WriteLine("Folder ID: " + file.Id);
            return file.Id;
        }
        public async Task movePhotos(List<string> PhotoIds, string folderId)
        {
            foreach (var id in PhotoIds)
            {
                var copyRequest = driveService.Files.Copy(null, id);
                copyRequest.Fields = "id";
                var newId = await copyRequest.ExecuteAsync();
                var request = driveService.Files.Update(null,newId.Id);
                request.AddParents = folderId;
                await request.ExecuteAsync();
            }
        }
        public async Task<List<string>> getStoredPhotos(string username)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent("{\"username\": \""+username+"\"}", Encoding.UTF8,
                                    "application/json");
            var response = await client.PostAsync("https://localhost:7214/api/metadata/GetAll", content);
            
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var ids = await response.Content.ReadAsStringAsync();  //Make sure to add a reference to System.Net.Http.Formatting.dll
                ids = ids.Replace("[", "");
                ids = ids.Replace("\"", "");
                ids = ids.Replace("]", "");
                var list_ids = ids.Split(",").ToList();
                Console.WriteLine(ids);
                return list_ids;
            }
            else
            {
                Console.WriteLine("getStoredPhotos");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return new List<string>();
            }

        }
        public async Task<string> getUserPhotos()
        {
            await setUp();
            var ids = await getFileIds();
            var memoryStreams = await downloadFiles(ids);
            var fileCreation = await writeStreamstoFile(memoryStreams, ids);
            //var folder = await createFolder("testing");
            Console.WriteLine(await sendFilePathstoDB(ids, fileCreation));
            //await movePhotos(ids, folder);
            //if (fileCreation == 1)
            //{
            //    deleteFiles(ids);
            //}
            return username;
        }
        public async Task<bool> albumGenerator(ResponseModel options)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //Console.WriteLine(test);
            //Console.WriteLine("{\"username\": \"" + username + "\"," +
            //                                 "\"options\" : \"" + options + "\"}");
            string options_string = Newtonsoft.Json.JsonConvert.SerializeObject(options);
            //options_string = options_string.Replace("{", "");
            //options_string = options_string.Replace("}", "");
            var content = new StringContent(options_string, Encoding.UTF8,
                                    "application/json");
            var response = await client.PostAsync("https://localhost:7214/api/metadata/GetFilteredData", content);
            Console.WriteLine(options_string);
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var ids = await response.Content.ReadAsStringAsync();  //Make sure to add a reference to System.Net.Http.Formatting.dll
                ids = ids.Replace("[", "");
                ids = ids.Replace("\"", "");
                ids = ids.Replace("]", "");
                var list_ids = ids.Split(",").ToList();
                Console.WriteLine("IDS:");
                Console.WriteLine(ids);
                string folderId = await createFolder("output");
                await movePhotos(list_ids, folderId);
                return true;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return false;
            }
        }
    }
}
