# Azure Virtual Machines — Deep Dive: Create, Manage, Disks, Images, Scale Sets & Load Balancing

---

## Table of Contents

1. [Purpose & Audience](#purpose--audience)  
2. [High-level Overview — What is an Azure VM?](#high-level-overview---what-is-an-azure-vm)  
3. [VM SKUs, Sizes & Families — Choosing the right VM](#vm-skus-sizes--families---choosing-the-right-vm)  
4. [Networking & Security Primitives for VMs](#networking--security-primitives-for-vms)  
5. [Creating a Virtual Machine — Options & Examples](#creating-a-virtual-machine---options--examples)  
   - Azure Portal (steps)  
   - Azure CLI examples  
   - PowerShell (Az module) examples  
   - ARM template / Bicep snippets  
6. [Managing Virtual Machines — Day-2 Operations](#managing-virtual-machines---day-2-operations)  
   - Start/Stop/Restart/Deallocate  
   - Resize / Change size and SKU  
   - Update OS, Extensions, VM Agent, and Custom Script Extension  
   - Diagnostics: Boot diagnostics, serial console, boot logs  
   - OS patching strategies & update management  
   - Backups and snapshots  
   - VM encryption and key management  
7. [Creating and Managing Disks](#creating-and-managing-disks)  
   - Disk types: Standard HDD, Standard SSD, Premium SSD, Ultra Disk, Ephemeral OS Disk  
   - Managed Disks vs Unmanaged Disks (legacy)  
   - Creating, attaching, detaching disks (CLI/PowerShell)  
   - Resize, change performance tier, snapshots, backup and restore  
   - Disk Encryption Sets and encryption at rest  
8. [Creating and Managing Images](#creating-and-managing-images)  
   - Capture a VM image (generalize / specialized)  
   - Shared Image Gallery and versioning (best practices)  
   - Create VM from custom image (CLI/PowerShell/Bicep)  
   - Golden image pipeline tips (Packer, Azure Image Builder)  
9. [Creating and Managing Scale Sets (VMSS)](#creating-and-managing-scale-sets-vmss)  
   - VMSS overview and use-cases  
   - VMSS orchestration modes: Uniform vs Flexible  
   - VMSS + autoscale, health probes, custom images, managed identities  
   - Example: create VMSS with custom image & autoscale (CLI/Bicep)  
   - Rolling upgrades and upgrade policies (manual, automatic, rolling)  
10. [Load Balancing Virtual Machines](#load-balancing-virtual-machines)  
    - Azure Load Balancer (Basic vs Standard) — L4 load balancing  
    - Application Gateway — L7, WAF, path-based routing  
    - Traffic Manager vs Front Door — DNS/global traffic management vs global HTTP(s) load balancing & edge  
    - Health probes, backend pools, NAT rules, inbound/outbound patterns  
    - Example: public LB + NAT + probe (CLI snippets)  
11. [Monitoring, Logging & Alerts](#monitoring-logging--alerts)  
    - Azure Monitor, Diagnostic settings, Metrics, Log Analytics, Alerts  
    - Guest-level monitoring: Azure Monitor agent, VM Insights, dependency mapping  
12. [Availability, Resiliency & SLAs](#availability-resiliency--slas)  
    - Availability Zones vs Availability Sets vs Zone-redundant services  
    - SLA considerations for single VM, VMSS, and sets of VMs behind load balancer  
13. [Security Best Practices](#security-best-practices)  
    - Network access controls (NSG & ASG), Just-In-Time VM access, Azure Firewall, Private Endpoints  
    - Managed identity and secrets (Key Vault)  
    - Hardened images, vulnerability assessment (Microsoft Defender for Cloud)  
14. [Cost Management & Sizing Recommendations](#cost-management--sizing-recommendations)  
    - Reserved Instances, Spot VMs, Auto-shutdown, right-sizing, disk tiers  
15. [Troubleshooting Checklist & Common Issues](#troubleshooting-checklist--common-issues)  
16. [Appendix — Useful CLI & PowerShell Commands (Cheat Sheet)](#appendix---useful-cli--powershell-commands-cheat-sheet)  
17. [References & Further Reading](#references--further-reading)

---

## 1. Purpose & Audience

This is a thorough, practical reference for provisioning and operating Azure Virtual Machines (VMs). It includes step-by-step examples (Azure CLI, PowerShell, ARM/Bicep), operational guidance for disks, images, scale sets, load balancing, monitoring, resiliency, and security best practices. Intended for cloud engineers, SREs, platform teams, and architects.

---

## 2. High-level Overview — What is an Azure VM?

An Azure Virtual Machine is an Infrastructure-as-a-Service (IaaS) compute resource: a virtualized server that runs in Microsoft Azure's datacenters. VMs provide OS-level control (Windows/Linux), and you manage the OS, runtime, and applications. Azure provides underlying compute, networking, and durable storage (Managed Disks) and offers services for scale, availability, and management.

Key responsibilities:
- You manage the VM OS, patches, security, installed software and user access.
- Azure manages hypervisor, physical hosts, and platform networking.

---

## 3. VM SKUs, Sizes & Families — Choosing the right VM

Azure offers families tuned for workloads:

- General purpose: B-series (burstable), D-series (balanced CPU/memory), DS-series (with premium storage)
- Compute optimized: F-series (high CPU per core)
- Memory optimized: E-series, M-series (large memory)
- Storage optimized: Lsv2 (high IOPS and throughput)
- GPU: NV, NC, ND series for graphics, AI and compute
- Confidential Computing: DC-series
- High-performance: HB/HPC, HBv3 for HPC workloads

Consider:
- vCPU count and generation (newer CPUs deliver better IPC)
- Memory-to-CPU ratio
- Local ephemeral storage (some SKUs provide temp disk)
- Network bandwidth and Accelerated Networking support (SR-IOV)
- Max disk count and throughput per VM
- Pricing and availability per region

Use `az vm list-skus` to view sizes available in a region:
```bash
az vm list-skus --location eastus --output table --size
```

---

## 4. Networking & Security Primitives for VMs

- Virtual Network (VNet) and subnets provide segmentation and IP address allocation.
- Network Security Group (NSG) controls inbound/outbound traffic by rules (5-tuple).
- Application Security Group (ASG) groups VMs for simplification of NSG rules.
- Public IP (dynamic/static) — assign to NIC or Load Balancer frontend.
- Private Endpoint / Private Link: access PaaS privately over VNet.
- Azure Firewall and Azure Firewall Manager for perimeter control.
- Just-In-Time (JIT) VM access via Azure Security Center reduces exposure to management ports.

Important VM NIC settings:
- IP forwarding, Accelerated Networking, DNS servers, NIC-level NSG.

---

## 5. Creating a Virtual Machine — Options & Examples

There are multiple ways to provision a VM:

- Azure Portal (GUI)
- Azure CLI (`az vm`)
- PowerShell (Az module)
- ARM templates or Bicep (declarative infrastructure-as-code)
- SDKs (Python, .NET) and Terraform

### Azure Portal steps (concise)
1. Resource Group → Create
2. Create resource → Virtual Machine
3. Choose Image (Ubuntu, Windows Server), Size, Authentication (SSH key or password)
4. Configure Disks (OS disk type, encryption), and optionally attach Data disks
5. Networking: VNet, Subnet, Public IP, NIC NSG
6. Management: Monitoring (boot diagnostics), auto-shutdown, backup
7. Review + Create

### Azure CLI (example: Ubuntu VM with SSH key)
```bash
# variables
RG="myResourceGroup"
LOC="eastus"
VM="myVM"
USERNAME="azureuser"
SSH_KEY_PATH="$HOME/.ssh/id_rsa.pub"

az group create -n $RG -l $LOC

az vm create \
  --resource-group $RG \
  --name $VM \
  --image UbuntuLTS \
  --size Standard_B2s \
  --admin-username $USERNAME \
  --ssh-key-values "$SSH_KEY_PATH" \
  --storage-sku Premium_LRS \
  --public-ip-sku Standard \
  --output json
```

Notes:
- Use Standard SKU for public IP and Load Balancer compatibility.
- Set `--storage-sku` to choose OS disk performance.

### PowerShell (Az module)
```powershell
Connect-AzAccount
$rg = "myResourceGroup"
$loc = "eastus"
New-AzResourceGroup -Name $rg -Location $loc

$cred = Get-Credential -Message "Enter username and password"
New-AzVm `
  -ResourceGroupName $rg `
  -Name "myVM" `
  -Location $loc `
  -Image "Win2019Datacenter" `
  -Credential $cred `
  -Size "Standard_DS2_v2"
```

### ARM / Bicep snippet (Bicep)
```bicep
param vmName string = 'myvm'
param adminUser string = 'azureuser'
param adminPublicKey string

resource vnet 'Microsoft.Network/virtualNetworks@2021-05-01' = {
  name: '${vmName}-vnet'
  location: resourceGroup().location
  properties: { addressSpace: { addressPrefixes: ['10.0.0.0/16'] } }
}

resource nic 'Microsoft.Network/networkInterfaces@2021-05-01' = {
  name: '${vmName}-nic'
  location: resourceGroup().location
  properties: {
    ipConfigurations: [
      {
        name: 'ipconfig1'
        properties: {
          subnet: { id: vnet::subnets[0].id } // assume subnet created
          privateIPAllocationMethod: 'Dynamic'
          publicIPAddress: {
            id: resourceId('Microsoft.Network/publicIPAddresses','${vmName}-pip')
          }
        }
      }
    ]
  }
}

resource vm 'Microsoft.Compute/virtualMachines@2021-07-01' = {
  name: vmName
  location: resourceGroup().location
  properties: {
    hardwareProfile: { vmSize: 'Standard_DS1_v2' }
    storageProfile: {
      imageReference: { publisher: 'Canonical', offer: 'UbuntuServer', sku: '18_04-lts', version: 'latest' }
      osDisk: { createOption: 'FromImage', managedDisk: { storageAccountType: 'Premium_LRS' } }
    }
    osProfile: {
      computerName: vmName
      adminUsername: adminUser
      linuxConfiguration: {
        disablePasswordAuthentication: true
        ssh: { publicKeys: [ { path: '/home/${adminUser}/.ssh/authorized_keys', keyData: adminPublicKey } ] }
      }
    }
    networkProfile: { networkInterfaces: [ { id: nic.id } ] }
  }
}
```

---

## 6. Managing Virtual Machines — Day-2 Operations

### Start / Stop / Deallocate
- `az vm start -g RG -n VM`
- `az vm stop -g RG -n VM` — stops VM but retains resources and bill compute unless deallocated
- `az vm deallocate -g RG -n VM` — releases compute so you do not pay for vCPU minutes (still pay for storage)
- PowerShell: `Start-AzVM`, `Stop-AzVM`, `Stop-AzVM -StayProvisioned:$false` (deallocate)

### Resize
- Deallocate VM, change size, start VM:
```bash
az vm deallocate -g $RG -n $VM
az vm resize -g $RG -n $VM --size Standard_D4s_v3
az vm start -g $RG -n $VM
```
- Not all sizes available in every region or cluster — check quota and available SKUs.

### Extensions & VM Agent
- VM Agent required for many platform operations and extensions.
- Install extensions (Custom Script, Azure Monitor Agent, Log Analytics):
```bash
az vm extension set --publisher Microsoft.Azure.Extensions --name CustomScript --resource-group $RG --vm-name $VM --settings '{"fileUris":["https://.../script.sh"],"commandToExecute":"bash script.sh"}'
```

### Boot Diagnostics & Serial Console
- Boot Diagnostics captures screenshot and serial logs; useful for boot failures.
- Enable during VM creation or update:
```bash
az vm boot-diagnostics enable -g $RG -n $VM --storage https://mystorage.blob.core.windows.net/
```
- Serial Console access requires RBAC and Boot Diagnostics enabled.

### OS Patching & Update Management
- Use Azure Update Management (Automation Account) to schedule patching (Windows & Linux).
- Use VMSS rolling upgrades to patch instances without downtime.

### Backups & Snapshots
- Snapshots (disk-level) are point-in-time, read-only copies:
```bash
az snapshot create -g $RG -n mySnapshot --source /subscriptions/.../resourceGroups/$RG/providers/Microsoft.Compute/disks/$diskName
```
- Azure Backup (VM-level) to schedule backups and retention.
- Restore VM from backup via Recovery Services vault.

### Encryption
- Platform-managed encryption at rest is enabled by default (Storage Service Encryption).
- Use Azure Disk Encryption (Windows: BitLocker; Linux: DM-Crypt) or Disk Encryption Sets to bring your own key (BYOK) with Key Vault.
- For ultra-sensitive data, use HSMs and customer-managed keys.

---

## 7. Creating and Managing Disks

### Disk Types & Characteristics
- Standard HDD — lowest cost, best for infrequent access.
- Standard SSD — moderate performance and cost.
- Premium SSD — high IOPS and low latency (recommended for production).
- Ultra Disk — extreme performance with configurable IOPS and throughput.
- Ephemeral OS Disks — store OS disk on node's local SSD, very low latency, not persisted on VM deallocation (good for stateless scale sets).

### Managed Disks (recommended)
- Azure manages the storage accounts backing the disks; you request a managed disk resource.
- Create managed disk (CLI):
```bash
az disk create -g $RG -n myDataDisk --size-gb 512 --sku Premium_LRS
```
- Attach disk to VM:
```bash
az vm disk attach -g $RG --vm-name $VM --name myDataDisk --new
# or attach existing
az vm disk attach -g $RG --vm-name $VM --disk myDataDisk
```
- Detach:
```bash
az vm disk detach -g $RG --vm-name $VM --name myDataDisk
```
- Resize (online for supported OSes, but recommended to unmount inside OS after increasing on disk)
```bash
az disk update -g $RG -n myDataDisk --size-gb 1024
# In Linux: grow partition and filesystem (growpart, resize2fs/xfs_growfs)
```

### Snapshots and Incremental Snapshots
- Take a snapshot:
```bash
az snapshot create -g $RG -n mySnapshot --source /subscriptions/.../disks/myDataDisk
```
- Incremental snapshots save only delta changes — cost-effective for frequent backups.

### Disk performance considerations
- Disk throughput and IOPS depend on disk SKU and VM size; consult Azure docs and combine with caching settings.
- Use write-through or write-back caching settings (beware of data loss on VM crashes).
- For SQL/DB workloads, follow vendor guidance (separate data, log, tempdb disks; use premium/ultra).

---

## 8. Creating and Managing Images

### Capture a custom image
- Generalize a VM (Windows: `sysprep /generalize`; Linux: deprovision azure agent with `waagent -deprovision+user`), then capture:
```bash
az vm deallocate -g $RG -n $VM
az vm generalize -g $RG -n $VM
az image create -g $RG -n myImage --source $VM
```
- Specialized image (skip generalize) if you need per-machine identity preserved (rare).

### Shared Image Gallery (SIG)
- Use Shared Image Gallery for versioned images, replication across regions, and better scale with VMSS.
- Create gallery and image definition, then create versions:
```bash
az sig create -g $RG -n myGallery
az sig image-definition create -g $RG --gallery-name myGallery --gallery-image-definition myImageDef --os-type Linux --publisher MyCorp --offer myOffer --sku v1
az sig image-version create -g $RG --gallery-name myGallery --gallery-image-definition myImageDef --gallery-image-version 1.0.0 --managed-image "/subscriptions/<sub>/resourceGroups/$RG/providers/Microsoft.Compute/images/myImage"
```
- VMSS supports SIG as an image source (recommended for production).

### Build golden images
- Tools: Packer (HashiCorp) with Azure builder, Azure Image Builder (ARM-driven service).
- Automate image creation pipeline, integrate with CI/CD, and publish to SIG with versioning.

---

## 9. Creating and Managing Scale Sets (VMSS)

### VM Scale Sets overview
- VMSS allows automatic scaling of multiple identical VMs. Use with Standard load balancer or Application Gateway.
- Use cases: stateless web services, microservices frontends, batch workloads.

### Orchestration modes
- Uniform (classic VMSS): VMs identical, scale operations via model.
- Flexible (newer mode): supports flexible orchestration and custom placement — better for complex topologies and stateful workloads.

### Create VMSS (CLI example with auto-scale)
```bash
az vmss create \
  --resource-group $RG \
  --name myScaleSet \
  --image UbuntuLTS \
  --upgrade-policy-mode Automatic \
  --admin-username azureuser \
  --instance-count 2 \
  --vm-sku Standard_B2s \
  --custom-data cloud-init.txt \
  --lb myLoadBalancer
```

### Autoscale rules
```bash
az monitor autoscale create --resource-group $RG --resource myScaleSet --resource-type Microsoft.Compute/virtualMachineScaleSets --name myAutoscale --min-count 2 --max-count 10 --count 2

az monitor autoscale rule create --resource-group $RG --autoscale-name myAutoscale \
  --condition "Percentage CPU > 70 avg 5m" --scale to 5 --scale-interval 5
```

### Health & Update policy
- Upgrade policies: `Automatic`, `Rolling`, `Manual` (for managed images).
- Rolling upgrade example: configure `rollingUpgradePolicy` with maxBatch instance percent and pause time for orchestration.

### VMSS and images
- Use Shared Image Gallery or Marketplace image as VMSS model source.
- For stateful workloads, combine VMSS flexible mode and managed disks with instance IDs.

### Managed identity & extensions
- Assign system-assigned or user-assigned managed identity to VMSS for secure access to Key Vault and other resources.
- Use Custom Script Extension or cloud-init for VM configuration or use cloud-init in Linux images.

---

## 10. Load Balancing Virtual Machines

Azure provides multiple load-balancing options. Pick based on protocol, L7 needs, global vs regional, and performance.

### Azure Load Balancer (L4)
- Types: Basic (legacy) and Standard (recommended).
- Scenarios: distribute TCP/UDP across VMs in backend pools.
- Components: frontend IP, backend pool (VM NICs or VMSS), health probe, load balancing rule, inbound NAT rules for per-VM RDP/SSH.
- Example: create Public Standard LB and NAT rule (CLI):
```bash
# Create or use existing public IP
az network public-ip create -g $RG -n myPublicIP --sku Standard --allocation-method Static

az network lb create -g $RG -n myLB --sku Standard --public-ip-address myPublicIP --backend-pool-name myBackendPool

az network lb probe create -g $RG --lb-name myLB -n tcpProbe --protocol tcp --port 80

az network lb rule create -g $RG --lb-name myLB -n LBRuleHTTP --protocol tcp --frontend-port 80 --backend-port 80 --probe-name tcpProbe --frontend-ip-name LoadBalancerFrontEnd --backend-pool-name myBackendPool
```

- Inbound NAT (RDP/SSH to specific VMs):
```bash
az network lb inbound-nat-rule create -g $RG --lb-name myLB -n MyNatRule --frontend-ip-name LoadBalancerFrontEnd --protocol Tcp --frontend-port 50000 --backend-port 22
```
- Attach VMs to backend pool (NIC-level):
```bash
az network nic ip-config address-pool add -g $RG --nic-name myNic --ip-config-name ipconfig1 --lb-name myLB --address-pool myBackendPool
```

### Application Gateway (L7)
- Features: HTTP/HTTPS routing, path-based routing, cookie affinity, session affinity, WAF (Web Application Firewall), end-to-end SSL/tls re-encryption/termination.
- Use when you need WAF rules, URL-based routing, host-based routing, or cookie-based affinity.

Create via Portal or CLI:
```bash
az network application-gateway create -g $RG -n myAppGw --sku WAF_v2 --capacity 2 --vnet-name myVnet --subnet myAppGwSubnet --frontend-port 443 --http-settings-cookie-based-affinity Enabled
```

### Azure Front Door & Traffic Manager
- Front Door: global HTTP(s) edge service with routing, caching, WAF — good for global web apps and low-latency routing.
- Traffic Manager: DNS-based traffic routing across regions (priority/weighted/performance) — works at DNS layer, no TLS termination.

### Choosing a LB
- L4 load balancer: non-HTTP, extreme throughput, simple TCP/UDP distribution.
- Application Gateway: web apps and WAF.
- Front Door or CDN: global HTTP caching, fast failover, edge optimizations.
- Traffic Manager: DNS-level redirection across endpoints.

### Health probes
- Use application-level health probe that checks a URL returning 200 OK rather than raw TCP for better failure detection.
- Configure appropriate probe interval and unhealthy threshold to balance sensitivity vs false positives.

---

## 11. Monitoring, Logging & Alerts

- Azure Monitor: collect metrics and logs.
  - Configure Diagnostic Settings on VMs and resources to send logs to Log Analytics, Event Hubs, or Storage Accounts.
- VM Insights: agent-based solution for guest-level metrics, process maps, dependencies.
- Azure Monitor agent (AMA) / Log Analytics agent / Diagnostic extension:
```bash
# enable diagnostics
az vm diagnostics set --resource-group $RG --vm-name $VM --settings vm-diagnostics.json
```
- Alerts: metric alerts (CPU > 80%), log alerts, activity log alerts, action groups (email, webhook, runbook).
- Application Insights for app-level telemetry in PaaS or apps running on VMs.

---

## 12. Availability, Resiliency & SLAs

- Single VM SLA is limited unless in Availability Set or Zone.
- Availability Set (for regions without zones): distributes VMs across fault domains and update domains.
- Availability Zone: physical datacenter separation. Deploy VMs across three zones for resiliency.
- VMSS backed by zones gives high availability and autoscale combined benefits.
- Recommended pattern: place multiple VMs in an Availability Set or across Zones and put behind a Load Balancer to meet higher SLAs.

---

## 13. Security Best Practices

- Use NSG and ASG to restrict traffic. Default deny inbound and allow outbound patterns.
- Lock management endpoints: restrict SSH (Linux) and RDP (Windows) via Just-in-Time (JIT) or use private jump hosts / bastion.
- Use Azure Bastion for RDP/SSH in browser without public IP on VMs.
- Use managed identities for apps to access Key Vault or storage; never store credentials in code.
- Keep VM agents and extensions up-to-date.
- Harden OS image: disable unnecessary services, enable firewall, apply baseline security configs (CIS Benchmarks).
- Use Microsoft Defender for Cloud for vulnerability assessments, threat protection and recommendations.

---

## 14. Cost Management & Sizing Recommendations

- Use Pricing Calculator and cost alerts. Tag resources by team, env, cost-center.
- Right-size VMs: avoid overprovisioning CPU/RAM. Use telemetry (Azure Advisor) to recommend resizing.
- Use Reserved Instances or Savings Plans for steady-state workloads.
- Use Spot Virtual Machines for interruptible workloads to save up to 90% on compute.
- Turn off non-production VMs during off-hours (auto-shutdown or automation runbooks).

---

## 15. Troubleshooting Checklist & Common Issues

- SSH / RDP failure:
  - Check NSG rules, VM NIC IP config, public IP allocation, and effective NSG in portal.
  - Use serial console and boot diagnostics to check OS boot and logs.
  - For Linux: ensure SSH daemon running, firewall rules (ufw/firewalld), and correct SSH keys/permissions.
- Disk full / boot failure:
  - Use serial console to get to shell; attach disk to recovery VM to inspect logs.
- VM not starting or stuck in provisioning:
  - Check `az vm get-instance-view` or `Get-AzVM -Status` for power state and provisioning errors.
- Performance issues:
  - Check disk IOPS limits and VM network bandwidth; ensure disks attached match workload (Premium/Ultra).
- Extension failures:
  - Validate VM agent installed and extension logs under `C:\WindowsAzure\Logs\Plugins` (Windows) or `/var/log/azure` (Linux).
- Unexpected costs:
  - Check storage snapshots, unattached disks, public IPs (static), and running VMs; review Cost Analysis.

---

## 16. Appendix — Useful CLI & PowerShell Commands (Cheat Sheet)

Azure CLI
```bash
# Resource group
az group create -n myRG -l eastus

# Create VM
az vm create -g myRG -n myVM --image UbuntuLTS --admin-username azureuser --ssh-key-values ~/.ssh/id_rsa.pub

# Start/Stop/Deallocate
az vm start -g myRG -n myVM
az vm stop -g myRG -n myVM
az vm deallocate -g myRG -n myVM

# Resize VM
az vm deallocate -g myRG -n myVM
az vm resize -g myRG -n myVM --size Standard_D4s_v3
az vm start -g myRG -n myVM

# Create managed disk
az disk create -g myRG -n myDataDisk --size-gb 512 --sku Premium_LRS

# Snapshot
az snapshot create -g myRG -n snap1 --source /subscriptions/.../disks/myDataDisk

# Create image from VM
az vm deallocate -g myRG -n myVM
az vm generalize -g myRG -n myVM
az image create -g myRG -n myImage --source myVM

# VMSS
az vmss create -g myRG -n myss --image UbuntuLTS --instance-count 2 --upgrade-policy-mode automatic

# Load balancer basics
az network lb create -g myRG -n myLB --public-ip-address myPIP --backend-pool-name myPool
```

PowerShell (Az module)
```powershell
Connect-AzAccount
New-AzResourceGroup -Name myRG -Location eastus
New-AzVM -ResourceGroupName myRG -Name myVM -Image Win2019Datacenter -Size Standard_DS2_v2 -Credential (Get-Credential)
Stop-AzVM -ResourceGroupName myRG -Name myVM -Force
New-AzDisk -ResourceGroupName myRG -DiskName myDisk -DiskSizeGB 512 -SkuName Premium_LRS
New-AzVmss -ResourceGroupName myRG -Name myScaleSet -VirtualMachineSize Standard_B2s -InstanceCount 2 -ImageName UbuntuLTS
```

---

## 17. References & Further Reading

- Azure Virtual Machines docs: https://learn.microsoft.com/azure/virtual-machines  
- VM sizes and features: https://learn.microsoft.com/azure/virtual-machines/sizes  
- Managed disks & snapshots: https://learn.microsoft.com/azure/managed-disks  
- Shared Image Gallery: https://learn.microsoft.com/azure/virtual-machines/images/shared-image-galleries  
- VM Scale Sets: https://learn.microsoft.com/azure/virtual-machine-scale-sets  
- Azure Load Balancer: https://learn.microsoft.com/azure/load-balancer  
- Application Gateway & WAF: https://learn.microsoft.com/azure/application-gateway  
- Azure CLI: https://learn.microsoft.com/cli/azure  
- Azure PowerShell (Az): https://learn.microsoft.com/powershell/azure  
- Azure Architecture Center — VM availability & resiliency patterns: https://learn.microsoft.com/azure/architecture

---

Prepared as a comprehensive reference for building and operating virtual machine-based workloads on Azure. If you'd like, I can:  
- generate a ready-to-run Bicep template for a three-tier application (VNet, VMSS front-end, internal DB VMs, public Application Gateway),  
- produce a step-by-step lab (portal + CLI) that provisions a VM, attaches disks, takes snapshots and creates an image, or  
- create a checklist for production readiness (monitoring, backup, security, cost). Which should I produce next?