import type { ReadExpenseDto, ExpenseForCreationDto, ExpenseForUpdateDto } from "../models/expense.model";

export class ExpenseService {
  static async getAll(): Promise<ReadExpenseDto[]> {
    const res = await fetch("/api/expenses");
    if (!res.ok) throw new Error("Failed to fetch expenses");
    return res.json();
  }

  static async getById(id: number): Promise<ReadExpenseDto> {
    const res = await fetch(`/api/expenses/${id}`);
    if (!res.ok) throw new Error("Failed to fetch expense");
    return res.json();
  }

  static async create(expense: ExpenseForCreationDto): Promise<ReadExpenseDto> {
    const res = await fetch("/api/expenses", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(expense),
    });
    if (!res.ok) throw new Error("Failed to create expense");
    return res.json();
  }

  static async update(expense: ExpenseForUpdateDto): Promise<ReadExpenseDto> {
    const res = await fetch(`/api/expenses/${expense.id}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(expense),
    });
    if (!res.ok) throw new Error("Failed to update expense");
    return res.json();
  }

  static async delete(id: number): Promise<void> {
    const res = await fetch(`/api/expenses/${id}`, {
      method: "DELETE" });
    if (!res.ok) throw new Error("Failed to delete expense");
  }
} 