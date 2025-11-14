import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminRoutingModule } from './admin-routing.module';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AddDoctorsComponent } from './add-doctors/add-doctors.component';
import { AdminLayoutComponent } from './admin-layout/admin-layout.component';
import { FormsModule } from '@angular/forms';
import { FullCalendarModule } from '@fullcalendar/angular';
import { ManageSlotsComponent } from './manage-slots/manage-slots.component';
import { ManageCredentialsComponent } from './manage-credentials/manage-credentials.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { ViewDoctorsComponent } from './view-doctors/view-doctors.component';



@NgModule({
  declarations: [
    DashboardComponent,
    AddDoctorsComponent,
    AdminLayoutComponent,
    ManageSlotsComponent,
    ManageCredentialsComponent,
    ViewDoctorsComponent
  ],
  imports: [
    FullCalendarModule,
    CommonModule,
    AdminRoutingModule,
    FormsModule,
    NgbPaginationModule,
    NgSelectModule,
  ]
})
export class AdminModule { }
