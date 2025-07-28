import type {
  ReadExpenseDto,
  ExpenseForCreationDto,
  ExpenseForUpdateDto
} from "../models/expense.model";

const API_BASE = import.meta.env.VITE_API_URL;

export class ExpenseService {
  static async getAll(): Promise<ReadExpenseDto[]> {
    const res = await fetch(`${API_BASE}/api/expenses`);
    if (!res.ok) {
      const text = await res.text();
      throw new Error(`Failed to fetch expenses: ${res.status} ${res.statusText} - ${text}`);
    }
    return res.json();
  }

  static async getById(id: number): Promise<ReadExpenseDto> {
    const res = await fetch(`${API_BASE}/api/expenses/${id}`);
    if (!res.ok) {
      const text = await res.text();
      throw new Error(`Failed to fetch expense: ${res.status} ${res.statusText} - ${text}`);
    }
    return res.json();
  }

  static async create(expense: ExpenseForCreationDto): Promise<ReadExpenseDto> {
    const res = await fetch(`${API_BASE}/api/expenses`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(expense),
    });
    if (!res.ok) {
      const text = await res.text();
      throw new Error(`Failed to create expense: ${res.status} ${res.statusText} - ${text}`);
    }
    return res.json();
  }

  static async update(expense: ExpenseForUpdateDto): Promise<void> {
    const res = await fetch(`${API_BASE}/api/expenses/${expense.id}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(expense),
    });
    if (!res.ok) {
      const text = await res.text();
      throw new Error(`Failed to update expense: ${res.status} ${res.statusText} - ${text}`);
    }
  
  }

  static async delete(id: number): Promise<void> {
    const res = await fetch(`${API_BASE}/api/expenses/${id}`, {
      method: "DELETE"
    });
    if (!res.ok) {
      const text = await res.text();
      throw new Error(`Failed to delete expense: ${res.status} ${res.statusText} - ${text}`);
    }
  }
}
