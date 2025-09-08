import { Component } from '@angular/core';

@Component({
  selector: 'app-auth',
  template: `
    <div class="auth-container">
      <div class="auth-card">
        <div class="auth-header">
          <i class="fas fa-users"></i>
          <h1>RH Management</h1>
          <p>Système de Gestion des Ressources Humaines</p>
        </div>
        <router-outlet></router-outlet>
      </div>
    </div>
  `,
  styleUrls: ['./auth.component.css']
})
export class AuthComponent { }