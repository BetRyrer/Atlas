import { ErrorState } from '../../../shared/components/ui/ErrorState';
import { Spinner } from '../../../shared/components/ui/Spinner';
import { MatrixGrid } from '../components/MatrixGrid';
import { useMatrix } from '../hooks/useMatrix';

export function MatrixPage() {
  const { data: matrix, isPending, isError, error, refetch } = useMatrix();

  return (
    <div className="flex flex-col gap-4">
      <h1 className="text-xl font-semibold text-neutral-900 dark:text-neutral-100">Matrice de couverture</h1>

      {isPending ? <Spinner label="Loading matrix" /> : null}
      {isError ? <ErrorState error={error} onRetry={() => refetch()} /> : null}
      {!isPending && !isError && matrix ? <MatrixGrid matrix={matrix} /> : null}
    </div>
  );
}
