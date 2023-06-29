DROP PROCEDURE _sii_rpr_sps_getFGProdOrders

GO
/****** Object:  StoredProcedure [dbo].[_sii_rpr_sps_getFGProdOrders]    Script Date: 8/3/2018 1:17:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tom Bond, Synesis Intl. Inc
-- Create date: 08/01/2018
-- Description:	Get Production Orders
-- =============================================
CREATE PROCEDURE [dbo].[_sii_rpr_sps_getFGProdOrders] 
	-- Add the parameters for the stored procedure here
	@productionLine nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    IF (@productionLine = '')
	 	SELECT DocNum as SAPOrderNo, U_SII_YanJanOrderId as YJNOrder, U_SII_CurrentDie as AperatureDieNo, 
			Coalesce(U_SII_NoOfSlits,0) as NoOfSlits, StartDate, DueDate, 'U' as FactoryCode, OITM.ItemCode, OITM.ItemName as ItemDescription,
			OITM.U_SII_IRMS as IRMS, pl.U_SII_LineNo as ProductionMachineNo, OWOR.U_PMX_PLCD as ProductionLine, U_SII_BatchNo as BatchNo,
			OWOR.U_SII_MatlCode as MaterialCode, OWOR.U_SII_ProdName as ProductName, OWOR.U_SII_JumboRollNo as JumboRoll
		FROM OWOR
		INNER JOIN OITM ON OWOR.ItemCode = OITM.ItemCode 
		LEFT JOIN [@SII_PRODLINES] pl ON OWOR.U_PMX_PLCD = pl.Code
		WHERE OWOR.Status = 'R' AND OITM.ItmsGrpCod <> 103
	ELSE
	    SELECT DocNum as SAPOrderNo, U_SII_YanJanOrderId as YJNOrder, U_SII_CurrentDie as AperatureDieNo, 
			Coalesce(U_SII_NoOfSlits,0)  as NoOfSlits, StartDate, DueDate, 'U' as FactoryCode, OITM.ItemCode, OITM.ItemName as ItemDescription,
			OITM.U_SII_IRMS as IRMS, pl.U_SII_LineNo as ProductionMachineNo, OWOR.U_PMX_PLCD as ProductionLine, U_SII_BatchNo as BatchNo,
			OWOR.U_SII_MatlCode as MaterialCode, OWOR.U_SII_ProdName as ProductName,OWOR.U_SII_JumboRollNo as JumboRoll
		FROM OWOR
		INNER JOIN OITM ON OWOR.ItemCode = OITM.ItemCode 
		LEFT JOIN [@SII_PRODLINES] pl ON OWOR.U_PMX_PLCD = pl.Code
		WHERE OWOR.Status = 'R' AND OITM.ItmsGrpCod <> 103 AND OWOR.U_PMX_PLCD=@productionLine
END


