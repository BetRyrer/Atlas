import { isRouteErrorResponse, useRouteError } from 'react-router-dom';
import { ErrorState } from '../shared/components/ui/ErrorState';

export function RouteErrorBoundary() {
  const error = useRouteError();

  const message = isRouteErrorResponse(error)
    ? new Error(`${error.status} ${error.statusText}`)
    : error;

  return (
    <div className="flex min-h-screen items-center justify-center bg-neutral-50 p-6 dark:bg-neutral-950">
      <div className="w-full max-w-md">
        <ErrorState error={message} />
      </div>
    </div>
  );
}
