/*
/******�������� ������� �������� ���********************************************************************/
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
-----! ��� ��� ���� ���������!-------
/*select * from lsa_test..DCReception
where DCreceiver like '��� ������� �����'*/

use OIK_Tat
go
declare @MAIN_DC varchar(max), @Remote_DC_Filter varchar(max), @Second_DC varchar(max)
set @MAIN_DC='��� ����������' 
set @Remote_DC_Filter='ICCP ��� ���������' 
set @Second_DC='��������� ���'
-- select name from rtu where id in (select RTUid from dtset where SetType=6 and Enable=1) -- ������ ������� ��

--INSERT INTO LSA_test..DCReception (DCreceiver, AdressNaborReceiver,  EnergyObject, TypeOI,TypeOTI,NameOtiReceiver,NumbOtiReceiver,DCsource)  
 
SELECT  @MAIN_DC  as [��-����������]
,isnull(RCV_I.[Addr_Id],'') as [����� � ������ ��-��]
, et.Abbr+' '+EnObj.Name as [������]
, '��' as [��� ��]
,p.Abbr as [��� ��]
,iif(AllTI.OutOfWork =0,'!_�������_! ','')+allti.name  as [������������ �� � ��-����������]
,AllTI.ID as [����� �� � �� ����������]
,@Second_DC as [��-��������]
--,'-' as [������������ �� � ��-���������],'-' as [����� �� � �� ���������],p.Abbr as [��� �� � ���������]
FROM          AllTI INNER JOIN  EnObj ON AllTI.EObject = EnObj.ID 
inner join  TICat c on allti.Category=c.ID 
LEFT OUTER JOIN EnObj AS EO_Host ON EO_Host.ID = EnObj.Higher 
inner join  ParTypes p on allti.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join (SELECT RCV.ID, ���, RTU,[Addr_Id],inv FROM(
	  select p.OI ,p.IDOI as id, ' ����� ��' as ���, RTU.Name as RTU,cast(isnull(mask,addr) as varchar) as 'Addr_Id',inv
		from dtParam2 p,dtSet s INNER JOIN RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
		union
		--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.RemoteDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
		 select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.RemoteDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
	  --select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(iif(Scope=1,'::',i.RemoteDomain+'::')+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID 
	  where s.Enable=1 and s.trans=1 and rtu.Name not like '%ICCPAVT%'
) AS RCV WHERE (RCV.OI = 'I')) AS RCV_I on AllTI.ID=RCV_I.ID
where /*[��-��������]*/ RCV_I.RTU like '%'+@Remote_DC_Filter+'%'
--order by [��-��������], [����� � ������ ��-��]

union
SELECT @MAIN_DC as [��-����������] 
,isnull(RCV_S.[Addr_Id],'') as [����� � ������ ��-��]
,et.Abbr+' '+EnObj.Name as [������] 
,'��' as [��� ��]
, t.Abbr as [���]
,iif(defts.OutOfWork =0,'!_�������_! ','')+defts.name  as [������������ �C � ��-����������] 
,defts.ID as [����� �C � �� ����������]
,@Second_DC as [��-��������] 
FROM         defts INNER JOIN EnObj ON defts.EObject = EnObj.ID Inner join oik..TSCat c on defts.Category=c.ID Inner join oik..TSType t on defts.TSType=t.ID Inner join oik..EnObjType et on EnObj.Type=et.ID
inner join (SELECT RCV.ID, ���, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' ����� ��' as ���, RTU.Name as RTU,cast(isnull(mask,addr) as varchar) as 'Addr_Id',inv
		from dtParam2 p,dtSet s INNER JOIN RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
		union
		--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.RemoteDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
		select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.RemoteDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
	  --select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(iif(Scope=1,'::',i.RemoteDomain+'::')+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  --select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(Identifier as varchar) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID 
	  where s.Enable=1 and s.trans=1 and rtu.Name not like '%ICCPAVT%'
) AS RCV WHERE (RCV.OI = 'S')) AS RCV_S on defts.ID=RCV_S.ID
where /*[��-��������]*/ RCV_S.RTU like '%'+@Remote_DC_Filter+'%'

order by  [����� � ������ ��-��]
