import type { PropsWithChildren, ReactNode } from 'react';

interface EmptyStateProps extends PropsWithChildren {
  title: string;
  action?: ReactNode;
}

export function EmptyState({ title, action, children }: EmptyStateProps) {
  return (
    <div className="flex flex-col items-center justify-center gap-3 rounded-lg border border-dashed border-neutral-300 py-12 text-center dark:border-neutral-700">
      <p className="text-sm font-medium text-neutral-700 dark:text-neutral-300">{title}</p>
      {children ? <p className="max-w-sm text-sm text-neutral-500 dark:text-neutral-400">{children}</p> : null}
      {action}
    </div>
  );
}
