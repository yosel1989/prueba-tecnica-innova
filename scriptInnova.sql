/**

CREATE DATABASE "EscuelaXYZ";
USE "EscuelaXYZ";


**/

/**** Tabla Genero ***/
DROP TABLE IF EXISTS Genero;
CREATE TABLE Genero(
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Nombre VARCHAR(50) NOT NULL
);
GO
/**** Registros  Genero ***/
INSERT INTO Genero VALUES ('Masculino'), ('Femenino');
GO





/**** Tabla TipoDocumento ***/
DROP TABLE IF EXISTS TipoDocumento;
CREATE TABLE TipoDocumento(
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Nombre VARCHAR(50) NOT NULL
);
GO
/**** Registros  TipoDocumento ***/
INSERT INTO TipoDocumento VALUES ('DNI'), ('Pasaporte'), ('Otros');





/**** Tabla Alumno ****/
DROP TABLE IF EXISTS Alumno;
CREATE TABLE Alumno(
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Nombre VARCHAR(250) NOT NULL,
	Apellido VARCHAR(250) NOT NULL,
	FechaNacimiento DATE NOT NULL,
	IdGenero INT NOT NULL,
	IdTipoDocumento INT NOT NULL,
	NumeroDocumento VARCHAR(50) NOT NULL UNIQUE,
	CONSTRAINT pk_genero FOREIGN KEY (IdGenero) REFERENCES Genero(Id),
	CONSTRAINT pk_tipoDocumento FOREIGN KEY (IdTipoDocumento) REFERENCES TipoDocumento(Id)
);
GO
/**** Registros  Alumno ***/
INSERT INTO Alumno VALUES 
('Pepe Lucho 1', 'Alvarado Gutierres', '1989-04-03', 1, 1, '45846461'), 
('Pepe Lucho 2', 'Paz Sanchez', '1989-04-03', 1, 1, '45846462'), 
('Pepe Lucho 3', 'Gavilan Diaz', '1989-04-03', 1, 1, '45846463');







/**** Tabla Aula ***/
DROP TABLE IF EXISTS Aula;
CREATE TABLE Aula(
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Nombre VARCHAR(50) NOT NULL,
	Aforo INT NOT NULL DEFAULT 5,
	IdEstado BIT NOT NULL DEFAULT 1
);
GO
/**** Registros  Aula ***/
INSERT INTO Aula(Nombre) VALUES ('Matematica'), ('Fisica'), ('Religion');





/**** Tabla Docente ****/
DROP TABLE IF EXISTS Docente;
CREATE TABLE Docente(
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Nombre VARCHAR(250) NOT NULL,
	Apellido VARCHAR(250) NOT NULL,
	FechaNacimiento DATE NOT NULL,
	IdGenero INT NOT NULL,
	IdTipoDocumento INT NOT NULL,
	NumeroDocumento VARCHAR(50) NOT NULL UNIQUE,
	CONSTRAINT pk_docenteGenero FOREIGN KEY (IdGenero) REFERENCES Genero(Id),
	CONSTRAINT pk_docenteTipoDocumento FOREIGN KEY (IdTipoDocumento) REFERENCES TipoDocumento(Id)
);
GO
/**** Registros  Alumno ***/
INSERT INTO Docente VALUES 
('Docente 1', 'Alvarado Gutierres', '1989-04-03', 1, 1, '45846466'), 
('Docente 2', 'Paz Sanchez', '1989-04-03', 1, 1, '45846467'), 
('Docente 3', 'Gavilan Diaz', '1989-04-03', 1, 3, '45846469');

GO


/*** Tabla relacion Docente Aula ***/
DROP TABLE IF EXISTS DocenteAula;
CREATE TABLE DocenteAula(
	IdDocente INT NOT NULL,
	IdAula INT NOT NULL,
	CONSTRAINT pk_docenteAula_Docente FOREIGN KEY (IdDocente) REFERENCES Docente(Id),
	CONSTRAINT pk_docenteAula_Aula FOREIGN KEY (IdAula) REFERENCES Aula(Id),
	CONSTRAINT pk_docenteAula_Id PRIMARY KEY (IdDocente, IdAula)
);
/**** Registros  Alumno ***/
INSERT INTO DocenteAula VALUES 
(1,1), 
(2,2), 
(3,3);
GO


/*** Tabla relacion Docente Aula ***/
DROP TABLE IF EXISTS AlumnoAula;
CREATE TABLE AlumnoAula(
	IdAlumno INT NOT NULL,
	IdAula INT NOT NULL,
	CONSTRAINT pk_alumnoAula_Alumno FOREIGN KEY (IdAlumno) REFERENCES Alumno(Id),
	CONSTRAINT pk_alumnoAula_Aula FOREIGN KEY (IdAula) REFERENCES Aula(Id)
);
/**** Registros  Alumno ***/
INSERT INTO AlumnoAula VALUES 
(1,1), 
(2,1), 
(3,1);
go




/*** PROCEDIMIENTO LISTAR ALUMNOS ***/
CREATE OR ALTER PROCEDURE SP_Alumno_Listar
AS
BEGIN
	SELECT 
	AL.*,
	g.Nombre AS Genero,
	td.Nombre AS TipoDocumento
	FROM Alumno AL
	INNER JOIN Genero G ON AL.IdGenero = G.Id
	INNER JOIN TipoDocumento TD ON AL.IdTipoDocumento = TD.Id;
END;
GO
-- EXEC SP_Alumno_Listar





/**** PROCEDIMIENTO CREAR ALUMNO***/
CREATE OR ALTER PROCEDURE SP_Alumno_Crear
	@pNombre VARCHAR(250),
	@pApellido VARCHAR(250),
	@pIdGenero INT,
	@pIdTipoDocumento INT,
	@pFechaNacimiento DATE,
	@pNumeroDocumento VARCHAR(50)
AS
BEGIN
	DECLARE @vFechaRegistra DATETIME = GETDATE();
	DECLARE @Id INT = 0;
	DECLARE @ErrorNumero INT = 0, @ErrorDetalle VARCHAR(MAX) = ''
	DECLARE @vMensajeError VARCHAR(MAX) = 'Ocurrio un error al intentar registrar el Alumno'

	BEGIN TRY
		BEGIN TRANSACTION

		INSERT INTO Alumno(Nombre, Apellido, FechaNacimiento, IdGenero, IdTipoDocumento, NumeroDocumento) VALUES(
			@pNombre,
			@pApellido,
			@pFechaNacimiento,
			@pIdGenero,
			@pIdTipoDocumento,
			@pNumeroDocumento
		);

		SET @Id = @@IDENTITY;

						  
		COMMIT TRAN
	END TRY
	BEGIN CATCH
		SELECT @ErrorNumero = ERROR_NUMBER(), @ErrorDetalle = ERROR_MESSAGE()
		GOTO Error_Rollback
	END CATCH

	Error_Rollback:
	IF @@TRANCOUNT > 0
	BEGIN
		SELECT 
			'Error' = 1, 
			'ErrorNumero' = @ErrorNumero,
			'ErrorDetalle' = @ErrorDetalle,
			'Mensaje' = @vMensajeError,
			'IdVenta' = 0;
		ROLLBACK TRAN
	END
	ELSE
	BEGIN
		SELECT 
			AL.*,
			g.Nombre AS Genero,
			td.Nombre AS TipoDocumento
			FROM Alumno AL
			INNER JOIN Genero G ON AL.IdGenero = G.Id
			INNER JOIN TipoDocumento TD ON AL.IdTipoDocumento = TD.Id
		WHERE AL.Id = @Id;
	END
END;
GO










/**** PROCEDIMIENTO MODIFICAR ALUMNO***/
CREATE OR ALTER PROCEDURE SP_Alumno_Modificar
	@pId INT,
	@pNombre VARCHAR(250),
	@pApellido VARCHAR(250),
	@pIdGenero INT,
	@pIdTipoDocumento INT,
	@pFechaNacimiento DATE,
	@pNumeroDocumento VARCHAR(50)
AS
BEGIN
	DECLARE @vFechaRegistra DATETIME = GETDATE();
	DECLARE @ErrorNumero INT = 0, @ErrorDetalle VARCHAR(MAX) = ''
	DECLARE @vMensajeError VARCHAR(MAX) = 'Ocurrio un error al intentar modificar el Alumno'

	BEGIN TRY
		BEGIN TRANSACTION

		UPDATE Alumno SET 
			Nombre = @pNombre, 
			Apellido = @pApellido, 
			FechaNacimiento = @pFechaNacimiento, 
			IdGenero = @pIdGenero, 
			IdTipoDocumento = @pIdTipoDocumento, 
			NumeroDocumento = @pNumeroDocumento
		WHERE Id = @pId;

						  
		COMMIT TRAN
	END TRY
	BEGIN CATCH
		SELECT @ErrorNumero = ERROR_NUMBER(), @ErrorDetalle = ERROR_MESSAGE()
		GOTO Error_Rollback
	END CATCH

	Error_Rollback:
	IF @@TRANCOUNT > 0
	BEGIN
		SELECT 
			'Error' = 1, 
			'ErrorNumero' = @ErrorNumero,
			'ErrorDetalle' = @ErrorDetalle,
			'Mensaje' = @vMensajeError,
			'IdVenta' = 0;
		ROLLBACK TRAN
	END
	ELSE
	BEGIN
		SELECT 
			AL.*,
			g.Nombre AS Genero,
			td.Nombre AS TipoDocumento
			FROM Alumno AL
			INNER JOIN Genero G ON AL.IdGenero = G.Id
			INNER JOIN TipoDocumento TD ON AL.IdTipoDocumento = TD.Id
		WHERE AL.Id = @pId;
	END
END;
GO







/**** PROCEDIMIENTO ELIMINAR ALUMNO ***/
CREATE OR ALTER PROCEDURE SP_Alumno_Eliminar
	@pId INT
AS
BEGIN
	DECLARE @vFechaRegistra DATETIME = GETDATE();
	DECLARE @ErrorNumero INT = 0, @ErrorDetalle VARCHAR(MAX) = ''
	DECLARE @vMensajeError VARCHAR(MAX) = 'Ocurrio un error al intentar eliminar el Alumno'

	BEGIN TRY
		BEGIN TRANSACTION


		DELETE AlumnoAula WHERE IdAlumno = @pId;
		DELETE Alumno WHERE Id = @pId;

						  
		COMMIT TRAN
	END TRY
	BEGIN CATCH
		SELECT @ErrorNumero = ERROR_NUMBER(), @ErrorDetalle = ERROR_MESSAGE()
		GOTO Error_Rollback
	END CATCH

	Error_Rollback:
	IF @@TRANCOUNT > 0
	BEGIN
		SELECT 
			'Error' = 1, 
			'ErrorNumero' = @ErrorNumero,
			'ErrorDetalle' = @ErrorDetalle,
			'Mensaje' = @vMensajeError,
			'IdVenta' = 0;
		ROLLBACK TRAN
	END
	ELSE
	BEGIN
		SELECT Exito = 1;
	END
END;
GO





/*** PROCEDIMIENTO LISTAR ALUMNOS POR AULA***/
CREATE OR ALTER PROCEDURE SP_Alumno_ListarByAula
	@pIdAula INT
AS
BEGIN
	SELECT 
	AL.*,
	g.Nombre AS Genero,
	td.Nombre AS TipoDocumento
	FROM Alumno AL
	INNER JOIN AlumnoAula AA on AL.Id = AA.IdAlumno
	INNER JOIN Genero G ON AL.IdGenero = G.Id
	INNER JOIN TipoDocumento TD ON AL.IdTipoDocumento = TD.Id
	WHERE AA.IdAula = @pIdAula;
END;
GO
-- EXEC SP_Alumno_ListarByAula 1




/*** PROCEDIMIENTO LISTAR ALUMNOS POR AULA DEL Docente***/
CREATE OR ALTER PROCEDURE SP_Alumno_ListarByAulaAndDocente
	@pIdAula INT,
	@pIdDocente INT
AS
BEGIN
	SELECT 
	AL.*,
	g.Nombre AS Genero,
	td.Nombre AS TipoDocumento
	FROM Alumno AL
	INNER JOIN AlumnoAula AA on AL.Id = AA.IdAlumno
	INNER JOIN Aula A on AA.IdAula = A.Id
	INNER JOIN DocenteAula DA ON A.Id = DA.IdAula
	INNER JOIN Genero G ON AL.IdGenero = G.Id
	INNER JOIN TipoDocumento TD ON AL.IdTipoDocumento = TD.Id
	WHERE AA.IdAula = @pIdAula AND DA.IdDocente = @pIdDocente;
END;
GO
-- EXEC SP_Alumno_ListarByAulaAndDocente 1,2









/**** PROCEDIMIENTO CREAR ALUMNO***/
CREATE OR ALTER PROCEDURE SP_Alumno_AsignarAula
	@pIdAlumno INT,
	@pIdAula INT
AS
BEGIN
	DECLARE @vFechaRegistra DATETIME = GETDATE();
	DECLARE @AforoActual INT = 0;
	DECLARE @ErrorNumero INT = 0, @ErrorDetalle VARCHAR(MAX) = ''
	DECLARE @vMensajeError VARCHAR(MAX) = 'Ocurrio un error al intentar asignar el alumno al aula'

	BEGIN TRY
		BEGIN TRANSACTION

		SET @AforoActual = (SELECT COUNT(*) FROM AlumnoAula WHERE IdAula = @pIdAula);

		-- Verificar si el alumno existe
		IF (SELECT COUNT(*) FROM Alumno WHERE Id = @pIdAlumno) = 0
		BEGIN
			SET @vMensajeError = 'El alumno ya se encuentra registrado en la base de datos'
			GOTO Error_Rollback
		END
		-- Verificar si ya esta asignado a la aula
		IF (SELECT COUNT(*) FROM AlumnoAula WHERE IdAlumno = @pIdAlumno AND IdAula = @pIdAula) > 0
		BEGIN
			SET @vMensajeError = 'El alumno ya se encuentra asignado en el autla'
			GOTO Error_Rollback
		END
		-- select * from AlumnoAula

		-- Verificar AFORO
		IF @AforoActual >= (SELECT Aforo FROM Aula WHERE Id = @pIdAula)
		BEGIN
			SET @vMensajeError = 'El aula llego a su aforo maximo'
			GOTO Error_Rollback
		END


		INSERT INTO AlumnoAula VALUES(@pIdAlumno, @pIdAula);
						  
		COMMIT TRAN
	END TRY
	BEGIN CATCH
		SELECT @ErrorNumero = ERROR_NUMBER(), @ErrorDetalle = ERROR_MESSAGE()
		GOTO Error_Rollback
	END CATCH

	Error_Rollback:
	IF @@TRANCOUNT > 0
	BEGIN
		SELECT
			'Error' = 1,
			'ErrorNumero' = @ErrorNumero,
			'ErrorDetalle' = @ErrorDetalle,
			'Mensaje' = @vMensajeError,
			'IdVenta' = 0;
		ROLLBACK TRAN
	END
	ELSE
	BEGIN
		SELECT Exito = 1;
	END
END;
GO
-- EXEC SP_Alumno_AsignarAula 4, 1



--select * from Alumno







/**** PROCEDIMIENTO ELIMINAR ALUMNO DEL AULA ***/
CREATE OR ALTER PROCEDURE SP_Alumno_EliminarAula
	@pIdAlumno INT,
	@pIdAula INT
AS
BEGIN
	DECLARE @vFechaRegistra DATETIME = GETDATE();
	DECLARE @ErrorNumero INT = 0, @ErrorDetalle VARCHAR(MAX) = ''
	DECLARE @vMensajeError VARCHAR(MAX) = 'Ocurrio un error al intentar eliminar el Alumno del aula'

	BEGIN TRY
		BEGIN TRANSACTION


		DELETE AlumnoAula WHERE IdAlumno = @pIdAlumno AND IdAula = @pIdAula;

						  
		COMMIT TRAN
	END TRY
	BEGIN CATCH
		SELECT @ErrorNumero = ERROR_NUMBER(), @ErrorDetalle = ERROR_MESSAGE()
		GOTO Error_Rollback
	END CATCH

	Error_Rollback:
	IF @@TRANCOUNT > 0
	BEGIN
		SELECT 
			'Error' = 1, 
			'ErrorNumero' = @ErrorNumero,
			'ErrorDetalle' = @ErrorDetalle,
			'Mensaje' = @vMensajeError,
			'IdVenta' = 0;
		ROLLBACK TRAN
	END
	ELSE
	BEGIN
		SELECT Exito = 1;
	END
END;
GO
