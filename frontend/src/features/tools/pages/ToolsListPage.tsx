import { useCallback, useMemo, useRef, useState } from 'react';
import { useSearchParams } from 'react-router-dom';
import { Button } from '../../../shared/components/ui/Button';
import { EmptyState } from '../../../shared/components/ui/EmptyState';
import { ErrorState } from '../../../shared/components/ui/ErrorState';
import { Modal } from '../../../shared/components/ui/Modal';
import { Pagination } from '../../../shared/components/ui/Pagination';
import { Spinner } from '../../../shared/components/ui/Spinner';
import { ToolFilters } from '../components/ToolFilters';
import { ToolForm } from '../components/ToolForm';
import { ToolTable } from '../components/ToolTable';
import { useCreateTool } from '../hooks/useToolMutations';
import { useTools } from '../hooks/useTools';
import type { LicenseType, ToolQueryFilters, ToolSortColumn } from '../types';
import type { ToolApiInput } from '../types/toolFormSchema';

const PAGE_SIZE = 10;
const DEFAULT_SORT_COLUMN: ToolSortColumn = 'name';

export function ToolsListPage() {
  const [searchParams, setSearchParams] = useSearchParams();
  const [isCreateOpen, setIsCreateOpen] = useState(false);

  // React Router recreates `setSearchParams` on every navigation (it closes over the
  // current searchParams). Routing through a ref keeps `updateParams` — and everything
  // derived from it — stable, so effects that depend on it don't re-fire after navigating.
  const setSearchParamsRef = useRef(setSearchParams);
  setSearchParamsRef.current = setSearchParams;

  const filters: ToolQueryFilters = useMemo(
    () => ({
      search: searchParams.get('search') ?? undefined,
      categoryId: searchParams.get('categoryId') ? Number(searchParams.get('categoryId')) : undefined,
      licenseType: (searchParams.get('licenseType') as LicenseType | null) ?? undefined,
      sortBy: (searchParams.get('sortBy') as ToolSortColumn | null) ?? DEFAULT_SORT_COLUMN,
      descending: searchParams.get('descending') === 'true',
      page: searchParams.get('page') ? Number(searchParams.get('page')) : 1,
      pageSize: PAGE_SIZE,
    }),
    [searchParams],
  );

  const { data, isPending, isError, error, refetch } = useTools(filters);
  const createTool = useCreateTool();

  const updateParams = useCallback(
    (partial: Partial<ToolQueryFilters>) => {
      setSearchParamsRef.current((prev) => {
        const next = new URLSearchParams(prev);
        const current: ToolQueryFilters = {
          search: prev.get('search') ?? undefined,
          categoryId: prev.get('categoryId') ? Number(prev.get('categoryId')) : undefined,
          licenseType: (prev.get('licenseType') as LicenseType | null) ?? undefined,
          sortBy: (prev.get('sortBy') as ToolSortColumn | null) ?? DEFAULT_SORT_COLUMN,
          descending: prev.get('descending') === 'true',
          page: prev.get('page') ? Number(prev.get('page')) : 1,
          pageSize: PAGE_SIZE,
        };
        const merged = { ...current, ...partial };

        if (merged.search) next.set('search', merged.search);
        else next.delete('search');

        if (merged.categoryId) next.set('categoryId', String(merged.categoryId));
        else next.delete('categoryId');

        if (merged.licenseType) next.set('licenseType', merged.licenseType);
        else next.delete('licenseType');

        if (merged.sortBy !== DEFAULT_SORT_COLUMN) next.set('sortBy', merged.sortBy);
        else next.delete('sortBy');

        if (merged.descending) next.set('descending', 'true');
        else next.delete('descending');

        if (partial.page !== undefined) next.set('page', String(partial.page));
        else next.delete('page');

        return next;
      });
    },
    [],
  );

  const handleSearchChange = useCallback(
    (value: string) => updateParams({ search: value || undefined, page: 1 }),
    [updateParams],
  );
  const handleCategoryChange = useCallback(
    (value: number | undefined) => updateParams({ categoryId: value, page: 1 }),
    [updateParams],
  );
  const handleLicenseTypeChange = useCallback(
    (value: LicenseType | undefined) => updateParams({ licenseType: value, page: 1 }),
    [updateParams],
  );

  const handleSortChange = useCallback(
    (column: ToolSortColumn) => {
      setSearchParamsRef.current((prev) => {
        const next = new URLSearchParams(prev);
        const currentSortBy = (prev.get('sortBy') as ToolSortColumn | null) ?? DEFAULT_SORT_COLUMN;
        const currentDescending = prev.get('descending') === 'true';
        const nextDescending = currentSortBy === column ? !currentDescending : false;

        if (column !== DEFAULT_SORT_COLUMN) next.set('sortBy', column);
        else next.delete('sortBy');

        if (nextDescending) next.set('descending', 'true');
        else next.delete('descending');

        next.delete('page');

        return next;
      });
    },
    [],
  );

  function handleCreate(values: ToolApiInput) {
    createTool.mutate(values, { onSuccess: () => setIsCreateOpen(false) });
  }

  return (
    <div className="flex flex-col gap-4">
      <div className="flex items-center justify-between">
        <h1 className="text-xl font-semibold text-neutral-900 dark:text-neutral-100">Outils</h1>
        <Button onClick={() => setIsCreateOpen(true)}>Ajouter un outil</Button>
      </div>

      <ToolFilters
        search={filters.search ?? ''}
        categoryId={filters.categoryId}
        licenseType={filters.licenseType}
        onSearchChange={handleSearchChange}
        onCategoryChange={handleCategoryChange}
        onLicenseTypeChange={handleLicenseTypeChange}
      />

      {isPending ? <Spinner label="Loading tools" /> : null}

      {isError ? <ErrorState error={error} onRetry={() => refetch()} /> : null}

      {!isPending && !isError && data && data.items.length === 0 ? (
        <EmptyState title="Aucun outil ne correspond à ces filtres.">
          Essayez d'élargir votre recherche ou vos filtres.
        </EmptyState>
      ) : null}

      {!isPending && !isError && data && data.items.length > 0 ? (
        <>
          <ToolTable
            tools={data.items}
            sortBy={filters.sortBy}
            descending={filters.descending}
            onSortChange={handleSortChange}
          />
          <Pagination
            page={data.page}
            totalPages={data.totalPages}
            onPageChange={(page) => updateParams({ page })}
          />
        </>
      ) : null}

      {isCreateOpen ? (
        <Modal title="Ajouter un outil" onClose={() => setIsCreateOpen(false)}>
          <ToolForm
            submitLabel="Créer"
            isSubmitting={createTool.isPending}
            onSubmit={handleCreate}
          />
        </Modal>
      ) : null}
    </div>
  );
}
