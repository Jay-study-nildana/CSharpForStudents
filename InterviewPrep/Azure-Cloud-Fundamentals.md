# Microsoft Azure — Cloud Overview, Core Services & Resource Management  
Interview & Practical Reference Guide

---

## Table of Contents

1. [Scope & Purpose](#scope--purpose)  
2. [Cloud Computing — High Level](#cloud-computing---high-level)  
3. [Azure — General Overview](#azure---general-overview)  
4. [Azure Portal — Quick Tour & Workflow](#azure-portal---quick-tour--workflow)  
5. [Cloud Computing Benefits](#cloud-computing-benefits)  
6. [CapEx vs OpEx in Cloud](#capex-vs-opex-in-cloud)  
7. [Cloud Deployment Models — Public / Private / Hybrid](#cloud-deployment-models---public--private--hybrid)  
8. [Core Azure Services — Detailed Notes](#core-azure-services---detailed-notes)  
   - Compute: VMs, VM Scale Sets, App Service, AKS, Functions  
   - Storage: Blob, Managed Disks, File, Queue, Table, Archive  
   - Networking: VNet, Subnets, NSG, Route Tables, Load Balancer, Application Gateway, Azure Firewall, VPN/ExpressRoute, CDN  
   - Databases & Data: Azure SQL, Azure Database for MySQL/Postgres, Cosmos DB, Azure Cache for Redis, Synapse, Data Factory  
   - Identity & Access: Azure Active Directory (AAD), Managed Identity, RBAC, Conditional Access  
   - Management & Monitoring: Azure Monitor, Log Analytics, Application Insights, Advisor, Cost Management, Security Center / Microsoft Defender for Cloud  
   - Integration: Service Bus, Event Grid, Event Hubs, Logic Apps  
   - Backup & DR: Azure Backup, Azure Site Recovery  
   - Serverless & Edge: Functions, Logic Apps, IoT Hub, IoT Edge, Azure Stack  
9. [Regions, Availability Zones, & Availability Sets](#regions-availability-zones--availability-sets)  
10. [Resource Groups — Organization & Lifecycles](#resource-groups---organization--lifecycles)  
11. [Azure Resource Manager (ARM) — Principles & Examples](#azure-resource-manager-arm---principles--examples)  
    - Declarative templates (ARM templates / Bicep), CLI and PowerShell examples  
12. [Governance: Policies, Locks, Tags & RBAC](#governance-policies-locks-tags--rbac)  
13. [Cost Management & Pricing Considerations](#cost-management--pricing-considerations)  
14. [Security & Compliance Essentials](#security--compliance-essentials)  
15. [Operational Tips & Best Practices](#operational-tips--best-practices)  
16. [Short Q&A — Key Concepts & Interview Questions](#short-qa---key-concepts--interview-questions)  
17. [References & Further Reading](#references--further-reading)

---

## 1. Scope & Purpose

This document is a compact but thorough reference covering Microsoft Azure fundamentals, cloud concepts, core services, resource management with Azure Resource Manager, availability and resiliency constructs, governance, cost considerations (CapEx vs OpEx), and practical examples (Azure CLI, PowerShell, ARM/Bicep). It is intended for developers, architects, SREs and interview preparation.

---

## 2. Cloud Computing — High Level

Cloud computing delivers computing services (compute, storage, networking, databases, analytics, etc.) over the Internet (on demand). Common service models:

- IaaS (Infrastructure as a Service) — virtual machines, storage, networking. You manage OS/apps.
- PaaS (Platform as a Service) — managed platform for apps (App Service, Azure SQL) — less infra ops.
- SaaS (Software as a Service) — fully managed apps (Office 365, Dynamics).

Key operational properties:
- Elasticity: scale up/down or out automatically or on demand.
- On-demand self-service: provision services via portal/API/CLI.
- Measured service: pay-for-what-you-use.

---

## 3. Azure — General Overview

Microsoft Azure is a global public cloud platform offering IaaS, PaaS and SaaS solutions. Azure concepts to know:

- Region: geographic location (e.g., East US, West Europe).
- Availability Zone: physically separate datacenter within a region (where available).
- Resource: manageable item (VM, storage account).
- Resource Group: logical container for related resources.
- Subscription: billing and quota boundary.
- Tenant: Azure AD directory for identity.

Azure provides native integration with Microsoft identity (Azure AD), enterprise support, hybrid capabilities (ExpressRoute, Azure Arc), and many managed services.

---

## 4. Azure Portal — Quick Tour & Workflow

Azure Portal (https://portal.azure.com) is the web-based UI for managing Azure resources.

Typical workflow:
1. Sign in with Azure AD account. Subscription and RBAC determine access.
2. Create a resource group (`Resource groups` blade).
3. Provision resources via “Create a resource” (wizard) or deploy ARM/Bicep/CLI/PowerShell templates.
4. Configure networking (VNet/subnets/NSGs), storage, compute, identity (managed identities), and monitoring.
5. Add alerts, diagnostics and log collection (Azure Monitor).
6. Use Cost Management to set budgets and review billing.

Portal features:
- Dashboard customization, resource explorer, Cloud Shell (bash/PowerShell), Resource Graph, Marketplace.

Tip: use Cloud Shell for authenticated CLI (az) & PowerShell without local auth.

---

## 5. Cloud Computing Benefits

- Elasticity & scalability — handle variable demand.
- Reduced time-to-market — provision services quickly.
- Operational efficiency — managed services reduce operational overhead.
- Global reach — deploy close to users across regions.
- Cost optimization — pay-as-you-go, reserved instances, spot VMs, auto-scaling.
- Innovation velocity — serverless, managed databases, ML services accelerate development.

---

## 6. CapEx vs OpEx in Cloud

- CapEx (Capital Expenditure): up-front spending on physical servers, datacenter hardware, networking. Depreciated over time. Large initial investment.
- OpEx (Operating Expenditure): ongoing, pay-as-you-go costs (compute hours, storage, networking, managed services). Greater flexibility; expenses become operational.

Cloud shifts spending model:
- Less CapEx: no large capital hardware purchases.
- More OpEx: predictable monthly/usage costs, but needs cost governance to avoid overruns (budgets, policies, tags).

Cost levers:
- Right-size VMs, use reserved instances or savings plans, auto scale, use managed PaaS instead of large VMs, use cold/archival storage tiers.

---

## 7. Cloud Deployment Models — Public / Private / Hybrid

- Public cloud: resources hosted and managed by cloud provider (Azure). Scales easily, lower operational burden.
- Private cloud: dedicated infrastructure for single organization (on-premises or hosted). Greater control, compliance advantages, higher CapEx.
- Hybrid cloud: combination of both; common for gradual migration, latency-sensitive workloads, data sovereignty. Azure hybrid tools: ExpressRoute, Azure Arc, Azure Stack.

When to choose hybrid:
- Regulatory constraints, legacy systems requiring on-prem data access, low-latency locality to on-prem resources.

---

## 8. Core Azure Services — Detailed Notes

Below are common service categories with short details and example uses.

### Compute

- Virtual Machines (VM)
  - IaaS VMs with many SKUs (B, D, E, F series, M series). Choose OS image, VM size, disk type (Standard HDD/SSD, Premium SSD, Ultra Disk).
  - Managed Disks: Azure manages disk storage and durability.
  - Example CLI:
    ```bash
    az vm create -g MyRG -n MyVM --image UbuntuLTS --size Standard_DS2_v2 --admin-username azureuser
    ```
- Virtual Machine Scale Sets (VMSS)
  - Autoscaling group of identical VMs, integrates with load balancer.
- Azure App Service (Web Apps)
  - PaaS for hosting web apps (.NET, Node, Python, Java). Autoscale, deployment slots, built-in TLS.
- Azure Kubernetes Service (AKS)
  - Managed Kubernetes; autoscaling, integrations with ACR, Azure Monitor for containers.
- Azure Functions
  - Serverless compute; event-driven (HTTP, Timer, ServiceBus). Consumption plan (pay per execution) or Premium / Dedicated.

### Storage

- Storage Account
  - Provides Blob, File, Queue, Table (Table via Cosmos now), ADLS Gen2 for big data.
- Blob storage tiers: Hot, Cool, Archive.
  - Example: upload with `az storage blob upload`.
- Azure Files: managed SMB or NFS share, mountable on VMs.
- Managed Disks: OS/data disks for VMs, snapshot/backup support.

### Networking

- Virtual Network (VNet): isolated network within Azure; subnets, NSGs to control traffic.
- Network Security Group (NSG): ACL for inbound/outbound at NIC/subnet levels.
- Azure Load Balancer: L4 load balancing (TCP/UDP) — internal and public.
- Application Gateway: L7 load balancer with WAF, path-based routing, SSL offload.
- Azure Firewall: stateful firewall as a service.
- VPN Gateway and ExpressRoute: site-to-site VPN or private dedicated connection from on-prem to Azure.
- Azure DNS, CDN, Front Door (global HTTP load balancing with WAF and caching).

### Databases & Data Services

- Azure SQL Database: PaaS relational DB with single DB / elastic pools / managed instance options.
- Azure Database for MySQL / PostgreSQL: managed open-source databases.
- Cosmos DB: globally distributed, multi-model (Core SQL, Mongo API, Gremlin, Cassandra), single-digit ms latencies.
- Azure Cache for Redis: in-memory caching.
- Synapse Analytics: data warehousing, big data analytics; integrates with Spark, SQL Pools.
- Data Factory: ETL/ELT orchestration.

### Identity & Security

- Azure Active Directory (AAD): identity and access management, SSO, conditional access.
- Managed Identities: for VMs and services to get tokens without credentials.
- Azure Key Vault: secrets and certificates management.
- Azure Policy, Blueprints, Security Center / Defender for Cloud.

### Monitoring & Management

- Azure Monitor: metrics, logs, alerts. Includes Log Analytics workspace.
- Application Insights: APM for web apps (traces, metrics, distributed traces).
- Azure Advisor: best-practice recommendations.
- Cost Management and Billing: budgets, alerts, cost analysis.

### Integration & Messaging

- Service Bus: message broker (queues, topics/subscriptions).
- Event Hubs: telemetry ingestion at scale.
- Event Grid: event routing with pub/sub model.
- Logic Apps: low-code orchestration, connectors.

### Backup & Disaster Recovery

- Azure Backup: agent-based or VM-level backup.
- Azure Site Recovery (ASR): orchestrated failover for VMs between regions or on-prem to Azure.

### Serverless & Edge

- Functions, Logic Apps, Durable Functions, Event Grid, IoT Hub, Azure Stack / Azure Arc for hybrid.

---

## 9. Regions, Availability Zones & Availability Sets

### Regions
- Geographic data center locations. Choose region based on latency, data residency, and available services.

### Availability Zones
- Physically separate datacenters within a region. Use zones to protect from datacenter-level failures.
- Example design: spread VM instances across 3 zones, use zonal managed disks, and zone-redundant services.

### Availability Sets
- Legacy model inside single datacenter: distribute VMs across update domains and fault domains to avoid simultaneous updates or hardware failures.
- Use when region lacks zones; VMs in an availability set get guaranteed SLA when configured with load-balanced endpoints.

SLA notes:
- Single VM has no SLA for compute unless in availability set or use Premium SSD and zone redundancy. Multi-instance with load balancer in availability set/zone gives higher SLA (e.g., 99.95%).

---

## 10. Resource Groups — Organization & Lifecycles

A Resource Group (RG) is a logical container for resources that share the same lifecycle, permissions and policies.

Best practices:
- Group resources with same lifecycle and owner (e.g., app + storage + network).
- Use tagging (owner, environment, cost center) for cost allocation and automation.
- Apply Azure Policy at resource group or subscription scope.
- Avoid overly large RG containing unrelated resources — makes RBAC and deletion risky.

CLI example:
```bash
az group create -n MyApp-RG -l eastus
az group delete -n MyApp-RG
```

Note: deleting an RG deletes all contained resources — use locks to protect critical RGs.

---

## 11. Azure Resource Manager (ARM) — Principles & Examples

Azure Resource Manager (ARM) is the deployment and management service for Azure. It provides:

- Declarative provisioning via JSON ARM templates or Bicep (recommended).
- Role-based access control (RBAC) and resource tagging.
- Idempotent deployments with `az deployment` or `New-AzResourceGroupDeployment`.

### ARM Template Example (skeleton)
```json
{
  "$schema": "https://schema.management.azure.com/schemas/2019-04-01/deploymentTemplate.json#",
  "contentVersion": "1.0.0.0",
  "parameters": {
    "vmName": { "type": "string", "defaultValue": "myVM" }
  },
  "resources": [
    {
      "type": "Microsoft.Compute/virtualMachines",
      "apiVersion": "2021-07-01",
      "name": "[parameters('vmName')]",
      "location": "[resourceGroup().location]",
      "properties": {
        "hardwareProfile": { "vmSize": "Standard_DS1_v2" },
        "storageProfile": {
          "imageReference": { "publisher":"Canonical","offer":"UbuntuServer","sku":"18.04-LTS","version":"latest" }
        },
        "osProfile": { "computerName":"[parameters('vmName')]", "adminUsername":"azureuser" },
        "networkProfile": { "networkInterfaces": [ { "id": "[resourceId('Microsoft.Network/networkInterfaces','myNic')]" } ] }
      }
    }
  ],
  "outputs": { "vmName": { "type": "string", "value": "[parameters('vmName')]" } }
}
```

### Bicep (recommended modern language, simpler)
```bicep
param vmName string = 'myVM'
param location string = resourceGroup().location

resource nic 'Microsoft.Network/networkInterfaces@2021-08-01' = {
  name: '${vmName}-nic'
  location: location
  properties: { /* ... */ }
}

resource vm 'Microsoft.Compute/virtualMachines@2021-07-01' = {
  name: vmName
  location: location
  properties: {
    hardwareProfile: { vmSize: 'Standard_DS1_v2' }
    networkProfile: { networkInterfaces: [ { id: nic.id } ] }
    /* ... */
  }
}
output vmName string = vm.name
```

### Deploy via CLI & PowerShell
Azure CLI:
```bash
az deployment group create -g MyRG -f azuredeploy.json -p @parameters.json
```

PowerShell (Az module):
```powershell
New-AzResourceGroupDeployment -ResourceGroupName MyRG -TemplateFile .\azuredeploy.json -TemplateParameterFile .\params.json
```

ARM allows template functions, linked templates, nested deployments, and incremental/complete modes.

---

## 12. Governance: Policies, Locks, Tags & RBAC

- Azure Policy: enforce rules across subscriptions (e.g., only allow specific VM SKUs, enforce tag presence).
  - Example: deny public IPs on NICs or require costCenter tag.
- Role-Based Access Control (RBAC): built-in roles (Owner, Contributor, Reader) and custom roles.
- Management Groups: hierarchical grouping of subscriptions for governance.
- Resource Locks: Can set `ReadOnly` or `Delete` locks to prevent accidental modification/deletion.
- Tags: key-value pairs used for cost allocation, owner, environment (`env:prod`).

Example: assign policy via CLI
```bash
az policy assignment create --policy "/providers/Microsoft.Authorization/policyDefinitions/..." --name "enforce-tags" --scope "/subscriptions/<id>/resourceGroups/MyRG"
```

---

## 13. Cost Management & Pricing Considerations

- Azure Pricing Calculator (estimate cost) and Cost Management for analysis and budgets.
- Pricing levers:
  - Pay-as-you-go vs Reserved Instances (1 or 3 years) vs Spot instances (preemptible).
  - Use auto-scale to reduce idle resources.
  - Storage tiering (Hot/Cool/Archive).
  - Use Serverless (Functions) for spiky workloads.
- Tags and resource grouping enable cost allocation.
- Alerts: create budgets and alerts via Cost Management.

Example budget alert via CLI:
```bash
az consumption budget create --amount 1000 --time-grain Monthly --name "TeamBudget" --scope "/subscriptions/<id>"
```

---

## 14. Security & Compliance Essentials

- Identity first: use Azure AD, enforce MFA, Conditional Access.
- Use Managed Identities for resources to avoid secrets.
- Key Vault for secrets and keys; integrate with AAD and RBAC.
- Network security: NSGs, Azure Firewall, WAF on Application Gateway, private endpoints.
- Encryption: at-rest (managed by Azure) and in-transit (TLS). Customer-managed keys via Key Vault (CMK).
- Vulnerability & posture: use Microsoft Defender for Cloud, Azure Security Center recommendations.
- Compliance: Azure provides many certifications (ISO, SOC, GDPR) — use Compliance Manager for mapping.

---

## 15. Operational Tips & Best Practices

- Use Infrastructure-as-Code (ARM templates / Bicep / Terraform) for reproducible deployments.
- Separate environments (Dev/Test/Prod) in different resource groups or subscriptions.
- Use Service Principals or Managed Identities for automation (avoid user credentials).
- Automate monitoring & alerts; centralize logs in Log Analytics.
- Use deployment slots for App Service to swap releases with zero-downtime.
- Implement health probes for load balancers, and set graceful shutdown handling in apps.
- Practice disaster recovery: define RTO/RPO, replicate critical data, test failovers.
- Secure devops: store secrets in Key Vault and integrate with CI/CD (Azure DevOps, GitHub Actions).
- Tag resources for ownership, environment, project and cost center.

---

## 16. Short Q&A — Key Concepts & Interview Questions

Q: What is an Azure Resource Group?  
A: A logical container that holds related resources and provides a scope for RBAC and lifecycle operations.

Q: When should you use Availability Zones vs Availability Sets?  
A: Use Availability Zones when the region supports them and you need higher resiliency (zonal separation). Use Availability Sets when zones are not available; sets protect against rack-level failures and planned maintenance.

Q: What is Azure Resource Manager (ARM)?  
A: ARM is the control plane for Azure that provides a consistent API for resource deployment and management. It supports declarative templates (ARM/Bicep) and RBAC/policies.

Q: What is the difference between Azure App Service and AKS?  
A: App Service is PaaS for web apps with managed platform features. AKS is managed Kubernetes for container orchestration and microservices; more control and complexity.

Q: How do you secure an Azure VM?  
A: Use NSGs, Azure Firewall, patch OS, use Azure Defender, enable disk encryption (BitLocker/DM-Crypt) with Key Vault, restrict management ports via Just-In-Time (JIT), use Managed Identity for app secrets, apply monitoring.

Q: How to avoid unexpected cloud cost spikes?  
A: Use budgets, alerts, autoscale, reserved/spot instances where applicable, tagging and periodic cost review.

---

## 17. References & Further Reading

- Microsoft Learn — Azure fundamentals: https://learn.microsoft.com/azure/  
- Azure Architecture Center — best practices & reference architectures: https://learn.microsoft.com/azure/architecture/  
- ARM template reference & Bicep docs: https://learn.microsoft.com/azure/azure-resource-manager/  
- Azure CLI & PowerShell docs: https://learn.microsoft.com/cli/azure & https://learn.microsoft.com/powershell/azure/  
- Azure Well-Architected Framework — cost, reliability, performance, security, operational excellence.

---

Prepared as a practical and interview-ready reference for Microsoft Azure fundamentals, core services, resource management and governance. If you'd like, I can:  
- generate an ARM / Bicep starter template for a sample 3-tier app,  
- produce a one-page Azure architecture checklist for production readiness, or  
- create a short hands-on lab (CLI & Portal steps) to provision a VNet, subnet, VM, and basic monitoring. Which would you like next?