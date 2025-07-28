import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from "@/components/ui/form";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Textarea } from "@/components/ui/textarea";
import { Checkbox } from "@/components/ui/checkbox";
import { type Category, type PaymentMethod, type ReadExpenseDto, type Vendor } from "@/models/expense.model";
import { ExpenseService } from "@/services/expense.service";
import { toast } from "sonner";
import { useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import { VendorService } from "@/services/vendor.service";
import { CategoryService } from "@/services/Category.service";
import { paymentMethodService } from "@/services/PaymentMethod.service";


const schema = z.object({
  description: z.string().min(1, "Description is required"),
  amount: z.number().min(0.01, "Amount must be greater than 0"),
  date: z.string().min(1, "Date is required"),
  isRecurring: z.boolean(),
  categoryId: z.number().min(1, "Category is required"),
  vendorId: z.number().min(1, "Vendor is required"),
  paymentMethodId: z.number().min(1, "Payment method is required"),
  notes: z.string().optional(),
});

type ExpenseFormValues = z.infer<typeof schema>;

export default function ExpenseForm({
  isAddExpense,
  expense,
  setIsOpen,
}: 
{
  isAddExpense: boolean;
  expense: ReadExpenseDto;
  setIsOpen: (v: boolean) => void;
}) 

{

  
  const form = useForm<ExpenseFormValues>({
    resolver: zodResolver(schema),
    mode: "onChange", // Live validation feedback!
    defaultValues: {
      description: expense.description ?? "",
      amount: expense.amount,
      date: expense.date.substring(0, 10),
      isRecurring: expense.isRecurring,
      categoryId: expense.categoryId,
      vendorId: expense.vendorId,
      paymentMethodId: expense.paymentMethodId,
      notes: expense.notes ?? "",
    },
  });

  const navigate = useNavigate();

  const onSubmit = async (data: ExpenseFormValues) => {
    try {
      if (isAddExpense) {
        await ExpenseService.create(data);
        toast.success("Expense added!");
      } else {
        await ExpenseService.update({ ...data, id: expense.id });
        toast.success("Expense updated!");
      }
      setIsOpen(false);
      navigate("/expenses");
    } catch (err) {
      toast.error("Something went wrong. Please try again.");
      console.error(err);
    }
  };

  // Vendors
  const [vendors, setVendors] = useState<Vendor[] | null>(null)
  const [vendorsLoading, setVendorsLoading] = useState(true);
  const [vendorsError, setVendorsError] = useState<string | null>(null);

  // Payment Methods
  const [paymentMethods, setPaymentMethods] = useState<PaymentMethod[] | null> (null);
  const [paymentMethodsLoading, setPaymentMethodsLoading] = useState(true);
  const [paymentMethodsError, setPaymentMethodsError] = useState<string | null>(null);

  // Categories
  const [categories, setCategories] = useState<Category[] | null>(null);
  const [categoriesLoading, setCategoriesLoading] = useState(true);
  const [categoriesError, setCategoriesError] = useState<string | null>(null);

  // Load Vendors
  useEffect(() => {
    VendorService.getAll()
      .then(setVendors)
      .catch(() => setVendorsError("Failed to load vendors"))
      .finally(() => setVendorsLoading(false));
  }, []);

  // Load Payment Methods
  useEffect(() => {
    paymentMethodService.getAll()
      .then(setPaymentMethods)
      .catch(() => setPaymentMethodsError("Failed to load payment methods"))
      .finally(() => setPaymentMethodsLoading(false));
  }, []);

  // Load Categories
  useEffect(() => {
    CategoryService.getAll()
      .then(setCategories)
      .catch(() => setCategoriesError("Failed to load categories"))
      .finally(() => setCategoriesLoading(false));
  }, []);

  if (vendorsLoading || paymentMethodsLoading || categoriesLoading) {
    return <div>Loading…</div>;
  }

  // Optionally, display errors
  if (vendorsError || paymentMethodsError || categoriesError) {
    return (
      <div>
        {vendorsError && <div className="text-red-500">{vendorsError}</div>}
        {paymentMethodsError && <div className="text-red-500">{paymentMethodsError}</div>}
        {categoriesError && <div className="text-red-500">{categoriesError}</div>}
      </div>
    );
  }


  return (
    <div className="overflow-y-auto max-h-[70vh] p-4">
      <Form {...form}>
        <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4 px-2 sm:px-4">
          <FormField
            name="description"
            control={form.control}
            render={({ field }) => (
              <FormItem>
                <FormLabel>Description</FormLabel>
                <FormControl>
                  <Input {...field} placeholder="Description" />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />

          {/* Amount and Date on same row for md+, stacked on mobile */}
          <div className="flex flex-col md:flex-row gap-4">
            <FormField
              name="amount"
              control={form.control}
              render={({ field }) => (
                <FormItem className="flex-1">
                  <FormLabel>Amount</FormLabel>
                  <FormControl>
                    <Input
                      {...field}
                      type="number"
                      step="0.01"
                      min="0"
                      placeholder="Amount"
                      value={field.value}
                      onChange={e => field.onChange(parseFloat(e.target.value))}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              name="date"
              control={form.control}
              render={({ field }) => (
                <FormItem className="flex-1">
                  <FormLabel>Date</FormLabel>
                  <FormControl>
                    <Input {...field} type="date" />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
          </div>

          {/* Recurring Checkbox */}
          <FormField
            name="isRecurring"
            control={form.control}
            render={({ field }) => (
              <FormItem className="flex flex-row items-center gap-3">
                <FormControl>
                  <Checkbox
                    checked={field.value}
                    onCheckedChange={field.onChange}
                    id="isRecurring"
                  />
                </FormControl>
                <FormLabel htmlFor="isRecurring" className="mb-0 cursor-pointer">Recurring</FormLabel>
                <FormMessage />
              </FormItem>
            )}
          />

          {/* Category, Vendor, Payment - ALWAYS vertical for dialog */}
          <div className="flex flex-col gap-4">
            <FormField
              name="categoryId"
              control={form.control}
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Category</FormLabel>
                  <Select
                    value={String(field.value)}
                    onValueChange={val => field.onChange(Number(val))}
                  >
                    <FormControl>
                      <SelectTrigger className={`w-full ${form.formState.errors.categoryId ? "border-red-500" : ""}`}>
                        <SelectValue placeholder="Select category" />
                      </SelectTrigger>
                    </FormControl>
                    <SelectContent>
                      {categories?.map(cat => (
                        <SelectItem key={cat.id} value={String(cat.id)}>
                          {cat.name}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              name="vendorId"
              control={form.control}
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Vendor</FormLabel>
                  <Select
                    value={String(field.value)}
                    onValueChange={val => field.onChange(Number(val))}
                  >
                    <FormControl>
                      <SelectTrigger className={`w-full ${form.formState.errors.vendorId ? "border-red-500" : ""}`}>
                        <SelectValue placeholder="Select vendor" />
                      </SelectTrigger>
                    </FormControl>
                    <SelectContent>
                      {vendors?.map(v => (
                        <SelectItem key={v.id} value={String(v.id)}>
                          {v.name}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              name="paymentMethodId"
              control={form.control}
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Payment Method</FormLabel>
                  <Select
                    value={String(field.value)}
                    onValueChange={val => field.onChange(Number(val))}
                  >
                    <FormControl>
                      <SelectTrigger className={`w-full ${form.formState.errors.paymentMethodId ? "border-red-500" : ""}`}>
                        <SelectValue placeholder="Select payment method" />
                      </SelectTrigger>
                    </FormControl>
                    <SelectContent>
                      {paymentMethods?.map(p => (
                        <SelectItem key={p.id} value={String(p.id)}>
                          {p.name}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                  <FormMessage />
                </FormItem>
              )}
            />
          </div>

          <FormField
            name="notes"
            control={form.control}
            render={({ field }) => (
              <FormItem>
                <FormLabel>Notes</FormLabel>
                <FormControl>
                  <Textarea {...field} placeholder="Notes (optional)" />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />

          <Button
            type="submit"
            className="w-full"
            disabled={!form.formState.isValid || form.formState.isSubmitting}
          >
            Save
          </Button>
        </form>
      </Form>
    </div>
  );
}
