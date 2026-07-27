import type { PropsWithChildren } from 'react';

export type Tone = 'neutral' | 'blue' | 'green' | 'amber' | 'red' | 'purple' | 'indigo';

export const toneClasses: Record<Tone, string> = {
  neutral: 'bg-neutral-100 text-neutral-700 dark:bg-neutral-800 dark:text-neutral-300',
  blue: 'bg-blue-100 text-blue-800 dark:bg-blue-900/40 dark:text-blue-300',
  green: 'bg-green-100 text-green-800 dark:bg-green-900/40 dark:text-green-300',
  amber: 'bg-amber-100 text-amber-800 dark:bg-amber-900/40 dark:text-amber-300',
  red: 'bg-red-100 text-red-800 dark:bg-red-900/40 dark:text-red-300',
  purple: 'bg-purple-100 text-purple-800 dark:bg-purple-900/40 dark:text-purple-300',
  indigo: 'bg-indigo-100 text-indigo-800 dark:bg-indigo-900/40 dark:text-indigo-300',
};

/** Fixed hue order for assigning a distinct tone per item (e.g. an avatar per row) — never cycle arbitrarily. */
export const categoricalTones: readonly Tone[] = ['blue', 'green', 'amber', 'purple', 'red', 'indigo'];

interface BadgeProps extends PropsWithChildren {
  tone?: Tone;
}

export function Badge({ tone = 'neutral', children }: BadgeProps) {
  return (
    <span
      className={`inline-flex items-center rounded-full px-2 py-0.5 text-xs font-medium ${toneClasses[tone]}`}
    >
      {children}
    </span>
  );
}
