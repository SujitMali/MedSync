import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PatientService {

  constructor(private http: HttpClient) { }

  getDoctorsList(filters: any): Observable<any> {
    return this.http.post(`${environment.doctorApiUrl}/GetAllDoctorsList`, filters);
  }

  getAllFiltersDropdown(): Observable<any> {
    return this.http.get(`${environment.doctorApiUrl}/GetDropdownData`);
  }

  getDoctorSlots(doctorId: number, appointmentDate: string): Observable<any> {
    return this.http.get(`${environment.doctorApiUrl}/GetDoctorSlots?doctorId=${doctorId}&appointmentDate=${appointmentDate}`);
  }

  submitAppointmentWithFiles(formData: FormData): Observable<any> {
    debugger;
    return this.http.post(`${environment.appointmentApiUrl}/BookAppointment`, formData);
  }
}



