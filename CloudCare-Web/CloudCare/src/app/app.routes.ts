import { Routes } from '@angular/router';
import { FinanceTrackerComponent } from './pages/finance/finance-tracker.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';

export const routes: Routes = [
    {
        path: '',
        pathMatch: 'full',
        redirectTo: 'dashboard'
    },
    {
        path: 'dashboard',
        component: DashboardComponent

    },
    {
        path: 'finance',
        component: FinanceTrackerComponent
    },



    
];
