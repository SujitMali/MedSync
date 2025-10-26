import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Doctor } from '../Shared/Models/doctor.model';
import { DoctorSchedule } from '../Shared/Models/doctor-schedule.model';
import { SlotDuration } from '../Shared/Models/slot-duration.model';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private baseUrl = 'https://localhost:44398/api/admin'; // backend URL

  constructor(private http: HttpClient) { }

  getDropdownData(): Observable<any> {
    return this.http.get(`${this.baseUrl}/get-dropdown-data`);
  }


  addDoctor(formData: FormData) {
    return this.http.post(`${this.baseUrl}/add-doctor`, formData);
  }


  // Get list of all active doctors
  getDoctorsList(): Observable<Doctor[]> {

    return this.http.get<Doctor[]>(`${this.baseUrl}/get-doctors-list`);
  }

  // Get schedule for a specific doctor
  getDoctorSchedule(doctorID: number): Observable<DoctorSchedule[]> {
    return this.http.get<DoctorSchedule[]>(`${this.baseUrl}/get-doctor-schedule/${doctorID}`);
  }

  // Save or update doctor's schedule
  saveDoctorSchedule(schedule: DoctorSchedule[]): Observable<any> {
    return this.http.post(`${this.baseUrl}/save-doctor-schedules`, schedule);
  }

  getSlotDurations(): Observable<SlotDuration[]> {
    return this.http.get<SlotDuration[]>(`${this.baseUrl}/get-slot-durations`);
  }

}
