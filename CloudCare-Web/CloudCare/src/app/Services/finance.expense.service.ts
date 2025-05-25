import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { IExpenseRead } from '../Models/finance.expense-read.model';
import { IExpenseCreation } from '../Models/finance.expense-creation.model';
import { IExpenseUpdate } from '../Models/finance.expense-update.model';
import { CategoryService } from './finance.category.service';
import { VendorService } from './finance.vendor.service';
import { PaymentMethodService } from './finance.payment-method.service';


@Injectable({
  providedIn: 'root'
})
export class ExpenseService {
    
    private _expenses: BehaviorSubject<IExpenseRead[]> = new BehaviorSubject<IExpenseRead[]>([]);
    public expenses$: Observable<IExpenseRead[]> = this._expenses.asObservable();
    



constructor(
  private http: HttpClient,
  private categoryService: CategoryService,
  private vendorService: VendorService,
  private paymentMethodService: PaymentMethodService
) {}

  getExpenses(): void{
    //httpClient returns observable which means we need to subscribe(), which then actually calls it
     this.http.get<IExpenseRead[]>("/api/expenses").subscribe(expense =>{
      this._expenses.next(expense);
      console.log(this._expenses.value);
     }

     );
  }
  addExpense(expense: IExpenseCreation): void {


  this.http.post<IExpenseRead>(`/api/expenses`, expense, { observe: 'response' }).subscribe({
    next: (res) => {
      if (res.status === 201 || res.status === 200) {
        const newExpense = res.body;

        if (newExpense) {
          const updated = [...this._expenses.getValue(), newExpense];
          this._expenses.next(updated);
          console.log(' Expense added:', newExpense);
        }
      } else {
        console.warn(' Unexpected response status:', res.status);
      }
    },
    error: (err) => {
      console.error(' Failed to add expense:', err);
    }
  });
}
removeExpense(expenseID: number): void {
  this.http.delete<void>(`/api/expenses/${expenseID}`, { observe: 'response' }).subscribe({
    next: (res) => {
      if (res.status === 200 || res.status === 204) {
        const updated = this._expenses.value.filter(e => e.id !== expenseID);
        this._expenses.next(updated);
        console.log(' Expense deleted on server.');
      } else {
        console.warn(' Unexpected response status:', res.status);
      }
    },
    error: (err) => {
      console.error(' Error deleting expense:', err);
    }
  });
}

//TODO: Make it where the if we are trying to update a expene and if there are no changes. dont send to the API to edit
updateExpense(expense: IExpenseUpdate): void {
  this.http.put<void>(`/api/expenses`, expense, { observe: 'response' }).subscribe({
    next: (res) => {
      if (res.status === 200 || res.status === 204) {
        debugger;
        // Step 1: Get the original expense to preserve userId
        const original = this._expenses.value.find(e => e.id === expense.id);

        // Step 2: Create updated object with names
        const updatedExpense = {
          ...expense,
          userId: original?.userId ?? 0,
          category: this.categoryService.getNameById(expense.categoryId),
          vendor: this.vendorService.getNameById(expense.vendorId),
          paymentMethod: this.paymentMethodService.getNameById(expense.paymentMethodId)
        };

        // Step 3: Remove old, add updated, and push new list
        const updatedList = [
          ...this._expenses.getValue().filter(e => e.id !== expense.id),
          updatedExpense
        ];

        this._expenses.next(updatedList);

        console.log(`✅ Expense ID ${expense.id} successfully updated.`);
      }
    },
    error: (err) => {
      console.error(`❌ Failed to update expense ID ${expense.id}:`, err);
    }
  });
}
  
}
