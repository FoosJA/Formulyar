/*
/******Создание таблицы МУН********************************************************************/
CREATE TABLE LSA_test..SMTN_Transform
(DC varchar(max),
EnergyObject varchar(max),
NameTransform varchar(max),
IDTransform int,
Winding varchar(max),
IDWinding int,
NameOIfact varchar(max),
NumbOIfact int,
formula varchar(max),
formulaKod varchar(max),
NameRpn varchar(max),
NumbRpn int,
NameTNV varchar(max),
NumbTNV int,
NameTS varchar(max),
NumbTS int,
)

*/
use OIK
go

--INSERT INTO LSA_test..SMTN_Transform (DC,EnergyObject,NameTransform ,IDTransform, Winding ,IDWinding ,NameOIfact,
--NumbOIfact,formula ,formulaKod,NameRpn ,NumbRpn ,NameTNV ,NumbTNV ,NameTS ,NumbTS )

--запрос для таблицы СМТН для АТ
SELECT 'ОДУ Средней Волги' as DC
--,EO_Host.Abbr as [Объект размещения] --для РДУ
, (select Abbr from EnObj where EnObj.id = EO_Host.higher)as [Объект размещения]
,EnObj.Name as [Имя AT(T)]
,EnObj.ID as [id AT(T)]
,EO_Obm.Name AS [OBM]
,EO_Obm.ID as [id OBM]
,a1.Name as [Имя Iф]
,I1.OIID AS [Iф]
, UPPER(replace(replace(CAST(isnull(fi.txt,'') AS VARCHAR(MAX)),CHAR(13),' '),CHAR(10),' ')) as [формула]
,UPPER(replace(replace(CAST(isnull(fi.Frml,'') AS VARCHAR(MAX)),CHAR(13),' '),CHAR(10),' ')) as [код формулы]
,a3.Name as [Имя RPN]
,isnull(RPN.OIID,'') AS [RPN]
,a2.Name as [имя TNV]
,isnull(TNV.OIID ,'') AS [TNV]
,isnull(d1.Name,'') as [имя TS]
,isnull(Stat.OIID,'') as [TS]
FROM ovc_PowerTransformer 
INNER JOIN EnObj AS EnObj ON ovc_PowerTransformer.ID = EnObj.ID
INNER JOIN ovc_Depend ON ovc_PowerTransformer.ID = ovc_Depend.Higher
LEFT OUTER JOIN EnObj AS EO_Host ON EO_Host.ID = EnObj.Higher 
LEFT OUTER JOIN EnObj AS EO_Obm ON EO_Obm.ID = ovc_Depend.ID 
LEFT OUTER JOIN enFactorOI AS I1 ON EO_Obm.ID = I1.IDEnObj and (I1.IDFactor = 12)
LEFT OUTER JOIN TIFormulas AS FI ON I1.OIID =result and FI.OutOfWork=1
left OUTER JOIN allti as A1 on a1.id=I1.OIID
LEFT OUTER JOIN enFactorOI AS RPN ON ovc_PowerTransformer.ID = RPN.IDEnObj and (RPN.IDFactor = 107)
left OUTER JOIN allti as a3 on a3.id=RPN.OIID
LEFT OUTER JOIN enFactorOI AS TNV ON ovc_PowerTransformer.ID = TNV.IDEnObj and (TNV.IDFactor = 11)
left OUTER JOIN allti as A2 on a2.id=TNV.OIID
LEFT OUTER JOIN enFactorOI AS Stat ON ovc_PowerTransformer.ID = Stat.IDEnObj and (Stat.IDFactor = 100)
LEFT OUTER JOIN DefTS AS d1 ON d1.ID = Stat.OIID
ORDER BY EnObj.Voltage DESC, EnObj.Name, EO_Obm.Voltage DESC
