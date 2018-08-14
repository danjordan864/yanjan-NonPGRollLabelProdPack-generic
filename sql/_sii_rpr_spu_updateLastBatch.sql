DROP PROCEDURE _sii_rpr_spu_updateLastBatch 
/****** Object:  StoredProcedure [dbo].[_sii_rpr_spu_updateLastBatch]    Script Date: 8/8/2018 11:23:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tom Bond, Synesis Intl. Inc
-- Create date: 08/01/2018
-- Description:	Get Production Orders
-- =============================================
CREATE PROCEDURE [dbo].[_sii_rpr_spu_updateLastBatch] 
	-- Add the parameters for the stored procedure here
	@prodMachine nvarchar(50),
	@prodStartDate DateTime,
	@matlCode nvarchar(2),
	@prodName nvarchar(2)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @newBatchNo as int,
	        @newBatchCode as nvarchar(1),
		    @yjnOrderNo as nvarchar(9),
			@prodMo as int,
			@prodMoCode as nvarchar(1),
			@prodYr as int,
			@lineNo as nvarchar(1)

	SELECT @lineNo = U_SII_LineNo FROM [@SII_PRODLINES] pl WHERE pl.Code = @prodMachine
	SELECT @prodYr = RIGHT(YEAR(@prodStartDate),1)

	SELECT @prodMo = MONTH(@prodStartDate)

	IF(@prodMo < 10)
		SELECT @prodMoCode = @prodMo
	ELSE
		SELECT @prodMoCode = CASE WHEN @prodMo = 10 THEN 'O' WHEN @prodMo = 11 THEN 'N' ELSE 'D' END

	SELECT @newBatchNo = Coalesce(Max(U_SII_LastBatch),0) +1 FROM [@SII_BATCHCOUNT] WHERE U_SII_ProdMonth = @prodMo 
		AND U_SII_ProdYr = @prodYr AND U_SII_ProdLine = @prodMachine

	SELECT @newBatchCode = CASE WHEN @newBatchNo < 10 THEN Cast(@newBatchNo as nvarchar(1)) 
		WHEN @newBatchNo = 10 THEN '0'
		WHEN @newBatchNo = 11 THEN 'A'
		WHEN @newBatchNo = 12 THEN 'B'
		WHEN @newBatchNo = 13 THEN 'C'
		WHEN @newBatchNo = 14 THEN 'D'
		WHEN @newBatchNo = 15 THEN 'E'
		WHEN @newBatchNo = 16 THEN 'F'
		WHEN @newBatchNo = 17 THEN 'G'
		WHEN @newBatchNo = 18 THEN 'H'
		WHEN @newBatchNo = 19 THEN 'I'
		WHEN @newBatchNo = 20 THEN 'J'
		WHEN @newBatchNo = 21 THEN 'K'
		WHEN @newBatchNo = 22 THEN 'L'
		WHEN @newBatchNo = 23 THEN 'M'
		WHEN @newBatchNo = 24 THEN 'N'
		WHEN @newBatchNo = 25 THEN 'O'
		WHEN @newBatchNo = 26 THEN 'P'
		WHEN @newBatchNo = 27 THEN 'Q'
		WHEN @newBatchNo = 28 THEN 'R'
		WHEN @newBatchNo = 29 THEN 'S'
		WHEN @newBatchNo = 30 THEN 'T'
		WHEN @newBatchNo = 31 THEN 'U'
		WHEN @newBatchNo = 32 THEN 'V'
		WHEN @newBatchNo = 33 THEN 'W'
		WHEN @newBatchNo = 34 THEN 'X'
		WHEN @newBatchNo = 35 THEN 'Y'
		WHEN @newBatchNo = 36 THEN 'Z'
		END

    IF (@newBatchNo >1)
		UPDATE [@SII_BATCHCOUNT] SET U_SII_ProdLine = @prodMachine, U_SII_ProdMonth = @prodMo, U_SII_ProdYr = @prodYr,
			U_SII_LastBatch = @newBatchNo, U_SII_LastBatchCode = @newBatchCode WHERE U_SII_ProdLine = @prodMachine 
			AND U_SII_ProdMonth = @prodMo AND U_SII_ProdYr = @prodYr
	ELSE
		INSERT INTO [dbo].[@SII_BATCHCOUNT]
           ([Name]
           ,[U_SII_ProdLine]
           ,[U_SII_ProdMonth]
           ,[U_SII_ProdYr]
           ,[U_SII_LastBatch]
		   ,[U_SII_LastBatchCode])
		VALUES
           (null
           ,@prodMachine
           ,@prodMo
           ,@prodYr
           ,@newBatchNo
		   ,@newBatchCode)

   
	SELECT @newBatchCode as NewBatch, 'U' + @lineNo + @matlCode + @prodName + CAST(@prodYr as nvarchar(1)) + @prodMoCode + @newBatchCode as YJNOrder
END
