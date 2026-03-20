Problem: VSCode_LaunchJson_For_DotNet_Console

Solution (launch.json snippet)

1. Create `.vscode/launch.json` with this content:

```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": ".NET Core Launch (console)",
      "type": "coreclr",
      "request": "launch",
      "preLaunchTask": "build",
      "program": "${workspaceFolder}/src/ProjectName/bin/Debug/net7.0/ProjectName.dll",
      "args": [],
      "cwd": "${workspaceFolder}/src/ProjectName",
      "console": "internalConsole",
      "stopAtEntry": false
    },
    {
      "name": ".NET Core Attach",
      "type": "coreclr",
      "request": "attach",
      "processId": "${command:pickProcess}"
    }
  ]
}
```

2. How to use:
   - Run → Start Debugging (F5) to launch the console app; set breakpoints in source before starting.
   - Use the "Attach" configuration to attach to a running process (useful for services).

3. VS Code tips:
   - Ensure C# extension (Omnisharp / .NET Debugger) is installed.
   - Use the Debug view to see Variables, Watch, Call Stack, Breakpoints.
   - For hot reload work, use `dotnet watch` in terminal and attach.

Notes
- Adjust `program` path to target framework and build configuration. Use `${command:pickProcess}` for attach convenience.