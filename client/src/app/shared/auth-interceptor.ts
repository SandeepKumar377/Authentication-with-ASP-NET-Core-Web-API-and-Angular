import { HttpInterceptorFn } from '@angular/common/http';
import { AuthService } from './services/auth-service';
import { inject } from '@angular/core';
import { tap } from 'rxjs';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const toastr = inject(ToastrService);

  if (authService.isLoggedIn()) {
    const token = authService.getToken();
    const authReq = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${token}`)
    });
    return next(authReq).pipe(
      tap({
        error: (err) => {
          if (err.status === 401) {
            authService.deleteToken();
            setTimeout(() => {
              toastr.info('Please log in again.', 'Session Expired');
            }, 1500);
            router.navigateByUrl('/signin');
          }
          else if (err.status === 403) {
            toastr.error('Access Denied : ', 'You do not have permission to access this resource.');
          }
        }
      })
    );
  }
  else {
    return next(req);
  }
};
