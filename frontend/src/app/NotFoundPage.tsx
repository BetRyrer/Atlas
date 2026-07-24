import { Link } from 'react-router-dom';
import { Button } from '../shared/components/ui/Button';

export function NotFoundPage() {
  return (
    <div className="flex flex-1 flex-col items-center justify-center gap-3 py-24 text-center">
      <p className="text-sm font-semibold uppercase tracking-wide text-accent-600 dark:text-accent-500">
        Erreur 404
      </p>
      <h1 className="text-2xl font-semibold text-neutral-900 dark:text-neutral-100">
        Cette page n'existe pas.
      </h1>
      <p className="max-w-sm text-sm text-neutral-600 dark:text-neutral-400">
        L'adresse demandée ne correspond à aucune page du catalogue. Elle a peut-être été
        déplacée ou supprimée.
      </p>
      <Link to="/dashboard" className="mt-2">
        <Button>Retour au tableau de bord</Button>
      </Link>
    </div>
  );
}
