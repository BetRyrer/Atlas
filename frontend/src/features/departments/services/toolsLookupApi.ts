import { httpClient } from '../../../shared/api/httpClient';
import type { PagedResult } from '../../../shared/types/pagedResult';

export interface ToolOption {
  id: number;
  name: string;
}

const MAX_TOOLS_FOR_SELECTION = 100;

export const toolsLookupApi = {
  async getAllForSelection(): Promise<ToolOption[]> {
    const { data } = await httpClient.get<PagedResult<ToolOption>>('/tools', {
      params: { page: 1, pageSize: MAX_TOOLS_FOR_SELECTION },
    });
    return data.items;
  },
};
