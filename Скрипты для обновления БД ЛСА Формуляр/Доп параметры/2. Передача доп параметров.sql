use OIK_Tat
go
declare @MAIN_DC varchar(max), @Remote_DC_Filter varchar(max), @Second_DC varchar(max) 
set @MAIN_DC='РДУ Татарстана' -- Заполняется название своего ДЦ
/* --Запрос ДЦ приема\передачи
select distinct RTU.Name
from dtParam6  p INNER JOIN dtSet s on p.SetID=s.ID INNER JOIN RTU ON s.RTUID = RTU.ID INNER JOIN RTU_ICCP I ON s.RTUID = I.RTUID where s.Enable=1 and s.trans=2  and p.OI= 'C'
*/
set @Remote_DC_Filter='ICCP РДУ Самарское' -- Заполняется фрагмент названия узла обмена смежного ДЦ (можно взять из списка, формируемого запросом выше). Для отключения фильтрации = ''.
set @Second_DC='Самарское РДУ'
/******Заполняю таблицы ТИ отдельно из каждой БД********************************************************************/

--INSERT INTO lsa_test..DCTransmit (DCsource, AdressNaborSource, EnergyObject,TypeOI, TypeOTI, NameOtiSource, NumbOtiSource, DCreceiver)

/********* Заполнение СП со стороны ДЦ Источника ***********/
SELECT @MAIN_DC as [ДЦ-источник]
,SND_S.[Addr_Id] as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,'СП' as [Тип ОИ]------!!!!!
,p.Abbr as [Тип]
,iif(DefSParam.OutOfWork =0,'!_ВЫВЕДЕН_! ','')+DefSParam.name as [Наименование ТС в ДЦ-источнике]
,DefSParam.ID as [Номер ТС в ДЦ источнике] 
,@Second_DC as [ДЦ-получатель]
FROM   DefSParam INNER JOIN  EnObj ON DefSParam.EObject = EnObj.ID 
--Inner join  TSCat c on DefSParam.Category=c.ID
inner join  ParTypes p on DefSParam.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join (SELECT SND.ID, Тип, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' Набор ТМ' as Тип, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from  dtParam2 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union 
select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(i.LocalDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(/*iif(Scope=1,'::',i.LocalDomain+'::')+*/cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = 'C')) AS SND_S on DefSParam.ID=SND_S.ID------!!!!!
where /*[ДЦ-получатель]*/ snd_S.RTU like '%'+@Remote_DC_Filter+'%'

union
SELECT @MAIN_DC as [ДЦ-источник]
,SND_S.[Addr_Id] as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,'ПВ' as [Тип ОИ]------!!!!!
,p.Abbr as [Тип]
,DefDRParam.name as [Наименование ТС в ДЦ-источнике]
,DefDRParam.ID as [Номер ТС в ДЦ источнике] 
,@Second_DC as [ДЦ-получатель]
FROM   DefDRParam INNER JOIN  EnObj ON DefDRParam.EObject = EnObj.ID 

inner join  ParTypes p on DefDRParam.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join (SELECT SND.ID, Тип, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' Набор ТМ' as Тип, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from  dtParam2 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(i.LocalDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = 'H')) AS SND_S on DefDRParam.ID=SND_S.ID------!!!!!
where /*[ДЦ-получатель]*/ snd_S.RTU like '%'+@Remote_DC_Filter+'%'

union
SELECT @MAIN_DC as [ДЦ-источник]
,SND_S.[Addr_Id] as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,'МСК' as [Тип ОИ]------!!!!!
,p.Abbr as [Тип]
,iif(DefSIntParam.OutOfWork =0,'!_ВЫВЕДЕН_! ','')+DefSIntParam.name as [Наименование ТС в ДЦ-источнике]
,DefSIntParam.ID as [Номер ТС в ДЦ источнике] 
,@Second_DC as [ДЦ-получатель]
FROM   DefSIntParam INNER JOIN  EnObj ON DefSIntParam.EObject = EnObj.ID 
--Inner join  TSCat c on DefSIntParam.Category=c.ID
inner join  ParTypes p on DefSIntParam.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join (SELECT SND.ID, Тип, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' Набор ТМ' as Тип, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from  dtParam2 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(i.LocalDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(/*iif(Scope=1,'::',i.LocalDomain+'::')+*/cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = 'M')) AS SND_S on DefSIntParam.ID=SND_S.ID------!!!!!
where /*[ДЦ-получатель]*/ snd_S.RTU like '%'+@Remote_DC_Filter+'%'

union
SELECT @MAIN_DC as [ДЦ-источник]
,SND_S.[Addr_Id] as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,'ПЛ' as [Тип ОИ]------!!!!!
,p.Abbr as [Тип]
,DefPlan.name as [Наименование ТС в ДЦ-источнике]
,DefPlan.ID as [Номер ТС в ДЦ источнике] 
,@Second_DC as [ДЦ-получатель]
FROM   DefPlan INNER JOIN  EnObj ON DefPlan.EObject = EnObj.ID 
--Inner join  TSCat c on DefPlan.Category=c.ID
inner join  ParTypes p on DefPlan.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join (SELECT SND.ID, Тип, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' Набор ТМ' as Тип, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from  dtParam2 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(i.LocalDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(/*iif(Scope=1,'::',i.LocalDomain+'::')+*/cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = 'P')) AS SND_S on DefPlan.ID=SND_S.ID------!!!!!
where /*[ДЦ-получатель]*/ snd_S.RTU like '%'+@Remote_DC_Filter+'%'

union
SELECT @MAIN_DC as [ДЦ-источник]
,SND_S.[Addr_Id] as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,'ЕИ' as [Тип ОИ]------!!!!!
,p.Abbr as [Тип]
,DefDayParam.name as [Наименование ТС в ДЦ-источнике]
,DefDayParam.ID as [Номер ТС в ДЦ источнике] 
,@Second_DC as [ДЦ-получатель]
FROM   DefDayParam INNER JOIN  EnObj ON DefDayParam.EObject = EnObj.ID 
--Inner join  TSCat c on DefDayParam.Category=c.ID
inner join  ParTypes p on DefDayParam.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join (SELECT SND.ID, Тип, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' Набор ТМ' as Тип, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from  dtParam2 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(i.LocalDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(/*iif(Scope=1,'::',i.LocalDomain+'::')+*/cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = 'U')) AS SND_S on DefDayParam.ID=SND_S.ID------!!!!!
where /*[ДЦ-получатель]*/ snd_S.RTU like '%'+@Remote_DC_Filter+'%'

union
SELECT @MAIN_DC as [ДЦ-источник]
,SND_S.[Addr_Id] as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,'СВ' as [Тип ОИ]------!!!!!
,p.Abbr as [Тип]
,DEFDRPARAM.name as [Наименование ТС в ДЦ-источнике]
,DEFDRPARAM.ID as [Номер ТС в ДЦ источнике] 
,@Second_DC as [ДЦ-получатель]
FROM   DEFDRPARAM INNER JOIN  EnObj ON DEFDRPARAM.EObject = EnObj.ID 
--Inner join  TSCat c on DEFDRPARAM.Category=c.ID
inner join  ParTypes p on DEFDRPARAM.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join (SELECT SND.ID, Тип, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' Набор ТМ' as Тип, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from  dtParam2 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(i.LocalDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(/*iif(Scope=1,'::',i.LocalDomain+'::')+*/cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = 'W')) AS SND_S on DEFDRPARAM.ID=SND_S.ID------!!!!!
where /*[ДЦ-получатель]*/ snd_S.RTU like '%'+@Remote_DC_Filter+'%'

union
SELECT @MAIN_DC as [ДЦ-источник]
,SND_S.[Addr_Id] as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,'ЧАС' as [Тип ОИ]------!!!!!
,p.Abbr as [Тип]
,vDefUShour1.name as [Наименование ТС в ДЦ-источнике]
,vDefUShour1.ID as [Номер ТС в ДЦ источнике] 
,@Second_DC as [ДЦ-получатель]
FROM   vDefUShour1 INNER JOIN  EnObj ON vDefUShour1.EObject = EnObj.ID 
--Inner join  TSCat c on vDefUShour1.Category=c.ID
inner join  ParTypes p on vDefUShour1.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join (SELECT SND.ID, Тип, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' Набор ТМ' as Тип, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from  dtParam2 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(i.LocalDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' Набор ICCP' as Тип, RTU.Name as RTU,cast(/*iif(Scope=1,'::',i.LocalDomain+'::')+*/cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = 'Л')) AS SND_S on vDefUShour1.ID=SND_S.ID------!!!!!
where /*[ДЦ-получатель]*/ snd_S.RTU like '%'+@Remote_DC_Filter+'%'

order by [ДЦ-получатель], [Адрес в наборе ДЦ-ДЦ]