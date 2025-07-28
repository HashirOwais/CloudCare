"use client";
import { useState } from "react";
import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { SquarePen, Trash2, Eye, MoreVertical } from "lucide-react";
import { ResponsiveDialog } from "@/components/ui/responsive-dialog";
import DeleteForm from "@/features/Expenses/DeleteForm"; // create this

import type { ReadExpenseDto } from "@/models/expense.model";
import ExpenseForm from "@/features/Expenses/ExpenseForm";

export function ExpenseRowActions({ expense }: { expense: ReadExpenseDto }) {
  const [isFormOpen, setIsFormOpen] = useState(false);
  const [isDeleteOpen, setIsDeleteOpen] = useState(false);
  const [isViewOpen, setIsViewOpen] = useState(false);

  function formatDate(date: string) {
  return new Date(date).toLocaleDateString("en-US", {
    day: "numeric",
    month: "short",
    year: "numeric",
  });
}

  return (
    <>
      {/* Edit */}
      <ResponsiveDialog
        isOpen={isFormOpen}
        setIsOpen={setIsFormOpen}
        title="Edit Expense"
      >
        <ExpenseForm
          expense={expense}
          setIsOpen={setIsFormOpen}
          isAddExpense={false}
        />
      </ResponsiveDialog>
      {/* Delete */}
      <ResponsiveDialog
        isOpen={isDeleteOpen}
        setIsOpen={setIsDeleteOpen}
        title="Delete Expense"
        description="Are you sure you want to delete this expense?"
      >
        <DeleteForm expense={expense} setIsOpen={setIsDeleteOpen} />
      </ResponsiveDialog>
      {/* View */}
<ResponsiveDialog
  isOpen={isViewOpen}
  setIsOpen={setIsViewOpen}
  title="Expense Details"
>
  <div className="space-y-2 px-2 py-2">
    <div>
      <div className="text-lg font-semibold">{expense.description}</div>
      <div className="text-muted-foreground">{expense.category}</div>
    </div>
    <div>
      <span className="font-bold">Amount:</span> ${Number(expense.amount).toFixed(2)}
    </div>
    <div>
      <span className="font-bold">Date:</span> {formatDate(expense.date)}
    </div>
    <div>
      <span className="font-bold">Vendor:</span> {expense.vendor || "N/A"}
    </div>
    <div>
      <span className="font-bold">Payment Method:</span> {expense.paymentMethod || "N/A"}
    </div>
    <div>
      <span className="font-bold">Recurring:</span> {expense.isRecurring ? "Yes" : "No"}
    </div>
    {expense.notes && (
      <div>
        <span className="font-bold">Notes:</span> {expense.notes}
      </div>
    )}
  </div>
</ResponsiveDialog>


      <DropdownMenu>
        <DropdownMenuTrigger asChild>
          <Button variant="ghost" className="h-8 w-8 p-0">
            <span className="sr-only">Open menu</span>
            <MoreVertical className="h-4 w-4" />
          </Button>
        </DropdownMenuTrigger>
        <DropdownMenuContent align="end" className="w-[160px] z-50">
          <DropdownMenuItem
            onClick={() => setIsViewOpen(true)}
            className="flex items-center gap-2"
          >
            <Eye className="h-4 w-4" />
            View
          </DropdownMenuItem>
          <DropdownMenuItem
            onClick={() => setIsFormOpen(true)}
            className="flex items-center gap-2"
          >
            <SquarePen className="h-4 w-4" />
            Edit
          </DropdownMenuItem>
          <DropdownMenuSeparator />
          <DropdownMenuItem
            onClick={() => setIsDeleteOpen(true)}
            className="flex items-center gap-2 text-red-500"
          >
            <Trash2 className="h-4 w-4" />
            Delete
          </DropdownMenuItem>
        </DropdownMenuContent>
      </DropdownMenu>
    </>
  );
}
