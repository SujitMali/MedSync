import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './Shared/Components/login/login.component';
import { NotAuthorizedComponent } from './Shared/Components/not-authorized/not-authorized.component';
import { authGuard } from './core/guards/auth.guard';


const routes: Routes = [
  { path: 'admin', loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule) },
  { path: '', loadChildren: () => import('./patient/patient.module').then(m => m.PatientModule) },
  { path: 'doctor', loadChildren: () => import('./doctor/doctor.module').then(m => m.DoctorModule) },
  { path: 'login', component: LoginComponent, canActivate: [authGuard] },
  { path: 'unauthorized', component: NotAuthorizedComponent }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
