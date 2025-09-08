import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { of, throwError } from 'rxjs';
import { LoginComponent } from './login.component';
import { AuthService } from '../auth.service';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let authService: jasmine.SpyObj<AuthService>;
  let router: jasmine.SpyObj<Router>;
  let activatedRoute: any;

  beforeEach(() => {
    const authServiceSpy = jasmine.createSpyObj('AuthService', ['login'], {
      currentUser$: of(null)
    });
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    activatedRoute = {
      snapshot: {
        queryParams: {}
      }
    };

    TestBed.configureTestingModule({
      declarations: [LoginComponent],
      imports: [ReactiveFormsModule],
      providers: [
        { provide: AuthService, useValue: authServiceSpy },
        { provide: Router, useValue: routerSpy },
        { provide: ActivatedRoute, useValue: activatedRoute }
      ]
    });

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    authService = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form with validators', () => {
    expect(component.loginForm.get('email')?.hasError('required')).toBe(true);
    expect(component.loginForm.get('password')?.hasError('required')).toBe(true);
    
    component.loginForm.patchValue({
      email: 'invalid-email',
      password: '123'
    });
    
    expect(component.loginForm.get('email')?.hasError('email')).toBe(true);
    expect(component.loginForm.get('password')?.hasError('minlength')).toBe(true);
  });

  it('should call authService.login on valid form submission', () => {
    const mockResponse = {
      token: 'test-token',
      user: {
        id: 1,
        email: 'test@example.com',
        firstName: 'Test',
        lastName: 'User',
        role: 'ADMIN',
        createdAt: new Date(),
        updatedAt: new Date()
      }
    };

    authService.login.and.returnValue(of(mockResponse));
    
    component.loginForm.patchValue({
      email: 'test@example.com',
      password: 'password123'
    });

    component.onSubmit();

    expect(authService.login).toHaveBeenCalledWith({
      email: 'test@example.com',
      password: 'password123'
    });
    expect(router.navigate).toHaveBeenCalledWith(['/dashboard/admin']);
  });

  it('should handle login error', () => {
    const mockError = { error: { message: 'Invalid credentials' } };
    authService.login.and.returnValue(throwError(() => mockError));
    
    component.loginForm.patchValue({
      email: 'test@example.com',
      password: 'wrongpassword'
    });

    component.onSubmit();

    expect(component.errorMessage).toBe('Invalid credentials');
    expect(component.isLoading).toBe(false);
  });

  it('should set demo credentials', () => {
    component.loginAsAdmin();
    expect(component.loginForm.get('email')?.value).toBe('admin@example.com');
    expect(component.loginForm.get('password')?.value).toBe('admin123');

    component.loginAsManager();
    expect(component.loginForm.get('email')?.value).toBe('manager@example.com');
    expect(component.loginForm.get('password')?.value).toBe('manager123');

    component.loginAsEmployee();
    expect(component.loginForm.get('email')?.value).toBe('employee@example.com');
    expect(component.loginForm.get('password')?.value).toBe('employee123');
  });
});