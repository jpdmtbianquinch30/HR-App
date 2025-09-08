import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot, CanActivate } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { map, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    return this.authService.currentUser$.pipe(
      take(1),
      map(user => {
        if (user) {
          // Check if route has role requirements
          if (route.data && route.data['roles']) {
            const requiredRoles = route.data['roles'] as string[];
            if (!requiredRoles.includes(user.role)) {
              // User doesn't have required role, redirect to appropriate dashboard
              this.redirectToDashboard(user.role);
              return false;
            }
          }
          return true;
        }

        // Not logged in, redirect to login page with return url
        this.router.navigate(['/auth/login'], { 
          queryParams: { returnUrl: state.url } 
        });
        return false;
      })
    );
  }

  private redirectToDashboard(role: string) {
    switch (role) {
      case 'ADMIN':
        this.router.navigate(['/dashboard/admin']);
        break;
      case 'MANAGER':
        this.router.navigate(['/dashboard/manager']);
        break;
      case 'EMPLOYE':
        this.router.navigate(['/dashboard/employee']);
        break;
      default:
        this.router.navigate(['/auth/login']);
    }
  }
}