using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB_Queries;

namespace Profiles
{
    public class Profile
    {
        public string? PhotoTakingFreq { get; set; } = "";
        public string? AveTime { get; set; } = "";
        public string? Weekday { get; set; } = "";
        public string? CameraModel { get; set; } = "";
        public string? Flash { get; set; } = "";
        public string? AveBrightness { get; set; } = "";
        public string? GpsCords { get; set; } = "";
    }
}