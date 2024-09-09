CREATE table roles (
	id INT IDENTITY PRIMARY KEY,
	nombre VARCHAR(45) NOT NULL,
);

CREATE TABLE usuario_has_roles (
	id INT IDENTITY PRIMARY KEY,
	usuario_id INT,
	roles_id INT
);

CREATE TABLE usuarios (
	id INT IDENTITY PRIMARY KEY,
	nombre VARCHAR(45) NOT NULL,
	correo VARCHAR(45) NOT NULL,
	contrasena VARCHAR(45) NOT NULL,
);

CREATE TABLE permisos (
	id INT IDENTITY PRIMARY KEY,
	nombre VARCHAR(45) NOT NULL,
	estado_id INT,
	roles_id INT
);

CREATE TABLE bitacora (
	id INT IDENTITY PRIMARY KEY,
	usuario_id INT,
	login_status VARCHAR(8)
);

ALTER TABLE usuario_has_roles ADD FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE CASCADE;
ALTER TABLE usuario_has_roles ADD FOREIGN KEY (roles_id) REFERENCES roles(id) ON DELETE CASCADE;
ALTER TABLE permisos ADD FOREIGN KEY (estado_id) REFERENCES usuario_has_roles(id) ON DELETE CASCADE;
ALTER TABLE permisos ADD FOREIGN KEY (roles_id) REFERENCES roles(id);
ALTER TABLE bitacora ADD FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE CASCADE;

INSERT INTO roles (nombre) VALUES('administrador');
INSERT INTO usuarios (nombre, correo, contrasena) VALUES ('Luis', 'luis@email.com', 'password');
INSERT INTO usuario_has_roles (usuario_id, roles_id) VALUES (1, 1);
INSERT INTO permisos (nombre, estado_id, roles_id) VALUES ('iniciar session', 1, 1);

CREATE PROCEDURE get_permisos_usuarios
AS
SELECT DISTINCT u.id AS usuario_id, u.nombre AS usuario, r.nombre AS rol, p.nombre AS permiso
FROM usuario_has_roles as uhr
JOIN usuarios u ON u.id = uhr.usuario_id
JOIN roles r ON r.id = uhr.roles_id 
JOIN permisos p ON p.nombre = p.nombre
