using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.ExceptionServices;
using System.Text;
using DB_Queries;
using Geocoding;
using Geocoding.Google;
using metadata_extractor;
using metadata_extractor.Models;
using Npgsql;
using NpgsqlTypes;
using Profiles;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProfileMaker
{
    class MakeProfile
    {
        /**
         * Returns a string representing the average timespan a string took their photos
         * between a specified start and end date 
        */

        public MakeProfile()
        {
        }
        public string PhotoTakingFreq(PFQueries q)
        {
            var dates = q.DatesNoParam();

            if (dates.Count <= 1)
            {
                return "0";
            }
            else
            {
                List<TimeSpan> time_diff = new List<TimeSpan>();
                for (int i = 1; i < dates.Count; i++)
                {
                    time_diff.Add(dates[i] - dates[i - 1]);
                }
                double tick_ave = time_diff.Average(d => d.Ticks);
                TimeSpan ave_dif = TimeSpan.FromTicks((long)tick_ave);

                return ave_dif.ToString();
            }
        }

        /** 
         * Return string of the period of the day photos are dated/taken on average
         */
        public string AveTimeTaken(PFQueries q)
        {
            var times = q.TimeList();

            var start_time = new TimeOnly(0,0,0);
            var end_time = new TimeOnly(1,0,0);
            var old_count = 0;
            var new_count = 0;

            string ave_time = "";

            if (times.Count <= 1)
            {
                return "0";
            }
            else
            {
                foreach (var t in times)
                {
                    TimeOnly.TryParse(t, out TimeOnly result);
                    if (start_time <= result && result <= end_time)
                    {
                        new_count++;
                    } else
                    {
                        start_time.AddHours(1);
                        end_time.AddHours(1);
                        old_count = new_count;
                        new_count = 1;
                    }
                    if (new_count > old_count)
                    {
                        ave_time = start_time.ToString() + ", " + end_time.ToString();
                    }
                }
                return ave_time;
            }
        }

        /** 
         * Return string of the day of the week photos are dated/taken on average
         */
        public string AveDayTaken(PFQueries q)
        {
            var dates = q.DatesNoParam();
            string[] days_of_weeks = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            int count_old = 0;
            int count_new = 0;
            string ave_day = "";

            if (dates.Count <= 1)
            {
                return "0";
            }
            else
            {
                foreach (var day in days_of_weeks)
                {
                    foreach (var date in dates)
                    {
                        if (date.ToString("dddd") == day)
                        {
                            count_new++;
                        }
                        if (count_new > count_old)
                        {
                            ave_day = date.ToString("dddd");
                            break;
                        }
                    }
                    count_old = count_new;
                    count_new = 0;
                }
                return ave_day;
            }
        }

        /**
         * Returns string of the location where a user takes their photos most frequently
         */

        public async Task<string> MostFreqLocation(PFQueries q)
        {
            NpgsqlPoint[] coords = q.GetCoords().ToArray();
            List<List<double>> coordinates = new List<List<double>>();

            foreach (NpgsqlPoint coord in coords)
            {
                var lon = coord.X;
                var lat = coord.Y;
                lon = Math.Round(lon, 6);
                lat = Math.Round(lat, 6);
                coordinates.Add(new List<double> { lon, lat });
            }
            Dictionary<List<double>, int> frequencyMap = new Dictionary<List<double>, int>();

            var coordinatesArray = coordinates.ToArray();

            foreach (var num in coordinatesArray)
            {
                if (frequencyMap.ContainsKey(num))
                    frequencyMap[num]++;
                else
                    frequencyMap[num] = 1;
            }

            int maxCount = frequencyMap.Values.Max();
            List<double> modeCoord = frequencyMap.First(kvp => kvp.Value == maxCount).Key;

            Console.WriteLine("modeCoord: " + modeCoord.ToArray()[0] + modeCoord.ToArray()[1]);
            string coordString = modeCoord.ToArray()[0].ToString() + ", " + modeCoord.ToArray()[1].ToString();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetAsync("https://api.geoapify.com/v1/geocode/reverse?lat=" + modeCoord.ToArray()[0].ToString().Replace(",",".") + "&lon=" + modeCoord.ToArray()[1].ToString().Replace(",", ".") + "&apiKey=f71a9e5c0a014fc0bc1e95c8c22abbe5");
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                Console.WriteLine("https://api.geoapify.com/v1/geocode/reverse?lat=" + modeCoord.ToArray()[0].ToString().Replace(",", ".") + "&lon=" + modeCoord.ToArray()[1].ToString().Replace(",", ".") + "&apiKey=f71a9e5c0a014fc0bc1e95c8c22abbe5");
                var city = await response.Content.ReadAsStringAsync();
                var indexCity = city.IndexOf("city\":");
                city = city.Substring(indexCity, indexCity + 30);
                var indexColon = city.IndexOf(":");
                var indexComma = city.IndexOf(",");
                city = city.Substring(indexColon + 1, indexComma - 5);
                city = city.Replace("\"", "");
                city = city.Replace(",", "");
                return city;
            }
            return "rip";
        }

        /**
         * Returns string of the most frequent camera model used, as well as the photos using
         * that camera model.
         */
        public string MostFreqCamModel(PFQueries q)
        {
            var models = q.ModelDict();
            int count_old = 0;
            int count_new = 0;
            string most_freq_model = "";
            List<string> model_lst = new List<string>();
            foreach (var photo in models)
            {
                if (model_lst.Contains(photo.Value) == false)
                {
                    model_lst.Add(photo.Value);
                    Console.WriteLine("New model:" + model_lst.Last());
                }

            }

            if (models.Count <= 1)
            {
                return most_freq_model;
            }
            else
            {
                foreach (var item in model_lst)
                {
                    foreach (var photo in models)
                    {
                        if (item.Equals(photo.Value))
                        {
                            count_new++;
                        }
                        if (count_new > count_old)
                        {
                            most_freq_model = photo.Value;
                            break;
                        }
                    }
                    count_old = count_new;
                    count_new = 0;
                }
                return most_freq_model;
            }
        }

        /**
         * Returns string of the location where a user takes their photos most frequently
         */
        public string AveBrightnessVal(PFQueries q)
        {
            var brightness_values = q.BrightnessValueList();
            return brightness_values;
        }

        /**
         * Returns a Profile object, which contains variables representing user-selected 
         * information to grab from their photos
         */
        public Profile CreateProfile(string username)
        {
            Profile userProfile = new Profile();
            var connString = Constants.connString;
            PFQueries q = new PFQueries(connString, username);
            List<string> all_filters = new List<string>()
            {
                "Time of Day", "GPS", "Camera Model", "Week Day", "Brightness"
            };;

            foreach (var filter in all_filters)
            {
                switch (filter)
                {
                    case "Time of Day":
                        userProfile.AveTime = AveTimeTaken(q);
                        Console.WriteLine(userProfile.AveTime);
                        break;
                    case "GPS":
                        var city = MostFreqLocation(q).Result;
                        userProfile.GpsCords = city;
                        Console.WriteLine("city: " + city);
                        break;
                    case "Camera Model":
                        userProfile.CameraModel = MostFreqCamModel(q);
                        break;
                    case "Week Day":
                        userProfile.Weekday = AveDayTaken(q);
                        break;
                    case "Brightness":
                        userProfile.AveBrightness = AveBrightnessVal(q);
                        break;
                }
            }
            return userProfile;
        }
    }
}