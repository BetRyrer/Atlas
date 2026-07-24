import type { UsageLevel } from '../../../shared/types/usageLevel';

export type LicenseType = 'Proprietary' | 'OpenSource' | 'Freemium' | 'Internal';

export const licenseTypes: LicenseType[] = ['Proprietary', 'OpenSource', 'Freemium', 'Internal'];

export interface Category {
  id: number;
  name: string;
  description: string;
}

export interface ToolListItem {
  id: number;
  name: string;
  vendor: string;
  version: string;
  licenseType: LicenseType;
  categoryId: number;
  categoryName: string;
  logoUrl: string | null;
}

export interface ToolDepartmentLink {
  departmentId: number;
  departmentName: string;
  usageLevel: UsageLevel;
  referent: string | null;
  adoptedOn: string | null;
}

export interface ToolDetail {
  id: number;
  name: string;
  vendor: string;
  version: string;
  description: string;
  licenseType: LicenseType;
  documentationUrl: string | null;
  categoryId: number;
  categoryName: string;
  foundedYear: number | null;
  logoUrl: string | null;
  youtubeVideoUrl: string | null;
  availableVersions: string[];
  createdAt: string;
  updatedAt: string;
  departments: ToolDepartmentLink[];
}

export type ToolSortColumn = 'name' | 'vendor' | 'version' | 'category' | 'license';

export interface ToolQueryFilters {
  search?: string;
  categoryId?: number;
  licenseType?: LicenseType;
  sortBy: ToolSortColumn;
  descending: boolean;
  page: number;
  pageSize: number;
}
