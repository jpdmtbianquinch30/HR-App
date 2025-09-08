import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-employee-dashboard',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="container">
      <h1>Dashboard Employé</h1>
      <p>Bienvenue dans votre espace employé</p>
    </div>
  `,
  styles: []
})
export default class DashboardComponent {}