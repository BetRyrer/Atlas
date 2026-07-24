export function Spinner({ label = 'Loading' }: { label?: string }) {
  return (
    <div role="status" aria-label={label} className="flex items-center justify-center py-12">
      <div className="h-6 w-6 animate-spin rounded-full border-2 border-neutral-300 border-t-accent-600" />
      <span className="sr-only">{label}</span>
    </div>
  );
}
