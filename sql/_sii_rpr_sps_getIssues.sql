DROP PROCEDURE _sii_rpr_spi_addPallet

GO
/****** Object:  StoredProcedure [dbo].[_sii_rpr_sps_getIssues]    Script Date: 8/3/2018 1:17:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tom Bond, Synesis Intl. Inc
-- Create date: 08/06/2018
-- Description:	Get Issues
-- =============================================
CREATE PROCEDURE [dbo].[_sii_rpr_spi_addPallet] 
	-- Add the parameters for the stored procedure here
	@productionOrder int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	 	SELECT WOR1.LineNum, WOR1.ItemCode, WOR1.BaseQty
		FROM WOR1
		INNER JOIN OWOR ON OWOR.DocEntry = WOR1.DocEntry 
		WHERE WOR1.IssueType = 'M' AND OWOR.DocNum =  @productionOrder
END


