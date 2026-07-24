import { useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { Badge } from '../../../shared/components/ui/Badge';
import { Button } from '../../../shared/components/ui/Button';
import { ErrorState } from '../../../shared/components/ui/ErrorState';
import { Modal } from '../../../shared/components/ui/Modal';
import { Spinner } from '../../../shared/components/ui/Spinner';
import { UsageLevelBadge } from '../../../shared/components/ui/UsageLevelBadge';
import { LicenseBadge } from '../components/LicenseBadge';
import { ToolForm } from '../components/ToolForm';
import { ToolLogo } from '../components/ToolLogo';
import { YoutubeEmbed } from '../components/YoutubeEmbed';
import { useTool } from '../hooks/useTool';
import { useDeleteTool, useUpdateTool } from '../hooks/useToolMutations';
import { availableVersionsToFormValue, type ToolApiInput } from '../types/toolFormSchema';

export function ToolDetailPage() {
  const { toolId } = useParams<{ toolId: string }>();
  const id = Number(toolId);
  const navigate = useNavigate();
  const [isEditOpen, setIsEditOpen] = useState(false);

  const { data: tool, isPending, isError, error, refetch } = useTool(id);
  const updateTool = useUpdateTool(id);
  const deleteTool = useDeleteTool();

  if (isPending) {
    return <Spinner label="Loading tool" />;
  }

  if (isError || !tool) {
    return <ErrorState error={error} onRetry={() => refetch()} />;
  }

  function handleUpdate(values: ToolApiInput) {
    updateTool.mutate(values, { onSuccess: () => setIsEditOpen(false) });
  }

  function handleDelete() {
    if (!window.confirm(`Supprimer « ${tool!.name} » ?`)) {
      return;
    }

    deleteTool.mutate(id, { onSuccess: () => navigate('/tools') });
  }

  return (
    <div className="flex flex-col gap-6">
      <div>
        <Link to="/tools" className="text-sm text-accent-600 hover:underline dark:text-accent-500">
          ← Retour aux outils
        </Link>
      </div>

      <div className="flex items-start justify-between">
        <div className="flex items-center gap-3">
          <ToolLogo name={tool.name} logoUrl={tool.logoUrl} size={48} />
          <div>
            <h1 className="text-xl font-semibold text-neutral-900 dark:text-neutral-100">{tool.name}</h1>
            <p className="mt-1 text-sm text-neutral-600 dark:text-neutral-400">
              {tool.vendor} · v{tool.version}
              {tool.foundedYear ? ` · créé en ${tool.foundedYear}` : ''}
            </p>
          </div>
        </div>
        <div className="flex gap-2">
          <Button variant="secondary" onClick={() => setIsEditOpen(true)}>
            Modifier
          </Button>
          <Button variant="danger" onClick={handleDelete} disabled={deleteTool.isPending}>
            Supprimer
          </Button>
        </div>
      </div>

      <div className="flex flex-wrap gap-2">
        <LicenseBadge licenseType={tool.licenseType} />
        <Badge tone="neutral">{tool.categoryName}</Badge>
      </div>

      <p className="max-w-2xl text-sm leading-relaxed text-neutral-700 dark:text-neutral-300">
        {tool.description}
      </p>

      {tool.documentationUrl ? (
        <a
          href={tool.documentationUrl}
          target="_blank"
          rel="noreferrer"
          className="w-fit text-sm text-accent-600 hover:underline dark:text-accent-500"
        >
          Documentation officielle ↗
        </a>
      ) : null}

      {tool.youtubeVideoUrl ? (
        <YoutubeEmbed url={tool.youtubeVideoUrl} title={`Présentation de ${tool.name}`} />
      ) : null}

      {tool.availableVersions.length > 0 ? (
        <section>
          <h2 className="mb-2 text-sm font-semibold text-neutral-900 dark:text-neutral-100">
            Versions disponibles
          </h2>
          <div className="flex flex-wrap gap-2">
            {tool.availableVersions.map((version) => (
              <Badge key={version} tone="blue">
                {version}
              </Badge>
            ))}
          </div>
        </section>
      ) : null}

      <section>
        <h2 className="mb-2 text-sm font-semibold text-neutral-900 dark:text-neutral-100">
          Départements utilisateurs
        </h2>
        {tool.departments.length === 0 ? (
          <p className="text-sm text-neutral-500 dark:text-neutral-400">
            Aucun département n'utilise encore cet outil.
          </p>
        ) : (
          <ul className="flex flex-col gap-2">
            {tool.departments.map((link) => (
              <li
                key={link.departmentId}
                className="flex items-center justify-between rounded-md border border-neutral-200 px-3 py-2 text-sm dark:border-neutral-800"
              >
                <Link
                  to={`/departments/${link.departmentId}`}
                  className="font-medium text-accent-600 hover:underline dark:text-accent-500"
                >
                  {link.departmentName}
                </Link>
                <div className="flex items-center gap-2 text-neutral-600 dark:text-neutral-400">
                  {link.referent ? <span>{link.referent}</span> : null}
                  <UsageLevelBadge usageLevel={link.usageLevel} />
                </div>
              </li>
            ))}
          </ul>
        )}
      </section>

      {isEditOpen ? (
        <Modal title="Modifier l'outil" onClose={() => setIsEditOpen(false)}>
          <ToolForm
            submitLabel="Enregistrer"
            isSubmitting={updateTool.isPending}
            onSubmit={handleUpdate}
            defaultValues={{
              name: tool.name,
              vendor: tool.vendor,
              version: tool.version,
              description: tool.description,
              licenseType: tool.licenseType,
              documentationUrl: tool.documentationUrl ?? '',
              categoryId: tool.categoryId,
              foundedYear: tool.foundedYear ?? '',
              logoUrl: tool.logoUrl ?? '',
              youtubeVideoUrl: tool.youtubeVideoUrl ?? '',
              availableVersions: availableVersionsToFormValue(tool.availableVersions),
            }}
          />
        </Modal>
      ) : null}
    </div>
  );
}
