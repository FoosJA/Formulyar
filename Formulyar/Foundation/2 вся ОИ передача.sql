
declare @MAIN_DC varchar(max)
set @MAIN_DC='РДУ Татарстана' -- Заполняется название своего ДЦ

--INSERT INTO lsa_test..DCTransmit (DCsource, AdressNaborSource, EnergyObject,TypeOI, TypeOTI, NameOtiSource, NumbOtiSource, DCreceiver)

SELECT @MAIN_DC as [ДЦ-источник] 
,isnull(snd_I.[Addr_Id],'') as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name +isnull(' ('+RCV_I.RTU+')','') as [Объект] 
, 'ТИ' as [Категория]
,p.Abbr as [Тип]
,iif(AllTI.OutOfWork =0,'!_ВЫВЕДЕН_! ','')+allti.name as [Наименование ТИ в ДЦ-источнике]
,AllTI.ID as [Номер ОИ в ДЦ источнике]
,snd_I.RTU as [ДЦ-приемник] 
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

union 

SELECT @MAIN_DC as [ДЦ-источник]
,SND_S.[Addr_Id] as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name +isnull(' ('+RCV_S.RTU+')','') as [Объект] 
,'ТС' as [Категория]
, t.Abbr as [Тип]
,iif(defts.OutOfWork =0,'!_ВЫВЕДЕН_! ','')+defts.name as [Наименование ТС в ДЦ-источнике]
,defts.ID as [Номер ОИ в ДЦ источнике] 
,SND_S.RTU as [ДЦ-приемник] 
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

union

SELECT @MAIN_DC as [ДЦ-источник]
,SND_S.[Addr_Id] as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,'ИС' as [Категория]
,p.Abbr as [Тип]
,DEFDRPARAM.name as [Наименование ТС в ДЦ-источнике]
,DEFDRPARAM.ID as [Номер ОИ в ДЦ источнике] 
,SND_S.RTU as [ДЦ-приемник] 
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
) AS SND WHERE (snd.OI = 'J')) AS SND_S on DEFDRPARAM.ID=SND_S.ID------!!!!!

union

SELECT @MAIN_DC as [ДЦ-источник]
,SND_S.[Addr_Id] as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,'СВ' as [Категория]
,p.Abbr as [Тип]
,DEFDRPARAM.name as [Наименование ТС в ДЦ-источнике]
,DEFDRPARAM.ID as [Номер ОИ в ДЦ источнике] 
,SND_S.RTU as [ДЦ-приемник] 
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

union

SELECT @MAIN_DC as [ДЦ-источник]
,SND_S.[Addr_Id] as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,'ПЛ' as [Категория]
,p.Abbr as [Тип]
,DefPlan.name as [Наименование ТС в ДЦ-источнике]
,DefPlan.ID as [Номер ОИ в ДЦ источнике] 
,SND_S.RTU as [ДЦ-приемник] 
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

union

SELECT @MAIN_DC as [ДЦ-источник]
,SND_S.[Addr_Id] as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,'ЕИ' as [Категория]
,p.Abbr as [Тип]
,DefDayParam.name as [Наименование ТС в ДЦ-источнике]
,DefDayParam.ID as [Номер ОИ в ДЦ источнике] 
,SND_S.RTU as [ДЦ-приемник] 
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

UNION

SELECT @MAIN_DC as [ДЦ-источник]
,SND_S.[Addr_Id] as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,'СП' as [Категория]
,p.Abbr as [Тип]
,iif(DefSParam.OutOfWork =0,'!_ВЫВЕДЕН_! ','')+DefSParam.name as [Наименование ТС в ДЦ-источнике]
,DefSParam.ID as [Номер ОИ в ДЦ источнике] 
,SND_S.RTU as [ДЦ-приемник] 
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
) AS SND WHERE (snd.OI = 'C')) AS SND_S on DefSParam.ID=SND_S.ID------!!

union

SELECT @MAIN_DC as [ДЦ-источник]
,SND_S.[Addr_Id] as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,'ПВ' as [Категория]
,p.Abbr as [Тип]
,DefDRParam.name as [Наименование ТС в ДЦ-источнике]
,DefDRParam.ID as [Номер ОИ в ДЦ источнике] 
,SND_S.RTU as [ДЦ-приемник] 
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
--where /*[ДЦ-получатель]*/ snd_S.RTU like '%'+@Remote_DC_Filter+'%'

union

SELECT @MAIN_DC as [ДЦ-источник]
,SND_S.[Addr_Id] as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,'МСК' as [Категория]
,p.Abbr as [Тип]
,iif(DefSIntParam.OutOfWork =0,'!_ВЫВЕДЕН_! ','')+DefSIntParam.name as [Наименование ТС в ДЦ-источнике]
,DefSIntParam.ID as [Номер ОИ в ДЦ источнике] 
,SND_S.RTU as [ДЦ-приемник] 
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

union

SELECT @MAIN_DC as [ДЦ-источник]
,SND_S.[Addr_Id] as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,'МИН' as [Категория]
,p.Abbr as [Тип]
,vDefUShour1.name as [Наименование ТС в ДЦ-источнике]
,vDefUShour1.ID as [Номер ОИ в ДЦ источнике] 
,SND_S.RTU as [ДЦ-приемник] 
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
) AS SND WHERE (snd.OI = 'Б')) AS SND_S on vDefUShour1.ID=SND_S.ID------!!!!!

union

SELECT @MAIN_DC as [ДЦ-источник]
,SND_S.[Addr_Id] as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,'ПМИН' as [Категория]
,p.Abbr as [Тип]
,vDefUShour1.name as [Наименование ТС в ДЦ-источнике]
,vDefUShour1.ID as [Номер ОИ в ДЦ источнике] 
,SND_S.RTU as [ДЦ-приемник] 
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
) AS SND WHERE (snd.OI = 'Г')) AS SND_S on vDefUShour1.ID=SND_S.ID------!!!!!

union

SELECT @MAIN_DC as [ДЦ-источник]
,SND_S.[Addr_Id] as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,'ДМИН' as [Категория]
,p.Abbr as [Тип]
,vDefUShour1.name as [Наименование ТС в ДЦ-источнике]
,vDefUShour1.ID as [Номер ОИ в ДЦ источнике] 
,SND_S.RTU as [ДЦ-приемник] 
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
) AS SND WHERE (snd.OI = 'З')) AS SND_S on vDefUShour1.ID=SND_S.ID------!!!!!

union

SELECT @MAIN_DC as [ДЦ-источник]
,SND_S.[Addr_Id] as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,'ЧЧАС' as [Категория]
,p.Abbr as [Тип]
,vDefUShour1.name as [Наименование ТС в ДЦ-источнике]
,vDefUShour1.ID as [Номер ОИ в ДЦ источнике] 
,SND_S.RTU as [ДЦ-приемник] 
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
) AS SND WHERE (snd.OI = 'И')) AS SND_S on vDefUShour1.ID=SND_S.ID------!!!!!

union

SELECT @MAIN_DC as [ДЦ-источник]
,SND_S.[Addr_Id] as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,'ПЧАС' as [Категория]
,p.Abbr as [Тип]
,vDefUShour1.name as [Наименование ТС в ДЦ-источнике]
,vDefUShour1.ID as [Номер ОИ в ДЦ источнике] 
,SND_S.RTU as [ДЦ-приемник] 
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
) AS SND WHERE (snd.OI = 'К')) AS SND_S on vDefUShour1.ID=SND_S.ID------!!!!!

union

SELECT @MAIN_DC as [ДЦ-источник]
,SND_S.[Addr_Id] as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,'ПЧАС' as [Категория]
,p.Abbr as [Тип]
,vDefUShour1.name as [Наименование ТС в ДЦ-источнике]
,vDefUShour1.ID as [Номер ОИ в ДЦ источнике] 
,SND_S.RTU as [ДЦ-приемник] 
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
) AS SND WHERE (snd.OI = 'К')) AS SND_S on vDefUShour1.ID=SND_S.ID------!!!!!

union

SELECT @MAIN_DC as [ДЦ-источник]
,SND_S.[Addr_Id] as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,'ЧАС' as [Категория]
,p.Abbr as [Тип]
,vDefUShour1.name as [Наименование ТС в ДЦ-источнике]
,vDefUShour1.ID as [Номер ОИ в ДЦ источнике] 
,SND_S.RTU as [ДЦ-приемник] 
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

union

SELECT @MAIN_DC as [ДЦ-источник]
,SND_S.[Addr_Id] as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,'СУТ' as [Категория]
,p.Abbr as [Тип]
,vDefUShour1.name as [Наименование ТС в ДЦ-источнике]
,vDefUShour1.ID as [Номер ОИ в ДЦ источнике] 
,SND_S.RTU as [ДЦ-приемник] 
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
) AS SND WHERE (snd.OI = 'П')) AS SND_S on vDefUShour1.ID=SND_S.ID------!!!!!

union

SELECT @MAIN_DC as [ДЦ-источник]
,SND_S.[Addr_Id] as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,'МЕС' as [Категория]
,p.Abbr as [Тип]
,vDefUShour1.name as [Наименование ТС в ДЦ-источнике]
,vDefUShour1.ID as [Номер ОИ в ДЦ источнике] 
,SND_S.RTU as [ДЦ-приемник] 
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
) AS SND WHERE (snd.OI = 'У')) AS SND_S on vDefUShour1.ID=SND_S.ID------!!!!!

union

SELECT @MAIN_DC as [ДЦ-источник]
,SND_S.[Addr_Id] as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,'ТМИН' as [Категория]
,p.Abbr as [Тип]
,vDefUShour1.name as [Наименование ТС в ДЦ-источнике]
,vDefUShour1.ID as [Номер ОИ в ДЦ источнике] 
,SND_S.RTU as [ДЦ-приемник] 
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
) AS SND WHERE (snd.OI = 'Ф')) AS SND_S on vDefUShour1.ID=SND_S.ID------!!!!!

union

SELECT @MAIN_DC as [ДЦ-источник]
,SND_S.[Addr_Id] as [Адрес в наборе ДЦ-ДЦ]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,'ТМИН' as [Категория]
,p.Abbr as [Тип]
,vDefUShour1.name as [Наименование ТС в ДЦ-источнике]
,vDefUShour1.ID as [Номер ОИ в ДЦ источнике] 
,SND_S.RTU as [ДЦ-приемник] 
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
) AS SND WHERE (snd.OI = 'Ф')) AS SND_S on vDefUShour1.ID=SND_S.ID------!!!!!

order by [Категория],[Номер ОИ в ДЦ источнике], [Адрес в наборе ДЦ-ДЦ]