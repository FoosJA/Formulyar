/*
/******Создание таблицы перелачи ОТИ********************************************************************/
CREATE TABLE DCReception
(DCreceiver varchar(max),
AdressNaborReceiver sql_variant,
EnergyObject varchar(max),
TypeOI varchar(max),
TypeOTI varchar(max),
NameOtiReceiver varchar(max),
NumbOtiReceiver int,
DCsource varchar(max))

*/
-----! ДЛЯ ОДУ НАДО ПРОВЕРЯТЬ!-------
/*select * from lsa_test..DCReception
where DCreceiver like 'ОДУ Средней Волги'*/

use OIK_Tat
go
declare @MAIN_DC varchar(max), @Remote_DC_Filter varchar(max), @Second_DC varchar(max)
set @MAIN_DC='РДУ Татарстана' 
set @Remote_DC_Filter='ICCP РДУ Самарское' 
set @Second_DC='Самарское РДУ'
-- select name from rtu where id in (select RTUid from dtset where SetType=6 and Enable=1) -- Список смежных ДЦ

--INSERT INTO LSA_test..DCReception (DCreceiver, AdressNaborReceiver,  EnergyObject, TypeOI,TypeOTI,NameOtiReceiver,NumbOtiReceiver,DCsource)  
 
SELECT  @MAIN_DC  as [ДЦ-получатель]
,isnull(RCV_I.[Addr_Id],'') as [Адрес в наборе ДЦ-ДЦ]
, et.Abbr+' '+EnObj.Name as [Объект]
, 'ТИ' as [Тип ОИ]
,p.Abbr as [Тип ТИ]
,iif(AllTI.OutOfWork =0,'!_ВЫВЕДЕН_! ','')+allti.name  as [Наименование ТИ в ДЦ-получателе]
,AllTI.ID as [Номер ТИ в ДЦ получателе]
,@Second_DC as [ДЦ-источник]
--,'-' as [Наименование ТИ в ДЦ-источнике],'-' as [Номер ТИ в ДЦ источнике],p.Abbr as [Тип ТИ в источнике]
FROM          AllTI INNER JOIN  EnObj ON AllTI.EObject = EnObj.ID 
inner join  TICat c on allti.Category=c.ID 
LEFT OUTER JOIN EnObj AS EO_Host ON EO_Host.ID = EnObj.Higher 
inner join  ParTypes p on allti.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join (SELECT RCV.ID, Тип, RTU,[Addr_Id],inv FROM(
	  select p.OI ,p.IDOI as id, ' Набор ТМ' as Тип, RTU.Name as RTU,cast(isnull(mask,addr) as varchar) as 'Addr_Id',inv
		from dtParam2 p,dtSet s INNER JOIN RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
		union
		--select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(i.RemoteDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
		 select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(i.RemoteDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
	  --select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(iif(Scope=1,'::',i.RemoteDomain+'::')+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID 
	  where s.Enable=1 and s.trans=1 and rtu.Name not like '%ICCPAVT%'
) AS RCV WHERE (RCV.OI = 'I')) AS RCV_I on AllTI.ID=RCV_I.ID
where /*[ДЦ-источник]*/ RCV_I.RTU like '%'+@Remote_DC_Filter+'%'
--order by [ДЦ-источник], [Адрес в наборе ДЦ-ДЦ]

union
SELECT @MAIN_DC as [ДЦ-получатель] 
,isnull(RCV_S.[Addr_Id],'') as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name as [Объект] 
,'ТС' as [Тип ОИ]
, t.Abbr as [Тип]
,iif(defts.OutOfWork =0,'!_ВЫВЕДЕН_! ','')+defts.name  as [Наименование ТC в ДЦ-получателе] 
,defts.ID as [Номер ТC в ДЦ получателе]
,@Second_DC as [ДЦ-источник] 
FROM         defts INNER JOIN EnObj ON defts.EObject = EnObj.ID Inner join oik..TSCat c on defts.Category=c.ID Inner join oik..TSType t on defts.TSType=t.ID Inner join oik..EnObjType et on EnObj.Type=et.ID
inner join (SELECT RCV.ID, Тип, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' Набор ТМ' as Тип, RTU.Name as RTU,cast(isnull(mask,addr) as varchar) as 'Addr_Id',inv
		from dtParam2 p,dtSet s INNER JOIN RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
		union
		--select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(i.RemoteDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
		select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(i.RemoteDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
	  --select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(iif(Scope=1,'::',i.RemoteDomain+'::')+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  --select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(Identifier as varchar) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID 
	  where s.Enable=1 and s.trans=1 and rtu.Name not like '%ICCPAVT%'
) AS RCV WHERE (RCV.OI = 'S')) AS RCV_S on defts.ID=RCV_S.ID
where /*[ДЦ-источник]*/ RCV_S.RTU like '%'+@Remote_DC_Filter+'%'

order by  [Адрес в наборе ДЦ-ДЦ]
