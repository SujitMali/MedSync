using System;


namespace MedSync_ClassLibraries.Models
{
    public class DoctorSlotModel
    {
        public DateTime SlotStart { get; set; }
        public DateTime SlotEnd { get; set; }
        public bool IsAvailable { get; set; }
    }

}
