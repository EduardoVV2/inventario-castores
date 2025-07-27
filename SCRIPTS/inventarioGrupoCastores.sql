CREATE DATABASE IF NOT EXISTS inventario_castores;

-- Crear un usuario "usuario_castores" con contraseña "admincastores" y darle todos los privilegios
CREATE USER 'usuario_castores'@'localhost' IDENTIFIED BY 'admincastores';

GRANT ALL PRIVILEGES ON inventario_castores.* TO 'usuario_castores'@'localhost';
FLUSH PRIVILEGES;

USE inventario_castores;

CREATE TABLE rolesUsuarios (
    idRol INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(13) NOT NULL,
    activo BOOLEAN DEFAULT TRUE
);

CREATE TABLE usuarios (
    idUsuario INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    correo VARCHAR(50) NOT NULL UNIQUE,
    contrasena VARCHAR(255) NOT NULL,
    idRol INT,
    FOREIGN KEY (idRol) REFERENCES rolesUsuarios(idRol)
);

CREATE TABLE productos (
    idProducto INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    descripcion TEXT,
    cantidad INT DEFAULT 0,
    activo BOOLEAN DEFAULT TRUE
);

CREATE TABLE movimientos (
    idMovimiento INT AUTO_INCREMENT PRIMARY KEY,
    idProducto INT,
    idUsuario INT,
    tipo ENUM('entrada', 'salida') NOT NULL,
    cantidad INT NOT NULL,
    fechaHora DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (idProducto) REFERENCES productos(idProducto),
    FOREIGN KEY (idUsuario) REFERENCES usuarios(idUsuario)
);

-- Agregar los roles
INSERT INTO rolesUsuarios (nombre) VALUES ("Administrador"), ("Almacenista");

-- Agregar dos usuarios para pruebas
INSERT INTO usuarios (nombre, correo, contrasena, idRol) VALUES ("Juan Manuel Perez", "juanperez@castores.com", "grupo%Castores1", 1), ("Mariana Rodriguez", "marianita@castores.com", "grupo%Castores1", 2);