import { AppointmentFileModel } from "./appointment-file-model.model";
import { AppointmentModel } from "./appointment-model.model";

export interface DoctorAppointmentsViewModel {
    appointments: AppointmentModel[];
    appointmentFiles: AppointmentFileModel[];
    totalRecords: number;
}