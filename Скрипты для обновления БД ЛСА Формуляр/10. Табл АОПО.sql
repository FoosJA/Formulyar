/*
/******Создание таблицы АОПО********************************************************************/
CREATE TABLE LSA_test..Smtn_Aopo
(DC varchar(max),
EquipmentName varchar(max),
EquipmentID int,
EnergyObject varchar(max),
Step int,
ElementName varchar(max),
NumbTIobject int,
NameTIobject varchar(max),
NumbTIstandart int,
NameTIstandart varchar(max),
NumbTIsetting int,
NameTIsetting varchar(max),
NumbTSitog int,
NameTSitog varchar(max),
NumbTS int,
NameTS varchar(max)
)
*/

use OIK_Tat
go
DECLARE @MAIN_DC varchar(max)
set @MAIN_DC='РДУ Татарстана'

--insert into LSA_test..Smtn_Aopo(DC,EquipmentName,EquipmentID,EnergyObject,ElementName,NumbTIsetting,NameTIsetting,
--NumbTSitog,NameTSitog,NumbTS,NameTS)

-----------------------------------------------------
select Ti.DC,TI.EquipmentName,Ti.EquipmentID,TI.EnergyObject,
--TI.Step, --TS.Step,
TI.ElementName, --TS.ElementName,
TI.NumbSetting, TI.NameSetting,
TS.NumbSitog,TS.NameSitog, TS.NumbS,TS.NameS--, TS.formula
from
--Таблица ТИ СМТН АОПО
(SELECT 
@MAIN_DC as [DC],
dbo.fn_EnNameEquip(EO2.ID) AS [EquipmentName],
EO2.ID as [EquipmentID],
eo2.name as [EnergyObject],
EnObj.ID as [Step],
EnObj.Name as [ElementName],
dbo.ovc_Limiting.OIIDcrash as [NumbSetting],--Идентификатор параметра ОИ для для указания динамической уставки Iав.доп.
dbo.fn_GetNameOI(dbo.ovc_Limiting.OIcrash, dbo.ovc_Limiting.OIIDcrash) as [NameSetting]
FROM dbo.ovc_Limiting INNER JOIN
EnObj ON dbo.ovc_Limiting.ID = EnObj.ID 
INNER JOIN enobj as eo2 on eo2.id = EnObj.higher
WHERE ((dbo.ovc_Limiting.OIcrash IS NOT NULL)) and EnObj.Name like '%АОПО%') as TI
left join
--Таблица ТС СМТН АОПО
(SELECT 
EO2.ID as [EquipmentID],
EnObj.ID as [Step],
EnObj.Name as [ElementName],
 dbo.fn_GetNameOI(Stat.OI, Stat.OIID) as [NameSitog],
Stat.OIID  as [NumbSitog] ,
isnull(F_Stat.Txt,'') as [formula],
f_stat2.sid as [NumbS],
DefTS.Name as [NameS]
FROM ovc_Depend 
INNER JOIN EnObj ON EnObj.ID = ovc_Depend.ID and not (EnObj.Type in (25,33,39,117)) --and (EnObj.Type = 118) 
INNER JOIN enobj as eo2 on eo2.id = EnObj.higher
INNER JOIN EnObj AS EO_Conn ON EnObj.Higher = EO_Conn.id
INNER JOIN enFactorOI AS Stat ON EnObj.ID = Stat.IDEnObj and (Stat.IDFactor = 100)
left JOIN TSFormulas AS F_Stat ON Stat.OIID = F_Stat.Result and F_Stat.OutOfWork=1
left Join TSFormulasR as f_stat2 on f_stat2.FID=F_Stat.ID and f_stat2.OI='S'
left join DefTS on DefTS.ID=f_stat2.sid
) as TS on TI.[EquipmentID]=TS.[EquipmentID] and TI.Step=TS.Step


union


select Ti.DC,TI.EquipmentName,Ti.EquipmentID,TI.EnergyObject,
--TI.Step,-- TS.Step,
TI.ElementName, --TS.ElementName,
TI.NumbSetting, TI.NameSetting,
TS.NumbSitog,TS.NameSitog, TS.NumbS,TS.NameS
from
--Таблица ТИ СМТН АОПО
(SELECT 
@MAIN_DC as [DC],
dbo.fn_EnNameEquip(EO2.ID) AS [EquipmentName],
EO2.ID as [EquipmentID],
eo2.name as [EnergyObject],
EnObj.ID as [Step],
EnObj.Name as [ElementName],
dbo.ovc_Limiting.OIIDmax as [NumbSetting],--Идентификатор параметра ОИ для для указания динамической уставки Iдл.доп.
dbo.fn_GetNameOI(dbo.ovc_Limiting.OImax, dbo.ovc_Limiting.OIIDmax) as [NameSetting]
FROM dbo.ovc_Limiting INNER JOIN
EnObj ON dbo.ovc_Limiting.ID = EnObj.ID 
INNER JOIN enobj as eo2 on eo2.id = EnObj.higher
WHERE ((dbo.ovc_Limiting.OIcrash IS NOT NULL)) and EnObj.Name like '%АОПО%') as TI
left join
--Таблица ТС СМТН АОПО
(SELECT 
EO2.ID as [EquipmentID],
EnObj.ID as [Step],
EnObj.Name as [ElementName],
 dbo.fn_GetNameOI(Stat.OI, Stat.OIID) as [NameSitog],
Stat.OIID  as [NumbSitog] ,
isnull(F_Stat.Txt,'') as [formula],
f_stat2.sid as [NumbS],
DefTS.Name as [NameS]
FROM ovc_Depend 
INNER JOIN EnObj ON EnObj.ID = ovc_Depend.ID and not (EnObj.Type in (25,33,39,117)) --and (EnObj.Type = 118) 
INNER JOIN enobj as eo2 on eo2.id = EnObj.higher
INNER JOIN EnObj AS EO_Conn ON EnObj.Higher = EO_Conn.id
INNER JOIN enFactorOI AS Stat ON EnObj.ID = Stat.IDEnObj and (Stat.IDFactor = 100)
left JOIN TSFormulas AS F_Stat ON Stat.OIID = F_Stat.Result and F_Stat.OutOfWork=1
left Join TSFormulasR as f_stat2 on f_stat2.FID=F_Stat.ID and f_stat2.OI='S'
left join DefTS on DefTS.ID=f_stat2.sid
) as TS on TI.[EquipmentID]=TS.[EquipmentID] and TI.Step=TS.Step
order by EquipmentName, EnergyObject,NumbSetting

