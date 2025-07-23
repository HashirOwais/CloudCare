// ------------------ payment-method.service.ts ------------------
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { PaymentMethod } from '../Models/finance.payment-method.model';


@Injectable({ providedIn: 'root' })
export class PaymentMethodService {
  private methods: PaymentMethod[] = [];

  constructor(private http: HttpClient) {}

  load(): Observable<PaymentMethod[]> {
    return this.http.get<PaymentMethod[]>('/api/paymentmethods').pipe(
      tap(res => this.methods = res)
    );
  }

  getAll(): PaymentMethod[] {
    return this.methods;
  }

  getNameById(id: number): string {
    return this.methods.find(p => p.id === id)?.name ?? 'Unknown';
  }
}
