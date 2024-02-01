using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using static Unsplasharp.UnsplasharpClient;
using Unsplasharp.Models;


namespace metadata_extractor
{

	public class DataCleaner
	{
		public void unitRemover(string[] toChange, Dictionary<string, string> data)
		{
			foreach (var item in toChange)
			{
				try
				{
					data[item] = data[item].Remove(data[item].IndexOf(" "));
				}
				catch (Exception)
				{; }
			}
		}

		public void stringToBool(Dictionary<string, string> data)
		{
				try
				{
					if (data["Flash"].ToLower().Contains("not"))
						data["Flash"] = "FALSE";
					else
						data["Flash"] = "TRUE";
				}
				catch (Exception)
				{; }
		}

		public void degToDec(Dictionary<string, string> data)
		{
			try
			{
				var latitude = data["GPS_Latitude"];
				var longitude = data["GPS_Longitude"];
				var latDeg = latitude.Substring(0, latitude.IndexOf("°"));
				var latMin = latitude.Substring(latitude.IndexOf("°") + 1,
							latitude.IndexOf("'") - latitude.IndexOf("°") - 1);
				var latSec = latitude.Substring(latitude.IndexOf("'") + 1).Replace("\"","");
				var lat = double.Parse(latDeg) + double.Parse(latMin) / 60 +
					double.Parse(latSec) / 3600;
				var longDeg = longitude.Substring(0, longitude.IndexOf("°"));
				var longMin = longitude.Substring(longitude.IndexOf("°") + 1,
									longitude.IndexOf("'") - longitude.IndexOf("°") - 1);
				var longSec = longitude.Substring(longitude.IndexOf("'") + 1).Replace("\"", "");
				var longi = double.Parse(longDeg) + double.Parse(longMin) / 60 +
					double.Parse(longSec) / 3600;
				data["GPS_Coordinates"] = $"({lat}: {longi})";
				data["GPS_Coordinates"] = data["GPS_Coordinates"].Replace(",",".");
				data["GPS_Coordinates"] = data["GPS_Coordinates"].Replace(":", ",");
				data.Remove("GPS_Longitude");
				data.Remove("GPS_Latitude");
			}
			catch (Exception)
			{; }
		}
		public void timeStampFormat(Dictionary<string, string> data)
		{
			try
			{
				var timeStamp = data["Date_Time"];
				string[] dateAndTime = timeStamp.Split(' ');
				dateAndTime[0] = dateAndTime[0].Replace(":", "-");
				data["Date_Time"] = $"{dateAndTime[0]} {dateAndTime[1]}";
			}
			catch (Exception)
			{; }
		}

		public void pointToArray(string[] toChange, Dictionary<string, string> data)
		{
			foreach (var item in toChange)
			{
				try
				{
					var point = data[item];
					point = point.Replace("(", "");
					point = point.Replace(")", "");
					point = point.Replace(",", ".");
					var array = point.Split(". ");
					data[item] = $"{{{array[0]},{array[1]},{array[2]}}}";
								}
				catch (Exception)
				{ ; }
			}
		}

		public void formatDoubles(Dictionary<string, string> data, string[] Doubles)
		{
			foreach (var item in Doubles)
			{
				try
				{
					data[item] = data[item].Replace(",", ".");
				}
				catch (Exception)
				{ ; }
			}
		}
		public void formatData(Dictionary<string, string> data, string[] unitRemoval, string[] pointArray, string[] Doubles)
		{
			unitRemover(unitRemoval, data);
			stringToBool(data);
			degToDec(data);
			timeStampFormat(data);
			pointToArray(pointArray, data);
			formatDoubles(data, Doubles);
		}
		
	}
}