/*
/******Создание таблицы ОТИ********************************************************************/
CREATE TABLE Lsa_test..AllOTI
(NumbOti int,
NameOti varchar(max),
TypeOI varchar(max),
TypeOTI varchar(max),
Category varchar(max),
EnergyObject varchar(max),
DC varchar(max),
txt  varchar(max),
frm  varchar(max),
TT varchar(max),
TN varchar(max),
MeasTrans varchar(max)
)
/******Очищение таблицы ОТИ********************************************************************/

truncate  table AllOTI
*/

/******Заполняю таблицы отдельно из каждой БД********************************************************************/
use OIK_Tat
go

DECLARE @MAIN_DC varchar(max)
set @MAIN_DC='РДУ Татарстана'

	--INSERT INTO LSA_test..AllOTI (NumbOti,NameOti,TypeOI,TypeOTI,Category,EnergyObject,DC,txt,frm ) 

----ВЫВОД ТИ ДЦ ----
SELECT AllTI.ID as [ID]
, allti.name as [Наименование ОИ]
, 'ТИ' as [Тип ОИ]
, p.Abbr as [Тип]
,TICat.Name as [категория]
,et.Abbr+' '+EnObj.Name +isnull(' ('+RCV_I.RTU+')','') as [Объект]
,@MAIN_DC as [ДЦ] 
,cast(frm.txt as varchar(max))
,cast(frm.Frml as varchar(max))
FROM       AllTI INNER JOIN  EnObj ON AllTI.EObject = EnObj.ID   
inner join  ParTypes p on AllTI.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
left join (SELECT RCV.ID, Тип, RTU,[Addr_Id],inv FROM(
      SELECT 'I'  as 'OI', DefTI.ID as ID,' ТМ' as Тип,RTU.Name as RTU,addr as 'Addr_Id',inv FROM DefTI INNER JOIN RTU ON DefTI.RTUID = RTU.ID
union SELECT 'S', DefTS.ID,' ТМ' as Тип, 	RTU.Name,mask as 'Addr_Id', inv FROM DefTS INNER JOIN RTU ON DefTS.RTUID = RTU.ID
union SELECT p.OI, p.IDOI, ' Набор ТМ', 	RTU.Name,isnull(mask,addr) as 'Addr_Id'	, inv FROM dtParam2 AS p INNER JOIN dtSet AS s ON p.SetID = s.ID INNER JOIN RTU ON s.RTUID = RTU.ID WHERE (s.Enable = 1) AND (s.Trans = 1)
union select p.OI ,p.IDOI, ' Набор ММО', 	RTU.Name,ExtIDOI, iif(ScaleFactor<0,1,0) as inv from dtParam3 p,dtSet s INNER JOIN RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=1
) AS RCV WHERE (RCV.OI = 'I')) AS RCV_I on AllTI.ID=RCV_I.ID
inner join TICat on allti.Category=TICat.ID 

 LEFT OUTER  JOIN
  ( select  Frml,Result,txt from TIFormulas
 left join (select distinct OI,FID from TIFormulasR) as TIFormulasR on TIFormulas.ID=TIFormulasR.FID where Ftype=0)
   as frm oN frm.Result = AllTI.ID
where AllTI.OutOfWork <>0 

union 
----ВЫВОД ТС ДЦ
SELECT DefTS.ID as [ID]
,defts.name as [Наименование ОИ]
,'ТС' as [Тип ОИ]
, t.Abbr as [Тип]
,TSCat.Name as [категория]
,et.Abbr+' '+EnObj.Name +isnull(' ('+RCV_S.RTU+')','') as [Объект] 
,@MAIN_DC as [ДЦ]
,cast(frm.txt as varchar(max))
,cast(frm.Frml as varchar(max))
FROM   DefTS INNER JOIN  EnObj ON defts.EObject = EnObj.ID
Inner join  TSCat c on defts.Category=c.ID 
Inner join  TSType t on defts.TSType=t.ID 
Inner join  EnObjType et on EnObj.Type=et.ID
left join (SELECT RCV.ID, Тип, RTU,[Addr_Id],[inv] FROM(
      SELECT 'I'  as 'OI', DefTI.ID as ID,' ТМ' as Тип,RTU.Name as RTU,addr as 'Addr_Id',inv FROM DefTI INNER JOIN RTU ON DefTI.RTUID = RTU.ID
union SELECT 'S' , DefTS.ID,' ТМ' as Тип, 	RTU.Name,mask as 'Addr_Id', inv FROM DefTS INNER JOIN RTU ON DefTS.RTUID = RTU.ID
union SELECT p.OI, p.IDOI, ' Набор ТМ', 	RTU.Name,isnull(mask,addr) as 'Addr_Id'	, inv FROM dtParam2 AS p INNER JOIN dtSet AS s ON p.SetID = s.ID INNER JOIN RTU ON s.RTUID = RTU.ID WHERE (s.Enable = 1) AND (s.Trans = 1)
union select p.OI ,p.IDOI, ' Набор ММО', 	RTU.Name,ExtIDOI, iif(ScaleFactor<0,1,0) as inv from dtParam3 p,dtSet s INNER JOIN RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=1
) AS RCV WHERE (RCV.OI = 'S')) AS RCV_S on defts.ID=RCV_S.ID
 Inner join TSCat on defts.Category=TSCat.ID
 LEFT OUTER  JOIN
  ( select  Frml,Result,txt,TSFormulasR.oi from TSFormulas
 left join (select distinct OI,FID from TSFormulasR) as TSFormulasR on TSFormulas.ID=TSFormulasR.FID)
   as frm oN frm.Result = defts.ID
 where defts.OutOfWork <>0
order by [Тип ОИ],  ID


   