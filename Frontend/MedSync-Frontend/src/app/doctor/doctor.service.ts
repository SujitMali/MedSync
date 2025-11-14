import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { AppointmentFilterModel } from '../Shared/Models/appointment-filter.model';
import { DoctorAppointmentsViewModel } from '../Shared/Models/doctor-appointments-view-model.model';

@Injectable({ providedIn: 'root' })
export class DoctorAppointmentsService {

  constructor(private http: HttpClient) { }

  getAppointmentStatuses(): Observable<any[]> {
    return this.http.get<any>(`${environment.appointmentApiUrl}/GetAllAppointmentStatus`).pipe(
      map(res => res.data || [])
    );
  }

  updateDoctorAppointment(request: any): Observable<any> {
    return this.http.put<any>(`${environment.appointmentApiUrl}/UpdateDoctorAppointment`, request);
  }


  getDoctorSlots(doctorId: number, appointmentDate: string): Observable<any> {
    return this.http.get(`${environment.doctorApiUrl}/GetDoctorSlots?doctorId=${doctorId}&appointmentDate=${appointmentDate}`);
  }


  getDoctorAppointments(filter: AppointmentFilterModel): Observable<DoctorAppointmentsViewModel> {
    debugger;
    return this.http.post<any>(`${environment.appointmentApiUrl}/GetAppointments`, filter).pipe(
      map(res => ({
        appointments: res.data || [],
        appointmentFiles: res.files || [],
        totalRecords: res.totalRecords || 0
      }))

    );
  }

  previewAppointmentFile(appointmentId: number, fileName: string): Observable<Blob> {
    return this.http.get(`${environment.appointmentApiUrl}/PreviewAppointmentFile`, {
      params: { appointmentId, fileName },
      responseType: 'blob'
    });
  }
}
















// getDoctorAppointments(filter: AppointmentFilterModel): Observable<DoctorAppointmentsViewModel> {
//   debugger;
//   return this.http.post<any>(`${environment.appointmentApiUrl}/GetAppointments`, filter).pipe(
//     map(res => ({
//       appointments: (res.data || []).map((a: any) => ({
//         appointmentID: a.AppointmentID,
//         doctorID: a.DoctorID,
//         patientID: a.PatientID,
//         patientName: a.PatientName,
//         doctorName: a.DoctorName,
//         appointmentDate: a.AppointmentDate,
//         startTime: a.StartTime,
//         endTime: a.EndTime,
//         reasonForVisit: a.ReasonForVisit || '',
//         status: a.StatusName || a.Status,
//         createdOn: a.CreatedOn,
//         modifiedOn: a.ModifiedOn,
//         isActive: a.IsActive,
//         medicalConcern: a.MedicalConcern,
//         medicalHistory: a.MedicalHistory,
//         insuranceDetails: a.InsuranceDetails,
//         patientEmail: a.PatientEmail,
//         patientPhone: a.PatientPhone,
//         patientDateOfBirth: a.PatientDOB,
//         patientGender: a.PatientGender,
//         patientBloodGroup: a.PatientBloodGroup
//       })),

//       appointmentFiles: res.files || [],
//       totalRecords: res.totalRecords || 0
//     }))

//   );
// }
