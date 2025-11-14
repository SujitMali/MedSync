import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Doctor } from '../Shared/Models/doctor.model';
import { DoctorSchedule } from '../Shared/Models/doctor-schedule.model';
import { SlotDuration } from '../Shared/Models/slot-duration.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private http: HttpClient) { }


  //!--------------------------------------------------------------------------------------------------------------
  //! Admin --> Dashboard
  //!--------------------------------------------------------------------------------------------------------------


  getAllAppointments(filter: any): Observable<any> {
    return this.http.post(`${environment.appointmentApiUrl}/GetAppointments`, filter);
  }

  // getDoctorsDropdown(): Observable<any> {
  //   return this.http.get<any>(`${environment.doctorApiUrl}/GetDoctorsDropdown`);
  // }

  getAppointmentStatusesDropdown(): Observable<any> {
    return this.http.get(`${environment.appointmentApiUrl}/GetAllAppointmentStatus`);
  }

  previewAppointmentFile(appointmentId: number, fileName: string): Observable<Blob> {
    return this.http.get(`${environment.appointmentApiUrl}/PreviewAppointmentFile`, {
      params: {
        appointmentId: appointmentId.toString(),
        fileName: fileName
      },
      responseType: 'blob'
    });
  }
  //!--------------------------------------------------------------------------------------------------------------





  //!--------------------------------------------------------------------------------------------------------------
  //! Admin --> View Doctors, Add Doctors
  //!--------------------------------------------------------------------------------------------------------------
  getDropdownData(): Observable<any> {
    return this.http.get(`${environment.doctorApiUrl}/GetDropdownData`);
  }

  getAllDoctorsList(filters: any): Observable<any> {
    return this.http.post(`${environment.doctorApiUrl}/GetAllDoctorsList`, filters, {
      headers: { 'Content-Type': 'application/json' }
    });
  }

  addEditDoctor(formData: FormData) {
    return this.http.post(`${environment.doctorApiUrl}/AddEditDoctor`, formData);
  }
  //!--------------------------------------------------------------------------------------------------------------




  //!--------------------------------------------------------------------------------------------------------------
  //! Admin --> Manage Schedules
  //!--------------------------------------------------------------------------------------------------------------
  getDoctorSchedule(doctorID: number): Observable<DoctorSchedule[]> {
    return this.http.get<DoctorSchedule[]>(`${environment.scheduleApiUrl}/GetDoctorSchedule?doctorID=${doctorID}`);
  }

  saveDoctorSchedule(schedule: DoctorSchedule[]): Observable<any> {
    return this.http.post(`${environment.scheduleApiUrl}/SaveDoctorSchedules`, schedule);
  }

  getSlotDurations(): Observable<SlotDuration[]> {
    return this.http.get<SlotDuration[]>(`${environment.scheduleApiUrl}/GetSlotDurations`);
  }
  //!--------------------------------------------------------------------------------------------------------------



  //!--------------------------------------------------------------------------------------------------------------
  //! Admin --> Manage Credentials
  //!--------------------------------------------------------------------------------------------------------------
  getDoctorsWithoutUserCredentials(): Observable<any> {
    return this.http.get(`${environment.doctorApiUrl}/GetDoctorsWithoutUserCredentials`);
  }

  addUserCredentials(payload: any): Observable<any> {
    return this.http.post(`${environment.doctorApiUrl}/AddUserCredentials`, payload);
  }
  //!--------------------------------------------------------------------------------------------------------------


}
