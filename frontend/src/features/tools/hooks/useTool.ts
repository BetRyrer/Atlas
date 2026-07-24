import { useQuery } from '@tanstack/react-query';
import { toolsApi } from '../services/toolsApi';
import { toolKeys } from './toolKeys';

export function useTool(id: number) {
  return useQuery({
    queryKey: toolKeys.detail(id),
    queryFn: () => toolsApi.getById(id),
  });
}
