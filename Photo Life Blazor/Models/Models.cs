using System;
using System.ComponentModel.DataAnnotations;

namespace metadata_extractor.Models
{
	public class ResponseModel
	{
		public string username { get; set; }
		public bool? inputIsFlash { get; set; }
		public string? inputModel { get; set; }
		public DateTime[]? inputTimeRange { get; set; }
		public double[]? inputCoord { get; set; }
		public int? inputWidth { get; set; }
		public int? inputHeight { get; set; }
		public string? inputOrientation { get; set; }
		public int? inputXRes { get; set; }
		public int? inputYRes { get; set; }
		public string? inputSoftware { get; set; }
		public double? inputExposureTime { get; set; }
		public double? inputShutterSpeedValue { get; set; }
		public double? inputBrightnessValue { get; set; }
		public string? inputSceneType { get; set; }
		public string? inputExposureMode { get; set; }
		public string? inputLensModel { get; set; }
		public string? inputFileType { get; set; }
		public string? inputFileName { get; set; }
		public float? inputFileSize { get; set; }
		public double? inputGPSAltitude { get; set; }
		public double? inputGPSImgDirection { get; set; }
		public double? inputGPSHorizontalPositioningError { get; set; }
	}
}