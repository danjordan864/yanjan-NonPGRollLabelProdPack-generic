select bundle.U_SII_SSCC as 'Bundle Number', Sum(T3.Qty) as 'Weight (KILOS)'
from ODLN T0
inner join DLN1 T1 on T0.DocEntry = T1.DocEntry
inner join OITM T2 on T1.ItemCode = T2.ItemCode
inner join (select S0.DocEntry, S0.DocLine, S1.SysNumber, -sum(S1.Quantity) as Qty from OITL S0 inner join ITL1 S1 
on S0.LogEntry = S1.LogEntry where S0.DocType = 15 group by S0.DocEntry, S0.DocLine, S1.SysNumber) T3 
on T1.DocEntry = T3.DocEntry and T1.LineNum = T3.DocLine
inner join OBTN T4 on T3.SysNumber = T4.SysNumber and T1.ItemCode = T4.ItemCode
inner join [@SII_ROLLS] rolls on rolls.Code = T4.DistNumber
inner join [@SII_PG_BUNDLE] bundle on rolls.U_SII_SSCC = bundle.U_SII_InternalSSCC
where T1.Quantity > 0 and T2.ManBtchNum = 'Y' and T0.DocEntry = 12
GROUP BY bundle.U_SII_SSCC

