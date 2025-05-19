// src/app/models/expense-base.model.ts
/**
 * Shared fields for creating/updating and reading an expense.
 */
export interface IExpenseBase {
  /** Optional description of the expense. */
  description?: string;
  /** Required: must be > 0 */
  amount: number;
  /** Required ISO date string, e.g. "2025-05-18T00:00:00Z" */
  date: string;
  /** Is this a recurring expense? */
  isRecurring: boolean;
  /** Required: ID of the selected category */
  categoryId: number;
  /** Required: ID of the selected vendor */
  vendorId: number;
  /** Required: ID of the selected payment method */
  paymentMethodId: number;
  /** Optional notes, max ~500 chars */
  notes?: string;
  /** Optional receipt URL */
  receiptUrl?: string;
}





