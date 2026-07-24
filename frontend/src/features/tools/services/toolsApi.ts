import { httpClient } from '../../../shared/api/httpClient';
import type { PagedResult } from '../../../shared/types/pagedResult';
import type { ToolDetail, ToolListItem, ToolQueryFilters } from '../types';
import type { ToolApiInput } from '../types/toolFormSchema';

export const toolsApi = {
  async getPaged(filters: ToolQueryFilters): Promise<PagedResult<ToolListItem>> {
    const { data } = await httpClient.get<PagedResult<ToolListItem>>('/tools', { params: filters });
    return data;
  },

  async getById(id: number): Promise<ToolDetail> {
    const { data } = await httpClient.get<ToolDetail>(`/tools/${id}`);
    return data;
  },

  async create(input: ToolApiInput): Promise<ToolDetail> {
    const { data } = await httpClient.post<ToolDetail>('/tools', input);
    return data;
  },

  async update(id: number, input: ToolApiInput): Promise<ToolDetail> {
    const { data } = await httpClient.put<ToolDetail>(`/tools/${id}`, input);
    return data;
  },

  async remove(id: number): Promise<void> {
    await httpClient.delete(`/tools/${id}`);
  },
};
