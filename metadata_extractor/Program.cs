// See https://aka.ms/new-console-template for more information
using DB_Queries;
using metadata_extractor;
using MetadataExtractor;
using Microsoft.Extensions.Configuration;

//open connection with database (definetly a bad idea to have the password harcodeded, but that's a problem for another day :) )

IConfigurationRoot config = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json")
	.AddEnvironmentVariables()
	.Build();

var settings = config.GetRequiredSection("ConnString").Get<Dictionary<string,string>>();

var connString = $"Host={settings["Host"]};Username={settings["Username"]};" +
	$"Password={settings["Password"]};Database={settings["Database"]}";

//connection is open 

string[] relevantFields = { "Image Width", "Image Height", "Media White Point", 
    "Red Colorant", "Green Colorant", "Blue Colorant", "Model", 
    "Orientation", "X Resolution", "Y Resolution", "Software", "Date/Time",
    "Exposure Time", "Exif Version", "Shutter Speed Value", 
    "Brightness Value", "Flash", "Scene Type","Exposure Mode", "Lens Model",
    "GPS Latitude", "GPS Longitude","GPS Altitude", "GPS Img Direction", 
    "GPS Horizontal Positioning Error","Detected File Type Name", "File Name", 
    "File Size"};

string[] relevantDir = { "HEIC Primary Item Properties", "ICC Profile", "Exif IFD0", 
    "Exif SubIFD", "GPS", "File Type", "File" };

string[] noQuoteType = { "Image_Width", "Image_Height", "X_Resolution", "Y_Resolution",
"Exposure_Time", "Shutter_Speed_Value", "GPS_Altitude", "GPS_Img_Direction", "GPS_Horizontal_Positioning_Error",
"Brightness_Value", "File_Size"};


string[] quoteType = {"Model", "Orientation", "Software",
"Date_Time", "Media_White_Point", "Red_Colorant", "Green_Colorant", "Blue_Colorant", "Scene_Type",
"Exposure_Mode", "Lens_Model", "Exif_Version", "Detected_File_Type_Name", "File_Name", "Flash","GPS_Coordinates"};

string[] removeUnits = { "X_Resolution", "Y_Resolution", "Exposure_Time", "Shutter_Speed_Value", "GPS_Altitude", 
    "GPS_Img_Direction", "GPS_Horizontal_Positioning_Error", "File_Size"};
string[] pointToArrayFields = { "Media_White_Point", "Red_Colorant", "Green_Colorant", "Blue_Colorant"};
string[] doubles = {"Brightness_Value", "GPS_Img_Direction","GPS_Altitude", "GPS_Horizontal_Positioning_Error", "Exposure_Time"};
int count = 0;
/*this reads the metadata from the image and stores it in a list of directories and
stores it*/


IEnumerable<MetadataExtractor.Directory> directories = 
    ImageMetadataReader.ReadMetadata("C:\\Users\\acer\\OneDrive\\Escritorio\\ALE" +
    "\\CS\\COMPS\\Metadata Extractor\\metadata_extractor\\metadata_extractor" +
    "\\IMG_9450.HEIC");

/*this iterates through the list of directories and insertes the relevant fields into the
database*/
Dictionary<string, string> dataDict = new Dictionary<string, string>();
DataCleaner dataCleaner = new DataCleaner();
DataInsert dataInsert = new DataInsert(connString);

foreach (var directory in directories)
{
    if (relevantDir.Contains(directory.Name))
        foreach (var tag in directory.Tags)

            if (relevantFields.Contains(tag.Name))
            {
                var columnName = tag.Name.Replace(" ", "_");
                columnName = columnName.Replace("/", "_");
                try
                {
				    dataDict.Add(columnName, tag.Description);
                    count ++;
				}catch(Exception)
                {
                    ;
                }      
            }
}

/*
dataCleaner.formatData(dataDict, removeUnits, pointToArrayFields, doubles);
foreach (KeyValuePair<string, string> author in dataDict)
{
	Console.WriteLine("Key: {0}, Value: {1}",
		author.Key, author.Value);
}
dataInsert.insertData(dataDict, quoteType, noQuoteType);
*/

var queries = new Queries(connString);
var result = queries.Width(4032);
foreach (var item in result)
{
	Console.WriteLine(item);
}