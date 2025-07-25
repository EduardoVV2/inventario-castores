CREATE DATABASE IF NOT EXISTS inventario_castores;

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
    fecha_hora DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (idProducto) REFERENCES productos(idProducto),
    FOREIGN KEY (idUsuario) REFERENCES usuarios(idUsuario)
);

INSERT INTO rolesUsuarios (nombre) VALUES "Administrador", "Almacenista";