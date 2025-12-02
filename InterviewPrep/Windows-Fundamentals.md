# Windows — Fundamentals, Kernel, Boot Process, File Systems, Components, Device & Memory Management  
Practical Reference for Developers, Sysadmins & Troubleshooters

---

## Table of Contents

1. [Scope & Purpose](#scope--purpose)  
2. [Windows Fundamentals — General Introduction](#windows-fundamentals---general-introduction)  
3. [Windows Editions & Roles — Client vs Server](#windows-editions--roles---client-vs-server)  
4. [Windows Kernel — Architecture & Components](#windows-kernel---architecture--components)  
   - Kernel types and the NT hybrid kernel  
   - User mode vs kernel mode components  
   - Communication channels (syscalls, IOCTLs, IRPs)  
   - The Executive: major subsystems  
5. [Booting Process — Overview & Phases](#booting-process---overview--phases)  
   - UEFI vs BIOS/legacy boot  
   - Boot phases & Windows components involved  
   - Types of boot (cold/warm/safe mode/PE/PXE/hibernation)  
   - Common boot troubleshooting commands  
6. [File Systems — FAT, NTFS & Windows File Operations](#file-systems---fat-ntfs--windows-file-operations)  
   - FAT variants (FAT12/16/32, exFAT)  
   - NTFS internals & features (MFT, journaling, ACLs, streams, reparse points, USN journal)  
   - File operations, permissions, encryption (EFS) and full-disk encryption (BitLocker)  
   - Useful file-system tools and commands  
7. [Windows Components — Registry & Services](#windows-components---registry--services)  
   - Registry structure, tools and best practices  
   - Windows services: management, types and patterns  
8. [Device Management — Drivers, Device Manager & Power Management](#device-management---drivers-device-manager--power-management)  
   - Device Manager, driver store, driver signing and verifier  
   - Power states (S0–S5), ACPI, hibernation and Wake Sources  
   - Event Viewer, Task Manager and common diagnostic flows  
9. [Memory Management — Concepts & Practical Commands](#memory-management---concepts--practical-commands)  
   - Virtual address space, paging, pagefile and working set  
   - Physical memory states and memory compression/caching  
   - Tools to inspect memory (Performance Monitor, RAMMap, Task Manager, Get-Process)  
10. [Other Topics: Backup, Patching, Antivirus & Best Practices](#other-topics-backup-patching-antivirus--best-practices)  
11. [Troubleshooting Recipes & Useful Commands (PowerShell & CMD)](#troubleshooting-recipes--useful-commands-powershell--cmd)  
12. [Glossary & Quick Reference](#glossary--quick-reference)  
13. [References & Further Reading](#references--further-reading)

---

## 1. Scope & Purpose

This document provides a concise but detailed reference to Windows internals and operational topics most useful for developers, system administrators, and technical interviews. It covers Windows client and server differences, kernel architecture (NT hybrid), boot phases, major file systems (FAT/NTFS), the registry, services, device & power management, memory management, and practical tooling and commands with examples.

All command examples use Windows PowerShell or CMD where noted. Adjust for your environment and Windows versions (Windows 10/11, Windows Server 2016/2019/2022).

---

## 2. Windows Fundamentals — General Introduction

- Windows is an operating system family from Microsoft comprising desktop (Client) and server variants. It includes kernel, device drivers, user-mode subsystems (Win32, .NET runtime), and management services.
- Key responsibilities: process and thread management, memory management, I/O management, networking, security, user interaction (GUI), and service hosting.

Core components:
- Kernel (NT kernel / ntoskrnl.exe) — manages low-level resources.
- Executive — collections of subsystems for memory, I/O, process, object manager.
- User-mode processes — Win32 subsystem, services, apps.
- Device drivers — run in kernel mode (with some user-mode drivers available via UMDF).

---

## 3. Windows Editions & Roles — Client vs Server

Client OS (e.g., Windows 10, 11):
- Optimized for interactive use, GUI, desktop applications, power management, multimedia.
- Features: Windows Store, consumer-focused features, client-side management.

Server OS (e.g., Windows Server 2016/2019/2022):
- Optimized for headless operation, roles (AD, DNS, DHCP, IIS, Hyper-V), multi-user performance, higher RAM/CPU support, server-grade security features.
- Includes Server Core option (minimal UI) and Nano Server (lightweight container-focused image in older versions).

Key differences:
- Licensing, default installed roles/features, networking stacks and cluster features, long-term servicing channels, maximum supported hardware (RAM/CPU), and default security hardening.

---

## 4. Windows Kernel — Architecture & Components

Windows uses a hybrid kernel (NT kernel) that blends monolithic and microkernel ideas. Main pieces:

- HAL (Hardware Abstraction Layer): abstracts platform differences (largely folded into kernel/firmware on modern systems).
- Kernel (ntoskrnl.exe): scheduling, interrupt dispatch, low-level synchronization.
- Executive: higher-level kernel-mode services including:
  - I/O Manager (I/O request packets - IRPs)
  - Object Manager (central registry of kernel objects)
  - Process and Thread Manager (create/terminate, scheduling)
  - Memory Manager (virtual memory, paging, working set, commit)
  - Security Reference Monitor (access checks, tokens)
  - Plug and Play Manager (device enumeration)
  - Power Manager (ACPI integration)

User mode vs Kernel mode:
- User mode: applications and many subsystems run with restricted privileges and separate address spaces; faults typically terminate process.
- Kernel mode: drivers and core OS components run with full privileges and shared address space. Bugs here can cause system-wide crashes (blue screen, bugcheck).

Communication between user-mode and kernel-mode:
- System calls: user-mode libraries (e.g., ntdll.dll) trap into kernel to execute privileged operations.
- Device I/O: application calls CreateFile/ReadFile/DeviceIoControl; Windows I/O Manager creates IRPs passed to drivers.
- IOCTLs (DeviceIoControl) let applications send custom control codes to drivers.
- APCs, completion ports, and overlapped I/O are common user/kernel coordination mechanisms.

IRP flow example (simplified):
- User calls ReadFile → I/O Manager creates IRP_MJ_READ → passes to driver stack → lower driver performs hardware read → completes IRP and signals completion to user via synchronous return, event, or I/O completion port.

Drivers and signing:
- Kernel drivers must be signed on modern Windows (especially on 64-bit). Use driver verifier and tools (Driver Verifier Manager) for testing.

---

## 5. Booting Process — Overview & Phases

High-level stages (UEFI-based modern boot):

1. Firmware initialization (UEFI/BIOS)  
   - POST (Power-On Self Test), initialize basic hardware, enumerate devices.  
   - UEFI parses boot entries and file system to find Boot Manager.

2. Boot Manager (Windows Boot Manager — bootmgr)  
   - Reads BCD (Boot Configuration Data), selects boot loader/entry.

3. Windows Boot Loader (winload.exe / winload.efi)  
   - Loads kernel (ntoskrnl.exe) and HAL, loads essential drivers (boot-start drivers), initializes kernel structures.

4. Kernel initialization  
   - Kernel initializes subsystems, starts Session Manager (smss.exe).

5. Session Manager (smss.exe)  
   - Starts environment for user sessions, creates system processes, starts Winlogon and Service Control Manager.

6. Winlogon and Logon process  
   - Winlogon handles interactive logon, starts user shell (explorer.exe) after authentication.

7. Services start — service control manager (services.exe) starts auto services.

Boot phases for troubleshooting:
- Boot Device selection (UEFI/BIOS)  
- Boot loader / BCD configuration  
- Kernel load / boot drivers  
- Session initialization / logon

Types of boot:
- Cold boot: power off → power on.
- Warm boot: restart without full power off.
- Safe mode (minimal drivers): F8 or Recovery options; useful for diagnosing driver/driver-init issues.
- Windows PE (WinPE): minimal OS for recovery and deployment.
- PXE / Network boot: boot image over network (WDS).
- Resume from hibernation: OS state saved to hiberfil.sys and restored.

Boot troubleshooting tools & commands:
- View BCD entries:
```powershell
bcdedit /v
```
- Repair boot records (use in WinRE):
```cmd
bootrec /fixmbr
bootrec /fixboot
bootrec /rebuildbcd
```
- View boot log (enable):
```powershell
bcdedit /set {default} bootlog Yes
```
- Use Event Viewer (System log) and `Get-WinEvent -LogName System` to find boot/shutdown events.
- Use `msconfig` (System Configuration) to change boot options and enable safe boot.

---

## 6. File System — Overview, FAT, NTFS & Operations

Overview
- Filesystems provide directories, files, metadata, and block mapping on storage media.
- Windows supports FAT variants (removable and legacy), exFAT (flash-friendly), and NTFS (default for Windows system drives). ReFS is used in some server/Storage Spaces scenarios.

FAT family
- FAT12/16/32: simple File Allocation Table, no permissions, limited file sizes and robustness; historically used on floppy/disks and some removable media.
- exFAT: designed for flash storage, large file support, simpler than NTFS (used for SDXC, USB drives).

NTFS (New Technology File System)
- Features:
  - Master File Table (MFT) — metadata records per file (attributes).
  - Journaling / recoverability: NTFS logs metadata changes for recovery (transaction-like).
  - Security: NTFS ACLs (DACLs and SACLs) with granular permissions (Allow/Deny, inheritance).
  - Alternate Data Streams (ADS): store additional named streams in the same file.
  - Reparse points & junctions: symbolic links, mount points, volume mount points.
  - Sparse files, compression (NTFS compression), encryption (EFS — per-file encryption).
  - USN Journal: logs changes to volume for change tracking.
- NTFS metadata and operations are handled by NTFS.sys driver and I/O Manager.

NTFS internals (brief):
- MFT contains file records (File Record Header), attributes (DATA, FILE_NAME, SECURITY_DESCRIPTOR).
- Allocation uses clusters; MFT itself is a file and has reserved MFT zone.
- Volume metadata in $MFT, $Bitmap, $LogFile, $Volume, $AttrDef, $Upcase, $Quota, $ObjId, $Reparse, $UsnJrnl.

Example file operations & permissions:
- Change owner/permissions via Windows Explorer or `icacls` CLI:
```powershell
icacls "C:\MyFolder" /grant "DOMAIN\User:(OI)(CI)F" /T
```
- View alternate data streams:
```powershell
Get-Item -Path .\file.txt -Stream *
```

Encryption:
- EFS (Encrypting File System): file-level encryption using user certificate — transparent to user when logged on.
- BitLocker: full volume encryption (hardware TPM recommended). Use `manage-bde` to manage.

File-system tools:
- chkdsk to check & repair:
```cmd
chkdsk C: /f /r
```
- fsutil for low-level operations:
```powershell
fsutil usn queryjournal C:
fsutil sparse setflag <file>
```
- mountvol for mounting/unmounting volumes.

Important notes:
- Use NTFS for system drives for security, permissions and advanced features.
- Beware of Alternate Data Streams for malware hiding; scanners may need to inspect ADS.

---

## 7. Windows Components — Registry & Services

Registry
- Hierarchical database storing configuration for OS, drivers, services and applications.
- Main hives:
  - HKEY_LOCAL_MACHINE (HKLM) — system and machine-wide settings.
  - HKEY_CURRENT_USER (HKCU) — per-user settings.
  - HKEY_CLASSES_ROOT, HKEY_USERS, HKEY_CURRENT_CONFIG.
- Tools:
  - regedit.exe – GUI registry editor (careful; editing can brick system).
  - reg.exe – command-line registry manipulation.
  - PowerShell: `Get-ItemProperty`, `Set-ItemProperty`, `Remove-ItemProperty`.
- Best practices:
  - Back up registry keys before changes (`reg export`).
  - Avoid storing large binary blobs in registry; use files for large data.
  - Use Group Policy for enterprise-wide configuration.

Services
- Windows services are long-running background processes managed by Service Control Manager (SCM).
- Service types: own process, share process (svchost grouping), kernel drivers (service-like).
- Management:
  - GUI: Services MMC (`services.msc`)
  - CMD: `sc.exe query`, `sc create`, `sc start`, `sc stop`
  - PowerShell:
```powershell
Get-Service -Name wuauserv
Start-Service -Name wuauserv
Stop-Service -Name wuauserv
Set-Service -Name 'MyService' -StartupType Automatic
```
- Service account context: LocalSystem, LocalService, NetworkService, or custom domain account.
- Use event logs (System/Application) to diagnose service failures; check service recovery options.

---

## 8. Device Management — Device Manager, Power & Event/Task Tools

Device Manager & drivers
- Device Manager displays enumerated hardware and driver status (devmgmt.msc).
- Driver management:
  - Driver store: managed by Windows (driver packages).
  - pnputil to manage driver packages:
```powershell
pnputil /enum-drivers
pnputil /add-driver .\driver.inf /install
pnputil /delete-driver oemXX.inf /uninstall
```
  - Update drivers via Windows Update, vendor installers, or Device Manager.

Driver signing
- Kernel drivers must be signed for 64-bit Windows; enable test-signing mode for development:
```cmd
bcdedit /set testsigning on
```
- Use Driver Verifier (verifier.exe) to stress-test drivers.

Power management
- ACPI governs power states; common system states (S-states):
  - S0 — Working
  - S1–S3 — Sleep (S3 = standby, suspend to RAM)
  - S4 — Hibernate (suspend to disk; system state in hiberfil.sys)
  - S5 — Soft Off (no context)
- Device power states (Dx): D0 (on) to D3 (off).
- Configure power plans via Control Panel or PowerShell:
```powershell
powercfg /list
powercfg /change standby-timeout-ac 30
powercfg /hibernate on
```
- Troubleshoot wake sources:
```powershell
powercfg -lastwake
powercfg -devicequery wake_armed
```

Event Viewer, Task Manager & Diagnostics
- Event Viewer (eventvwr.msc) shows System, Application, Security logs; use `Get-WinEvent` in PowerShell for queries:
```powershell
Get-WinEvent -LogName System -MaxEvents 50 | Format-Table TimeCreated, Id, LevelDisplayName, Message -AutoSize
```
- Task Manager (Ctrl+Shift+Esc) shows processes, performance, startup impact, users, services, and app history.
- More advanced tools:
  - Process Explorer (Sysinternals) — deep process inspection.
  - Process Monitor (Procmon) — trace file/registry/activity.
  - Device Performance & Health via Windows Security app or `Get-ComputerInfo`.

---

## 9. Memory Management — Terms & Practical Commands

Core concepts
- Virtual address space: each process gets its own linear virtual address space (x86: 32-bit legacy split, x64: large address space).
- Paging: virtual pages mapped to physical frames; pagefile (pagefile.sys) holds swapped pages.
- Working set: set of pages resident in physical memory for a process. Windows trims working set to manage memory.
- Commit charge: total virtual memory committed (in use or backed by pagefile).
- Page fault: soft page fault (page in RAM but not mapped) vs hard page fault (page must be read from disk).
- Memory lists: Active, Standby, Modified, Free.

Modern features:
- Memory compression (introduced in Windows 10): compress pages in RAM before paging to disk to reduce disk IO.
- SuperFetch / SysMain: preloads frequently used apps into memory (can be tuned or disabled).

Practical inspection tools and commands

Task Manager (Performance → Memory) — shows available, committed, cached memory.

PowerShell / Performance counters
```powershell
Get-Process | Sort-Object WorkingSet -Descending | Select-Object -First 10 Name, Id, @{n='WorkingSetMB';e={$_.WorkingSet/1MB}}
Get-Counter '\Memory\Available MBytes','\Memory\Committed Bytes' -MaxSamples 1
```

RAMMap (Sysinternals) — detailed physical memory usage breakdown.

Pagefile management
- View and set pagefile:
```powershell
Get-WmiObject -Class Win32_PageFileUsage
# change via GUI or sysdm.cpl -> Performance -> Advanced -> Virtual memory
```

Troubleshooting memory pressure:
- Check for processes consuming large working sets.
- Use `RAMMap` to see what caches/sections are occupying memory.
- Consider adding physical RAM, tuning pagefile, or investigating memory leaks.

---

## 10. Other Topics — Backup, Patching, Antivirus

Backup strategies
- File-level backups (File History, robocopy), volume-level (VSS snapshots), image-based backups (system image), and cloud backups.
- VSS (Volume Shadow Copy Service) provides consistent snapshots for files/databases.
- Built-in tools:
  - File History for user files (client).
  - Windows Server Backup (wbadmin) for server backups:
```powershell
wbadmin start backup -backupTarget:D: -include:C: -allCritical -quiet
```
  - `vssadmin list shadows` to view shadow copies.

Patching & updates
- Windows Update (Settings > Update & Security) and `wuauclt`/`usoclient` for legacy control.
- Windows Update for Business, WSUS, and SCCM/ConfigMgr for enterprise patch management.
- PowerShell: PSWindowsUpdate module (install from PSGallery) to query/install updates:
```powershell
Install-Module PSWindowsUpdate
Get-WindowsUpdate
Install-WindowsUpdate -AcceptAll -AutoReboot
```
- Use maintenance windows to perform updates, and test patches in staging before production.

Antivirus / Endpoint Protection
- Microsoft Defender (built-in) — manage via Security Center or PowerShell:
```powershell
Get-MpComputerStatus
Start-MpScan -ScanType QuickScan
Update-MpSignature
```
- For enterprise, use Microsoft Defender for Endpoint, third-party AV (Symantec, McAfee, CrowdStrike).
- Best practices: real-time protection, scheduled scans, up-to-date signatures, exclusion rules for performance-sensitive folders (with caution).

---

## 11. Troubleshooting Recipes & Useful Commands (PowerShell & CMD)

System info and logs
```powershell
systeminfo
Get-WinEvent -LogName System -MaxEvents 50
Get-HotFix
```

Disk & filesystem
```powershell
# list disks and partitions
Get-Disk
Get-Partition
# check and repair FS
chkdsk C: /f /r
# SFC and DISM
sfc /scannow
DISM /Online /Cleanup-Image /RestoreHealth
```

Boot troubleshooting (from WinRE / elevated cmd)
```cmd
bcdedit /enum
bootrec /fixmbr
bootrec /fixboot
bootrec /rebuildbcd
```

Service & processes
```powershell
Get-Service -Name wuauserv
sc query wuauserv
Get-Process | Sort-Object CPU -Descending | Select -First 10
tasklist /svc
taskkill /pid 1234 /f
```

Driver & device
```powershell
pnputil /enum-drivers
driverquery /v
verifier /query
```

Event log search example
```powershell
Get-WinEvent -FilterHashtable @{LogName='System'; Id=41} -MaxEvents 10
```

Network & connectivity
```powershell
ipconfig /all
Get-NetAdapter
Test-NetConnection -ComputerName google.com -Port 443
tracert google.com
```

Security & audit
```powershell
Get-EventLog -LogName Security -Newest 50
```

Restore points & system restore
- Create a restore point:
```powershell
CheckPoint-Computer -Description "Pre-Patch" -RestorePointType "MODIFY_SETTINGS"
```
- Use System Restore from WinRE to roll back problematic updates/drivers.

---

## 12. Glossary & Quick Reference

- NT kernel (ntoskrnl.exe): Windows kernel binary.
- HAL: Hardware Abstraction Layer.
- IRP: I/O Request Packet.
- IOCTL: I/O control codes passed to device drivers.
- MFT: Master File Table (NTFS).
- USN Journal: Update Sequence Number journal for change tracking.
- ACL: Access Control List.
- EFS: Encrypting File System (file-level).
- BitLocker: Full volume encryption.
- VSS: Volume Shadow Copy Service.
- SCM: Service Control Manager.
- ACPI: Advanced Configuration and Power Interface.
- S0–S5: System power states.
- Pagefile (pagefile.sys): swap file for virtual memory.

---

## 13. References & Further Reading

- Microsoft Docs — Windows Internals, Kernel Architecture: https://learn.microsoft.com/windows-hardware/drivers/  
- "Windows Internals" (Patterson & Russinovich) — deep dive into Windows internals.  
- NTFS technical paper and MSDN documentation for filesystem details.  
- PowerShell documentation and `Get-Help` for cmdlet reference.  
- Sysinternals Suite — Process Explorer, Process Monitor, Autoruns, RAMMap (https://docs.microsoft.com/sysinternals/)

---

Prepared as an operational and interview-ready reference for Windows fundamentals, kernel architecture, boot sequence, filesystems, registry/services, device & power management, memory behavior, and operational tooling. If you want, I can convert parts into printable cheat-sheets, create a one-page troubleshooting checklist, or generate PowerShell scripts that gather system diagnostics for triage. Which would you like next?