import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { IExpenseRead } from '../Models/expense-read.model';

@Injectable({
  providedIn: 'root'
})
export class ExpenseService {
  private expenseSubject = new BehaviorSubject<IExpenseRead[]>([]);

  public expenses$ = this.expenseSubject.asObservable();

  constructor(private http: HttpClient) {}

  loadExpenses() {
    this.http.get<IExpenseRead[]>('http://localhost:5134/api/expenses')
      .subscribe(data => this.expenseSubject.next(data));
  }
}
