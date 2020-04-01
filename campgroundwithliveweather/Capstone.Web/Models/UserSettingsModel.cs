using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Web.Models
{
    public class UserSettingsModel
    {
        public string TempType { get; set; } = "F";
        public bool ShowAdvisory { get; set; }
    }
}
