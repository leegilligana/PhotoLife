using DB_Queries;
using Npgsql;
using Profiles;

namespace ProfileMaker
{
    class Program
    {
        /**
         * Returns a string representing the average timespan a string took their photos
         * between a specified start and end date 
        */
        public string PhotoTakingFreq(PFQueries q, DateTime s, DateTime e)
        {
            var dates = q.Dates(s, e);

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
        public string AveTimeTaken(PFQueries q, DateTime s, DateTime e)
        {
            var dates = q.Dates(s, e);

            if (dates.Count <= 1)
            {
                return "0";
            }
            else
            {

                double sinSum = 0.0;
                double cosSum = 0.0;

                foreach (DateTime time in dates)
                {
                    double radians = 2 * Math.PI * time.TimeOfDay.TotalHours / 24;
                    sinSum += Math.Sin(radians);
                    cosSum += Math.Cos(radians);
                }

                double avgRadians = Math.Atan2(sinSum / dates.Count, cosSum / dates.Count);
                double avgHours = 24 * avgRadians / (2 * Math.PI);

                if (avgHours < 0)
                {
                    avgHours += 24;
                }

                return TimeSpan.FromHours(avgHours).ToString();
            }
        }

        /** 
         * Return string of the day of the week photos are dated/taken on average
         */
        public string AveDayTaken(PFQueries q, DateTime s, DateTime e)
        {
            var dates = q.Dates(s, e);
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
        public string MostFreqLocation(PFQueries q)
        {

            return "";
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
            string[] model_arr = { };
            foreach (var photo in models)
            {
                if (!model_arr.Contains(photo.Value)) { model_arr.Append(photo.Value); }
            }

            if (models.Count <= 1)
            {
                return "0";
            }
            else
            {
                foreach (var item in model_arr)
                {
                    foreach (var photo in models)
                    {
                        if (item == photo.Value)
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
        public string AveBrightness(PFQueries q)
        {
            var brightness_values = q.BrightnessValueList();
            var ave_value = brightness_values.Average();
            return ave_value.ToString();
        }

        /**
         * Returns a Profile object, which contains variables representing user-selected 
         * information to grab from their photos
         */
        public Profile CreateProfile(string filters)
        {
            Profile userProfile = new Profile();
            return userProfile;
        }

        static void Main(string[] args)
        {
            var connString = "Host=cs400f23acd.mathcs.carleton.edu;Username=photolife;" +
            "Password=BlueWTRgrass23&;Database=photolife";

            Program program = new Program();
            PFQueries q_lookup = new PFQueries(connString, "Alejandro");

            DateTime s = new DateTime(2001, 1, 1);
            DateTime e = new DateTime(2024, 1, 31);
            Profile usr_profile = new Profile();
            usr_profile.CameraModel = program.MostFreqCamModel(q_lookup);
            Console.WriteLine(usr_profile.CameraModel);
        }
    }
}