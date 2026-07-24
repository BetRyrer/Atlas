import { Link } from 'react-router-dom';
import { Badge } from '../../../shared/components/ui/Badge';
import { Button } from '../../../shared/components/ui/Button';
import { UsageLevelBadge } from '../../../shared/components/ui/UsageLevelBadge';
import { ToolLogo } from '../../tools';
import type { DepartmentToolLink } from '../types';

interface DepartmentToolsTableProps {
  tools: DepartmentToolLink[];
  onUnlink: (toolId: number) => void;
  isUnlinking: boolean;
}

export function DepartmentToolsTable({ tools, onUnlink, isUnlinking }: DepartmentToolsTableProps) {
  return (
    <div className="overflow-x-auto rounded-lg border border-neutral-200 dark:border-neutral-800">
      <table className="min-w-full divide-y divide-neutral-200 text-sm dark:divide-neutral-800">
        <thead className="bg-neutral-50 dark:bg-neutral-900">
          <tr>
            <th scope="col" className="px-4 py-2 text-left font-medium text-neutral-600 dark:text-neutral-400">
              Outil
            </th>
            <th scope="col" className="px-4 py-2 text-left font-medium text-neutral-600 dark:text-neutral-400">
              Catégorie
            </th>
            <th scope="col" className="px-4 py-2 text-left font-medium text-neutral-600 dark:text-neutral-400">
              Usage
            </th>
            <th scope="col" className="px-4 py-2 text-left font-medium text-neutral-600 dark:text-neutral-400">
              Référent
            </th>
            <th scope="col" className="px-4 py-2 text-left font-medium text-neutral-600 dark:text-neutral-400">
              Adopté le
            </th>
            <th scope="col" className="px-4 py-2" />
          </tr>
        </thead>
        <tbody className="divide-y divide-neutral-200 bg-white dark:divide-neutral-800 dark:bg-neutral-950">
          {tools.map((link) => (
            <tr key={link.toolId} className="hover:bg-neutral-50 dark:hover:bg-neutral-900">
              <td className="px-4 py-2">
                <Link
                  to={`/tools/${link.toolId}`}
                  className="flex items-center gap-2 font-medium text-accent-600 hover:underline dark:text-accent-500"
                >
                  <ToolLogo name={link.toolName} logoUrl={link.logoUrl} size={24} />
                  {link.toolName}
                </Link>
              </td>
              <td className="px-4 py-2">
                <Badge tone="neutral">{link.categoryName}</Badge>
              </td>
              <td className="px-4 py-2">
                <UsageLevelBadge usageLevel={link.usageLevel} />
              </td>
              <td className="px-4 py-2 text-neutral-700 dark:text-neutral-300">{link.referent ?? '—'}</td>
              <td className="px-4 py-2 text-neutral-700 dark:text-neutral-300">{link.adoptedOn ?? '—'}</td>
              <td className="px-4 py-2 text-right">
                <Button
                  variant="ghost"
                  disabled={isUnlinking}
                  onClick={() => onUnlink(link.toolId)}
                  aria-label={`Retirer ${link.toolName}`}
                >
                  Retirer
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
