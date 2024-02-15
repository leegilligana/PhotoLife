using DB_Queries;
using Npgsql;

/*
 Pull metadata from the user's given images to:
    Figure out and display photos taken at a consistent location (need GPS info) --X
    Figure out and display photos taken at a consistent time (need dataTime) --X
    Figure out average time span user takes a photo in days/weeks/months --X

Problems:
    1) colorants do not give exact values in the DB when I use 'GetValue'. Returns "System.Double[]".
    2) Need photos in the DB to see if list works right.
 */

namespace ProfileMaker
{
    class Program
    {
        // Returns a string representing the average timespan a string took their photos
        // within a given date period
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

        // Return string of the period of the day photos are dated/taken on average
        public string AvePeriodTaken(PFQueries q, DateTime s, DateTime e)
        {
            var dates = q.Dates(s, e);

            if (dates.Count >= 1)
            {
                return "0";
            } 
            else
            {

                return "";
            }
        }

        // Return string of the day of the week photos are dated/taken on average
        public string AveDayTaken(PFQueries q, DateTime s, DateTime e)
        {

            return "";
        }

        // Return string of the location photos are taken most often
        public string AveLocationTaken(PFQueries q, DateTime s, DateTime e)
        {

            return "";
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
            DateTime e = new DateTime(2024, 1 , 31);
            Console.WriteLine(program.PhotoTakingFreq(q_lookup, s, e));

        }
    }
}

/*
 * 
 */