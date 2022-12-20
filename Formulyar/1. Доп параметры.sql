use OIK
go

DECLARE @MAIN_DC varchar(max)
set @MAIN_DC='��� ������� �����'

	--INSERT INTO LSA_test..AllOTI (NumbOti,NameOti,TypeOI,TypeOTI,Category,EnergyObject,DC ) 

SELECT DefSParam.ID as [ID]
,DefSParam.name as [������������ ��]
,'��' as [��� ��]
, p.Abbr as [���]
,'����������� ��������� ������������' as [���������]
,et.Abbr+' '+EnObj.Name  as [������] 
,@MAIN_DC as [��]
FROM   DefSParam INNER JOIN  EnObj ON DefSParam.EObject = EnObj.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join  ParTypes p on DefSParam.Type=p.ID 
 where DefSParam.OutOfWork <>0 

union
SELECT DefDRParam.ID as [ID]
,DefDRParam.name as [������������ ��]
,'��' as [��� ��]
, p.Abbr as [���]
,'��-2 (�����������)' as [���������]
,et.Abbr+' '+EnObj.Name  as [������] 
,@MAIN_DC as [��]
FROM   DefDRParam INNER JOIN  EnObj ON DefDRParam.EObject = EnObj.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join  ParTypes p on DefDRParam.Type=p.ID 


union
SELECT DefSIntParam.ID as [ID]
,DefSIntParam.name as [������������ ��]
,'���' as [��� ��]
, p.Abbr as [���]
,'����������� ��������� �������������' as [���������]
,et.Abbr+' '+EnObj.Name  as [������] 
,@MAIN_DC as [��]
FROM   DefSIntParam INNER JOIN  EnObj ON DefSIntParam.EObject = EnObj.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join  ParTypes p on DefSIntParam.Type=p.ID 
 where DefSIntParam.OutOfWork <>0 

 union
 SELECT DefPlan.ID as [ID]
,DefPlan.name as [������������ ��]
,'��' as [��� ��]
, p.Abbr as [���]
,'�����' as [���������]
,et.Abbr+' '+EnObj.Name  as [������] 
,@MAIN_DC as [��]
FROM   DefPlan INNER JOIN  EnObj ON DefPlan.EObject = EnObj.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join  ParTypes p on DefPlan.Type=p.ID 
 
 union
SELECT DefDayParam.ID as [ID]
,DefDayParam.name as [������������ ��]
,'��' as [��� ��]
, p.Abbr as [���]
,'���������� ����������' as [���������]
,et.Abbr+' '+EnObj.Name  as [������] 
,@MAIN_DC as [��]
FROM   DefDayParam INNER JOIN  EnObj ON DefDayParam.EObject = EnObj.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join  ParTypes p on DefDayParam.Type=p.ID 
 
 union
SELECT DEFDRPARAM.ID as [ID]
,DEFDRPARAM.name as [������������ ��]
,'��' as [��� ��]
, p.Abbr as [���]
,'��-1 (����������,���)' as [���������]
,et.Abbr+' '+EnObj.Name  as [������] 
,@MAIN_DC as [��]
FROM   DEFDRPARAM INNER JOIN  EnObj ON DEFDRPARAM.EObject = EnObj.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join  ParTypes p on DEFDRPARAM.Type=p.ID 
 
 union
SELECT vDefUShour1.ID as [ID]
,vDefUShour1.name as [������������ ��]
,'���' as [��� ��]
, p.Abbr as [���]
,'������������� ��������� 1 ���' as [���������]
,et.Abbr+' '+EnObj.Name  as [������] 
,@MAIN_DC as [��]
FROM   vDefUShour1 INNER JOIN  EnObj ON vDefUShour1.EObject = EnObj.ID
Inner join  EnObjType et on EnObj.Type=et.ID
inner join  ParTypes p on vDefUShour1.Type=p.ID 
order by [��� ��],  ID



