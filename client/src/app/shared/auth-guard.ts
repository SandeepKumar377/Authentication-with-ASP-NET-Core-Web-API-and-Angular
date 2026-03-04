import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from './services/auth-service';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isLoggedIn()) {
    const claimRequired = route.data['claimRequired'] as Function;
    if (claimRequired) {
      const userClaims = authService.getUserClaims();
      if (!claimRequired(userClaims)) {
        router.navigateByUrl('/forbidden');
        return false;
      }
      return true;
    }
    return true;
  }
  else {
    router.navigateByUrl('/signin');
    return false;
  }
};
