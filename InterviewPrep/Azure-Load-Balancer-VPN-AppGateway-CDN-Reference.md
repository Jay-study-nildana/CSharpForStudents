# Azure Networking Services — Load Balancer, VPN Gateway, Application Gateway & CDN  
Deep reference with architecture, components, configuration examples and monitoring guidance

---

## Table of Contents

1. [Purpose & Audience](#purpose--audience)  
2. [Azure Load Balancer — Overview & Use Cases](#azure-load-balancer---overview--use-cases)  
3. [Load Balancer Algorithms & Traffic Distribution](#load-balancer-algorithms--traffic-distribution)  
4. [Load Balancer Components & Architecture](#load-balancer-components--architecture)  
5. [Azure Load Balancer — Types, SKUs & Behavioural Differences](#azure-load-balancer---types-skus--behavioural-differences)  
6. [Configuring Azure Load Balancer — Examples (CLI / PowerShell)](#configuring-azure-load-balancer---examples-cli--powershell)  
7. [Health Probes, Session Affinity & NAT Rules](#health-probes-session-affinity--nat-rules)  
8. [Common Load Balancer Patterns & Best Practices](#common-load-balancer-patterns--best-practices)  
9. [VPN Gateway — Overview & Use Cases](#vpn-gateway---overview--use-cases)  
10. [VPN Gateway Types, SKUs & Tunnels](#vpn-gateway-types-skus--tunnels)  
11. [Site-to-Site & VNet-to-VNet VPN Examples (CLI / PowerShell)](#site-to-site--vnet-to-vnet-vpn-examples-cli--powershell)  
12. [ExpressRoute vs VPN Gateway: When to use which](#expressroute-vs-vpn-gateway-when-to-use-which)  
13. [Azure Application Gateway — L7 Load Balancer & WAF](#azure-application-gateway---l7-load-balancer--waf)  
14. [Application Gateway Components & Features](#application-gateway-components--features)  
15. [Configuring Application Gateway — Example (CLI / Bicep)](#configuring-application-gateway---example-cli--bicep)  
16. [TLS, End-to-end SSL, WAF Policies & Rewrite Rules](#tls-end-to-end-ssl-waf-policies--rewrite-rules)  
17. [Azure CDN — Overview & Use Cases](#azure-cdn---overview--use-cases)  
18. [Azure CDN Profiles & Endpoints — Concepts](#azure-cdn-profiles--endpoints---concepts)  
19. [Creating & Configuring CDN Profiles/Endpoints (CLI / Portal)](#creating--configuring-cdn-profilesendpoints-cli--portal)  
20. [CDN Features — Caching, Compression, Rules Engine, Geo-filtering, Purge](#cdn-features---caching-compression-rules-engine-geo-filtering-purge)  
21. [Custom Domains, Certificates & HTTPS for CDN](#custom-domains-certificates--https-for-cdn)  
22. [Monitoring Azure CDN Resources — Metrics, Logs & Alerts](#monitoring-azure-cdn-resources---metrics-logs--alerts)  
23. [Monitoring & Diagnostics for Load Balancer, App Gateway & VPN Gateway](#monitoring--diagnostics-for-load-balancer-app-gateway--vpn-gateway)  
24. [Security Considerations & Best Practices Across Services](#security-considerations--best-practices-across-services)  
25. [Troubleshooting Checklist & Common Pitfalls](#troubleshooting-checklist--common-pitfalls)  
26. [Costs & Sizing Considerations](#costs--sizing-considerations)  
27. [References & Further Reading](#references--further-reading)

---

## 1. Purpose & Audience

This document is a deep, practical reference for Azure networking services focusing on: Azure Load Balancer (L4), Load Balancer algorithms and components, VPN Gateway (IPsec/IKE), Azure Application Gateway (L7 + WAF), and Azure CDN (profiles, endpoints, monitoring). It includes architecture notes, CLI/PowerShell examples, design patterns, monitoring/diagnostics and operational best practices for cloud engineers, SREs and architects.

---

## 2. Azure Load Balancer — Overview & Use Cases

Azure Load Balancer is a Layer-4 (TCP/UDP) regional load-balancing service that distributes inbound flows to healthy backend pool instances in a VNet. It's used for:

- Distributing network/transport traffic across Virtual Machines and VM Scale Sets
- Providing high availability for stateful or stateless TCP/UDP workloads
- NAT rules for direct management port access (SSH/RDP)
- Outbound SNAT for VMs without public IPs (via Standard LB)

Common use cases:
- Web servers when you want simple TCP/SSL pass-through and scale (but for advanced HTTP features use Application Gateway or Front Door)
- Database clusters, custom TCP services, UDP-based services (DNS, RTP, gaming)

---

## 3. Load Balancer Algorithms & Traffic Distribution

Azure Load Balancer uses a deterministic 5-tuple hashing algorithm by default to map flows to backend pool members. Key points:

- **Hashing inputs**: source IP, source port, destination IP, destination port, and protocol.
- The default distribution algorithm is "hash-based, ephemeral 2-tuple/5-tuple depending on NAT scenario."
- For Standard Load Balancer:
  - For inbound flows (frontend IP -> backend IP), LB selects a backend using a hash; flows stay on the selected backend until flow ends (flow affinity achieved at 5-tuple level).
  - For outbound SNAT, sources may be translated and hashing/affinity affect flows.
- **Session affinity**:
  - LB is L4 and affinity is by flow. To achieve client "stickiness" at L7 level (HTTP), use Application Gateway or implement the application to set cookies and route accordingly.
- **Per-packet vs per-flow**:
  - Azure LB is per-flow; it keeps flow state; not per-packet round-robin.

Implications:
- Short-lived backend server IP changes (drain/replace) require health probes and graceful drain; open flows remain mapped.
- For almost-even distribution ensure many clients/ports (diverse 5-tuple) — BLOB of flows from same source port may skew distribution.

---

## 4. Load Balancer Components & Architecture

Core components:

- **Frontend IP Configuration**: public (internet-facing) or internal (private) IP address associated with LB.
- **Backend Pool**: NICs (or VMSS) that receive traffic. Backend targets are VM NIC IP configurations or virtual machine scale sets.
- **Health Probe**: Application or TCP probe that checks backend health. LB routes traffic only to healthy targets.
- **Load Balancing Rule**: binds frontend IP + port to backend port + backend pool, and associates a health probe.
- **Inbound NAT Rules**: expose direct access (RDP/SSH) to specific backend instances by mapping frontend port to backend port (useful for management).
- **Outbound rules / SNAT** (Standard LB): control outbound connections and SNAT behavior for VMs without public IPs.

Architecture diagram (simplified):

```
  Internet
    |
  Frontend IP (Public)
    |
 [Azure Load Balancer]
    | -- LB Rule (TCP 80) --> Backend Pool: VM NICs (10.1.1.4, 10.1.1.5,...)
    | -- Health Probe (HTTP /health)
    | -- NAT Rules (50022 -> VM1:22, 50023 -> VM2:22)
```

---

## 5. Azure Load Balancer — Types, SKUs & Behavioural Differences

- **Basic vs Standard SKU**
  - **Basic**: legacy, limited features: no zone-redundant SLAs, limited scale, no diagnostic logs, single availability set/VMSS limits.
  - **Standard** (recommended): hardened, supports cross-zone, higher scale, integrates with NSG, diagnostics, secure by default (deny all inbound unless explicitly allowed), supports Availability Zones, backend pool of up to thousands of instances (depending on SKU).
- **Public vs Internal**:
  - Public LB: frontend is public IP, used for internet-facing traffic.
  - Internal LB (ILB): frontend is private IP inside a VNet, used for internal service distribution or NAT scenarios.

Choose Standard SKU for production.

---

## 6. Configuring Azure Load Balancer — Examples (CLI / PowerShell)

### Create a Standard Public Load Balancer with backend pool and rule (CLI)
```bash
# variables
RG="rg-lb"
LOC="eastus"
LB="mylb"
PIP="mylb-pip"
VNET="vnet-prod"
SUBNET="app-subnet"

# create pip (standard)
az network public-ip create -g $RG -n $PIP --sku Standard --allocation-method Static

# create lb
az network lb create -g $RG -n $LB --sku Standard --public-ip-address $PIP --frontend-ip-name LBFrontend --backend-pool-name LBBackPool

# create health probe (HTTP /health on port 80)
az network lb probe create -g $RG --lb-name $LB -n httpProbe --protocol Http --port 80 --path /health

# create LB rule to forward 80 -> 80
az network lb rule create -g $RG --lb-name $LB -n httpRule \
  --protocol tcp --frontend-port 80 --backend-port 80 --frontend-ip-name LBFrontend --backend-pool-name LBBackPool --probe-name httpProbe

# Add NIC to backend pool (assume NIC exists)
az network nic ip-config address-pool add -g $RG --nic-name myvmNic --ip-config-name ipconfig1 --lb-name $LB --address-pool LBBackPool
```

### Inbound NAT rule (SSH to backend VM)
```bash
az network lb inbound-nat-rule create -g $RG --lb-name $LB -n nat-rule-ssh --protocol Tcp --frontend-port 50022 --backend-port 22 --frontend-ip-name LBFrontend
# Associate NAT rule to VM NIC IP config
az network nic ip-config inbound-nat-rule add -g $RG --nic-name myvmNic --ip-config-name ipconfig1 --inbound-nat-rule nat-rule-ssh --lb-name $LB
```

### PowerShell equivalent (concise)
```powershell
$rg = "rg-lb"; $loc="eastus"
$pip = New-AzPublicIpAddress -Name mylb-pip -ResourceGroupName $rg -Location $loc -AllocationMethod Static -Sku Standard
$frontend = New-AzLoadBalancerFrontendIpConfig -Name LBFrontend -PublicIpAddress $pip
$backendPool = New-AzLoadBalancerBackendAddressPoolConfig -Name LBBackPool
$probe = New-AzLoadBalancerProbeConfig -Name httpProbe -Protocol Http -Port 80 -RequestPath "/health" -IntervalInSeconds 15 -ProbeCount 2
$rule = New-AzLoadBalancerRuleConfig -Name httpRule -FrontendIpConfiguration $frontend -BackendAddressPool $backendPool -Probe $probe -Protocol Tcp -FrontendPort 80 -BackendPort 80

$lb = New-AzLoadBalancer -ResourceGroupName $rg -Name mylb -Location $loc -Sku Standard -FrontendIpConfiguration $frontend -BackendAddressPool $backendPool -LoadBalancingRule $rule -Probe $probe
```

---

## 7. Health Probes, Session Affinity & NAT Rules

### Health Probes
- Types: TCP probe (TCP connect), HTTP/HTTPS probe (expects HTTP 200 OK). Recommended to use application-level probe so backend can signal healthy/degraded state.
- Probe frequency and thresholds adjustable (interval, unhealthy threshold) — tune for failover sensitivity without flapping.

### Session Affinity
- LB is flow-based; client traffic maps using hash (5-tuple). For HTTP session sticky routing at L7 use Application Gateway or implement sticky cookies.
- For Basic load balancer (older), there was a "session persistence" option (ClientIP). For production use Application Gateway/Front Door for HTTP-level persistence.

### NAT Rules
- Inbound NAT: map frontend port to a backend instance's port (useful for admin access).
- Outbound rules: control SNAT behavior for backends without public IP; Standard LB's outbound rules replace basic default SNAT behavior and allow configuring idle timeouts and allocated public IPs.

---

## 8. Common Load Balancer Patterns & Best Practices

- Place VMs for the same service in a backend pool behind LB rule with health probe.
- Use VM Scale Sets to scale backends automatically.
- Use Standard SKU with a Standard Public IP and Standard Load Balancer to get predictable behavior and integration with NSGs.
- Health probe endpoint: return 200 quickly and include readiness check for application dependencies (DB connectivity).
- For in-place upgrades: remove instance from backend pool, let existing flows drain, then update and re-add.
- Secure management: avoid public IPs on VMs; use NAT + jump host or Azure Bastion.

---

## 9. VPN Gateway — Overview & Use Cases

Azure VPN Gateway provides encrypted IPsec/IKE tunnels to connect on-premises networks, other VNets, and remote clients to Azure VNets. Two principal VPN types:

- **Site-to-Site (S2S)**: VPN tunnel between on-premises VPN device and Azure VPN Gateway (IPsec/IKE).
- **Point-to-Site (P2S)**: individual client-to-VNet VPN using OpenVPN, IKEv2, or SSTP (Windows).
- **VNet-to-VNet**: similar to S2S but between two Azure Virtual Network Gateways (useful across regions or subscriptions).
- **ExpressRoute** is alternate for private connectivity without traversing public Internet.

Common uses:
- Hybrid cloud connectivity with on-premises.
- Secure inter-region connectivity without public exposure (when combined with ExpressRoute/Global Reach).
- Remote admin (P2S) for developers or administrators when bastions are not used.

---

## 10. VPN Gateway Types, SKUs & Tunnels

- VPN Gateway SKUs: Basic, VpnGw1/VpnGw2/VpnGw3, HighPerformance SKUs (GatewaySKUs vary by region and support for BGP, throughput).
- **Route-based** vs **Policy-based**:
  - Route-based (recommended): uses IP routing and is supported by most modern devices and Azure features (BGP, VNet peering, multiple routes).
  - Policy-based (static): legacy, limited.
- Supports active-active or active-standby configurations for high availability.
- Tunnels:
  - **IPsec/IKE**: IKEv1/IKEv2 with configurable pre-shared keys or certificates (P2S uses certificates or Azure AD authentication).
  - **BGP**: dynamic route exchange supported with ExpressRoute or S2S (when enabled with compatible devices) via BGP.

---

## 11. Site-to-Site & VNet-to-VNet VPN Examples (CLI / PowerShell)

### S2S VPN (high-level CLI steps)
1. Create virtual network and gateway subnet (`GatewaySubnet`).
2. Create public IP for VPN gateway.
3. Create VPN gateway (route-based).
4. Create local network gateway (represents on-prem network and public IP).
5. Create VPN connection between azure gateway and local gateway.

CLI example (abbreviated):
```bash
# create gateway subnet
az network vnet subnet create -g rg-network --vnet-name vnet-prod -n GatewaySubnet --address-prefix 10.1.255.0/27

# public ip
az network public-ip create -g rg-network -n vpn-gateway-pip --allocation-method Dynamic --sku Standard

# create vpn gateway
az network vnet-gateway create -g rg-network -n vpngw --public-ip-address vpn-gateway-pip --vnet vnet-prod --gateway-type Vpn --vpn-type RouteBased --sku VpnGw1 --no-wait

# local network gateway (on-prem)
az network local-gateway create -g rg-network -n onprem-gw --gateway-ip-address 198.51.100.2 --local-address-prefixes 10.100.0.0/16

# connection
az network vpn-connection create -g rg-network -n conn-1 --vnet-gateway1 vpngw --local-gateway2 onprem-gw --shared-key "REPLACE_with_psk"
```

### Point-to-Site VPN (P2S)
- Configure VPN gateway with P2S configuration (address pool, auth type).
- Use Azure AD auth or certificate auth for clients; download VPN client package for Windows/macOS.

CLI snippet to update for P2S:
```bash
az network vnet-gateway update -g rg-network -n vpngw --set vpnClientConfiguration.vpnClientAddressPool.addressPrefixes='172.16.201.0/24'
# upload root cert public key to enable certificate auth
```

---

## 12. ExpressRoute vs VPN Gateway: When to Use Which

- **VPN Gateway** (over Internet):
  - Good for secure, small-to-medium capacity, low-cost connectivity.
  - Internet-based so latency/jitter depends on ISP.
- **ExpressRoute** (private dedicated circuits via connectivity provider):
  - Use for high throughput, predictable latency, compliance/regulatory requirements and when connecting multiple regions/onsite with private peering.
- Hybrid: often use ExpressRoute for production critical apps and VPN for backup/failover (Active/Passive).

---

## 13. Azure Application Gateway — L7 Load Balancer & WAF

Application Gateway is an L7 (HTTP/HTTPS) load balancer that provides advanced web delivery features:

- Path-based routing and host-based routing
- Cookie-based session affinity
- SSL/TLS termination and end-to-end SSL
- Web Application Firewall (WAF) for OWASP rules
- URL-based rewrites and redirects
- Autoscaling (v2 SKU)
- Integration with Private Link and WAF policies

Use Application Gateway when you need application-layer routing, WAF protection, or cookie affinity for web apps.

---

## 14. Application Gateway Components & Features

- **Frontend IP configuration**: Public or Private.
- **Listeners**: Port/protocol combos (HTTP/HTTPS) that receive requests; can be multi-site (host name based).
- **Routing rules**: define how listeners map to backend pools and HTTP settings.
- **Backend pools**: list of backend targets (VMs, VMSS, IPs, FQDN).
- **HTTP settings**: backend port, protocol (HTTP/HTTPS), cookie-based affinity, connection draining, probe options.
- **Health probes**: application-level checks per backend.
- **WAF**: enabled in v2 SKU with managed rule sets (OWASP CRS); supports custom rules, exclusions and logging.
- **Rewrite rules & redirect configuration**: modify request/response headers and URL paths.
- **Autoscaling & zone redundancy** (v2): scale based on request volume.

---

## 15. Configuring Application Gateway — Example (CLI / Bicep)

### CLI example: create App Gateway (simplified)
```bash
# create subnet for App Gateway (gateway must be in dedicated subnet named anything)
az network vnet subnet create -g rg-network --vnet-name vnet-prod -n appgw-subnet --address-prefix 10.1.10.0/24

# public ip
az network public-ip create -g rg-network -n appgw-pip --allocation-method Static --sku Standard

# create app gw (simplified)
az network application-gateway create -g rg-network -n appgw1 --sku WAF_v2 --capacity 2 --vnet-name vnet-prod --subnet appgw-subnet --public-ip-address appgw-pip --http-settings-cookie-based-affinity Disabled --frontend-port 80
```

### Bicep snippet (conceptual)
```bicep
resource appgw 'Microsoft.Network/applicationGateways@2022-05-01' = {
  name: 'appgw1'
  location: resourceGroup().location
  sku: { name: 'WAF_v2'; tier: 'WAF_v2'; capacity: 2 }
  properties: {
    gatewayIPConfigurations: [ /* ... */ ]
    frontendIPConfigurations: [ /* public ip */ ]
    frontendPorts: [ { name: 'port80'; properties: { port: 80 } } ]
    backendAddressPools: [ { name: 'backendPool'; properties: { backendAddresses: [ { fqdn: 'myapp.internal' } ] } } ]
    httpListeners: [ /* listener mapping */ ]
    requestRoutingRules: [ /* rules */ ]
  }
}
```

---

## 16. TLS, End-to-end SSL, WAF Policies & Rewrite Rules

- **TLS options**:
  - Terminate TLS at Application Gateway (offload) then use HTTP to backends OR re-encrypt to backend by specifying HTTPS backend and providing trust (certificate or trusted CA).
  - Use TLS 1.2/1.3 where supported.
- **WAF**:
  - Managed rule sets (OWASP) block SQLi/XSS by default; configure custom rules to allow app-specific behavior.
  - Exclusions: tune WAF to avoid false positives for legitimate patterns (e.g., certain POST bodies).
  - Logging with Diagnostic Settings to Log Analytics and integration with SIEM.
- **Rewrite rules**:
  - Add/modify headers (HSTS, X-Frame-Options), URL rewrites for clean paths, add tracing headers for APM correlation.

Example: Configure WAF diagnostics to Log Analytics:
```bash
az monitor diagnostic-settings create --resource-id "/subscriptions/<sub>/resourceGroups/rg-network/providers/Microsoft.Network/applicationGateways/appgw1" --name appgw-waf-logs --workspace "/subscriptions/<sub>/resourceGroups/rg-monitor/providers/Microsoft.OperationalInsights/workspaces/myWorkspace" --logs '[{"category":"ApplicationGatewayFirewallLog","enabled":true}]'
```

---

## 17. Azure CDN — Overview & Use Cases

Azure CDN caches static and dynamic content at Microsoft's global edge points of presence (POPs) to improve performance and reduce origin load for web apps, media streaming, downloads, and API acceleration. Use cases:

- Fast global delivery of static assets (images, CSS, JS)
- Streaming and large file distribution
- Offloading origin compute and bandwidth costs
- Global failover and geo-based routing (when combined with Front Door or Traffic Manager)

Azure offers multiple CDN providers/offerings (Microsoft CDN, Verizon, Akamai) with feature differences; newer unified options and integrations exist (Azure CDN Standard/Premium, Verizon/Akamai profiles).

---

## 18. Azure CDN Profiles & Endpoints — Concepts

- **CDN Profile**: management container (billing, provider selection, SKU) where you choose the CDN provider (Akamai, Verizon, Microsoft) and SKU (Standard/Premium).
- **CDN Endpoint**: a hostname (e.g., `myapp.azureedge.net`) created under a profile. Each endpoint points to an origin (storage account, web app, custom origin).
- **Origin**: source where CDN fetches content from (Storage account blob, web app, external origin).
- **Caching rules & TTL**: control how long content is cached at the edge, either respect origin cache headers or override.
- **Custom domain mapping**: map `cdn.mycompany.com` to CDN endpoint and enable HTTPS (managed cert or bring your own cert).
- **Rules Engine (Premium/Standard features)**: perform request/response header modifications, path rewrites, redirect, cache key modifications, route to different origins, geo-filtering.

Hierarchy:
```
CDN Profile -> CDN Endpoint(s) -> Origin(s)
```

---

## 19. Creating & Configuring CDN Profiles/Endpoints (CLI / Portal)

### Portal steps (concise)
1. Create a **CDN Profile** (choose pricing tier/provider & resource group).
2. Add **CDN Endpoint**: provide endpoint name, origin type (Storage, Web App, custom), origin hostname, origin host header.
3. Configure caching and compression, set query string caching behavior, enable HTTP/2 and enable caching rules.
4. Add Custom Domain and enable HTTPS.
5. Enable diagnostic logs to storage/Log Analytics.

### CLI: create CDN Profile & Endpoint
```bash
# create profile
az cdn profile create -g rg-cdn -n myCdnProfile --sku Standard_Microsoft

# create endpoint pointing to storage account
az cdn endpoint create -g rg-cdn --profile-name myCdnProfile -n mycdnendpoint --origin mywebapp.azurewebsites.net --origin-host-header mywebapp.azurewebsites.net --force

# enable compression
az cdn endpoint update -g rg-cdn --profile-name myCdnProfile -n mycdnendpoint --enable-compression true
```

### PowerShell (Az.Cdn)
```powershell
New-AzCdnProfile -ResourceGroupName rg-cdn -Name myCdnProfile -Sku Standard_Microsoft
New-AzCdnEndpoint -ProfileName myCdnProfile -ResourceGroupName rg-cdn -Name mycdnendpoint -Origin mywebapp.azurewebsites.net -OriginHostHeader mywebapp.azurewebsites.net
```

---

## 20. CDN Features — Caching, Compression, Rules Engine, Geo-filtering, Purge

### Caching behavior
- TTL control via:
  - Origin response headers (`Cache-Control`, `Expires`) — default respected
  - CDN **caching rules** override TTL per path or pattern (e.g., set TTL for `/api/*` shorter than static content)
- Query string caching: options include ignore, include, or use specific query strings as cache key.

### Compression & Content optimization
- Enable compression for text-based resources (gzip, brotli on supported SKUs) at edge.
- Use cache-control `immutable` for long-lived assets and versioning via filenames for efficient caching.

### Rules Engine (advanced)
- Conditional routing based on path, header, query string, cookies, geo-location.
- Common actions: Rewrite URL, Set/Remove Header, Cache Purge or Cache Key modifications, Redirect (301/302), Block request by IP or geo.
- Example: route `/mobile/*` to a specific origin, or strip cookies for static asset caching.

### Geo-filtering
- Allow or block requests by client geolocation (country) — useful for compliance or content licensing.

### Purge (invalidate) cache
- Purge by content path or wildcard to force edge to re-fetch from origin.
```bash
az cdn endpoint purge -g rg-cdn --profile-name myCdnProfile -n mycdnendpoint --content-paths '["/images/*","/css/site.css"]'
```

### Origin failover & multiple origins
- Some CDN SKUs support origin groups with health probes and failover between origins.

---

## 21. Custom Domains, Certificates & HTTPS for CDN

- Map custom domain (CNAME or apex using ALIAS/ANAME) to CDN endpoint (`myapp.azureedge.net`).
- **Enable HTTPS**:
  - **Managed certificate (recommended)**: Let Azure provision and auto-renew a certificate for your custom domain (free).
  - **Bring Your Own Certificate (BYOC)**: upload certificate (PFX) or integrate with Key Vault depending on SKU.
- Steps:
  1. Add custom domain in CDN endpoint settings.
  2. Validate domain ownership with CNAME record.
  3. Enable HTTPS and await certificate provisioning.

Notes:
- For apex domains, use ANAME/ALIAS or use Front Door for CNAME flattening (DNS provider dependent).
- HSTS headers can be applied at origin or via rules engine.

---

## 22. Monitoring Azure CDN Resources — Metrics, Logs & Alerts

Key telemetry to monitor:
- **Cache Hit Ratio** (edge): percentage of requests served from edge vs origin — high hit ratio reduces origin load and cost.
- **Bandwidth (Bytes Transferred)**: important for billing & scaling.
- **Requests per second / Throughput**: detect traffic spikes.
- **Origin Latency / Origin Fetches**: indicates origin performance; high origin fetch or latency indicates cache misses or slow origin.
- **Purge / Invalidation operations** counts.
- **HTTP status codes** distribution (4xx/5xx) to find origin or client issues.

### Diagnostic settings for CDN
- Enable diagnostic logs (store to Storage Account, Event Hub, or Log Analytics).
  - **Access Logs**: request-level logs (client IP, path, status, cache status).
  - **Metrics**: aggregated metrics via Azure Monitor.
- CLI:
```bash
az monitor diagnostic-settings create --resource /subscriptions/<sub>/resourceGroups/rg-cdn/providers/Microsoft.Cdn/profiles/myCdnProfile/endpoints/mycdnendpoint --name cdnDiagnostics --workspace /subscriptions/<sub>/resourceGroups/rg-monitor/providers/Microsoft.OperationalInsights/workspaces/myWorkspace --logs '[{"category":"AccessLog","enabled":true}]' --metrics '[{"category":"All","enabled":true}]'
```

### Using Log Analytics / Traffic Analytics
- Ingest access logs into Log Analytics and run queries (Kusto) to produce dashboards:
  - Top URLs by traffic
  - Cache hit/miss ratio per path
  - Regions generating most requests
  - 4xx/5xx errors by origin or URL
- Example Kusto query snippet:
```kusto
AzureDiagnostics
| where ResourceProvider == "MICROSOFT.CDN" and Category == "AccessLog"
| summarize count() by bin(TimeGenerated, 1h), cs_uri_stem
| top 10 by count_
```

### Alerts
- Create metric alerts:
  - Low cache hit ratio (e.g., < 75% for 5 minutes)
  - High origin 5xx rate (> threshold)
  - Surge in bandwidth or request rate (protect cost)
- Create log alerts based on Kusto query (e.g., repeated 500 responses).

---

## 23. Monitoring & Diagnostics for Load Balancer, App Gateway & VPN Gateway

### Load Balancer
- **Metrics**: Data Path Availability, DipAvailability, SNAT connection count, SNAT Port Usage, Packet count.
- **Diagnostics**: enable load balancer diagnostics to send metrics/logs.
- **NSG Flow logs**: for traffic flows to/from backend pool.

### Application Gateway
- **Metrics**: TotalRequests, FailedRequests, ResponseStatus, Throughput, Throughput vs Backend health.
- **WAF Logs**: blocked requests with rule engine details.
- **Health Probes**: check backend health metrics.
- Enable diagnostic settings and route to Log Analytics for dashboards and WAF investigation.

### VPN Gateway
- **Metrics**: TunnelStatus, TunnelBytesIn/TunnelBytesOut, P2S Connection Count, Ingress/Egress bytes.
- **Diagnostics**: VPN diagnostic logs for connection failures; monitor BGP sessions and connection health.

---

## 24. Security Considerations & Best Practices Across Services

- **Least privilege**: restrict management plane access via Azure RBAC and Privileged Identity Management (PIM).
- **Network segmentation**: hub-and-spoke with centralized firewall to inspect and control east-west and north-south traffic.
- **Private connectivity**: prefer Private Endpoint for PaaS and Private Link for service access; reduce public exposure.
- **TLS everywhere**: terminate at edge or do end-to-end encryption to origin as required by compliance.
- **DDoS protection**: enable **DDoS Protection Standard** for public-facing endpoints with predictable mitigation and alerts.
- **Logging & retention**: keep logs for required retention periods for audits and security investigations.
- **WAF tuning**: monitor for false positives and periodically tune WAF rules.
- **Key/secret management**: use Key Vault + Managed Identity for certificate/credential management for load balancers and gateways.

---

## 25. Troubleshooting Checklist & Common Pitfalls

- **Connectivity**:
  - Check NSG effective rules (NIC & subnet), UDRs, and route table precedence.
  - Use Network Watcher `connection troubleshoot` and `ip flow verify`.
- **Health probe failing**:
  - Probe path must be accessible and return expected code (200). Ensure backend answers on probe port and path with correct host header if required.
- **Unexpected distribution**:
  - Check hashing-affinity (5-tuple), client source port variability; consider additional backend instances or change design if skew persists.
- **SSL issues**:
  - Ensure cert validity, hostname matches, intermediate CA chain installed on origin if doing end-to-end TLS.
- **DNS resolution**:
  - For Private Endpoints ensure Private DNS Zone auto-registration or manual DNS records linked to VNets.
- **CDN cache misses**:
  - Check cache-control headers, query string behavior, and rules engine that might vary cache key.
- **High SNAT port usage**:
  - Standard LB allows outbound rules; add public IPs or use NAT gateway to expand SNAT capacity for many outbound flows.

---

## 26. Costs & Sizing Considerations

- **Load Balancer**: Standard LB charged per SKU hour + data processed; Basic has different model and limited features.
- **Application Gateway**: charged by instance capacity units (v2) + data processed; WAF adds cost.
- **VPN Gateway**: charged per gateway SKU hourly + egress; ExpressRoute separately billed by provider and Microsoft.
- **CDN**: charged for data transfer (GB) and requests; cost varies by CDN provider/sku and edge location; caching reduces origin egress costs.
- Use reserved or committed plans where available; right-size, use autoscaling, and configure caching to reduce origin bandwidth.

---

## 27. References & Further Reading

- Azure Load Balancer documentation: https://learn.microsoft.com/azure/load-balancer/  
- Azure VPN Gateway documentation: https://learn.microsoft.com/azure/vpn-gateway/  
- Azure Application Gateway & WAF: https://learn.microsoft.com/azure/application-gateway/  
- Azure CDN docs: https://learn.microsoft.com/azure/cdn/  
- Network Watcher & NSG Flow Logs: https://learn.microsoft.com/azure/network-watcher/  
- Azure Architecture Center — hub-and-spoke, global load balancing: https://learn.microsoft.com/azure/architecture/

---

Prepared as a detailed, practical reference for architects and engineers designing and operating Azure L4/L7 load balancing, secure VPN connectivity, and content delivery at edge (CDN). If you’d like, I can produce:

- a step-by-step lab (CLI + portal) that wires up a VM Scale Set behind LB and exposes it via Application Gateway + CDN,  
- a Bicep template for a hub-and-spoke architecture including Firewall and NAT, or  
- a monitoring workbook (Log Analytics queries and dashboards) for CDN + App Gateway + LB.  

Which would you like next?