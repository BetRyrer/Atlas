import { useQuery } from '@tanstack/react-query';
import { httpClient } from '../../../shared/api/httpClient';
import type { Matrix } from '../types';

export function useMatrix() {
  return useQuery({
    queryKey: ['matrix'] as const,
    queryFn: async () => {
      const { data } = await httpClient.get<Matrix>('/matrix');
      return data;
    },
  });
}
