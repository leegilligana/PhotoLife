using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB_Queries;

namespace Profiles
{
    internal class Profile
    {
        public string? PhotoTakingFreq { get; set; } = "";
        public string? AveTimeTaken { get; set; } = "";
        public string? AveDayTaken { get; set; } = "";
        public string? CameraModel { get; set; } = "";
        public string? Flash { get; set; } = "";
        public string? BrightnessValu { get; set; } = "";
        public string? GpsCords { get; set; } = "";
    }
}
