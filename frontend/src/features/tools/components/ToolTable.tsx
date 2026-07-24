import { Link } from 'react-router-dom';
import type { ToolListItem, ToolSortColumn } from '../types';
import { LicenseBadge } from './LicenseBadge';
import { ToolLogo } from './ToolLogo';

interface ToolTableProps {
  tools: ToolListItem[];
  sortBy: ToolSortColumn;
  descending: boolean;
  onSortChange: (column: ToolSortColumn) => void;
}

interface SortableHeaderProps {
  label: string;
  column: ToolSortColumn;
  sortBy: ToolSortColumn;
  descending: boolean;
  onSortChange: (column: ToolSortColumn) => void;
}

function SortableHeader({ label, column, sortBy, descending, onSortChange }: SortableHeaderProps) {
  const isActive = sortBy === column;

  return (
    <th
      scope="col"
      aria-sort={isActive ? (descending ? 'descending' : 'ascending') : 'none'}
      className="px-4 py-2 text-left font-medium text-neutral-600 dark:text-neutral-400"
    >
      <button
        type="button"
        onClick={() => onSortChange(column)}
        className="inline-flex items-center gap-1 hover:text-neutral-900 dark:hover:text-neutral-100"
      >
        {label}
        <span aria-hidden="true" className="text-[10px]">
          {isActive ? (descending ? '▼' : '▲') : ''}
        </span>
      </button>
    </th>
  );
}

export function ToolTable({ tools, sortBy, descending, onSortChange }: ToolTableProps) {
  return (
    <div className="overflow-x-auto rounded-lg border border-neutral-200 dark:border-neutral-800">
      <table className="min-w-full divide-y divide-neutral-200 text-sm dark:divide-neutral-800">
        <thead className="bg-neutral-50 dark:bg-neutral-900">
          <tr>
            <SortableHeader label="Nom" column="name" sortBy={sortBy} descending={descending} onSortChange={onSortChange} />
            <SortableHeader label="Éditeur" column="vendor" sortBy={sortBy} descending={descending} onSortChange={onSortChange} />
            <SortableHeader label="Version" column="version" sortBy={sortBy} descending={descending} onSortChange={onSortChange} />
            <SortableHeader label="Catégorie" column="category" sortBy={sortBy} descending={descending} onSortChange={onSortChange} />
            <SortableHeader label="Licence" column="license" sortBy={sortBy} descending={descending} onSortChange={onSortChange} />
          </tr>
        </thead>
        <tbody className="divide-y divide-neutral-200 bg-white dark:divide-neutral-800 dark:bg-neutral-950">
          {tools.map((tool) => (
            <tr key={tool.id} className="hover:bg-neutral-50 dark:hover:bg-neutral-900">
              <td className="px-4 py-2">
                <Link
                  to={`/tools/${tool.id}`}
                  className="flex items-center gap-2 font-medium text-accent-600 hover:underline dark:text-accent-500"
                >
                  <ToolLogo name={tool.name} logoUrl={tool.logoUrl} size={24} />
                  {tool.name}
                </Link>
              </td>
              <td className="px-4 py-2 text-neutral-700 dark:text-neutral-300">{tool.vendor}</td>
              <td className="px-4 py-2 text-neutral-700 dark:text-neutral-300">{tool.version}</td>
              <td className="px-4 py-2 text-neutral-700 dark:text-neutral-300">{tool.categoryName}</td>
              <td className="px-4 py-2">
                <LicenseBadge licenseType={tool.licenseType} />
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
