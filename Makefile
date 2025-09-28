# Makefile para Windows - PerlaMetro

# -------------------------------------
# Colores
# -------------------------------------
GREEN := \033[0;32m
YELLOW := \033[0;33m
NC := \033[0m

# -------------------------------------
# Variables
# -------------------------------------
HOST_PATH := .

# -------------------------------------
# Setup completo del proyecto
# -------------------------------------
setup:
	@echo "$(GREEN)Setting up project...$(NC)"
	@echo "$(YELLOW)1. Restaurando paquetes NuGet...$(NC)"
	@dotnet restore
	@echo "$(YELLOW)2. Instalando herramientas (dotnet-ef)...$(NC)"
	@dotnet tool install --global dotnet-ef || echo "$(YELLOW)EF Core tools ya instaladas$(NC)"
	@echo "$(YELLOW)3. Construyendo proyecto...$(NC)"
	@dotnet build
	@echo "$(GREEN)Setup completo!$(NC)"
	@echo "$(YELLOW)Usa 'make run' para iniciar la aplicación$(NC)"

# -------------------------------------
# Restaurar paquetes
# -------------------------------------
restore:
	@echo "$(GREEN)Restaurando paquetes NuGet...$(NC)"
	@dotnet restore
	@echo "$(GREEN)Paquetes restaurados$(NC)"

# -------------------------------------
# Construir proyecto
# -------------------------------------
build:
	@echo "$(GREEN)Construyendo proyecto...$(NC)"
	@dotnet build
	@echo "$(GREEN)Build completado$(NC)"

# -------------------------------------
# Levantar PostgreSQL en Docker
# -------------------------------------
db-up:
	@echo "$(GREEN)Iniciando contenedor PostgreSQL...$(NC)"
	@docker run --name perla_postgres \
	-e POSTGRES_USER=postgres \
	-e POSTGRES_PASSWORD=pass123 \
	-e POSTGRES_DB=api-perla \
	-p 5433:5432 -d postgres:15 || \
	(echo "$(YELLOW)Contenedor ya existe, iniciando...$(NC)" && docker start perla_postgres)
	@echo "$(YELLOW)Esperando a que PostgreSQL esté listo...$(NC)"
	@powershell -Command "Start-Sleep -Seconds 10"
	@echo "$(GREEN)PostgreSQL listo$(NC)"

# -------------------------------------
# Detener PostgreSQL
# -------------------------------------
db-down:
	@echo "$(GREEN)Deteniendo contenedor PostgreSQL...$(NC)"
	@docker stop perla_postgres 2>/dev/null || echo "$(YELLOW)Contenedor no estaba corriendo$(NC)"
	@echo "$(GREEN)PostgreSQL detenido$(NC)"

# -------------------------------------
# Ejecutar aplicación
# -------------------------------------
run:
	@echo "$(GREEN)Iniciando aplicación...$(NC)"
	@cd $(HOST_PATH) && dotnet watch run
