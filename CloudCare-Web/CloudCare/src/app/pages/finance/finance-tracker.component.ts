import { Component } from '@angular/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ExpenseService } from '../../Services/expense.service';
import { IExpenseRead } from '../../Models/expense-read.model';
import { Observable } from 'rxjs';


@Component({
  selector: 'app-finance-tracker',
  standalone: true,
    imports: [
    MatTabsModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatTableModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    MatNativeDateModule,
    MatDialogModule,
  ],
  template: `
  <h1>{{expenses$}}</h1>
    
  `,
  styles: ``
})
export class FinanceTrackerComponent {
  public expenses$!: Observable<IExpenseRead[]>;

  constructor(private expenseSvc: ExpenseService) {
    this.expenses$ = this.expenseSvc.expenses$;
    this.expenseSvc.loadExpenses();
  }
}

