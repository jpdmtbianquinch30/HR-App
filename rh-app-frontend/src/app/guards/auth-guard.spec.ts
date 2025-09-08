import { TestBed } from '@angular/core/testing';
import { Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { of } from 'rxjs';
import { AuthGuard } from './auth-guard';
import { AuthService } from '../auth/auth.service';
import { User } from '../shared/models/user';

describe('AuthGuard', () => {
  let guard: AuthGuard;
  let authService: jasmine.SpyObj<AuthService>;
  let router: jasmine.SpyObj<Router>;

  const mockUser: User = {
    id: 1,
    email: 'test@example.com',
    firstName: 'Test',
    lastName: 'User',
    role: 'ADMIN',
    createdAt: new Date(),
    updatedAt: new Date()
  };

  const createMockActivatedRouteSnapshot = (data?: any): ActivatedRouteSnapshot => {
    return {
      url: [],
      params: {},
      queryParams: {},
      fragment: null,
      data: data || {},
      outlet: 'primary',
      component: null,
      routeConfig: null,
      root: {} as ActivatedRouteSnapshot,
      parent: null,
      firstChild: null,
      children: [],
      pathFromRoot: [],
      paramMap: jasmine.createSpyObj('ParamMap', ['get', 'getAll', 'has', 'keys']),
      queryParamMap: jasmine.createSpyObj('ParamMap', ['get', 'getAll', 'has', 'keys']),
      title: undefined,
      toString: () => ''
    } as ActivatedRouteSnapshot;
  };

  const createMockRouterStateSnapshot = (url: string): RouterStateSnapshot => {
    return {
      url,
      root: {} as ActivatedRouteSnapshot
    } as RouterStateSnapshot;
  };
  beforeEach(() => {
    const authServiceSpy = jasmine.createSpyObj('AuthService', [], {
      currentUser$: of(mockUser)
    });
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    TestBed.configureTestingModule({
      providers: [
        AuthGuard,
        { provide: AuthService, useValue: authServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    });

    guard = TestBed.inject(AuthGuard);
    authService = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });

  it('should allow access for authenticated user with correct role', (done) => {
    const route = createMockActivatedRouteSnapshot({ roles: ['ADMIN'] });
    const state = createMockRouterStateSnapshot('/dashboard/admin');

    const result = guard.canActivate(route, state);
    if (typeof result === 'object' && 'subscribe' in result) {
      result.subscribe(canActivate => {
        expect(canActivate).toBe(true);
        done();
      });
    } else {
      expect(result).toBe(true);
      done();
    }
  });

  it('should deny access and redirect for user without required role', (done) => {
    // Set user with EMPLOYE role trying to access ADMIN route
    Object.defineProperty(authService, 'currentUser$', {
      value: of({ ...mockUser, role: 'EMPLOYE' } as User)
    });
    
    const route = createMockActivatedRouteSnapshot({ roles: ['ADMIN'] });
    const state = createMockRouterStateSnapshot('/dashboard/admin');

    const result = guard.canActivate(route, state);
    if (typeof result === 'object' && 'subscribe' in result) {
      result.subscribe(canActivate => {
        expect(canActivate).toBe(false);
        expect(router.navigate).toHaveBeenCalled();
        done();
      });
    }
  });

  it('should deny access and redirect to login for unauthenticated user', (done) => {
    Object.defineProperty(authService, 'currentUser$', {
      value: of(null)
    });
    
    const route = createMockActivatedRouteSnapshot();
    const state = createMockRouterStateSnapshot('/dashboard/admin');

    const result = guard.canActivate(route, state);
    if (typeof result === 'object' && 'subscribe' in result) {
      result.subscribe(canActivate => {
        expect(canActivate).toBe(false);
        expect(router.navigate).toHaveBeenCalledWith(['/login'], {
          queryParams: { returnUrl: '/dashboard/admin' }
        });
        done();
      });
    }
  });
});