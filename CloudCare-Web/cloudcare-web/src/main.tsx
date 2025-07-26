import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import { createBrowserRouter, RouterProvider } from 'react-router-dom'
import NotFound from './pages/NotFound.tsx'
import DashboardLayout from './layout/DashboardLayout.tsx'
import Dashboard from './pages/Dashboard.tsx'
import Expenses from './pages/Expenses.tsx'
import TimeTracking from './pages/TimeTracking.tsx'
import Attendance from './pages/Attendance.tsx'
import Invoices from './pages/Invoices.tsx'
import Settings from './pages/Settings.tsx'

const router = createBrowserRouter([
  {
    path: '/',
    element: <DashboardLayout />,
    children: [
      {
        path: '/',
        element: <Dashboard />,
      },
      {
        path: '/expenses',
        element: <Expenses />,
      },
      {
        path: '/time-tracking',
        element: <TimeTracking />,
      },
      {
        path: '/attendance',
        element: <Attendance />,
      },
      {
        path: '/invoices',
        element: <Invoices />,
      },
      {
        path: '/settings',
        element: <Settings />,
      },
    ],
  },
  {
    path: '*',
    element: <NotFound />,
  },
])

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <RouterProvider router={router} />
  </StrictMode>,
)
