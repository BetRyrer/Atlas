import { httpClient } from '../../../shared/api/httpClient';
import type { PagedResult } from '../../../shared/types/pagedResult';
import type { LicenseType } from '../../tools';

export interface DashboardTool {
  id: number;
  licenseType: LicenseType;
}

export interface DashboardDepartment {
  id: number;
}

export interface DashboardCategory {
  id: number;
}

const MAX_TOOLS = 100;

export const dashboardApi = {
  async getTools(): Promise<DashboardTool[]> {
    const { data } = await httpClient.get<PagedResult<DashboardTool>>('/tools', {
      params: { page: 1, pageSize: MAX_TOOLS },
    });
    return data.items;
  },

  async getDepartments(): Promise<DashboardDepartment[]> {
    const { data } = await httpClient.get<DashboardDepartment[]>('/departments');
    return data;
  },

  async getCategories(): Promise<DashboardCategory[]> {
    const { data } = await httpClient.get<DashboardCategory[]>('/categories');
    return data;
  },
};
