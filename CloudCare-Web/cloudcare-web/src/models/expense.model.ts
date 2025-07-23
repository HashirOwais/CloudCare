export interface ReadExpenseDto {
  id: number;
  userId: number;
  description?: string;
  amount: number;
  date: string; // ISO string
  isRecurring: boolean;
  category?: string;
  vendor?: string;
  paymentMethod?: string;
  categoryId: number;
  vendorId: number;
  paymentMethodId: number;
  notes?: string;
  receiptUrl?: string;
}

export interface ExpenseForCreationDto {
  description?: string;
  amount: number;
  date: string; // ISO string
  isRecurring: boolean;
  categoryId: number;
  vendorId: number;
  paymentMethodId: number;
  notes?: string;
  receiptUrl?: string;
}

export interface ExpenseForUpdateDto {
  id: number;
  description?: string;
  amount: number;
  date: string; // ISO string
  isRecurring: boolean;
  categoryId: number;
  vendorId: number;
  paymentMethodId: number;
  notes?: string;
  receiptUrl?: string;
}

export interface Category {
  id: number;
  name: string;
}

export interface Vendor {
  id: number;
  name: string;
}

export interface PaymentMethod {
  id: number;
  name: string;
} 