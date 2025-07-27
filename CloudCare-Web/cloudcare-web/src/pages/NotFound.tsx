

    // src/app/not-found.tsx
    import { Button } from "@/components/ui/button"; // Assuming you have shadcn/ui button
import { Link } from "react-router-dom";

    export default function NotFound() {
      return (
        <div className="flex flex-col items-center justify-center min-h-screen text-center">
          <h1 className="text-9xl font-extrabold text-primary">404</h1>
          <h2 className="text-3xl font-bold tracking-tight text-foreground sm:text-4xl mt-4">
            Page Not Found
          </h2>
          <p className="mt-4 text-muted-foreground">
            The page you are looking for does not exist.
          </p>
          <Link to="/">
            <Button className="mt-6">Go back home</Button>
          </Link>
        </div>
      );
    }