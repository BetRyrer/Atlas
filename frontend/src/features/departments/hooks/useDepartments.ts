import { useQuery } from '@tanstack/react-query';
import { departmentsApi } from '../services/departmentsApi';
import { departmentKeys } from './departmentKeys';

export function useDepartments() {
  return useQuery({
    queryKey: departmentKeys.lists(),
    queryFn: departmentsApi.getAll,
  });
}
