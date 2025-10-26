import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AddDoctorsComponent } from './add-doctors/add-doctors.component';
import { AdminLayoutComponent } from './admin-layout/admin-layout.component';
import { FormsModule } from '@angular/forms';
import { FullCalendarModule } from '@fullcalendar/angular';
import { ManageSlotsComponent } from './manage-slots/manage-slots.component';



@NgModule({
  declarations: [
    DashboardComponent,
    AddDoctorsComponent,
    AdminLayoutComponent,
    ManageSlotsComponent
  ],
  imports: [
    FullCalendarModule,
    CommonModule,
    AdminRoutingModule,
    FormsModule

  ]
})
export class AdminModule { }
