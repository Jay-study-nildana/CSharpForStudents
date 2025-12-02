# PowerShell Scripting — Reference Guide with Examples

---

## Table of Contents

1. [Scope & Purpose](#scope--purpose)  
2. [PowerShell — General Overview](#powershell---general-overview)  
   - What & Why  
   - Short History & Evolution  
   - PowerShell vs Command Prompt (cmd.exe)  
   - PowerShell ISE and modern editor choices  
3. [Key Concepts & Application Areas](#key-concepts--application-areas)  
4. [Features — Remoting, Background Jobs, Transactions, File Transfer](#features---remoting-background-jobs-transactions-file-transfer)  
   - Remoting (Enter-PSSession, Invoke-Command, PSSessions)  
   - Background Jobs and ThreadJobs  
   - Transactions (limited/provider-dependent support)  
   - Network file transfer: Invoke-WebRequest, Invoke-RestMethod, Start-BitsTransfer, SMB copy  
5. [CmdLet — Basics and Advanced Cmdlets](#cmdlet---basics-and-advanced-cmdlets)  
   - Naming convention Verb-Noun and discoverability  
   - Common built-in cmdlets  
   - Advanced functions that behave like cmdlets (CmdletBinding)  
6. [Scripts — Creating, Running & Language Features](#scripts---creating-running--language-features)  
   - Script files, execution policy, dot-sourcing, module layout  
   - Data types, variables, constants (read-only), arrays, hashtables, PSCustomObject  
   - Comparison operators and matching operators  
   - Looping and flow control: for, foreach, foreach-object, while, do/while, switch  
7. [Practical Examples & Recipes](#practical-examples--recipes)  
8. [Debugging & Logging](#debugging--logging)  
9. [Best Practices & Security Considerations](#best-practices--security-considerations)  
10. [References & Further Reading](#references--further-reading)

---

## 1. Scope & Purpose

This document is a practical reference for PowerShell scripting: core concepts, cmdlets, remoting, background jobs, transaction notes, file transfer over network, script authoring patterns, data types, variables, operators, looping, debugging and more — with code examples you can run or adapt.

Target: Windows PowerShell and PowerShell (Core) / PowerShell 7+. Most examples are compatible with PowerShell 5.1 and 7+; some cross-platform notes included.

---

## 2. PowerShell — General Overview

What & Why
- PowerShell is an object-based shell and scripting language (designed by Microsoft). Unlike traditional shells that pass text, PowerShell passes .NET objects between commands (cmdlets).
- Great for automation, configuration management, administration, orchestration, DevOps tasks, cloud automation (Azure, AWS), and cross-platform scripting with PowerShell Core.

Short History & Evolution
- Windows PowerShell (1.0 in 2006 → 5.1 in Windows Management Framework).
- PowerShell Core (cross-platform) built on .NET Core; PowerShell 7+ is the modern cross-platform release.
- PowerShellGalleries and modules grew rapidly; PowerShell is now used on Linux/macOS too.

PowerShell vs Command Prompt (cmd.exe)
- PowerShell:
  - Passes objects (rich metadata) through pipeline.
  - Verb-Noun cmdlet naming (Get-Process, Stop-Service).
  - Rich language features (functions, classes, modules, error handling).
  - Integrated help system (Get-Help).
- CMD:
  - Text-oriented, limited scripting features.
  - Useful for very simple tasks and backward compatibility.

PowerShell ISE and Modern Editor Choices
- PowerShell ISE: built-in Windows script editor (no longer actively developed for cross-platform).
- Recommended: Visual Studio Code with PowerShell extension (cross-platform, IntelliSense, debugging).
- Other editors: JetBrains Rider (with plugins), Sublime, or plain editors + `pwsh` CLI.

---

## 3. Key Concepts & Application Areas

- Cmdlets: small .NET-based commands (Verb-Noun). They follow conventions and are discoverable via `Get-Command` and documented via `Get-Help`.
- Pipelines: pass objects from one cmdlet to another (`|`), allowing composition and filtering.
- Providers: present data stores as drives (Registry `HKLM:`, filesystem `C:`).
- Modules: packages of functions, cmdlets, types, and help (distributed as `.psm1`/.dll and through PSGallery).
- Remoting: run commands on remote machines via WinRM or SSH transport.
- Jobs: background execution (local or remote).
- Desired State Configuration (DSC): declarative configuration management.
- PowerShell classes and modules allow production-grade code organization.

---

## 4. Features — Remoting, Background Jobs, Transactions, File Transfer

Remoting
- Enable remoting on Windows hosts:
```powershell
# Enable WinRM-based remoting (run as admin)
Enable-PSRemoting -Force
```
- Enter interactive remote session:
```powershell
Enter-PSSession -ComputerName server01 -Credential (Get-Credential)
# or with SSH transport (PowerShell 7+)
Enter-PSSession -HostName server.example.com -UserName user
```
- Run a command on one or many remote machines:
```powershell
# single machine
Invoke-Command -ComputerName server01 -ScriptBlock { Get-Process }

# multiple machines in parallel (returns results)
$servers = 'web01','web02'
Invoke-Command -ComputerName $servers -ScriptBlock { Get-Service w3svc } -AsJob
```
- Persistent sessions:
```powershell
$s = New-PSSession -ComputerName server01
Invoke-Command -Session $s -ScriptBlock { Get-EventLog -LogName System -Newest 10 }
Remove-PSSession $s
```
- Import remote modules:
```powershell
Import-PSSession -Session $s -Module SqlServer
```

Background Jobs
- Run tasks in background on local or remote hosts.
```powershell
# Start a local background job
$job = Start-Job -ScriptBlock { Get-Process | Where-Object CPU -gt 10 }

# Check job status and results
Get-Job
Receive-Job -Job $job -Keep
Remove-Job -Job $job

# Run on remote machines as job
Invoke-Command -ComputerName server01 -ScriptBlock { Get-ChildItem C:\ } -AsJob
```
- Use ThreadJob module for lower-overhead thread-based jobs:
```powershell
Install-Module -Name ThreadJob
Start-ThreadJob -ScriptBlock { 1..1000 | ForEach-Object { $_ * 2 } }
Get-Job | Receive-Job
```

Transactions (limited, provider-dependent)
- PowerShell includes transaction cmdlets: `Start-Transaction`, `Complete-Transaction`, `Undo-Transaction`, `Get-Transaction`. Important: only commands and providers that explicitly support transactional semantics will participate (this is limited; TxF was deprecated on Windows).
- Example (conceptual — provider must support transactions):
```powershell
Start-Transaction
New-Item -Path C:\MyTest\file1.txt -ItemType File
New-Item -Path C:\MyTest\file2.txt -ItemType File
# If all good
Complete-Transaction
# Or to roll back
Undo-Transaction
```
- Note: transactional support varies by environment and cmdlet. Use explicit error handling and idempotent scripts in production.

Network File Transfer
- HTTP/HTTPS:
```powershell
# simple download
Invoke-WebRequest -Uri "https://example.com/file.zip" -OutFile "C:\temp\file.zip"

# REST API and JSON parsing
$response = Invoke-RestMethod -Uri "https://api.example.com/items/1"
$response.name
```
- BITS (Background Intelligent Transfer Service) — good for large file transfers and resiliency (Windows):
```powershell
Start-BitsTransfer -Source "https://example.com/large.iso" -Destination "C:\temp\large.iso"
```
- SMB / copy:
```powershell
Copy-Item -Path "\\fileserver\share\backup.zip" -Destination "C:\backups\" -Verbose
```
- SSH/SCP (PowerShell Core on systems with `scp`):
```powershell
scp user@server:/path/file.zip C:\temp\          # uses underlying scp
```

---

## 5. CmdLet — Basics and Advanced Cmdlets

Cmdlet Naming and Discovery
- Format: Verb-Noun (e.g., `Get-Process`, `Set-Item`, `Remove-Item`).
- Use `Get-Command` and `Get-Help`:
```powershell
Get-Command -Verb Get
Get-Help Get-Process -Full
Update-Help  # update local help (requires admin)
```

Common built-in cmdlets
- `Get-ChildItem` (ls), `Get-Item`, `Set-Item`, `Remove-Item`
- `Get-Content`, `Set-Content`, `Add-Content`
- `Get-Process`, `Stop-Process`, `Start-Process`
- `Get-Service`, `Start-Service`, `Stop-Service`
- `Get-EventLog` / `Get-WinEvent`, `Get-EventSubscriber`
- `Get-Module`, `Import-Module`, `Install-Module` (PowerShellGet)
- `Test-Connection` (ping), `Test-NetConnection`

Advanced Function (acts like a cmdlet)
- Use `[CmdletBinding()]` to make a function behave like an advanced cmdlet:
```powershell
function Get-Even {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory,$true,ValueFromPipeline)]
        [int[]] $Value
    )
    process {
        foreach ($v in $Value) {
            if ($v % 2 -eq 0) { Write-Output $v }
        }
    }
}
# usage
1..10 | Get-Even
```
- Supports `-WhatIf` and `-Confirm` when you add `[CmdletBinding(SupportsShouldProcess=$true)]` and call `if ($PSCmdlet.ShouldProcess(...)) { ... }`.

Parameter binding examples:
```powershell
function Set-Greeting {
    [CmdletBinding()]
    param(
        [Parameter(Position=0, Mandatory=$true)]
        [string] $Name,
        [Parameter()]
        [ValidateSet('Hi','Hello','Greetings')]
        [string] $Prefix = 'Hello'
    )
    "$Prefix, $Name"
}
Set-Greeting -Name Bob -Prefix Hi
```

Pipeline input attributes:
- `ValueFromPipeline`, `ValueFromPipelineByPropertyName` allow pipeline binding.

---

## 6. Scripts — Creating, Running & Language Features

Creating Scripts
- PowerShell scripts are files with `.ps1` extension.
- Use a clear header comment, and modules for reusable code.
- Example script `backup.ps1`:
```powershell
param(
    [Parameter(Mandatory=$true)]
    [string] $Source
)

$dest = "C:\backups\$(Split-Path $Source -Leaf)-$(Get-Date -Format yyyyMMdd).zip"
Compress-Archive -Path $Source -DestinationPath $dest -Force
Write-Output "Backup saved to $dest"
```

Execution Policy & Running Scripts
- Check current policy:
```powershell
Get-ExecutionPolicy -List
```
- Set user-scoped policy:
```powershell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser -Force
```
- Run script:
```powershell
# From current directory (explicit path)
.\backup.ps1 -Source "C:\Data\MyApp"

# Dot-sourcing to load functions into current scope
. .\myscript.ps1
```

Modules
- Create `.psm1` with functions and a module manifest `.psd1`.
- Import module:
```powershell
Import-Module ./MyTools.psm1
Get-Module -ListAvailable
```
- Publish to PSGallery with `Publish-Module` (requires PowerShellGet and publisher settings).

Data types & variables
- Variables are prefixed with `$`:
```powershell
$a = 5
$b = "hello"
$pi = 3.14159
```
- Common types: `[int]`, `[string]`, `[bool]`, `[datetime]`, arrays and hashtables.
- Arrays:
```powershell
$arr = 1,2,3
$arr += 4
$arr[0] # 1
```
- Hashtables:
```powershell
$h = @{ Name = 'Alice'; Age = 30 }
$h['Name']
```
- PSCustomObject for structured objects:
```powershell
$obj = [PSCustomObject]@{ Name='App'; Version='1.0' }
$obj.Version
```

Constants / Read-only variables
- You can set variable options to prevent change:
```powershell
Set-Variable -Name 'MyConst' -Value 42 -Option Constant
# or
Set-Variable -Name 'MyReadOnly' -Value "x" -Option ReadOnly
```
- `Constant` cannot be removed or changed in the session.

Comparison & Matching Operators
- Numeric & equality:
  - `-eq`, `-ne`, `-lt`, `-le`, `-gt`, `-ge`
- String and wildcard:
  - `-like` (wildcard `*`), `-notlike`
- Regex:
  - `-match` (case-insensitive), `-cmatch` (case-sensitive), `-notmatch`
- Collection membership:
  - `-contains` (collection contains value), `-in` (value is in collection)
- Examples:
```powershell
if ($value -eq 10) { "ten" }
'abc' -like 'a*'   # True
'abc' -match '\w+' # True
```

Looping and flow control
- `for`, `foreach`, `while`, `do { } while`, `switch`
```powershell
for ($i=0; $i -lt 5; $i++) { $i }

foreach ($f in Get-ChildItem) {
    $f.Name
}

# pipeline foreach-object (streaming)
Get-ChildItem | ForEach-Object { $_.Name }
```

Error handling
- Use `try/catch/finally` and `$ErrorActionPreference`:
```powershell
try {
    Get-Item 'C:\does-not-exist' -ErrorAction Stop
} catch [System.Management.Automation.ItemNotFoundException] {
    Write-Warning "Missing item: $_"
} catch {
    Write-Error "General error: $_"
} finally {
    Write-Host "Cleanup"
}
```
- `-ErrorAction Stop` forces non-terminating errors into catchable exceptions.

---

## 7. Practical Examples & Recipes

List services and stop one safely
```powershell
Get-Service -Name wuauserv, bits | Format-Table -AutoSize

# Stop service if running
$svc = Get-Service wuauserv
if ($svc.Status -eq 'Running') {
    Stop-Service -Name wuauserv -Force -WhatIf  # remove -WhatIf to perform
}
```

Find and remove old files
```powershell
Get-ChildItem -Path "C:\Logs" -Recurse -File |
    Where-Object { $_.LastWriteTime -lt (Get-Date).AddDays(-30) } |
    ForEach-Object { Remove-Item $_.FullName -Force -Verbose }
```

Parallel remoting with throttling
```powershell
$servers = Get-Content servers.txt
$script = { Get-EventLog -LogName System -Newest 10 }

# run up to 10 at a time
$jobs = foreach ($s in $servers) {
    Start-Job -ScriptBlock { param($hn,$sb) Invoke-Command -ComputerName $hn -ScriptBlock $sb } -ArgumentList $s,$script
    # throttle logic could be added here
}
```

Making an advanced cmdlet-like function with pipeline input
```powershell
function Test-UriAvailability {
    [CmdletBinding()]
    param(
        [Parameter(ValueFromPipeline=$true, Position=0, Mandatory=$true)]
        [string[]] $Uri
    )
    process {
        foreach ($u in $Uri) {
            try {
                $r = Invoke-WebRequest -Uri $u -Method Head -TimeoutSec 10 -ErrorAction Stop
                [PSCustomObject]@{ Uri = $u; Status = $r.StatusCode }
            } catch {
                [PSCustomObject]@{ Uri = $u; Status = 'Error'; Message = $_.Exception.Message }
            }
        }
    }
}
# usage
'http://example.com','https://nonexistent.test' | Test-UriAvailability
```

Download large file with resumable BITS
```powershell
Start-BitsTransfer -Source "https://example.com/large.bin" -Destination "C:\temp\large.bin"
```

Import a CSV and save JSON
```powershell
Import-Csv users.csv | ConvertTo-Json -Depth 4 | Set-Content users.json -Encoding utf8
```

Create a scheduled task (register a scheduled job)
```powershell
$trigger = New-ScheduledTaskTrigger -Daily -At 3am
$action = New-ScheduledTaskAction -Execute 'PowerShell.exe' -Argument '-File C:\Scripts\backup.ps1'
Register-ScheduledTask -TaskName "NightlyBackup" -Trigger $trigger -Action $action -User "SYSTEM"
```

---

## 8. Debugging & Logging

- Use `Write-Output` / `Write-Verbose` / `Write-Warning` / `Write-Error` / `Write-Debug` to communicate intent and debugging information.
- Enable verbose/debug:
```powershell
$VerbosePreference = 'Continue'
# or call function with -Verbose, -Debug
MyFunction -Verbose
```
- Use `Set-PSDebug -Trace 1` to trace script execution (verbose, lower-level).
- `pdb` equivalent: use Visual Studio Code debugger (set breakpoints, watch variables).
- Insert `Stop-Process` or `Read-Host` for quick breakpoints, or `Enter-PSHostProcess` and `Debug-Process` for advanced debugging.
- Logging to files:
```powershell
Start-Transcript -Path "C:\Logs\script.log" -Append
# run script logic
Stop-Transcript
```

---

## 9. Best Practices & Security Considerations

- Use modules and modularize scripts (`.psm1`), keep `param()` and validation attributes for inputs.
- Use `CmdletBinding()` and proper parameter attributes for reusability and pipeline support.
- Use `-WhatIf` and `-Confirm` when performing destructive operations; implement `SupportsShouldProcess`.
- Avoid hard-coded credentials. Use `Get-Credential`, Windows Credential Manager, Azure Key Vault, or encrypt secrets.
- Set proper file permissions for scripts and logs.
- Use `Set-ExecutionPolicy -Scope CurrentUser RemoteSigned` rather than machine-wide Unrestricted where possible.
- Test scripts in a non-production environment first. Use `-WhatIf` and `-Confirm`.
- Keep third-party modules up to date and verify sources (PSGallery has many community modules).
- Prefer secure protocols (WinRM over HTTPS, SSH transport for remoting, TLS for web transfers).
- Use error handling with `try/catch/finally` and fail gracefully.
- Use version control (Git) for scripts, adopt a CI/CD flow for automation scripts.

---

## 10. References & Further Reading

- Microsoft Docs — PowerShell documentation: https://learn.microsoft.com/powershell  
- PowerShell Gallery: https://www.powershellgallery.com/  
- PowerShell Team Blog and GitHub repo: https://github.com/PowerShell/PowerShell  
- Books: "Learn PowerShell in a Month of Lunches" (Don Jones & Jeffrey Hicks), "PowerShell in Depth"  
- VS Code PowerShell extension: https://marketplace.visualstudio.com/items?itemName=ms-vscode.PowerShell

---

Prepared as a practical, example-rich reference to PowerShell scripting for automation, administration, and DevOps tasks. If you’d like, I can:  
- generate a starter script repository with examples and a CI job,  
- convert key examples to ready-to-run `.ps1` files, or  
- produce a printable cheat-sheet of common cmdlets and idioms. Which would you like next?