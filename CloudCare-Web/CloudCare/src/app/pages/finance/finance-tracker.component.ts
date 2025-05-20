import { Component, OnInit, AfterViewInit, ViewChild, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatSort, Sort, MatSortModule } from '@angular/material/sort';
import { MatTabsModule } from '@angular/material/tabs';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule, MatDateRangePicker, MatDateRangeInput } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { LiveAnnouncer } from '@angular/cdk/a11y';
import { ExpenseService } from '../../Services/expense.service';
import { IExpenseRead } from '../../Models/expense-read.model';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { ExpenseFormComponent } from './components/expense-form.component';


@Component({
  selector: 'app-finance-tracker',
  standalone: true,
  imports: [
    CommonModule,
    MatTabsModule,
    MatTableModule,
    MatSortModule,
    MatIconModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatDialogModule
  ],
  template: `
    <div class="finance-tracker-page">
      <h1>Finance Tracker</h1>

      <mat-tab-group>
        <mat-tab label="Expenses">
          <section class="filters-card">
            <div class="filters-header">
              <mat-icon>filter_list</mat-icon>
              <span>Filters</span>
            </div>
            <div class="filter-bar">
              <!-- Date‐range picker -->
              <mat-form-field appearance="outline" class="filter-field">
                <mat-label>Date range</mat-label>
                <mat-date-range-input [rangePicker]="picker">
                  <input matStartDate placeholder="From">
                  <input matEndDate placeholder="To">
                </mat-date-range-input>
                <mat-datepicker-toggle matSuffix [for]="picker"></mat-datepicker-toggle>
                <mat-date-range-picker #picker></mat-date-range-picker>
              </mat-form-field>

              <!-- Vendor autocomplete or input -->
              <mat-form-field appearance="outline" class="filter-field">
                <mat-label>Vendor</mat-label>
                <input matInput placeholder="e.g. Walmart">
              </mat-form-field>
            </div>
          </section>

          <div class="table-container">
            <table
              mat-table
              [dataSource]="dataSource"
              matSort
              (matSortChange)="announceSortChange($event)"
              class="mat-elevation-z1"
            >
              <!-- Date Column -->
              <ng-container matColumnDef="date">
                <th mat-header-cell *matHeaderCellDef mat-sort-header> Date </th>
                <td mat-cell *matCellDef="let e"> {{ e.date | date:'shortDate' }} </td>
              </ng-container>

              <!-- Description Column -->
              <ng-container matColumnDef="description">
                <th mat-header-cell *matHeaderCellDef mat-sort-header> Description </th>
                <td mat-cell *matCellDef="let e"> {{ e.description }} </td>
              </ng-container>

              <!-- Amount Column -->
              <ng-container matColumnDef="amount">
                <th mat-header-cell *matHeaderCellDef mat-sort-header> Amount </th>
                <td mat-cell *matCellDef="let e"> {{ '$' + e.amount }} </td>
              </ng-container>

              <!-- Category Column -->
              <ng-container matColumnDef="category">
                <th mat-header-cell *matHeaderCellDef mat-sort-header> Category </th>
                <td mat-cell *matCellDef="let e"> {{ e.category }} </td>
              </ng-container>

              <!-- Payment Column -->
              <ng-container matColumnDef="paymentMethod">
                <th mat-header-cell *matHeaderCellDef mat-sort-header> Payment </th>
                <td mat-cell *matCellDef="let e"> {{ e.paymentMethod }} </td>
              </ng-container>

              <!-- Actions Column -->
             <ng-container matColumnDef="actions">
  <th mat-header-cell *matHeaderCellDef> Actions </th>
  <td mat-cell *matCellDef="let expense">
    <button mat-icon-button color="primary" (click)="editExpense(expense)">
      <mat-icon>edit</mat-icon>
    </button>

    <button mat-icon-button color="warn" (click)="deleteExpense(expense.id)">
      <mat-icon>delete</mat-icon>
    </button>
  </td>
</ng-container>

              <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
              <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
            </table>
          </div>

          <!-- FAB -->
          <button mat-fab color="primary" class="floating-fab" (click)="openExpenseForm()">
            <mat-icon>add</mat-icon>
          </button>
        </mat-tab>

        <mat-tab label="Recurring Expenses">
          <p>Recurring expense content here…</p>
        </mat-tab>

        <mat-tab label="Categories & Vendors">
          <p>Category/vendor summary here…</p>
        </mat-tab>
      </mat-tab-group>
    </div>
  `,
  styles: [`
    .finance-tracker-page {
      padding: 1.5rem;
      background: #fafafa;
    }
    h1 {
      margin-bottom: 1rem;
    }
    .filters-card {
      background: white;
      padding: 1rem;
      border-radius: 8px;
      box-shadow: 0 1px 3px rgba(0,0,0,0.1);
      margin-bottom: 1rem;
    }
    .filters-header {
      display: flex;
      align-items: center;
      gap: 0.5rem;
      font-weight: 600;
      margin-bottom: 0.75rem;
    }
    .filter-bar {
      display: flex;
      gap: 1rem;
      flex-wrap: wrap;
    }
    .filter-field {
      flex: 1;
      min-width: 180px;
    }
    .table-container {
      background: white;
      border-radius: 4px;
      overflow: auto;
      box-shadow: 0 1px 2px rgba(0,0,0,0.1);
    }
    table {
      width: 100%;
    }
    .floating-fab {
      position: fixed;
      bottom: 1.5rem;
      right: 1.5rem;
    }
  `]
})
export class FinanceTrackerComponent implements OnInit, AfterViewInit {
  private _liveAnnouncer = inject(LiveAnnouncer);
  @ViewChild(MatSort) sort!: MatSort;

  displayedColumns = ['date', 'description', 'amount', 'category', 'paymentMethod', 'actions'];
  dataSource = new MatTableDataSource<IExpenseRead>([]);

  constructor(public expenseSvc: ExpenseService, private dialog: MatDialog) {}
//we need to inject the dialog service so we can open the model, etc dialog.open()
  ngOnInit(): void {
    this.expenseSvc.getExpenses();
    this.expenseSvc.expenses$.subscribe(data => this.dataSource.data = data);
  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
  }

  announceSortChange(sortState: Sort) {
    const dir = sortState.direction;
    this._liveAnnouncer.announce(dir ? `Sorted ${dir}ending` : 'Sorting cleared');
  }

  editExpense(e: IExpenseRead) {
    console.log('Edit clicked:', e);
    const dialogRef = this.dialog.open(ExpenseFormComponent, {
    width: '500px',
    data: e //  passing expense
  });

  dialogRef.afterClosed().subscribe(result => { //afterclosed is triggered when the model is closed and it reurns the res
    if (result) {
      console.log('Created:', result);

      //for edit 
    }
  });

  }
  openExpenseForm() {
    console.log('Open form clicked');
    //creating the dialog, Open the model 
  const dialogRef = this.dialog.open(ExpenseFormComponent, {
    width: '500px',
    data: null // No data passed
  });

  dialogRef.afterClosed().subscribe(result => { //afterclosed is triggered when the model is closed and it reurns the res
    if (result) {
      console.log('Created:', result);

      //call the svc for create expesne 
    }
  });
  }

  deleteExpense(id: number) {
  if (confirm('Are you sure you want to delete this expense?')) {
      //call delete function 
      console.log('Deleted expense:', id); 
      
  }
}
}