// custom-navbar.component.ts
import { Component, EventEmitter, Output } from '@angular/core';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

export type MenuItem = {
  icon: string;
  label: string;
  route?: string;
};

@Component({
  selector: 'app-custom-navbar',
  standalone: true,
  imports: [CommonModule, MatListModule, MatIconModule, RouterModule],
  template: `
    <div class="sidenav-header">
      <img width="80" height="80" src="assets/pfp.png" />
      <div class="header-text">
        <h2>Daycare Name</h2>
        <p>Provider Name</p>
      </div>
    </div>

    <mat-nav-list>
      <a mat-list-item *ngFor="let item of menuItems" [routerLink]="item.route"
         routerLinkActive #rla="routerLinkActive" [class.active]="rla.isActive"
         (click)="onMenuItemClick()">
        <mat-icon matListItemIcon>{{item.icon}}</mat-icon>
        <span matListItemTitle>{{item.label}}</span>
      </a>
    </mat-nav-list>
  `,
  styles: [`
    .sidenav-header {
      padding-top: 24px;
      text-align: center;
    }

    .sidenav-header img {
      border-radius: 50%;
      object-fit: cover;
      margin-bottom: 10px;
      box-shadow: 0 2px 4px rgba(0,0,0,0.2);
    }

    .header-text h2 {
      margin: 0;
      font-size: 1.1rem;
      line-height: 1.4rem;
      font-weight: 600;
    }

    .header-text p {
      margin: 0;
      font-size: 0.85rem;
      color: #666;
    }

    .active {
      background-color: rgba(0, 0, 0, 0.04);
      border-left: 4px solid #3f51b5;
    }
  `]
})
export class CustomNavbarComponent {
  @Output() closeSidenav = new EventEmitter<void>();

  menuItems: MenuItem[] = [
    { icon: "dashboard", label: "Dashboard", route: "/dashboard" },
    { icon: "account_balance_wallet", label: "Finance Tracker", route: "/finance" },
    { icon: "groups", label: "Students", route: "/students" },
    { icon: "schedule", label: "Time Tracking", route: "/time-tracking" },
    { icon: "receipt_long", label: "Reports", route: "/reports" },
    { icon: "settings", label: "Settings", route: "/settings" },
    { icon: "logout", label: "Logout", route: "/logout" }
  ];

  onMenuItemClick() {
    this.closeSidenav.emit();
  }
}
