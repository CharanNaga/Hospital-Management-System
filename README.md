Architecture: Microservices
Deployment: Azure Kubernetes Service (AKS)
Containerization: Docker
Authentication: JWT
Pattern: Repository + Dependency Injection
Cloud: Azure
AI Layer: Rule-based Diet Recommendation Engine


🏥 AI-Powered Hospital Management System

Cloud-Native Microservices Architecture using ASP.NET Core, Docker & AKS

📌 Project Overview

This project is a cloud-native microservices-based Hospital Management System built using ASP.NET Core 8, deployed on Azure Kubernetes Service (AKS), and containerized with Docker.

The system manages:

Patient Registration

Doctor Management

Appointment Scheduling

Bed Allocation

Discharge Processing

AI-based Diet Recommendation Engine

It demonstrates enterprise-level architecture patterns including JWT Authentication, Repository Pattern, Database-per-Service, Health Checks, and Kubernetes Deployment.

🏗 Architecture

Microservices Architecture

Database per service pattern

RESTful APIs

JWT-based Authentication

Docker Multi-stage builds

Azure Container Registry (ACR)

Azure Kubernetes Service (AKS)

Kubernetes Ingress

Horizontal Pod Autoscaling

Health Checks

🧱 Microservices Overview
| Service            | Responsibility                              | Database      |
| ------------------ | ------------------------------------------- | ------------- |
| PatientService     | Patient registration & records              | PatientDB     |
| DoctorService      | Doctor details management                   | DoctorDB      |
| AppointmentService | Appointment scheduling                      | AppointmentDB |
| BedService         | Bed allocation & availability               | BedDB         |
| DischargeService   | Discharge workflow & AI diet recommendation | DischargeDB   |
| AuthService        | JWT authentication & user management        | AuthDB        |

![Architecture](image.png)

🛠 Tech Stack
Backend

ASP.NET Core 8 Web API

Entity Framework Core

SQL Server

Security

JWT Authentication

Role-based Authorization

DevOps & Cloud

Docker

Azure Container Registry (ACR)

Azure Kubernetes Service (AKS)

Kubernetes Ingress Controller

kubectl

Azure CLI


🧠 AI Integration

The DischargeService includes an AI-based rule engine that:

Analyzes diagnosis

Evaluates patient age

Recommends personalized diet plans

Example:

Cardiac patient → Low Sodium Diet

Diabetic patient → Sugar-Controlled Diet

Pediatric patient → High Protein Diet

Architecture is AI-ready and can be extended to integrate:

Azure OpenAI

ML.NET

External ML APIs

🚀 How to Run Locally

1️⃣ Clone Repository
git clone https://github.com/your-username/hospital-management-system.git
cd hospital-management-system

2️⃣ Update Connection Strings

Modify appsettings.json in each service:
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=PatientDB;Trusted_Connection=True;TrustServerCertificate=True;"
}

3️⃣ Run Each Service
dotnet run

Swagger will be available at:
https://localhost:{port}/swagger

🐳 Docker Build

Build Docker image:
docker build -t patientservice .
docker run -p 5001:80 patientservice

☁ Azure Deployment
Step 1 – Create ACR
az acr create --resource-group rg-hospital --name hospitalacr --sku Basic

Step 2 – Push Images
docker tag patientservice hospitalacr.azurecr.io/patientservice:v1
docker push hospitalacr.azurecr.io/patientservice:v1

Step 3 – Create AKS Cluster
az aks create \
  --resource-group rg-hospital \
  --name hospital-aks \
  --node-count 2 \
  --enable-addons monitoring \
  --generate-ssh-keys

  Step 4 – Deploy to AKS
  kubectl apply -f k8s/patient-deployment.yaml
  kubectl apply -f k8s/patient-service.yaml

  📊 Kubernetes Architecture

Deployment per microservice

ClusterIP services

Ingress for routing

Horizontal Pod Autoscaler

Health checks enabled

Example:
kubectl get pods
kubectl get services
kubectl get ingress

📈 Scalability

Supports:

Horizontal scaling

Independent service deployment

Fault isolation

Zero-downtime rolling updates

🔍 Health Monitoring

Each service exposes:
/health
Used by Kubernetes liveness & readiness probes.

🎯 Key Concepts Demonstrated

Clean Architecture

Repository Pattern

Dependency Injection

Microservices

Containerization

Kubernetes Orchestration

Cloud Deployment Strategy

AI Extension Layer

Production-Ready Patterns


🧑‍💻 Author

Bondili Charan Singh
Cloud & .NET Developer
India

📜 License

This project is for educational and portfolio purposes.