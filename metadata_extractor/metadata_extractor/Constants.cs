using System;
using Microsoft.Extensions.Configuration;

namespace metadata_extractor
{
    public static class Constants
    {
        static IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        public static Dictionary<string, string> settings = config.GetRequiredSection("ConnString").Get<Dictionary<string, string>>();

        public static string connString = $"Host={settings["Host"]};Username={settings["Username"]};" +
            $"Password={settings["Password"]};Database={settings["Database"]}";

        public static string[] relevantFields = { "Image Width", "Image Height", "Media White Point",
    "Red Colorant", "Green Colorant", "Blue Colorant", "Model",
    "Orientation", "X Resolution", "Y Resolution", "Software", "Date/Time",
    "Exposure Time", "Exif Version", "Shutter Speed Value",
    "Brightness Value", "Flash", "Scene Type","Exposure Mode", "Lens Model",
    "GPS Latitude", "GPS Longitude","GPS Altitude", "GPS Img Direction",
    "GPS Horizontal Positioning Error","Detected File Type Name", "File Name",
    "File Size"};

        public static string[] relevantDir = { "HEIC Primary Item Properties", "ICC Profile", "Exif IFD0",
    "Exif SubIFD", "GPS", "File Type", "File" };

        public static string[] noQuoteType = { "Image_Width", "Image_Height", "X_Resolution", "Y_Resolution",
"Exposure_Time", "Shutter_Speed_Value", "GPS_Altitude", "GPS_Img_Direction", "GPS_Horizontal_Positioning_Error",
"Brightness_Value", "File_Size"};


        public static string[] quoteType = {"Model", "Orientation", "Software",
"Date_Time", "Media_White_Point", "Red_Colorant", "Green_Colorant", "Blue_Colorant", "Scene_Type",
"Exposure_Mode", "Lens_Model", "Exif_Version", "Detected_File_Type_Name", "File_Name", "Flash","GPS_Coordinates"};

        public static string[] removeUnits = { "X_Resolution", "Y_Resolution", "Exposure_Time", "Shutter_Speed_Value", "GPS_Altitude",
    "GPS_Img_Direction", "GPS_Horizontal_Positioning_Error", "File_Size"};
        public static string[] pointToArrayFields = { "Media_White_Point", "Red_Colorant", "Green_Colorant", "Blue_Colorant" };
        public static string[] doubles = { "Brightness_Value", "GPS_Img_Direction", "GPS_Altitude", "GPS_Horizontal_Positioning_Error", "Exposure_Time" };

    }
}

