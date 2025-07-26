import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { useState } from "react";

export function NewUserForm() {
  const [name, setName] = useState("");
  const [daycare, setDaycare] = useState("");
  const [submitting, setSubmitting] = useState(false);

  // Replace with your actual submit logic
  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    setSubmitting(true);
    // ...submit to API...
    setTimeout(() => setSubmitting(false), 1000);
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-muted">
      <Card className="w-full max-w-sm">
        <CardHeader>
          <CardTitle>Complete Your Profile</CardTitle>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSubmit} className="space-y-4">
            <Input
              placeholder="Your Name"
              value={name}
              onChange={e => setName(e.target.value)}
              required
            />
            <Input
              placeholder="Daycare Name"
              value={daycare}
              onChange={e => setDaycare(e.target.value)}
              required
            />
            <Button className="w-full" type="submit" disabled={submitting}>
              {submitting ? "Saving..." : "Save"}
            </Button>
          </form>
        </CardContent>
      </Card>
    </div>
  );
}