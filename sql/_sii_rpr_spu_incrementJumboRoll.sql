DROP procedure _sii_rpr_spu_incrementJumboRoll
GO
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
CREATE PROCEDURE [dbo].[_sii_rpr_spu_incrementJumboRoll]
@docNum int,
@nextJumboRoll int OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT @nextJumboRoll = U_SII_JumboRollNo + 1 FROM OWOR WHERE OWOR.DocNum = @docNum
	UPDATE OWOR SET U_SII_JumboRollNo = @nextJumboRoll WHERE OWOR.DocNum = @docNum
     
END

