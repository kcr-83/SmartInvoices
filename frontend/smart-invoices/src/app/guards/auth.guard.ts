import { inject } from '@angular/core';
import { Router, type CanActivateFn } from '@angular/router';
import { DOCUMENT } from '@angular/common';
import { UserService } from '../services/user.service';

/**
 * Strażnik uwierzytelniania - chroni trasy wymagające zalogowania
 */
export const authGuard: CanActivateFn = (route, state) => {
  const userService = inject(UserService);
  const router = inject(Router);
  const document = inject(DOCUMENT);

  if (userService.isAuthenticated()) {
    return true;
  }

  // Zapisz URL próby dostępu do przekierowania po zalogowaniu (tylko w przeglądarce)
  if (typeof document.defaultView?.sessionStorage !== 'undefined') {
    document.defaultView.sessionStorage.setItem('redirectUrl', state.url);
  }

  // Przekieruj do strony logowania
  return router.parseUrl('/login');
};
