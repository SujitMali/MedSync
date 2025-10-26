using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedSync_ClassLibraries.Models
{
    public class SlotDurationModel
    {
        public int SlotDurationID { get; set; }
        public int DurationMinutes { get; set; }
        public bool IsActive { get; set; }
    }
}
