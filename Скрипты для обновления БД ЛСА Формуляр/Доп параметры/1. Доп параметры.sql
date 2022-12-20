use OIK
go

DECLARE @MAIN_DC varchar(max)
set @MAIN_DC='ОДУ Средней Волги'

	--INSERT INTO LSA_test..AllOTI (NumbOti,NameOti,TypeOI,TypeOTI,Category,EnergyObject,DC ) 

SELECT DefSParam.ID as [ID]
,DefSParam.name as [Наименование ОИ]
,'СП' as [Тип ОИ]
, p.Abbr as [Тип]
,'Специальные параметры вещественные' as [категория]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,@MAIN_DC as [ДЦ]
FROM   DefSParam INNER JOIN  EnObj ON DefSParam.EObject = EnObj.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join  ParTypes p on DefSParam.Type=p.ID 
 where DefSParam.OutOfWork <>0 

union
SELECT DefDRParam.ID as [ID]
,DefDRParam.name as [Наименование ОИ]
,'ПВ' as [Тип ОИ]
, p.Abbr as [Тип]
,'СВ-2 (усредненная)' as [категория]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,@MAIN_DC as [ДЦ]
FROM   DefDRParam INNER JOIN  EnObj ON DefDRParam.EObject = EnObj.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join  ParTypes p on DefDRParam.Type=p.ID 


union
SELECT DefSIntParam.ID as [ID]
,DefSIntParam.name as [Наименование ОИ]
,'МСК' as [Тип ОИ]
, p.Abbr as [Тип]
,'Специальные параметры целочисленные' as [категория]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,@MAIN_DC as [ДЦ]
FROM   DefSIntParam INNER JOIN  EnObj ON DefSIntParam.EObject = EnObj.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join  ParTypes p on DefSIntParam.Type=p.ID 
 where DefSIntParam.OutOfWork <>0 

 union
 SELECT DefPlan.ID as [ID]
,DefPlan.name as [Наименование ОИ]
,'ПЛ' as [Тип ОИ]
, p.Abbr as [Тип]
,'Планы' as [категория]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,@MAIN_DC as [ДЦ]
FROM   DefPlan INNER JOIN  EnObj ON DefPlan.EObject = EnObj.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join  ParTypes p on DefPlan.Type=p.ID 
 
 union
SELECT DefDayParam.ID as [ID]
,DefDayParam.name as [Наименование ОИ]
,'ЕИ' as [Тип ОИ]
, p.Abbr as [Тип]
,'Ежедневная информация' as [категория]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,@MAIN_DC as [ДЦ]
FROM   DefDayParam INNER JOIN  EnObj ON DefDayParam.EObject = EnObj.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join  ParTypes p on DefDayParam.Type=p.ID 
 
 union
SELECT DEFDRPARAM.ID as [ID]
,DEFDRPARAM.name as [Наименование ОИ]
,'СВ' as [Тип ОИ]
, p.Abbr as [Тип]
,'СВ-1 (мгновенная,СДВ)' as [категория]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,@MAIN_DC as [ДЦ]
FROM   DEFDRPARAM INNER JOIN  EnObj ON DEFDRPARAM.EObject = EnObj.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join  ParTypes p on DEFDRPARAM.Type=p.ID 
 
 union
SELECT vDefUShour1.ID as [ID]
,vDefUShour1.name as [Наименование ОИ]
,'ЧАС' as [Тип ОИ]
, p.Abbr as [Тип]
,'Универсальные хранилища 1 час' as [категория]
,et.Abbr+' '+EnObj.Name  as [Объект] 
,@MAIN_DC as [ДЦ]
FROM   vDefUShour1 INNER JOIN  EnObj ON vDefUShour1.EObject = EnObj.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join  ParTypes p on vDefUShour1.Type=p.ID 
order by [Тип ОИ],  ID



