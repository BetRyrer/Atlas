import type { ToolQueryFilters } from '../types';

export const toolKeys = {
  all: ['tools'] as const,
  lists: () => [...toolKeys.all, 'list'] as const,
  list: (filters: ToolQueryFilters) => [...toolKeys.lists(), filters] as const,
  details: () => [...toolKeys.all, 'detail'] as const,
  detail: (id: number) => [...toolKeys.details(), id] as const,
};

export const categoryKeys = {
  all: ['categories'] as const,
  list: () => [...categoryKeys.all, 'list'] as const,
};
