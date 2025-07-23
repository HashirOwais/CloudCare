import type { User } from "../models/user.model";

export class UserService {
  static async getCurrentUser(): Promise<User> {
    const res = await fetch("/api/users/me");
    if (!res.ok) throw new Error("Failed to fetch user");
    return res.json();
  }

  static async updateUser(user: Partial<User>): Promise<User> {
    const res = await fetch("/api/users/me", {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(user),
    });
    if (!res.ok) throw new Error("Failed to update user");
    return res.json();
  }
} 