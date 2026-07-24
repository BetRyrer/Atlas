import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import { Button } from '../../../shared/components/ui/Button';
import { Input } from '../../../shared/components/ui/Input';
import { Select } from '../../../shared/components/ui/Select';
import { Textarea } from '../../../shared/components/ui/Textarea';
import { useCategories } from '../hooks/useCategories';
import { licenseTypes } from '../types';
import {
  toApiInput,
  toolFormSchema,
  type ToolApiInput,
  type ToolFormInput,
  type ToolFormSchema,
} from '../types/toolFormSchema';

interface ToolFormProps {
  defaultValues?: Partial<ToolFormInput>;
  submitLabel: string;
  isSubmitting: boolean;
  onSubmit: (values: ToolApiInput) => void;
}

export function ToolForm({ defaultValues, submitLabel, isSubmitting, onSubmit }: ToolFormProps) {
  const { data: categories } = useCategories();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<ToolFormInput, unknown, ToolFormSchema>({
    resolver: zodResolver(toolFormSchema),
    defaultValues: {
      name: '',
      vendor: '',
      version: '',
      description: '',
      licenseType: 'OpenSource',
      documentationUrl: '',
      categoryId: '',
      foundedYear: '',
      logoUrl: '',
      youtubeVideoUrl: '',
      availableVersions: '',
      ...defaultValues,
    },
  });

  function submit(values: ToolFormSchema) {
    onSubmit(toApiInput(values));
  }

  return (
    <form onSubmit={handleSubmit(submit)} className="flex flex-col gap-4">
      <Input id="tool-form-name" label="Nom" {...register('name')} error={errors.name?.message} />
      <Input id="tool-form-vendor" label="Éditeur" {...register('vendor')} error={errors.vendor?.message} />
      <Input id="tool-form-version" label="Version" {...register('version')} error={errors.version?.message} />
      <Textarea
        id="tool-form-description"
        label="Description"
        rows={5}
        {...register('description')}
        error={errors.description?.message}
      />
      <Input
        id="tool-form-documentationUrl"
        label="Documentation (URL)"
        {...register('documentationUrl')}
        error={errors.documentationUrl?.message}
      />
      <Select
        id="tool-form-licenseType"
        label="Licence"
        {...register('licenseType')}
        error={errors.licenseType?.message}
      >
        {licenseTypes.map((type) => (
          <option key={type} value={type}>
            {type}
          </option>
        ))}
      </Select>
      <Select
        id="tool-form-categoryId"
        label="Catégorie"
        {...register('categoryId')}
        error={errors.categoryId?.message}
        defaultValue=""
      >
        <option value="" disabled>
          Sélectionner une catégorie
        </option>
        {categories?.map((category) => (
          <option key={category.id} value={category.id}>
            {category.name}
          </option>
        ))}
      </Select>
      <Input
        id="tool-form-foundedYear"
        label="Année de création"
        type="number"
        {...register('foundedYear')}
        error={errors.foundedYear?.message}
      />
      <Input
        id="tool-form-logoUrl"
        label="Logo (URL)"
        {...register('logoUrl')}
        error={errors.logoUrl?.message}
      />
      <Input
        id="tool-form-youtubeVideoUrl"
        label="Vidéo de présentation (YouTube)"
        placeholder="https://www.youtube.com/watch?v=..."
        {...register('youtubeVideoUrl')}
        error={errors.youtubeVideoUrl?.message}
      />
      <Input
        id="tool-form-availableVersions"
        label="Versions disponibles (séparées par des virgules)"
        placeholder="ex. 2.43, 2.44, 2.45"
        {...register('availableVersions')}
        error={errors.availableVersions?.message}
      />
      <div className="flex justify-end gap-2 pt-2">
        <Button type="submit" disabled={isSubmitting}>
          {isSubmitting ? 'Enregistrement…' : submitLabel}
        </Button>
      </div>
    </form>
  );
}
