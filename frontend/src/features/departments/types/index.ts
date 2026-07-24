import type { UsageLevel } from '../../../shared/types/usageLevel';

export interface DepartmentListItem {
  id: number;
  name: string;
  description: string;
  headCount: number;
}

export interface DepartmentToolLink {
  toolId: number;
  toolName: string;
  categoryName: string;
  logoUrl: string | null;
  usageLevel: UsageLevel;
  referent: string | null;
  adoptedOn: string | null;
}

export interface DepartmentDetail {
  id: number;
  name: string;
  description: string;
  headCount: number;
  tools: DepartmentToolLink[];
}

export interface LinkToolInput {
  toolId: number;
  usageLevel: UsageLevel;
  referent?: string;
  adoptedOn?: string;
}
