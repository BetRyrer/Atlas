<div align="center">

<p>
  <img src="https://cdn.simpleicons.org/dotnet" width="36" height="36" alt=".NET" />
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/csharp/csharp-original.svg" width="36" height="36" alt="C#" />
  <img src="https://cdn.simpleicons.org/sqlite" width="36" height="36" alt="SQLite" />
  <img src="https://cdn.simpleicons.org/jsonwebtokens" width="36" height="36" alt="JWT" />
  &nbsp;&nbsp;
  <img src="https://cdn.simpleicons.org/react" width="36" height="36" alt="React" />
  <img src="https://cdn.simpleicons.org/typescript" width="36" height="36" alt="TypeScript" />
  <img src="https://cdn.simpleicons.org/vite" width="36" height="36" alt="Vite" />
  <img src="https://cdn.simpleicons.org/tailwindcss" width="36" height="36" alt="Tailwind CSS" />
  &nbsp;&nbsp;
  <img src="https://cdn.simpleicons.org/docker" width="36" height="36" alt="Docker" />
</p>

# Atlas

### Catalogue d'outillage technique

</div>

Application interne permettant de recenser les outils logiciels utilisés par
les différents services d'une organisation (Dev Back, Dev Front, QA, Ops,
Data, Sécurité), avec une matrice de couverture départements × outils.

Projet réalisé dans le cadre d'une candidature en alternance, avec une
attention particulière portée à la qualité du C# côté back-end. Toutes les
données (départements, outils, éditeurs) sont fictives ou réutilisées à des
fins d'illustration.

<div align="center">

![Démo Atlas](docs/screenshots/demo.gif)

*Connexion → tableau de bord → recherche/tri/pagination des outils → détail
d'un outil → départements → matrice de couverture.*

</div>

## Sommaire

- [Fonctionnalités](#fonctionnalités)
- [Stack technique](#stack-technique)
- [Prérequis](#prérequis)
- [Lancement en local](#lancement-en-local)
- [Lancement via Docker](#lancement-via-docker)
- [Authentification](#authentification)
- [Architecture](#architecture)
- [Choix techniques](#choix-techniques)
- [Vers la production](#vers-la-production)

## Fonctionnalités

- **Catalogue d'outils** : liste paginée, recherche, filtres (catégorie, type
  de licence), tri par colonne ; fiche détaillée par outil (description,
  année de création, logo, versions disponibles, licence, documentation
  officielle, vidéo de présentation YouTube, départements utilisateurs).
- **Départements** : fiche par département avec la liste des outils liés
  (niveau d'usage, référent, date d'adoption), liaison/déliaison d'un outil
  depuis l'interface (mutations optimistes avec rollback en cas d'erreur).
- **Matrice de couverture** : grille départements × outils avec cellules
  colorées par niveau d'usage.
- **Tableau de bord** : indicateurs clés (nombre d'outils, de départements,
  de catégories), répartition par type de licence, sert aussi de menu de
  navigation.
- **Authentification JWT** : connexion, session persistée, déconnexion
  automatique sur jeton expiré/invalide.
- **Notifications** : retour visuel (toasts) sur chaque création, modification,
  liaison ou suppression.
- **Page 404** dédiée pour toute route inconnue.

## Stack technique

**Back-end** : .NET 8, ASP.NET Core Web API, EF Core 8 (SQLite), AutoMapper,
FluentValidation, JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`),
Serilog, Swashbuckle.

**Front-end** : Vite, React 18, TypeScript strict, TanStack Query v5, React
Router v6, Tailwind CSS v4, Axios, React Hook Form + Zod.

## Prérequis

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 20+](https://nodejs.org) et npm
- Docker et Docker Compose (optionnel, pour le lancement conteneurisé)

## Lancement en local

### Back-end

```bash
cd backend/Atlas.Api
dotnet run
```

L'API démarre sur `http://localhost:5101` (profil `http` de
`launchSettings.json`). En environnement `Development`, elle applique
automatiquement les migrations EF Core et **seed** la base SQLite (6
départements, 8 catégories, 40 outils, ~70 liaisons départements↔outils) au
démarrage. Swagger est disponible sur `/swagger`.

### Front-end

```bash
cd frontend
npm install
npm run dev
```

L'application est servie sur `http://localhost:5173`. La variable
`VITE_API_BASE_URL` (voir `frontend/.env.example`) doit pointer vers l'URL de
l'API — par défaut `http://localhost:5101/api`, cohérente avec la CORS
`Cors:FrontendOrigin` définie côté API sur `http://localhost:5173`.

Connectez-vous avec le compte de démonstration : **`admin` / `Atlas2024!`**.

## Lancement via Docker

```bash
cp .env.example .env
docker compose up --build
```

- API exposée sur `http://localhost:8080` (health check sur `/health`).
- Front-end servi par nginx sur `http://localhost:5173`.
- La base SQLite est persistée dans le volume nommé `atlas-data`.
- `JWT_SECRET` est **requis** dans `.env` (`docker compose` refuse de démarrer
  sans) — `.env.example` fournit une valeur de démo, à régénérer pour un
  usage réel (`openssl rand -base64 48`).

En environnement `Production` (celui utilisé par Docker Compose par défaut),
les migrations EF Core sont appliquées au démarrage et le compte de
démonstration `admin` est toujours créé s'il n'existe pas encore — sans quoi
une instance Docker fraîche serait impossible à explorer. En revanche, **le
catalogue d'exemple (départements, catégories, outils) reste réservé au
développement local** : une base Docker fraîche démarre donc avec un
catalogue vide, prêt à recevoir de vraies données.

## Authentification

L'API est protégée par défaut (politique d'autorisation globale, `[AllowAnonymous]`
uniquement sur `POST /api/auth/login`) : toute route nécessite un jeton JWT
Bearer obtenu via login. Le mot de passe est haché en PBKDF2/SHA-256 (BCL pure,
sans dépendance externe) ; le jeton est signé en HMAC-SHA256 avec une clé
définie par `Jwt:Secret`.

Côté front, le jeton est conservé dans `localStorage`, injecté automatiquement
sur chaque requête par un intercepteur Axios, et une réponse 401 déclenche un
événement global qui déconnecte l'utilisateur et le renvoie vers `/login` (en
conservant la page initialement demandée pour y revenir après connexion).

Après connexion, l'utilisateur arrive sur un **tableau de bord** (`/dashboard`)
qui sert à la fois de page d'accueil et de menu : indicateurs clés (nombre
d'outils, de départements, de catégories), répartition par type de licence,
et cartes de navigation vers les autres sections.

## Architecture

### Back-end — Clean Architecture

```
backend/Atlas.Domain           (aucune dépendance externe)
backend/Atlas.Application      (→ Domain)
backend/Atlas.Infrastructure   (→ Application, Domain)
backend/Atlas.Api              (→ Application, Infrastructure)
```

- **Domain** : entités (`Tool`, `Category`, `Department`, `DepartmentTool`)
  avec setters privés, modifiées via des méthodes métier explicites
  (`Tool.UpdateVersion`, `Tool.LinkTo`, `Tool.Unlink`...).
- **Application** : DTOs séparés par intention (`ToolListDto` /
  `ToolDetailDto` / `CreateToolDto` / `UpdateToolDto`), interfaces de
  persistance (`IToolRepository`, `IDepartmentRepository`,
  `ICategoryRepository`, `IUnitOfWork`), services métier, validators
  FluentValidation, profils AutoMapper — regroupés par feature
  (`Tools/`, `Departments/`, `Matrix/`, `Categories/`, `Auth/`).
- **Infrastructure** : `AppDbContext`, configurations EF Core par entité
  (`IEntityTypeConfiguration<T>`), repositories, `DatabaseSeeder`,
  `Security/` (hachage de mot de passe, génération du JWT).
- **Api** : contrôleurs fins (validation déléguée à un `IAsyncActionFilter`
  qui exécute FluentValidation avant l'action, appel au service, retour du
  status code), middleware global traduisant les exceptions métier en
  `ProblemDetails` RFC 7807, authentification JWT Bearer appliquée par
  défaut à tous les contrôleurs.

### Front-end — feature-based

```
src/app/            App, router (lazy + Suspense), ProtectedRoute, providers
src/features/auth/            page de connexion
src/features/dashboard/       tableau de bord / menu (page d'accueil)
src/features/tools/           pages, components, hooks, services, types
src/features/departments/     pages, components, hooks, services, types
src/features/matrix/          pages, components, hooks
src/shared/          api (client Axios), auth (contexte JWT), components/ui, layout
```

Une feature n'importe jamais l'intérieur d'une autre — uniquement son
`index.ts`. Les composants sont purement présentationnels et consomment des
hooks TanStack Query ; les appels HTTP vivent exclusivement dans `services/`.

## Choix techniques

**Pourquoi Clean Architecture ?** Le Domain ne dépend d'aucun package
externe : les règles métier (liaison département↔outil, invariants sur les
entités) restent exprimées et vérifiables sans base de données ni framework
web — la couche Application ne dépend que d'interfaces (`IToolRepository`,
`IUnitOfWork`...), jamais d'EF Core directement.

**Pourquoi une entité de jointure porteuse de données (`DepartmentTool`) ?**
La relation département↔outil transporte de l'information propre à la
relation elle-même (niveau d'usage, référent, date d'adoption) — elle n'est
pas une simple table de liaison mais un concept métier à part entière, donc
une entité avec sa propre clé composite plutôt qu'une collection implicite.

**Pourquoi la projection `ProjectTo` d'AutoMapper ?** Les endpoints de
lecture (listes paginées, détails, matrice) n'ont besoin que des champs
exposés par leur DTO. `ProjectTo` traduit le mapping en expression LINQ
exécutée par EF Core *dans la requête SQL* — on évite de charger les entités
complètes (et leurs navigations) juste pour les jeter après mapping.

**Pourquoi TanStack Query plutôt qu'un state manager global ?** L'essentiel
de l'état de cette application est de l'état serveur (listes, détails,
matrice), pas de l'état UI partagé entre composants distants. TanStack Query
gère le cache, l'invalidation ciblée après mutation, les mutations
optimistes avec rollback et les états de chargement/erreur sans qu'il soit
nécessaire de dupliquer cet état dans un store type Redux/Zustand.

## Vers la production

Ce dépôt prépare une mise en production sans la réaliser entièrement. Ce qui
est déjà en place :

- Configuration par environnement (`appsettings.Production.json` avec logs
  Serilog structurés en JSON).
- Health check `/health` (`AspNetCore.HealthChecks.Sqlite`), utilisé par le
  `healthcheck` Docker Compose.
- Dockerfiles multi-stage non-root (API) et build Vite → nginx (front),
  volume nommé pour la persistance SQLite.

Ce qu'un déploiement réel nécessiterait en plus :

- **Base de données** : migrer de SQLite vers PostgreSQL ou SQL Server pour
  la concurrence en écriture et la réplication — l'abstraction
  `IToolRepository` / `IUnitOfWork` limite l'impact du changement de
  provider EF Core à `Atlas.Infrastructure`.
- **Secrets** : sortir les chaînes de connexion, origines CORS et surtout
  `Jwt:Secret` des fichiers `appsettings*.json` vers un gestionnaire de
  secrets (Azure Key Vault, variables d'environnement injectées par la
  plateforme) — la valeur commitée n'est qu'une démo.
- **HTTPS** : terminer TLS en amont (reverse proxy / ingress) plutôt que
  dans les conteneurs applicatifs.
- **CI/CD** : pipeline exécutant `dotnet build -warnaserror`, `npm run
  build`, build et push des images Docker, puis déploiement.
- **Données de référence** : seul le compte `admin` de démonstration est créé
  en production (`SeedAdminUserAsync`, idempotent) ; le catalogue d'exemple
  (`SeedCatalogAsync`) reste réservé au développement. Un environnement de
  production a besoin d'un processus de seed contrôlé (migration de données)
  pour les catégories et départements réels, et d'une vraie gestion des
  comptes (le login actuel n'a pas d'écran d'inscription).

## Licence AutoMapper

Ce projet utilise AutoMapper 16.1.1, patché contre la CVE de déni de service
par récursion incontrôlée (GHSA-rvv3-g6hj-g44x). Les versions récentes
d'AutoMapper nécessitent une licence commerciale au-delà d'un usage
développement/test/projet personnel — c'est le cas ici.
