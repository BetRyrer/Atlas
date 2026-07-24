using Atlas.Application.Auth;
using Atlas.Domain.Entities;
using Atlas.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAdminUserAsync(AppDbContext dbContext, IPasswordHasher passwordHasher)
    {
        if (await dbContext.Users.AnyAsync())
        {
            return;
        }

        await dbContext.Users.AddRangeAsync(CreateUsers(passwordHasher));
        await dbContext.SaveChangesAsync();
    }

    public static async Task SeedCatalogAsync(AppDbContext dbContext)
    {
        if (await dbContext.Categories.AnyAsync())
        {
            return;
        }

        var categories = CreateCategories();
        await dbContext.Categories.AddRangeAsync(categories);

        var departments = CreateDepartments();
        await dbContext.Departments.AddRangeAsync(departments);

        await dbContext.SaveChangesAsync();

        var categoriesByName = categories.ToDictionary(category => category.Name);
        var tools = CreateTools(categoriesByName);
        await dbContext.Tools.AddRangeAsync(tools);

        await dbContext.SaveChangesAsync();

        var departmentsByName = departments.ToDictionary(department => department.Name);
        var toolsByName = tools.ToDictionary(tool => tool.Name);
        LinkDepartmentsToTools(departmentsByName, toolsByName);

        await dbContext.SaveChangesAsync();
    }

    private static List<User> CreateUsers(IPasswordHasher passwordHasher) =>
    [
        new User("admin", "Camille Dubois", passwordHasher.Hash("Atlas2024!"))
    ];

    private static List<Category> CreateCategories() =>
    [
        new("Version Control", "Source code management and collaboration."),
        new("CI/CD", "Build, test and deployment pipelines."),
        new("Project Management", "Planning, tracking and team collaboration."),
        new("Monitoring & Observability", "Metrics, logs and application health."),
        new("Containerization & Infrastructure", "Container orchestration and infrastructure as code."),
        new("Database", "Relational and non-relational data stores."),
        new("Design", "UI/UX design and prototyping."),
        new("Developer Tools", "IDEs, editors and API clients.")
    ];

    private static List<Department> CreateDepartments() =>
    [
        new("Développement Back-End", "Conception et maintenance des services et API internes.", 12),
        new("Développement Front-End", "Conception des interfaces utilisateur web et mobile.", 9),
        new("Assurance Qualité", "Tests fonctionnels, non-régression et validation des livraisons.", 6),
        new("Opérations", "Exploitation, supervision et infrastructure de production.", 5),
        new("Data & Analytics", "Ingestion, modélisation et restitution des données.", 7),
        new("Sécurité", "Audit, conformité et sécurisation du système d'information.", 4)
    ];

    private static List<Tool> CreateTools(IReadOnlyDictionary<string, Category> categories)
    {
        var versionControl = categories["Version Control"].Id;
        var ciCd = categories["CI/CD"].Id;
        var projectManagement = categories["Project Management"].Id;
        var monitoring = categories["Monitoring & Observability"].Id;
        var infrastructure = categories["Containerization & Infrastructure"].Id;
        var database = categories["Database"].Id;
        var design = categories["Design"].Id;
        var developerTools = categories["Developer Tools"].Id;

        return
        [
            new Tool(
                "Git",
                "Software Freedom Conservancy",
                "2.45",
                "Git is a free and open-source distributed version control system that tracks changes in source code during software development. It lets multiple developers work on the same codebase simultaneously through branching and merging, while keeping a complete history of every change. It underpins most modern software collaboration workflows, including pull requests and code review.",
                LicenseType.OpenSource,
                "https://git-scm.com/doc",
                versionControl,
                foundedYear: 2005,
                logoUrl: Logo("git"),
                availableVersions: ["2.43", "2.44", "2.45"],
                youtubeVideoUrl: Youtube("xQujH0ElTUg")),
            new Tool(
                "GitHub",
                "GitHub Inc.",
                "N/A",
                "GitHub is a cloud-based platform for hosting Git repositories, built around collaborative software development. It adds pull requests, code review, issue tracking, and CI/CD automation through GitHub Actions on top of Git. It is the most widely used platform for open-source and enterprise software collaboration.",
                LicenseType.Freemium,
                "https://docs.github.com",
                versionControl,
                foundedYear: 2008,
                logoUrl: Logo("github"),
                youtubeVideoUrl: Youtube("pBy1zgt0XPc")),
            new Tool(
                "GitLab",
                "GitLab Inc.",
                "17.1",
                "GitLab is a complete DevOps platform that combines Git repository hosting with built-in CI/CD pipelines, issue tracking, and container registries in a single application. It can be used as a managed cloud service or self-hosted for organizations with strict data governance needs. Its integrated pipeline features make it a common alternative to assembling separate tools for source control and automation.",
                LicenseType.Freemium,
                "https://docs.gitlab.com",
                versionControl,
                foundedYear: 2011,
                logoUrl: Logo("gitlab"),
                availableVersions: ["16.11", "17.0", "17.1"],
                youtubeVideoUrl: Youtube("Jve98tlZ394")),
            new Tool(
                "Bitbucket",
                "Atlassian",
                "N/A",
                "Bitbucket is Atlassian's Git repository hosting service, tightly integrated with Jira and Confluence for teams already using the Atlassian suite. It provides pull requests, code review, and built-in pipelines for continuous integration. It is popular among teams that want native integration between issue tracking and source control.",
                LicenseType.Freemium,
                "https://support.atlassian.com/bitbucket-cloud",
                versionControl,
                foundedYear: 2008,
                logoUrl: Logo("bitbucket"),
                youtubeVideoUrl: Youtube("BR79iEFLHsQ")),

            new Tool(
                "Jenkins",
                "Jenkins Community",
                "2.452",
                "Jenkins is a free, open-source automation server used to build, test, and deploy software through configurable pipelines. Its plugin ecosystem, one of the largest in the CI/CD space, lets it integrate with virtually any tool in the software delivery chain. It remains a common choice for teams needing full control over their build infrastructure.",
                LicenseType.OpenSource,
                "https://www.jenkins.io/doc",
                ciCd,
                foundedYear: 2011,
                logoUrl: Logo("jenkins"),
                availableVersions: ["2.426 LTS", "2.452", "2.462"],
                youtubeVideoUrl: Youtube("ecTqBWP4k88")),
            new Tool(
                "GitLab CI",
                "GitLab Inc.",
                "17.1",
                "GitLab CI is the continuous integration and delivery engine built into GitLab, defined through a YAML pipeline file stored alongside the code. It runs automated tests, builds, and deployments triggered by every commit or merge request. Being native to GitLab, it avoids the need to wire up a separate CI system.",
                LicenseType.Freemium,
                "https://docs.gitlab.com/ee/ci",
                ciCd,
                foundedYear: 2012,
                logoUrl: Logo("gitlab"),
                youtubeVideoUrl: Youtube("Jve98tlZ394")),
            new Tool(
                "GitHub Actions",
                "GitHub Inc.",
                "N/A",
                "GitHub Actions is GitHub's native automation platform for building CI/CD pipelines directly from a repository. Workflows are defined as YAML files and triggered by repository events such as pushes or pull requests, drawing from a large marketplace of reusable actions. It removed the need for many teams to run a separate CI server.",
                LicenseType.Freemium,
                "https://docs.github.com/actions",
                ciCd,
                foundedYear: 2019,
                logoUrl: Logo("githubactions"),
                youtubeVideoUrl: Youtube("WQMz0AnJ6uU")),
            new Tool(
                "CircleCI",
                "Circle Internet Services",
                "N/A",
                "CircleCI is a cloud-based continuous integration and delivery platform that automates build, test, and deployment pipelines. It emphasizes fast pipeline execution through caching and parallelism, and integrates with most major Git hosting providers. Many teams use it as a fully managed alternative to self-hosted CI servers.",
                LicenseType.Freemium,
                "https://circleci.com/docs",
                ciCd,
                foundedYear: 2011,
                logoUrl: Logo("circleci"),
                youtubeVideoUrl: Youtube("dLdy3WUizAI")),
            new Tool(
                "Azure DevOps",
                "Microsoft",
                "N/A",
                "Azure DevOps is Microsoft's suite of development tools covering repositories, pipelines, boards, test plans, and artifact management in one product. It is commonly used by organizations already invested in the Microsoft ecosystem to manage the full software delivery lifecycle. Azure Pipelines, its CI/CD component, supports both cloud and on-premises build agents.",
                LicenseType.Proprietary,
                "https://learn.microsoft.com/azure/devops",
                ciCd,
                foundedYear: 2018,
                logoUrl: DeviconLogo("azure/azure-original"),
                youtubeVideoUrl: Youtube("JhqpF-5E10I")),
            new Tool(
                "SonarQube",
                "SonarSource",
                "10.5",
                "SonarQube is a static code analysis platform that continuously inspects code quality and security vulnerabilities across dozens of programming languages. It is typically wired into CI/CD pipelines as a quality gate that can block merges when code fails to meet defined standards. Teams use it to track technical debt and enforce coding standards over time.",
                LicenseType.Freemium,
                "https://docs.sonarsource.com",
                ciCd,
                foundedYear: 2008,
                logoUrl: Logo("sonarqubeserver"),
                availableVersions: ["9.9 LTS", "10.4", "10.5"],
                youtubeVideoUrl: Youtube("xeTwG9XFFTE")),

            new Tool(
                "Jira",
                "Atlassian",
                "N/A",
                "Jira is Atlassian's issue tracking and agile project management tool, widely used to plan and track work using Scrum or Kanban boards. It supports customizable workflows, backlogs, sprints, and reporting for software teams of any size. It has become a de facto standard for agile software development tracking.",
                LicenseType.Proprietary,
                "https://support.atlassian.com/jira-software-cloud",
                projectManagement,
                foundedYear: 2002,
                logoUrl: Logo("jira"),
                youtubeVideoUrl: Youtube("Z-a1RB9HvDI")),
            new Tool(
                "Confluence",
                "Atlassian",
                "N/A",
                "Confluence is Atlassian's team documentation and knowledge base tool, used to write and organize technical documentation, meeting notes, and product specifications. It integrates closely with Jira, letting teams link documentation directly to the work items it describes. It is commonly the central knowledge repository for engineering organizations.",
                LicenseType.Proprietary,
                "https://support.atlassian.com/confluence-cloud",
                projectManagement,
                foundedYear: 2004,
                logoUrl: Logo("confluence"),
                youtubeVideoUrl: Youtube("ttBOaCNIEUk")),
            new Tool(
                "Trello",
                "Atlassian",
                "N/A",
                "Trello is a lightweight, visual task management tool based on Kanban-style boards, lists, and cards. Its simplicity makes it popular for smaller teams or individual task tracking that doesn't need the full complexity of a dedicated agile tool like Jira. Automation rules and power-ups extend it for more structured workflows.",
                LicenseType.Freemium,
                "https://support.atlassian.com/trello",
                projectManagement,
                foundedYear: 2011,
                logoUrl: Logo("trello"),
                youtubeVideoUrl: Youtube("ESshFQ-8804")),
            new Tool(
                "Notion",
                "Notion Labs",
                "N/A",
                "Notion is an all-in-one workspace combining notes, documents, wikis, and lightweight project tracking in a single flexible tool. Its block-based editor and database views let teams build custom trackers, knowledge bases, and dashboards without dedicated software. It has become popular as a general-purpose alternative to juggling several separate tools.",
                LicenseType.Freemium,
                "https://www.notion.so/help",
                projectManagement,
                foundedYear: 2016,
                logoUrl: Logo("notion"),
                youtubeVideoUrl: Youtube("Vicx5Kz6hs4")),

            new Tool(
                "Grafana",
                "Grafana Labs",
                "11.1",
                "Grafana is an open-source platform for querying, visualizing, and alerting on metrics and logs from a wide range of data sources such as Prometheus, Elasticsearch, or SQL databases. It is the standard front-end for building operational dashboards in modern observability stacks. Teams use it to monitor system health and diagnose incidents in real time.",
                LicenseType.OpenSource,
                "https://grafana.com/docs",
                monitoring,
                foundedYear: 2014,
                logoUrl: Logo("grafana"),
                availableVersions: ["10.4", "11.0", "11.1"],
                youtubeVideoUrl: Youtube("lILY8eSspEo")),
            new Tool(
                "Prometheus",
                "Prometheus Authors",
                "2.53",
                "Prometheus is an open-source monitoring and alerting toolkit that collects and stores metrics as time-series data, originally built at SoundCloud. Its pull-based model and powerful query language (PromQL) make it the de facto standard for monitoring cloud-native and Kubernetes environments. It is frequently paired with Grafana for visualization.",
                LicenseType.OpenSource,
                "https://prometheus.io/docs",
                monitoring,
                foundedYear: 2012,
                logoUrl: Logo("prometheus"),
                availableVersions: ["2.51", "2.52", "2.53"],
                youtubeVideoUrl: Youtube("STVMGrYIlfg")),
            new Tool(
                "Datadog",
                "Datadog Inc.",
                "N/A",
                "Datadog is a cloud-based monitoring and observability platform that unifies infrastructure metrics, application performance monitoring, and log management in one product. It provides pre-built integrations for hundreds of cloud services, making it fast to deploy across complex environments. Many organizations use it as a single pane of glass for production monitoring.",
                LicenseType.Proprietary,
                "https://docs.datadoghq.com",
                monitoring,
                foundedYear: 2010,
                logoUrl: Logo("datadog"),
                youtubeVideoUrl: Youtube("YmJcbAI_OCg")),
            new Tool(
                "New Relic",
                "New Relic Inc.",
                "N/A",
                "New Relic is an application performance monitoring platform that helps teams trace requests, diagnose slow code paths, and monitor infrastructure health. Its distributed tracing capabilities are especially useful for debugging performance issues across microservices architectures. It is commonly used alongside or instead of building custom monitoring pipelines.",
                LicenseType.Proprietary,
                "https://docs.newrelic.com",
                monitoring,
                foundedYear: 2008,
                logoUrl: Logo("newrelic"),
                youtubeVideoUrl: Youtube("-KYbAk4_wMs")),
            new Tool(
                "Kibana",
                "Elastic",
                "8.14",
                "Kibana is the visualization and exploration layer of the Elastic Stack, used to search, analyze, and build dashboards over data stored in Elasticsearch. It is particularly common for log analysis and full-text search use cases at scale. Teams use it to investigate incidents and monitor application and infrastructure logs.",
                LicenseType.OpenSource,
                "https://www.elastic.co/guide/kibana",
                monitoring,
                foundedYear: 2013,
                logoUrl: Logo("kibana"),
                availableVersions: ["8.12", "8.13", "8.14"],
                youtubeVideoUrl: Youtube("24xqBti7xHY")),

            new Tool(
                "Docker",
                "Docker Inc.",
                "26.1",
                "Docker is a platform for building, shipping, and running applications inside lightweight, portable containers. It packages an application together with all its dependencies so it behaves identically across development, testing, and production environments. It fundamentally changed how modern software is packaged and deployed.",
                LicenseType.Freemium,
                "https://docs.docker.com",
                infrastructure,
                foundedYear: 2013,
                logoUrl: Logo("docker"),
                availableVersions: ["25.0", "26.0", "26.1"],
                youtubeVideoUrl: Youtube("V9IJj4MzZBc")),
            new Tool(
                "Kubernetes",
                "Cloud Native Computing Foundation",
                "1.30",
                "Kubernetes is an open-source container orchestration platform originally developed at Google, used to automate the deployment, scaling, and management of containerized applications. It handles scheduling, self-healing, load balancing, and rolling updates across clusters of machines. It has become the standard for running production workloads at scale.",
                LicenseType.OpenSource,
                "https://kubernetes.io/docs",
                infrastructure,
                foundedYear: 2014,
                logoUrl: Logo("kubernetes"),
                availableVersions: ["1.28", "1.29", "1.30"],
                youtubeVideoUrl: Youtube("PziYflu8cB8")),
            new Tool(
                "Docker Compose",
                "Docker Inc.",
                "2.27",
                "Docker Compose is a tool for defining and running multi-container Docker applications using a single YAML configuration file. It is commonly used in local development to spin up an application's full stack of services — databases, caches, and APIs — with one command. It complements Docker for simpler, single-host orchestration needs.",
                LicenseType.OpenSource,
                "https://docs.docker.com/compose",
                infrastructure,
                foundedYear: 2014,
                logoUrl: Logo("docker"),
                availableVersions: ["2.25", "2.26", "2.27"],
                youtubeVideoUrl: Youtube("HG6yIjZapSA")),
            new Tool(
                "Terraform",
                "HashiCorp",
                "1.8",
                "Terraform is an open-source infrastructure-as-code tool that lets teams define cloud and on-premises infrastructure in a declarative configuration language. It plans and applies changes across dozens of providers — AWS, Azure, GCP, and more — while tracking the current state of provisioned resources. It is widely used to make infrastructure changes repeatable, reviewable, and version-controlled.",
                LicenseType.Freemium,
                "https://developer.hashicorp.com/terraform/docs",
                infrastructure,
                foundedYear: 2014,
                logoUrl: Logo("terraform"),
                availableVersions: ["1.6", "1.7", "1.8"],
                youtubeVideoUrl: Youtube("ZFLWA1kQ3ls")),
            new Tool(
                "Ansible",
                "Red Hat",
                "2.17",
                "Ansible is an open-source automation tool for configuration management, application deployment, and orchestration, using simple YAML playbooks. Unlike some alternatives, it requires no agents on managed machines, connecting over SSH instead. It is commonly used to keep server fleets consistently configured at scale.",
                LicenseType.OpenSource,
                "https://docs.ansible.com",
                infrastructure,
                foundedYear: 2012,
                logoUrl: Logo("ansible"),
                availableVersions: ["2.15", "2.16", "2.17"],
                youtubeVideoUrl: Youtube("FKDUG8QlcFc")),

            new Tool(
                "PostgreSQL",
                "PostgreSQL Global Development Group",
                "16",
                "PostgreSQL is a powerful open-source object-relational database known for standards compliance, extensibility, and support for advanced data types such as JSON and arrays. It is widely regarded as one of the most reliable and feature-complete relational databases available. Many organizations choose it as their default database for new applications.",
                LicenseType.OpenSource,
                "https://www.postgresql.org/docs",
                database,
                foundedYear: 1996,
                logoUrl: Logo("postgresql"),
                availableVersions: ["14", "15", "16"],
                youtubeVideoUrl: Youtube("ZOKFGDzAg78")),
            new Tool(
                "MySQL",
                "Oracle Corporation",
                "8.4",
                "MySQL is one of the world's most widely used open-source relational database management systems, known for its simplicity, speed, and broad ecosystem support. It powers a large share of web applications, from small projects to some of the largest sites on the internet. Its ubiquity makes it a common default choice for relational storage.",
                LicenseType.OpenSource,
                "https://dev.mysql.com/doc",
                database,
                foundedYear: 1995,
                logoUrl: Logo("mysql"),
                availableVersions: ["8.0", "8.4"],
                youtubeVideoUrl: Youtube("Y2WzWDlnBco")),
            new Tool(
                "SQL Server",
                "Microsoft",
                "2022",
                "Microsoft SQL Server is an enterprise relational database engine offering strong integration with the wider Microsoft ecosystem, including .NET, Azure, and Power BI. It provides advanced features for high availability, security, and business intelligence out of the box. It remains a common choice for organizations standardized on Microsoft technology.",
                LicenseType.Proprietary,
                "https://learn.microsoft.com/sql/sql-server",
                database,
                foundedYear: 1989,
                logoUrl: DeviconLogo("microsoftsqlserver/microsoftsqlserver-plain"),
                availableVersions: ["2019", "2022"],
                youtubeVideoUrl: Youtube("pF8n-8DPvjc")),
            new Tool(
                "MongoDB",
                "MongoDB Inc.",
                "7.0",
                "MongoDB is a document-oriented NoSQL database that stores data as flexible, JSON-like documents rather than rows and columns. Its schema-less design makes it well suited to applications with evolving or unstructured data models. It is one of the most popular choices for teams that need horizontal scalability without a rigid relational schema.",
                LicenseType.Freemium,
                "https://www.mongodb.com/docs",
                database,
                foundedYear: 2009,
                logoUrl: Logo("mongodb"),
                availableVersions: ["6.0", "7.0"],
                youtubeVideoUrl: Youtube("EE8ZTQxa0AM")),
            new Tool(
                "Redis",
                "Redis Ltd.",
                "7.4",
                "Redis is an in-memory data store used as a database, cache, and message broker, prized for its very low latency and rich set of data structures such as lists, sets, and sorted sets. It is commonly placed in front of slower databases to cache frequently accessed data. It also powers real-time features like leaderboards, rate limiting, and pub/sub messaging.",
                LicenseType.OpenSource,
                "https://redis.io/docs",
                database,
                foundedYear: 2009,
                logoUrl: Logo("redis"),
                availableVersions: ["7.2", "7.4"],
                youtubeVideoUrl: Youtube("8sHCdz_tOjk")),

            new Tool(
                "Figma",
                "Figma Inc.",
                "N/A",
                "Figma is a collaborative, browser-based interface design tool used to create UI mockups, prototypes, and design systems. Its real-time multiplayer editing lets multiple designers and stakeholders work in the same file simultaneously, similar to collaborative document editors. It has largely replaced desktop-only design tools for many product teams.",
                LicenseType.Freemium,
                "https://help.figma.com",
                design,
                foundedYear: 2016,
                logoUrl: Logo("figma"),
                youtubeVideoUrl: Youtube("Cx2dkpBxst8")),
            new Tool(
                "Adobe XD",
                "Adobe",
                "N/A",
                "Adobe XD is Adobe's user experience design tool for creating wireframes, interactive prototypes, and high-fidelity UI designs. It integrates with the wider Adobe Creative Cloud suite, making it convenient for teams already using Photoshop or Illustrator assets. It supports both design and basic prototyping workflows in one application.",
                LicenseType.Proprietary,
                "https://helpx.adobe.com/xd",
                design,
                foundedYear: 2016,
                logoUrl: DeviconLogo("xd/xd-plain"),
                youtubeVideoUrl: Youtube("TfdrHObZ8zY")),
            new Tool(
                "Sketch",
                "Sketch B.V.",
                "N/A",
                "Sketch is a vector-based interface design tool built exclusively for macOS, historically one of the pioneers of modern UI design tooling. It relies on a plugin ecosystem and cloud-based collaboration features to support design handoff and team workflows. It remains popular among design teams working natively on Mac.",
                LicenseType.Proprietary,
                "https://www.sketch.com/docs",
                design,
                foundedYear: 2010,
                logoUrl: Logo("sketch"),
                availableVersions: ["99", "100"],
                youtubeVideoUrl: Youtube("qywB0JHQeC4")),

            new Tool(
                "Visual Studio",
                "Microsoft",
                "2022",
                "Visual Studio is Microsoft's full-featured integrated development environment, most commonly used for .NET, C++, and other Microsoft-stack development. It provides a debugger, designer tools, and deep integration with Azure and Microsoft's broader developer ecosystem. It is the primary IDE for many enterprise .NET development teams.",
                LicenseType.Freemium,
                "https://learn.microsoft.com/visualstudio",
                developerTools,
                foundedYear: 1997,
                logoUrl: DeviconLogo("visualstudio/visualstudio-plain"),
                availableVersions: ["2019", "2022"],
                youtubeVideoUrl: Youtube("5AOp8zFu4Vg")),
            new Tool(
                "Visual Studio Code",
                "Microsoft",
                "1.90",
                "Visual Studio Code is a free, lightweight, and highly extensible source code editor built by Microsoft. Its large extension marketplace lets it support virtually any programming language or framework, while remaining fast and simple compared to full IDEs. It has become the most widely used code editor across the industry.",
                LicenseType.OpenSource,
                "https://code.visualstudio.com/docs",
                developerTools,
                foundedYear: 2015,
                logoUrl: DeviconLogo("vscode/vscode-original"),
                availableVersions: ["1.88", "1.89", "1.90"],
                youtubeVideoUrl: Youtube("B-s71n0dHUk")),
            new Tool(
                "Rider",
                "JetBrains",
                "2024.1",
                "Rider is JetBrains' cross-platform IDE for .NET development, offering deep code analysis, refactoring tools, and debugging support on Windows, macOS, and Linux. It is often chosen by teams wanting JetBrains' code intelligence for .NET without being tied to Windows-only tooling. It integrates with the same plugin ecosystem as other JetBrains IDEs.",
                LicenseType.Proprietary,
                "https://www.jetbrains.com/rider/documentation",
                developerTools,
                foundedYear: 2017,
                logoUrl: Logo("rider"),
                availableVersions: ["2023.3", "2024.1"],
                youtubeVideoUrl: Youtube("xkPtX492IhI")),
            new Tool(
                "IntelliJ IDEA",
                "JetBrains",
                "2024.1",
                "IntelliJ IDEA is JetBrains' flagship IDE for Java and other JVM languages, known for its deep code analysis, intelligent refactoring, and built-in tooling for frameworks like Spring. It set the standard that many other modern IDEs, including Android Studio, are built upon. It remains the primary choice for professional Java development.",
                LicenseType.Freemium,
                "https://www.jetbrains.com/idea/documentation",
                developerTools,
                foundedYear: 2001,
                logoUrl: Logo("intellijidea"),
                availableVersions: ["2023.3", "2024.1"],
                youtubeVideoUrl: Youtube("GSKERVTMWqs")),
            new Tool(
                "ReSharper",
                "JetBrains",
                "2024.1",
                "ReSharper is a JetBrains extension for Visual Studio that adds advanced code analysis, refactoring, and navigation tools on top of the base IDE. It highlights code smells, suggests fixes, and automates common refactoring tasks to keep large C# codebases maintainable. Many .NET teams treat it as a near-essential productivity add-on.",
                LicenseType.Proprietary,
                "https://www.jetbrains.com/resharper/documentation",
                developerTools,
                foundedYear: 2004,
                logoUrl: Logo("resharper"),
                availableVersions: ["2023.3", "2024.1"],
                youtubeVideoUrl: Youtube("pZmj27kK4B8")),
            new Tool(
                "Postman",
                "Postman Inc.",
                "11",
                "Postman is a widely used tool for designing, testing, and documenting APIs, letting developers send requests and inspect responses without writing client code. It supports collections, environment variables, and automated test scripts, making it useful for both manual exploration and CI-integrated API testing. It has become a standard part of API development workflows.",
                LicenseType.Freemium,
                "https://learning.postman.com/docs",
                developerTools,
                foundedYear: 2012,
                logoUrl: Logo("postman"),
                availableVersions: ["10", "11"],
                youtubeVideoUrl: Youtube("0dYCrdNUjuc")),
            new Tool(
                "npm",
                "npm Inc.",
                "10",
                "npm is the default package manager and package registry for the JavaScript and Node.js ecosystem, used to install, publish, and manage project dependencies. Its registry hosts the largest collection of open-source packages of any language ecosystem. Virtually every Node.js and front-end JavaScript project relies on it to manage dependencies.",
                LicenseType.OpenSource,
                "https://docs.npmjs.com",
                developerTools,
                foundedYear: 2010,
                logoUrl: Logo("npm"),
                availableVersions: ["9", "10"],
                youtubeVideoUrl: Youtube("h0thRmdftnU")),
            new Tool(
                "Webpack",
                "Webpack Contributors",
                "5.91",
                "Webpack is a module bundler for JavaScript applications that combines source files, stylesheets, and assets into optimized bundles for the browser. Its loader and plugin system allows it to process virtually any asset type as part of the build pipeline. It has been a foundational tool in the modern front-end build toolchain, though newer tools like Vite now compete for the same role.",
                LicenseType.OpenSource,
                "https://webpack.js.org",
                developerTools,
                foundedYear: 2012,
                logoUrl: Logo("webpack"),
                availableVersions: ["5.89", "5.90", "5.91"],
                youtubeVideoUrl: Youtube("vj4K8FMMQds"))
        ];
    }

    private static string Logo(string simpleIconsSlug) => $"https://cdn.simpleicons.org/{simpleIconsSlug}";

    private static string Youtube(string videoId) => $"https://www.youtube.com/watch?v={videoId}";

    // A handful of trademarked logos (Microsoft, Adobe) were pulled from Simple Icons;
    // Devicon still hosts them.
    private static string DeviconLogo(string iconPath) =>
        $"https://cdn.jsdelivr.net/gh/devicons/devicon/icons/{iconPath}.svg";

    private static void LinkDepartmentsToTools(
        IReadOnlyDictionary<string, Department> departments,
        IReadOnlyDictionary<string, Tool> tools)
    {
        var backEnd = departments["Développement Back-End"];
        var frontEnd = departments["Développement Front-End"];
        var qa = departments["Assurance Qualité"];
        var ops = departments["Opérations"];
        var data = departments["Data & Analytics"];
        var security = departments["Sécurité"];

        Link(backEnd, tools["Git"], UsageLevel.Primary, "Camille Dubois", new DateOnly(2020, 3, 1));
        Link(backEnd, tools["GitHub"], UsageLevel.Primary, "Camille Dubois", new DateOnly(2020, 3, 1));
        Link(backEnd, tools["Jenkins"], UsageLevel.Primary, "Camille Dubois", new DateOnly(2021, 6, 15));
        Link(backEnd, tools["SonarQube"], UsageLevel.Primary, "Camille Dubois", new DateOnly(2021, 9, 1));
        Link(backEnd, tools["Docker"], UsageLevel.Primary, "Julien Fabre", new DateOnly(2021, 1, 10));
        Link(backEnd, tools["PostgreSQL"], UsageLevel.Primary, "Julien Fabre", new DateOnly(2020, 5, 20));
        Link(backEnd, tools["SQL Server"], UsageLevel.Secondary, "Julien Fabre", new DateOnly(2020, 5, 20));
        Link(backEnd, tools["Redis"], UsageLevel.Secondary, "Julien Fabre", new DateOnly(2022, 2, 1));
        Link(backEnd, tools["Visual Studio"], UsageLevel.Primary, "Camille Dubois", new DateOnly(2019, 11, 1));
        Link(backEnd, tools["Rider"], UsageLevel.Secondary, "Camille Dubois", new DateOnly(2023, 4, 1));
        Link(backEnd, tools["Postman"], UsageLevel.Primary, "Julien Fabre", new DateOnly(2021, 3, 1));
        Link(backEnd, tools["Jira"], UsageLevel.Primary, "Camille Dubois", new DateOnly(2019, 11, 1));
        Link(backEnd, tools["Confluence"], UsageLevel.Secondary, "Camille Dubois", new DateOnly(2019, 11, 1));
        Link(backEnd, tools["Kubernetes"], UsageLevel.Evaluating, "Julien Fabre", new DateOnly(2024, 1, 15));
        Link(backEnd, tools["Terraform"], UsageLevel.Evaluating, "Julien Fabre", new DateOnly(2024, 2, 1));

        Link(frontEnd, tools["Git"], UsageLevel.Primary, "Manon Perrin", new DateOnly(2020, 3, 1));
        Link(frontEnd, tools["GitHub"], UsageLevel.Primary, "Manon Perrin", new DateOnly(2020, 3, 1));
        Link(frontEnd, tools["GitLab CI"], UsageLevel.Secondary, "Manon Perrin", new DateOnly(2022, 6, 1));
        Link(frontEnd, tools["Figma"], UsageLevel.Primary, "Léa Girard", new DateOnly(2021, 4, 1));
        Link(frontEnd, tools["Adobe XD"], UsageLevel.Secondary, "Léa Girard", new DateOnly(2021, 4, 1));
        Link(frontEnd, tools["Visual Studio Code"], UsageLevel.Primary, "Manon Perrin", new DateOnly(2020, 3, 1));
        Link(frontEnd, tools["npm"], UsageLevel.Primary, "Manon Perrin", new DateOnly(2020, 3, 1));
        Link(frontEnd, tools["Webpack"], UsageLevel.Primary, "Manon Perrin", new DateOnly(2020, 3, 1));
        Link(frontEnd, tools["Jira"], UsageLevel.Primary, "Léa Girard", new DateOnly(2020, 3, 1));
        Link(frontEnd, tools["Postman"], UsageLevel.Secondary, "Manon Perrin", new DateOnly(2021, 5, 1));
        Link(frontEnd, tools["SonarQube"], UsageLevel.Secondary, "Manon Perrin", new DateOnly(2022, 9, 1));

        Link(qa, tools["Jira"], UsageLevel.Primary, "Nicolas Roche", new DateOnly(2020, 6, 1));
        Link(qa, tools["Postman"], UsageLevel.Primary, "Nicolas Roche", new DateOnly(2020, 6, 1));
        Link(qa, tools["SonarQube"], UsageLevel.Primary, "Nicolas Roche", new DateOnly(2021, 9, 1));
        Link(qa, tools["Confluence"], UsageLevel.Secondary, "Nicolas Roche", new DateOnly(2020, 6, 1));
        Link(qa, tools["GitHub Actions"], UsageLevel.Evaluating, "Nicolas Roche", new DateOnly(2024, 3, 1));
        Link(qa, tools["GitLab CI"], UsageLevel.Evaluating, "Nicolas Roche", new DateOnly(2024, 3, 1));

        Link(ops, tools["Docker"], UsageLevel.Primary, "Sophie Marchand", new DateOnly(2020, 1, 15));
        Link(ops, tools["Kubernetes"], UsageLevel.Primary, "Sophie Marchand", new DateOnly(2021, 2, 1));
        Link(ops, tools["Terraform"], UsageLevel.Primary, "Sophie Marchand", new DateOnly(2021, 7, 1));
        Link(ops, tools["Ansible"], UsageLevel.Primary, "Sophie Marchand", new DateOnly(2020, 8, 1));
        Link(ops, tools["Grafana"], UsageLevel.Primary, "Thomas Lambert", new DateOnly(2020, 5, 1));
        Link(ops, tools["Prometheus"], UsageLevel.Primary, "Thomas Lambert", new DateOnly(2020, 5, 1));
        Link(ops, tools["Jenkins"], UsageLevel.Secondary, "Sophie Marchand", new DateOnly(2021, 7, 1));
        Link(ops, tools["Azure DevOps"], UsageLevel.Secondary, "Thomas Lambert", new DateOnly(2022, 4, 1));
        Link(ops, tools["Datadog"], UsageLevel.Evaluating, "Thomas Lambert", new DateOnly(2024, 5, 1));

        Link(data, tools["PostgreSQL"], UsageLevel.Primary, "Inès Benali", new DateOnly(2020, 9, 1));
        Link(data, tools["MongoDB"], UsageLevel.Primary, "Inès Benali", new DateOnly(2021, 11, 1));
        Link(data, tools["Redis"], UsageLevel.Secondary, "Inès Benali", new DateOnly(2022, 3, 1));
        Link(data, tools["Grafana"], UsageLevel.Secondary, "Hugo Petit", new DateOnly(2021, 6, 1));
        Link(data, tools["Kibana"], UsageLevel.Primary, "Hugo Petit", new DateOnly(2022, 1, 15));
        Link(data, tools["MySQL"], UsageLevel.Secondary, "Inès Benali", new DateOnly(2020, 9, 1));
        Link(data, tools["Notion"], UsageLevel.Secondary, "Hugo Petit", new DateOnly(2023, 2, 1));
        Link(data, tools["Jira"], UsageLevel.Secondary, "Hugo Petit", new DateOnly(2021, 6, 1));

        Link(security, tools["GitHub"], UsageLevel.Primary, "Antoine Rousseau", new DateOnly(2021, 2, 1));
        Link(security, tools["SonarQube"], UsageLevel.Primary, "Antoine Rousseau", new DateOnly(2021, 9, 1));
        Link(security, tools["Datadog"], UsageLevel.Secondary, "Antoine Rousseau", new DateOnly(2023, 1, 1));
        Link(security, tools["New Relic"], UsageLevel.Evaluating, "Antoine Rousseau", new DateOnly(2024, 4, 1));
        Link(security, tools["Ansible"], UsageLevel.Secondary, "Antoine Rousseau", new DateOnly(2022, 5, 1));
        Link(security, tools["Confluence"], UsageLevel.Secondary, "Antoine Rousseau", new DateOnly(2021, 2, 1));
        Link(security, tools["Terraform"], UsageLevel.Evaluating, "Antoine Rousseau", new DateOnly(2024, 4, 1));
    }

    private static void Link(Department department, Tool tool, UsageLevel usageLevel, string referent, DateOnly adoptedOn) =>
        tool.LinkTo(department, usageLevel, referent, adoptedOn);
}
