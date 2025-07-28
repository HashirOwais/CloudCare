import { type Category } from "@/models/expense.model";

const API_BASE = import.meta.env.VITE_API_URL;


export class CategoryService {

    static async getAll() : Promise<Category[]>
    {
    const res = await fetch(`${API_BASE}/api/categories`);
    if (!res.ok) {
      const text = await res.text();
      throw new Error(`Failed to fetch expenses: ${res.status} ${res.statusText} - ${text}`);
    }
    return res.json();
  }
}