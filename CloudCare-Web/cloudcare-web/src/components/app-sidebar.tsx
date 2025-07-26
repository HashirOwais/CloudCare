import * as React from "react"
import { Link } from "react-router-dom"
import { 
  Building2, 
  DollarSign, 
  Clock, 
  Users, 
  FileText,
  Home,
  Settings
} from "lucide-react"

import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
  SidebarRail,
} from "@/components/ui/sidebar"
import { NavUser } from "@/components/nav-user"

export function AppSidebar({ ...props }: React.ComponentProps<typeof Sidebar>) {
  return (
    <Sidebar collapsible="icon" variant="sidebar" className="w-64" {...props}>
      <SidebarHeader>
        <div className="flex items-center gap-2 px-2 group-data-[collapsible=icon]:justify-center">
          <div className="flex h-6 w-6 items-center justify-center rounded-lg bg-gradient-to-br from-blue-500 to-purple-600 text-white shrink-0">
            <Building2 className="h-4 w-4" />
          </div>
          <div className="flex flex-col group-data-[collapsible=icon]:hidden">
            <span className="text-sm font-semibold text-foreground">CloudCare</span>
            <span className="text-xs text-muted-foreground">Daycare Management</span>
          </div>
        </div>
      </SidebarHeader>
      <SidebarContent>
        <SidebarMenu className="space-y-1">
          <SidebarMenuItem className="group-data-[collapsible=icon]:flex group-data-[collapsible=icon]:justify-center">
            <SidebarMenuButton size="lg" asChild tooltip="Dashboard" className="group-data-[collapsible=icon]:justify-center">
              <Link to="/">
                <Home className="h-5 w-5" />
                <span className="group-data-[collapsible=icon]:hidden">Dashboard</span>
              </Link>
            </SidebarMenuButton>
          </SidebarMenuItem>
          <SidebarMenuItem className="group-data-[collapsible=icon]:flex group-data-[collapsible=icon]:justify-center">
            <SidebarMenuButton size="lg" asChild tooltip="Expenses" className="group-data-[collapsible=icon]:justify-center">
              <Link to="/expenses">
                <DollarSign className="h-5 w-5" />
                <span className="group-data-[collapsible=icon]:hidden">Expenses</span>
              </Link>
            </SidebarMenuButton>
          </SidebarMenuItem>
          <SidebarMenuItem className="group-data-[collapsible=icon]:flex group-data-[collapsible=icon]:justify-center">
            <SidebarMenuButton size="lg" asChild tooltip="Time Tracking" className="group-data-[collapsible=icon]:justify-center">
              <Link to="/time-tracking">
                <Clock className="h-5 w-5" />
                <span className="group-data-[collapsible=icon]:hidden">Time Tracking</span>
              </Link>
            </SidebarMenuButton>
          </SidebarMenuItem>
          <SidebarMenuItem className="group-data-[collapsible=icon]:flex group-data-[collapsible=icon]:justify-center">
            <SidebarMenuButton size="lg" asChild tooltip="Attendance" className="group-data-[collapsible=icon]:justify-center">
              <Link to="/attendance">
                <Users className="h-5 w-5" />
                <span className="group-data-[collapsible=icon]:hidden">Attendance</span>
              </Link>
            </SidebarMenuButton>
          </SidebarMenuItem>
          <SidebarMenuItem className="group-data-[collapsible=icon]:flex group-data-[collapsible=icon]:justify-center">
            <SidebarMenuButton size="lg" asChild tooltip="Invoices" className="group-data-[collapsible=icon]:justify-center">
              <Link to="/invoices">
                <FileText className="h-5 w-5" />
                <span className="group-data-[collapsible=icon]:hidden">Invoices</span>
              </Link>
            </SidebarMenuButton>
          </SidebarMenuItem>
          <SidebarMenuItem className="group-data-[collapsible=icon]:flex group-data-[collapsible=icon]:justify-center">
            <SidebarMenuButton size="lg" asChild tooltip="Settings" className="group-data-[collapsible=icon]:justify-center">
              <Link to="/settings">
                <Settings className="h-5 w-5" />
                <span className="group-data-[collapsible=icon]:hidden">Settings</span>
              </Link>
            </SidebarMenuButton>
          </SidebarMenuItem>
        </SidebarMenu>
      </SidebarContent>
      <SidebarFooter>
        <NavUser user={{
          name: "Admin User",
          email: "admin@daycare.com",
          avatar: "/avatars/admin.jpg",
          daycare: "CloudCare Daycare"
        }} />
      </SidebarFooter>
      <SidebarRail />
    </Sidebar>
  )
}
