import { inject } from '@angular/core';
import { Router, type CanActivateFn } from '@angular/router';
import { UserService } from '../services/user.service';

/**
 * Strażnik uprawnień administratora - chroni trasy tylko dla adminów
 */
export const adminGuard: CanActivateFn = (route, state) => {
  const userService = inject(UserService);
  const router = inject(Router);

  // Sprawdź czy użytkownik jest zalogowany
  if (!userService.isAuthenticated()) {
    sessionStorage.setItem('redirectUrl', state.url);
    return router.parseUrl('/login');
  }

  // Sprawdź czy użytkownik ma uprawnienia administratora
  if (userService.isAdmin()) {
    return true;
  }

  // Przekieruj do strony braku uprawnień
  return router.parseUrl('/unauthorized');
};
