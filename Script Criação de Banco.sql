--CREATE DATABASE Crm
--USE Crm
--ALTER AUTHORIZATION ON DATABASE::Crm TO sa

CREATE TABLE Customer
	(
		IdCustomer INT IDENTITY NOT NULL,
		Name VARCHAR(100) NOT NULL,
		DocumentNumber VARCHAR(20) NOT NULL,
		BirthDate DATETIME NOT NULL,
		Gender CHAR(1) NOT NULL,
		CustomerDate DATETIME NOT NULL
	)

ALTER TABLE Customer ADD CONSTRAINT PK_Customer PRIMARY KEY(IdCustomer)
ALTER TABLE Customer ADD CONSTRAINT CH_GenderCustomer CHECK (Gender IN ('M','F','I'))
ALTER TABLE Customer ADD CONSTRAINT CH_BirthDateCustomer CHECK (BirthDate < GETDATE())
ALTER TABLE Customer ADD CONSTRAINT UN_DocumentNumberCustomer UNIQUE (DocumentNumber)

CREATE TABLE Address
	(
		IdAddress INT IDENTITY NOT NULL,
		IdCustomer INT NOT NULL,
		PostalCode VARCHAR(20),
		PublicPlace VARCHAR(200) NOT NULL,
		Number VARCHAR(20) NOT NULL,
		Neighborhood VARCHAR(100) NOT NULL,
		Complement VARCHAR(200),
		City VARCHAR(100) NOT NULL,
		FS CHAR(2) NOT NULL,
		AddressDate DATETIME NOT NULL
	)

ALTER TABLE Address ADD CONSTRAINT PK_Address PRIMARY KEY(IdAddress)
ALTER TABLE Address ADD CONSTRAINT FK_Address_Customer FOREIGN KEY(IdCustomer) REFERENCES Customer(IdCustomer)

CREATE TABLE Telephone
	(
		IdTelephone INT IDENTITY NOT NULL,
		IdCustomer INT NOT NULL,
		PhoneNumber VARCHAR(11) NOT NULL,
		Status BIT NOT NULL
	)

ALTER TABLE Telephone ADD CONSTRAINT PK_Telephone PRIMARY KEY(IdTelephone)
ALTER TABLE Telephone ADD CONSTRAINT FK_Telephone_Customer FOREIGN KEY(IdCustomer) REFERENCES Customer(IdCustomer)

CREATE TABLE SubjectGroup
	(
		IdSubjectGroup INT IDENTITY NOT NULL,
		Name VARCHAR(50) NOT NULL
	)

ALTER TABLE SubjectGroup ADD CONSTRAINT PK_SubjectGroup PRIMARY KEY(IdSubjectGroup)

CREATE TABLE SubjectSubGroup
	(
		IdSubjectSubGroup INT NOT NULL,
		IdSubjectGroup INT NOT NULL,
		Name VARCHAR(50) NOT NULL
	)

ALTER TABLE SubjectSubGroup ADD CONSTRAINT PK_SubjectSubGroup PRIMARY KEY (IdSubjectSubGroup, IdSubjectGroup)
ALTER TABLE SubjectSubGroup ADD CONSTRAINT FK_SubjectSubGroup_SubjectGroup FOREIGN KEY(IdSubjectGroup) REFERENCES SubjectGroup(IdSubjectGroup)

CREATE TABLE SubjectDetail
	(
		IdSubjectDetail INT NOT NULL,
		IdSubjectSubGroup INT NOT NULL,
		IdSubjectGroup INT NOT NULL,
		Name VARCHAR(100) NOT NULL
	)

ALTER TABLE SubjectDetail ADD CONSTRAINT PK_SubjectDetail PRIMARY KEY(IdSubjectGroup, IdSubjectSubGroup, IdSubjectDetail)
ALTER TABLE SubjectDetail ADD CONSTRAINT FK_SubjectDetail_SubjectSubGroup FOREIGN KEY(IdSubjectSubGroup, IdSubjectGroup) REFERENCES SubjectSubGroup(IdSubjectSubGroup, IdSubjectGroup)

CREATE TABLE Agent
	(
		IdAgent INT IDENTITY NOT NULL,
		Name VARCHAR(100) NOT NULL,
		Login VARCHAR(30),
		Password VARCHAR(255),
		Status BIT NOT NULL
	)

ALTER TABLE Agent ADD CONSTRAINT PK_Agent PRIMARY KEY(IdAgent)

CREATE TABLE Contact
	(
		IdContact INT IDENTITY NOT NULL,
		IdCustomer INT NOT NULL,
		IdAgent INT NOT NULL,
		ContactDate DATETIME NOT NULL,
		Detail TEXT NOT NULL
	)

ALTER TABLE Contact ADD CONSTRAINT PK_Contact PRIMARY KEY(IdContact)
ALTER TABLE Contact ADD CONSTRAINT FK_Contact_Customer FOREIGN KEY(IdCustomer) REFERENCES Customer(IdCustomer)
ALTER TABLE Contact ADD CONSTRAINT FK_Contact_Agent FOREIGN KEY(IdAgent) REFERENCES Agent(IdAgent)

CREATE TABLE OccurrenceStatus
	(
		IdOccurrenceStatus INT IDENTITY NOT NULL,
		Name VARCHAR(50) NOT NULL
	)

ALTER TABLE OccurrenceStatus ADD CONSTRAINT PK_OccurrenceStatus PRIMARY KEY(IdOccurrenceStatus)

CREATE TABLE Occurrence
	(
		IdOccurrence INT IDENTITY NOT NULL,
		IdCustomer INT NOT NULL,
		IdAgent INT NOT NULL,
		OccurrenceDate DATETIME NOT NULL,
		IdSubjectGroup INT NOT NULL,
		IdSubjectSubGroup INT NOT NULL,
		IdSubjectDetail INT NOT NULL,
		IdOccurrenceStatus INT NOT NULL
	)

ALTER TABLE Occurrence ADD CONSTRAINT PK_Occurrence PRIMARY KEY(IdOccurrence)
ALTER TABLE Occurrence ADD CONSTRAINT FK_Occurrence_Customer FOREIGN KEY(IdCustomer) REFERENCES Customer(IdCustomer)
ALTER TABLE Occurrence ADD CONSTRAINT FK_Occurrence_Agent FOREIGN KEY(IdAgent) REFERENCES Agent(IdAgent)
ALTER TABLE Occurrence ADD CONSTRAINT FK_Occurrence_SubjectDetail FOREIGN KEY(IdSubjectGroup, IdSubjectSubGroup, IdSubjectDetail) REFERENCES SubjectDetail(IdSubjectGroup, IdSubjectSubGroup, IdSubjectDetail)
ALTER TABLE Occurrence ADD CONSTRAINT FK_Occurrence_OccurrenceStatus FOREIGN KEY(IdOccurrenceStatus) REFERENCES OccurrenceStatus(IdOccurrenceStatus)

CREATE TABLE OccurrenceUpdateType
	(
		IdOccurrenceUpdateType INT IDENTITY NOT NULL,
		Name VARCHAR(50) NOT NULL
	)

ALTER TABLE OccurrenceUpdateType ADD CONSTRAINT PK_OccurrenceUpdateType PRIMARY KEY(IdOccurrenceUpdateType)

CREATE TABLE OccurrenceUpdate
	(
		IdOccurrenceUpdate INT IDENTITY NOT NULL,
		IdOccurrenceUpdateType INT NOT NULL,
		IdOccurrence INT NOT NULL,
		IdAgent INT NOT NULL,
		UpdateDate DATETIME NOT NULL,
		UpdateMessage TEXT,
	)

ALTER TABLE OccurrenceUpdate ADD CONSTRAINT PK_OccurrenceUpdate PRIMARY KEY(IdOccurrenceUpdate)
ALTER TABLE OccurrenceUpdate ADD CONSTRAINT FK_OccurrenceUpdate_OccurrenceUpdateType FOREIGN KEY(IdOccurrenceUpdateType) REFERENCES OccurrenceUpdateType(IdOccurrenceUpdateType)
ALTER TABLE OccurrenceUpdate ADD CONSTRAINT FK_OccurrenceUpdate_Occurrence FOREIGN KEY(IdOccurrence) REFERENCES Occurrence(IdOccurrence)
ALTER TABLE OccurrenceUpdate ADD CONSTRAINT FK_OccurrenceUpdate_Agent FOREIGN KEY(IdAgent) REFERENCES Agent(IdAgent)

CREATE TABLE SchedulingType
	(
		IdSchedulingType INT IDENTITY NOT NULL,
		Name VARCHAR(50) NOT NULL
	)

ALTER TABLE SchedulingType ADD CONSTRAINT PK_SchedulingType PRIMARY KEY(IdSchedulingType)

CREATE TABLE Scheduling
	(
		IdScheduling INT IDENTITY NOT NULL,
		IdSchedulingType INT NOT NULL,
		IdOccurrence INT NOT NULL,
		IdAgent INT NOT NULL,
		SchedulingDate DATETIME NOT NULL,
		SchedulingDateScheduled DATETIME NOT NULL,
		SchedulingDateRealized DATETIME,
		Status BIT NOT NULL
	)

ALTER TABLE Scheduling ADD CONSTRAINT PK_Scheduling PRIMARY KEY(IdScheduling)
ALTER TABLE Scheduling ADD CONSTRAINT FK_Scheduling_SchedulingType FOREIGN KEY(IdSchedulingType) REFERENCES SchedulingType(IdSchedulingType)
ALTER TABLE Scheduling ADD CONSTRAINT FK_Scheduling_Occurrence FOREIGN KEY(IdOccurrence) REFERENCES Occurrence(IdOccurrence)
ALTER TABLE Scheduling ADD CONSTRAINT FK_Scheduling_Agent FOREIGN KEY(IdAgent) REFERENCES Agent(IdAgent)

GO
ALTER PROCEDURE sp_GravarCliente
	(
		@Nome VARCHAR(100),
		@Genero CHAR(1),
		@Documento VARCHAR(20),
		@DataNascimento DATETIME,
		@IdCliente INT = NULL
	)
AS
BEGIN
	BEGIN TRANSACTION
	BEGIN TRY
		IF @IdCliente IS NOT NULL
			BEGIN
				UPDATE
					Customer
				SET
					Name = @Nome,
					Gender = @Genero,
					DocumentNumber = @Documento,
					BirthDate = @DataNascimento
				OUTPUT
					inserted.IdCustomer
				WHERE
					IdCustomer = @IdCliente
			END
		ELSE
			BEGIN
				INSERT INTO Customer
				(
					Name,
					Gender,
					DocumentNumber,
					BirthDate,
					CustomerDate
				)
				OUTPUT
					inserted.IdCustomer
				VALUES
				(
					@Nome,
					@Genero,
					@Documento,
					@DataNascimento,
					GETDATE()
				)
			END
		
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SELECT
			ERROR_MESSAGE() AS ErrorMessage
	END CATCH
END

ALTER PROCEDURE sp_GravarEndereco
	(
		@IdCliente INT,
		@Cep VARCHAR(20),
		@Logradouro VARCHAR(200),
		@Numero VARCHAR(20),
		@Bairro VARCHAR(100),
		@Complemento VARCHAR(200) = NULL,
		@Cidade VARCHAR(100),
		@Uf CHAR(2)
	)
AS
BEGIN
	BEGIN TRANSACTION
	BEGIN TRY

	DECLARE @CONT INT
	SELECT @CONT = COUNT(0) FROM Address WITH (NOLOCK) WHERE IdCustomer = @IdCliente

	IF @CONT > 0
		BEGIN
			UPDATE
				Address
			SET
				PostalCode = @Cep,
				PublicPlace = @Logradouro,
				Number = @Numero,
				Neighborhood = @Bairro,
				Complement = @Complemento,
				City = @Cidade,
				FS = @Uf,
				AddressDate = GETDATE()
			WHERE
				IdCustomer = @IdCliente
		END
	ELSE
		BEGIN
			INSERT INTO Address
				(
					IdCustomer,
					PostalCode,
					PublicPlace,
					Number,
					Neighborhood,
					Complement,
					City,
					FS,
					AddressDate
				)
			VALUES
				(
					@IdCliente,
					@Cep,
					@Logradouro,
					@Numero,
					@Bairro,
					@Complemento,
					@Cidade,
					@Uf,
					GETDATE()
				)
		END
	COMMIT
	END TRY
	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS ErrorMessage
		ROLLBACK TRANSACTION
	END CATCH
END

CREATE PROCEDURE sp_GravarContato
	(
		@IdCliente INT,
		@IdAgente INT,
		@Detalhe VARCHAR(8000)
	)
AS
BEGIN
	BEGIN TRANSACTION
	BEGIN TRY

		INSERT INTO Contact
			(
				IdCustomer,
				IdAgent,
				ContactDate,
				Detail
			)
		VALUES
			(
				@IdCliente,
				@IdAgente,
				GETDATE(),
				@Detalhe
			)
	COMMIT
	END TRY
	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS ErrorMessage
		ROLLBACK TRANSACTION
	END CATCH
END

CREATE PROCEDURE sp_ObterClientes
AS
BEGIN
	SELECT
		IdCustomer Codigo,
		Name Nome,
		Gender Genero,
		DocumentNumber Documento,
		BirthDate DataNascimento
	FROM
		Customer WITH (NOLOCK)
END

ALTER PROCEDURE sp_ObterContatos	
	(
		@IdCliente INT
	)
AS
BEGIN
	SELECT
		IdContact,
		IdCustomer,
		Agent.IdAgent,
		Agent.Name,
		ContactDate,
		Detail
	FROM
		Contact WITH (NOLOCK)
	INNER JOIN Agent WITH (NOLOCK) ON Contact.IdAgent = Agent.IdAgent
	WHERE
		IdCustomer = @IdCliente
END

CREATE PROCEDURE sp_ObterCliente
	(
		@IdCliente INT
	)
AS
BEGIN
	SELECT
		IdCustomer Codigo,
		Name Nome,
		Gender Genero,
		DocumentNumber Documento,
		BirthDate DataNascimento
	FROM
		Customer WITH (NOLOCK)
	WHERE
		IdCustomer = @IdCliente
END
ALTER PROCEDURE sp_GravarTelefone
	(
		@IdCliente INT,
		@Telefone VARCHAR(11)
	)
AS
BEGIN
	DECLARE @CONT INT
	SELECT @CONT = COUNT(0) FROM Telephone WITH (NOLOCK) WHERE IdCustomer = @IdCliente AND PhoneNumber = @Telefone

	IF @CONT > 0
		BEGIN
			RETURN;
		END

	INSERT INTO Telephone
		(
			IdCustomer,
			PhoneNumber,
			Status
		)
	VALUES
		(
			@IdCliente,
			@Telefone,
			1
		)
END
CREATE PROCEDURE sp_ObterTelefones
	(
		@IdCliente INT
	)
AS
BEGIN
	SELECT
		PhoneNumber Telefone
	FROM
		Telephone WITH (NOLOCK)
	WHERE
		IdCustomer = @IdCliente
END

CREATE PROCEDURE sp_ObterEndereco
	(
		@IdCliente INT
	)
AS
BEGIN
	SELECT
		PostalCode,
		PublicPlace,
		Number,
		Neighborhood,
		Complement,
		City,
		FS
	FROM
		Address WITH (NOLOCK)
	WHERE
		IdCustomer = @IdCliente
END

CREATE PROCEDURE sp_ValidarLogin
	(
		@Login VARCHAR(20),
		@Senha VARCHAR(20)
	)
AS
BEGIN
	SELECT
		IdAgent
	FROM
		Agent WITH (NOLOCK)
	WHERE
		Login = @Login
		AND Password = @Senha
END

SELECT
	SubjectDetail.IdSubjectGroup IdGrupo,
	SubjectDetail.IdSubjectSubGroup IdSubGrupo,
	SubjectDetail.IdSubjectDetail IdDetalhe,
	SubjectGroup.Name Grupo,
	SubjectSubGroup.Name SubGrupo,
	SubjectDetail.Name Detalhe
FROM SubjectDetail WITH (NOLOCK)
INNER JOIN SubjectGroup WITH (NOLOCK) ON SubjectDetail.IdSubjectGroup = SubjectGroup.IdSubjectGroup
INNER JOIN SubjectSubGroup WITH (NOLOCK) ON SubjectDetail.IdSubjectGroup = SubjectSubGroup.IdSubjectGroup AND SubjectDetail.IdSubjectSubGroup = SubjectSubGroup.IdSubjectSubGroup

CREATE PROCEDURE sp_ObterGrupoOcorrencia
AS
BEGIN
	SELECT
		Name Grupo,
		IdSubjectGroup IdGrupo
	FROM
		SubjectGroup WITH (NOLOCK)
END

ALTER PROCEDURE sp_ObterSubGrupoOcorrencia
	(
		@IdGrupo INT
	)
AS
BEGIN
	SELECT
		Name SubGrupo,
		IdSubjectSubGroup IdSubGrupo
	FROM
		SubjectSubGroup WITH (NOLOCK)
	WHERE
		IdSubjectGroup = @IdGrupo
END

ALTER PROCEDURE sp_ObterDetalheOcorrencia
	(
		@IdGrupo INT,
		@IdSubGrupo INT
	)
AS
BEGIN
	SELECT
		Name Detalhe,
		IdSubjectDetail IdDetalhe
	FROM
		SubjectDetail WITH (NOLOCK)
	WHERE
		IdSubjectGroup = @IdGrupo
		AND IdSubjectSubGroup = @IdSubGrupo
END

SELECT * FROM Customer	
SELECT * FROM Address
SELECT * FROM Telephone
SELECT * FROM Agent

--INSERT INTO Telephone VALUES (1,'11989287765',1)
