import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="container">
      <h1>Dashboard Administrateur</h1>
      <p>Bienvenue dans l'interface d'administration</p>
    </div>
  `,
  styles: []
})
export default class DashboardComponent {}