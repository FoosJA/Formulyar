SELECT
'ОДУ Средней Волги' ,
dbo.fn_EnNameEquip(EO2.ID) AS [Оборудование],
EO2.ID as [EquipmentID],
eo2.name as [Объект дисп],
EnObj.ID as [элементID],
EnObj.Name as [элемент],
dbo.ovc_Limiting.OIIDmax [ID ТИ],--Идентификатор параметра ОИ для для указания динамической уставки Iдл.доп.
dbo.fn_GetNameOI(dbo.ovc_Limiting.OImax, dbo.ovc_Limiting.OIIDmax) as [Наим ТИ]
FROM dbo.ovc_Limiting INNER JOIN
EnObj ON dbo.ovc_Limiting.ID = EnObj.ID 
INNER JOIN enobj as eo2 on eo2.id = EnObj.higher
WHERE dbo.ovc_Limiting.OImax IS NOT NULL and EnObj.Name like '%АОПО%'
union
SELECT 
'ОДУ Средней Волги' ,
dbo.fn_EnNameEquip(EO2.ID) AS [Оборудование],
EO2.ID as [EquipmentID],
eo2.name as [Объект дисп],
EnObj.ID as [элементID],
EnObj.Name as [элемент],
dbo.ovc_Limiting.OIIDcrash as [ID ТИ],--Идентификатор параметра ОИ для для указания динамической уставки Iав.доп.
dbo.fn_GetNameOI(dbo.ovc_Limiting.OIcrash, dbo.ovc_Limiting.OIIDcrash) as [Наим ТИ]
FROM dbo.ovc_Limiting INNER JOIN
EnObj ON dbo.ovc_Limiting.ID = EnObj.ID 
INNER JOIN enobj as eo2 on eo2.id = EnObj.higher
WHERE dbo.ovc_Limiting.OIcrash IS NOT NULL and EnObj.Name like '%АОПО%'
order by eo2.name