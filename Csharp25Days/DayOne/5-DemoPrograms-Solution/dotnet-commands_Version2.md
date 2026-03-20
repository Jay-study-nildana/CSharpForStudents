# dotnet CLI — Generic Steps for Any Console Demo Project

This guide gives a short, generic set of steps you can apply to create, build, run, and manage any of your simple console programs using the dotnet CLI. Replace placeholders like <ProjectName> with the actual name for each program (for example GreetingApp).

Prerequisite
- .NET SDK installed and available on your PATH. Verify:
```bash
dotnet --info
```

1) Create a solution (optional but useful)
```bash
dotnet new sln -n MyConsoleDemos
```
This creates MyConsoleDemos.sln to group multiple projects.

2) Create a new console project
```bash
dotnet new console -o <ProjectName>
```
- This creates a folder named `<ProjectName>` with a .csproj and a default Program.cs.
- To target a particular framework, add `-f net8.0` (or other target), e.g. `dotnet new console -o <ProjectName> -f net8.0`.

3) Add the project to the solution (if using a solution)
From the folder where the .sln lives:
```bash
dotnet sln MyConsoleDemos.sln add <ProjectName>/<ProjectName>.csproj
```

4) Replace the default Program.cs with your named file (optional)
If you have a source file named after your program (e.g., GreetingApp.cs), copy it into the project folder and remove the default Program.cs to avoid duplicate Main entry points.
```bash
# Copy/replace example (POSIX)
cp /path/to/GreetingApp.cs <ProjectName>/GreetingApp.cs
rm <ProjectName>/Program.cs

# Windows PowerShell example
Copy-Item C:\path\to\GreetingApp.cs -Destination .\<ProjectName>\GreetingApp.cs
Remove-Item .\<ProjectName>\Program.cs
```
Note: The compiler compiles all `.cs` files in the project folder. You can use any file names; just ensure only one `Main` entry point per executable project.

5) Restore dependencies (usually automatic during build)
```bash
dotnet restore <ProjectName>/<ProjectName>.csproj
```
This fetches NuGet packages if your project needs them.

6) Build the project
```bash
dotnet build <ProjectName>/<ProjectName>.csproj
```
- Fix any compiler errors reported, then rebuild.
- Build the entire solution:
```bash
dotnet build MyConsoleDemos.sln
```

7) Run the project
From inside the project folder:
```bash
cd <ProjectName>
dotnet run
```
Or run from any folder:
```bash
dotnet run --project <ProjectName>/<ProjectName>.csproj
```

8) Run in Debug/Release modes
- Default is Debug configuration. Build with Release optimizations:
```bash
dotnet build -c Release <ProjectName>/<ProjectName>.csproj
```
- Run with specified configuration:
```bash
dotnet run -c Release --project <ProjectName>/<ProjectName>.csproj
```

9) Clean the project
```bash
dotnet clean <ProjectName>/<ProjectName>.csproj
```

10) Publish (create distributable output)
```bash
dotnet publish -c Release -o publish/<ProjectName> <ProjectName>/<ProjectName>.csproj
```
- The publish folder contains all files needed to run the app on the target runtime.
- Add `-r <RID>` and `--self-contained true` for platform-specific standalone executables.

11) Useful development helpers
- Open solution in VS Code:
```bash
code .
```
- Hot reload (if desired for rapid iteration):
```bash
dotnet watch --project <ProjectName>/<ProjectName>.csproj run
```

12) Initialize Git and make an initial commit
```bash
git init
git add .
git commit -m "Initial commit: <ProjectName> console demo"
```
Recommended .gitignore entries: bin/, obj/, .vs/, *.user

13) Troubleshooting tips (generic)
- Compiler errors: Read the file/line reported and fix the source code. Errors prevent assembly creation.
- Duplicate Main: If you see a duplicate entry-point error, remove extra `Main` methods or ensure only one executable project has a `Main`.
- Missing dependencies: Run `dotnet restore` or inspect the .csproj's `<PackageReference>` elements.
- Runtime exceptions: Use `dotnet run` under your IDE debugger or add Console logging to identify the failing stack trace.
- If a renamed file is not picked up: ensure it is inside the project folder; the SDK includes all `.cs` files by default unless the project file explicitly changes that behavior.

14) Generic checklist for running any demo
- Create project: `dotnet new console -o <ProjectName>`
- Place your <ProjectName>.cs file into the project folder (remove default Program.cs if necessary)
- Build: `dotnet build <ProjectName>/<ProjectName>.csproj`
- Run: `dotnet run --project <ProjectName>/<ProjectName>.csproj`
- If used in a solution: `dotnet sln add <ProjectName>/<ProjectName>.csproj`

15) Automation (optional)
You can put repetitive steps in a shell script or batch file to create multiple projects at once. If you want, I can prepare a script that:
- Creates a solution
- Scaffolds n console projects
- Adds them to the solution

If you’d like that script, tell me your preferred shell (bash or PowerShell) and the list of project names to include.