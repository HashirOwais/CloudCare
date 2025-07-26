import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { DashboardLayout } from './layouts/DashboardLayout';
import { FinancesPage } from './features/finances/pages/FinancesPage';
import { TimeTrackingPage } from './features/time-tracking/pages/TimeTrackingPage';
import { InvoicesPage } from './features/invoices/pages/InvoicesPage';
import { ReportingPage } from './features/reporting/pages/ReportingPage';
import { AttendancePage } from './features/attendance/pages/AttendancePage';
import { ProfilePage } from './features/profile/pages/ProfilePage';
import { LoginPage } from './features/auth/pages/LoginPage';
import { NewUserForm } from './features/auth/pages/NewUserForm';
import './App.css';

function App() {
  return (
    <Router>
      <Routes>
        {/* Dashboard routes (protected) */}
        <Route path="/" element={<DashboardLayout />}>
          <Route index element={<Navigate to="finances" replace />} />
          <Route path="finances" element={<FinancesPage />} />
          <Route path="time-tracking" element={<TimeTrackingPage />} />
          <Route path="invoices" element={<InvoicesPage />} />
          <Route path="reporting" element={<ReportingPage />} />
          <Route path="attendance" element={<AttendancePage />} />
          <Route path="profile" element={<ProfilePage />} />
        </Route>
        {/* Auth routes */}
        <Route path="/login" element={<LoginPage />} />
        <Route path="/NewUserForm" element={<NewUserForm />} />
      </Routes>
    </Router>
  );
}

export default App;
