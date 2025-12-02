# Network Fundamentals, Protocols, OSI/TCP‑IP, Security & Web Services — Reference Guide

---

## Table of Contents

1. [Scope & Purpose](#scope--purpose)  
2. [Network Fundamentals — General Introduction](#network-fundamentals---general-introduction)  
3. [Network Devices & Their Roles](#network-devices--their-roles)  
   - Switch, Router, Hub, NIC, Repeater, Load Balancer, Firewall, Proxy  
4. [Network Communication — Types, Topologies & Layering](#network-communication---types-topologies--layering)  
   - LAN/WAN/PAN/MAN, Topologies, Host-to-Host model  
   - TCP/IP layers vs OSI model  
   - Encapsulation & De-Encapsulation, Peer-to-Peer communication  
5. [Protocols — Overview & Common Application Protocols](#protocols---overview--common-application-protocols)  
   - TCP/IP operation; Application layer services (FTP, HTTP/HTTPS, SMTP, IMAP/POP3, DNS, DHCP)  
   - Example commands: curl, dig, nslookup, ftp, telnet  
6. [OSI Transport Layer — TCP, UDP, ICMP](#osi-transport-layer---tcp-udp-icmp)  
   - TCP roles: connection management, reliability, flow & congestion control, header fields, handshake/teardown  
   - UDP characteristics and use cases  
   - ICMP types and uses (ping/traceroute)  
   - Practical tools: netstat/ss, tcpdump, wireshark examples  
7. [OSI Network Layer — IP, Routing & Forwarding](#osi-network-layer---ip-routing--forwarding)  
   - IPv4/IPv6 basics, subnetting, CIDR, ARP, next-hop, routing tables, static vs dynamic routing (RIP/OSPF/BGP)  
   - Packet forwarding and TTL handling  
   - `ip route` examples, `route` output, viewing routing table  
8. [OSI Data Link Layer — MAC, VLANs & Protocols](#osi-data-link-layer---mac-vlans--protocols)  
   - Sublayers: LLC and MAC, framing, error detection (CRC), ARP, MAC learning, switching, VLAN tagging (802.1Q), STP  
9. [OSI Physical Layer — Media & Signaling](#osi-physical-layer---media--signaling)  
   - Copper (UTP/STP), fiber (single-mode/multimode), wireless (802.11); connectors and line coding, bit rates and bandwidth  
10. [TCP/IP Suite — General Overview & Mapping](#tcpip-suite---general-overview--mapping)  
11. [Security — Firewalls, Proxies & Load Balancers](#security---firewalls-proxies--load-balancers)  
    - Types of firewalls (packet, stateful, application), common rules, NAT, PAT, proxy uses, SSL/TLS offloading, WAFs  
    - Example iptables/nft rules and simple firewall policy patterns  
12. [Web Services — Overview (REST, SOAP, gRPC)](#web-services---overview-rest-soap-grpc)  
    - Design styles, message formats, examples with curl and sample JSON/XML payloads, authentication considerations (OAuth, Basic, API keys, mTLS)  
13. [Diagnostics & Troubleshooting Tools & Commands](#diagnostics--troubleshooting-tools--commands)  
14. [Best Practices & Security Considerations](#best-practices--security-considerations)  
15. [Glossary & Quick Reference Ports](#glossary--quick-reference-ports)  
16. [References & Further Reading](#references--further-reading)

---

## 1. Scope & Purpose

This document is a compact, practical reference for networking fundamentals focused on concepts, common protocols, OSI/TCP‑IP models, devices, security primitives (firewalls, proxies), and web services. It includes command examples and quick troubleshooting recipes useful for developers, SREs and network beginners.

---

## 2. Network Fundamentals — General Introduction

A computer network is a set of interconnected devices that can exchange data. Networks are characterized by:
- topology (how devices are connected)
- protocols (rules for communication)
- addressing and routing (how packets reach destinations)
- services (DNS, DHCP, HTTP, etc.)

Key performance attributes:
- Bandwidth (throughput) — capacity (bits/sec)
- Latency — time to traverse network (ms)
- Jitter — variation of latency
- Loss — packet drop rate
- Reliability & availability

---

## 3. Network Devices & Their Roles

- NIC (Network Interface Card)
  - Physical/electrical (or virtual) interface on host. Has MAC address; operates at Data Link + Physical layers.

- Repeater
  - Layer 1 device that regenerates signal to extend cable distance (simple electrical repeater).

- Hub
  - A Layer 1 multiport repeater: forwards received bits to all ports (collision domain shared).

- Switch
  - Layer 2 device that forwards frames based on MAC table. Provides segmentation of collision domains. Supports VLANs (802.1Q), port security, link aggregation (LACP).

- Router
  - Layer 3 device that forwards packets between networks based on routing table and IP addresses. Performs route selection, can apply ACLs, NAT.

- Load Balancer (LB)
  - Distributes traffic across backends (L4: network-level, L7: application-level). Supports health checks, persistence/sticky sessions, SSL termination.

- Firewall
  - Filters traffic by policy. Types: packet-filtering (stateless), stateful inspection, application-layer (proxy/WAF). Often integrated with NAT.

- Proxy
  - Intermediary that forwards requests on behalf of clients. Types: forward proxy, reverse proxy, transparent proxy, caching proxy.

- Other: Bridge (L2), Gateway (L3+), IDS/IPS (detection/prevention), VPN concentrators.

Illustration (simple):
```
[Client NIC] -- [Switch] -- [Router] -- [Firewall] -- [Load Balancer] -- [Servers]
```

---

## 4. Network Communication — Types, Topologies & Layering

Network types:
- LAN (Local Area Network): limited area, high speed.
- WAN (Wide Area Network): spans geographic regions.
- MAN (Metropolitan), PAN (Personal Area Network).
- CAN (Campus Area Network), SAN (Storage Area Network).

Topologies:
- Bus, Star, Ring, Mesh (partial/full), Hybrid. Modern networks commonly use hierarchical star/tree with redundancy.

Host-to-host communication model
- Application -> Transport -> Network -> Data Link -> Physical (sender)
- Physical -> Data Link -> Network -> Transport -> Application (receiver)
- Encapsulation: each layer wraps the payload with headers (and trailers at Data Link).

TCP/IP vs OSI
- TCP/IP (practical): Application, Transport, Internet, Link (or Network Interface).
- OSI (conceptual): 7 layers — Application, Presentation, Session, Transport, Network, Data Link, Physical.

Encapsulation example (HTTP over TLS over TCP over IP over Ethernet):
- App: HTTP request (text)
- Presentation: TLS encrypts HTTP -> TLS record
- Transport: TCP segments with seq/ack
- Network: IP packet with src/dst IP
- Data Link: Ethernet frame with src/dst MAC
- Physical: bits on the wire (electrical/optical/wireless)

Peer-to-peer communication
- Each layer at sender exchanges with its peer at receiver via the encapsulation/de-encapsulation chain. Peer protocols define control information exchanged between same-level entities.

---

## 5. Protocols — Overview & Common Application Protocols

Purpose: provide standard services and message formats to enable interoperable communication.

Common Application layer protocols:
- HTTP / HTTPS — web; stateless, request/response. HTTPS = HTTP over TLS.
- FTP — file transfer (control on port 21, data on port 20 or negotiated passive ports).
- SMTP — sending mail (port 25/587 with TLS).
- POP3 / IMAP — mail retrieval (POP3 p 110/995, IMAP p 143/993).
- DNS — name resolution (UDP/53; TCP for zone transfers or large responses).
- DHCP — dynamic host configuration (UDP 67/68) — assigns IP, gateway, DNS.
- SSH — secure shell (port 22) for remote administration.
- Telnet — insecure legacy remote shell (port 23).
- SNMP — network management (161/162).
- TLS/SSL — encryption and server authentication (TLS 1.2/1.3 recommended).

TCP/IP features
- Best-effort delivery (IP) with optional reliable transport (TCP).
- Fragmentation (IP) and reassembly.
- ICMP for control messages (unreachables, TTL exceeded).

Example: DNS lookup with dig
```bash
dig +noall +answer www.example.com
# or
nslookup www.example.com 8.8.8.8
```

Example: test HTTP endpoint
```bash
curl -v https://api.example.com/health
```

Example: show DHCP lease info (Linux)
```bash
# systemd-resolved + DHCP client info:
nmcli device show eth0 | grep IP4.DNS
```

---

## 6. OSI Transport Layer — TCP, UDP, ICMP

### TCP — core mechanisms
- Connection-oriented, reliable, byte-stream abstraction over IP.
- Header highlights: Source Port, Dest Port, Seq, Ack, Data Offset, Flags (SYN/ACK/FIN/RST), Window, Checksum, Options (MSS, SACK, Timestamps).
- Three-Way Handshake (establish connection):
  1. Client -> Server: SYN, seq=x
  2. Server -> Client: SYN+ACK, seq=y, ack=x+1
  3. Client -> Server: ACK, ack=y+1
- Teardown: FIN/ACK sequence (can be four-way).

Reliability & flow control
- Sequence numbers and ACKs enable ordered delivery and retransmission on loss.
- Sliding window and advertised window control flow (receiver capacity).
- Congestion control algorithms: Slow Start, Congestion Avoidance, Fast Retransmit, Fast Recovery (classic TCP Reno/HyStart, and newer algorithms like CUBIC).
- Retransmission timeout (RTO) adapts by RTT estimates.

Practical: view TCP connections and stats
```bash
# Linux
ss -tulpan
# or
netstat -tnp

# Capture a handshake
sudo tcpdump -n -i eth0 'tcp[tcpflags] & (tcp-syn|tcp-fin) != 0' -w tcp-flags.pcap
```

### UDP
- Connectionless; minimal header (src port, dst port, length, checksum).
- No guaranteed delivery, ordering, or congestion control.
- Use cases: DNS queries, video streaming, VoIP, DNS, DHCP, time-sensitive apps favoring low latency.

### ICMP (Internet Control Message Protocol)
- Used for network diagnostics and error reporting: echo request/reply (ping), destination unreachable, TTL exceeded (used by traceroute).
- Example: `ping` and `traceroute`:
```bash
ping -c 4 8.8.8.8
traceroute 8.8.8.8
# Windows: tracert 8.8.8.8
```

---

## 7. OSI Network Layer — IP, Routing & Packet Forwarding

### IPv4 basics
- 32-bit address, usually represented as dotted decimal with CIDR suffix (e.g., 192.168.1.0/24).
- Subnet mask defines network vs host bits.

CIDR & subnetting example
- /24 = 255.255.255.0 -> 256 addresses (254 usable hosts)
- /28 -> 16 addresses (14 usable)

Calculate network and broadcast:
- Network = host & mask
- Broadcast = network | inverted-mask

ARP (Address Resolution Protocol)
- Maps IPv4 to MAC on LAN. ARP request: "who has IP X?"; ARP reply includes MAC.
- View ARP table:
```bash
arp -n              # Linux
ip neigh show
```

Routing fundamentals
- Router maintains routing table with destination prefix, next-hop, interface, and metric.
- Next hop: IP of next router to forward packet to reach some destination.
- Packet forwarding: router decrements TTL, consults routing table, forwards to next hop, possibly performs NAT.

View route table
```bash
ip route show
route -n
```

Dynamic routing protocols
- RIP: distance-vector, simple, hop-count limited to 15.
- OSPF: link-state, areas, faster convergence for interior routing.
- BGP: exterior gateway protocol for Internet routing (policy-based).
- EIGRP: Cisco proprietary (hybrid).

Example static route (Linux)
```bash
sudo ip route add 10.10.0.0/16 via 192.168.1.1 dev eth0
```

IPv6 notes
- 128-bit addresses, IPv6 uses neighbor discovery (replaces ARP), multicast-based address resolution, no NAT typically.

---

## 8. OSI Data Link Layer — Sublayers & Protocols

Sublayers:
- Logical Link Control (LLC): provides multiplexing, flow & error control services.
- Media Access Control (MAC): frame delimiting, addressing (MAC), error detection (FCS/CRC).

Frame structure (Ethernet II):
- Preamble | Dest MAC | Src MAC | EtherType | Payload | FCS

Services:
- Framing, addressing, error detection (CRC), MAC-level flow control, link aggregation.

Switching and MAC learning
- Switch builds MAC table: learns source MAC on ingress port; forwards frames to port matching dest MAC; floods if unknown.
- CAM table entries age out; security features can lock ports to specific MACs.

VLANs (802.1Q)
- Tagging adds 4-byte VLAN tag (TPID + TCI) to frames; isolates traffic into virtual LANs on a single physical switch.
- Trunk ports carry multiple VLANs tagged; access ports typically untagged for a single VLAN.

Spanning Tree Protocol (STP)
- Prevents loops in Layer 2 networks by creating a loop-free tree; blocks redundant ports; RSTP (rapid) is faster.

Protocols at Data Link
- PPP (Point-to-Point Protocol), HDLC, SLIP (legacy), LACP for link aggregation.

---

## 9. OSI Physical Layer — Media & Signaling

Role: transmission of raw bits over a physical medium.

Media types:
- Copper (UTP Cat5e/Cat6/Cat6A/Cat7): 100BASE-TX, 1000BASE-T, 10GBASE-T (longer cables & better shielding required).
- Fiber:
  - Multimode (OM1/OM2/OM3/OM4) for shorter distances, cheaper transceivers.
  - Single-mode for long distance.
  - Connectors: LC, SC, ST.
- Wireless: 802.11 family (a/b/g/n/ac/ax). Medium is shared radio spectrum; uses modulation schemes (OFDM, QAM).
- Coaxial (legacy, cable networks).

Signaling and encoding:
- Line coding (NRZ, Manchester), modulation (QAM), symbol rate vs bit rate.
- Channel capacity approximated by Shannon-Hartley theorem.

Physical layer services
- Bit synchronization, bit transmission, electrical/optical/air interface, physical connectors and layout.

---

## 10. TCP/IP Suite — General Overview & Mapping

Common mapping:
- Application: HTTP, DNS, SMTP, SSH
- Transport: TCP, UDP
- Internet: IP (IPv4/IPv6), ICMP
- Link: Ethernet, Wi‑Fi, PPP

TCP/IP was developed as a practical, interoperable stack used on the Internet. OSI remains a conceptual model for learning and design.

---

## 11. Security — Firewalls, Proxies & Load Balancers

Firewalls
- Packet-filtering (stateless): inspect headers and allow/deny based on rules (ACLs).
- Stateful firewalls: track connection state (e.g., ESTABLISHED, RELATED) and apply rules based on the state.
- Application-layer firewalls / WAFs: inspect HTTP/HTTPS payload for attacks (SQLi, XSS).
- Host-based firewall (Windows Firewall, ufw, firewalld, iptables/nftables on Linux) vs network firewall (border devices).

IP Tables (legacy) example (Linux):
```bash
# Allow established/related, allow SSH, drop rest (IPv4)
sudo iptables -P INPUT DROP
sudo iptables -A INPUT -m conntrack --ctstate ESTABLISHED,RELATED -j ACCEPT
sudo iptables -A INPUT -p tcp --dport 22 -j ACCEPT
sudo iptables -A INPUT -i lo -j ACCEPT
# Save rules depends on distro (iptables-save/iptables-restore)
```

nftables example:
```bash
sudo nft add table inet filter
sudo nft 'add chain inet filter input { type filter hook input priority 0 ; }'
sudo nft add rule inet filter input ct state established,related accept
sudo nft add rule inet filter input iif "lo" accept
sudo nft add rule inet filter input tcp dport 22 accept
sudo nft add rule inet filter input drop
```

NAT (Network Address Translation)
- SNAT/Masquerade: outbound traffic from private network rewritten to public IP.
- DNAT/Port Forwarding: inbound traffic forwarded to internal host.

Proxy uses
- Forward proxy: client-side intermediary, can provide caching, filtering or anonymity.
- Reverse proxy: placed in front of servers (NGINX, HAProxy, Envoy) to load-balance, terminate TLS, do routing, caching, security filtering.
- Transparent proxy: intercepts traffic without client configuration.

Load balancer types & uses
- L4 (transport-level): distribute by TCP/UDP (fast, simple).
- L7 (application-level): content-based routing (HTTP host/path), cookie-based persistence, SSL termination.
- Health checks and circuit breaking are common features.

TLS termination in LB
- Offloading TLS at LB reduces server CPU and centralizes cert management; use mTLS for strong mutual auth when needed.

WAF example
- Deploy WAF in front of application to block OWASP Top 10 threats; commonly integrated with CDNs or reverse proxies.

---

## 12. Web Services — Overview (REST, SOAP, gRPC)

Web service styles
- REST (Representational State Transfer)
  - Principles: stateless, resources identified by URIs, use of HTTP verbs (GET/POST/PUT/DELETE), common use of JSON format.
  - Example:
```bash
# GET resource
curl -v https://api.example.com/v1/orders/123

# POST create
curl -X POST -H "Content-Type: application/json" -d '{"customerId":42,"items":[...]}'
  https://api.example.com/v1/orders
```

- SOAP (Simple Object Access Protocol)
  - XML-based RPC protocol often using WSDL. Heavier than REST, supports WS-* standards (security, transactions).
  - Example payload (XML) over HTTP POST.

- gRPC
  - RPC framework using HTTP/2 and protocol buffers (binary). Strong typing, streaming support, high performance.
  - Use for microservices internal comms; requires codegen for stubs.

Design concerns:
- Idempotency (safe retries): GET, PUT are idempotent; POST is not necessarily.
- Versioning: URI versioning (/v1/), header-based, or content negotiation.
- Pagination, filtering and rate limiting for APIs.
- Authentication: API keys, OAuth2 (Bearer tokens), JWT, mTLS.
- CORS: cross-origin resource sharing for browsers — configure Access-Control-Allow-* headers on server.

Example REST JSON response
```json
{
  "id": 123,
  "status": "shipped",
  "items": [{"sku":"ABC","qty":2}],
  "links": {"self":"/orders/123"}
}
```

Monitoring and Observability
- Instrument APIs with metrics (request latency, error rate), logs (structured), and distributed tracing (W3C Trace Context, OpenTelemetry).

---

## 13. Diagnostics & Troubleshooting Tools & Commands

Common tools and commands (Linux/Windows network diagnostics):
- ping — test ICMP echo
- traceroute / tracert — path discovery (TTL-based)
- curl — HTTP client for debugging endpoints
- dig / nslookup — DNS queries
- ss / netstat — socket and connection listing
- tcpdump / tshark — packet capture and basic filters
  ```bash
  # capture TCP handshake on port 443
  sudo tcpdump -i eth0 'tcp port 443 and (tcp[tcpflags] & (tcp-syn|tcp-ack) != 0)'
  ```
- wireshark — GUI packet analysis and protocol dissection
- iperf3 — bandwidth testing (client/server)
- nmap — port scanning and service discovery
- ip route / route / netstat -r — view routing table
- arp / ip neigh — check local ARP/neighbor cache
- ss -s ; ss -tuln — socket summaries and listening ports

Example: check open ports and which process owns them
```bash
sudo ss -tulpn
# or
sudo netstat -tulpn
```

Capture HTTP transaction with curl and view raw headers:
```bash
curl -v -H "Accept: application/json" https://api.example.com/health
```

Trace an HTTP request using curl with timing:
```bash
curl -w "@curl-format.txt" -o /dev/null -s "https://example.com"
# curl-format.txt contains variables like %{time_total}
```

---

## 14. Best Practices & Security Considerations

- Use encryption: TLS 1.2+ (prefer 1.3) for all sensitive traffic; avoid obsolete ciphers.
- Principle of least privilege: restrict ACLs, firewall rules, and service accounts.
- Network segmentation: separate management, application, and database networks; use VLANs and firewalls between tiers.
- Use HTTPS + HSTS, secure cookies, and proper CSRF protections for web apps.
- Implement monitoring, logging, and alerting for network health and security events (IDS/IPS, SIEM).
- Harden endpoints: update OS and network devices, use strong auth (keys, 2FA), and disable unused services.
- Rate limiting and quotas to protect APIs and backend resources.
- Regular backups, DR planning, and test restores.
- Document ingress/egress rules and maintain an inventory of network devices and interfaces.

---

## 15. Glossary & Quick Reference Ports

Common TCP/UDP ports:
- 20/21 FTP data/control
- 22 SSH
- 23 Telnet (insecure)
- 25 SMTP
- 53 DNS (UDP/TCP)
- 67/68 DHCP (server/client)
- 80 HTTP
- 110 POP3
- 123 NTP
- 143 IMAP
- 443 HTTPS
- 3306 MySQL
- 5432 PostgreSQL
- 6379 Redis
- 9200 Elasticsearch

Quick OSI mapping reminders:
- Layer 7: Application — HTTP, FTP, DNS, SMTP
- Layer 4: Transport — TCP, UDP
- Layer 3: Network — IP, ICMP
- Layer 2: Data Link — Ethernet, ARP, VLANs
- Layer 1: Physical — cabling, signalling

---

## 16. References & Further Reading

- RFC 791 — IPv4, RFC 2460 — IPv6  
- RFC 793 — TCP, RFC 768 — UDP, RFC 792 — ICMP  
- "Computer Networking: A Top-Down Approach" — Kurose & Ross  
- Wireshark and tcpdump documentation  
- IETF RFCs for specific protocols (DNS, DHCP, SMTP, HTTP/1.1 RFC 7230–7235, HTTP/2, HTTP/3)  
- NIST and vendor docs for secure network architecture and firewall design

---

Prepared as a practical network fundamentals reference covering devices, protocols, OSI/TCP‑IP models, routing and switching basics, diagnostics and security considerations, and an overview of web services and common application-layer protocols. If you'd like, I can generate:  
- a one-page cheat sheet (PDF/Markdown) of the most important commands and ports,  
- a hands-on lab with `tcpdump`/`wireshark` exercises, or  
- a small set of sample firewall and reverse-proxy configuration files (nginx, HAProxy, nft/iptables). Which would you like next?