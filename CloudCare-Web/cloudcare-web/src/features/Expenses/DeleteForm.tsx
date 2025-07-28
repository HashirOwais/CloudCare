import { Button } from "@/components/ui/button";
import type { ReadExpenseDto } from "@/models/expense.model";
import { ExpenseService } from "@/services/expense.service";
import { useNavigate } from "react-router-dom";

export default function DeleteForm({
  expense,
  setIsOpen,
}: {
  expense: ReadExpenseDto;
  setIsOpen: (v: boolean) => void;
}) 
{
  const navigate = useNavigate()
const onDelete = async () => {
  try {
    await ExpenseService.delete(expense.id);
    setIsOpen(false);
    navigate('/expenses')
  } catch (err) {
    // Handle error (show toast or error message if you want)
    console.error("Failed to delete expense:", err);
  }
};


  return (
    <div className="flex flex-col gap-4 p-4">
      <div>
        Are you sure you want to delete the expense <b>{expense.description}</b>?
      </div>
      <div className="flex gap-2 justify-end">
        <Button variant="outline" onClick={() => setIsOpen(false)}>
          Cancel
        </Button>
        <Button variant="destructive" onClick={onDelete}>
          Delete
        </Button>
      </div>
    </div>
  );
}
