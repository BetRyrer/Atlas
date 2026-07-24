import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import { Button } from '../../../shared/components/ui/Button';
import { Input } from '../../../shared/components/ui/Input';
import { Select } from '../../../shared/components/ui/Select';
import { usageLevelLabels } from '../../../shared/types/usageLevel';
import { useToolOptions } from '../hooks/useToolOptions';
import {
  linkToolFormSchema,
  type LinkToolFormInput,
  type LinkToolFormSchema,
} from '../types/linkToolFormSchema';

const usageLevels: LinkToolFormSchema['usageLevel'][] = ['Primary', 'Secondary', 'Evaluating'];

interface LinkToolFormProps {
  isSubmitting: boolean;
  onSubmit: (values: LinkToolFormSchema, toolName: string) => void;
}

export function LinkToolForm({ isSubmitting, onSubmit }: LinkToolFormProps) {
  const { data: toolOptions } = useToolOptions();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LinkToolFormInput, unknown, LinkToolFormSchema>({
    resolver: zodResolver(linkToolFormSchema),
    defaultValues: { toolId: '', usageLevel: 'Primary', referent: '', adoptedOn: '' },
  });

  function submit(values: LinkToolFormSchema) {
    const toolName = toolOptions?.find((tool) => tool.id === values.toolId)?.name ?? '';
    onSubmit(values, toolName);
  }

  return (
    <form onSubmit={handleSubmit(submit)} className="flex flex-col gap-4">
      <Select
        id="link-tool-form-toolId"
        label="Outil"
        {...register('toolId')}
        error={errors.toolId?.message}
        defaultValue=""
      >
        <option value="" disabled>
          Sélectionner un outil
        </option>
        {toolOptions?.map((tool) => (
          <option key={tool.id} value={tool.id}>
            {tool.name}
          </option>
        ))}
      </Select>
      <Select
        id="link-tool-form-usageLevel"
        label="Niveau d'usage"
        {...register('usageLevel')}
        error={errors.usageLevel?.message}
      >
        {usageLevels.map((level) => (
          <option key={level} value={level}>
            {usageLevelLabels[level]}
          </option>
        ))}
      </Select>
      <Input
        id="link-tool-form-referent"
        label="Référent"
        {...register('referent')}
        error={errors.referent?.message}
      />
      <Input
        id="link-tool-form-adoptedOn"
        label="Adopté le"
        type="date"
        {...register('adoptedOn')}
        error={errors.adoptedOn?.message}
      />
      <div className="flex justify-end gap-2 pt-2">
        <Button type="submit" disabled={isSubmitting}>
          {isSubmitting ? 'Liaison…' : 'Lier l’outil'}
        </Button>
      </div>
    </form>
  );
}
