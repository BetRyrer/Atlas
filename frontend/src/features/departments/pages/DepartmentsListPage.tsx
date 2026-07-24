import { Link } from 'react-router-dom';
import { EmptyState } from '../../../shared/components/ui/EmptyState';
import { ErrorState } from '../../../shared/components/ui/ErrorState';
import { Spinner } from '../../../shared/components/ui/Spinner';
import { useDepartments } from '../hooks/useDepartments';

export function DepartmentsListPage() {
  const { data: departments, isPending, isError, error, refetch } = useDepartments();

  return (
    <div className="flex flex-col gap-4">
      <h1 className="text-xl font-semibold text-neutral-900 dark:text-neutral-100">Départements</h1>

      {isPending ? <Spinner label="Loading departments" /> : null}
      {isError ? <ErrorState error={error} onRetry={() => refetch()} /> : null}

      {!isPending && !isError && departments && departments.length === 0 ? (
        <EmptyState title="Aucun département enregistré." />
      ) : null}

      {!isPending && !isError && departments && departments.length > 0 ? (
        <div className="grid grid-cols-1 gap-3 sm:grid-cols-2 lg:grid-cols-3">
          {departments.map((department) => (
            <Link
              key={department.id}
              to={`/departments/${department.id}`}
              className="flex flex-col gap-1 rounded-lg border border-neutral-200 bg-white p-4 transition-colors hover:border-accent-300 dark:border-neutral-800 dark:bg-neutral-900"
            >
              <span className="font-medium text-neutral-900 dark:text-neutral-100">{department.name}</span>
              <span className="text-sm text-neutral-600 dark:text-neutral-400">{department.description}</span>
              <span className="mt-2 text-xs font-medium text-neutral-500 dark:text-neutral-500">
                {department.headCount} personnes
              </span>
            </Link>
          ))}
        </div>
      ) : null}
    </div>
  );
}
