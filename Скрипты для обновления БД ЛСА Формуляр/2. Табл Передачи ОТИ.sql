/*
/******�������� ������� �������� ���********************************************************************/
CREATE TABLE DCTransmit
(DCsource varchar(max),
AdressNaborSource sql_variant,
EnergyObject varchar(max),
TypeOI varchar(max),
TypeOTI varchar(max),
NameOtiSource varchar(max),
NumbOtiSource int,
DCreceiver varchar(max))

/******�������� ������� ������� ���********************************************************************/

truncate  table DCTransmit

select *
delete
from lsa_test..DCTransmit
where  DCreceiver like '��� ������� �����' and DCsource like '���������� ���'
*/

use OIK_Tat
go
declare @MAIN_DC varchar(max), @Remote_DC_Filter varchar(max), @Second_DC varchar(max) 
set @MAIN_DC='��� ����������' -- ����������� �������� ������ ��
-- select name from rtu where id in (select RTUid from dtset where SetType=6 and Enable=1) -- ������ ������� ��
set @Remote_DC_Filter='ICCP ��� ���������' -- ����������� �������� �������� ���� ������ �������� �� (����� ����� �� ������, ������������ �������� ����). ��� ���������� ���������� = ''.
set @Second_DC='��������� ���'
/******�������� ������� �� �������� �� ������ ��********************************************************************/

--INSERT INTO lsa_test..DCTransmit (DCsource, AdressNaborSource, EnergyObject,TypeOI, TypeOTI, NameOtiSource, NumbOtiSource, DCreceiver)

/********* ���������� �� �� ������� �� ��������� ***********/
SELECT @MAIN_DC as [��-��������] 
,isnull(snd_I.[Addr_Id],'') as [����� � ������ ��-��]
,et.Abbr+' '+EnObj.Name +isnull(' ('+RCV_I.RTU+')','') as [������] 
, '��'
,p.Abbr as [���]
,iif(AllTI.OutOfWork =0,'!_�������_! ','')+allti.name as [������������ �� � ��-���������]
,AllTI.ID as [����� �� � �� ���������]
--,isnull(snd_I.RTU,'') as [��-����������]
,@Second_DC as [��-����������]
FROM         AllTI INNER JOIN EnObj ON AllTI.EObject = EnObj.ID 
inner join  TICat c on allti.Category=c.ID 
inner join  ParTypes p on allti.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
left join (SELECT RCV.ID, ���, RTU,[Addr_Id],inv FROM(
      SELECT 'I'  as 'OI', DefTI.ID as ID,' ��' as ���,RTU.Name as RTU,addr as 'Addr_Id',inv FROM DefTI INNER JOIN RTU ON DefTI.RTUID = RTU.ID
union SELECT 'S', DefTS.ID,' ��' as ���, 	RTU.Name,mask as 'Addr_Id', inv FROM DefTS INNER JOIN RTU ON DefTS.RTUID = RTU.ID
union SELECT p.OI, p.IDOI, ' ����� ��', 	RTU.Name,isnull(mask,addr) as 'Addr_Id'	, inv FROM dtParam2 AS p INNER JOIN dtSet AS s ON p.SetID = s.ID INNER JOIN RTU ON s.RTUID = RTU.ID WHERE (s.Enable = 1) AND (s.Trans = 1)
union select p.OI ,p.IDOI, ' ����� ���', 	RTU.Name,ExtIDOI, iif(ScaleFactor<0,1,0) as inv from dtParam3 p,dtSet s INNER JOIN RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=1
) AS RCV WHERE (RCV.OI = 'I')) AS RCV_I on AllTI.ID=RCV_I.ID
inner join (SELECT SND.ID, ���, RTU,[Addr_Id],inv FROM(
select		p.OI ,p.IDOI as id, ' ����� ��' as ���, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from dtParam2 p,dtSet s INNER JOIN RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union --select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+':'+/*cast(s.SetID as varchar)+*/':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv -- � ����������� � ������
		select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(/*iif(Scope=1,'::',i.LocalDomain+'::')+*/cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from dtParam6 p INNER JOIN dtSet s on p.SetID=s.ID INNER JOIN RTU ON s.RTUID = RTU.ID INNER JOIN RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = 'I')) AS SND_I on AllTI.ID=SND_I.ID
where /*[��-����������]*/ snd_I.RTU like+@Remote_DC_Filter
--order by [��-����������], [����� � ������ ��-��]

union 

SELECT @MAIN_DC as [��-��������]
,SND_S.[Addr_Id] as [����� � ������ ��-��]
,et.Abbr+' '+EnObj.Name +isnull(' ('+RCV_S.RTU+')','') as [������] 
,'��' as [��� ��]
, t.Abbr as [���]
,iif(defts.OutOfWork =0,'!_�������_! ','')+defts.name as [������������ �� � ��-���������]
,defts.ID as [����� �� � �� ���������] 
,@Second_DC as [��-����������]
FROM   defts INNER JOIN  EnObj ON defts.EObject = EnObj.ID 
Inner join  TSCat c on defts.Category=c.ID
Inner join  TSType t on defts.TSType=t.ID 
Inner join  EnObjType et on EnObj.Type=et.ID
left join (SELECT RCV.ID, ���, RTU,[Addr_Id],[inv] 
			FROM( SELECT 'I'  as 'OI', DefTI.ID as ID,' ��' as ���, RTU.Name as RTU,addr as 'Addr_Id',inv 
					FROM  DefTI INNER JOIN  RTU ON DefTI.RTUID = RTU.ID
union SELECT 'S' , DefTS.ID,' ��' as ���, 	RTU.Name,mask as 'Addr_Id', inv FROM  DefTS INNER JOIN  RTU ON DefTS.RTUID = RTU.ID
union SELECT p.OI, p.IDOI, ' ����� ��', 	RTU.Name,isnull(mask,addr) as 'Addr_Id'	, inv FROM  dtParam2 AS p INNER JOIN  dtSet AS s ON p.SetID = s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID WHERE (s.Enable = 1) AND (s.Trans = 1)
union select p.OI ,p.IDOI, ' ����� ���', 	RTU.Name,ExtIDOI, iif(ScaleFactor<0,1,0) as inv from  dtParam3 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=1
) AS RCV WHERE (RCV.OI = 'S')) AS RCV_S on defts.ID=RCV_S.ID
inner join (SELECT SND.ID, ���, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' ����� ��' as ���, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from  dtParam2 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union 
select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
-- p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(/*iif(Scope=1,'::',i.LocalDomain+'::')+*/cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = 'S')) AS SND_S on defts.ID=SND_S.ID
where /*[��-����������]*/ snd_S.RTU like '%'+@Remote_DC_Filter+'%'
order by [��-����������], [����� � ������ ��-��]