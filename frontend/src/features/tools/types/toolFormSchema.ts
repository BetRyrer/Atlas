import { z } from 'zod';
import { licenseTypes, type LicenseType } from './index';

const currentYear = new Date().getFullYear();

export const toolFormSchema = z.object({
  name: z.string().trim().min(1, 'Name is required').max(100),
  vendor: z.string().trim().min(1, 'Vendor is required').max(100),
  version: z.string().trim().min(1, 'Version is required').max(50),
  description: z.string().trim().min(1, 'Description is required').max(2000),
  licenseType: z.enum(licenseTypes as [LicenseType, ...LicenseType[]]),
  documentationUrl: z
    .string()
    .trim()
    .url('Must be a valid URL')
    .optional()
    .or(z.literal('')),
  categoryId: z.coerce.number().int().positive('Category is required'),
  foundedYear: z.preprocess(
    (value) => (value === '' ? undefined : value),
    z.coerce.number().int().min(1970, 'Must be 1970 or later').max(currentYear, 'Cannot be in the future').optional(),
  ),
  logoUrl: z.string().trim().url('Must be a valid URL').optional().or(z.literal('')),
  youtubeVideoUrl: z
    .string()
    .trim()
    .url('Must be a valid URL')
    .refine((url) => url.includes('youtube.com') || url.includes('youtu.be'), {
      message: 'Must be a youtube.com or youtu.be URL',
    })
    .optional()
    .or(z.literal('')),
  availableVersions: z.string().trim().optional().or(z.literal('')),
});

export type ToolFormSchema = z.infer<typeof toolFormSchema>;
export type ToolFormInput = z.input<typeof toolFormSchema>;

export interface ToolApiInput {
  name: string;
  vendor: string;
  version: string;
  description: string;
  licenseType: LicenseType;
  documentationUrl?: string;
  categoryId: number;
  foundedYear?: number;
  logoUrl?: string;
  youtubeVideoUrl?: string;
  availableVersions: string[];
}

export function toApiInput(values: ToolFormSchema): ToolApiInput {
  return {
    name: values.name,
    vendor: values.vendor,
    version: values.version,
    description: values.description,
    licenseType: values.licenseType,
    documentationUrl: values.documentationUrl || undefined,
    categoryId: values.categoryId,
    foundedYear: values.foundedYear || undefined,
    logoUrl: values.logoUrl || undefined,
    youtubeVideoUrl: values.youtubeVideoUrl || undefined,
    availableVersions: values.availableVersions
      ? values.availableVersions.split(',').map((version) => version.trim()).filter(Boolean)
      : [],
  };
}

export function availableVersionsToFormValue(versions: string[]): string {
  return versions.join(', ');
}
