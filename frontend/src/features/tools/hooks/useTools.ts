import { keepPreviousData, useQuery } from '@tanstack/react-query';
import { toolsApi } from '../services/toolsApi';
import type { ToolQueryFilters } from '../types';
import { toolKeys } from './toolKeys';

export function useTools(filters: ToolQueryFilters) {
  return useQuery({
    queryKey: toolKeys.list(filters),
    queryFn: () => toolsApi.getPaged(filters),
    placeholderData: keepPreviousData,
  });
}
