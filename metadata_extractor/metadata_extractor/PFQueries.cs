using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Device.Location;
using NpgsqlTypes;
using System.Data;
using System.Collections;

namespace DB_Queries
{
    internal class PFQueries
    {
        private NpgsqlConnection conn;
        private string owner;
        public PFQueries(string connString, string user)
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

        // returns dictionary of photos with their camera model
        public Dictionary<string, string> ModelDict()
        {
            var cmd = new NpgsqlCommand($"SELECT file_name, model FROM photolife WHERE owner = @owner;", conn);
            cmd.Parameters.AddWithValue("owner", owner);
            var reader = cmd.ExecuteReader();
            Dictionary<string, string> dct = new Dictionary<string, string>();
            while (reader.Read())
            {
                try
                {
                    dct.Add(reader.GetString(reader.GetOrdinal("file_name")),
                            reader.GetString(reader.GetOrdinal("model")));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            reader.Close();
            return dct;
        }

        // returns list of datetimes
        public List<DateTime> Dates(DateTime start, DateTime end)
        {
            var cmd = new NpgsqlCommand("SELECT date_time FROM photolife WHERE " +
                "owner = @owner AND date_time >= @start AND date_time <= @end ORDER BY date_time;", conn);
            cmd.Parameters.AddWithValue("start", start);
            cmd.Parameters.AddWithValue("end", end);
            cmd.Parameters.AddWithValue("owner", owner);
            var reader = cmd.ExecuteReader();

            List<DateTime> dates = new List<DateTime>();
            int i = 0;
            while (reader.Read())
            {
                dates.Add(reader.GetDateTime("date_time"));
                i++;
            }
            reader.Close();
            return dates;
        }

        public List<DateTime> DatesNoParam()
        {
            var cmd = new NpgsqlCommand("SELECT date_time FROM photolife WHERE " +
                "owner = @owner ORDER BY date_time;", conn);
            cmd.Parameters.AddWithValue("owner", owner);
            var reader = cmd.ExecuteReader();

            List<DateTime> dates = new List<DateTime>();
            int i = 0;
            while (reader.Read())
            {
                dates.Add(reader.GetDateTime("date_time"));
                i++;
            }
            reader.Close();
            return dates;
        }

        // Need to fix; does not return average time from date_time yet
        public string DayTimeAve(DateTime start, DateTime end)
        {
            var cmd = new NpgsqlCommand("SELECT AVE(EXTRACT(HOUR FROM date_time))::timestamp FROM photolife WHERE " +
                "owner = @owner AND date_time >= @start AND date_time <= @end ORDER BY date_time;", conn);
            cmd.Parameters.AddWithValue("start", start);
            cmd.Parameters.AddWithValue("end", end);
            cmd.Parameters.AddWithValue("owner", owner);
            var reader = cmd.ExecuteReader();
            return reader.GetString(reader.GetOrdinal("avg"));
        }

        public string DayTimeAveNoParam()
        {
            var cmd = new NpgsqlCommand("SELECT AVE(EXTRACT(HOUR FROM date_time))::timestamp FROM photolife WHERE " +
                "owner = @owner ORDER BY date_time;", conn);
            cmd.Parameters.AddWithValue("owner", owner);
            var reader = cmd.ExecuteReader();
            return reader.GetString(reader.GetOrdinal("avg"));
        }

        public List<string> TimeList()
        {
            var cmd = new NpgsqlCommand("SELECT date_time :: timestamp :: time FROM photolife WHERE owner = @owner ORDER BY date_time;", conn);
            cmd.Parameters.AddWithValue("owner", owner);
            List<string> time_list = new List<string>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                time_list.Add(reader.GetString(0));
            }
            return time_list;
        }

        public string[] Location(double inputLat, double inputLong, int inputDistance)
        {
            var locList = new List<string>();
            GeoCoordinate inputCoordinate = new GeoCoordinate(inputLat, inputLong);
            var cmd = new NpgsqlCommand("SELECT file_name, gps_coordinates FROM test3 WHERE owner = @owner;", conn);
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

        // returns list of brightness values from photos having it in the database
        public string BrightnessValueList()
        {
            var cmd = new NpgsqlCommand("SELECT AVG(CAST(brightness_value AS decimal)) FROM photolife WHERE owner = @owner;", conn);
            cmd.Parameters.AddWithValue("owner", owner);
            var reader = cmd.ExecuteReader();
            reader.Read();
            var bright_value = reader.GetDecimal(0);

            reader.Close();
            return bright_value.ToString();
        }

        public void deleteAll()
        {
            var cmd = new NpgsqlCommand("DELETE FROM test3 where Owner = @owner;", conn);
            cmd.Parameters.AddWithValue("owner", owner);
            cmd.ExecuteNonQuery();
        }

        /**
         * Updated DataReader. Original only grabbed data from hardcoded column. 
         * Now a string can be passed in for a specific column.
         * Returns a list of strings representing data of a column in the Database
         */
        public string[] DataReader(NpgsqlDataReader reader, string col = "0")
        {
            var list = new List<string>();
            // checks to make sure col is can be converted to an int
            if (Int32.TryParse(col, out int i)) // if an int, read from DataReader using int
            {
                while (reader.Read())
                {
                    list.Add(reader.GetString(i));
                }
            }
            else // if not an int, read from Datareader using string
            {
                while (reader.Read())
                {
                    list.Add(reader.GetString(reader.GetOrdinal(col)));
                }
            }
            reader.Close();
            return list.ToArray();
        }
    }
}
