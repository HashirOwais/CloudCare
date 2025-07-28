import {type Vendor } from './../models/expense.model';

const API_BASE = import.meta.env.VITE_API_URL;


export class VendorService {

    static async getAll() : Promise<Vendor[]>
    {
    const res = await fetch(`${API_BASE}/api/vendors`);
    if (!res.ok) {
      const text = await res.text();
      throw new Error(`Failed to fetch expenses: ${res.status} ${res.statusText} - ${text}`);
    }
    return res.json();
  }
}