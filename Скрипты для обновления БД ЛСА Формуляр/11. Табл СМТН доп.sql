/*
create table LSA_test..SMTN_Dop
(DC varchar(max),
EnergyObject varchar(max),
Equipment varchar(max),
TypeITO varchar(max),
NumbOI int,
Formula varchar(max),
FormulaCode varchar(max),
NameOI varchar(max))
**/

use OIK
go

declare @MainDC varchar(max)
set @MainDC='Œƒ” —Â‰ÌÂÈ ¬ÓÎ„Ë'
--insert into LSA_test..SMTN_Dop(DC, EnergyObject,Equipment ,
--TypeITO ,NumbOI, NameOI  ,Formula ,FormulaCode)

-- ‰ÓÔ “» ‚ —Ã“Õ
SELECT 
@MainDC
,eo2.name
, EnObj.Name
,'“»' 
, ovc_Limiting.OIIDmax
,dbo.fn_GetNameOI(ovc_Limiting.OImax, ovc_Limiting.OIIDmax)
,cast(isnull(F_MAX.Txt,'') as varchar(max))
,isnull(F_MAX.Frml,'')  
FROM ovc_Limiting INNER JOIN
EnObj ON ovc_Limiting.ID = EnObj.ID 
INNER JOIN enobj as eo2 on eo2.id = EnObj.higher
left JOIN
TIFormulas AS F_MAX ON ovc_Limiting.OIIDmax = F_MAX.Result AND (F_MAX.Ftype = 0)
left JOIN
TIFormulas AS F_CR ON ovc_Limiting.OIIDcrash = F_CR.Result AND (F_CR.Ftype = 0)
WHERE (ovc_Limiting.OImax IS NOT NULL) and EnObj.Name not like '%¿ŒœŒ%'

union

SELECT @MainDC
,eo2.name
, EnObj.Name
,'“»' 
, ovc_Limiting.OIIDmax
,dbo.fn_GetNameOI(ovc_Limiting.OIcrash, ovc_Limiting.OIIDcrash)
,cast(isnull(F_MAX.Txt,'') as varchar(max))
,isnull(F_MAX.Frml,'') 
FROM OIKEDIT.dbo.ovc_Limiting INNER JOIN
EnObj ON OIKEDIT.dbo.ovc_Limiting.ID = EnObj.ID 
INNER JOIN enobj as eo2 on eo2.id = EnObj.higher
left JOIN
TIFormulas AS F_MAX ON OIKEDIT.dbo.ovc_Limiting.OIIDmax = F_MAX.Result AND (F_MAX.Ftype = 0)
left JOIN
TIFormulas AS F_CR ON OIKEDIT.dbo.ovc_Limiting.OIIDcrash = F_CR.Result AND (F_CR.Ftype = 0)
WHERE ovc_Limiting.OIcrash IS NOT NULL and EnObj.Name not like '%¿ŒœŒ%'

union

SELECT @MainDC,EO_Conn.name 
, EnObj.Name 
,'“—' 
,isnull(Stat.OIID ,'') 
,dbo.fn_GetNameOI(Stat.OI, Stat.OIID)
,cast(isnull(F_Stat.Txt,'') as varchar(max))   
,isnull(F_Stat.Frml,'') 
FROM ovc_Depend 
INNER JOIN EnObj ON EnObj.ID = ovc_Depend.ID and not (EnObj.Type in (25,33,39,117)) --and (EnObj.Type = 118) 
INNER JOIN EnObj AS EO_Conn ON EnObj.Higher = EO_Conn.id
INNER JOIN enFactorOI AS Stat ON EnObj.ID = Stat.IDEnObj and (Stat.IDFactor = 100)
left JOIN TSFormulas AS F_Stat ON Stat.OIID = F_Stat.Result and F_Stat.OutOfWork=1
where EnObj.Name not like '%¿ŒœŒ%'
