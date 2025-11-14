import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DoctorRoutingModule } from './doctor-routing.module';
import { ViewAppointmentRequestComponent } from './view-appointment-request/view-appointment-request.component';
import { FormsModule } from '@angular/forms';
import { FullCalendarModule } from '@fullcalendar/angular';
import { DoctorLayoutComponent } from './doctor-layout/doctor-layout.component';
import { NgSelectModule } from '@ng-select/ng-select';


@NgModule({
  declarations: [
    ViewAppointmentRequestComponent,
    DoctorLayoutComponent
  ],
  imports: [
    CommonModule,
    DoctorRoutingModule,
    FormsModule,
    FullCalendarModule,
    NgSelectModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class DoctorModule { }
