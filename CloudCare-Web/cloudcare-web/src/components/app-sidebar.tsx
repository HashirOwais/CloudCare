import * as React from "react";
import { useLocation, NavLink } from "react-router-dom";
import {
  Sidebar,
  SidebarContent,
  SidebarGroup,
  SidebarGroupContent,
  SidebarGroupLabel,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
  SidebarRail,
  SidebarInput,
  SidebarProvider,
  SidebarTrigger,
  SidebarInset,
} from "./ui/sidebar";
import { Avatar, AvatarFallback, AvatarImage } from "./ui/avatar";
import { Separator } from "./ui/separator";
import { LogOut, User, Home, Clock, FileText, BarChart, Calendar } from "lucide-react";

const navItems = [
  { to: "/finances", label: "Finances", icon: <Home size={20} /> },
  { to: "/time-tracking", label: "Time Tracking", icon: <Clock size={20} /> },
  { to: "/invoices", label: "Invoices", icon: <FileText size={20} /> },
  { to: "/reporting", label: "Reporting", icon: <BarChart size={20} /> },
  { to: "/attendance", label: "Attendance", icon: <Calendar size={20} /> },
];

export function AppSidebar() {
  const location = useLocation();
  // Placeholder user data; replace with Auth0 user data when available
  const user = {
    name: "Jane Doe",
    daycare: "Happy Kids Daycare",
    avatar: "/avatar.png",
  };
  return (
    <Sidebar>
      <SidebarHeader>
        <div className="flex items-center gap-2 px-4 py-4">
          <span className="font-bold text-lg">CloudCare</span>
        </div>
      </SidebarHeader>
      <SidebarContent>
        <SidebarGroup>
          <SidebarGroupLabel>Main</SidebarGroupLabel>
          <SidebarGroupContent>
            <SidebarMenu>
              {navItems.map((item) => (
                <SidebarMenuItem key={item.to}>
                  <SidebarMenuButton asChild isActive={location.pathname === item.to}>
                    <NavLink to={item.to} className="flex items-center gap-2">
                      {item.icon}
                      {item.label}
                    </NavLink>
                  </SidebarMenuButton>
                </SidebarMenuItem>
              ))}
            </SidebarMenu>
          </SidebarGroupContent>
        </SidebarGroup>
        <Separator className="my-2" />
        <SidebarGroup>
          <SidebarGroupLabel>Account</SidebarGroupLabel>
          <SidebarGroupContent>
            <SidebarMenu>
              <SidebarMenuItem>
                <SidebarMenuButton asChild isActive={location.pathname === "/profile"}>
                  <NavLink to="/profile" className="flex items-center gap-2">
                    <User size={20} /> Profile
                  </NavLink>
                </SidebarMenuButton>
              </SidebarMenuItem>
              <SidebarMenuItem>
                <SidebarMenuButton asChild>
                  <button className="flex items-center gap-2 w-full text-left">
                    <LogOut size={20} /> Logout
                  </button>
                </SidebarMenuButton>
              </SidebarMenuItem>
            </SidebarMenu>
          </SidebarGroupContent>
        </SidebarGroup>
      </SidebarContent>
      <SidebarRail />
      {/* User profile at the bottom */}
      <div className="flex flex-col items-center gap-2 px-4 py-4 mt-auto border-t">
        <Avatar className="h-14 w-14">
          <AvatarImage src={user.avatar} alt={user.name} />
          <AvatarFallback>{user.name.split(' ').map(n => n[0]).join('')}</AvatarFallback>
        </Avatar>
        <div className="text-base font-semibold mt-2">{user.name}</div>
        <div className="text-xs text-muted-foreground">{user.daycare}</div>
      </div>
    </Sidebar>
  );
} 