import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-doctor-layout',
  templateUrl: './doctor-layout.component.html',
  styleUrl: './doctor-layout.component.scss'
})
export class DoctorLayoutComponent {

  isCollapsed = false;
  isLocked = true;
  activeMenu = 'Dashboard';
  showUserMenu = false;

  user: any;

  constructor(
    private router: Router,
    private authService: AuthService
  ) {
    this.user = this.authService['getUserFromStorage']();
  }

  menuItems = [
    { label: 'View Appointments', icon: 'fa-gauge', route: 'view-appointment-request' },
    // { label: 'Add Doctor', icon: 'fa-user-doctor', route: '' },
    // { label: 'Manage Schedules', icon: 'fa-users', route: '' },
    // { label: 'Appointments', icon: 'fa-calendar-check', route: '' },
    // { label: 'Reports', icon: 'fa-chart-column', route: '' },
    // { label: 'Settings', icon: 'fa-gear', route: '' },
  ];

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

  toggleUserMenu() {
    this.showUserMenu = !this.showUserMenu;
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }


}


