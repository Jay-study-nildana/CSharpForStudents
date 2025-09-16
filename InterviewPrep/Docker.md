# Docker Interview Reference Guide for .NET Developers

---

## Table of Contents

1. [Introduction to Docker](#introduction-to-docker)
2. [Core Docker Concepts](#core-docker-concepts)
3. [Docker Architecture](#docker-architecture)
4. [Why Use Docker in .NET Development?](#why-use-docker-in-net-development)
5. [The Docker Workflow for .NET Developers](#the-docker-workflow-for-net-developers)
6. [Writing Dockerfiles for .NET](#writing-dockerfiles-for-net)
7. [Multi-Stage Builds](#multi-stage-builds)
8. [Persisting & Managing Data](#persisting--managing-data)
9. [Networking in Docker](#networking-in-docker)
10. [Docker Compose Overview](#docker-compose-overview)
11. [Best Practices for .NET + Docker](#best-practices-for-net--docker)
12. [Common Docker Commands](#common-docker-commands)
13. [Sample .NET Docker Project](#sample-net-docker-project)
14. [Common Interview Questions & Answers](#common-interview-questions--answers)
15. [Troubleshooting & Debugging Tips](#troubleshooting--debugging-tips)
16. [Resources & Further Reading](#resources--further-reading)

---

## 1. Introduction to Docker

Docker is an open-source platform that automates application deployment inside lightweight, portable containers. Containers bundle code, runtime, libraries, and dependencies—making it easy to run your application consistently across different environments.

- **Key Point:** Docker is not a virtual machine. Containers share the host OS kernel and use less overhead.

---

## 2. Core Docker Concepts

| Term          | Definition                                                                 |
|---------------|----------------------------------------------------------------------------|
| **Image**     | Read-only template with instructions for creating a Docker container        |
| **Container** | Runnable instance of an image                                              |
| **Dockerfile**| Text file with instructions for building a Docker image                    |
| **Registry**  | Storage for Docker images (e.g., Docker Hub, Azure Container Registry)     |
| **Volume**    | Special directory on the host for persistent/container-shared data         |
| **Network**   | Virtual network for containers to communicate                              |

---

## 3. Docker Architecture

- **Docker Daemon:** Background process managing containers/images.
- **Docker Client:** CLI for interacting with Docker.
- **Docker Registries:** Store and distribute images.
- **Docker Compose:** Tool for defining and running multi-container apps.

Diagram:

```
  +-------------+        +--------------+       +-------------+
  | Docker CLI  | <----> | Docker Daemon| <-->  | Registries  |
  +-------------+        +--------------+       +-------------+
                               |
                        +-------------+
                        | Containers  |
                        +-------------+
```

---

## 4. Why Use Docker in .NET Development?

- **Consistency:** Identical environments from dev to production.
- **Isolation:** Run multiple .NET apps with different dependencies.
- **Portability:** Works on Windows, Linux, Mac, on-premises or cloud.
- **Simplified CI/CD:** Build, test, and deploy using containers.
- **Microservices:** Each service in its own container.

---

## 5. The Docker Workflow for .NET Developers

1. **Write App Code:** Develop your .NET project (ASP.NET Core, console, etc).
2. **Create Dockerfile:** Describe how to build your app image.
3. **Build Image:**
   ```
   docker build -t my-dotnet-app .
   ```
4. **Run Container:**
   ```
   docker run -d -p 8080:80 my-dotnet-app
   ```
5. **(Optional) Push to Registry:**
   ```
   docker tag my-dotnet-app mydockerhubid/my-dotnet-app:latest
   docker push mydockerhubid/my-dotnet-app:latest
   ```

---

## 6. Writing Dockerfiles for .NET

### ASP.NET Core Example

```dockerfile
# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "MyDotNetApp.dll"]
```

**Explanation:**
- Uses multi-stage build for smaller images.
- Publishes your .NET app to a folder, then copies only published output into the final image.

---

## 7. Multi-Stage Builds

- **Why?** To keep images small and secure.
- **How?** Use multiple `FROM` statements. Build dependencies are not included in final image.
- **Example:** Above Dockerfile.

---

## 8. Persisting & Managing Data

Containers are ephemeral. To persist data:

- **Volumes:**  
  ```
  docker run -v mydata:/app/data my-dotnet-app
  ```
- **Bind Mounts:**  
  ```
  docker run -v /host/path:/app/data my-dotnet-app
  ```
- **Docker Compose Volumes:**  
  ```yaml
  services:
    db:
      image: postgres
      volumes:
        - dbdata:/var/lib/postgresql/data
  volumes:
    dbdata:
  ```

---

## 9. Networking in Docker

- **Bridge Network (default):** Isolates containers.
- **Host Network:** Container shares host’s network stack.
- **Custom Networks:** For explicit inter-container communication.
- **Expose Ports:**  
  ```
  docker run -p 5000:80 my-dotnet-app
  ```
  Host port 5000 maps to container port 80.

---

## 10. Docker Compose Overview

- **Purpose:** Run multi-container .NET solutions (web + db + cache).
- **File:** `docker-compose.yml`
- **Example:**

```yaml
version: '3.4'
services:
  web:
    image: my-dotnet-app
    build: .
    ports:
      - "8080:80"
    depends_on:
      - db
  db:
    image: postgres
    environment:
      POSTGRES_PASSWORD: ExamplePassword
    volumes:
      - dbdata:/var/lib/postgresql/data
volumes:
  dbdata:
```
- **Usage:**  
  ```
  docker-compose up
  ```

---

## 11. Best Practices for .NET + Docker

- Use official Microsoft images for SDK/runtime.
- Prefer multi-stage builds.
- Avoid running as root in containers.
- Set non-root user for runtime image:
  ```dockerfile
  # after COPY and before ENTRYPOINT
  RUN useradd -m appuser
  USER appuser
  ```
- Keep Dockerfiles tidy: use `.dockerignore`.
- Use environment variables for config/secrets (never hardcode secrets).
- Minimize image layers and size.
- Clean up intermediate files in Dockerfile.

---

## 12. Common Docker Commands

| Command                                | Description                         |
|-----------------------------------------|-------------------------------------|
| `docker build -t tag .`                 | Build image from Dockerfile         |
| `docker images`                         | List local images                   |
| `docker run -d -p host:container image` | Run container in detached mode      |
| `docker ps`                             | List running containers             |
| `docker stop <container>`               | Stop a running container            |
| `docker rm <container>`                 | Remove a container                  |
| `docker rmi <image>`                    | Remove an image                     |
| `docker logs <container>`               | View container logs                 |
| `docker exec -it <container> sh`        | Get shell access to a container     |
| `docker network ls`                     | List docker networks                |
| `docker volume ls`                      | List docker volumes                 |

---

## 13. Sample .NET Docker Project

**Project Structure:**

```
/MyDotNetApp
  /Controllers
  /Models
  /Views
  Program.cs
  Startup.cs
  MyDotNetApp.csproj
  Dockerfile
  .dockerignore
  docker-compose.yml
```

**.dockerignore Example:**
```
bin/
obj/
*.md
.git/
```

---

## 14. Common Interview Questions & Answers

### Basic

**1. What is Docker and why is it useful?**

> Docker is a platform that allows you to package applications and their dependencies into lightweight, portable containers. It’s useful because it ensures consistency across different environments, simplifies deployment, and allows applications to run reliably regardless of where they are deployed.

**2. Explain the difference between an image and a container.**

> An image is a read-only template with instructions for creating a Docker container. A container is a running instance of an image, with its own filesystem, processes, and network, isolated from the host and other containers.

**3. What is a Dockerfile?**

> A Dockerfile is a text file with a set of instructions to build a Docker image. It specifies the base image, copies files, installs dependencies, sets environment variables, exposes ports, and defines the startup command for the container.

**4. How does Docker help with microservices and scalability?**

> Docker allows each microservice to run in its own container, isolated from others. This makes it easy to scale services independently, update or roll back specific services, and maintain a consistent environment across development, testing, and production.

**5. How do you persist data in Docker?**

> By using Docker volumes or bind mounts. Volumes are managed by Docker and are suitable for persistent storage, while bind mounts map a host directory to the container. Both approaches ensure that data is not lost when the container stops or is removed.

---

### Intermediate

**6. Describe a multi-stage build and its benefits.**

> A multi-stage build in Docker uses multiple FROM statements in a Dockerfile to separate build and runtime environments. This allows you to copy only the necessary artifacts to the final image, reducing image size and improving security since build tools and dependencies are not present in the runtime image.

**7. How do you debug a failing .NET container?**

> - Use `docker logs <container>` to check logs for errors.
> - Use `docker exec -it <container> sh` or `bash` to access the container’s shell for manual inspection.
> - Check the Dockerfile for missing dependencies or incorrect paths.
> - Ensure the correct ports are exposed and mapped.
> - Verify environment variables and configuration files.

**8. How would you expose your .NET app running in Docker to the host?**

> Use the `-p` flag with `docker run`, e.g., `docker run -p 5000:80 my-dotnet-app`, which maps port 80 in the container to port 5000 on the host. The app will then be accessible at `localhost:5000`.

**9. What is Docker Compose and how does it help .NET developers?**

> Docker Compose is a tool for defining and running multi-container Docker applications using a `docker-compose.yml` file. It simplifies managing related services (like web apps, databases, caches) by allowing you to start, stop, and configure them together, especially useful for .NET solutions with multiple dependencies.

**10. How can you make your Dockerized .NET app production-ready?**

> - Use multi-stage builds to minimize image size.
> - Use official base images.
> - Set a non-root user.
> - Store secrets using environment variables or secret management tools.
> - Regularly update images for security.
> - Set resource limits.
> - Use health checks and logging.

---

### Advanced

**11. How do you handle environment-specific configuration in containers?**

> Through environment variables, configuration files, or Docker secrets. In .NET, you can use `appsettings.{Environment}.json` files and override settings using environment variables or Docker Compose’s `environment` key.

**12. Explain the concept of orchestration (Kubernetes, Swarm) as it relates to Docker.**

> Orchestration tools like Kubernetes and Docker Swarm manage clusters of Docker containers, handling deployment, scaling, networking, and health monitoring automatically, enabling reliable and scalable production environments for containerized applications.

**13. How do you secure secrets in Docker containers?**

> Never store secrets in images or Dockerfiles. Use environment variables, Docker secrets (with Swarm), or external secret management solutions like Azure Key Vault. Also, set permissions carefully and avoid exposing sensitive information in logs.

**14. What are best practices for minimizing Docker image size?**

> - Use multi-stage builds.
> - Base your images on the smallest possible official images (e.g., `alpine` where possible).
> - Only copy needed files.
> - Clean up temporary files and caches in the Dockerfile.
> - Avoid installing unnecessary packages.

**15. How do you monitor and log containers in production?**

> Use centralized logging solutions (e.g., ELK Stack, Azure Monitor) by forwarding container logs. For monitoring, use tools like Prometheus, Grafana, or cloud-native solutions to track container health, resource usage, and events.

---

## 15. Troubleshooting & Debugging Tips

- **Check logs:**  
  ```
  docker logs <container>
  ```
- **Shell access:**  
  ```
  docker exec -it <container> bash
  ```
- **Check image history:**  
  ```
  docker history <image>
  ```
- **Use `docker inspect` for config/details.**
- **Common issues:**
  - Port conflicts
  - Missing dependencies in image
  - File permissions
  - Environment variable misconfiguration

---

## 16. Resources & Further Reading

- [Official Docker Documentation](https://docs.docker.com/)
- [Dockerizing .NET Applications](https://learn.microsoft.com/en-us/dotnet/core/docker/)
- [Microsoft Container Samples](https://github.com/dotnet/dotnet-docker)
- [Docker Best Practices for .NET Apps](https://learn.microsoft.com/en-us/dotnet/architecture/cloud-native/)
- [Play With Docker (Free Lab)](https://labs.play-with-docker.com/)

---

**Practical Exercise:**

1. Clone a sample ASP.NET Core app from GitHub.
2. Write a multi-stage Dockerfile.
3. Build and run your app in Docker.
4. Access the app in your browser.
5. Add a database (e.g., Postgres) using Docker Compose.
6. Persist data using Docker volumes.
7. Push your image to Docker Hub.

---

*Prepared for first-time .NET developer interview candidates. This guide covers conceptual, practical, and best-practice aspects of Docker in .NET development environments.*