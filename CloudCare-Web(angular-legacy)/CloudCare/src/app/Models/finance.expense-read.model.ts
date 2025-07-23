// src/app/models/expense-read.model.ts
import { IExpenseBase } from './finance.expense-base.model';

/**
 * What the frontend receives when fetching expenses (maps to ReadExpenseDto).
 */
export interface IExpenseRead extends IExpenseBase {
  /** Unique database ID */
  id: number;
  /** Owner user ID */
  userId: number;
  /** Display names instead of just IDs */
  category: string;
  vendor: string;
  paymentMethod: string;
}