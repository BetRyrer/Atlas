import { Link } from 'react-router-dom';

interface NavCardProps {
  to: string;
  title: string;
  description: string;
}

export function NavCard({ to, title, description }: NavCardProps) {
  return (
    <Link
      to={to}
      className="flex flex-col gap-1 rounded-lg border border-neutral-200 bg-white p-4 transition-colors hover:border-accent-300 dark:border-neutral-800 dark:bg-neutral-900"
    >
      <span className="font-medium text-neutral-900 dark:text-neutral-100">{title}</span>
      <span className="text-sm text-neutral-600 dark:text-neutral-400">{description}</span>
    </Link>
  );
}
