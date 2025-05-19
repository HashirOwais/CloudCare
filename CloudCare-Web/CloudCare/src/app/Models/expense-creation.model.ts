// src/app/models/expense-creation.model.ts
import { IExpenseBase } from './expense-base.model';

/**
 * Payload for creating a new expense (maps to ExpenseForCreationDto).
 */
export interface IExpenseCreation extends IExpenseBase {}
