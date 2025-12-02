# Azure Virtual Network — Deep Reference  
Detailed notes, examples (Portal, CLI, PowerShell, Bicep/ARM), design guidance and troubleshooting

---

## Table of Contents

1. [Purpose & Audience](#purpose--audience)  
2. [High-level Overview — What is an Azure Virtual Network (VNet)?](#high-level-overview---what-is-an-azure-virtual-network-vnet)  
3. [Design Fundamentals & Best Practices](#design-fundamentals--best-practices)  
4. [Creating a Virtual Network](#creating-a-virtual-network)  
   - Portal (step-by-step)  
   - Azure CLI examples  
   - PowerShell (Az) examples  
   - Bicep / ARM example  
5. [Adding, Changing, Deleting Subnets](#adding-changing-deleting-subnets)  
   - CLI & PowerShell examples  
   - Important considerations (IP ranges, delegations, service endpoints)  
6. [Connecting Virtual Networks (Connectivity Options)](#connecting-virtual-networks-connectivity-options)  
   - VNet Peering (regional & global)  
   - VNet-to-VNet VPN (IPsec)  
   - ExpressRoute (private) and ExpressRoute + Global Reach  
   - Virtual Network Gateway concepts  
   - VNet Integration for PaaS workloads (App Service) and Private Endpoint  
7. [Virtual Network Peering — Details & Examples](#virtual-network-peering---details--examples)  
   - Requirements and limitations  
   - CLI / PowerShell examples (create peering both sides)  
   - Use-cases and routing behavior  
8. [Filter Network Traffic — Network Security Options](#filter-network-traffic---network-security-options)  
   - Network Security Groups (NSG) — structure, default rules, example rules  
   - Application Security Groups (ASG) — grouping dynamic workloads  
   - Azure Firewall — stateful L3–L7 firewall as a service  
   - Web Application Firewall (WAF) on Application Gateway / Front Door  
   - NVA (Network Virtual Appliance) pattern  
   - DDoS Protection (Basic vs Standard)  
9. [Router & Route Network Traffic — Route Tables and Route Server](#router--route-network-traffic---route-tables-and-route-server)  
   - User-Defined Routes (UDR) / route tables — next hop types, examples  
   - Azure Route Server (BGP with NVAs) — use-cases and basics  
   - Forced tunneling, default routes and internet egress patterns  
10. [Restricting Networks & Securing Traffic](#restricting-networks--securing-traffic)  
    - Private Endpoint (Private Link) vs Service Endpoints — comparison & examples  
    - Service Tags and selecting network rules for platform services  
    - Locking down storage, SQL and other PaaS with Private Endpoint + Firewall rules  
11. [Monitoring Virtual Networks & Observability](#monitoring-virtual-networks--observability)  
    - Network Watcher features: topology, connection troubleshoot, packet capture, NSG flow logs  
    - Diagnostic settings to Log Analytics and storage, Azure Monitor metrics, alerts  
    - Traffic Analytics (flow logs + workspace) and packet capture samples  
12. [Virtual Network TAP & Traffic Capture / Packet Mirroring](#virtual-network-tap--traffic-capture--packet-mirroring)  
13. [Virtual Network Terminal Access Point (explanation & related features)](#virtual-network-terminal-access-point-explanation--related-features)  
14. [Operational Considerations & Troubleshooting Checklist](#operational-considerations--troubleshooting-checklist)  
15. [Limits, Quotas & Regional Capabilities](#limits-quotas--regional-capabilities)  
16. [Appendix — Useful Commands & Snippets (CLI / PowerShell / Bicep)](#appendix---useful-commands--snippets-cli--powershell--bicep)  
17. [References & Further Reading](#references--further-reading)

---

## 1. Purpose & Audience

This document is targeted at cloud engineers, platform teams and architects who design, deploy and operate Azure networking. It collects comprehensive practical guidance (Portal, CLI, PowerShell, Bicep/ARM) and deep notes about VNet creation, subnets, peering, routing, traffic filtering, security controls, monitoring, and troubleshooting.

---

## 2. High-level Overview — What is an Azure Virtual Network (VNet)?

- An Azure Virtual Network (VNet) is the fundamental networking boundary in Azure. It provides an isolated, private network in which you run Azure resources (VMs, App Service Environment, Kubernetes nodes, etc.).
- VNets provide:
  - IP address space (CIDR) and subnets
  - Routing (system routes + user-defined routes)
  - Network Security Groups applied at subnet/NIC
  - Integration with VPN, ExpressRoute, Peering, Private Link
  - Service endpoints and Private Endpoints for PaaS connectivity

Key properties:
- A VNet spans a single Azure region (cannot span regions); you can connect VNets across regions using VNet peering or VPN/ExpressRoute.
- VNets are scalable and can contain multiple subnets, each with its own NSG, route tables and delegations.

---

## 3. Design Fundamentals & Best Practices

- IP planning: use RFC 1918 private ranges and plan for future growth. Avoid overlapping address spaces when peering or connecting on-prem.
- Subnet sizing: allocate enough IPs for scale and future growth. Note Azure reserves 5 addresses per subnet (first four + broadcast-like reserved).
- One workload per subnet: logically group tiers (web, app, db) or workloads requiring different NSG rules.
- Use Network Security Groups at subnet level for coarse rules; use NIC-level NSG for per-VM exceptions.
- Prefer Private Endpoint for PaaS resources for tight isolation, rather than service endpoints, when possible.
- Use Shared services VNet pattern: central NGFW (Azure Firewall) VNet with hub-and-spoke architecture for enterprise.
- Use Azure Firewall or NVAs for east-west filtering across spoke VNets (hub-and-spoke).
- Use UDRs and route server for complex pathing to NVAs.
- Use Shared Image Gallery and VMSS for scale and consistent deployments.
- Use Bicep/ARM/Terraform to declare networks; keep environments reproducible.

---

## 4. Creating a Virtual Network

### Portal (step-by-step)
1. Sign in to portal.azure.com → "Create a resource" → Networking → Virtual network.  
2. Basics: choose subscription, resource group, name, region.  
3. IP Addresses: choose IPv4/IPv6, set address space (e.g., 10.1.0.0/16).  
4. Subnets: create at least one subnet (e.g., 10.1.0.0/24 for "app-subnet"). You can add later.  
5. Security: enable or attach DDoS Protection Standard (optional), Service Endpoints, Firewall?  
6. Tags & Review → Create.

### Azure CLI (az) — create VNet + subnet
```bash
# Create resource group
az group create -n rg-network -l eastus

# Create VNet with initial subnet
az network vnet create \
  --resource-group rg-network \
  --name vnet-prod \
  --address-prefix 10.1.0.0/16 \
  --subnet-name app-subnet \
  --subnet-prefix 10.1.1.0/24
```

Add another subnet:
```bash
az network vnet subnet create \
  --resource-group rg-network \
  --vnet-name vnet-prod \
  --name db-subnet \
  --address-prefix 10.1.2.0/24
```

### PowerShell (Az module)
```powershell
Connect-AzAccount
New-AzResourceGroup -Name rg-network -Location eastus

$subnet1 = New-AzVirtualNetworkSubnetConfig -Name app-subnet -AddressPrefix 10.1.1.0/24
$vnet = New-AzVirtualNetwork -ResourceGroupName rg-network -Location eastus -Name vnet-prod -AddressPrefix 10.1.0.0/16 -Subnet $subnet1

# Add another subnet
Add-AzVirtualNetworkSubnetConfig -Name db-subnet -AddressPrefix 10.1.2.0/24 -VirtualNetwork $vnet
$vnet | Set-AzVirtualNetwork
```

### Bicep example (vnet + subnets)
```bicep
param location string = resourceGroup().location

resource vnet 'Microsoft.Network/virtualNetworks@2022-07-01' = {
  name: 'vnet-prod'
  location: location
  properties: {
    addressSpace: { addressPrefixes: [ '10.1.0.0/16' ] }
    subnets: [
      { name: 'app-subnet'; properties: { addressPrefix: '10.1.1.0/24' } }
      { name: 'db-subnet'; properties: { addressPrefix: '10.1.2.0/24' } }
    ]
  }
}
```

---

## 5. Adding, Changing, Deleting Subnets

### Add subnet
- CLI example (shown previously): `az network vnet subnet create ...`
- PowerShell: `Add-AzVirtualNetworkSubnetConfig` then `Set-AzVirtualNetwork`.

### Change subnet prefix (shrinking/expanding)
- **Important:** Changing a subnet's address prefix requires the subnet to be empty of IP-configured resources (NICs, VMs, gateways). To expand/modify:
  1. Deallocate and remove resources in that subnet or move them (complex).
  2. Use `az network vnet subnet update --address-prefixes` to add an additional prefix (Azure supports multiple prefixes on a subnet since 2020).
- Example add secondary prefix:
```bash
az network vnet subnet update \
  --resource-group rg-network \
  --vnet-name vnet-prod \
  --name app-subnet \
  --address-prefixes 10.1.1.0/24 10.1.3.0/24
```
- To **shrink** a prefix, you often must recreate subnet or move resources to a new subnet.

### Delete subnet
- CLI:
```bash
az network vnet subnet delete -g rg-network --vnet-name vnet-prod -n db-subnet
```
- You cannot delete a subnet if it still hosts resources (VM NICs, gateway IP configurations, service endpoints in use, etc.). Remove resources first.

### Subnet delegation
- Delegate a subnet to a service (e.g., `Microsoft.Web/serverFarms`, `Microsoft.Sql/managedInstances`).
- CLI example:
```bash
az network vnet subnet create -g rg-network --vnet-name vnet-prod -n sql-mi-subnet --address-prefix 10.1.4.0/24 --delegations "Microsoft.Sql/managedInstances"
```

### Service endpoints & private endpoints on subnets
- You can enable service endpoints on a subnet to allow traffic from that subnet to PaaS over Microsoft backbone and optionally lock down the service to that subnet.
- Example enable Storage service endpoint:
```bash
az network vnet subnet update -g rg-network --vnet-name vnet-prod -n app-subnet --service-endpoints Microsoft.Storage
```

---

## 6. Connecting Virtual Networks (Connectivity Options)

### VNet Peering
- Peer two VNets: routes between VNets are exchanged and traffic flows through Azure backbone (low latency). VNets can be in same or different subscriptions and regions (`global peering`).
- Peering is **non-transitive**: traffic between VNetA->VNetC via VNetB peering does not automatically route unless you configure gateway transit/allow forwarded traffic or a hub-and-spoke with central router/appliance.

### VNet-to-VNet VPN (IPsec)
- A site-to-site IPsec tunnel between virtual network gateways. Useful for cross-region connectivity if peering not possible, or to connect on-prem network to VNet via VPN Gateway.

### ExpressRoute
- Private connectivity between on-premises and Azure over a dedicated circuit with a provider. Provides private peering and optionally Microsoft peering for services.

### VNet Integration for App Service (Outbound)
- App Service can be integrated with a VNet for outbound calls (VNet Integration) or via regional VNet Integration.

### Private Endpoint & Private Link
- Private Endpoint brings PaaS resource into the VNet using a NIC and private IP — access is private; DNS must be configured to resolve service to private IP.

---

## 7. Virtual Network Peering — Details & Examples

### Key characteristics
- **Low latency, high bandwidth** connectivity via Azure backbone.
- **Non-transitive**: peering does not automatically transit to other peers.
- Works **cross-subscription** and **cross-tenancy** (requires permissions).
- Supports **network security groups**, **service endpoints**, and **private endpoints** in peered VNets.
- Peering is created **bidirectionally** (set up peering on each VNet or use `--allow-vnet-access` to auto-config).

### CLI example (two VNets in same subscription)
```bash
# Create peering from vnet-a to vnet-b
az network vnet peering create --name vnetA-to-vnetB --resource-group rg-network --vnet-name vnet-a --remote-vnet vnet-b --allow-vnet-access

# Create reverse peering
az network vnet peering create --name vnetB-to-vnetA --resource-group rg-network --vnet-name vnet-b --remote-vnet vnet-a --allow-vnet-access
```

### Advanced options
- `--allow-forwarded-traffic` — allow traffic forwarded from a virtual appliance to cross the peering.
- `--allow-gateway-transit` / `--use-remote-gateways` — share gateway (route) from one VNet to peered VNets: common in hub-and-spoke designs. Use carefully: only one VNet can provide gateway transit and permissions must be set.

### When to use peering
- Low-latency cross-VNet communication between application tiers in different VNets or subscriptions.
- Hub-and-spoke: hub VNet with central services (firewall, VPN gateway) and peered spokes for workloads — combine with route tables and firewall/NVA to centralize egress/inspection.

---

## 8. Filter Network Traffic — Network Security Options

### Network Security Groups (NSG)
- NSGs are stateless rules applied to subnets or NICs (subnet-level filters affect all NICs in the subnet, NIC-level override for exceptions).
- Each rule has: priority (100–4096; lower executes first), name, direction (Inbound/Outbound), protocol (TCP/UDP/Any), source/destination (IP or ASG or Service Tag), source/destination port ranges, action (Allow/Deny).
- Azure includes default rules (allow VNet, AzureLoadBalancer, deny all inbound Internet, allow outbound internet). Custom rules supersede based on priority.

Example NSG rule (allow HTTP inbound from Internet):
```bash
az network nsg create -g rg-network -n nsg-app
az network nsg rule create -g rg-network --nsg-name nsg-app -n AllowHTTP --priority 100 --protocol Tcp --direction Inbound --source-address-prefix Internet --source-port-range '*' --destination-port-range 80 --access Allow
```

Apply NSG to subnet:
```bash
az network vnet subnet update -g rg-network --vnet-name vnet-prod -n app-subnet --network-security-group nsg-app
```

Best practices:
- Use deny-by-default: define least privilege with specific allow rules then deny all other inbound.
- Use NSG flow logs + Traffic Analytics to validate rules.

### Application Security Groups (ASG)
- Logical groupings of NICs, referenced by NSG rules to simplify rules for dynamic/scale-out workloads. Example: create `ASG-Web`, assign NICs to ASG, and create NSG rule where source is `ASG-Web`.

CLI example:
```bash
az network asg create -g rg-network -n asg-web
# Associate NIC to ASG
az network nic update -g rg-network -n vmNicName --add ipConfigurations[0].applicationSecurityGroupIds "/subscriptions/.../resourceGroups/rg-network/providers/Microsoft.Network/applicationSecurityGroups/asg-web"
```

### Azure Firewall
- Managed, stateful network firewall deployed in a dedicated subnet (`AzureFirewallSubnet`). Supports:
  - Network rules (L3-L4), Application rules (HTTP/HTTPS FQDN filtering), NAT rules.
  - Threat Intelligence, TLS inspection (preview/limited).
  - Built-in high availability and scaling.
- Typically deployed in hub VNet in hub-and-spoke pattern; route spoke traffic to firewall via UDR.

Example basic rule: allow outbound 443 to all:
- Create firewall, create application rule collection, configure destination FQDN.

### NVAs (Network Virtual Appliances)
- Third-party virtual appliances (Palo Alto, Fortinet, Cisco) provide advanced inspection or features not yet in Azure Firewall. Usually combined with Azure Route Server or UDR to forward traffic to NVAs.

### DDoS Protection
- Basic DDoS protection included. **DDoS Protection Standard** is a paid SKU for large-scale protection with telemetry and mitigation (recommended for public-facing endpoints).

---

## 9. Router & Route Network Traffic — Route Tables and Route Server

### System routes
- Each VNet has system routes (local VNet, peering, Internet via 0.0.0.0/0 through Azure Internet if SNAT needed), and routes from VPN/ExpressRoute.

### User-Defined Routes (UDR)
- Attach a route table to a subnet and add custom routes to control next-hop (Virtual Appliance, Internet, Virtual Network Gateway, None).
- Next hop types: `VirtualNetworkGateway`, `Internet`, `VirtualAppliance` (NVA IP), `VnetLocal`, `None`.

Example: force-l3 inspection of outbound traffic to firewall (UDR)
```bash
az network route-table create -g rg-network -n rt-spoke
az network route-table route create -g rg-network --route-table-name rt-spoke -n default-route --address-prefix 0.0.0.0/0 --next-hop-type VirtualAppliance --next-hop-ip-address 10.0.0.4
# Associate with subnet
az network vnet subnet update -g rg-network --vnet-name spoke-vnet --name app-subnet --route-table rt-spoke
```

Notes:
- When `VirtualAppliance` is next hop, traffic is forwarded to the specified IP and must be processed/forwarded by the appliance.
- Be careful with route overlap and ensure return path for packets (source NAT or ensure NVAs have routes back).

### Azure Route Server
- Simplifies dynamic exchange of routes with NVAs via BGP. Useful when you have multiple NVAs or complex topologies.
- Azure Route Server can peer with on-prem BGP routers and advertise learned routes.

---

## 10. Restrict Virtual Network & Securing Network Traffic

### Service Endpoints
- Extend VNet identity to Azure PaaS services over Azure backbone — you can then lock service to subnets with service endpoint ACLs.
- Support: Storage, SQL, Cosmos DB, Key Vault (varies by service).
- Limitations: service still uses public IP ranges; not private IPs. Better replaced by Private Endpoint for stronger isolation.

Enable service endpoint on subnet (CLI):
```bash
az network vnet subnet update -g rg-network --vnet-name vnet-prod -n app-subnet --service-endpoints Microsoft.Storage
```

### Private Endpoint (Private Link)
- Creates a NIC with private IP in your subnet that maps to a specific PaaS resource (Storage account, SQL, Key Vault, etc). This makes service traffic traverse your VNet privately.
- Recommended for secure production environment.

Create private endpoint (CLI):
```bash
az network private-endpoint create -g rg-network -n pe-storage --vnet-name vnet-prod --subnet app-subnet --private-connection-resource-id "/subscriptions/.../resourceGroups/rg-storage/providers/Microsoft.Storage/storageAccounts/mystorage" --group-ids blob --connection-name conn1
```
- Configure Private DNS zone for service resolution (e.g., `privatelink.blob.core.windows.net`) and link to your VNet.

### Service Tags
- Use service tags in NSG rules (e.g., `Storage`, `Sql`, `AzureLoadBalancer`) to allow traffic to platform-managed IP ranges without maintaining IP lists.

### Harden network access
- Use Private Link + Firewall rules to restrict cross-network access.
- Protect management ports (22, 3389): restrict via NSG to jump/bastion host, or use Azure Bastion (browser RDP/SSH without public IP on VMs) and Just-In-Time (JIT) access.
- Block inbound Internet to subnets hosting sensitive workloads; use API Gateway or reverse proxy for edge service.

---

## 11. Monitoring Virtual Networks & Observability

### Network Watcher
- Enable Network Watcher in a region to use:
  - Topology: visualize network resources in RG/VNet.
  - Connection troubleshoot: test path between VM and endpoint (`az network watcher test-connectivity`).
  - IP flow verify: simulate NSG/route effect (`az network watcher test-ip-flow`).
  - Packet capture: capture traffic on VM (requires NIC access & extension).
  - NSG Flow Logs: export flows to Log Analytics or storage for Traffic Analytics.
  - Next hop: determine next hop for VM traffic.

Examples:
```bash
# Run connectivity check
az network watcher test-connectivity -g rg-network --source-resource vnet-prod-vm --dest-address 8.8.8.8 --dest-port 53

# Start packet capture
az network watcher packet-capture create -g rg-network -n pc1 --vm vnet-prod-vm --storage-account mystorage --time-limit 300
```

### NSG Flow Logs & Traffic Analytics
- Activate flow logs on NSG and send to Log Analytics workspace. Traffic Analytics gives insights and visualizations.

Enable NSG flow logs (CLI):
```bash
az network watcher flow-log configure --resource-group rg-network --nsg nsg-app --enabled true --retention 7 --workspace /subscriptions/<sub>/resourceGroups/rg-monitor/providers/Microsoft.OperationalInsights/workspaces/myWorkspace
```

### Diagnostics & Alerts
- Configure Diagnostic Settings on Firewalls, Gateways, and Load Balancers to send logs/metrics to Log Analytics or Event Hubs.
- Create metric and log alerts (e.g., dropped packets, bandwidth spikes, connectivity tests failing) with action groups.

---

## 12. Virtual Network TAP & Traffic Capture / Packet Mirroring

### Virtual Network TAP
- TAP (Traffic Analytics Pipeline) duplicates network traffic from a NIC or load-balancer backend pool to a NICS in a monitoring VM/collector or to Azure Packet Capture for analysis.
- Use-cases: intrusion detection system (IDS), deep packet inspection, traffic analytics, forensics.

Create VNet TAP (conceptual steps):
1. Create target NIC in monitoring VM.
2. Create TAP resource linked to source (NIC or load balancer) and target NIC.
3. Capture mirrored traffic at collector level (e.g., Suricata, Zeek).

### Packet capture using Network Watcher
- For ad-hoc troubleshooting use network watcher packet capture that saves to storage or retrieval.

---

## 13. Virtual Network Terminal Access Point (explanation & related features)

The term "Virtual Network Terminal Access Point" isn't a standard Azure product name. It is likely the user refers to one of the following relevant concepts:

- **Azure Bastion (managed PaaS)** — secure RDP/SSH access to VMs directly from the portal, without exposing public IPs. This is the recommended "terminal access point" to manage VMs securely.
  - Deploy Azure Bastion in a dedicated subnet named `AzureBastionSubnet`.
  - Bastion supports browser-based RDP/SSH from the Azure Portal and keeps management endpoints private.

- **Azure Virtual Network TAP** — capture/forward traffic to monitoring appliances (discussed above).

- **Network endpoint / Private Endpoint** — a private IP in your VNet that acts as an access point to PaaS services (makes services accessible privately inside the VNet).

- **NSG + Jumpbox (Bastion host)** pattern — classic terminal access pattern (less preferred vs Azure Bastion).

Recommendation: use **Azure Bastion** as the secure terminal access point for admin sessions to VMs (no public IPs, built-in security), or a properly secured jump host with strict NSG & monitoring if Bastion is not available.

---

## 14. Operational Considerations & Troubleshooting Checklist

- Connectivity issues:
  - Verify NSG effective rules (Portal: Network Interface → Effective security rules).
  - Check route tables (`ip route` on Linux VM) and Azure route table (`az network route-table route list`).
  - Use Network Watcher connection troubleshoot.

- DNS resolution:
  - Ensure Private DNS Zone or custom DNS servers are configured for private endpoints and cross-VNet resolution.
  - For VNet peering, Private DNS zones must be linked to VNets that need resolution.

- Unexpected traffic egress costs:
  - Check peered VNets, public IP egress, and cross-region egress costs. Use diagnostic tools to locate large flows.

- Latency and throughput:
  - Check VM SKU network bandwidth, NIC configuration and accelerated networking settings.

- Route conflicts and black hole:
  - Avoid overlapping IPs. If traffic hits `NextHopType: None` packets dropped — check route table.

- Packet capture and logs:
  - Use packet capture and NSG flow logs for forensic or transient issues.

- Security incidents:
  - Enable Azure Security Center / Defender alerts, investigate using Log Analytics, and use Just-In-Time access to limit exposure.

---

## 15. Limits, Quotas & Regional Capabilities

- Each subscription & region has default quotas: VNets per region, IP addresses per VNet, peering limits, route table entries, NSG rules. Check Azure subscription limits and quotas docs.
- Not all regions support **Availability Zones**, **some SKU types** (Ultra Disks, certain VM SKUs) or **feature previews**. Validate per-region availability.

Typical limits (subject to change) — check docs:
- Max VNets per region per subscription (varies)
- Max peering per VNet (varies by SKU)
- Max address prefixes per VNet / subnet count
- NSG rules up to 1000+ (depends on SKU)

---

## 16. Appendix — Useful Commands & Snippets (CLI / PowerShell / Bicep)

### CLI: common operations
```bash
# Create RG
az group create -n rg-network -l eastus

# Create VNet + subnets
az network vnet create -g rg-network -n vnet-prod --address-prefix 10.1.0.0/16 --subnet-name app-subnet --subnet-prefix 10.1.1.0/24

# Add subnet
az network vnet subnet create -g rg-network --vnet-name vnet-prod -n db-subnet --address-prefix 10.1.2.0/24

# Create NSG and rule
az network nsg create -g rg-network -n nsg-app
az network nsg rule create -g rg-network --nsg-name nsg-app -n AllowWeb --priority 100 --source-address-prefix Internet --destination-port-range 80 --access Allow --protocol Tcp --direction Inbound

# Associate NSG to subnet
az network vnet subnet update -g rg-network --vnet-name vnet-prod -n app-subnet --network-security-group nsg-app

# Create peering
az network vnet peering create -g rg-network --vnet-name vnet-a --name a-to-b --remote-vnet /subscriptions/<sub>/resourceGroups/rg-network/providers/Microsoft.Network/virtualNetworks/vnet-b --allow-vnet-access

# Create UDR and route to NVA
az network route-table create -g rg-network -n rt-app
az network route-table route create -g rg-network --route-table-name rt-app -n default-route --address-prefix 0.0.0.0/0 --next-hop-type VirtualAppliance --next-hop-ip-address 10.1.1.10
az network vnet subnet update -g rg-network --vnet-name vnet-prod -n app-subnet --route-table rt-app
```

### PowerShell (Az)
```powershell
# Create VNet & Subnet
$subnetConfig = New-AzVirtualNetworkSubnetConfig -Name app-subnet -AddressPrefix "10.1.1.0/24"
$vnet = New-AzVirtualNetwork -ResourceGroupName rg-network -Location eastus -Name vnet-prod -AddressPrefix "10.1.0.0/16" -Subnet $subnetConfig

# Create NSG and rule
$nsg = New-AzNetworkSecurityGroup -ResourceGroupName rg-network -Location eastus -Name nsg-app
$nsg | Add-AzNetworkSecurityRuleConfig -Name AllowWeb -Description "Allow HTTP" -Access Allow -Protocol Tcp -Direction Inbound -Priority 100 -SourceAddressPrefix Internet -SourcePortRange * -DestinationPortRange 80 -DestinationAddressPrefix *
$nsg | Set-AzNetworkSecurityGroup

# Associate NSG
Set-AzVirtualNetworkSubnetConfig -VirtualNetwork $vnet -Name app-subnet -AddressPrefix 10.1.1.0/24 -NetworkSecurityGroup $nsg
$vnet | Set-AzVirtualNetwork
```

### Bicep — VNet + NSG + subnet association
```bicep
param location string = resourceGroup().location

resource nsg 'Microsoft.Network/networkSecurityGroups@2022-05-01' = {
  name: 'nsg-app'
  location: location
  properties: {
    securityRules: [
      {
        name: 'AllowHttp'
        properties: {
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRange: '80'
          sourceAddressPrefix: 'Internet'
          destinationAddressPrefix: '*'
          access: 'Allow'
          priority: 100
          direction: 'Inbound'
        }
      }
    ]
  }
}

resource vnet 'Microsoft.Network/virtualNetworks@2022-05-01' = {
  name: 'vnet-prod'
  location: location
  properties: {
    addressSpace: { addressPrefixes: [ '10.1.0.0/16' ] }
    subnets: [
      {
        name: 'app-subnet'
        properties: {
          addressPrefix: '10.1.1.0/24'
          networkSecurityGroup: { id: nsg.id }
        }
      }
    ]
  }
}
```

---

## 17. References & Further Reading

- Azure Virtual Network docs — https://learn.microsoft.com/azure/virtual-network/  
- VNet Peering — https://learn.microsoft.com/azure/virtual-network/virtual-network-peering-overview  
- Azure Firewall — https://learn.microsoft.com/azure/firewall/  
- ExpressRoute — https://learn.microsoft.com/azure/expressroute/  
- Network Watcher docs and NSG flow logs — https://learn.microsoft.com/azure/network-watcher/  
- Private Link & Private Endpoint — https://learn.microsoft.com/azure/private-link/  
- Azure Route Server — https://learn.microsoft.com/azure/route-server/  
- Bicep & ARM templates — https://learn.microsoft.com/azure/azure-resource-manager/bicep/

---

Prepared as a deep, actionable reference for Azure Virtual Network design, creation, security, routing, peering and monitoring. If you’d like, I can:

- generate a ready-to-deploy Bicep template for a hub-and-spoke network with Azure Firewall and a spoke VMSS,  
- produce a one-page printable cheat-sheet of common CLI/PowerShell commands for network ops, or  
- create a step-by-step lab that provisions a VNet, peering, NSG, UDR, and tests connectivity with Network Watcher.  

Which would you like next?