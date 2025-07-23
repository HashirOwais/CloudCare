// src/app/models/expense-update.model.ts

import { IExpenseBase } from './finance.expense-base.model';

/**
 * DTO for updating an existing expense.
 * Extends the base expense model and includes the expense ID.
 */
export interface IExpenseUpdate extends IExpenseBase {
  /** The unique identifier of the expense to update */
  id: number;
}