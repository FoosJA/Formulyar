
declare @MAIN_DC varchar(max)
set @MAIN_DC='��� ����������' -- ����������� �������� ������ ��

--INSERT INTO lsa_test..DCTransmit (DCsource, AdressNaborSource, EnergyObject,TypeOI, TypeOTI, NameOtiSource, NumbOtiSource, DCreceiver)

SELECT @MAIN_DC as [��-��������] 
,isnull(snd_I.[Addr_Id],'') as [����� � ������ ��-��]
,et.Abbr+' '+EnObj.Name +isnull(' ('+RCV_I.RTU+')','') as [������] 
, '��' as [���������]
,p.Abbr as [���]
,iif(AllTI.OutOfWork =0,'!_�������_! ','')+allti.name as [������������ �� � ��-���������]
,AllTI.ID as [����� �� � �� ���������]
,snd_I.RTU as [��-��������] 
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

union 

SELECT @MAIN_DC as [��-��������]
,SND_S.[Addr_Id] as [����� � ������ ��-��]
,et.Abbr+' '+EnObj.Name +isnull(' ('+RCV_S.RTU+')','') as [������] 
,'��' as [���������]
, t.Abbr as [���]
,iif(defts.OutOfWork =0,'!_�������_! ','')+defts.name as [������������ �� � ��-���������]
,defts.ID as [����� �� � �� ���������] 
,SND_S.RTU as [��-��������] 
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

union

SELECT @MAIN_DC as [��-��������]
,SND_S.[Addr_Id] as [����� � ������ ��-��]
,et.Abbr+' '+EnObj.Name  as [������] 
,'��' as [���������]
,p.Abbr as [���]
,DEFDRPARAM.name as [������������ �� � ��-���������]
,DEFDRPARAM.ID as [����� �� � �� ���������] 
,SND_S.RTU as [��-��������] 
FROM   DEFDRPARAM INNER JOIN  EnObj ON DEFDRPARAM.EObject = EnObj.ID 
--Inner join  TSCat c on DEFDRPARAM.Category=c.ID
inner join  ParTypes p on DEFDRPARAM.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join (SELECT SND.ID, ���, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' ����� ��' as ���, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from  dtParam2 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(/*iif(Scope=1,'::',i.LocalDomain+'::')+*/cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = 'J')) AS SND_S on DEFDRPARAM.ID=SND_S.ID------!!!!!

union

SELECT @MAIN_DC as [��-��������]
,SND_S.[Addr_Id] as [����� � ������ ��-��]
,et.Abbr+' '+EnObj.Name  as [������] 
,'��' as [���������]
,p.Abbr as [���]
,DEFDRPARAM.name as [������������ �� � ��-���������]
,DEFDRPARAM.ID as [����� �� � �� ���������] 
,SND_S.RTU as [��-��������] 
FROM   DEFDRPARAM INNER JOIN  EnObj ON DEFDRPARAM.EObject = EnObj.ID 
--Inner join  TSCat c on DEFDRPARAM.Category=c.ID
inner join  ParTypes p on DEFDRPARAM.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join (SELECT SND.ID, ���, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' ����� ��' as ���, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from  dtParam2 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(/*iif(Scope=1,'::',i.LocalDomain+'::')+*/cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = 'W')) AS SND_S on DEFDRPARAM.ID=SND_S.ID------!!!!!

union

SELECT @MAIN_DC as [��-��������]
,SND_S.[Addr_Id] as [����� � ������ ��-��]
,et.Abbr+' '+EnObj.Name  as [������] 
,'��' as [���������]
,p.Abbr as [���]
,DefPlan.name as [������������ �� � ��-���������]
,DefPlan.ID as [����� �� � �� ���������] 
,SND_S.RTU as [��-��������] 
FROM   DefPlan INNER JOIN  EnObj ON DefPlan.EObject = EnObj.ID 
--Inner join  TSCat c on DefPlan.Category=c.ID
inner join  ParTypes p on DefPlan.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join (SELECT SND.ID, ���, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' ����� ��' as ���, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from  dtParam2 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(/*iif(Scope=1,'::',i.LocalDomain+'::')+*/cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = 'P')) AS SND_S on DefPlan.ID=SND_S.ID------!!!!!

union

SELECT @MAIN_DC as [��-��������]
,SND_S.[Addr_Id] as [����� � ������ ��-��]
,et.Abbr+' '+EnObj.Name  as [������] 
,'��' as [���������]
,p.Abbr as [���]
,DefDayParam.name as [������������ �� � ��-���������]
,DefDayParam.ID as [����� �� � �� ���������] 
,SND_S.RTU as [��-��������] 
FROM   DefDayParam INNER JOIN  EnObj ON DefDayParam.EObject = EnObj.ID 
--Inner join  TSCat c on DefDayParam.Category=c.ID
inner join  ParTypes p on DefDayParam.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join (SELECT SND.ID, ���, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' ����� ��' as ���, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from  dtParam2 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(/*iif(Scope=1,'::',i.LocalDomain+'::')+*/cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = 'U')) AS SND_S on DefDayParam.ID=SND_S.ID------!!!!!

UNION

SELECT @MAIN_DC as [��-��������]
,SND_S.[Addr_Id] as [����� � ������ ��-��]
,et.Abbr+' '+EnObj.Name  as [������] 
,'��' as [���������]
,p.Abbr as [���]
,iif(DefSParam.OutOfWork =0,'!_�������_! ','')+DefSParam.name as [������������ �� � ��-���������]
,DefSParam.ID as [����� �� � �� ���������] 
,SND_S.RTU as [��-��������] 
FROM   DefSParam INNER JOIN  EnObj ON DefSParam.EObject = EnObj.ID 
--Inner join  TSCat c on DefSParam.Category=c.ID
inner join  ParTypes p on DefSParam.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join (SELECT SND.ID, ���, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' ����� ��' as ���, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from  dtParam2 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union 
select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(/*iif(Scope=1,'::',i.LocalDomain+'::')+*/cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = 'C')) AS SND_S on DefSParam.ID=SND_S.ID------!!

union

SELECT @MAIN_DC as [��-��������]
,SND_S.[Addr_Id] as [����� � ������ ��-��]
,et.Abbr+' '+EnObj.Name  as [������] 
,'��' as [���������]
,p.Abbr as [���]
,DefDRParam.name as [������������ �� � ��-���������]
,DefDRParam.ID as [����� �� � �� ���������] 
,SND_S.RTU as [��-��������] 
FROM   DefDRParam INNER JOIN  EnObj ON DefDRParam.EObject = EnObj.ID 
inner join  ParTypes p on DefDRParam.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join (SELECT SND.ID, ���, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' ����� ��' as ���, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from  dtParam2 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = 'H')) AS SND_S on DefDRParam.ID=SND_S.ID------!!!!!
--where /*[��-����������]*/ snd_S.RTU like '%'+@Remote_DC_Filter+'%'

union

SELECT @MAIN_DC as [��-��������]
,SND_S.[Addr_Id] as [����� � ������ ��-��]
,et.Abbr+' '+EnObj.Name  as [������] 
,'���' as [���������]
,p.Abbr as [���]
,iif(DefSIntParam.OutOfWork =0,'!_�������_! ','')+DefSIntParam.name as [������������ �� � ��-���������]
,DefSIntParam.ID as [����� �� � �� ���������] 
,SND_S.RTU as [��-��������] 
FROM   DefSIntParam INNER JOIN  EnObj ON DefSIntParam.EObject = EnObj.ID 
--Inner join  TSCat c on DefSIntParam.Category=c.ID
inner join  ParTypes p on DefSIntParam.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join (SELECT SND.ID, ���, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' ����� ��' as ���, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from  dtParam2 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(/*iif(Scope=1,'::',i.LocalDomain+'::')+*/cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = 'M')) AS SND_S on DefSIntParam.ID=SND_S.ID------!!!!!

union

SELECT @MAIN_DC as [��-��������]
,SND_S.[Addr_Id] as [����� � ������ ��-��]
,et.Abbr+' '+EnObj.Name  as [������] 
,'���' as [���������]
,p.Abbr as [���]
,vDefUShour1.name as [������������ �� � ��-���������]
,vDefUShour1.ID as [����� �� � �� ���������] 
,SND_S.RTU as [��-��������] 
FROM   vDefUShour1 INNER JOIN  EnObj ON vDefUShour1.EObject = EnObj.ID 
--Inner join  TSCat c on vDefUShour1.Category=c.ID
inner join  ParTypes p on vDefUShour1.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join (SELECT SND.ID, ���, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' ����� ��' as ���, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from  dtParam2 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(/*iif(Scope=1,'::',i.LocalDomain+'::')+*/cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = '�')) AS SND_S on vDefUShour1.ID=SND_S.ID------!!!!!

union

SELECT @MAIN_DC as [��-��������]
,SND_S.[Addr_Id] as [����� � ������ ��-��]
,et.Abbr+' '+EnObj.Name  as [������] 
,'����' as [���������]
,p.Abbr as [���]
,vDefUShour1.name as [������������ �� � ��-���������]
,vDefUShour1.ID as [����� �� � �� ���������] 
,SND_S.RTU as [��-��������] 
FROM   vDefUShour1 INNER JOIN  EnObj ON vDefUShour1.EObject = EnObj.ID 
--Inner join  TSCat c on vDefUShour1.Category=c.ID
inner join  ParTypes p on vDefUShour1.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join (SELECT SND.ID, ���, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' ����� ��' as ���, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from  dtParam2 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(/*iif(Scope=1,'::',i.LocalDomain+'::')+*/cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = '�')) AS SND_S on vDefUShour1.ID=SND_S.ID------!!!!!

union

SELECT @MAIN_DC as [��-��������]
,SND_S.[Addr_Id] as [����� � ������ ��-��]
,et.Abbr+' '+EnObj.Name  as [������] 
,'����' as [���������]
,p.Abbr as [���]
,vDefUShour1.name as [������������ �� � ��-���������]
,vDefUShour1.ID as [����� �� � �� ���������] 
,SND_S.RTU as [��-��������] 
FROM   vDefUShour1 INNER JOIN  EnObj ON vDefUShour1.EObject = EnObj.ID 
--Inner join  TSCat c on vDefUShour1.Category=c.ID
inner join  ParTypes p on vDefUShour1.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join (SELECT SND.ID, ���, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' ����� ��' as ���, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from  dtParam2 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(/*iif(Scope=1,'::',i.LocalDomain+'::')+*/cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = '�')) AS SND_S on vDefUShour1.ID=SND_S.ID------!!!!!

union

SELECT @MAIN_DC as [��-��������]
,SND_S.[Addr_Id] as [����� � ������ ��-��]
,et.Abbr+' '+EnObj.Name  as [������] 
,'����' as [���������]
,p.Abbr as [���]
,vDefUShour1.name as [������������ �� � ��-���������]
,vDefUShour1.ID as [����� �� � �� ���������] 
,SND_S.RTU as [��-��������] 
FROM   vDefUShour1 INNER JOIN  EnObj ON vDefUShour1.EObject = EnObj.ID 
--Inner join  TSCat c on vDefUShour1.Category=c.ID
inner join  ParTypes p on vDefUShour1.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join (SELECT SND.ID, ���, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' ����� ��' as ���, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from  dtParam2 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(/*iif(Scope=1,'::',i.LocalDomain+'::')+*/cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = '�')) AS SND_S on vDefUShour1.ID=SND_S.ID------!!!!!

union

SELECT @MAIN_DC as [��-��������]
,SND_S.[Addr_Id] as [����� � ������ ��-��]
,et.Abbr+' '+EnObj.Name  as [������] 
,'����' as [���������]
,p.Abbr as [���]
,vDefUShour1.name as [������������ �� � ��-���������]
,vDefUShour1.ID as [����� �� � �� ���������] 
,SND_S.RTU as [��-��������] 
FROM   vDefUShour1 INNER JOIN  EnObj ON vDefUShour1.EObject = EnObj.ID 
--Inner join  TSCat c on vDefUShour1.Category=c.ID
inner join  ParTypes p on vDefUShour1.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join (SELECT SND.ID, ���, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' ����� ��' as ���, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from  dtParam2 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(/*iif(Scope=1,'::',i.LocalDomain+'::')+*/cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = '�')) AS SND_S on vDefUShour1.ID=SND_S.ID------!!!!!

union

SELECT @MAIN_DC as [��-��������]
,SND_S.[Addr_Id] as [����� � ������ ��-��]
,et.Abbr+' '+EnObj.Name  as [������] 
,'����' as [���������]
,p.Abbr as [���]
,vDefUShour1.name as [������������ �� � ��-���������]
,vDefUShour1.ID as [����� �� � �� ���������] 
,SND_S.RTU as [��-��������] 
FROM   vDefUShour1 INNER JOIN  EnObj ON vDefUShour1.EObject = EnObj.ID 
--Inner join  TSCat c on vDefUShour1.Category=c.ID
inner join  ParTypes p on vDefUShour1.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join (SELECT SND.ID, ���, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' ����� ��' as ���, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from  dtParam2 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(/*iif(Scope=1,'::',i.LocalDomain+'::')+*/cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = '�')) AS SND_S on vDefUShour1.ID=SND_S.ID------!!!!!

union

SELECT @MAIN_DC as [��-��������]
,SND_S.[Addr_Id] as [����� � ������ ��-��]
,et.Abbr+' '+EnObj.Name  as [������] 
,'���' as [���������]
,p.Abbr as [���]
,vDefUShour1.name as [������������ �� � ��-���������]
,vDefUShour1.ID as [����� �� � �� ���������] 
,SND_S.RTU as [��-��������] 
FROM   vDefUShour1 INNER JOIN  EnObj ON vDefUShour1.EObject = EnObj.ID 
--Inner join  TSCat c on vDefUShour1.Category=c.ID
inner join  ParTypes p on vDefUShour1.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join (SELECT SND.ID, ���, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' ����� ��' as ���, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from  dtParam2 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(/*iif(Scope=1,'::',i.LocalDomain+'::')+*/cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = '�')) AS SND_S on vDefUShour1.ID=SND_S.ID------!!!!!

union

SELECT @MAIN_DC as [��-��������]
,SND_S.[Addr_Id] as [����� � ������ ��-��]
,et.Abbr+' '+EnObj.Name  as [������] 
,'���' as [���������]
,p.Abbr as [���]
,vDefUShour1.name as [������������ �� � ��-���������]
,vDefUShour1.ID as [����� �� � �� ���������] 
,SND_S.RTU as [��-��������] 
FROM   vDefUShour1 INNER JOIN  EnObj ON vDefUShour1.EObject = EnObj.ID 
--Inner join  TSCat c on vDefUShour1.Category=c.ID
inner join  ParTypes p on vDefUShour1.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join (SELECT SND.ID, ���, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' ����� ��' as ���, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from  dtParam2 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(/*iif(Scope=1,'::',i.LocalDomain+'::')+*/cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = '�')) AS SND_S on vDefUShour1.ID=SND_S.ID------!!!!!

union

SELECT @MAIN_DC as [��-��������]
,SND_S.[Addr_Id] as [����� � ������ ��-��]
,et.Abbr+' '+EnObj.Name  as [������] 
,'���' as [���������]
,p.Abbr as [���]
,vDefUShour1.name as [������������ �� � ��-���������]
,vDefUShour1.ID as [����� �� � �� ���������] 
,SND_S.RTU as [��-��������] 
FROM   vDefUShour1 INNER JOIN  EnObj ON vDefUShour1.EObject = EnObj.ID 
--Inner join  TSCat c on vDefUShour1.Category=c.ID
inner join  ParTypes p on vDefUShour1.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join (SELECT SND.ID, ���, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' ����� ��' as ���, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from  dtParam2 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(/*iif(Scope=1,'::',i.LocalDomain+'::')+*/cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = '�')) AS SND_S on vDefUShour1.ID=SND_S.ID------!!!!!

union

SELECT @MAIN_DC as [��-��������]
,SND_S.[Addr_Id] as [����� � ������ ��-��]
,et.Abbr+' '+EnObj.Name  as [������] 
,'����' as [���������]
,p.Abbr as [���]
,vDefUShour1.name as [������������ �� � ��-���������]
,vDefUShour1.ID as [����� �� � �� ���������] 
,SND_S.RTU as [��-��������] 
FROM   vDefUShour1 INNER JOIN  EnObj ON vDefUShour1.EObject = EnObj.ID 
--Inner join  TSCat c on vDefUShour1.Category=c.ID
inner join  ParTypes p on vDefUShour1.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join (SELECT SND.ID, ���, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' ����� ��' as ���, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from  dtParam2 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(/*iif(Scope=1,'::',i.LocalDomain+'::')+*/cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = '�')) AS SND_S on vDefUShour1.ID=SND_S.ID------!!!!!

union

SELECT @MAIN_DC as [��-��������]
,SND_S.[Addr_Id] as [����� � ������ ��-��]
,et.Abbr+' '+EnObj.Name  as [������] 
,'����' as [���������]
,p.Abbr as [���]
,vDefUShour1.name as [������������ �� � ��-���������]
,vDefUShour1.ID as [����� �� � �� ���������] 
,SND_S.RTU as [��-��������] 
FROM   vDefUShour1 INNER JOIN  EnObj ON vDefUShour1.EObject = EnObj.ID 
--Inner join  TSCat c on vDefUShour1.Category=c.ID
inner join  ParTypes p on vDefUShour1.Type=p.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join (SELECT SND.ID, ���, RTU,[Addr_Id],[inv] FROM(
	  select p.OI ,p.IDOI as id, ' ����� ��' as ���, RTU.Name as RTU,cast(isnull(mask,addr) as sql_variant) as 'Addr_Id',inv from  dtParam2 p, dtSet s INNER JOIN  RTU ON s.RTUID = RTU.ID where p.SetID=s.ID and s.Enable=1 and s.trans=2
union select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+':'+cast(s.SetID as varchar)+':'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(i.LocalDomain+'::'+cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv
--select p.OI  as 'OI',p.IDOI as ID, ' ����� ICCP' as ���, RTU.Name as RTU,cast(/*iif(Scope=1,'::',i.LocalDomain+'::')+*/cast(Identifier as varchar) as sql_variant) as 'Addr_Id', inv 
	  from  dtParam6 p INNER JOIN  dtSet s on p.SetID=s.ID INNER JOIN  RTU ON s.RTUID = RTU.ID INNER JOIN  RTU_ICCP I ON s.RTUID = I.RTUID where 
	  s.Enable=1 and s.trans=2 and rtu.Name not like '%ICCPAVT%'
) AS SND WHERE (snd.OI = '�')) AS SND_S on vDefUShour1.ID=SND_S.ID------!!!!!

order by [���������],[����� �� � �� ���������], [����� � ������ ��-��]