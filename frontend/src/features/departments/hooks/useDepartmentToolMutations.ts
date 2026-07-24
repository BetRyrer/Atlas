import { useMutation, useQueryClient } from '@tanstack/react-query';
import { getErrorMessage } from '../../../shared/api/apiError';
import { useToast } from '../../../shared/components/ui/Toast';
import { departmentsApi } from '../services/departmentsApi';
import type { DepartmentDetail, DepartmentToolLink, LinkToolInput } from '../types';
import { departmentKeys } from './departmentKeys';

interface DepartmentContext {
  previous: DepartmentDetail | undefined;
}

export function useLinkTool(departmentId: number) {
  const queryClient = useQueryClient();
  const { showToast } = useToast();

  return useMutation<DepartmentToolLink, unknown, LinkToolInput & { toolName: string }, DepartmentContext>({
    mutationFn: (input) => departmentsApi.linkTool(departmentId, input),
    onMutate: async (input) => {
      await queryClient.cancelQueries({ queryKey: departmentKeys.detail(departmentId) });
      const previous = queryClient.getQueryData<DepartmentDetail>(departmentKeys.detail(departmentId));

      if (previous) {
        const optimisticLink: DepartmentToolLink = {
          toolId: input.toolId,
          toolName: input.toolName,
          categoryName: '…',
          logoUrl: null,
          usageLevel: input.usageLevel,
          referent: input.referent ?? null,
          adoptedOn: input.adoptedOn ?? null,
        };

        queryClient.setQueryData<DepartmentDetail>(departmentKeys.detail(departmentId), {
          ...previous,
          tools: [...previous.tools, optimisticLink],
        });
      }

      return { previous };
    },
    onSuccess: () => {
      showToast('Outil lié au département avec succès.');
    },
    onError: (error, _input, context) => {
      if (context?.previous) {
        queryClient.setQueryData(departmentKeys.detail(departmentId), context.previous);
      }
      showToast(getErrorMessage(error), 'error');
    },
    onSettled: () => {
      queryClient.invalidateQueries({ queryKey: departmentKeys.detail(departmentId) });
    },
  });
}

export function useUnlinkTool(departmentId: number) {
  const queryClient = useQueryClient();
  const { showToast } = useToast();

  return useMutation<void, unknown, number, DepartmentContext>({
    mutationFn: (toolId) => departmentsApi.unlinkTool(departmentId, toolId),
    onMutate: async (toolId) => {
      await queryClient.cancelQueries({ queryKey: departmentKeys.detail(departmentId) });
      const previous = queryClient.getQueryData<DepartmentDetail>(departmentKeys.detail(departmentId));

      if (previous) {
        queryClient.setQueryData<DepartmentDetail>(departmentKeys.detail(departmentId), {
          ...previous,
          tools: previous.tools.filter((link) => link.toolId !== toolId),
        });
      }

      return { previous };
    },
    onSuccess: () => {
      showToast('Outil retiré du département avec succès.');
    },
    onError: (error, _toolId, context) => {
      if (context?.previous) {
        queryClient.setQueryData(departmentKeys.detail(departmentId), context.previous);
      }
      showToast(getErrorMessage(error), 'error');
    },
    onSettled: () => {
      queryClient.invalidateQueries({ queryKey: departmentKeys.detail(departmentId) });
    },
  });
}
