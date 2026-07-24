import { z } from 'zod';

export const linkToolFormSchema = z.object({
  toolId: z.coerce.number().int().positive('Please select a tool'),
  usageLevel: z.enum(['Primary', 'Secondary', 'Evaluating']),
  referent: z.string().trim().max(100).optional().or(z.literal('')),
  adoptedOn: z.string().optional().or(z.literal('')),
});

export type LinkToolFormSchema = z.infer<typeof linkToolFormSchema>;
export type LinkToolFormInput = z.input<typeof linkToolFormSchema>;
