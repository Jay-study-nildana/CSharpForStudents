Problem: Git_Repo_Skeleton_and_Initial_Commits

Solution (skeleton plan, .gitignore, commit examples)

Repository skeleton (minimal)
```
/README.md
/.gitignore
/src/ProjectName/ProjectName.csproj
/src/ProjectName/Program.cs
/tests/ProjectName.Tests/ProjectName.Tests.csproj
/docs/ (optional)
/build/ (CI scripts)
```

Minimal .gitignore for C#/.NET
```
# Visual Studio
.vs/
*.user
*.suo

# Build
bin/
obj/

# Rider
.idea/

# OS
.DS_Store
Thumbs.db

# VS Code
.vscode/

# NuGet
*.nupkg
packages/
```

Initial commit sequence (example commands)
```bash
git init
git add README.md .gitignore src/ tests/
git commit -m "Init: project skeleton with README and .gitignore"
```

Three example commit messages
- "Init: add project and test project skeleton"
- "Feature: add Program boilerplate and sample HelloWorld"
- "CI: add GitHub Actions workflow for build and test"

Tips
- One logical change per commit; write a short imperative header and optional body for details.