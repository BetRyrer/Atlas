import { httpClient } from '../../../shared/api/httpClient';
import type { DepartmentDetail, DepartmentListItem, DepartmentToolLink, LinkToolInput } from '../types';

export const departmentsApi = {
  async getAll(): Promise<DepartmentListItem[]> {
    const { data } = await httpClient.get<DepartmentListItem[]>('/departments');
    return data;
  },

  async getById(id: number): Promise<DepartmentDetail> {
    const { data } = await httpClient.get<DepartmentDetail>(`/departments/${id}`);
    return data;
  },

  async linkTool(id: number, input: LinkToolInput): Promise<DepartmentToolLink> {
    const { data } = await httpClient.post<DepartmentToolLink>(`/departments/${id}/tools`, input);
    return data;
  },

  async unlinkTool(id: number, toolId: number): Promise<void> {
    await httpClient.delete(`/departments/${id}/tools/${toolId}`);
  },
};
