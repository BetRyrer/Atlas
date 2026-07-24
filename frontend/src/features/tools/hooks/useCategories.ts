import { useQuery } from '@tanstack/react-query';
import { categoriesApi } from '../services/categoriesApi';
import { categoryKeys } from './toolKeys';

export function useCategories() {
  return useQuery({
    queryKey: categoryKeys.list(),
    queryFn: categoriesApi.getAll,
    staleTime: 5 * 60_000,
  });
}
