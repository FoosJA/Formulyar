/*
/******Создание таблицы перелачи ОТИ********************************************************************/
CREATE TABLE DCTransmit
(DCsource varchar(max),
AdressNaborSource sql_variant,
EnergyObject varchar(max),
TypeOI varchar(max),
TypeOTI varchar(max),
NameOtiSource varchar(max),
NumbOtiSource int,
DCreceiver varchar(max))

/******Очищение таблицы примема ОТИ********************************************************************/

truncate  table DCTransmit

select *
delete
from lsa_test..DCTransmit
where  DCreceiver like 'ОДУ Средней Волги' and DCsource like 'Пензенское РДУ'
*/

use OIK_Tat
go
declare @MAIN_DC varchar(max), @Remote_DC_Filter varchar(max), @Second_DC varchar(max) 
set @MAIN_DC='РДУ Татарстана' -- Заполняется название своего ДЦ
-- select name from rtu where id in (select RTUid from dtset where SetType=6 and Enable=1) -- Список смежных ДЦ
set @Remote_DC_Filter='ICCP РДУ Самарское' -- Заполняется фрагмент названия узла обмена смежного ДЦ (можно взять из списка, формируемого запросом выше). Для отключения фильтрации = ''.
set @Second_DC='Самарское РДУ'
/******Заполняю таблицы ТИ отдельно из каждой БД********************************************************************/

--INSERT INTO lsa_test..DCTransmit (DCsource, AdressNaborSource, EnergyObject,TypeOI, TypeOTI, NameOtiSource, NumbOtiSource, DCreceiver)

/********* Заполнение ТИ со стороны ДЦ Источника ***********/
SELECT @MAIN_DC as [ДЦ-источник] 
,isnull(snd_I.[Addr_Id],'') as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name +isnull(' ('+RCV_I.RTU+')','') as [Объект] 
, 'ТИ'
,p.Abbr as [Тип]
,iif(AllTI.OutOfWork =0,'!_ВЫВЕДЕН_! ','')+allti.name as [Наименование ТИ в ДЦ-источнике]
,AllTI.ID as [Номер ТИ в ДЦ источнике]
--,isnull(snd_I.RTU,'') as [ДЦ-получатель]
,@Second_DC as [ДЦ-получатель]
FROM         AllTI INNER JOIN EnObj ON AllTI.EObject = EnObj.ID 
inner join  TICat c on allti.Category=c.ID 
inner join  ParTypes p on allti.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
left join (SELECT RCV.ID, Тип, RTU,[Addr_Id],inv FROM(
      SELECT 'I'  as 'OI', DefTI.ID as ID,' ТМ' as Тип,RTU.Name as RTU,addr as 'Addr_Id',inv FROM DefTI INNER JOIN RTU ON DefTI.RTUID = RTU.ID
union SELECT 'S', DefTS.ID,' ТМ' as Тип, 	RTU.Name,mask as 'Addr_Id', inv FROM DefTS INNER JOIN RTU ON DefTS.RTUID = RTU.ID
union SELECT p.OI, p.IDOI, ' Набор ТМ', 	RTU.Name,isnull(mask,addr) as 'Addr_Id'	, inv FROM dtParam2 AS p INNER JOIN dtSet AS s ON p.SetID = s.ID INNER JOIN RTU ON s.RTUID = RTU.ID WHERE (s.Enable = 1) AND (s.Trans = 1)
union select p.OI ,p.IDOI, ' Набор ММО', 	RTU.Name,ExtIDOI, iif(ScaleFactor<0,1,0) as inv from dtParam3 p,dtSet s INNER JOIN RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=1
) AS RCV WHERE (RCV.OI = 'I')) AS RCV_I on AllTI.ID=RCV_I.ID
inner join (SELECT SND.ID, Тип, RTU,[Addr_Id],inv FROM(
select		p.OI ,p.IDOI as id, ' Набор ТМ' as Тип, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from dtParam2 p,dtSet s INNER JOIN RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union --select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(i.LocalDomain+':'+/*cast(s.SetID as varchar)+*/':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv -- с добавлением № набора
		select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(/*iif(Scope=1,'::',i.LocalDomain+'::')+*/cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from dtParam6 p INNER JOIN dtSet s on p.SetID=s.ID INNER JOIN RTU ON s.RTUID = RTU.ID INNER JOIN RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = 'I')) AS SND_I on AllTI.ID=SND_I.ID
where /*[ДЦ-получатель]*/ snd_I.RTU like+@Remote_DC_Filter
--order by [ДЦ-получатель], [Адрес в наборе ДЦ-ДЦ]

union 

SELECT @MAIN_DC as [ДЦ-источник]
,SND_S.[Addr_Id] as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name +isnull(' ('+RCV_S.RTU+')','') as [Объект] 
,'ТС' as [Тип ОИ]
, t.Abbr as [Тип]
,iif(defts.OutOfWork =0,'!_ВЫВЕДЕН_! ','')+defts.name as [Наименование ТС в ДЦ-источнике]
,defts.ID as [Номер ТС в ДЦ источнике] 
,@Second_DC as [ДЦ-получатель]
FROM   defts INNER JOIN  EnObj ON defts.EObject = EnObj.ID 
Inner join  TSCat c on defts.Category=c.ID
Inner join  TSType t on defts.TSType=t.ID 
Inner join  EnObjType et on EnObj.Type=et.ID
left join (SELECT RCV.ID, Тип, RTU,[Addr_Id],[inv] 
			FROM( SELECT 'I'  as 'OI', DefTI.ID as ID,' ТМ' as Тип, RTU.Name as RTU,addr as 'Addr_Id',inv 
					FROM  DefTI INNER JOIN  RTU ON DefTI.RTUID = RTU.ID
union SELECT 'S' , DefTS.ID,' ТМ' as Тип, 	RTU.Name,mask as 'Addr_Id', inv FROM  DefTS INNER JOIN  RTU ON DefTS.RTUID = RTU.ID
union SELECT p.OI, p.IDOI, ' Набор ТМ', 	RTU.Name,isnull(mask,addr) as 'Addr_Id'	, inv FROM  dtParam2 AS p INNER JOIN  dtSet AS s ON p.SetID = s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID WHERE (s.Enable = 1) AND (s.Trans = 1)
union select p.OI ,p.IDOI, ' Набор ММО', 	RTU.Name,ExtIDOI, iif(ScaleFactor<0,1,0) as inv from  dtParam3 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=1
) AS RCV WHERE (RCV.OI = 'S')) AS RCV_S on defts.ID=RCV_S.ID
inner join (SELECT SND.ID, Тип, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' Набор ТМ' as Тип, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from  dtParam2 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union 
select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(i.LocalDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
-- p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(/*iif(Scope=1,'::',i.LocalDomain+'::')+*/cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = 'S')) AS SND_S on defts.ID=SND_S.ID
where /*[ДЦ-получатель]*/ snd_S.RTU like '%'+@Remote_DC_Filter+'%'
order by [ДЦ-получатель], [Адрес в наборе ДЦ-ДЦ]