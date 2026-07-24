import { useQuery } from '@tanstack/react-query';
import { toolsLookupApi } from '../services/toolsLookupApi';
import { toolOptionKeys } from './departmentKeys';

export function useToolOptions() {
  return useQuery({
    queryKey: toolOptionKeys.all,
    queryFn: toolsLookupApi.getAllForSelection,
    staleTime: 5 * 60_000,
  });
}
