USE [master]
GO
/****** Object:  Database [ProjectManagement]    Script Date: 03-08-2025 20:42:43 ******/
CREATE DATABASE [ProjectManagement]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ProjectManagement', FILENAME = N'D:\SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\ProjectManagement.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ProjectManagement_log', FILENAME = N'D:\SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\ProjectManagement_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [ProjectManagement] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ProjectManagement].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ProjectManagement] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ProjectManagement] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ProjectManagement] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ProjectManagement] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ProjectManagement] SET ARITHABORT OFF 
GO
ALTER DATABASE [ProjectManagement] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ProjectManagement] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ProjectManagement] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ProjectManagement] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ProjectManagement] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ProjectManagement] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ProjectManagement] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ProjectManagement] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ProjectManagement] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ProjectManagement] SET  ENABLE_BROKER 
GO
ALTER DATABASE [ProjectManagement] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ProjectManagement] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ProjectManagement] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ProjectManagement] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ProjectManagement] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ProjectManagement] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ProjectManagement] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ProjectManagement] SET RECOVERY FULL 
GO
ALTER DATABASE [ProjectManagement] SET  MULTI_USER 
GO
ALTER DATABASE [ProjectManagement] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ProjectManagement] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ProjectManagement] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ProjectManagement] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ProjectManagement] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ProjectManagement] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'ProjectManagement', N'ON'
GO
ALTER DATABASE [ProjectManagement] SET QUERY_STORE = ON
GO
ALTER DATABASE [ProjectManagement] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [ProjectManagement]
GO
/****** Object:  Table [dbo].[ProjectDevelopers]    Script Date: 03-08-2025 20:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectDevelopers](
	[ProjectId] [bigint] NOT NULL,
	[DeveloperId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC,
	[DeveloperId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Projects]    Script Date: 03-08-2025 20:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Projects](
	[Id] [bigint] NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Description] [nvarchar](max) NULL,
	[ProjectManagerId] [int] NULL,
	[CreatedAt] [datetime] NULL,
	[Status] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProjectTasks]    Script Date: 03-08-2025 20:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectTasks](
	[Id] [bigint] NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[ProjectId] [bigint] NOT NULL,
	[AssignedToId] [int] NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
	[CreatedAt] [datetime] NULL,
	[DueDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 03-08-2025 20:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[PasswordHash] [nvarchar](max) NOT NULL,
	[Role] [nvarchar](50) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Projects] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[ProjectTasks] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[ProjectDevelopers]  WITH CHECK ADD FOREIGN KEY([DeveloperId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[ProjectDevelopers]  WITH CHECK ADD FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Projects] ([Id])
GO
ALTER TABLE [dbo].[Projects]  WITH CHECK ADD FOREIGN KEY([ProjectManagerId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[ProjectTasks]  WITH CHECK ADD  CONSTRAINT [FK_AssignedTo] FOREIGN KEY([AssignedToId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[ProjectTasks] CHECK CONSTRAINT [FK_AssignedTo]
GO
ALTER TABLE [dbo].[ProjectTasks]  WITH CHECK ADD  CONSTRAINT [FK_Project] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Projects] ([Id])
GO
ALTER TABLE [dbo].[ProjectTasks] CHECK CONSTRAINT [FK_Project]
GO
/****** Object:  StoredProcedure [dbo].[SP_AddDeveloperToProject]    Script Date: 03-08-2025 20:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_AddDeveloperToProject]
    @ProjectId BIGINT,
    @DeveloperId INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO ProjectDevelopers (ProjectId, DeveloperId)
        VALUES (@ProjectId, @DeveloperId);

        SELECT 
            200 AS ResponseCode, 
            'Assign Project Developer Successfully' AS ResponseMessage, 
            SCOPE_IDENTITY() AS ResponseId;
    END TRY
    BEGIN CATCH
        SELECT 
            500 AS ResponseCode, 
            ERROR_MESSAGE() AS ResponseMessage, 
            0 AS ResponseId;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_CreateProject]    Script Date: 03-08-2025 20:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_CreateProject]
    @Id bigint,
    @Name NVARCHAR(100),
    @Description NVARCHAR(MAX),
    @ProjectManagerId bigint,
    @Status NVARCHAR(20)
AS
BEGIN
    INSERT INTO Projects (Id, Name, Description, ProjectManagerId, Status)
    VALUES (@Id, @Name, @Description, @ProjectManagerId, @Status)

    SELECT 200 AS ResponseCode, 'Project created successfully' AS ResponseMessage, @Id AS ResponseId
END
GO
/****** Object:  StoredProcedure [dbo].[SP_CreateTask]    Script Date: 03-08-2025 20:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_CreateTask]
    @Id bigint,
    @Title NVARCHAR(200),
    @Description NVARCHAR(MAX),
    @ProjectId bigint,
    @AssignedToId int,
    @Status NVARCHAR(50),
    @CreatedAt DATETIME,
    @DueDate DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO ProjectTasks (Id, Title, Description, ProjectId, AssignedToId, Status, CreatedAt, DueDate)
        VALUES (@Id, @Title, @Description, @ProjectId, @AssignedToId, @Status, @CreatedAt, @DueDate);

        SELECT 
            200 AS ResponseCode, 
            'Task created successfully' AS ResponseMessage,
            @Id AS ResponseId;
    END TRY
    BEGIN CATCH
        SELECT 
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage,
            NULL AS ResponseId;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DeleteAllDevelopersFromProject]    Script Date: 03-08-2025 20:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_DeleteAllDevelopersFromProject]
    @ProjectId bigint
AS
BEGIN
    BEGIN TRY
        DELETE FROM ProjectDevelopers WHERE ProjectId = @ProjectId;

        SELECT 200 AS ResponseCode, 'All developers removed from project' AS ResponseMessage, @ProjectId AS ResponseId;
    END TRY
    BEGIN CATCH
        SELECT 500 AS ResponseCode, ERROR_MESSAGE() AS ResponseMessage, NULL AS ResponseId;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DeleteProject]    Script Date: 03-08-2025 20:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_DeleteProject]
    @Id bigint
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Projects WHERE Id = @Id)
    BEGIN
        DELETE FROM Projects WHERE Id = @Id
        SELECT 200 AS ResponseCode, 'Project deleted successfully' AS ResponseMessage, @Id AS ResponseId
    END
    ELSE
    BEGIN
        SELECT 404 AS ResponseCode, 'Project not found' AS ResponseMessage, 0 AS ResponseId
    END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DeleteTask]    Script Date: 03-08-2025 20:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_DeleteTask]
    @Id Bigint
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DELETE FROM ProjectTasks WHERE Id = @Id;

        SELECT 
            200 AS ResponseCode,
            'Task deleted successfully' AS ResponseMessage,
            @Id AS ResponseId;
    END TRY
    BEGIN CATCH
        SELECT 
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage,
            NULL AS ResponseId;
    END CATCH
END


GO
/****** Object:  StoredProcedure [dbo].[SP_DeleteUserById]    Script Date: 03-08-2025 20:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_DeleteUserById]
    @Id bigint
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Users WHERE Id = @Id)
    BEGIN
        DELETE FROM Users WHERE Id = @Id

        SELECT 
            200 AS ResponseCode,
            'User deleted successfully' AS ResponseMessage,
            @Id AS ResponseId
    END
    ELSE
    BEGIN
        SELECT 
            404 AS ResponseCode,
            'User not found' AS ResponseMessage,
            0 AS ResponseId
    END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetAllProjects]    Script Date: 03-08-2025 20:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_GetAllProjects]
AS
BEGIN
    SELECT * FROM Projects
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetAllUsers]    Script Date: 03-08-2025 20:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetAllUsers]
AS
BEGIN
    SELECT Id, Username, Email, Role, CreatedAt
    FROM Users
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetProjectById]    Script Date: 03-08-2025 20:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetProjectById]
    @Id bigint
AS
BEGIN
    SELECT * FROM Projects WHERE Id = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetTasksByProjectId]    Script Date: 03-08-2025 20:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetTasksByProjectId]
    @ProjectId Bigint
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * FROM ProjectTasks
    WHERE ProjectId = @ProjectId
END


GO
/****** Object:  StoredProcedure [dbo].[SP_Login_User_By_Email]    Script Date: 03-08-2025 20:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Login_User_By_Email]
    @Email NVARCHAR(100)
AS
BEGIN
    SELECT TOP 1 * FROM Users WHERE Email = @Email
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Register_User]    Script Date: 03-08-2025 20:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Register_User]
    @Username NVARCHAR(100),
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(MAX),
    @Role NVARCHAR(50),
    @CreatedAt DATETIME
AS
BEGIN
    INSERT INTO Users (Username, Email, PasswordHash, Role, CreatedAt)
    VALUES (@Username, @Email, @PasswordHash, @Role, @CreatedAt);
	Select 'Create User Successfuly' as ResponseMessage,200 as ResponseCode,SCOPE_IDENTITY() as ResponseId
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateProject]    Script Date: 03-08-2025 20:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_UpdateProject]
    @Id bigint,
    @Name NVARCHAR(100),
    @Description NVARCHAR(MAX),
    @Status NVARCHAR(20)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Projects WHERE Id = @Id)
    BEGIN
        UPDATE Projects
        SET Name = @Name,
            Description = @Description,
            Status = @Status
        WHERE Id = @Id

        SELECT 200 AS ResponseCode, 'Project updated successfully' AS ResponseMessage, @Id AS ResponseId
    END
    ELSE
    BEGIN
        SELECT 404 AS ResponseCode, 'Project not found' AS ResponseMessage, 0 AS ResponseId
    END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateTask]    Script Date: 03-08-2025 20:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_UpdateTask]
    @Id bigint,
    @Title NVARCHAR(200),
    @Description NVARCHAR(MAX),
    @Status NVARCHAR(50),
    @DueDate DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE ProjectTasks
        SET Title = @Title,
            Description = @Description,
            Status = @Status,
            DueDate = @DueDate
        WHERE Id = @Id;

        SELECT 
            200 AS ResponseCode,
            'Task updated successfully' AS ResponseMessage,
            @Id AS ResponseId;
    END TRY
    BEGIN CATCH
        SELECT 
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage,
            NULL AS ResponseId;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateTaskStatus]    Script Date: 03-08-2025 20:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_UpdateTaskStatus]
    @Id bigint,
    @Status NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE ProjectTasks
        SET Status = @Status
        WHERE Id = @Id;

        SELECT 
            200 AS ResponseCode,
            'Task status updated successfully' AS ResponseMessage,
            @Id AS ResponseId;
    END TRY
    BEGIN CATCH
        SELECT 
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage,
            NULL AS ResponseId;
    END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateUser]    Script Date: 03-08-2025 20:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_UpdateUser]
    @Id bigint,
    @Username NVARCHAR(100),
    @Email NVARCHAR(100)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Users WHERE Id = @Id)
    BEGIN
        UPDATE Users
        SET 
            Username = @Username,
            Email = @Email
        WHERE Id = @Id

        SELECT 
            200 AS ResponseCode,
            'User updated successfully' AS ResponseMessage,
            @Id AS ResponseId
    END
    ELSE
    BEGIN
        SELECT 
            404 AS ResponseCode,
            'User not found' AS ResponseMessage,
            0 AS ResponseId
    END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateUserRole]    Script Date: 03-08-2025 20:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_UpdateUserRole]
    @Id Bigint,
    @Role NVARCHAR(20)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Users WHERE Id = @Id)
    BEGIN
        UPDATE Users
        SET Role = @Role
        WHERE Id = @Id

        SELECT 
            200 AS ResponseCode,
            'User role updated successfully' AS ResponseMessage,
            @Id AS ResponseId
    END
    ELSE
    BEGIN
        SELECT 
            404 AS ResponseCode,
            'User not found' AS ResponseMessage,
            0 AS ResponseId
    END
END
GO
USE [master]
GO
ALTER DATABASE [ProjectManagement] SET  READ_WRITE 
GO
