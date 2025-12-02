# Azure Storage Services — Deep Reference  
Comprehensive notes, architecture, examples (CLI / PowerShell / SDK), best practices and operational guidance

---

## Table of Contents

1. [Purpose & Audience](#purpose--audience)  
2. [Types of Data: Structured, Semi-structured, Unstructured](#types-of-data-structured-semi-structured-unstructured)  
3. [Overview — Azure Storage Platform](#overview---azure-storage-platform)  
4. [Storage Account Types & SKUs](#storage-account-types--skus)  
5. [Redundancy & Durability Options](#redundancy--durability-options)  
6. [Authentication, Authorization & Security](#authentication-authorization--security)  
7. [Azure Blob Storage (Block / Append / Page Blobs)](#azure-blob-storage-block--append--page-blobs)  
   - Blob tiers, lifecycle, versioning, soft delete, immutability, leases  
   - Common scenarios & best practices  
   - Examples: az CLI, AzCopy, .NET, Python, Node SDK snippets  
8. [Azure Files (SMB / NFS / REST)](#azure-files-smb--nfs--rest)  
   - Mounting (Windows/Linux), Azure File Sync, snapshots, secrets (SAS / AD auth)  
   - Examples: mount commands, CLI creation, SDK snippets  
9. [Azure Queues (Queue Storage)](#azure-queues-queue-storage)  
   - Message model, visibility timeout, poison messages, scaling considerations  
   - Examples: CLI and SDK (C#, Python)  
10. [Azure Table Storage (NoSQL Key-Value)](#azure-table-storage-nosql-key-value)  
    - PartitionKey/RowKey model, scalability, OData REST, Azure.Data.Tables SDK  
    - When to use vs Cosmos DB Table API  
    - Examples: SDK snippet  
11. [Azure Disks (Managed Disks)](#azure-disks-managed-disks)  
    - Disk types, performance, snapshots, incremental snapshots, encryption, ephemeral OS disks  
    - Attach/detach, resizing considerations  
12. [Azure Data Lake Storage Gen2 (ADLS Gen2)](#azure-data-lake-storage-gen2-adls-gen2)  
    - Hierarchical Namespace (HNS), POSIX-style ACLs, integration with big data tooling  
    - Use-cases and performance considerations  
13. [Blob Storage & Azure Key Vault Integration](#blob-storage--azure-key-vault-integration)  
    - Customer-managed keys (CMK), Key Vault, Managed Identities, encryption at rest  
14. [Azure Storage SDKs & REST API](#azure-storage-sdks--rest-api)  
    - Languages supported and common patterns (DefaultAzureCredential, SAS, client libraries)  
    - Example code for uploads and downloads (.NET, Python, Node.js)  
15. [Operations & Tooling: Az CLI, PowerShell, Storage Explorer, AzCopy](#operations--tooling-az-cli-powershell-storage-explorer-azcopy)  
16. [Monitoring, Logging & Diagnostics](#monitoring-logging--diagnostics)  
17. [Cost, Performance & Capacity Considerations](#cost-performance--capacity-considerations)  
18. [Best Practices, Security & Compliance](#best-practices-security--compliance)  
19. [Common Patterns & Reference Commands / Snippets](#common-patterns--reference-commands--snippets)  
20. [References & Further Reading](#references--further-reading)

---

## 1. Purpose & Audience

This file is a comprehensive practical reference for engineers, architects and cloud operators working with Azure Storage. It covers service features, data types, account models, redundancy, security, data-lifecycle management, SDK usage, operational commands and monitoring.

---

## 2. Types of Data: Structured, Semi-structured and Unstructured

- Structured data
  - Rigid schema and tabular (SQL databases). Examples: relational rows.
  - Azure: Azure SQL, Cosmos DB (with strict schemas if chosen).

- Semi-structured data
  - Self-describing records where structure can vary (JSON, XML). Examples: logs, telemetry.
  - Azure: Blob storage for JSON files, ADLS Gen2 for hierarchical storage used by analytics, Cosmos DB.

- Unstructured data
  - Files and binary objects without predefined schema (images, videos, backups).
  - Azure: Blob storage (object storage), Azure Files (file shares) for shared file system.

Azure Storage is primarily optimized for large-scale object and file storage (semi-structured & unstructured), while Table Storage provides a simple structured (NoSQL) store.

---

## 3. Overview — Azure Storage Platform

Azure Storage provides a family of services under a Storage Account boundary:

- Blob storage — object store for block, append and page blobs.
- File storage — managed SMB/NFS file shares (Azure Files).
- Queue storage — message queue for decoupling components.
- Table storage — NoSQL key-value store (PartitionKey/RowKey).
- Disk storage — Managed disks attached to VMs.
- Data Lake Storage Gen2 — hierarchical object store with POSIX-like ACLs for analytics.

Access methods:
- REST API (HTTPS)
- SDKs for .NET, Java, Python, JavaScript/Node, Go, Ruby, PHP
- CLI (`az storage`), PowerShell (`Az.Storage`), AzCopy and Storage Explorer

---

## 4. Storage Account Types & SKUs

Primary account kinds:

- StorageV2 (general purpose v2) — recommended; supports blobs, files, queues, tables, lifecycle, soft delete, tiering.
- BlockBlobStorage — optimized for large block blobs; premium for low-latency block blob workloads.
- FileStorage — premium file share SKU for high throughput/IOPS.
- Storage (classic) — legacy (avoid).

Tiers (Blob):
- Hot — frequent access, highest storage cost but low access cost.
- Cool — infrequent access, lower storage cost, higher access cost and minimum retention.
- Archive — long-term, lowest storage cost, high retrieval latency and cost (rehydrate to Hot/Cool first).

Performance tiers for disks and premium block blobs (Provisioned IOPS/throughput).

---

## 5. Redundancy & Durability Options

Replication controls data durability and geographic redundancy:

- LRS (Locally-redundant storage): 3 copies in single region (rack-aware).
- ZRS (Zone-redundant storage): replicates across availability zones within region.
- GRS (Geo-redundant storage): LRS + asynchronous replication to paired region (read access only after failover).
- RA-GRS (Read-access GRS): read access to secondary region.
- GZRS / RA-GZRS: combination of zone and geo redundancy for higher resiliency.

Choose based on RPO/RTO, cost, compliance and read patterns.

---

## 6. Authentication, Authorization & Security

Primary methods:
- **Shared Key**: account key — full admin access; rotate frequently; avoid hard-coding.
- **Shared Access Signatures (SAS)**: scoped, time-limited tokens for delegated access (service SAS, account SAS, user delegation SAS).
- **Azure AD (OAuth 2.0)**: preferred for managed identities & RBAC; integrates with Azure AD to grant permissions (RBAC roles such as Storage Blob Data Contributor/Reader).
- **Managed Identities**: for VMs/Functions/App Service to obtain tokens and access storage without secrets.
- **Azure Key Vault**: store account keys or customer-managed keys (CMK) for encryption.

Security features:
- Encryption at rest (Storage service encryption) — Azure-managed keys by default.
- Customer-managed keys (CMK) in Key Vault for BYOK (see section on Key Vault).
- HTTPS enforced for REST access; allow secure transfer required.
- Private Endpoints (Private Link) — network-level private access with private IPs.
- Firewall and virtual network rules on storage account to restrict network access.
- Soft Delete and Immutable Blob Storage (WORM) for retention and legal hold.

---

## 7. Azure Blob Storage (Block / Append / Page Blobs)

### Blob types
- **Block blobs**: optimized for large media and files; upload blocks and commit; best for general object storage.
- **Append blobs**: optimized for append-only scenarios (logs). Supports append block operations.
- **Page blobs**: random read/write and used mainly for VHDs and Azure managed disks (512-byte pages) — less common to manage directly in Blob Storage.

### Features
- **Access tiers**: Hot/Cool/Archive at container/blob level.
- **Blob-level tiering**: set access tier per blob; lifecycle rules can transition between tiers.
- **Blob versioning**: maintain versions of blobs automatically.
- **Soft delete**: recover deleted blobs for retention period.
- **Snapshot**: point-in-time, read-only copy of blob — useful for backups.
- **Immutability & legal hold**: WORM policies and legal holds for compliance retention.
- **Blob leases**: exclusive lease (60s default, infinite) to coordinate concurrency (e.g., for leader election or safe deletes).
- **Customer-provided encryption & CMK** via Storage Account or encryption scope.

### Concurrency & consistency
- Azure Storage provides strong consistency. Use blob leases or conditional headers (If-Match ETag) for concurrency control.

### Example: create storage account, create container, upload blob (CLI + AzCopy)
```bash
# Create storage account
az storage account create -g rg -n mystorageacct -l eastus --sku Standard_RAGRS --kind StorageV2

# Create blob container (public access off)
az storage container create --name mycontainer --account-name mystorageacct --auth-mode login

# Upload file (AzCopy recommended)
azcopy copy "/local/path/largefile.bin" "https://mystorageacct.blob.core.windows.net/mycontainer/largefile.bin"
# or using az cli (needs --auth-mode login or account key)
az storage blob upload --container-name mycontainer --file ./largefile.bin --name largefile.bin --account-name mystorageacct
```

### SDK Examples

#### .NET (Azure.Storage.Blobs)
```csharp
using Azure.Identity;
using Azure.Storage.Blobs;
using System.Threading.Tasks;

var accountUrl = "https://mystorageacct.blob.core.windows.net";
var client = new BlobServiceClient(new Uri(accountUrl), new DefaultAzureCredential());

var container = client.GetBlobContainerClient("mycontainer");
await container.CreateIfNotExistsAsync();

var blob = container.GetBlobClient("file.txt");
using var stream = File.OpenRead("file.txt");
await blob.UploadAsync(stream, overwrite: true);
```

#### Python (azure-storage-blob)
```python
from azure.identity import DefaultAzureCredential
from azure.storage.blob import BlobServiceClient

account_url = "https://mystorageacct.blob.core.windows.net"
service = BlobServiceClient(account_url=account_url, credential=DefaultAzureCredential())
container = service.get_container_client("mycontainer")
container.create_container()
with open("file.txt","rb") as f:
    container.upload_blob(name="file.txt", data=f, overwrite=True)
```

#### Node.js (@azure/storage-blob)
```javascript
const { BlobServiceClient } = require('@azure/storage-blob');
const { DefaultAzureCredential } = require('@azure/identity');

const serviceClient = new BlobServiceClient("https://mystorageacct.blob.core.windows.net", new DefaultAzureCredential());
const containerClient = serviceClient.getContainerClient("mycontainer");
await containerClient.create();
const blockBlobClient = containerClient.getBlockBlobClient("file.txt");
await blockBlobClient.uploadFile("./file.txt");
```

---

## 8. Azure Files (SMB / NFS / REST)

Azure Files provides fully managed file shares accessible via SMB (Windows/Linux) and NFS (v4.1) for premium file shares. Support includes mounting on VMs, containers, and on-premises via File Sync.

### Features and options
- Protocols: SMB 3.0 (encryption in transit) for Windows mounting; NFS 4.1 supported on premium file shares for Linux workloads.
- **Azure File Sync**: cache and sync between on-premises Windows Server and Azure Files; tiering of cold files to cloud.
- Snapshots: share-level snapshots for point-in-time restore.
- Authentication:
  - SMB: Storage access key or AD DS integration (Azure AD Domain Services or on-prem AD with Kerberos for identity-based auth).
  - REST access via SAS tokens.
- Access tiers (Premium HDD/SSD for high IOPS and throughput).

### Mount examples

#### Windows (SMB)
```powershell
# Using storage account key
$acctName = "mystorageacct"
$key = (az storage account keys list -g rg -n $acctName --query '[0].value' -o tsv)
net use Z: "\\mystorageacct.file.core.windows.net\myshare" /u:Azure\$acctName $key
```

#### Linux (SMB)
```bash
sudo mkdir /mnt/azfiles
sudo mount -t cifs //mystorageacct.file.core.windows.net/myshare /mnt/azfiles -o vers=3.0,username=mystorageacct,password=<key>,dir_mode=0777,file_mode=0777,serverino
```

#### NFS (Linux, premium file shares)
```bash
sudo mount -t nfs -o vers=4.1 mystorageacct.file.core.windows.net:/myshare /mnt/azfiles
```

### Use-cases
- Lift-and-shift file shares, home directories, application file shares, container persistent volumes, database backups (not for DB data files), and integration with on-prem via Azure File Sync.

---

## 9. Azure Queues (Queue Storage)

- Simple message queue for decoupling producers and consumers. Not the same as Service Bus (which has advanced broker features).
- Message model: messages up to 64 KB (or larger via other patterns), visibility timeout, dequeue count.
- TTL and poison messages: messages can be retried; move to dead-letter via application logic if exceed dequeue threshold.
- Use for asynchronous work item processing with worker VMs / Functions.
- Throughput: suitable for medium-scale workloads; for advanced messaging use Service Bus.

### Example: send & receive via CLI and .NET SDK

#### CLI (az)
```bash
# create queue
az storage queue create --name myqueue --account-name mystorageacct

# enqueue message
az storage message put --queue-name myqueue --content "process-order:1234" --account-name mystorageacct

# peek messages
az storage message peek --queue-name myqueue --account-name mystorageacct

# receive (dequeue)
az storage message get --queue-name myqueue --num-messages 1 --account-name mystorageacct
```

#### .NET (Azure.Storage.Queues)
```csharp
using Azure.Storage.Queues;
var queueClient = new QueueClient(new Uri("https://mystorageacct.queue.core.windows.net/myqueue"), new DefaultAzureCredential());
await queueClient.CreateIfNotExistsAsync();
await queueClient.SendMessageAsync("process-order:1234");
var msg = await queueClient.ReceiveMessageAsync();
await queueClient.DeleteMessageAsync(msg.Value.MessageId, msg.Value.PopReceipt);
```

---

## 10. Azure Table Storage (NoSQL Key-Value)

- **Schema-less** key-value store keyed by `PartitionKey` (shard) and `RowKey` (unique within partition) with OData REST support. Suitable for large volumes of structured entities where simple queries by PK are primary.
- Strong throughput when partitioning strategy is well-designed (even distribution across partitions).
- For advanced features, multi-region low-latency globally-distributed NoSQL may require **Cosmos DB Table API**.

### Example (.NET: Azure.Data.Tables)
```csharp
using Azure.Data.Tables;
var serviceClient = new TableServiceClient(new Uri("https://mystorageacct.table.core.windows.net"), new DefaultAzureCredential());
var tableClient = serviceClient.GetTableClient("customers");
await tableClient.CreateIfNotExistsAsync();

var entity = new TableEntity("USA","123") { {"Name","Alice"}, {"Age",30} };
await tableClient.AddEntityAsync(entity);
var retrieved = await tableClient.GetEntityAsync<TableEntity>("USA","123");
```

---

## 11. Azure Disks (Managed Disks)

- Managed disks provide durable block storage for VMs; types: Standard HDD, Standard SSD, Premium SSD, Premium v2, Ultra Disk.
- Features:
  - Snapshots (full, incremental support)
  - Disk encryption and Disk Encryption Sets
  - Resize online: expand disk size in Azure then grow partition/filesystem inside the VM
  - Performance characteristics depend on disk size/sku and VM size
  - Ephemeral OS disks: local to host (very low latency, not persisted on deallocate) — used for stateless or cached workloads (VMSS).
- Use-cases: OS disk, data disks for databases, separate disks for logs/tempdb for performance tuning.

### CLI Example: create and attach managed disk
```bash
az disk create -g rg -n dataDisk1 --size-gb 512 --sku Premium_LRS
az vm disk attach -g rg --vm-name myvm --disk dataDisk1 --new
```

---

## 12. Azure Data Lake Storage Gen2 (ADLS Gen2)

- ADLS Gen2 is built on Blob Storage (StorageV2) with Hierarchical Namespace (HNS) enabled to provide filesystem semantics (directories, atomic rename) and POSIX-like ACLs.
- Targeted for big data analytics workloads: Databricks, HDInsight, Synapse, Spark.
- Features:
  - Hierarchical namespace -> faster directory operations
  - POSIX-style Access Control Lists (ACLs)
  - Tiering & lifecycle management via blob features
  - Integration with Azure Data Factory, Synapse, and compute engines
- Security: supports Azure AD auth, managed identities, ACLs for fine-grained access.

### Example: create account with HNS (CLI)
```bash
az storage account create -n myadlsacct -g rg -l eastus --kind StorageV2 --hierarchical-namespace true --sku Standard_RAGRS
```

---

## 13. Blob Storage & Azure Key Vault Integration

- **Customer-managed keys (CMK)**: use Key Vault (or Managed HSM) to hold encryption keys used to encrypt storage account data (encryption at rest with CMEK).
- **Storage account encryption**:
  - By default: Storage Service Encryption (SSE) with Microsoft-managed keys.
  - To bring your own key (BYOK): configure Key Vault key as encryption scope or set the account to use a Key Vault key (requires Key Vault soft-delete and permissions).
- **SAS token keys**: store SAS signing keys securely. Rotate keys and/or use user-delegation SAS via Azure AD to reduce risk.

### Example: grant Key Vault access to storage account
- Use Azure Portal or CLI to set Key Vault key permissions and link to storage account encryption scope. The storage account's managed identity must have `wrapKey`/`unwrapKey` permissions in Key Vault.

---

## 14. Azure Storage SDKs & REST API

Azure Storage exposes REST APIs for each service. Official client libraries are available and recommended:

- Libraries (modern):
  - .NET: `Azure.Storage.Blobs`, `Azure.Storage.Files.Shares`, `Azure.Storage.Queues`, `Azure.Data.Tables`
  - Python: `azure-storage-blob`, `azure-storage-file-share`, `azure-storage-queue`, `azure-data-tables`
  - Java: `com.azure:azure-storage-blob` etc.
  - Node: `@azure/storage-blob`, `@azure/storage-file-share`, `@azure/data-tables`
  - Go: `github.com/Azure/azure-sdk-for-go/sdk/storage/...`
- Common auth pattern:
  - `DefaultAzureCredential` (works with Managed Identity, CLI auth, VS credentials) from `@azure/identity` or `Azure.Identity` in .NET.
  - SAS token for scoped delegated access.
  - Shared key for full account administration (least preferred for long term).

### Example: Generate user-delegation SAS (.NET)
```csharp
var containerClient = new BlobContainerClient(new Uri("https://..."), new DefaultAzureCredential());
var userDelegationKey = await containerClient.GetUserDelegationKeyAsync(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(1));
var sas = containerClient.GenerateSasUri(BlobContainerSasPermissions.Read, DateTimeOffset.UtcNow.AddHours(1), userDelegationKey.Value);
```

---

## 15. Operations & Tooling: Az CLI, PowerShell, Storage Explorer, AzCopy

- **Az CLI**: `az storage account`, `az storage blob`, `az storage container` etc. Supports interactive login (`az login`).
- **PowerShell**: `Az.Storage` module with cmdlets: `New-AzStorageAccount`, `Get-AzStorageAccountKey`, `Get-AzStorageBlobContent`
- **AzCopy**: high-performance data transfer tool for uploading/downloading/copying between storage accounts and between on-prem and cloud. Example:
  ```bash
  azcopy copy "/local/dir/*" "https://mystorageacct.blob.core.windows.net/mycontainer?sas-token" --recursive
  ```
- **Storage Explorer**: GUI tool for browsing and operating storage accounts locally or signed in with your subscription.

Use AzCopy for large transfers; use SDK/REST for application integration.

---

## 16. Monitoring, Logging & Diagnostics

- **Metrics**: Transactions, Egress (GB), Ingress, Availability, SuccessE2ELatency, ServerLatency, Capacity.
- **Diagnostic logs**: send to Log Analytics, Event Hubs, or Storage account via Diagnostic Settings.
- **Blob-Level logging**: access logs (storage analytics is being deprecated for some features; use diagnostic settings & Log Analytics).
- **Alerts**: set metric alerts for errors, high egress, low availability, or capacity thresholds; set activity log alerts for management plane operations.
- **Tools**: Azure Monitor, Storage Analytics (legacy), Log Analytics + Kusto queries, Network Watcher for networking-related storage diagnostics.

Example to enable diagnostics to Log Analytics (CLI):
```bash
az monitor diagnostic-settings create --resource /subscriptions/<sub>/resourceGroups/rg/providers/Microsoft.Storage/storageAccounts/mystorageacct --workspace /subscriptions/<sub>/resourceGroups/rg/providers/Microsoft.OperationalInsights/workspaces/myWorkspace --logs '[{"category":"StorageRead","enabled":true},{"category":"StorageWrite","enabled":true}]' --metrics '[{"category":"All","enabled":true}]'
```

---

## 17. Cost, Performance & Capacity Considerations

- Costs: storage capacity (GB), requests (PUT/GET), egress (GB), operations like copy/restore, snapshot storage, snapshots incremental cost.
- Performance: choose correct disk/disk size, premium/ultra for low latency; use multiple data disks striped (or use managed storage features) as required.
- Throughput limits: per storage account and per blob/disc IO/throughput. For high throughput, use Premium/Ultra accounts or partition data across accounts/containers and use parallelism (AzCopy, SDK concurrency).
- For analytics (ADLS), choose appropriate hot/cold tiers and retention/lifecycle policies to minimize egress and storage costs.
- Avoid frequent rehydrations from Archive (costly and slow).

---

## 18. Best Practices, Security & Compliance

- Prefer Azure AD + RBAC + Managed Identities for app authentication.
- Use SAS for limited lifetime delegated access and user-delegation SAS where possible.
- Enable soft delete, versioning and object-level immutability if compliance requires.
- Use Private Endpoints and firewall rules to limit access to storage account to VNets only.
- Rotate account keys or disable shared key access entirely; use Key Vault and encryption scopes for CMK.
- Monitor and alert on anomalous access patterns and large egress.
- For PII or regulated data, use CMK and audit logs for compliance.

---

## 19. Common Patterns & Reference Commands / Snippets

- Create storage account (CLI)
```bash
az storage account create -g rg -n mystorageacct -l eastus --sku Standard_LRS --kind StorageV2 --https-only true
```

- Create container and set public access off (recommended)
```bash
az storage container create --name mycontainer --account-name mystorageacct --auth-mode login --public-access off
```

- Upload using AzCopy (fast & resumable)
```bash
azcopy copy "/local/data/*" "https://mystorageacct.blob.core.windows.net/mycontainer?<SAS>" --recursive
```

- Generate account SAS (dangerous: high privileges) — prefer user-delegation SAS
```bash
az storage account generate-sas -n mystorageacct --permissions rwdlacup --services b --resource-types sco --expiry 2025-12-31T23:59:00Z --https-only
```

- Mount Azure File share on Linux (using key)
```bash
sudo mount -t cifs //mystorageacct.file.core.windows.net/myshare /mnt/myshare -o vers=3.0,username=mystorageacct,password=<account-key>,dir_mode=0777,file_mode=0777,serverino
```

- Create ADLS Gen2 account (hierarchical namespace)
```bash
az storage account create -n myadlsacct -g rg -l eastus --kind StorageV2 --hierarchical-namespace true --sku Standard_GZRS
```

---

## 20. References & Further Reading

- Azure Storage overview: https://learn.microsoft.com/azure/storage/  
- Blobs: https://learn.microsoft.com/azure/storage/blobs/  
- Files: https://learn.microsoft.com/azure/storage/files/  
- Queues: https://learn.microsoft.com/azure/storage/queues/  
- Tables: https://learn.microsoft.com/azure/storage/tables/  
- Managed Disks: https://learn.microsoft.com/azure/virtual-machines/disks-types  
- ADLS Gen2: https://learn.microsoft.com/azure/storage/blobs/data-lake-storage-introduction  
- AzCopy: https://learn.microsoft.com/azure/storage/common/storage-use-azcopy-v10  
- Azure Storage SDKs: https://learn.microsoft.com/azure/storage/common/storage-sdks

---

Prepared as a detailed, practical operational and architectural reference to Azure Storage services including Blob/File/Table/Queue/Disk/ADLS, security, SDK patterns and example commands. If you want, I can:

- produce a Bicep template for a storage account + ADLS + File share + lifecycle policies,  
- create sample micro-app code (producer/consumer) that uses Queues + Blob storage in .NET/Python, or  
- generate a troubleshooting playbook (step-by-step commands to recover common issues, e.g., "accidentally deleted blob" or "mount failure").  

Which would you like next?