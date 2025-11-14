using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedSync_ClassLibraries.Models
{
    public class DoctorAppointmentsViewModel
    {
        public List<AppointmentModel> Appointments { get; set; } = new List<AppointmentModel> { };
        public List<AppointmentFileModel> AppointmentFiles { get; set; } = new List<AppointmentFileModel> { };
        public int TotalRecords { get; set; }
    }
}
