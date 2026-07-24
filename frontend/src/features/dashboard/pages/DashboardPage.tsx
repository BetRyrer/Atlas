import { useAuth } from '../../../shared/auth/AuthContext';
import { ErrorState } from '../../../shared/components/ui/ErrorState';
import { Spinner } from '../../../shared/components/ui/Spinner';
import { StatTile } from '../../../shared/components/ui/StatTile';
import { LicenseBreakdown } from '../components/LicenseBreakdown';
import { NavCard } from '../components/NavCard';
import { useDashboardStats } from '../hooks/useDashboardStats';

export function DashboardPage() {
  const { user } = useAuth();
  const { stats, isPending, isError, error, refetch } = useDashboardStats();

  return (
    <div className="flex flex-col gap-6">
      <div>
        <h1 className="text-xl font-semibold text-neutral-900 dark:text-neutral-100">
          Bonjour, {user?.displayName?.split(' ')[0] ?? user?.username}
        </h1>
        <p className="mt-1 text-sm text-neutral-600 dark:text-neutral-400">
          Vue d'ensemble du catalogue d'outillage.
        </p>
      </div>

      {isPending ? <Spinner label="Loading dashboard" /> : null}
      {isError ? <ErrorState error={error} onRetry={refetch} /> : null}

      {stats ? (
        <>
          <div className="grid grid-cols-1 gap-4 sm:grid-cols-3">
            <StatTile label="Outils" value={stats.totalTools} />
            <StatTile label="Départements" value={stats.totalDepartments} />
            <StatTile label="Catégories" value={stats.totalCategories} />
          </div>

          <div className="grid grid-cols-1 gap-4 lg:grid-cols-2">
            <div className="rounded-lg border border-neutral-200 bg-white p-4 dark:border-neutral-800 dark:bg-neutral-900">
              <h2 className="mb-3 text-sm font-semibold text-neutral-900 dark:text-neutral-100">
                Répartition par licence
              </h2>
              <LicenseBreakdown counts={stats.licenseCounts} total={stats.totalTools} />
            </div>

            <div className="grid grid-cols-1 gap-3 sm:grid-cols-2">
              <NavCard to="/tools" title="Outils" description="Parcourir et gérer le catalogue d'outils." />
              <NavCard
                to="/departments"
                title="Départements"
                description="Voir les outils utilisés par chaque service."
              />
              <NavCard
                to="/matrix"
                title="Matrice"
                description="Visualiser la couverture départements × outils."
              />
            </div>
          </div>
        </>
      ) : null}
    </div>
  );
}
