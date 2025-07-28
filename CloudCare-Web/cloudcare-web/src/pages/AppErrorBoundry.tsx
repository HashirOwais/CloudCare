import { Button } from "@/components/ui/button";
import { Link, useRouteError, isRouteErrorResponse } from "react-router-dom";

export default function AppErrorBoundary() {
  const error = useRouteError();
  let msg = "There was a problem loading this page.";

  if (isRouteErrorResponse(error)) {
    msg = `${error.status} ${error.statusText}`;
  } else if (error instanceof Error) {
    msg = error.message;
  }

  return (
    <div className="flex flex-col items-center justify-center min-h-screen text-center">
      <h1 className="text-8xl font-extrabold text-destructive">Error</h1>
      <h2 className="text-2xl font-bold tracking-tight text-foreground sm:text-3xl mt-4">
        Oops! Something went wrong.
      </h2>
      <p className="mt-4 text-muted-foreground">{msg}</p>
      <Link to="/">
        <Button className="mt-6">Go back home</Button>
      </Link>
    </div>
  );
}
