# Inventario Castores - Evaluación Técnica

## Tecnologías utilizadas
- IDE: Visual Studio Code
- Lenguaje: Java 24.0.2
- Framework: Spring Boot 3.5.4
- DBMS: MySQL Ver 8.0.42
- BuildTool: Apache Maven 3.8.7

## Estructura de carpetas
- `SCRIPTS`: Archivos complementarios para la prueba técnica
- `src/main/resources/templates`: Vistas
- `src/main/java/com/grupocastores/inventario_castores/model`: Modelos
- `src/main/java/com/grupocastores/inventario_castores/controller`: Controladores

## Cómo ejecutar

1. Importar el proyecto con la herramienta git clone.
2. Asegurarse de tener instaladas las tecnologías utilizadas mencionadas anteriormente.
3. Crear base de datos MySQL con el script en `/SCRIPTS/inventarioGrupoCastores.sql` donde hay instrucciones para ello.
3. Verificar la correcta conexión en archivo application.properties.
4. Ejecutar el comando `mvn spring-boot:run` en el directorio `/inventario-castores`.
5. Abrir en un navegador la URL `localhost.7070/`.