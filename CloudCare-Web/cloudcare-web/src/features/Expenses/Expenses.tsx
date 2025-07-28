import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import ViewAllExpenses from "./ViewAllExpenses";
import AddExpenses from "./AddExpenses";
import { useLoaderData } from "react-router-dom";
import type { ReadExpenseDto } from "@/models/expense.model";

export default function Expenses() {
    const { expenses } = useLoaderData() as { expenses: ReadExpenseDto[] };
    console.log(expenses);
  return (
    <div className="flex-1 p-4 md:p-8 pt-6 w-full">
      <h1 className="text-3xl font-bold mb-6">Expenses</h1>
      <Tabs defaultValue="view" className="w-full">
        <TabsList className="w-full flex mb-6">
          <TabsTrigger
            value="view"
            className="flex-1 text-lg data-[state=active]:bg-primary data-[state=active]:text-white"
          >
            View Expenses
          </TabsTrigger>
          <TabsTrigger
            value="add"
            className="flex-1 text-lg data-[state=active]:bg-primary data-[state=active]:text-white"
          >
            Add Expense
          </TabsTrigger>
        </TabsList>
        <TabsContent value="view" className="w-full">
          <ViewAllExpenses expenses={expenses} />
        </TabsContent>
        <TabsContent value="add" className="w-full">
          <AddExpenses />
        </TabsContent>
      </Tabs>
    </div>
  );
}
