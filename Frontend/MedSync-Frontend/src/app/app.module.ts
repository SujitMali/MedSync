import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule, provideHttpClient, withInterceptors } from '@angular/common/http';
import { FullCalendarModule } from '@fullcalendar/angular';
import { LoginComponent } from './Shared/Components/login/login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NotAuthorizedComponent } from './Shared/Components/not-authorized/not-authorized.component';
import { ServerOfflineComponent } from './Shared/Components/server-offline/server-offline.component';
import { authInterceptor } from './core/interceptors/auth.interceptor';
import { NgxSpinnerModule } from 'ngx-spinner';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { PatientModule } from './patient/patient.module';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    NotAuthorizedComponent,
    ServerOfflineComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FullCalendarModule,
    FormsModule,
    NgxSpinnerModule.forRoot({ type: 'ball-scale-multiple' }),
    BrowserAnimationsModule,
    NgbModule,
    ReactiveFormsModule,
    PatientModule
  ],
  providers: [provideHttpClient(withInterceptors([authInterceptor]))],
  bootstrap: [AppComponent]
})
export class AppModule { }
