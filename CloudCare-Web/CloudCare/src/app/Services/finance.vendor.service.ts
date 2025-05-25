// ------------------ vendor.service.ts ------------------
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { Vendor } from '../Models/finance.vendor.model';


@Injectable({ providedIn: 'root' })
export class VendorService {
  private vendors: Vendor[] = [];

  constructor(private http: HttpClient) {}

  load(): Observable<Vendor[]> {
    return this.http.get<Vendor[]>('/api/vendors').pipe(
      tap(res => this.vendors = res)
    );
  }

  getAll(): Vendor[] {
    return this.vendors;
  }

  getNameById(id: number): string {
    return this.vendors.find(v => v.id === id)?.name ?? 'Unknown';
  }
}
