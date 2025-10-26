import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PatientRoutingModule } from './patient-routing.module';
import { LandingPageComponent } from './landing-page/landing-page.component';


@NgModule({
  declarations: [
    LandingPageComponent
  ],
  imports: [
    CommonModule,
    PatientRoutingModule
  ]
})
export class PatientModule { }
