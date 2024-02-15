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
        public string? PhotoTakingFreq { get; }
        public Dictionary<string, string>? PhotoDateOrder { get; }
        public string? CameraModels { get;  }
        public string? CameraModelCount { get; }
        public string? Flash { get; }
        public string? DeviceModels { get; }
        public string? GpsCords {  get; }

        public Profile(string time, string? gpscords)
        {
            
        }
    }
}
