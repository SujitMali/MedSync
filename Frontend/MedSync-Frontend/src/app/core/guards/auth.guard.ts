import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { ToasterService } from '../../Shared/Services/toaster.service';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {

  const authService = inject(AuthService);
  const router = inject(Router);
  const toastr = inject(ToasterService);


  const requiredRoles = route.data['roles'] as string[] | undefined;
  const currentUrl = state.url;


  const isLoginRoute = currentUrl.includes('/login');
  const isAuthenticated = authService.isAuthenticated();


  if (isLoginRoute && isAuthenticated) {
    const role = authService.getUserRole();
    if (role === 'Admin') router.navigate(['/admin/dashboard']);
    else if (role === 'Doctor') router.navigate(['/doctor/view-appointment-request']);
    else router.navigate(['/']);
    return false;
  }

  if (!isAuthenticated && !isLoginRoute) {
    toastr.info('Please login to continue.');
    router.navigate(['/login']);
    return false;
  }

  if (requiredRoles && requiredRoles.length > 0) {
    const userRole = authService.getUserRole()?.toLowerCase();
    const isAuthorized = requiredRoles.map(r => r.toLowerCase()).includes(userRole || '');

    if (!isAuthorized) {
      toastr.warning('You are not authorized to access this page.');
      router.navigate(['/unauthorized']);
      return false;
    }
  }
  return true;
};
