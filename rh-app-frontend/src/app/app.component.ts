import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './auth/auth.service';
import { User } from './shared/models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'RH Frontend';
  currentUser: User | null = null;
  isMenuOpen = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
    });
  }

  toggleMenu() {
    this.isMenuOpen = !this.isMenuOpen;
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/auth/login']);
  }

  navigateToDashboard() {
    if (this.currentUser) {
      switch (this.currentUser.role) {
        case 'ADMIN':
          this.router.navigate(['/dashboard/admin']);
          break;
        case 'MANAGER':
          this.router.navigate(['/dashboard/manager']);
          break;
        case 'EMPLOYE':
          this.router.navigate(['/dashboard/employee']);
          break;
      }
    }
  }
}