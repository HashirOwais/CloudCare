import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import NotFound from "./pages/NotFound.tsx";
import DashboardLayout from "./layout/DashboardLayout.tsx";
import Dashboard from "./pages/Dashboard.tsx";
import TimeTracking from "./pages/TimeTracking.tsx";
import Invoices from "./pages/Invoices.tsx";
import Settings from "./pages/Settings.tsx";
import Login from "./pages/Login.tsx";
import UserRegistration from "./pages/UserRegistration.tsx";
import Expenses from "./features/Expenses/Expenses.tsx";
import Attendance from "./features/Attendance/Attendance.tsx";
import { ExpenseService } from "./services/expense.service.ts";
import type { ReadExpenseDto } from "./models/expense.model.ts";
import AppErrorBoundary from "./pages/AppErrorBoundry.tsx";
import { Toaster } from "sonner";

//LOADER FUNCTIONS
export async function expensesLoader(): Promise<{
  expenses: ReadExpenseDto[];
}> {
  const expenses = await ExpenseService.getAll();
  return { expenses }; // always return an object, not just the array
}

//

const router = createBrowserRouter([
  {
    path: "/",
    element: <DashboardLayout />,
    errorElement: <AppErrorBoundary />,
    children: [
      {
        path: "/",
        element: <Dashboard />,
      },
      {
        path: "/expenses",
        element: <Expenses />,
        loader: expensesLoader,
      },
      {
        path: "/time-tracking",
        element: <TimeTracking />,
      },
      {
        path: "/attendance",
        element: <Attendance />,
      },
      {
        path: "/invoices",
        element: <Invoices />,
      },
      {
        path: "/settings",
        element: <Settings />,
      },
    ],
  },
  {
    path: "/login",
    element: <Login />,
  },
  {
    path: "/register",
    element: <UserRegistration />,
  },
  {
    path: "*",
    element: <NotFound />,
  },
]);

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <Toaster richColors />
    <RouterProvider router={router} />
  </StrictMode>
);
