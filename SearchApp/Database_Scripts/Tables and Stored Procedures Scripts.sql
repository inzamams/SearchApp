GO
IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'User'))
BEGIN
		CREATE TABLE [dbo].[User](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Email] [nvarchar](300) NULL,
		[Password] [nvarchar](500) NULL,
		[Address] [nvarchar](1000) NULL,
		[CreatedDate] [datetime] NULL,
	 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

GO
IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'Flight'))
BEGIN
		CREATE TABLE [dbo].[Flight](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](100) NULL,
		[Source] [nvarchar](200) NULL,
		[Destination] [nvarchar](200) NULL,
		[Schedule] [datetime] NULL,
		[Price] [int] NULL,
	 CONSTRAINT [PK_Flight] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

GO
IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'UserSearchHistory'))
BEGIN
		CREATE TABLE [dbo].[UserSearchHistory](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[FlightId] [int] NOT NULL,
		[UserId] [int] NOT NULL,
	 CONSTRAINT [PK_UserSearchHistory] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'sp_GetFlightDetails')  
BEGIN
	DROP PROCEDURE sp_GetFlightDetails
END  
GO
CREATE PROCEDURE [dbo].[sp_GetFlightDetails]
(
	@Name NVARCHAR(100) = NULL,
	@Source NVARCHAR(200) = NULL,
	@Destination NVARCHAR(200) = NULL,
	@PriceFrom INT = 0,
	@PriceTo INT = 0,
	@Date NVARCHAR(100) = NULL,
	@UserId INT = 0
)
AS
BEGIN

	CREATE TABLE #Flight_Temp 
	(
		[Id] [int],
		[Name] [nvarchar](100),
		[Source] [nvarchar](200),
		[Destination] [nvarchar](200),
		[Schedule] [datetime],
		[Price] [int]
	)

	INSERT INTO #Flight_Temp
	SELECT [Id], [Name], [Source], [Destination], [Schedule], [Price] FROM dbo.Flight 
	WHERE 
		(@Name IS NULL OR [Name] LIKE '%'+ @Name +'%') AND 
		(@Source IS NULL OR [Source] LIKE '%'+ @Source +'%') AND
		(@Destination IS NULL OR [Destination] LIKE '%'+ @Destination +'%') AND
		(ISNULL(@PriceFrom,0) = 0 OR [Price] >= @PriceFrom) AND
		(ISNULL(@PriceTo,0) = 0 OR [Price] <= @PriceTo) AND
		(ISNULL(@PriceFrom,0) = 0 AND ISNULL(@PriceTo,0) = 0 OR [Price] BETWEEN @PriceFrom AND @PriceTo) AND
		(@Date IS NULL OR Schedule >= CONVERT(char(10), @Date, 126))
		ORDER BY [Price],[Schedule] ASC;
	 
	 INSERT INTO [dbo].[UserSearchHistory]
	 SELECT [Id], @UserId FROM #Flight_Temp;

	 SELECT [Id], [Name], [Source], [Destination],[Price], [Schedule] FROM #Flight_Temp;
	 
	 DROP TABLE #Flight_Temp; 

END
GO

GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'sp_GetUserSearchHistory')  
BEGIN
	DROP PROCEDURE sp_GetUserSearchHistory
END  
GO
CREATE PROCEDURE [dbo].[sp_GetUserSearchHistory]
(
	@UserId INT
)
AS
BEGIN

  WITH FlightCTE AS  (
	  SELECT f.[Name], f.[Source], f.[Destination], f.[Schedule], f.[Price], 
	  ROW_NUMBER() OVER(PARTITION BY [Name], [Source], [Destination], [Schedule], [Price] ORDER BY [Price], [Schedule]) AS row_num  
	  FROM [dbo].[UserSearchHistory] ush
	  INNER JOIN [dbo].[Flight] f ON ush.[FlightId] = f.[Id]
	  WHERE [UserId] = @UserId
  )
  SELECT  [Name], [Source], [Destination], [Schedule], [Price]
	FROM    FlightCTE
	WHERE   row_num = 1
	ORDER BY [Price], [Schedule];

END
GO

GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'sp_InsertUser')  
BEGIN
	DROP PROCEDURE sp_InsertUser
END  
GO
CREATE PROCEDURE [dbo].[sp_InsertUser]
(
	@Email NVARCHAR(300),
	@Password NVARCHAR(500),
	@Address NVARCHAR(1000)
)
AS 
BEGIN

	INSERT INTO [dbo].[User]([Email],[Password],[Address],[CreatedDate]) VALUES
	(@Email, @Password, @Address, GETUTCDATE())

	SELECT CAST(SCOPE_IDENTITY() as int)

END
GO

GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'sp_ValidateUser')  
BEGIN
	DROP PROCEDURE sp_ValidateUser
END  
GO
CREATE PROCEDURE [dbo].[sp_ValidateUser]
(
	@Email NVARCHAR(300),
	@Password NVARCHAR(500)
)
AS
BEGIN

	IF EXISTS (SELECT 1 FROM [dbo].[User] WHERE [Email] = @Email AND [Password] = @Password)
	BEGIN
		SELECT [Id], [Email], [Password] FROM [dbo].[User] WHERE [Email] = @Email 
	END

END
GO