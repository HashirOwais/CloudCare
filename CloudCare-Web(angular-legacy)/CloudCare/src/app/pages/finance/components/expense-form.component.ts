import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { IExpenseRead } from '../../../Models/finance.expense-read.model';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { Category } from '../../../Models/finance.category.model';
import { Vendor } from '../../../Models/finance.vendor.model';
import { PaymentMethod } from '../../../Models/finance.payment-method.model';
import { CategoryService } from '../../../Services/finance.category.service';
import { VendorService } from '../../../Services/finance.vendor.service';
import { PaymentMethodService } from '../../../Services/finance.payment-method.service';


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
    MatSelectModule,
    MatCheckboxModule
  ],
  template: `
    <h2 mat-dialog-title>{{ data ? 'Edit Expense' : 'New Expense' }}</h2>

    <form [formGroup]="expenseForm" (ngSubmit)="onSubmit()" class="expense-form">

      <!-- Date -->
      <mat-form-field appearance="outline" class="full-width">
        <mat-label>Date</mat-label>
        <input matInput [matDatepicker]="picker" formControlName="date" />
        <mat-datepicker-toggle matSuffix [for]="picker"></mat-datepicker-toggle>
        <mat-datepicker #picker></mat-datepicker>
      </mat-form-field>

      <!-- Category -->
      <mat-form-field appearance="outline" class="full-width">
        <mat-label>Category</mat-label>
        <mat-select formControlName="categoryId">
          <mat-option *ngFor="let category of categories" [value]="category.id">
            {{ category.name }}
          </mat-option>
        </mat-select>
      </mat-form-field>

      <!-- Vendor -->
      <mat-form-field appearance="outline" class="full-width">
        <mat-label>Vendor</mat-label>
        <mat-select formControlName="vendorId">
          <mat-option *ngFor="let vendor of vendors" [value]="vendor.id">
            {{ vendor.name }}
          </mat-option>
        </mat-select>
      </mat-form-field>

      <!-- Payment Method -->
      <mat-form-field appearance="outline" class="full-width">
        <mat-label>Payment Method</mat-label>
        <mat-select formControlName="paymentMethodId">
          <mat-option *ngFor="let method of paymentMethods" [value]="method.id">
            {{ method.name }}
          </mat-option>
        </mat-select>
      </mat-form-field>

      <!-- Amount -->
      <mat-form-field appearance="outline" class="full-width">
        <mat-label>Amount</mat-label>
        <input matInput type="number" formControlName="amount" />
      </mat-form-field>

      <!-- Description -->
      <mat-form-field appearance="outline" class="full-width">
        <mat-label>Description</mat-label>
        <input matInput formControlName="description" />
      </mat-form-field>

      <div class="full-width mat-body-1" style="margin-bottom: 16px;">
        <mat-checkbox formControlName="isRecurring" color="primary">
          Recurring Expense
        </mat-checkbox>
      </div>

      <!-- Actions -->
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
export class ExpenseFormComponent implements OnInit {
  expenseForm: FormGroup;
  categories: Category[] = [];
  vendors: Vendor[] = [];
  paymentMethods: PaymentMethod[] = [];



   //dialogRef is a reference to the currently open doalogbox so we can use the open and close and send data 
   //MATDOALONG is how we pass data from the parent to the dsoalog 
  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<ExpenseFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: IExpenseRead | null,
    private categoryService: CategoryService,
    private vendorService: VendorService,
    private paymentMethodService: PaymentMethodService
  ) {
    this.expenseForm = this.fb.group({
      id: [data?.id ?? null],
      date: [data?.date ?? new Date(), Validators.required],
      categoryId: [data?.categoryId ?? null, Validators.required],
      vendorId: [data?.vendorId ?? null, Validators.required],
      paymentMethodId: [data?.paymentMethodId ?? null, Validators.required],
      amount: [data?.amount ?? '', [Validators.required, Validators.min(0.01)]],
      description: [data?.description ?? ''],
      isRecurring: [data?.isRecurring ?? false],
    });
  }

  ngOnInit(): void {
    this.categoryService.load().subscribe(res => this.categories = res);
    this.vendorService.load().subscribe(res => this.vendors = res);
    this.paymentMethodService.load().subscribe(res => this.paymentMethods = res);
  }

  onSubmit() {
    if (this.expenseForm.valid) {
      this.dialogRef.close(this.expenseForm.value);
    }
  }
}

