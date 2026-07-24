import { licenseTypes, type LicenseType } from '../../tools';

interface LicenseBreakdownProps {
  counts: Record<LicenseType, number>;
  total: number;
}

const barToneClass: Record<LicenseType, string> = {
  Proprietary: 'bg-purple-500',
  OpenSource: 'bg-green-500',
  Freemium: 'bg-blue-500',
  Internal: 'bg-neutral-400',
};

export function LicenseBreakdown({ counts, total }: LicenseBreakdownProps) {
  return (
    <div className="flex flex-col gap-3">
      {licenseTypes.map((license) => {
        const count = counts[license];
        const percentage = total > 0 ? Math.round((count / total) * 100) : 0;

        return (
          <div key={license}>
            <div className="mb-1 flex items-center justify-between text-sm">
              <span className="text-neutral-700 dark:text-neutral-300">{license}</span>
              <span className="font-medium text-neutral-900 dark:text-neutral-100">{count}</span>
            </div>
            <div className="h-2 w-full overflow-hidden rounded-full bg-neutral-100 dark:bg-neutral-800">
              <div
                className={`h-full ${barToneClass[license]}`}
                style={{ width: `${percentage}%` }}
              />
            </div>
          </div>
        );
      })}
    </div>
  );
}
