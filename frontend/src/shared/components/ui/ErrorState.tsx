import { getErrorMessage } from '../../api/apiError';
import { Button } from './Button';

interface ErrorStateProps {
  error: unknown;
  onRetry?: () => void;
}

export function ErrorState({ error, onRetry }: ErrorStateProps) {
  return (
    <div
      role="alert"
      className="flex flex-col items-center gap-3 rounded-lg border border-red-200 bg-red-50 py-10 text-center dark:border-red-900/50 dark:bg-red-950/30"
    >
      <p className="text-sm font-medium text-red-800 dark:text-red-300">{getErrorMessage(error)}</p>
      {onRetry ? (
        <Button variant="secondary" onClick={onRetry}>
          Retry
        </Button>
      ) : null}
    </div>
  );
}
