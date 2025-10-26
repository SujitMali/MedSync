import { Component } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {
  dashboardCards = [
    { title: 'Total Doctors', value: 120, icon: 'fa-hospital' },
    { title: 'Total Patients', value: 450, icon: 'fa-users' },
    { title: 'Appointments', value: 78, icon: 'fa-calendar-check' },
    { title: 'Revenue', value: '$12,400', icon: 'fa-money-bill' },
  ];
}
