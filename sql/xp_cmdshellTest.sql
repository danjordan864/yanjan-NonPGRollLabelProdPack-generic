Create table ##PackLabel(Col varchar(250))
insert into ##PackLabel Values('Supplier Product, Production Date, Item Desc., Order No., Cust. Part No.(IRMS), Qty, QtyNoDecimal, Customer Shipping Lot, LotWithPrefix')
insert into ##PackLabel Values('C270-176,6/15/2018,P Formed Film Yanjan C270,9999,96888118,80.00,8000,U2P6C0861,XJWRU2P6C0861')

DECLARE @bcpCommand VARCHAR(8000)
SET @bcpCommand = 'bcp " SELECT * from ##PackLabel" queryout'
SET @bcpCommand = @bcpCommand + ' \\yjn-sap\Produmex\TestTrigger\test.csv -c -w -T -U sa -P B1Admin","-CRAW'
EXEC master..xp_cmdshell @bcpCommand
if @@error > 0
begin
select 0 as ReturnValue -- failure
return
end
drop table ##PackLabel
--drop table ##TempExportData2
--set @columnNames =' '
--set @columnConvert =' '
--set @tempSQL =' '
select 1 as ReturnValue