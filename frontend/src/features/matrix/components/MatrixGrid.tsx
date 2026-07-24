import { UsageLevelBadge } from '../../../shared/components/ui/UsageLevelBadge';
import type { UsageLevel } from '../../../shared/types/usageLevel';
import type { Matrix } from '../types';

const cellToneClass: Record<UsageLevel, string> = {
  Primary: 'bg-green-50 dark:bg-green-900/20',
  Secondary: 'bg-blue-50 dark:bg-blue-900/20',
  Evaluating: 'bg-amber-50 dark:bg-amber-900/20',
};

export function MatrixGrid({ matrix }: { matrix: Matrix }) {
  return (
    <div className="max-h-[70vh] overflow-auto rounded-lg border border-neutral-200 dark:border-neutral-800">
      <table className="border-separate border-spacing-0 text-sm">
        <thead>
          <tr>
            <th className="sticky left-0 top-0 z-20 min-w-[180px] border-b border-r border-neutral-200 bg-neutral-50 px-3 py-2 text-left font-medium text-neutral-600 dark:border-neutral-800 dark:bg-neutral-900 dark:text-neutral-400">
              Département
            </th>
            {matrix.tools.map((tool) => (
              <th
                key={tool.toolId}
                scope="col"
                className="sticky top-0 z-10 min-w-[140px] whitespace-nowrap border-b border-neutral-200 bg-neutral-50 px-3 py-2 text-left font-medium text-neutral-600 dark:border-neutral-800 dark:bg-neutral-900 dark:text-neutral-400"
              >
                {tool.toolName}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {matrix.rows.map((row) => (
            <tr key={row.departmentId}>
              <th
                scope="row"
                className="sticky left-0 z-10 min-w-[180px] whitespace-nowrap border-b border-r border-neutral-200 bg-white px-3 py-2 text-left font-medium text-neutral-900 dark:border-neutral-800 dark:bg-neutral-950 dark:text-neutral-100"
              >
                {row.departmentName}
              </th>
              {row.cells.map((cell) => (
                <td
                  key={cell.toolId}
                  className={`border-b border-neutral-200 px-3 py-2 text-center dark:border-neutral-800 ${
                    cell.usageLevel ? cellToneClass[cell.usageLevel] : ''
                  }`}
                >
                  {cell.usageLevel ? (
                    <UsageLevelBadge usageLevel={cell.usageLevel} />
                  ) : (
                    <span className="text-neutral-300 dark:text-neutral-700">—</span>
                  )}
                </td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
