import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-admin-layout',
  templateUrl: './admin-layout.component.html',
  styleUrls: ['./admin-layout.component.scss']
})
export class AdminLayoutComponent {
  isCollapsed = false;
  isLocked = true;
  activeMenu = 'Dashboard';
  showUserMenu = false;

  user: any;

  menuItems = [
    { label: 'Dashboard', icon: 'fa-gauge', route: 'dashboard' },
    { label: 'Add Doctor', icon: 'fa-user-doctor', route: 'add-doctor' },
    { label: 'Manage Credentials', icon: 'fa-calendar-check', route: 'manage-credentials' },
    { label: 'Manage Schedules', icon: 'fa-users', route: 'manage-schedules' },
    { label: 'View Doctors', icon: 'fa-chart-column', route: 'view-doctors' },
    { label: 'Settings', icon: 'fa-gear', route: 'settings' },
  ];

  constructor(
    private router: Router,
    private authService: AuthService
  ) {
    this.user = this.authService['getUserFromStorage']();
  }

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
