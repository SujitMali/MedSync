import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminLayoutComponent } from './admin-layout/admin-layout.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AddDoctorsComponent } from './add-doctors/add-doctors.component';
import { ManageSlotsComponent } from './manage-slots/manage-slots.component';
import { ManageCredentialsComponent } from './manage-credentials/manage-credentials.component';
import { authGuard } from '../core/guards/auth.guard';
import { ViewDoctorsComponent } from './view-doctors/view-doctors.component';

const routes: Routes = [
  {
    path: '',
    component: AdminLayoutComponent,
    canActivate: [authGuard],
    data: { roles: ['Admin'] },
    children: [
      { path: 'dashboard', component: DashboardComponent },
      { path: 'add-doctor', component: AddDoctorsComponent },
      { path: 'manage-schedules', component: ManageSlotsComponent },
      { path: 'manage-credentials', component: ManageCredentialsComponent },
      { path: 'view-doctors', component: ViewDoctorsComponent },
      { path: 'edit-doctor/:id', component: AddDoctorsComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
