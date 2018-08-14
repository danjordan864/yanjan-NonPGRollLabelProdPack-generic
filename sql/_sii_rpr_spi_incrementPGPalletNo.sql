DROP procedure _sii_rpr_spi_incrementPGPalletNo
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
CREATE PROCEDURE [dbo].[_sii_rpr_spi_incrementPGPalletNo] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	INSERT INTO [dbo].[@SII_PG_PALLETNO]
           ([Name])
     VALUES
           ('')

	RETURN SCOPE_IDENTITY()
END
