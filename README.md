# OEED_ITT - Sistema de Gestión de Eventos Institucionales

Este proyecto fue desarrollado como proyecto escolar, nombrado **Organizador Escolar de Eventos Digital** del **Instituto Tecnológico de Tepic**. Es una aplicación web robusta que permite gestionar el ciclo de vida completo de eventos académicos y administrativos, desde la planeación hasta el registro de asistencia y retroalimentación.

> [!NOTE]
> **Nota de Contexto:** Este fue mi primer acercamiento formal a la programación web y al framework .NET. Representa mis bases en la ingeniería de software y el manejo de arquitecturas MVC. Actualmente, sigo mejorando mis prácticas y acepto cualquier crítica constructiva sobre el código.

---

## 🚀 Características principales

* **Gestión de Usuarios:** Sistema de autenticación y autorización basado en roles (Administradores, Organizadores, Staff y Alumnos).
* **Control de Eventos:** CRUD completo de eventos institucionales, incluyendo asignación de lugares y departamentos.
* **Inscripciones y Asistencia:** Módulo para que los usuarios se inscriban y el staff valide la asistencia en tiempo real.
* **Feedback:** Sistema de comentarios para evaluar la calidad de los eventos realizados.
* **Seguridad de Datos:** Implementación de variables de entorno para proteger credenciales sensibles.

## 🛠️ Tecnologías utilizadas

* **Backend:** .NET 8 (ASP.NET Core MVC)
* **Persistencia:** Entity Framework Core con SQL Server
* **Frontend:** Razor Pages, HTML5, CSS3, Bootstrap y jQuery
* **Configuración:** DotNetEnv para el manejo de variables de entorno (`.env`)

## 📦 Instalación y Configuración

1.  **Clonar el repositorio:**
    ```bash
    git clone [https://github.com/yael-cc/OEED_ITT.git])
    ```

2.  **Configurar variables de entorno:**
    Crea un archivo `.env` en la raíz del proyecto con tu cadena de conexión:
    ```env
    CADENA_SQL="Server=TU_SERVIDOR;Database=OEED_DB;User Id=TU_USER;Password=TU_PASSWORD;TrustServerCertificate=True;"
    ```

3.  **Restaurar paquetes y ejecutar:**
    ```bash
    dotnet restore
    dotnet run
    ```

---

## 📈 Áreas de Mejora (Roadmap Personal)
Al ser un proyecto de aprendizaje inicial, tengo identificadas las siguientes mejoras para futuras versiones o proyectos:
* Migrar la lógica de negocio de los controladores a una capa de Servicios.
* Implementar el patrón Repository para desacoplar el DbContext.
* Mejorar la validación de formularios en el cliente.

---
**Desarrollado por Cuevas Cruz Luis Angel Yael** - Estudiante egresado de Ingeniería en Sistemas Computacionales.