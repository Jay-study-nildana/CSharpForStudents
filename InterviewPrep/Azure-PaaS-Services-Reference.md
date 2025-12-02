# Azure PaaS Services — Deep Reference  
Comprehensive notes, architecture, examples (Portal / CLI / PowerShell / SDK / Bicep), best practices, security, monitoring and operational guidance.

---

## Table of Contents

1. [Purpose & Audience](#purpose--audience)  
2. [PaaS Overview — What & Why](#paas-overview---what--why)  
3. [Key PaaS Properties & Comparison with IaaS/SaaS](#key-paas-properties--comparison-with-iaassaas)  
4. [Azure Web App (App Service)](#azure-web-app-app-service)  
   - General Overview  
   - App Service Plan (pricing, SKUs, scaling)  
   - App Service Environments (ASE) & Isolated hosting  
   - Deployment (Kudu, ZIP Deploy, Git, GitHub Actions, FTP)  
   - Deployment Slots (swap, routing, slot settings)  
   - App Configuration & Feature Management integrations  
   - WebJobs (continuous / triggered)  
   - Networking, VNet Integration & Private Endpoints  
   - Monitoring & Diagnostics  
   - Security & Secrets (Managed Identity, Key Vault integration)  
   - CLI / ARM / Bicep / GitHub Actions examples  
5. [Azure Functions (Serverless)](#azure-functions-serverless)  
   - Overview & serverless model  
   - Benefits and trade-offs  
   - Triggers & Bindings (HTTP, Timer, Queue, Service Bus, EventGrid, Blob, Cosmos DB, Durable)  
   - Durable Functions (patterns: orchestrator, entities, fan-out/fan-in)  
   - Hosting Plans: Consumption, Premium, Dedicated (App Service), Elastic Premium, Dedicated with ASE, Isolated plan  
   - Scaling behavior, cold starts, pre-warmed instances and scaling limits  
   - Local development & Function Core Tools examples  
   - CI/CD and deployment options  
   - Security, Managed Identity & VNet Integration  
6. [Azure Service Bus (Enterprise Messaging)](#azure-service-bus-enterprise-messaging)  
   - Overview & differences vs Queues/Storage Queues / Event Hubs  
   - Messaging primitives: Namespaces, Queues, Topics & Subscriptions, Sessions, Dead-Letter Queue (DLQ), Scheduled Messages, ForwardTo/AutoForwarding  
   - Patterns: competing consumers, pub/sub, request/reply, transactions, duplicate detection, sessions for ordered processing  
   - Filters & Rules on subscriptions (SQL filter, correlation filter)  
   - SDK examples (.NET Azure.Messaging.ServiceBus) and CLI operations  
   - Scaling, throughput units, partitioning and Geo-disaster recovery (GeoDR)  
   - Security: RBAC, SAS, Managed Identity and encryption  
7. [Azure Logic Apps (Workflow & Integration)](#azure-logic-apps-workflow--integration)  
   - Overview: integration platform as a service (iPaaS) for orchestrating connectors & workflows  
   - Single-tenant vs Multi-tenant Logic Apps — differences, when to choose which  
   - Connectors & built-in actions (HTTP, SQL, Service Bus, Azure Functions, Dynamics, Office365, Salesforce, SAP)  
   - Workflow triggers: recurrence, webhook, polling, trigger conditions, built-in connectors  
   - Approval workflows and human interaction (Approvals connector, adaptive cards)  
   - Integration Account (B2B/EDI, maps, schemas) and Enterprise Integration Pack  
   - Monitoring, diagnostics and run history, retries and error handling  
   - CI/CD with ARM/Bicep and DevOps Pipelines  
8. [Common Cross-cutting Concerns & Best Practices](#common-cross-cutting-concerns--best-practices)  
   - Observability, logging, metrics and alerts (Application Insights, Log Analytics)  
   - Secrets & configuration management (App Configuration, Key Vault, Managed Identity)  
   - Networking & isolation (VNet Integration, Private Endpoints, Service Endpoints, ASE)  
   - Security & compliance considerations, Identity (AAD), RBAC, least privilege  
   - Cost control, scaling strategies & resilience patterns  
9. [Appendix — Commands, Snippets & Examples](#appendix---commands-snippets--examples)  
   - Useful Azure CLI, PowerShell, Bicep, and code samples (C#, JavaScript, Python)  
10. [References & Further Reading](#references--further-reading)

---

## 1. Purpose & Audience

This reference is for cloud engineers, platform teams, SREs, architects and developers adopting or operating Azure PaaS. It explains core services (Web Apps, Functions, Service Bus, Logic Apps), how to operate them, example code & templates, and practical guidance for security, networking, monitoring, and cost management.

---

## 2. PaaS Overview — What & Why

Platform as a Service (PaaS) provides a managed runtime and platform features so developers focus on code rather than infrastructure. In Azure, PaaS services handle patching, OS management, scaling primitives, built-in observability, integrated identity, deployment slots and more.

Benefits:
- Faster time to market; less operational overhead
- Built-in scaling and high availability
- Integration with platform-managed identity, storage, monitoring
- Pay for consumed resources or reserved capacity (depends on SKU)

Trade-offs:
- Less control over OS and environment than IaaS
- Constraints on custom system software, kernel modules or root-level access
- Potential vendor-service lock-in

---

## 3. Key PaaS Properties & Comparison with IaaS / SaaS

- IaaS (VMs): full control of OS, but responsibility for patching, scalability, HA design, backups.
- PaaS: managed platform, scale and health handled by provider; limited OS control.
- SaaS: fully managed application (Office 365) — no control over platform.
- PaaS examples in Azure: App Service (Web Apps), Functions, Logic Apps, Azure SQL (PaaS DB), API Management, Service Bus.

---

## 4. Azure Web App (App Service)

### General Overview
Azure Web App (App Service) is a fully managed platform for hosting web applications, REST APIs and mobile back ends. Supports multiple runtimes (.NET, .NET Core, Java, Node.js, Python, PHP, Ruby, containers) and deployment models (code or container).

Key features:
- Built-in auto-scaling, staging (deployment) slots, integrated authentication (EasyAuth), TLS termination, custom domains, integrated logging/diagnostics, continuous deployment (GitHub / Azure DevOps), Managed Identity.

### App Service Plan
An App Service Plan is the compute resource that defines the location, features and pricing tier (Free, Shared, Basic, Standard, Premium, PremiumV2/V3, Isolated). App Service instances run in the plan – multiple apps can share same plan.

- Choose SKU based on CPU/memory, auto-scale, VNet integration, dedicated instances, and features like PremiumV3 (better CPU & memory).
- Scale:
  - Vertical (change plan SKU)
  - Horizontal (scale-out instances). Auto-scale rules can be set by CPU, memory, queue length, or custom metrics.

CLI to create App Service Plan:
```bash
az appservice plan create -g myRG -n myPlan --sku P1v2 --is-linux
```

### App Service Environment (ASE)
ASE is a dedicated, single-tenant deployment of App Service in a customer VNet. Use when isolation, network control, or high scale is required (legacy ASE v3 recommended). ASE allows internal load balancing and integration with corporate network via ExpressRoute/VPN.

### Kudu Deployment (Advanced Tools)
Kudu is the deployment engine behind App Service. Exposes:
- Git deployment, deployment logs, process explorer, environment variables, site extensions, webjobs UI, diagnostics.
- Kudu REST API and console for debugging (accessible via SCM site).

Example: ZIP Deploy via Kudu:
```bash
az webapp deployment source config-zip --resource-group myRG --name myApp --src myapp.zip
```

### Deployment Slots
- Allow staging of app versions and swapping into production with warm-up. Each slot has its own host name and environment variables can be slot-specific (`slot settings`) (e.g., connection strings marked as slot settings will not swap).
- Blue/Green and Canary deployment patterns: use multiple slots and traffic routing to shift percentage of traffic to a slot (traffic routing available in App Service).

CLI create slot and swap:
```bash
az webapp deployment slot create -g myRG -n myApp -s staging
az webapp deployment slot swap -g myRG -n myApp --slot staging --target-slot production
```

### App Configuration & Feature Management
- Use Azure App Configuration for centralized configuration and feature flags (feature management SDK integrates with .NET, Java, Node).
- Application settings in App Service are exposed as environment variables; prefer retrieving secrets from Key Vault via Managed Identity or using App Configuration for feature flags and dynamic configuration.

Example: Add app setting (CLI):
```bash
az webapp config appsettings set -g myRG -n myApp --settings "ConnectionStrings__Default=... "
```

### WebJobs
- Background jobs for App Service (Windows / Linux): Continuous, Triggered (on-demand), or Scheduled (CRON-like).
- Great for small background tasks tied to the web app (processing queues, scheduled maintenance).
- WebJobs can be managed via Kudu or deployed as part of app. For advanced background work consider Functions or Kubernetes.

### Networking & VNet Integration
- Outbound VNet Integration: App Service can integrate with VNet to access resources in the private network.
- Private Endpoints for App Service (Private Endpoint for App Service Environments or regional access) for inbound private access.
- Use App Service with Application Gateway in front for WAF, URL-based routing, TLS offloading.

### Monitoring & Diagnostics
- App Service Diagnostics, Application Insights (APM), Log Stream (live), and container logs.
- Enable `Diagnostic logs` and route to Log Analytics for central monitoring.

### Security & Secrets
- Managed Identity (system-assigned / user-assigned) for retrieving secrets from Key Vault.
- Configure authentication/authorization (EasyAuth) for social logins and AAD.
- Restrict app access with Access Restrictions and IP filtering.

### Example Bicep snippet (Web App + Plan)
```bicep
resource plan 'Microsoft.Web/serverfarms@2021-02-01' = {
  name: 'asp-plan'
  location: resourceGroup().location
  sku: { name: 'P1v2'; capacity: 1 }
  properties: { reserved: true } // for Linux
}

resource site 'Microsoft.Web/sites@2021-02-01' = {
  name: 'myWebApp'
  location: resourceGroup().location
  properties: {
    serverFarmId: plan.id
    siteConfig: { linuxFxVersion: 'DOTNETCORE|6.0' }
  }
}
```

---

## 5. Azure Functions (Serverless)

### Overview & Serverless model
Azure Functions is a serverless compute service to run event-driven code. You are billed for the execution time and resources (Consumption) or provisioned instances (Premium or Dedicated). Functions support multiple languages (C#, JavaScript/TypeScript, Python, Java, PowerShell, Custom handlers / containers).

### Benefits
- Rapid development: write function for each event
- Elastic scale: autoscaling on triggers/traffic
- Lower ops overhead; automatic provisioning on Consumption plan
- Rich binding model to integrate services quickly

### Triggers & Bindings
- Triggers start a function: HTTP, Timer, Queue Storage, Service Bus, Event Grid, Blob, Cosmos DB, Event Hubs, Durable Functions orchestrator triggers.
- Bindings connect input and output declaratively: e.g., function receives a blob and outputs a message to Service Bus without boilerplate code.

Example function.json (bindings) or attributes in languages. C# isolated worker sample:
```csharp
[FunctionName("ProcessQueueMessage")]
public static async Task Run([ServiceBusTrigger("myqueue", Connection = "ServiceBusConn")] string message, ILogger log)
{
    log.LogInformation($"Received: {message}");
}
```

### Durable Functions
Durable Functions extend Functions with durable state and orchestrations: orchestrator functions, activity functions, entity functions.

Common patterns:
- Function chaining
- Fan-out/fan-in (parallel processing)
- Async HTTP APIs (orchestrator drives workflow; client polls status)
- Human interaction patterns (approval workflows)

Example Orchestrator in C#:
```csharp
[FunctionName("Orchestrator")]
public static async Task RunOrchestrator([OrchestrationTrigger] IDurableOrchestrationContext context)
{
    var outputs = new List<string>();
    outputs.Add(await context.CallActivityAsync<string>("SayHello", "Tokyo"));
    outputs.Add(await context.CallActivityAsync<string>("SayHello", "Seattle"));
}
```

### Hosting Plans
- Consumption: scales automatically, pay-per-execution, cold starts possible.
- Premium: pre-warmed instances, VNet support, unlimited execution duration, better scale-out and no cold start.
- Dedicated / App Service plan: run on existing App Service plans; useful for dev/test or when you need dedicated resources.
- Isolated: runs in an ASE for single-tenant, VNet-isolated deployments.

Choose plan based on latency sensitivity (avoid cold start with Premium), VNet isolation needs, long-running processes, and scaling behavior.

### Local development & Function Core Tools
- `func` CLI to run functions locally.
- Example local run:
```bash
func new --template HttpTrigger --name MyHttpTrigger
func start
```

### CI/CD
- Zip deploy, GitHub Actions (Azure/functions-action), or Azure Pipelines are common.
- Use slots and staging for production deployments.

### Networking & Security
- VNet Integration available for Premium and Dedicated plans.
- Use Managed Identity for secure access to Key Vault, Service Bus, Storage.
- Use Private Endpoints for Storage accounts used by functions for secure connectivity.

---

## 6. Azure Service Bus (Enterprise Messaging)

### Overview
Azure Service Bus is a fully managed enterprise messaging platform with features for reliable messaging and advanced patterns (topics, sessions, transactions). It supports AMQP 1.0 and HTTP.

### Components
- **Namespace**: administrative container for messaging entities.
- **Queue**: point-to-point messaging for competing consumers.
- **Topic**: publish/subscribe model where messages are published to topics and routed to subscriptions.
- **Subscription**: durable entity attached to a topic, can have filters and rules.
- **Dead-letter Queue (DLQ)**: built-in per-entity DLQ for messages that can't be processed.
- **Sessions**: provide FIFO semantics and stateful handling of related messages (sessionId).
- **Transactions**: group operations (send + receive) in a single atomic unit.

### Features & Patterns
- Duplicate detection: prevent duplicates based on messageId and time window.
- Scheduled messages: enqueue messages for future delivery.
- ForwardTo / AutoForwarding: chain queues/topics (forward to another entity).
- Filters & Rules: SQL-like filters on subscriptions to implement content-based routing.
- Partitioned entities: improve throughput and scale by partitioning (multi-broker).
- Message TTL and Auto-Delete-on-Idle behavior.

### SDK example (.NET: Azure.Messaging.ServiceBus)
```csharp
using Azure.Messaging.ServiceBus;

var client = new ServiceBusClient("Endpoint=sb://<namespace>.servicebus.windows.net/;SharedAccessKeyName=...;SharedAccessKey=...");
var sender = client.CreateSender("myqueue");
await sender.SendMessageAsync(new ServiceBusMessage("Hello"));

var processor = client.CreateProcessor("myqueue", new ServiceBusProcessorOptions());
processor.ProcessMessageAsync += async args => {
    string body = args.Message.Body.ToString();
    Console.WriteLine(body);
    await args.CompleteMessageAsync(args.Message);
};
processor.ProcessErrorAsync += args => { Console.WriteLine(args.Exception); return Task.CompletedTask; };
await processor.StartProcessingAsync();
```

### Admin (CLI)
```bash
az servicebus namespace create -g rg -n mynamespace --location eastus --sku Standard
az servicebus queue create -g rg --namespace-name mynamespace -n myqueue --max-size 1024
az servicebus topic create -g rg --namespace-name mynamespace -n mytopic
az servicebus topic subscription create -g rg --namespace-name mynamespace --topic-name mytopic --name mysubscription
```

### Security & Access
- Use Azure AD roles (Data Owner/Contributor/Reader) or SAS tokens and Shared Access Keys for connectivity.
- Managed Identity recommended for application auth to Service Bus.

### GeoDR & Disaster Recovery
- Geo-DR (alias) for failover between paired namespaces; configure pairing and failover policy.

---

## 7. Azure Logic Apps

### Overview
Logic Apps is a low-code/no-code workflow orchestration service that integrates hundreds of connectors (SaaS, on-premises, Azure services) to build automated workflows.

- Designer: visual designer in Portal or Visual Studio.
- Triggers: event-based, schedule-based, webhook-based.
- Actions: built-in connectors (HTTP, SQL, Service Bus) and managed connectors (Office 365, Salesforce).
- Stateful vs Stateless workflows: Stateless are cheaper and faster for high-throughput scenarios but lack long-running state.

### Single tenant vs Multi-tenant
- **Single-tenant (Logic Apps (Standard))**:
  - Runs on a dedicated hosting plan (Functions-like runtime).
  - Allows VNet Integration and local runtime (for enterprise, on-prem integration).
  - Better for more control, isolated networking.
- **Multi-tenant (Logic Apps (Consumption))**:
  - Multi-tenant hosted service with pay-per-run model.
  - Faster to get started, integrates many connectors, but limited VNet features (connectors use managed connector infrastructure).

### Common workflow types
- **Schedule-based**: run a job every hour, day, etc. (recurrence trigger).
- **Approval workflows**: send actionable approval emails/cards; integrates with Approvals and Teams.
- **Event-driven**: triggers on events from Event Grid, Service Bus, HTTP webhook or connectors.
- **Integration**: B2B/EDI with an Integration Account (schemas, maps, agreements).

### Monitoring & Error handling
- Each run produces run history; built-in retry policies per action, run-level settings, and diagnostics integration with Log Analytics.
- Implement compensating transactions and idempotence patterns for reliability.

### Example: simple Logic App Recurrence → HTTP → Parse JSON → Insert to SQL
Design visually or use ARM/Bicep template generated from designer. Use connector authentication via managed identity or service principal.

---

## 8. Common Cross-cutting Concerns & Best Practices

### Observability
- Instrument apps and functions with Application Insights (traces, custom metrics, distributed tracing).
- Centralize logs in Log Analytics and create dashboards/alerts for important metrics (error rate, latency, throughput).

### Secrets & Configuration
- Use Key Vault and Managed Identity for secrets.
- Use App Configuration for centralized feature flags and non-sensitive config; integrate with Web Apps and Functions.

### Networking & Isolation
- Use VNet Integration & Private Endpoints to prevent public exposure.
- Hub-and-spoke network topology: central hub for shared services (firewall, DNS, NAT) and spoke VNets for applications.

### Security & Identity
- Enforce AAD authentication for APIs and Web Apps (EasyAuth).
- Use RBAC and PIM for admin access. Limit contributor roles and avoid shared keys.

### Cost & Scaling
- Use autoscale rules that reduce cost for dev/test.
- Use Consumption plan for unpredictable bursts (Functions) and Premium for predictable but latency-sensitive workloads.
- Monitor and alert on cost anomalies.

---

## 9. Appendix — Commands, Snippets & Examples

### Create a Linux Web App with GitHub Actions (CLI)
```bash
az group create -n rg-web -l eastus
az appservice plan create -g rg-web -n plan-web --is-linux --sku P1v2
az webapp create -g rg-web -p plan-web -n my-web-app --runtime "NODE|18-lts"
# Configure GitHub Actions (in portal or use az webapp deployment github-actions add)
```

### Create an Azure Function (Consumption) in CLI
```bash
az functionapp create -g rg-func -n myfuncapp --storage-account mystorageacct --consumption-plan-location eastus --runtime dotnet --functions-version 4
```

### Service Bus: create namespace, queue & role assignment
```bash
az servicebus namespace create -g rg-mb -n sb-namespace --sku Standard
az servicebus queue create -g rg-mb --namespace-name sb-namespace -n orders --max-size 1024
az role assignment create --assignee <clientId> --role "Azure Service Bus Data Sender" --scope $(az servicebus namespace show -g rg-mb -n sb-namespace --query id -o tsv)
```

### Durable Function Example (C# Durable Orchestrator)
See Durable Functions patterns (fan-out/fan-in), with `Microsoft.Azure.WebJobs.Extensions.DurableTask` package.

---

## 10. References & Further Reading

- App Service docs: https://learn.microsoft.com/azure/app-service/  
- Azure Functions docs: https://learn.microsoft.com/azure/azure-functions/  
- Service Bus docs: https://learn.microsoft.com/azure/service-bus-messaging/  
- Logic Apps docs: https://learn.microsoft.com/azure/logic-apps/  
- Azure architecture center: https://learn.microsoft.com/azure/architecture/  
- Durable Functions patterns: https://learn.microsoft.com/azure/azure-functions/durable/durable-functions-overview

---

Prepared as a comprehensive and practical guide to Azure PaaS capabilities (Web Apps, Functions, Service Bus, Logic Apps), with configuration and operational examples. If you want, I can:

- generate ready-to-run Bicep templates and GitHub Actions workflows for CI/CD for Web Apps & Functions,  
- create a sample microservices demo using Functions + Service Bus + App Service front-end with instrumentation and deployment scripts, or  
- produce a one-page cheat sheet summarizing hosting plans, SKUs, and when to choose each PaaS service.  

Which would you like next?
``` ````