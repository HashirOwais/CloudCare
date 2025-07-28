import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import type { ReadExpenseDto } from "@/models/expense.model";

import { ExpenseRowActions } from "./ExpenseRowAction";

export default function ViewAllExpenses({
  expenses,
}: {
  expenses: ReadExpenseDto[];
}) {

  return (
    <div className="overflow-x-auto w-full">
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>Description</TableHead>
            <TableHead>Amount</TableHead>
            <TableHead>Date</TableHead>
            <TableHead>Category</TableHead>
            <TableHead></TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {expenses.map((e) => (
            <TableRow
              key={e.id}
            >
              <TableCell className="max-w-[8rem] truncate">
                {e.description}
              </TableCell>
              <TableCell>
                $
                {Number(e.amount).toLocaleString(undefined, {
                  minimumFractionDigits: 2,
                })}
              </TableCell>
              <TableCell>
                {new Date(e.date).toLocaleDateString("en-US", {
                  day: "numeric",
                  month: "short",
                  year: "numeric",
                })}
              </TableCell>
              <TableCell>{e.category}</TableCell>
              <TableCell>
                <ExpenseRowActions expense={e} />
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </div>
  );
}
