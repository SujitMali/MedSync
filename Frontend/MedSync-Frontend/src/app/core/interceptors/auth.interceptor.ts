import { HttpInterceptorFn, HttpRequest, HttpHandlerFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, finalize } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToasterService } from '../../Shared/Services/toaster.service';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req: HttpRequest<unknown>, next: HttpHandlerFn) => {
  const router = inject(Router);
  const spinner = inject(NgxSpinnerService);
  const toastr = inject(ToasterService);
  const authService = inject(AuthService);

  const publicEndpoints = ['/login'];
  const isPublic = publicEndpoints.some(url => req.url.includes(url));

  spinner.show();

  const token = authService.getToken();
  let authReq = req;

  if (!isPublic && token) {
    authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      debugger;
      if (error.status === 401) {

        toastr.error(error.error.message);
        authService.logout();
        router.navigate(['/login']);

      } else if (error.status === 403) {
        toastr.warning(error.error.message);
        router.navigate(['/unauthorized']);
      }
      return throwError(() => error);
    }),
    finalize(() => {
      spinner.hide();
    })
  );
};
