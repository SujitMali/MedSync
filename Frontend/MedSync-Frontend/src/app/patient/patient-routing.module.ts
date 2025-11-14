import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { ViewDoctorsListComponent } from './view-doctors-list/view-doctors-list.component';
import { BookAppointmentComponent } from './book-appointment/book-appointment.component';
import { PatientLayoutComponent } from './patient-layout/patient-layout.component';


const routes: Routes = [{
  path: '', component: PatientLayoutComponent, children: [
    { path: '', component: LandingPageComponent },
    { path: 'view-doctors-list', component: ViewDoctorsListComponent },
    { path: 'book-appointment/:doctorId', component: BookAppointmentComponent }
  ]
},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PatientRoutingModule { }
