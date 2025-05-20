import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { IExpenseRead } from '../Models/expense-read.model';

@Injectable({
  providedIn: 'root'
})
export class ExpenseService {
    private http = inject(HttpClient);
    
    private _expenses: BehaviorSubject<IExpenseRead[]> = new BehaviorSubject<IExpenseRead[]>([]);
    public expenses$: Observable<IExpenseRead[]> = this._expenses.asObservable();
    



  constructor() {}

  getExpenses(): void{
    //httpClient returns observable which means we need to subscribe(), which then actually calls it
     this.http.get<IExpenseRead[]>("/api/expenses").subscribe(expense =>{
      this._expenses.next(expense);
     }

     );
  }
  
  
}
