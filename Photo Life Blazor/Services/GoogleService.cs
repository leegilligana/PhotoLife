using System;
using System.Threading.Tasks;
using MetadataExtractor;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.Drive.v3;
using Google.Apis.Download;
using Google.Apis.Gmail.v1.Data;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authentication;
using Google.Apis.Oauth2;
using System.Net;
using Google.Apis.Oauth2.v2.Data;
using Google.Apis.Gmail.v1;
using Google.Apis.PeopleService.v1;
using Google.Apis.Oauth2.v2;
using Google.Apis.PeopleService.v1.Data;

namespace Photo_Life_Blazor.Services
{
    public class GoogleService
    {
        UserCredential? credential;
        DriveService? driveService;
        string? username;
        string folderID = "default";
        public async Task setUp()
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
                else
                {
                    Console.WriteLine("OOOPIES");
                }
            }
        }
        public async Task<List<string>> getFileIds()
        {
            List<string> fileIds = new List<string>();
            var request = driveService.Files.List();
            request.Q = "mimeType contains 'image/' and trashed = false and '" + folderID + "' in parents";
            var fileList = await request.ExecuteAsync();
            foreach (var file in fileList.Files)
            {
                fileIds.Add(file.Id);
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
                streams[i].CopyTo(file);
                file.Close();
            }
            return 1;
        }
        public void deleteFiles(List<string> Ids)
        {
            foreach (var id in Ids)
            {
                var path = System.IO.Directory.GetCurrentDirectory();
                File.Delete(path + "\\Photos\\" + id);
            }
        }

        public void readMetadata(string name)
        {
            var path = System.IO.Directory.GetCurrentDirectory();
            IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(path + "\\Photos\\" + name);
            foreach (var directory in directories)
                foreach (var tag in directory.Tags)
                    Console.WriteLine($"{directory.Name} - {tag.Name} = {tag.Description}");
        }
        public async Task getUserPhotos()
        {
            await setUp();
            var ids = await getFileIds();
            var memoryStreams = await downloadFiles(ids);
            var fileCreation = await writeStreamstoFile(memoryStreams, ids);
        }
    }
}
