import { useQuery } from '@tanstack/react-query';
import { departmentsApi } from '../services/departmentsApi';
import { departmentKeys } from './departmentKeys';

export function useDepartment(id: number) {
  return useQuery({
    queryKey: departmentKeys.detail(id),
    queryFn: () => departmentsApi.getById(id),
  });
}
