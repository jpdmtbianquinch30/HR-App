import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-manager-dashboard',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="container">
      <h1>Dashboard Manager</h1>
      <p>Bienvenue dans l'interface manager</p>
    </div>
  `,
  styles: []
})
export default class DashboardComponent {}