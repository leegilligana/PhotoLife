﻿using System;
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
        private string owner;
        public Queries(string connString, string user)
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connString);
            var dataSource = dataSourceBuilder.Build();
            conn = dataSource.OpenConnection();
            owner = user;

        }

        public string[] Flash(bool isFlash)
        {

            var cmd = new NpgsqlCommand("SELECT file_name FROM photolife WHERE owner = @owner AND Flash = @isFlash;", conn);
            cmd.Parameters.AddWithValue("isFlash", isFlash);
            cmd.Parameters.AddWithValue("owner", owner);
            return DataReader(cmd.ExecuteReader());
        }

        public string[] Model(string model)
        {
            var cmd = new NpgsqlCommand($"SELECT file_name FROM photolife WHERE owner = @owner AND model LIKE '%{model}%';", conn);
            cmd.Parameters.AddWithValue("owner", owner);
            return DataReader(cmd.ExecuteReader());
        }

        public string[] DateTimeRange(DateTime start, DateTime end)
        {
            var cmd = new NpgsqlCommand("SELECT file_name FROM photolife WHERE owner = @owner AND date_time >= @start AND date_time <= @end;", conn);
            cmd.Parameters.AddWithValue("start", start);
            cmd.Parameters.AddWithValue("end", end);
            cmd.Parameters.AddWithValue("owner", owner);
            return DataReader(cmd.ExecuteReader());
        }

        public string[] Location(double inputLat, double inputLong, int inputDistance)
        {
            var locList = new List<string>();
            GeoCoordinate inputCoordinate = new GeoCoordinate(inputLat, inputLong);
            var cmd = new NpgsqlCommand("SELECT file_name, gps_coordinates FROM photolife WHERE owner = @owner;", conn);
            cmd.Parameters.AddWithValue("owner", owner);
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
            var cmd = new NpgsqlCommand("SELECT file_name FROM photolife WHERE owner = @owner AND image_width = @width;", conn);
            cmd.Parameters.AddWithValue("width", width);
            cmd.Parameters.AddWithValue("owner", owner);
            return DataReader(cmd.ExecuteReader());
        }

        public string[] Height(int height)
        {
            var cmd = new NpgsqlCommand("SELECT file_name FROM photolife WHERE owner = @owner AND image_height = @height;", conn);
            cmd.Parameters.AddWithValue("height", height);
            cmd.Parameters.AddWithValue("owner", owner);
            return DataReader(cmd.ExecuteReader());
        }

        public string[] Orientation(string orientation)
        {
            var cmd = new NpgsqlCommand($"SELECT file_name FROM photolife WHERE owner = @owner AND orientation LIKE '%{orientation}%';", conn);
            cmd.Parameters.AddWithValue("owner", owner);
            return DataReader(cmd.ExecuteReader());
        }

        public string[] XResolution(int xRes)
        {
            var cmd = new NpgsqlCommand("SELECT file_name FROM photolife WHERE owner = @owner AND x_resolution = @xRes;", conn);
            cmd.Parameters.AddWithValue("xRes", xRes);
            cmd.Parameters.AddWithValue("owner", owner);
            return DataReader(cmd.ExecuteReader());
        }

        public string[] YResolution(int yRes)
        {
            var cmd = new NpgsqlCommand("SELECT file_name FROM photolife WHERE owner = @owner AND y_resolution = @yRes;", conn);
            cmd.Parameters.AddWithValue("yRes", yRes);
            cmd.Parameters.AddWithValue("owner", owner);
            return DataReader(cmd.ExecuteReader());
        }

        public string[] Software(string software)
        {
            var cmd = new NpgsqlCommand($"SELECT file_name FROM photolife WHERE owner = @owner AND software LIKE '%{software}%';", conn);
            cmd.Parameters.AddWithValue("owner", owner);
            return DataReader(cmd.ExecuteReader());
        }

        public string[] ExposureTime(double exposureTime)
        {
            var cmd = new NpgsqlCommand("SELECT file_name FROM photolife WHERE owner = @owner AND exposure_time = @exposureTime;", conn);
            cmd.Parameters.AddWithValue("exposureTime", exposureTime);
            cmd.Parameters.AddWithValue("owner", owner);
            return DataReader(cmd.ExecuteReader());
        }

        public string[] ShutterSpeedValue(double shutterSpeedValue)
        {
            var cmd = new NpgsqlCommand("SELECT file_name FROM photolife WHERE owner = @owner AND shutter_speed_value = @shutterSpeedValue;", conn);
            cmd.Parameters.AddWithValue("shutterSpeedValue", shutterSpeedValue);
            cmd.Parameters.AddWithValue("owner", owner);
            return DataReader(cmd.ExecuteReader());
        }

        public string[] BrightnessValue(double brightnessValue)
        {
            var cmd = new NpgsqlCommand("SELECT file_name FROM photolife WHERE owner = @owner AND brightness_value = @brightnessValue;", conn);
            cmd.Parameters.AddWithValue("brightnessValue", brightnessValue);
            cmd.Parameters.AddWithValue("owner", owner);
            return DataReader(cmd.ExecuteReader());
        }

        public string[] SceneType(string sceneType)
        {
            var cmd = new NpgsqlCommand($"SELECT file_name FROM photolife WHERE owner = @owner AND scene_type LIKE '%{sceneType}%';", conn);
            cmd.Parameters.AddWithValue("owner", owner);
            return DataReader(cmd.ExecuteReader());
        }

        public string[] ExposureMode(string exposureMode)
        {
            var cmd = new NpgsqlCommand($"SELECT file_name FROM photolife WHERE owner = @owner AND exposure_mode LIKE '%{exposureMode}%';", conn);
            cmd.Parameters.AddWithValue("owner", owner);
            return DataReader(cmd.ExecuteReader());
        }

        public string[] LensModel(string lensModel)
        {
            var cmd = new NpgsqlCommand($"SELECT file_name FROM photolife WHERE owner = @owner AND lens_model LIKE '%{lensModel}%';", conn);
            cmd.Parameters.AddWithValue("owner", owner);
            return DataReader(cmd.ExecuteReader());
        }

        public string[] FileType(string fileType)
        {
            var cmd = new NpgsqlCommand($"SELECT file_name FROM photolife WHERE owner = @owner AND detected_file_type_name LIKE '%{fileType}%';", conn);
            cmd.Parameters.AddWithValue("owner", owner);
            return DataReader(cmd.ExecuteReader());
        }

        public string[] FileName(string fileName)
        {
            var cmd = new NpgsqlCommand($"SELECT file_name FROM photolife WHERE owner = @owner AND file_name LIKE '%{fileName}%';", conn);
            cmd.Parameters.AddWithValue("owner", owner);
            return DataReader(cmd.ExecuteReader());
        }

        public string[] FileSize(float fileSize)
        {
            var cmd = new NpgsqlCommand("SELECT file_name FROM photolife WHERE owner = @owner AND file_size = @fileSize;", conn);
            cmd.Parameters.AddWithValue("fileSize", fileSize);
            cmd.Parameters.AddWithValue("owner", owner);
            return DataReader(cmd.ExecuteReader());
        }

        public string[] GPSAltitude(double gpsAltitude)
        {
            var cmd = new NpgsqlCommand("SELECT file_name FROM photolife WHERE owner = @owner AND gps_altitude = @gpsAltitude;", conn);
            cmd.Parameters.AddWithValue("gpsAltitude", gpsAltitude);
            cmd.Parameters.AddWithValue("owner", owner);
            return DataReader(cmd.ExecuteReader());
        }

        public string[] GPSImgDirection(double gpsImgDirection)
        {
            var cmd = new NpgsqlCommand("SELECT file_name FROM photolife WHERE owner = @owner AND gps_img_direction = @gpsImgDirection;", conn);
            cmd.Parameters.AddWithValue("gpsImgDirection", gpsImgDirection);
            cmd.Parameters.AddWithValue("owner", owner);
            return DataReader(cmd.ExecuteReader());
        }

        public string[] GPSHorizontalPositioningError(double gpsHorizontalPositioningError)
        {
            var cmd = new NpgsqlCommand("SELECT file_name FROM photolife WHERE owner = @owner AND gps_horizontal_positioning_error = @gpsHorizontalPositioningError;", conn);
            cmd.Parameters.AddWithValue("gpsHorizontalPositioningError", gpsHorizontalPositioningError);
            cmd.Parameters.AddWithValue("owner", owner);
            return DataReader(cmd.ExecuteReader());
        }

        public void DeleteAll()
        {
            var cmd = new NpgsqlCommand("DELETE FROM photolife WHERE owner = @owner;", conn);
            cmd.Parameters.AddWithValue("owner", owner);
            cmd.ExecuteNonQuery();
        }

        public string[] getAll()
        {
            var cmd = new NpgsqlCommand("SELECT file_name FROM photolife WHERE owner = @owner;", conn);
            cmd.Parameters.AddWithValue("owner", owner);
            return DataReader(cmd.ExecuteReader());
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

        public string[] finalResult(List<List<string>> allResultsList)
        {
            string[][] allResults = allResultsList.Select(a => a.ToArray()).ToArray();
            for (int n = 0; n < allResults.Length; n++)
            {
                allResults[0] = allResults[0].Intersect(allResults[n]).ToArray();
            }

            if (allResults.Count() == 0)
            {
                return System.Array.Empty<String>();
            }
            return allResults[0];
        }
        }
    }