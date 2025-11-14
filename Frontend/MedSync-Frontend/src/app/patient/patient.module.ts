import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PatientRoutingModule } from './patient-routing.module';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { ViewDoctorsListComponent } from './view-doctors-list/view-doctors-list.component';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { BookAppointmentComponent } from './book-appointment/book-appointment.component';
import { PatientLayoutComponent } from './patient-layout/patient-layout.component';
import { NgxSpinnerComponent } from "ngx-spinner";
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';


@NgModule({
  declarations: [
    LandingPageComponent,
    ViewDoctorsListComponent,
    BookAppointmentComponent,
    PatientLayoutComponent
  ],
  imports: [
    CommonModule,
    PatientRoutingModule,
    FormsModule,
    NgSelectModule,
    NgxSpinnerComponent,
    NgbPaginationModule
  ],
  exports: [
    PatientLayoutComponent
  ]
})
export class PatientModule { }
