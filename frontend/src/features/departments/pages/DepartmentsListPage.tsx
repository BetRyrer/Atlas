import { Link } from 'react-router-dom';
import { EmptyState } from '../../../shared/components/ui/EmptyState';
import { ErrorState } from '../../../shared/components/ui/ErrorState';
import { Spinner } from '../../../shared/components/ui/Spinner';
import { useDepartments } from '../hooks/useDepartments';

const avatarPalette = [
  'bg-blue-100 text-blue-700 dark:bg-blue-900/40 dark:text-blue-300',
  'bg-green-100 text-green-700 dark:bg-green-900/40 dark:text-green-300',
  'bg-amber-100 text-amber-700 dark:bg-amber-900/40 dark:text-amber-300',
  'bg-purple-100 text-purple-700 dark:bg-purple-900/40 dark:text-purple-300',
  'bg-red-100 text-red-700 dark:bg-red-900/40 dark:text-red-300',
  'bg-indigo-100 text-indigo-700 dark:bg-indigo-900/40 dark:text-indigo-300',
];

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
        <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
          {departments.map((department, index) => (
            <Link
              key={department.id}
              to={`/departments/${department.id}`}
              className="group flex flex-col gap-3 rounded-lg border border-neutral-200 bg-white p-4 shadow-sm transition-all hover:-translate-y-0.5 hover:border-accent-300 hover:shadow-md dark:border-neutral-800 dark:bg-neutral-900"
            >
              <div className="flex items-start justify-between gap-2">
                <div className="flex items-center gap-3">
                  <span
                    aria-hidden="true"
                    className={`flex h-10 w-10 shrink-0 items-center justify-center rounded-full text-sm font-semibold ${avatarPalette[index % avatarPalette.length]}`}
                  >
                    {department.name.charAt(0).toUpperCase()}
                  </span>
                  <span className="font-medium text-neutral-900 dark:text-neutral-100">{department.name}</span>
                </div>
                <svg
                  aria-hidden="true"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="currentColor"
                  strokeWidth="2"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  className="mt-1 h-4 w-4 shrink-0 text-neutral-300 transition-transform group-hover:translate-x-0.5 group-hover:text-accent-500 dark:text-neutral-600"
                >
                  <path d="M9 18l6-6-6-6" />
                </svg>
              </div>

              <p className="text-sm text-neutral-600 dark:text-neutral-400">{department.description}</p>

              <div className="mt-1 flex items-center gap-4 border-t border-neutral-100 pt-3 text-xs font-medium text-neutral-500 dark:border-neutral-800 dark:text-neutral-400">
                <span className="flex items-center gap-1.5">
                  <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" className="h-4 w-4">
                    <path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2" />
                    <circle cx="9" cy="7" r="4" />
                    <path d="M22 21v-2a4 4 0 0 0-3-3.87M16 3.13a4 4 0 0 1 0 7.75" />
                  </svg>
                  {department.headCount} personnes
                </span>
                <span className="flex items-center gap-1.5">
                  <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" className="h-4 w-4">
                    <path d="M14.7 6.3a1 1 0 0 0 0 1.4l1.6 1.6a1 1 0 0 0 1.4 0l3.77-3.77a6 6 0 0 1-7.94 7.94l-6.91 6.91a2.12 2.12 0 0 1-3-3l6.91-6.91a6 6 0 0 1 7.94-7.94l-3.76 3.76z" />
                  </svg>
                  {department.toolCount} outils
                </span>
              </div>
            </Link>
          ))}
        </div>
      ) : null}
    </div>
  );
}
