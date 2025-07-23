// app.component.ts
import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router'; 
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { CommonModule } from '@angular/common';
import { CustomNavbarComponent } from './layout/custom-navbar.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    CommonModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatSidenavModule,
    CustomNavbarComponent
  ],
  template: `
    <mat-toolbar class="mat-elevation-z3">
      <button mat-icon-button (click)="sidenavOpened.set(!sidenavOpened())">
        <mat-icon>menu</mat-icon>
      </button>
      <span>CloudCare</span>
    </mat-toolbar>

    <mat-sidenav-container class="container">
      <mat-sidenav mode="over" [(opened)]="sidenavOpened">
        <app-custom-navbar (closeSidenav)="sidenavOpened.set(false)" />
      </mat-sidenav>

      <mat-sidenav-content class="content">
        <router-outlet></router-outlet>
      </mat-sidenav-content>
    </mat-sidenav-container>
  `,
  styles: [`
    .container {
      height: calc(100vh - 64px);
    }

    mat-toolbar {
      position: sticky;
      top: 0;
      z-index: 10;
    }

    .content {
      padding: 24px;
    }
  `]
})
export class AppComponent {
  title = 'CloudCare';
  sidenavOpened = signal(false);
}