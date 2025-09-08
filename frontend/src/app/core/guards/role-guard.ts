import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const roleGuard: CanActivateFn = (route) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  
  const currentUser = authService.currentUserValue;
  const expectedRole = route.data['expectedRole'];

  if (!currentUser) {
    router.navigate(['/login']);
    return false;
  }

  if (currentUser.role !== expectedRole) {
    router.navigate(['/unauthorized']);
    return false;
  }

  return true;
};