import { useState } from 'react';
import { Link, useParams } from 'react-router-dom';
import { Button } from '../../../shared/components/ui/Button';
import { EmptyState } from '../../../shared/components/ui/EmptyState';
import { ErrorState } from '../../../shared/components/ui/ErrorState';
import { Modal } from '../../../shared/components/ui/Modal';
import { Spinner } from '../../../shared/components/ui/Spinner';
import { DepartmentToolsTable } from '../components/DepartmentToolsTable';
import { LinkToolForm } from '../components/LinkToolForm';
import { useDepartment } from '../hooks/useDepartment';
import { useLinkTool, useUnlinkTool } from '../hooks/useDepartmentToolMutations';
import type { LinkToolFormSchema } from '../types/linkToolFormSchema';

export function DepartmentDetailPage() {
  const { departmentId } = useParams<{ departmentId: string }>();
  const id = Number(departmentId);
  const [isLinkOpen, setIsLinkOpen] = useState(false);

  const { data: department, isPending, isError, error, refetch } = useDepartment(id);
  const linkTool = useLinkTool(id);
  const unlinkTool = useUnlinkTool(id);

  if (isPending) {
    return <Spinner label="Loading department" />;
  }

  if (isError || !department) {
    return <ErrorState error={error} onRetry={() => refetch()} />;
  }

  function handleLink(values: LinkToolFormSchema, toolName: string) {
    linkTool.mutate(
      {
        toolId: values.toolId,
        usageLevel: values.usageLevel,
        referent: values.referent || undefined,
        adoptedOn: values.adoptedOn || undefined,
        toolName,
      },
      { onSuccess: () => setIsLinkOpen(false) },
    );
  }

  return (
    <div className="flex flex-col gap-6">
      <div>
        <Link to="/departments" className="text-sm text-accent-600 hover:underline dark:text-accent-500">
          ← Retour aux départements
        </Link>
      </div>

      <div className="flex items-start justify-between">
        <div>
          <h1 className="text-xl font-semibold text-neutral-900 dark:text-neutral-100">{department.name}</h1>
          <p className="mt-1 text-sm text-neutral-600 dark:text-neutral-400">{department.description}</p>
          <p className="mt-1 text-xs font-medium text-neutral-500 dark:text-neutral-500">
            {department.headCount} personnes
          </p>
        </div>
        <Button onClick={() => setIsLinkOpen(true)}>Lier un outil</Button>
      </div>

      {department.tools.length === 0 ? (
        <EmptyState title="Aucun outil lié à ce département.">
          Utilisez « Lier un outil » pour commencer.
        </EmptyState>
      ) : (
        <DepartmentToolsTable
          tools={department.tools}
          onUnlink={(toolId) => unlinkTool.mutate(toolId)}
          isUnlinking={unlinkTool.isPending}
        />
      )}

      {isLinkOpen ? (
        <Modal title="Lier un outil" onClose={() => setIsLinkOpen(false)}>
          <LinkToolForm isSubmitting={linkTool.isPending} onSubmit={handleLink} />
        </Modal>
      ) : null}
    </div>
  );
}
