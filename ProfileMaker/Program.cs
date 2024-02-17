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
        public string AvePeriodTaken(PFQueries q, DateTime s, DateTime e)
        {

            return "";
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
        public string MostFreqCameraModel(PFQueries q)
        {
            var models = q.ModelDict();
            int count_old = 0;
            int count_new = 0;
            string most_freq_model = "";
            string[] model_arr = { };
            foreach (var cammodel in models)
            {
                if (!model_arr.Contains(cammodel.Value)) { model_arr.Append(cammodel.Value); }
            }

            if (models.Count <= 1)
            {
                return "0";
            }
            else
            {
                foreach (var item in model_arr)
                {
                    foreach (var cammodel in models)
                    {
                        if (item == cammodel.Value)
                        {
                            count_new++;
                        }
                        if (count_new > count_old)
                        {
                            most_freq_model = cammodel.Value;
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
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connString);
            var dataSource = dataSourceBuilder.Build();
            var conn = dataSource.OpenConnection();

            Program program = new Program();
            PFQueries q_lookup = new PFQueries(connString, "Alejandro");

            DateTime s = new DateTime(2001, 1, 1);
            DateTime e = new DateTime(2024, 1, 31);
            Console.WriteLine(program.PhotoTakingFreq(q_lookup, s, e));

        }
    }
}