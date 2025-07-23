import { NavLink, useLocation } from 'react-router-dom';
import { Home, Clock, FileText, BarChart, Calendar, User, LogOut } from 'lucide-react';

const navItems = [
  { to: '/finances', label: 'Finances', icon: <Home size={20} /> },
  { to: '/time-tracking', label: 'Time Tracking', icon: <Clock size={20} /> },
  { to: '/invoices', label: 'Invoices', icon: <FileText size={20} /> },
  { to: '/reporting', label: 'Reporting', icon: <BarChart size={20} /> },
  { to: '/attendance', label: 'Attendance', icon: <Calendar size={20} /> },
];

export function SideNav() {
  const location = useLocation();
  return (
    <nav className="flex flex-col justify-between h-full w-64 bg-background border-r">
      <div>
        <div className="p-6 text-2xl font-bold tracking-tight">CloudCare</div>
        <ul className="space-y-2">
          {navItems.map((item) => (
            <li key={item.to}>
              <NavLink
                to={item.to}
                className={({ isActive }) =>
                  `flex items-center px-6 py-3 text-base font-medium rounded-lg transition-colors ${
                    isActive || location.pathname === item.to
                      ? 'bg-muted text-primary'
                      : 'text-muted-foreground hover:bg-muted hover:text-primary'
                  }`
                }
              >
                <span className="mr-3">{item.icon}</span>
                {item.label}
              </NavLink>
            </li>
          ))}
        </ul>
      </div>
      <div className="mb-6">
        <ul className="space-y-2">
          <li>
            <NavLink
              to="/profile"
              className={({ isActive }) =>
                `flex items-center px-6 py-3 text-base font-medium rounded-lg transition-colors ${
                  isActive || location.pathname === '/profile'
                    ? 'bg-muted text-primary'
                    : 'text-muted-foreground hover:bg-muted hover:text-primary'
                }`
              }
            >
              <span className="mr-3"><User size={20} /></span>
              Profile
            </NavLink>
          </li>
          <li>
            <button className="flex items-center px-6 py-3 text-base font-medium rounded-lg text-muted-foreground hover:bg-muted hover:text-primary w-full">
              <span className="mr-3"><LogOut size={20} /></span>
              Logout
            </button>
          </li>
        </ul>
      </div>
    </nav>
  );
} 