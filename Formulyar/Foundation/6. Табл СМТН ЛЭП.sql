/*
/******Создание таблицы МУН********************************************************************/
CREATE TABLE LSA_test..SMTN_Line
(DC varchar(max),
NameLine varchar(max),
IdLine int,
EnergyObject varchar(max),
IdEnOb int,
NameOIfact varchar(max),
NumbOIfact int,
formula varchar(max),
formulaKod varchar(max),
NameOIa varchar(max),
NumbOIa int,
NameOIb varchar(max),
NumbOIb int,
NameOI varchar(max),
NumbOIc int,
NameTNV varchar(max),
NumbTNV int,
NameTSstat varchar(max),
NumbTSstat int,
NameTSstatVL varchar(max),
NumbTSstatVL int
)
*/
--UPDATE LSA_test..SMTN_Line SET EnergyObject = REPLACE (EnergyObject, 'Присоединение', ''); --У САРАТОВА ПРОВЕРЯТЬ ЭТОТ пункт

use OIK
go

--INSERT INTO LSA_test..SMTN_Line (DC, NameLine , IdLine, EnergyObject,IdEnOb ,NameOIfact ,NumbOIfact ,formula ,
--formulaKod ,NameOIa ,NumbOIa ,NameOIb ,NumbOIb ,NameOI ,NumbOIc ,NameTNV ,NumbTNV ,NameTSstat ,
--NumbTSstat ,NameTSstatVL,NumbTSstatVL )

SELECT 'ОДУ Средней Волги'
,EnObj.Name as [Объект контроля]
,[ovc_Line].id
,EO_Conn.Name AS [Присоединение]
, EO_Conn.ID 
,a1.Name as [Имя Iф]
,I1.OIID AS [№ Iф]
,UPPER(replace(replace(CAST(isnull(fi.txt,'') AS VARCHAR(MAX)),CHAR(13),' '),CHAR(10),' ')) as [формула]
,UPPER(replace(replace(CAST(isnull(fi.Frml,'') AS VARCHAR(MAX)),CHAR(13),' '),CHAR(10),' ')) as [код формулы]
,isnull(Aa.Name,'') AS [имя Ifa], isnull(FIa.OIID,'') AS [Ifa]
,isnull(Ab.Name,'') AS [имя Ifb], isnull(FIb.OIID,'') AS [Ifb]
,isnull(Ac.Name,'') AS [имя Ifc], isnull(FIc.OIID ,'') AS [Ifc]
,a2.Name as [имя TNV], isnull(TNV.OIID,'') AS [TNV]
,isnull(d1.Name,'') as [имя TS_Stat], isnull(Stat.OIID,'') as [TS_Stat]
,isnull(d2.Name,'') as [имя TS_Stat_VL],isnull(Stat_VL.OIID,'') as [TS_Stat_VL]
FROM [ovc_Line] INNER JOIN EnObj on EnObj.ID = [ovc_Line].id
INNER JOIN ovc_Depend AS EO1 ON EnObj.ID = EO1.ID 
LEFT OUTER JOIN EnObj AS EO_Conn ON EnObj.ID = EO_Conn.Higher AND EO_Conn.Type = 117
LEFT OUTER JOIN enFactorOI AS I1 ON EO_Conn.ID = I1.IDEnObj and (I1.IDFactor = 12)
LEFT OUTER JOIN TIFormulas AS FI ON I1.OIID =result and Ftype=0
left OUTER JOIN TIFormulasR ON TIFormulasR.FID = FI.ID --and isnull(TIFormulasR.OI,'')<>'S'
left OUTER JOIN allti as A1 on a1.id=I1.OIID
left OUTER JOIN defts as S2 on S2.id=TIFormulasR.SID and TIFormulasR.OI='S'
LEFT OUTER JOIN enFactorOI as FIa ON EO_Conn.ID = FIa.IDEnObj AND (FIa.IDFactor in (108))
left OUTER JOIN allti as Aa on Aa.id=FIa.OIID
LEFT OUTER JOIN enFactorOI as FIb ON EO_Conn.ID = FIb.IDEnObj AND (FIb.IDFactor in (109))
left OUTER JOIN allti as Ab on Ab.id=FIb.OIID
LEFT OUTER JOIN enFactorOI as FIc ON EO_Conn.ID = FIc.IDEnObj AND (FIc.IDFactor in (110))
left OUTER JOIN allti as Ac on Ac.id=FIc.OIID
LEFT OUTER JOIN enFactorOI AS TNV ON EO_Conn.ID = TNV.IDEnObj and (TNV.IDFactor = 11)
left OUTER JOIN allti as A2 on a2.id=TNV.OIID
LEFT OUTER JOIN enFactorOI AS Stat ON EO_Conn.ID = Stat.IDEnObj and (Stat.IDFactor = 100)
LEFT OUTER JOIN enFactorOI AS Stat_VL ON EnObj.ID = Stat_VL.IDEnObj and (Stat_VL.IDFactor = 100)
LEFT OUTER JOIN DefTS AS d1 ON d1.ID = Stat.OIID
LEFT OUTER JOIN DefTS AS d2 ON d2.ID = Stat_VL.OIID
WHERE (EO1.Higher IS NULL) and [ovc_Line].InControl=1
order by EnObj.Voltage desc ,EnObj.name, EO_Conn.Name
