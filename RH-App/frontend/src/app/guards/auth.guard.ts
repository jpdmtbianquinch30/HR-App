// guards/auth.guard.ts
@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const currentUser = this.authService.currentUserValue;
    if (currentUser && currentUser.token) {
      // V�rifier si la route n�cessite un r�le sp�cifique
      const expectedRole = route.data.expectedRole;
      if (expectedRole && currentUser.role !== expectedRole) {
        this.router.navigate(['/']);
        return false;
      }
      return true;
    }

    this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
    return false;
  }
}