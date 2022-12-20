/*
/******Создание таблицы МУН********************************************************************/
CREATE TABLE LSA_test..SMTN_Breaker
(DC varchar(max),
EnergyObject varchar(max),
NameBreaker varchar(max),
IDBreaker int,
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
NameTS varchar(max),
NumbTS int
)
*/
use OIK
go

--INSERT INTO LSA_test..SMTN_Breaker (DC ,EnergyObject,NameBreaker,IDBreaker,NameOIfact,NumbOIfact,formula,
--formulaKod,NameOIa ,NumbOIa ,NameOIb,NumbOIb,NameOI,NumbOIc ,NameTNV ,NumbTNV ,NameTS,NumbTS )

--запрос для таблицы СМТН для Выкл
SELECT  'ОДУ Средней Волги'
--,EO_Host.Abbr  as [EnergyObj] --для РДУ
 ,(select Abbr from enobj where enobj.id =(select higher from enobj where enobj.id = EO_Host.higher)) as [EnergyObj]
 , EnObj.Name as [Имя контролируемого объекта]
 ,ovc_Breaker.ID
 ,a1.Name as [Имя I1]
 ,I1.OIID AS [I1]
,UPPER(replace(replace(CAST(isnull(fi.txt,'') AS VARCHAR(MAX)),CHAR(13),' '),CHAR(10),' ')) as [формула]
,UPPER(replace(replace(CAST(isnull(fi.Frml,'') AS VARCHAR(MAX)),CHAR(13),' '),CHAR(10),' ')) as [код формулы]
,isnull(Aa.Name,'') AS [имя Ifa],isnull(FIa.OIID,'') AS [Ia] 
,isnull(Ab.Name,'') AS [имя Ifb],isnull(FIb.OIID,'') AS [Ib] 
,isnull(Ac.Name,'') AS [имя Ifc],isnull(FIc.OIID,'') AS [Ic] 
,a2.Name as [имя TNV],isnull(TNV.OIID,'') AS [TNV]
,isnull(d1.Name,'') as [имя TS_Stat],isnull(Stat.OIID,'') as [TS_Stat]
FROM ovc_Breaker 
INNER JOIN EnObj ON ovc_Breaker.ID = EnObj.ID
INNER JOIN EnObj AS EO_Host ON EO_Host.ID = EnObj.Higher
LEFT OUTER JOIN enFactorOI AS I1 ON ovc_Breaker.ID = I1.IDEnObj and (I1.IDFactor = 12)
left OUTER JOIN allti as A1 on a1.id=I1.OIID
LEFT OUTER JOIN TIFormulas AS FI ON I1.OIID =result and FI.OutOfWork=1
LEFT OUTER JOIN enFactorOI as FIa ON ovc_Breaker.ID = FIa.IDEnObj AND (FIa.IDFactor in (108))
left OUTER JOIN allti as Aa on Aa.id=FIa.OIID
LEFT OUTER JOIN enFactorOI as FIb ON ovc_Breaker.ID = FIb.IDEnObj AND (FIb.IDFactor in (109))
left OUTER JOIN allti as Ab on Ab.id=FIb.OIID
LEFT OUTER JOIN enFactorOI as FIc ON ovc_Breaker.ID = FIc.IDEnObj AND (FIc.IDFactor in (110))
left OUTER JOIN allti as Ac on Ac.id=FIc.OIID
LEFT OUTER JOIN enFactorOI AS TNV ON ovc_Breaker.ID = TNV.IDEnObj and (TNV.IDFactor = 11)
left OUTER JOIN allti as A2 on a2.id=TNV.OIID
LEFT OUTER JOIN enFactorOI AS Stat ON ovc_Breaker.ID = Stat.IDEnObj and (Stat.IDFactor = 100)
LEFT OUTER JOIN DefTS AS d1 ON d1.ID = Stat.OIID
order by EnObj.Voltage desc ,[EnergyObj],EnObj.name, [I1]