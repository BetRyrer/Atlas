import { useMutation, useQueryClient } from '@tanstack/react-query';
import { getErrorMessage } from '../../../shared/api/apiError';
import { useToast } from '../../../shared/components/ui/Toast';
import { toolsApi } from '../services/toolsApi';
import type { ToolApiInput } from '../types/toolFormSchema';
import { toolKeys } from './toolKeys';

export function useCreateTool() {
  const queryClient = useQueryClient();
  const { showToast } = useToast();

  return useMutation({
    mutationFn: (input: ToolApiInput) => toolsApi.create(input),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: toolKeys.lists() });
      showToast('Outil créé avec succès.');
    },
    onError: (error) => showToast(getErrorMessage(error), 'error'),
  });
}

export function useUpdateTool(id: number) {
  const queryClient = useQueryClient();
  const { showToast } = useToast();

  return useMutation({
    mutationFn: (input: ToolApiInput) => toolsApi.update(id, input),
    onSuccess: (data) => {
      queryClient.setQueryData(toolKeys.detail(id), data);
      queryClient.invalidateQueries({ queryKey: toolKeys.lists() });
      showToast('Outil mis à jour avec succès.');
    },
    onError: (error) => showToast(getErrorMessage(error), 'error'),
  });
}

export function useDeleteTool() {
  const queryClient = useQueryClient();
  const { showToast } = useToast();

  return useMutation({
    mutationFn: (id: number) => toolsApi.remove(id),
    onSuccess: (_data, id) => {
      queryClient.removeQueries({ queryKey: toolKeys.detail(id) });
      queryClient.invalidateQueries({ queryKey: toolKeys.lists() });
      showToast('Outil supprimé avec succès.');
    },
    onError: (error) => showToast(getErrorMessage(error), 'error'),
  });
}
