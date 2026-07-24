import { useQueries } from '@tanstack/react-query';
import type { LicenseType } from '../../tools';
import { licenseTypes } from '../../tools';
import {
  dashboardApi,
  type DashboardCategory,
  type DashboardDepartment,
  type DashboardTool,
} from '../services/dashboardApi';

export interface DashboardStats {
  totalTools: number;
  totalDepartments: number;
  totalCategories: number;
  licenseCounts: Record<LicenseType, number>;
}

function computeStats(
  tools: DashboardTool[],
  departments: DashboardDepartment[],
  categories: DashboardCategory[],
): DashboardStats {
  const licenseCounts = Object.fromEntries(licenseTypes.map((type) => [type, 0])) as Record<
    LicenseType,
    number
  >;

  for (const tool of tools) {
    licenseCounts[tool.licenseType] += 1;
  }

  return {
    totalTools: tools.length,
    totalDepartments: departments.length,
    totalCategories: categories.length,
    licenseCounts,
  };
}

export function useDashboardStats() {
  const results = useQueries({
    queries: [
      { queryKey: ['dashboard', 'tools'], queryFn: dashboardApi.getTools },
      { queryKey: ['dashboard', 'departments'], queryFn: dashboardApi.getDepartments },
      { queryKey: ['dashboard', 'categories'], queryFn: dashboardApi.getCategories },
    ],
  });

  const [toolsQuery, departmentsQuery, categoriesQuery] = results;

  const isPending = results.some((query) => query.isPending);
  const isError = results.some((query) => query.isError);
  const error = results.find((query) => query.error)?.error;

  const stats =
    toolsQuery.data && departmentsQuery.data && categoriesQuery.data
      ? computeStats(toolsQuery.data, departmentsQuery.data, categoriesQuery.data)
      : null;

  function refetch() {
    for (const query of results) {
      void query.refetch();
    }
  }

  return { stats, isPending, isError, error, refetch };
}
