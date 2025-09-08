import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth-guard';
import { roleGuard } from './core/guards/role-guard';
import { LoginComponent } from './features/auth/login/login.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { 
    path: 'admin', 
    canActivate: [authGuard, roleGuard],
    data: { expectedRole: 'ADMIN' },
    loadComponent: () => import('./features/admin/dashboard/dashboard.component').then(m => m.default || m)
  },
  { 
    path: 'manager', 
    canActivate: [authGuard, roleGuard],
    data: { expectedRole: 'MANAGER' },
    loadComponent: () => import('./features/manager/dashboard/dashboard.component').then(m => m.default || m)
  },
  { 
    path: 'employee', 
    canActivate: [authGuard, roleGuard],
    data: { expectedRole: 'EMPLOYE' },
    loadComponent: () => import('./features/employee/dashboard/dashboard.component').then(m => m.default || m)
  },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: '**', redirectTo: '/login' }
];
