import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-layout',
  templateUrl: './admin-layout.component.html',
  styleUrls: ['./admin-layout.component.scss']
})
export class AdminLayoutComponent {
  isCollapsed = false;
  isLocked = true;
  activeMenu = 'Dashboard';

  menuItems = [
    { label: 'Dashboard', icon: 'fa-gauge', route: 'dashboard' },
    { label: 'Add Doctor', icon: 'fa-user-doctor', route: 'add-doctor' },
    { label: 'Manage Schedules', icon: 'fa-users', route: 'manage-schedules' },
    { label: 'Appointments', icon: 'fa-calendar-check', route: 'appointments' },
    { label: 'Reports', icon: 'fa-chart-column', route: 'reports' },
    { label: 'Settings', icon: 'fa-gear', route: 'settings' },
  ];

  constructor(private router: Router) { }

  onSidebarHover(isHovering: boolean) {
    if (!this.isLocked) this.isCollapsed = !isHovering;
  }

  toggleSidebarLock() {
    this.isLocked = !this.isLocked;
    this.isCollapsed = !this.isLocked;
  }

  navigate(item: any) {
    this.activeMenu = item.label;
    this.router.navigate([`/admin/${item.route}`]);
  }
}
