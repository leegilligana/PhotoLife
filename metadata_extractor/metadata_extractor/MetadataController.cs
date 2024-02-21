using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using DB_Queries;
using System;
using Newtonsoft.Json;
using metadata_extractor.Models;
using MetadataExtractor.Util;
using static System.Collections.Specialized.BitVector32;
using System.ComponentModel;
using metadata_extractor;

namespace metadata_extractor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetadataController : ControllerBase
    {
        [HttpPost("[action]")]
        public string[] GetFilteredData([FromBody] ResponseModel responseModel)
        {
            Queries queries = new Queries(Constants.connString, responseModel.username);
            var filteredByFieldPhotos = new List<string>(); ;
            var listOfFilteredPhotos = new List<List<string>>();
            
            Type typeResponseModel = responseModel.GetType();
       
            foreach (PropertyDescriptor field in TypeDescriptor.GetProperties(typeResponseModel)) { 
                if (field.GetValue(responseModel) != null && field.Name != "username")
                {
                    switch (field.Name)
                    {
                        case "inputIsFlash":
                            filteredByFieldPhotos = queries.Flash((bool)responseModel.inputIsFlash).ToList();
                            break;
                        case "inputModel":
                            filteredByFieldPhotos = queries.Model(responseModel.inputModel).ToList();
                            break;
                        case "inputTimeRange":
                            filteredByFieldPhotos = queries.DateTimeRange(responseModel.inputTimeRange[0], responseModel.inputTimeRange[1]).ToList();
                            break;
                        case "inputCoord":
                            filteredByFieldPhotos = queries.Location(responseModel.inputCoord[0], responseModel.inputCoord[1], (int)responseModel.inputCoord[2]).ToList();
                            break;
                        case "inputWidth":
                            filteredByFieldPhotos = queries.Width((int)responseModel.inputWidth).ToList();
                            break;
                        case "inputHeight":
                            filteredByFieldPhotos = queries.Height((int)responseModel.inputHeight).ToList();
                            break;
                        case "inputOrientation":
                            filteredByFieldPhotos = queries.Orientation(responseModel.inputOrientation).ToList();
                            break;
                        case "inputXRes":
                            filteredByFieldPhotos = queries.XResolution((int)responseModel.inputXRes).ToList();
                            break;
                        case "inputYRes":
                            filteredByFieldPhotos = queries.YResolution((int)responseModel.inputYRes).ToList();
                            break;
                        case "inputSoftware":
                            filteredByFieldPhotos = queries.Software(responseModel.inputSoftware).ToList();
                            break;
                        case "inputExposureTime":
                            filteredByFieldPhotos = queries.ExposureTime((double)responseModel.inputExposureTime).ToList();
                            break;
                        case "inputShutterSpeedValue":
                            filteredByFieldPhotos = queries.ShutterSpeedValue((double)responseModel.inputShutterSpeedValue).ToList();
                            break;
                        case "inputBrightnessValue":
                            filteredByFieldPhotos = queries.BrightnessValue((double)responseModel.inputBrightnessValue).ToList();
                            break;
                        case "inputSceneType":
                            filteredByFieldPhotos = queries.SceneType(responseModel.inputSceneType).ToList();
                            break;
                        case "inputExposureMode":
                            filteredByFieldPhotos = queries.ExposureMode(responseModel.inputExposureMode).ToList();
                            break;
                        case "inputLensModel":
                            filteredByFieldPhotos = queries.LensModel(responseModel.inputLensModel).ToList();
                            break;
                        case "inputFileType":
                            filteredByFieldPhotos = queries.FileType(responseModel.inputFileType).ToList();
                            break;
                        case "inputFileName":
                            filteredByFieldPhotos = queries.FileName(responseModel.inputFileName).ToList();
                            break;
                        case "inputFileSize":
                            filteredByFieldPhotos = queries.FileSize((float)responseModel.inputFileSize).ToList();
                            break;
                        case "inputGPSAltitude":
                            filteredByFieldPhotos = queries.GPSAltitude((double)responseModel.inputGPSAltitude).ToList();
                            break;
                        case "inputGPSImgDirection":
                            filteredByFieldPhotos = queries.GPSImgDirection((double)responseModel.inputGPSImgDirection).ToList();
                            break;
                        case "inputGPSHorizontalPositioningError":
                            filteredByFieldPhotos = queries.GPSHorizontalPositioningError((double)responseModel.inputGPSHorizontalPositioningError).ToList();
                            break;
                        default:
                            break;
                    }
                    listOfFilteredPhotos.Add(filteredByFieldPhotos);
                }
            }
            
            String[] finalFilteredResult = queries.finalResult(listOfFilteredPhotos);
            return finalFilteredResult;
        }

        [HttpPost("[action]")]
        public string[] GetAll([FromBody] ResponseModel responseModel)
        {
            Queries queries = new Queries(Constants.connString, responseModel.username);
            return queries.getAll();
        }

        [HttpPost("[action]")]
        public bool InsertToDB([FromBody] Paths pathList) 
        {
            try
            {
                DataInsert inserter = new DataInsert(Constants.connString);
                foreach (string path in pathList.file_paths)
                {
                    inserter.InsertData((string)path, (string)pathList.owner);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
