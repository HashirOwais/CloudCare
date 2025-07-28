import {type PaymentMethod } from './../models/expense.model';

const API_BASE = import.meta.env.VITE_API_URL;


export class paymentMethodService {

    static async getAll() : Promise<PaymentMethod[]>
    {
    const res = await fetch(`${API_BASE}/api/paymentmethods`);
    if (!res.ok) {
      const text = await res.text();
      throw new Error(`Failed to fetch expenses: ${res.status} ${res.statusText} - ${text}`);
    }
    return res.json();
  }
}