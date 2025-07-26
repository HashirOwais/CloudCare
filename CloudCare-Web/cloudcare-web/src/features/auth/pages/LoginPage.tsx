import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { useAuth0 } from "@auth0/auth0-react";

export function LoginPage() {
  const { loginWithRedirect, isLoading } = useAuth0();

  return (
    <div className="flex min-h-screen items-center justify-center bg-muted">
      <Card className="w-full max-w-sm">
        <CardHeader>
          <CardTitle>Sign in to CloudCare</CardTitle>
        </CardHeader>
        <CardContent>
          <Button
            className="w-full"
            onClick={() => loginWithRedirect()}
            disabled={isLoading}
          >
            Sign in with Google
          </Button>
        </CardContent>
      </Card>
    </div>
  );
}