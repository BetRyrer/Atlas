import { httpClient } from '../../../shared/api/httpClient';
import type { Category } from '../types';

export const categoriesApi = {
  async getAll(): Promise<Category[]> {
    const { data } = await httpClient.get<Category[]>('/categories');
    return data;
  },
};
