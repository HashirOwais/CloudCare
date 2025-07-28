import { useState } from 'react'
import ExpenseForm from './ExpenseForm'
import type { ReadExpenseDto } from '@/models/expense.model';

const AddExpenses = () => {
  const EMPTY_EXPENSE: ReadExpenseDto = {
    id: 0,
    userId: 0,
    description: "",
    amount: 0,
    date: new Date().toISOString().substring(0, 10),
    isRecurring: false,
    category: "",
    vendor: "",
    paymentMethod: "",
    categoryId: 0,
    vendorId: 0,
    paymentMethodId: 0,
    notes: "",
    receiptUrl: ""
  };

  const [, setIsFormOpen] = useState(false);

  return (
    <div className="flex justify-center w-full min-h-[50vh]">
      <div className="w-full max-w-2xl px-2 sm:px-4">
        <div className="rounded-2xl shadow-lg bg-background p-6 border">
          {/* Your title/heading, optional */}
          <h2 className="text-2xl font-semibold mb-4">Add Expense</h2>
          <ExpenseForm expense={EMPTY_EXPENSE} setIsOpen={setIsFormOpen} isAddExpense={true} />
        </div>
      </div>
    </div>
  );
}

export default AddExpenses
