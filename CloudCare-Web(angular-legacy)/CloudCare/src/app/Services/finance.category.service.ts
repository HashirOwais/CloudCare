// ------------------ category.service.ts ------------------
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { Category } from '../Models/finance.category.model';


@Injectable({ providedIn: 'root' })
export class CategoryService {
  private categories: Category[] = [];

  constructor(private http: HttpClient) {}

  load(): Observable<Category[]> {
    return this.http.get<Category[]>('/api/categories').pipe(
      tap(res => this.categories = res)
    );
  }

  getAll(): Category[] {
    return this.categories;
  }

  getNameById(id: number): string {
    return this.categories.find(c => c.id === id)?.name ?? 'Unknown';
  }
}