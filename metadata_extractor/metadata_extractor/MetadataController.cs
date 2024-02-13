using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using DB_Queries;
using System;
using Newtonsoft.Json;
using metadata_extractor.Models;

namespace metadata_extractor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetadataController : ControllerBase
    {
    
        [HttpPost("[action]")]
        public string[] GetFlash([FromBody] ResponseModel responseModel)
        {
            Queries queries = new Queries(Constants.connString, responseModel.username);
            return queries.Flash(responseModel.inputIsFlash);
        }

        [HttpPost("[action]")]
        public string[] GetModel([FromBody] ResponseModel responseModel)
        {
            Queries queries = new Queries(Constants.connString, responseModel.username);
            return queries.Model(responseModel.inputModel);
        }

        [HttpPost("[action]")]
        public string[] GetDateTimeRange([FromBody] ResponseModel responseModel)
        {
            Queries queries = new Queries(Constants.connString, responseModel.username);
            return queries.DateTimeRange(responseModel.inputStart, responseModel.inputEnd);
        }

        [HttpPost("[action]")]
        public string[] GetLocation([FromBody] ResponseModel responseModel)
        {
            Queries queries = new Queries(Constants.connString, responseModel.username);
            return queries.Location(responseModel.inputX, responseModel.inputY, responseModel.inputDistance);
        }

        [HttpPost("[action]")]
        public string[] GetWidth([FromBody] ResponseModel responseModel)
        {
            Queries queries = new Queries(Constants.connString, responseModel.username);
            return queries.Width(responseModel.inputWidth);
        }

        [HttpPost("[action]")]
        public string[] GetHeight([FromBody] ResponseModel responseModel)
        {
            Queries queries = new Queries(Constants.connString, responseModel.username);
            return queries.Width(responseModel.inputHeight);
        }

        [HttpPost("[action]")]
        public string[] GetOrientation([FromBody] ResponseModel responseModel)
        {
            Queries queries = new Queries(Constants.connString, responseModel.username);
            return queries.Orientation(responseModel.inputOrientation);
        }

        [HttpPost("[action]")]
        public string[] GetXResolution([FromBody] ResponseModel responseModel)
        {
            Queries queries = new Queries(Constants.connString, responseModel.username);
            return queries.XResolution(responseModel.inputXRes);
        }

        [HttpPost("[action]")]
        public string[] GetYResolution([FromBody] ResponseModel responseModel)
        {
            Queries queries = new Queries(Constants.connString, responseModel.username);
            return queries.YResolution(responseModel.inputYRes);
        }

        [HttpPost("[action]")]
        public string[] GetSoftware([FromBody] ResponseModel responseModel)
        {
            Queries queries = new Queries(Constants.connString, responseModel.username);
            return queries.Software(responseModel.inputSoftware);
        }

        [HttpPost("[action]")]
        public string[] GetExposureTime([FromBody] ResponseModel responseModel)
        {
            Queries queries = new Queries(Constants.connString, responseModel.username);
            return queries.ExposureTime(responseModel.inputExposureTime);
        }

        [HttpPost("[action]")]
        public string[] GetShutterSpeedValue([FromBody] ResponseModel responseModel)
        {
            Queries queries = new Queries(Constants.connString, responseModel.username);
            return queries.ShutterSpeedValue(responseModel.inputShutterSpeedValue);
        }

        [HttpPost("[action]")]
        public string[] GetBrightnessValue([FromBody] ResponseModel responseModel)
        {
            Queries queries = new Queries(Constants.connString, responseModel.username);
            return queries.BrightnessValue(responseModel.inputBrightnessValue);
        }

        [HttpPost("[action]")]
        public string[] GetSceneType([FromBody] ResponseModel responseModel)
        {
            Queries queries = new Queries(Constants.connString, responseModel.username);
            return queries.SceneType(responseModel.inputSceneType);
        }

        [HttpPost("[action]")]
        public string[] GetExposureMode([FromBody] ResponseModel responseModel)
        {
            Queries queries = new Queries(Constants.connString, responseModel.username);
            return queries.ExposureMode(responseModel.inputExposureMode);
        }

        [HttpPost("[action]")]
        public string[] GetLensModel([FromBody] ResponseModel responseModel)
        {
            Queries queries = new Queries(Constants.connString, responseModel.username);
            return queries.LensModel(responseModel.inputLensModel);
        }

        [HttpPost("[action]")]
        public string[] GetFileType([FromBody] ResponseModel responseModel)
        {
            Queries queries = new Queries(Constants.connString, responseModel.username);
            return queries.FileType(responseModel.inputFileType);
        }

        [HttpPost("[action]")]
        public string[] GetFileName([FromBody] ResponseModel responseModel)
        {
            Queries queries = new Queries(Constants.connString, responseModel.username);
            return queries.FileName(responseModel.inputFileName);
        }

        [HttpPost("[action]")]
        public string[] GetFileSize([FromBody] ResponseModel responseModel)
        {
            Queries queries = new Queries(Constants.connString, responseModel.username);
            return queries.FileSize(responseModel.inputFileSize);
        }

        [HttpPost("[action]")]
        public string[] GetGPSAltitude([FromBody] ResponseModel responseModel)
        {
            Queries queries = new Queries(Constants.connString, responseModel.username);
            return queries.GPSAltitude(responseModel.inputGPSAltitude);
        }

        [HttpPost("[action]")]
        public string[] GetGPSImgDirection([FromBody] ResponseModel responseModel)
        {
            Queries queries = new Queries(Constants.connString, responseModel.username);
            return queries.GPSImgDirection(responseModel.inputGPSImgDirection);
        }

        [HttpPost("[action]")]
        public string[] GetGPSHorizontalPositioningError([FromBody] ResponseModel responseModel)
        {
            Queries queries = new Queries(Constants.connString, responseModel.username);
            return queries.GPSHorizontalPositioningError(responseModel.inputGPSHorizontalPositioningError);
        }

        [HttpPost("[action]")]
        public string[] finalResult([FromBody] ResponseModel responseModel)
        {
			Queries queries = new Queries(Constants.connString, responseModel.username);
			return queries.finalResult(responseModel.inputFinalResult);
		}

        [HttpPost("[action]")]
        public string[] getAll([FromBody] ResponseModel responseModel)
        {
			Queries queries = new Queries(Constants.connString, responseModel.username);
			return queries.getAll();
		}
    }
}