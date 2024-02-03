using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Device.Location;
using NpgsqlTypes;

namespace DB_Queries
{
	internal class Queries
	{
		private NpgsqlConnection conn;
		public Queries(string connString)
		{
			var dataSourceBuilder = new NpgsqlDataSourceBuilder(connString);
			var dataSource = dataSourceBuilder.Build();
			conn = dataSource.OpenConnection();

		}

		public string[] Flash(bool isFlash)
		{

			var cmd = new NpgsqlCommand("SELECT file_name FROM test3 WHERE Flash = @isFlash;", conn);
			cmd.Parameters.AddWithValue("isFlash", isFlash);
			return DataReader(cmd.ExecuteReader());
		}

		public string[] Model(string model)
		{
			var cmd = new NpgsqlCommand($"SELECT file_name FROM test3 WHERE model LIKE '%{model}%';", conn);
			return DataReader(cmd.ExecuteReader());
		}

		public string[] DateTimeRange(DateTime start, DateTime end)
		{
			var cmd = new NpgsqlCommand("SELECT file_name FROM test3 WHERE date_time >= @start AND date_time <= @end;", conn);
			cmd.Parameters.AddWithValue("start", start);
			cmd.Parameters.AddWithValue("end", end);
			return DataReader(cmd.ExecuteReader());
		}

		public string[] Location(double inputLat, double inputLong, int inputDistance)
		{
			var locList = new List<string>();
			GeoCoordinate inputCoordinate = new GeoCoordinate(inputLat, inputLong);
			var cmd = new NpgsqlCommand("SELECT file_name, gps_coordinates FROM test3;", conn);

			using (NpgsqlDataReader reader = cmd.ExecuteReader())
			{
				// Iterate over the rows
				while (reader.Read())
				{
					// Access the values for each row
					try
					{
						string filename = reader.GetString(reader.GetOrdinal("file_name"));
						NpgsqlPoint location = reader.GetFieldValue<NpgsqlPoint>(reader.GetOrdinal("gps_coordinates"));

						double picLat = Convert.ToDouble(location.X);
						double picLong = Convert.ToDouble(location.Y);
						GeoCoordinate picCoordinate = new GeoCoordinate(picLat, picLong);
						// Calculate the distance between the two locations in meters
						double distanceMeters = inputCoordinate.GetDistanceTo(picCoordinate);

						// Convert the distance to kilometers
						double distanceKilometers = distanceMeters / 1000;

						if (distanceKilometers <= inputDistance)
						{
							locList.Add(filename);
						}
					}
					catch (Exception)
					{
						;
					}

				}
				reader.Close();
			}
			return locList.ToArray();
		}

		public string[] Width(int width)
		{
			var cmd = new NpgsqlCommand("SELECT file_name FROM test3 WHERE image_width = @width;", conn);
			cmd.Parameters.AddWithValue("width", width);
			return DataReader(cmd.ExecuteReader());
		}

		public string[] Height(int height)
		{
			var cmd = new NpgsqlCommand("SELECT file_name FROM test3 WHERE image_height = @height;", conn);
			cmd.Parameters.AddWithValue("height", height);
			return DataReader(cmd.ExecuteReader());
		}

		public string[] Orientation(string orientation)
		{
			var cmd = new NpgsqlCommand($"SELECT file_name FROM test3 WHERE orientation LIKE '%{orientation}%';", conn);
			return DataReader(cmd.ExecuteReader());
		}

		public string[] XResolution(int xRes)
		{
			var cmd = new NpgsqlCommand("SELECT file_name FROM test3 WHERE x_resolution = @xRes;", conn);
			cmd.Parameters.AddWithValue("xRes", xRes);
			return DataReader(cmd.ExecuteReader());
		}

		public string[] YResolution(int yRes)
		{
			var cmd = new NpgsqlCommand("SELECT file_name FROM test3 WHERE y_resolution = @yRes;", conn);
			cmd.Parameters.AddWithValue("yRes", yRes);
			return DataReader(cmd.ExecuteReader());
		}

		public string[] Software(string software)
		{
			var cmd = new NpgsqlCommand($"SELECT file_name FROM test3 WHERE software LIKE '%{software}%';", conn);
			return DataReader(cmd.ExecuteReader());
		}

		public string[] ExposureTime(double exposureTime)
		{
			var cmd = new NpgsqlCommand("SELECT file_name FROM test3 WHERE exposure_time = @exposureTime;", conn);
			cmd.Parameters.AddWithValue("exposureTime", exposureTime);
			return DataReader(cmd.ExecuteReader());
		}

		public string[] ShutterSpeedValue(double shutterSpeedValue)
		{
			var cmd = new NpgsqlCommand("SELECT file_name FROM test3 WHERE shutter_speed_value = @shutterSpeedValue;", conn);
			cmd.Parameters.AddWithValue("shutterSpeedValue", shutterSpeedValue);
			return DataReader(cmd.ExecuteReader());
		}

		public string[] BrightnessValue(double brightnessValue)
		{
			var cmd = new NpgsqlCommand("SELECT file_name FROM test3 WHERE brightness_value = @brightnessValue;", conn);
			cmd.Parameters.AddWithValue("brightnessValue", brightnessValue);
			return DataReader(cmd.ExecuteReader());
		}

		public string[] SceneType(string sceneType)
		{
			var cmd = new NpgsqlCommand($"SELECT file_name FROM test3 WHERE scene_type LIKE '%{sceneType}%';", conn);
			return DataReader(cmd.ExecuteReader());
		}

		public string[] ExposureMode(string exposureMode)
		{
			var cmd = new NpgsqlCommand($"SELECT file_name FROM test3 WHERE exposure_mode LIKE '%{exposureMode}%';", conn);
			return DataReader(cmd.ExecuteReader());
		}

		public string[] LensModel(string lensModel)
		{
			var cmd = new NpgsqlCommand($"SELECT file_name FROM test3 WHERE lens_model LIKE '%{lensModel}%';", conn);
			return DataReader(cmd.ExecuteReader());
		}

		public string[] FileType(string fileType)
		{
			var cmd = new NpgsqlCommand($"SELECT file_name FROM test3 WHERE detected_file_type_name LIKE '%{fileType}%';", conn);
			return DataReader(cmd.ExecuteReader());
		}

		public string[] FileName(string fileName)
		{
			var cmd = new NpgsqlCommand($"SELECT file_name FROM test3 WHERE file_name LIKE '%{fileName}%';", conn);
			return DataReader(cmd.ExecuteReader());
		}

		public string[] FileSize(float fileSize)
		{
			var cmd = new NpgsqlCommand("SELECT file_name FROM test3 WHERE file_size = @fileSize;", conn);
			cmd.Parameters.AddWithValue("fileSize", fileSize);
			return DataReader(cmd.ExecuteReader());
		}

		public string[] GPSAltitude(double gpsAltitude)
		{
			var cmd = new NpgsqlCommand("SELECT file_name FROM test3 WHERE gps_altitude = @gpsAltitude;", conn);
			cmd.Parameters.AddWithValue("gpsAltitude", gpsAltitude);
			return DataReader(cmd.ExecuteReader());
		}

		public string[] GPSImgDirection(double gpsImgDirection)
		{
			var cmd = new NpgsqlCommand("SELECT file_name FROM test3 WHERE gps_img_direction = @gpsImgDirection;", conn);
			cmd.Parameters.AddWithValue("gpsImgDirection", gpsImgDirection);
			return DataReader(cmd.ExecuteReader());
		}

		public string[] GPSHorizontalPositioningError(double gpsHorizontalPositioningError)
		{
			var cmd = new NpgsqlCommand("SELECT file_name FROM test3 WHERE gps_horizontal_positioning_error = @gpsHorizontalPositioningError;", conn);
			cmd.Parameters.AddWithValue("gpsHorizontalPositioningError", gpsHorizontalPositioningError);
			return DataReader(cmd.ExecuteReader());
		}

		public void deleteAll(string user)
		{
			var cmd = new NpgsqlCommand("DELETE FROM test3 where Owner = @user;", conn);
			cmd.Parameters.AddWithValue("user", user);
			cmd.ExecuteNonQuery();
		}

		public string[] DataReader(NpgsqlDataReader reader)
		{
			var list = new List<string>();
			while (reader.Read())
			{
				list.Add(reader.GetString(0));
			}
			reader.Close();
			return list.ToArray();
		}
	}
}
