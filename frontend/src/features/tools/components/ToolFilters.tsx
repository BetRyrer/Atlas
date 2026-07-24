import { useEffect, useState } from 'react';
import { Input } from '../../../shared/components/ui/Input';
import { Select } from '../../../shared/components/ui/Select';
import { useDebounce } from '../../../shared/hooks/useDebounce';
import { useCategories } from '../hooks/useCategories';
import { licenseTypes, type LicenseType } from '../types';

interface ToolFiltersProps {
  search: string;
  categoryId: number | undefined;
  licenseType: LicenseType | undefined;
  onSearchChange: (value: string) => void;
  onCategoryChange: (value: number | undefined) => void;
  onLicenseTypeChange: (value: LicenseType | undefined) => void;
}

export function ToolFilters({
  search,
  categoryId,
  licenseType,
  onSearchChange,
  onCategoryChange,
  onLicenseTypeChange,
}: ToolFiltersProps) {
  const [searchInput, setSearchInput] = useState(search);
  const debouncedSearch = useDebounce(searchInput, 300);
  const { data: categories } = useCategories();

  useEffect(() => {
    onSearchChange(debouncedSearch);
  }, [debouncedSearch, onSearchChange]);

  return (
    <div className="grid grid-cols-1 gap-3 sm:grid-cols-3">
      <Input
        label="Recherche"
        id="search"
        name="search"
        placeholder="Nom ou éditeur…"
        value={searchInput}
        onChange={(event) => setSearchInput(event.target.value)}
      />
      <Select
        label="Catégorie"
        id="categoryId"
        name="categoryId"
        value={categoryId ?? ''}
        onChange={(event) =>
          onCategoryChange(event.target.value ? Number(event.target.value) : undefined)
        }
      >
        <option value="">Toutes les catégories</option>
        {categories?.map((category) => (
          <option key={category.id} value={category.id}>
            {category.name}
          </option>
        ))}
      </Select>
      <Select
        label="Licence"
        id="licenseType"
        name="licenseType"
        value={licenseType ?? ''}
        onChange={(event) =>
          onLicenseTypeChange((event.target.value || undefined) as LicenseType | undefined)
        }
      >
        <option value="">Toutes les licences</option>
        {licenseTypes.map((type) => (
          <option key={type} value={type}>
            {type}
          </option>
        ))}
      </Select>
    </div>
  );
}
