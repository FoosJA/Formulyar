SELECT eo2.name as [Объект дисп],
 dbo.fn_EnNameEquip(EO_Conn.ID) AS [Оборудование],
 EnObj.Name AS [элемент],
 dbo.fn_GetNameOI(Stat.OI, Stat.OIID) as [ТС дорасч],
Stat.OIID  as [имя ТС дорасч],
isnull(F_Stat.Txt,''),
f_stat2.sid
FROM ovc_Depend 
INNER JOIN EnObj ON EnObj.ID = ovc_Depend.ID and not (EnObj.Type in (25,33,39,117)) --and (EnObj.Type = 118) 
INNER JOIN enobj as eo2 on eo2.id = EnObj.higher
INNER JOIN EnObj AS EO_Conn ON EnObj.Higher = EO_Conn.id
INNER JOIN enFactorOI AS Stat ON EnObj.ID = Stat.IDEnObj and (Stat.IDFactor = 100)
left JOIN TSFormulas AS F_Stat ON Stat.OIID = F_Stat.Result and F_Stat.OutOfWork=1
left Join TSFormulasR as f_stat2 on f_stat2.FID=F_Stat.ID and f_stat2.OI='S'