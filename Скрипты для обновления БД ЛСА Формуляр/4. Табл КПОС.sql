/*
/******Создание таблицы перелачи ОТИ********************************************************************/
CREATE TABLE Lsa_test..KPOS
(DCsource varchar(max),
NameKS varchar(max),
IDks int ,
Equipment varchar(max),
NameOI varchar(max),
NumbOI int,
TypeOI varchar(max),
Formula varchar(max),
EnObj varchar(max)
)
*/

use OIK_Tat
go
declare @DC as varchar(max)
set @DC='РДУ Татарстана' 

/******Заполняю таблицы КПОС отдельно из каждой БД********************************************************************/
--INSERT INTO Lsa_test..KPOS (NameKS,IDks, NameOI ,TypeOI, NumbOI,DCsource,Formula,Equipment)

SELECT SectList.Name AS [Контролируемое сечение],
 SectList.IDSect as [id сечения]
,case when TIFormulasR.OI is null then a1.[name] else (case TIFormulasR.OI when 'I' then a2.[Name] when 'S' then S2.[Name]else  TIFormulasR.OI+cast(TIFormulasR.SID as varchar) end) end as [Параметр]
,IIF(isnull(TIFormulasR.OI,'I')='I', 'ТИ','ТС')  as [Тип ОИ]
,isnull(TIFormulasR.SID,SectList.IDTI) AS  [Номер параметра ОИ ДЦ]
,@DC AS [ДЦ источник]
,TIFormulas.Txt
---------------НАДО ПРОВЕРЯТЬ У ДЦ "ПС ПС"--------------------------------
/*,case when TIFormulasR.OI is null then (case et1.Abbr when 'ПС' then  et1.Abbr+' '+e1.Name when 'РДУ' then  e1.Name+' '+et1.Abbr  else e1.Name end) else 
(case TIFormulasR.OI when 'I' then (case et2.Abbr when 'РДУ' then  e2.Name+' '+et2.Abbr else e2.Name end)
					 when 'S' then (case et3.Abbr when 'РДУ' then  e3.Name+' '+et3.Abbr else e3.Name end) 
					 else  TIFormulasR.OI+cast(TIFormulasR.SID as varchar) 
end) 
end as [EO]*/

,case when TIFormulasR.OI is null then (case et1.Abbr when 'ПС' then  et1.Abbr+' '+e1.Name when 'РДУ' then  e1.Name+' '+et1.Abbr  else e1.Name end) else 
(case TIFormulasR.OI when 'I' then (case et2.Abbr when 'ПС' then  et2.Abbr+' '+e2.Name when 'РДУ' then  e2.Name+' '+et2.Abbr else e2.Name end)
					 when 'S' then (case et3.Abbr when 'ПС' then  et3.Abbr+' '+e3.Name when 'РДУ' then  e3.Name+' '+et3.Abbr else e3.Name end) 
					 else  TIFormulasR.OI+cast(TIFormulasR.SID as varchar) 
end) 
end as [EO]
--, case SectList.ArrDir when '1' then ('от шин') else ('к шинам') end as [Положительное направление]

FROM (SELECT DISTINCT psSect.[Order], psSect.Name, psSchem.IDSect, psSchem.IDTI,psSect.ControlType,Direct,ArrDir,
psSect.ID as SectID FROM psSect INNER JOIN psSchem ON psSect.ID = psSchem.IDSect 				
WHERE (psSect.ControlType = 0) and  (psSect.Direct in (0,1,2))
) AS SectList 


LEFT OUTER JOIN TIFormulas ON TIFormulas.Result = SectList.IDTI and TIFormulas.Ftype=0 
left OUTER JOIN TIFormulasR ON TIFormulasR.FID = TIFormulas.ID --and isnull(TIFormulasR.OI,'')<>'S'
left OUTER JOIN allti as A1 on a1.id=SectList.IDTI
left OUTER JOIN EnObj as e1 on e1.id=a1.EObject
left OUTER JOIN allti as A2 on a2.id=TIFormulasR.SID and TIFormulasR.OI='I'
left OUTER JOIN EnObj as e2 on e2.id=a2.EObject
left OUTER JOIN defts as S2 on S2.id=TIFormulasR.SID and TIFormulasR.OI='S'
left OUTER JOIN EnObj as e3 on e3.id=S2.EObject
left OUTER JOIN  EnObjType et1 on e1.Type=et1.ID
left OUTER JOIN  EnObjType et2 on e2.Type=et2.ID
left OUTER JOIN EnObjType et3 on e3.Type=et3.ID

order by SectList.Name, [Тип ОИ],[Номер параметра ОИ ДЦ]
