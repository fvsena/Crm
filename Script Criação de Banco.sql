CREATE DATABASE Crm
GO
USE Crm
ALTER AUTHORIZATION ON DATABASE::Crm TO sa

CREATE TABLE Customer
	(
		IdCustomer INT IDENTITY NOT NULL,
		Name VARCHAR(100) NOT NULL,
		DocumentNumber VARCHAR(20) NOT NULL,
		BirthDate DATETIME NOT NULL,
		Gender CHAR(1) NOT NULL,
		CustomerDate DATETIME NOT NULL
	)
GO
ALTER TABLE Customer ADD CONSTRAINT PK_Customer PRIMARY KEY(IdCustomer)
ALTER TABLE Customer ADD CONSTRAINT CH_GenderCustomer CHECK (Gender IN ('M','F','I'))
ALTER TABLE Customer ADD CONSTRAINT CH_BirthDateCustomer CHECK (BirthDate < GETDATE())
ALTER TABLE Customer ADD CONSTRAINT UN_DocumentNumberCustomer UNIQUE (DocumentNumber)
GO
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
GO
ALTER TABLE Address ADD CONSTRAINT PK_Address PRIMARY KEY(IdAddress)
ALTER TABLE Address ADD CONSTRAINT FK_Address_Customer FOREIGN KEY(IdCustomer) REFERENCES Customer(IdCustomer)
GO
CREATE TABLE Telephone
	(
		IdTelephone INT IDENTITY NOT NULL,
		IdCustomer INT NOT NULL,
		PhoneNumber VARCHAR(11) NOT NULL,
		Status BIT NOT NULL
	)
GO
ALTER TABLE Telephone ADD CONSTRAINT PK_Telephone PRIMARY KEY(IdTelephone)
ALTER TABLE Telephone ADD CONSTRAINT FK_Telephone_Customer FOREIGN KEY(IdCustomer) REFERENCES Customer(IdCustomer)
GO
CREATE TABLE SubjectGroup
	(
		IdSubjectGroup INT IDENTITY NOT NULL,
		Name VARCHAR(50) NOT NULL
	)
GO
ALTER TABLE SubjectGroup ADD CONSTRAINT PK_SubjectGroup PRIMARY KEY(IdSubjectGroup)
GO
CREATE TABLE SubjectSubGroup
	(
		IdSubjectSubGroup INT NOT NULL,
		IdSubjectGroup INT NOT NULL,
		Name VARCHAR(50) NOT NULL
	)
GO
ALTER TABLE SubjectSubGroup ADD CONSTRAINT PK_SubjectSubGroup PRIMARY KEY (IdSubjectSubGroup, IdSubjectGroup)
ALTER TABLE SubjectSubGroup ADD CONSTRAINT FK_SubjectSubGroup_SubjectGroup FOREIGN KEY(IdSubjectGroup) REFERENCES SubjectGroup(IdSubjectGroup)
GO
CREATE TABLE SubjectDetail
	(
		IdSubjectDetail INT NOT NULL,
		IdSubjectSubGroup INT NOT NULL,
		IdSubjectGroup INT NOT NULL,
		Name VARCHAR(100) NOT NULL
	)
GO
ALTER TABLE SubjectDetail ADD CONSTRAINT PK_SubjectDetail PRIMARY KEY(IdSubjectGroup, IdSubjectSubGroup, IdSubjectDetail)
ALTER TABLE SubjectDetail ADD CONSTRAINT FK_SubjectDetail_SubjectSubGroup FOREIGN KEY(IdSubjectSubGroup, IdSubjectGroup) REFERENCES SubjectSubGroup(IdSubjectSubGroup, IdSubjectGroup)
GO
CREATE TABLE Agent
	(
		IdAgent INT IDENTITY NOT NULL,
		Name VARCHAR(100) NOT NULL,
		Login VARCHAR(30),
		Password VARCHAR(255),
		Status BIT NOT NULL
	)
GO
ALTER TABLE Agent ADD CONSTRAINT PK_Agent PRIMARY KEY(IdAgent)
GO
INSERT INTO Agent VALUES ('admin','admin.crm','123',1)
GO
CREATE TABLE Contact
	(
		IdContact INT IDENTITY NOT NULL,
		IdCustomer INT NOT NULL,
		IdAgent INT NOT NULL,
		ContactDate DATETIME NOT NULL,
		Detail TEXT NOT NULL
	)
GO
ALTER TABLE Contact ADD CONSTRAINT PK_Contact PRIMARY KEY(IdContact)
ALTER TABLE Contact ADD CONSTRAINT FK_Contact_Customer FOREIGN KEY(IdCustomer) REFERENCES Customer(IdCustomer)
ALTER TABLE Contact ADD CONSTRAINT FK_Contact_Agent FOREIGN KEY(IdAgent) REFERENCES Agent(IdAgent)
GO
CREATE TABLE OccurrenceStatus
	(
		IdOccurrenceStatus INT IDENTITY NOT NULL,
		Name VARCHAR(50) NOT NULL
	)
GO
ALTER TABLE OccurrenceStatus ADD CONSTRAINT PK_OccurrenceStatus PRIMARY KEY(IdOccurrenceStatus)
GO
INSERT INTO OccurrenceStatus VALUES ('Pendente')
INSERT INTO OccurrenceStatus VALUES ('Em andamento')
INSERT INTO OccurrenceStatus VALUES ('Encerrado')
GO
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
GO
ALTER TABLE Occurrence ADD CONSTRAINT PK_Occurrence PRIMARY KEY(IdOccurrence)
ALTER TABLE Occurrence ADD CONSTRAINT FK_Occurrence_Customer FOREIGN KEY(IdCustomer) REFERENCES Customer(IdCustomer)
ALTER TABLE Occurrence ADD CONSTRAINT FK_Occurrence_Agent FOREIGN KEY(IdAgent) REFERENCES Agent(IdAgent)
ALTER TABLE Occurrence ADD CONSTRAINT FK_Occurrence_SubjectDetail FOREIGN KEY(IdSubjectGroup, IdSubjectSubGroup, IdSubjectDetail) REFERENCES SubjectDetail(IdSubjectGroup, IdSubjectSubGroup, IdSubjectDetail)
ALTER TABLE Occurrence ADD CONSTRAINT FK_Occurrence_OccurrenceStatus FOREIGN KEY(IdOccurrenceStatus) REFERENCES OccurrenceStatus(IdOccurrenceStatus)
GO
CREATE TABLE OccurrenceUpdateType
	(
		IdOccurrenceUpdateType INT IDENTITY NOT NULL,
		Name VARCHAR(50) NOT NULL
	)
GO
ALTER TABLE OccurrenceUpdateType ADD CONSTRAINT PK_OccurrenceUpdateType PRIMARY KEY(IdOccurrenceUpdateType)
GO
INSERT INTO OccurrenceUpdateType VALUES ('Abertura')
INSERT INTO OccurrenceUpdateType VALUES ('Atualização')
INSERT INTO OccurrenceUpdateType VALUES ('Finalização')
GO
CREATE TABLE OccurrenceUpdate
	(
		IdOccurrenceUpdate INT IDENTITY NOT NULL,
		IdOccurrenceUpdateType INT NOT NULL,
		IdOccurrence INT NOT NULL,
		IdAgent INT NOT NULL,
		UpdateDate DATETIME NOT NULL,
		UpdateMessage TEXT,
	)
GO
ALTER TABLE OccurrenceUpdate ADD CONSTRAINT PK_OccurrenceUpdate PRIMARY KEY(IdOccurrenceUpdate)
ALTER TABLE OccurrenceUpdate ADD CONSTRAINT FK_OccurrenceUpdate_OccurrenceUpdateType FOREIGN KEY(IdOccurrenceUpdateType) REFERENCES OccurrenceUpdateType(IdOccurrenceUpdateType)
ALTER TABLE OccurrenceUpdate ADD CONSTRAINT FK_OccurrenceUpdate_Occurrence FOREIGN KEY(IdOccurrence) REFERENCES Occurrence(IdOccurrence)
ALTER TABLE OccurrenceUpdate ADD CONSTRAINT FK_OccurrenceUpdate_Agent FOREIGN KEY(IdAgent) REFERENCES Agent(IdAgent)
GO
CREATE TABLE SchedulingType
	(
		IdSchedulingType INT IDENTITY NOT NULL,
		Name VARCHAR(50) NOT NULL
	)
GO
ALTER TABLE SchedulingType ADD CONSTRAINT PK_SchedulingType PRIMARY KEY(IdSchedulingType)
GO
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
GO
ALTER TABLE Scheduling ADD CONSTRAINT PK_Scheduling PRIMARY KEY(IdScheduling)
ALTER TABLE Scheduling ADD CONSTRAINT FK_Scheduling_SchedulingType FOREIGN KEY(IdSchedulingType) REFERENCES SchedulingType(IdSchedulingType)
ALTER TABLE Scheduling ADD CONSTRAINT FK_Scheduling_Occurrence FOREIGN KEY(IdOccurrence) REFERENCES Occurrence(IdOccurrence)
ALTER TABLE Scheduling ADD CONSTRAINT FK_Scheduling_Agent FOREIGN KEY(IdAgent) REFERENCES Agent(IdAgent)
GO
CREATE PROCEDURE sp_GravarCliente
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
GO
CREATE PROCEDURE sp_GravarEndereco
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
GO
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
GO
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
GO
CREATE PROCEDURE sp_ObterContatos	
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
GO
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
GO
CREATE PROCEDURE sp_GravarTelefone
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
GO
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
GO
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
GO
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
GO
CREATE PROCEDURE sp_ObterGrupoOcorrencia
AS
BEGIN
	SELECT
		Name Grupo,
		IdSubjectGroup IdGrupo
	FROM
		SubjectGroup WITH (NOLOCK)
END
GO
CREATE PROCEDURE sp_ObterSubGrupoOcorrencia
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
GO
CREATE PROCEDURE sp_ObterDetalheOcorrencia
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
GO
CREATE PROCEDURE sp_GravarGrupoOcorrencia
	(
		@Grupo VARCHAR(50)
	)
AS
BEGIN
	DECLARE @CONT INT
	SELECT @CONT = COUNT(Name) FROM SubjectGroup WITH (NOLOCK) WHERE Name = @Grupo

	IF @CONT > 0
		BEGIN
			RETURN
		END

	INSERT INTO SubjectGroup
		(
			Name
		)
	VALUES
		(
			@Grupo
		)
END
GO
CREATE PROCEDURE sp_GravarSubGrupoOcorrencia
	(
		@Grupo INT,
		@SubGrupo VARCHAR(50)
	)
AS
BEGIN
	DECLARE @CONT INT
	SELECT @CONT = COUNT(Name) FROM SubjectSubGroup WITH (NOLOCK) WHERE IdSubjectGroup = @Grupo AND Name = @SubGrupo

	IF @CONT > 0
		BEGIN
			RETURN
		END

	DECLARE @CodigoSubGrupo INT
	SELECT @CodigoSubGrupo = ISNULL(MAX(IdSubjectSubGroup),0)+1 FROM SubjectSubGroup WITH (NOLOCK) WHERE IdSubjectGroup = @Grupo

	INSERT INTO SubjectSubGroup
		(
			IdSubjectSubGroup,
			IdSubjectGroup,
			Name
		)
	VALUES
		(
			@CodigoSubGrupo,
			@Grupo,
			@SubGrupo
		)
END
GO
CREATE PROCEDURE sp_GravarDetalheOcorrencia
	(
		@Grupo INT,
		@SubGrupo INT,
		@Detalhe VARCHAR(100)
	)
AS
BEGIN
	DECLARE @CONT INT
	SELECT @CONT = COUNT(Name) FROM SubjectDetail WITH (NOLOCK) WHERE IdSubjectGroup = @Grupo AND IdSubjectSubGroup = @SubGrupo AND Name = @Detalhe

	IF @CONT > 0
		BEGIN
			RETURN
		END

	DECLARE @CodigoDetalhe INT
	SELECT @CodigoDetalhe = ISNULL(MAX(IdSubjectDetail),0)+1 FROM SubjectDetail WITH (NOLOCK) WHERE IdSubjectGroup = @Grupo AND IdSubjectSubGroup = @SubGrupo
	
	INSERT INTO SubjectDetail
		(
			IdSubjectGroup,
			IdSubjectSubGroup,
			IdSubjectDetail,
			Name
		)
	VALUES
		(
			@Grupo,
			@SubGrupo,
			@CodigoDetalhe,
			@Detalhe
		)
END
GO
CREATE PROCEDURE sp_GravarOcorrencia
	(
		@IdCliente INT,
		@IdAgente INT,
		@CodigoGrupo INT,
		@CodigoSubGrupo INT,
		@CodigoDetalhe INT,
		@Mensagem VARCHAR(255)
	)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			DECLARE @IdOcorrencia INT
			INSERT INTO Occurrence
				(
					IdCustomer,
					IdAgent,
					OccurrenceDate,
					IdSubjectGroup,
					IdSubjectSubGroup,
					IdSubjectDetail,
					IdOccurrenceStatus
				)
			VALUES
				(
					@IdCliente,
					@IdAgente,
					GETDATE(),
					@CodigoGrupo,
					@CodigoSubGrupo,
					@CodigoDetalhe,
					1
				)

			SET @IdOcorrencia = SCOPE_IDENTITY()

			INSERT INTO OccurrenceUpdate
				(
					IdOccurrenceUpdateType,
					IdOccurrence,
					IdAgent,
					UpdateDate,
					UpdateMessage
				)
			VALUES
				(
					1,
					@IdOcorrencia,
					@IdAgente,
					GETDATE(),
					@Mensagem
				)
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SELECT ERROR_MESSAGE() AS ErrorMessage
	END CATCH
	
END
GO
CREATE PROCEDURE sp_ObterOcorrencias
	(
		@Quantidade INT = 10
	)
AS
BEGIN
	SELECT TOP (@Quantidade)
		Occurrence.IdOccurrence AS [IdOcorrencia],
		Occurrence.IdCustomer AS [IdCliente],
		Occurrence.OccurrenceDate AS [DataAbertura],
		Customer.Name AS [Cliente],
		Agent.Name AS [Agente],
		SubjectGroup.Name AS [Grupo],
		SubjectSubGroup.Name AS [SubGrupo],
		SubjectDetail.Name AS [Detalhe],
		OccurrenceUpdate.UpdateMessage AS [UltimaAtualizacao]
	FROM
		Occurrence WITH (NOLOCK)
	INNER JOIN Agent WITH (NOLOCK) ON Occurrence.IdAgent = Agent.IdAgent
	INNER JOIN Customer WITH (NOLOCK) ON Occurrence.IdCustomer = Customer.IdCustomer
	INNER JOIN SubjectGroup WITH (NOLOCK) ON Occurrence.IdSubjectGroup = SubjectGroup.IdSubjectGroup
	INNER JOIN SubjectSubGroup WITH (NOLOCK) ON Occurrence.IdSubjectGroup = SubjectSubGroup.IdSubjectGroup AND Occurrence.IdSubjectSubGroup = SubjectSubGroup.IdSubjectSubGroup
	INNER JOIN SubjectDetail WITH (NOLOCK) ON Occurrence.IdSubjectGroup = SubjectDetail.IdSubjectGroup AND Occurrence.IdSubjectSubGroup = SubjectDetail.IdSubjectSubGroup AND Occurrence.IdSubjectDetail = SubjectDetail.IdSubjectDetail
	INNER JOIN
		(
			SELECT
				MAX(IdOccurrenceUpdate) IdOccurrenceUpdate,
				OccurrenceUpdate.IdOccurrence IdOccurrence
			FROM
				OccurrenceUpdate WITH (NOLOCK)
			GROUP BY
				OccurrenceUpdate.IdOccurrence
		) UPDATES ON UPDATES.IdOccurrence = Occurrence.IdOccurrence
	INNER JOIN OccurrenceUpdate WITH (NOLOCK) ON UPDATES.IdOccurrenceUpdate = OccurrenceUpdate.IdOccurrenceUpdate
	ORDER BY
		Occurrence.OccurrenceDate DESC
END
GO
CREATE PROCEDURE sp_BuscarOcorrencia
	(
		@Codigo INT
	)
AS
BEGIN
	SELECT
		Occurrence.IdOccurrence AS [IdOcorrencia],
		Occurrence.IdCustomer AS [IdCliente],
		Occurrence.OccurrenceDate AS [DataAbertura],
		Customer.Name AS [Cliente],
		Agent.Name AS [Agente],
		SubjectGroup.Name AS [Grupo],
		SubjectSubGroup.Name AS [SubGrupo],
		SubjectDetail.Name AS [Detalhe]
	FROM
		Occurrence WITH (NOLOCK)
	INNER JOIN Agent WITH (NOLOCK) ON Occurrence.IdAgent = Agent.IdAgent
	INNER JOIN Customer WITH (NOLOCK) ON Occurrence.IdCustomer = Customer.IdCustomer
	INNER JOIN SubjectGroup WITH (NOLOCK) ON Occurrence.IdSubjectGroup = SubjectGroup.IdSubjectGroup
	INNER JOIN SubjectSubGroup WITH (NOLOCK) ON Occurrence.IdSubjectGroup = SubjectSubGroup.IdSubjectGroup AND Occurrence.IdSubjectSubGroup = SubjectSubGroup.IdSubjectSubGroup
	INNER JOIN SubjectDetail WITH (NOLOCK) ON Occurrence.IdSubjectGroup = SubjectDetail.IdSubjectGroup AND Occurrence.IdSubjectSubGroup = SubjectDetail.IdSubjectSubGroup AND Occurrence.IdSubjectDetail = SubjectDetail.IdSubjectDetail
	WHERE
		Occurrence.IdOccurrence = @Codigo

	SELECT
		OccurrenceUpdate.IdOccurrenceUpdate AS [IdAtualizacao],
		OccurrenceUpdate.UpdateDate AS [DataAtualizacao],
		OccurrenceUpdate.IdAgent AS [IdAgente],
		Agent.Name AS [Agente],
		OccurrenceUpdateType.Name AS [TipoAtualizacao],
		OccurrenceUpdate.UpdateMessage AS [Mensagem]
	FROM
		OccurrenceUpdate WITH (NOLOCK)
	INNER JOIN Agent WITH (NOLOCK) ON OccurrenceUpdate.IdAgent = Agent.IdAgent
	INNER JOIN OccurrenceUpdateType WITH (NOLOCK) ON OccurrenceUpdate.IdOccurrenceUpdateType = OccurrenceUpdateType.IdOccurrenceUpdateType
	WHERE
		OccurrenceUpdate.IdOccurrence = @Codigo
END
GO
CREATE PROCEDURE sp_GravarAtualizacao
	(
		@IdOcorrencia INT,
		@IdAgente INT,
		@IdTipoAtualizacao INT,
		@Mensagem VARCHAR(255)
	)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			INSERT INTO OccurrenceUpdate
				(
					IdOccurrenceUpdateType,
					IdOccurrence,
					IdAgent,
					UpdateDate,
					UpdateMessage
				)
			VALUES
				(
					1,
					@IdOcorrencia,
					@IdAgente,
					GETDATE(),
					@Mensagem
				)
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SELECT ERROR_MESSAGE() AS ErrorMessage
	END CATCH
	
END