
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminLayoutComponent } from './admin-layout/admin-layout.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AddDoctorsComponent } from './add-doctors/add-doctors.component';
import { ManageSlotsComponent } from './manage-slots/manage-slots.component';

const routes: Routes = [
  {
    path: '',
    component: AdminLayoutComponent,
    children: [
      { path: 'dashboard', component: DashboardComponent },
      { path: 'add-doctor', component: AddDoctorsComponent },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'manage-schedules', component: ManageSlotsComponent },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
