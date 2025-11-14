import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ViewAppointmentRequestComponent } from './view-appointment-request/view-appointment-request.component';
import { DoctorLayoutComponent } from './doctor-layout/doctor-layout.component';
import { authGuard } from '../core/guards/auth.guard';

const routes: Routes = [
  {
    path: '',
    component: DoctorLayoutComponent,
    canActivate: [authGuard],
    data: { roles: ['Doctor'] },
    children: [
      { path: 'view-appointment-request', component: ViewAppointmentRequestComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DoctorRoutingModule { }