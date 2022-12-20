/*
/******�������� ������� ���********************************************************************/
CREATE TABLE LSA_test..MUN
(IDEnergyObject int,
DCsource varchar(max),
EnergyObject varchar(max),
VoltageLevel int,
NumbTiItog int,
NameTiItog varchar(max),
kontrol varchar(max),
formula varchar(max),
formulaCode varchar(max),
TypeOI varchar(max),
NumbOI int,
NameOI varchar(max),
formulaOI varchar(max),
dop varchar(max)
)

*/

use OIK_Tat
go
declare @MAIN_DC varchar(max)
set @MAIN_DC='��� ����������' -- ����������� �������� ��
--INSERT INTO LSA_test..MUN (IDEnergyObject ,DCsource ,EnergyObject ,VoltageLevel ,NumbTiItog ,NameTiItog ,kontrol ,
--formula ,formulaCode ,TypeOI ,NumbOI ,NameOI ,formulaOI ,dop )

select *
from (SELECT Udef.ID as [id �������]
,@MAIN_DC as [DC]
,et.Abbr+' '+EnObj.Name  as [������]
,Voltage.value [����� ���������]
,UDef.IDTI as [ID �� ����]
,AllTI.[name]  as [��� �� ����]
,'������' as [��������]
,cast(TIFormulas.txt as varchar(max))  AS [�������]
,cast(TIFormulas.Frml as varchar(max))  AS [������� ���]
,IIF(isnull(TIFormulasR.OI,'I')='I', '��','��') AS [��\��]
,isnull(TIFormulasR.SID,UDef.IDTI)  AS [ID]
,case when TIFormulasR.OI is null then AllTI.[name] else (case TIFormulasR.OI when 'I' then a2.[Name] when 'S' then S2.[Name]else  TIFormulasR.OI+cast(TIFormulasR.SID as varchar) end) end as [��� ���]

FROM UDef INNER JOIN EnObj ON EnObj.ID = UDef.EObject
Inner join  EnObjType et on EnObj.Type=et.ID
inner join AllTI on AllTI.ID=UDef.IDTI
inner join ParTypes p on allti.Type=p.ID
inner join ParTypes p1 on allti.Type=p.ID
LEFT OUTER JOIN TIFormulas ON TIFormulas.Result = UDef.IDTI and TIFormulas.Ftype=0 
left OUTER JOIN TIFormulasR ON TIFormulasR.FID = TIFormulas.ID --and isnull(TIFormulasR.OI,'')<>'S'
inner join Voltage on EnObj.Voltage=Voltage.ID
left OUTER JOIN allti as A2 on a2.id=TIFormulasR.SID and TIFormulasR.OI='I'
left OUTER JOIN defts as S2 on S2.id=TIFormulasR.SID and TIFormulasR.OI='S'
WHERE (InControl = 1) 

union

select  distinct Udef.ID as [id �������]
,@MAIN_DC as [DC]
,et.Abbr+' '+EnObj.Name  as [������]
,Voltage.value [����� ���������]
, UDef.IDTI2 as [ID �� ����]
,AllTI.[name]  as [��� �� ����]
, m.DIR as [��������]
,cast(TIFormulas.txt as varchar(max)) as [�������]
,cast(TIFormulas.Frml as varchar(max))  AS [������� ���]
,IIF(isnull(TIFormulasR.OI,'I')='I', '��','��') AS [��\��]
, isnull(TIFormulasR.SID,UDef.IDTI2)   AS [ID]
,case when TIFormulasR.OI is null then AllTI.[name] else (case TIFormulasR.OI when 'I' then a2.[Name] when 'S' then S2.[Name]else  TIFormulasR.OI+cast(TIFormulasR.SID as varchar) end) end as [��� ���]

from udef  INNER JOIN EnObj ON udef.EObject = EnObj.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join AllTI on AllTI.ID=UDef.IDTI2
inner join ParTypes p on allti.Type=p.ID
left join (SELECT distinct ID, '����'  as DIR FROM cuTLevelDir WHERE (Direct = 0)
     union SELECT distinct ID, '�����' as DIR FROM cuTLevelDir WHERE (Direct = 1) ) as M on UDef.ID=M.ID
LEFT OUTER JOIN TIFormulas ON TIFormulas.Result = UDef.IDTI2 and TIFormulas.Ftype=0 
left OUTER JOIN TIFormulasR ON TIFormulasR.FID = TIFormulas.ID --and isnull(TIFormulasR.OI,'')<>'S'
inner join Voltage on EnObj.Voltage=Voltage.ID
left OUTER JOIN allti as A2 on a2.id=TIFormulasR.SID and TIFormulasR.OI='I'
left OUTER JOIN defts as S2 on S2.id=TIFormulasR.SID and TIFormulasR.OI='S'
where [InControl2]=1) as TableMain
 left OUTER JOIN (select Txt, Result from TIFormulas)  as TIFormulas2 on TableMain.[ID]=TIFormulas2.Result and TableMain.[��\��] like '��'
  --left OUTER JOIN (select Txt, Result from TIFormulas)  as TIFormulas3 on TableMain.[ID]=TIFormulas3.Result and TableMain.[��\��] like '��'