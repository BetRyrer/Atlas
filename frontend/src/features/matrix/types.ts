import type { UsageLevel } from '../../shared/types/usageLevel';

export interface MatrixTool {
  toolId: number;
  toolName: string;
}

export interface MatrixCell {
  toolId: number;
  usageLevel: UsageLevel | null;
}

export interface MatrixRow {
  departmentId: number;
  departmentName: string;
  cells: MatrixCell[];
}

export interface Matrix {
  tools: MatrixTool[];
  rows: MatrixRow[];
}
