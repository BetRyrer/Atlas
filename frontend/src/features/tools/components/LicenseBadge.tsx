import { Badge } from '../../../shared/components/ui/Badge';
import type { LicenseType } from '../types';

const toneByLicense: Record<LicenseType, 'purple' | 'green' | 'blue' | 'neutral'> = {
  Proprietary: 'purple',
  OpenSource: 'green',
  Freemium: 'blue',
  Internal: 'neutral',
};

export function LicenseBadge({ licenseType }: { licenseType: LicenseType }) {
  return <Badge tone={toneByLicense[licenseType]}>{licenseType}</Badge>;
}
