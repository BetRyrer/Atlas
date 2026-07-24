import type { UsageLevel } from '../../types/usageLevel';
import { usageLevelLabels } from '../../types/usageLevel';
import { Badge } from './Badge';

const toneByUsageLevel: Record<UsageLevel, 'green' | 'blue' | 'amber'> = {
  Primary: 'green',
  Secondary: 'blue',
  Evaluating: 'amber',
};

export function UsageLevelBadge({ usageLevel }: { usageLevel: UsageLevel }) {
  return <Badge tone={toneByUsageLevel[usageLevel]}>{usageLevelLabels[usageLevel]}</Badge>;
}
