# DevOps Basics Reference Guide  
*(with GitHub, GitHub Actions, and Azure Cloud)*

---

## Table of Contents

1. [What is DevOps?](#what-is-devops)
2. [DevOps Lifecycle & Principles](#devops-lifecycle--principles)
3. [Source Control with Git & GitHub](#source-control-with-git--github)
4. [GitHub: Key Concepts](#github-key-concepts)
5. [Introduction to CI/CD](#introduction-to-cicd)
6. [GitHub Actions: Concepts & Workflow](#github-actions-concepts--workflow)
7. [Sample GitHub Actions Workflow](#sample-github-actions-workflow)
8. [Azure Cloud for DevOps](#azure-cloud-for-devops)
9. [Integrating GitHub with Azure](#integrating-github-with-azure)
10. [Common DevOps Interview Questions & Answers](#common-devops-interview-questions--answers)
11. [DevOps Best Practices](#devops-best-practices)
12. [Resources & Further Reading](#resources--further-reading)

---

## 1. What is DevOps?

**DevOps** is a set of practices, tools, and a cultural philosophy that automates and integrates the processes between software development and IT teams.  
**Goal:** Shorten the development lifecycle and deliver high-quality software continuously.

---

## 2. DevOps Lifecycle & Principles

**Phases:**
- Plan
- Code
- Build
- Test
- Release
- Deploy
- Operate
- Monitor

**Principles:**
- **Collaboration:** Developers & operations work together.
- **Automation:** Of build, test, deploy, and monitoring.
- **Continuous Integration (CI):** Frequently merging code.
- **Continuous Delivery/Deployment (CD):** Automated release to production.
- **Feedback & Monitoring:** Quick issue detection and resolution.

---

## 3. Source Control with Git & GitHub

- **Git:** Distributed version control system for tracking code changes.
- **GitHub:** Cloud platform for hosting Git repositories.

**Key Git commands:**
- `git clone <repo-url>`: Clone repository
- `git status`: Show changed files
- `git add <file>`: Stage file
- `git commit -m "message"`: Commit changes
- `git push`: Upload local commits
- `git pull`: Fetch and merge remote changes
- `git branch`: List/create branches
- `git checkout <branch>`: Switch branch

---

## 4. GitHub: Key Concepts

| Concept          | Description                                              |
|------------------|---------------------------------------------------------|
| **Repository**   | Project storage with code, issues, and documentation    |
| **Branch**       | Parallel version of the codebase                        |
| **Pull Request** | Propose changes to be merged into another branch        |
| **Issue**        | Track bugs, tasks, or features                          |
| **Actions**      | Automate workflows (CI/CD, testing, deployments, etc.)  |
| **Wiki/README**  | Project documentation                                   |

---

## 5. Introduction to CI/CD

- **Continuous Integration (CI):** Automatically building and testing code whenever changes are pushed.
- **Continuous Delivery (CD):** Automatically preparing code for release.
- **Continuous Deployment (CD):** Automatically releasing code to production.

**Benefits:**
- Faster feedback
- Less manual work
- Faster, more reliable releases

---

## 6. GitHub Actions: Concepts & Workflow

- **GitHub Actions**: Native CI/CD and automation service in GitHub.
- **Workflow**: Automated process defined in `.github/workflows/*.yml`.
- **Trigger**: Event that starts a workflow (e.g., push, pull_request).
- **Job**: Set of steps that run sequentially or in parallel.
- **Step**: Individual task (e.g., run command, set up environment).
- **Runner**: Machine where jobs run (GitHub-hosted or self-hosted).

---

## 7. Sample GitHub Actions Workflow

```yaml
# .github/workflows/dotnet.yml
name: .NET Build and Test

on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: Set up .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0.x'
      - name: Restore dependencies
        run: dotnet restore
      - name: Build
        run: dotnet build --no-restore
      - name: Test
        run: dotnet test --no-build --verbosity normal
```
**Explanation:**  
- Triggers on push or pull request to `main`
- Checks out code, sets up .NET, restores/builds/tests project

---

## 8. Azure Cloud for DevOps

- **Azure:** Microsoft’s cloud platform (compute, storage, databases, networking, AI, DevOps tools, etc.)
- **Azure DevOps:** Suite of tools for DevOps (Repos, Pipelines, Boards, Artifacts).
- **Azure App Service:** Host web apps and APIs.
- **Azure Container Registry:** Store Docker images.
- **Azure Key Vault:** Store secrets, keys, certificates.

---

## 9. Integrating GitHub with Azure

- **Continuous Deployment:** Connect GitHub repo to Azure App Service for auto-deployments.
- **GitHub Actions for Azure:** Use official actions to deploy to Azure services.

**Example: Deploy to Azure Web App via GitHub Actions**
```yaml
jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: Set up .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0.x'
      - name: Build and publish
        run: dotnet publish -c Release -o ${{env.DOTNET_ROOT}}/myapp
      - name: Deploy to Azure Web App
        uses: azure/webapps-deploy@v3
        with:
          app-name: 'YOUR-APP-NAME'
          slot-name: 'Production'
          publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
          package: ${{env.DOTNET_ROOT}}/myapp
```
- **publish-profile** is a secret you store in the repo Settings > Secrets

---

## 10. Common DevOps Interview Questions & Answers

### 1. What is DevOps and why is it important?
> DevOps combines software development and IT operations to shorten the development lifecycle and provide continuous delivery with high software quality. It encourages collaboration, automation, and monitoring, leading to faster and more reliable releases.

### 2. What is CI/CD?
> CI/CD stands for Continuous Integration and Continuous Delivery/Deployment. CI is about automatically testing and integrating changes into the codebase; CD is about automatically deploying those changes to staging or production environments.

### 3. What are the benefits of using GitHub Actions?
> GitHub Actions allows you to automate workflows directly in your GitHub repository, reducing manual steps, providing fast feedback, ensuring consistent builds/tests, and integrating deployment with various cloud providers.

### 4. How would you set up automated deployments from GitHub to Azure?
> Use GitHub Actions to build and test code, then use official Azure actions (like `azure/webapps-deploy`) to deploy to Azure App Service. Store secrets like publish profiles in GitHub Secrets.

### 5. What is Infrastructure as Code (IaC)?
> IaC is the practice of managing and provisioning infrastructure through machine-readable configuration files (e.g., ARM templates, Bicep, Terraform), enabling automation, versioning, and repeatability.

### 6. What is a GitHub runner?
> A runner is a server that executes GitHub Actions jobs. GitHub provides hosted runners, but you can also use self-hosted runners for custom environments.

### 7. How do you manage secrets in GitHub Actions and Azure?
> Use GitHub Secrets to store sensitive information (e.g., API keys, publish profiles). In Azure, use Azure Key Vault for managing secrets, and access them securely from applications or pipelines.

### 8. Explain the difference between Azure DevOps and GitHub Actions.
> Both offer CI/CD and collaboration tools. Azure DevOps is a comprehensive suite with Boards, Pipelines, Repos, Artifacts, and Test Plans. GitHub Actions is tightly integrated with GitHub for workflow automation but is focused on CI/CD and automation.

### 9. What is a pull request, and how does it fit in a DevOps workflow?
> A pull request is a way to propose changes in code, enabling code review and automated testing before merging into the main branch. It helps ensure code quality and collaboration.

### 10. What is blue-green deployment?
> Blue-green deployment is a release management strategy that reduces downtime and risk by running two identical production environments (blue and green). Only one serves live traffic; new versions are deployed to the idle environment, then traffic is switched over.

---

## 11. DevOps Best Practices

- Automate everything (build, test, deploy)
- Keep infrastructure as code
- Use feature branches and pull requests
- Run tests on every commit
- Use secrets management (never hardcode passwords/keys)
- Monitor applications and infrastructure
- Use small, frequent deployments
- Roll back quickly on failure

---

## 12. Resources & Further Reading

- [GitHub Docs](https://docs.github.com/)
- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [Azure DevOps Docs](https://docs.microsoft.com/en-us/azure/devops)
- [Azure for Students](https://azure.microsoft.com/en-us/free/students/)
- [Learn: GitHub Actions](https://lab.github.com/githubtraining/github-actions:-hello-world)
- [Learn: Azure Fundamentals](https://learn.microsoft.com/en-us/training/paths/azure-fundamentals/)

---

**Practical Exercise:**

1. Create a GitHub repository.
2. Add a simple .NET (or Node.js, Python, etc.) app.
3. Write a GitHub Actions workflow to build and test your app.
4. Set up an Azure Web App.
5. Configure GitHub Actions to deploy to Azure using a publish profile stored in GitHub Secrets.
6. Make code changes, open a pull request, and watch the CI/CD pipeline run.

---

*Prepared for first-time developer interview candidates. This guide covers conceptual, practical, and best-practice aspects of DevOps with GitHub, GitHub Actions, and Azure Cloud.*