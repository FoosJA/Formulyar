SELECT
'��� ������� �����' ,
dbo.fn_EnNameEquip(EO2.ID) AS [������������],
EO2.ID as [EquipmentID],
eo2.name as [������ ����],
EnObj.ID as [�������ID],
EnObj.Name as [�������],
dbo.ovc_Limiting.OIIDmax [ID ��],--������������� ��������� �� ��� ��� �������� ������������ ������� I��.���.
dbo.fn_GetNameOI(dbo.ovc_Limiting.OImax, dbo.ovc_Limiting.OIIDmax) as [���� ��]
FROM dbo.ovc_Limiting INNER JOIN
EnObj ON dbo.ovc_Limiting.ID = EnObj.ID 
INNER JOIN enobj as eo2 on eo2.id = EnObj.higher
WHERE dbo.ovc_Limiting.OImax IS NOT NULL and EnObj.Name like '%����%'
union
SELECT 
'��� ������� �����' ,
dbo.fn_EnNameEquip(EO2.ID) AS [������������],
EO2.ID as [EquipmentID],
eo2.name as [������ ����],
EnObj.ID as [�������ID],
EnObj.Name as [�������],
dbo.ovc_Limiting.OIIDcrash as [ID ��],--������������� ��������� �� ��� ��� �������� ������������ ������� I��.���.
dbo.fn_GetNameOI(dbo.ovc_Limiting.OIcrash, dbo.ovc_Limiting.OIIDcrash) as [���� ��]
FROM dbo.ovc_Limiting INNER JOIN
EnObj ON dbo.ovc_Limiting.ID = EnObj.ID 
INNER JOIN enobj as eo2 on eo2.id = EnObj.higher
WHERE dbo.ovc_Limiting.OIcrash IS NOT NULL and EnObj.Name like '%����%'
order by eo2.name