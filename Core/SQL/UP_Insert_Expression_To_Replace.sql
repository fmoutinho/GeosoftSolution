USE [GeodataScanBHPB_DEV]
GO
/****** Object:  StoredProcedure [dbo].[UP_Insert_Expression_To_Replace]    Script Date: 29/01/2018 13:43:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER procedure [dbo].[UP_Insert_Expression_To_Replace]   @Expression VARCHAR(200)
												  ,@Replacement VARCHAR(200)

AS

DECLARE @aux INT

SELECT
	@aux = count(*)

FROM
	dbo.Expression

WHERE
	[Description] = @Expression

IF(@AUX > 0 )
BEGIN

	UPDATE
		dbo.Expression
	SET
		Replacement = @replacement
	WHERE
		[Description] = @Expression

END
ELSE
	INSERT INTO dbo.Expression ([Description],[Replacement],[Action])

	VALUES (@Expression,@replacement,1)