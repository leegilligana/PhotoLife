using System;
using System.ComponentModel.DataAnnotations;

namespace metadata_extractor.Models
{
    public class ResponseModel
    {
        public string username { get; set; }
        public bool inputIsFlash { get; set; }
        public string inputModel { get; set; }
        public DateTime inputStart { get; set; }
        public DateTime inputEnd { get; set; }
        public double inputX { get; set; }
        public double inputY { get; set; }
        public int inputDistance { get; set; }
        public int inputWidth { get; set; }
        public int inputHeight { get; set; }
        public string inputOrientation { get; set; }
        public int inputXRes { get; set; }
        public int inputYRes { get; set; }
        public string inputSoftware { get; set; }
        public double inputExposureTime { get; set; }
        public double inputShutterSpeedValue { get; set; }
        public double inputBrightnessValue { get; set; }
        public string inputSceneType { get; set; }
        public string inputExposureMode { get; set; }
        public string inputLensModel { get; set; }
        public string inputFileType { get; set; }
        public string inputFileName { get; set; }
        public float inputFileSize { get; set; }
        public double inputGPSAltitude { get; set; }
        public double inputGPSImgDirection { get; set; }
        public double inputGPSHorizontalPositioningError { get; set; }
    }
    //public class PhotoMetaData
    //{
    //    public int Image_Width { get; set; }
    //    public int Image_Height { get; set; }
    //    public Array? Media_White_Point { get; set; }
    //    public Array? Red_Colorant { get; set; }
    //    public Array? Green_Colorant { get; set; }
    //    public Array? Blue_Colorant { get; set; }
    //    public string? Model { get; set; }
    //    public string? Orientation { get; set; }
    //    public int


    //    [DataType(DataType.Date)]
    //    public DateTime ReleaseDate { get; set; }
    //    public string Genre { get; set; }
    //    public decimal Price { get; set; }
    //}
}