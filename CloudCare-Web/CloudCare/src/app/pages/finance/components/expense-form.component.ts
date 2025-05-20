import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { IExpenseRead } from '../../../Models/expense-read.model';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';



//Mat_dialog_data gives you acess to the data you passed from the parent

@Component({
  selector: 'app-expense-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatSelectModule
  ],
  template: `
    <h2 mat-dialog-title>{{ data ? 'Edit Expense' : 'New Expense' }}</h2>

    <form [formGroup]="expenseForm" (ngSubmit)="onSubmit()" class="expense-form">
      
      <mat-form-field appearance="outline" class="full-width">
        <mat-label>Date</mat-label>
        <input matInput [matDatepicker]="picker" formControlName="date" />
        <mat-datepicker-toggle matSuffix [for]="picker"></mat-datepicker-toggle>
        <mat-datepicker #picker></mat-datepicker>
      </mat-form-field>

      <mat-form-field appearance="outline" class="full-width">
        <mat-label>Category</mat-label>
        <mat-select formControlName="category">
          <mat-option *ngFor="let category of categories" [value]="category">
            {{ category }}
          </mat-option>
        </mat-select>
      </mat-form-field>

      <mat-form-field appearance="outline" class="full-width">
        <mat-label>Vendor</mat-label>
        <mat-select formControlName="vendor">
          <mat-option *ngFor="let vendor of vendors" [value]="vendor">
            {{ vendor }}
          </mat-option>
        </mat-select>
      </mat-form-field>

      <mat-form-field appearance="outline" class="full-width">
        <mat-label>Payment Method</mat-label>
        <mat-select formControlName="paymentMethod">
          <mat-option *ngFor="let method of paymentMethods" [value]="method">
            {{ method }}
          </mat-option>
        </mat-select>
      </mat-form-field>

      <mat-form-field appearance="outline" class="full-width">
        <mat-label>Amount</mat-label>
        <input matInput type="number" formControlName="amount" />
      </mat-form-field>

      <mat-form-field appearance="outline" class="full-width">
        <mat-label>Description</mat-label>
        <input matInput formControlName="description" />
      </mat-form-field>

      <div class="actions">
        <button mat-button mat-dialog-close>Cancel</button>
        <button mat-flat-button color="primary" type="submit" [disabled]="expenseForm.invalid">
          <mat-icon>save</mat-icon> {{ data ? 'Update' : 'Add' }}
        </button>
      </div>
    </form>
  `,
  styles: `
    .expense-form {
      display: flex;
      flex-direction: column;
      gap: 1rem;
      padding: 1rem;
    }

    .full-width {
      width: 100%;
    }

    .actions {
      display: flex;
      justify-content: flex-end;
      gap: 0.5rem;
    }
  `
})
export class ExpenseFormComponent {
  expenseForm: FormGroup;

  //make these come from a sVC from a parent 

   categories = ['Utilities', 'Groceries', 'Rent', 'Supplies'];
  vendors = ['Walmart', 'Amazon', 'Costco', 'Local Vendor'];
  paymentMethods = ['Cash', 'Credit Card', 'Debit', 'e-Transfer'];

   //dialogRef is a reference to the currently open doalogbox so we can use the open and close and send data 
   //MATDOALONG is how we pass data from the parent to the dsoalog 
  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<ExpenseFormComponent>,
      @Inject(MAT_DIALOG_DATA) public data: IExpenseRead | null
  ) {
    this.expenseForm = this.fb.group({
      date: [data?.date ?? new Date(), Validators.required],
      category: [data?.category ?? '', Validators.required],
      vendor: [data?.vendor ?? '', Validators.required],
      amount: [data?.amount ?? '', [Validators.required, Validators.min(0.01)]],
      paymentMethod: [data?.paymentMethod ?? '', Validators.required],
      description: [data?.description ?? '']
    });
  }
    
  onSubmit() {
    if (this.expenseForm.valid) {
      this.dialogRef.close(this.expenseForm.value);
    }
  }

}
