select oti.NumbOti,oti.NameOti, oti.TypeOI,oti.TypeOTI, oti.Category,oti.EnergyObject,oti.DC,rec.DCsource, oti.txt,oti.frm,
pasport.TT,pasport.TN,pasport.MeasTrans,
app.OIK, app.AIP, app.CSPA, app.KOSMOS
from LSA_test..AllOTI as oti
left join LSA_test..DCReception as rec on rec.DCreceiver= oti.DC and rec.NumbOtiReceiver=oti.NumbOti
and rec.TypeOI=oti.TypeOI
left join LSA_test..PTIpasport as pasport on pasport.DC= oti.DC and pasport.NumbOti=oti.NumbOti
and pasport.TypeOI=oti.TypeOI 
left join LSA_test..ApplicationOTI as app on app.DC= oti.DC and app.NumbOti=oti.NumbOti
and app.TypeOI=oti.TypeOI 
order by oti.TypeOI, NumbOti

select * 
from (
select AllOTI.NumbOti, AllOTI.NameOti, AllOTI.TypeOI,AllOTI.TypeOTI,AllOTI.Category,AllOTI.EnergyObject,AllOTI.DC, aipoti.IDdc
from LSA_test..AllOTI 
left join LSA_test..AIPOTI on AllOTI.TypeOI=AIPOTI.TypeOI and AllOTI.NumbOti=AIPOTI.NumbOI
where AllOTI.DC='ОДУ Средней Волги' ) as tableAll
where tableAll.Iddc is null

select *
from LSA_test..KPOS
order by DCsource, NameKS,TypeOI, NumbOI

select Distinct IDEnergyObject,DCsource,EnergyObject,VoltageLevel,NumbTiItog,NameTiItog,formula,formulaCode
from LSA_test..MUN
order by DCsource,EnergyObject,NumbTiItog

select * 
from LSa_test..SMTN_AOPO
order by DC, EquipmentName, EnergyObject,NumbTIsetting

declare @MAIN_DC varchar(max)--Имя ДЦ, котое запрашивает ЛСА
declare @NUMB_OTI int ---номер ТИ, которую запрашивает ЛСА
declare @TYPE_OI varchar(max)
set @MAIN_DC='"+dc+@"' 
set @NUMB_OTI="+numb+@"
set @TYPE_OI='"+type+ @"'
select rec.NumbOtiReceiver as [Номер ТИ в ДЦ-получателе] ,rec.NameOtiReceiver as [Имя ТИ в ДЦ-получателе],
rec.TypeOI as [тип ОИ], rec.TypeOti as [Тип ТИ в ДЦ-получателе] , rec.EnergyObject ,rec.DCreceiver as [ДЦ-получатель]
from LSA_test..DCReception As rec join LSA_test..DCTransmit as sour on rec.AdressNaborReceiver  =sour.AdressNaborSource
and rec.DCsource=sour.DCsource and rec.DCreceiver=sour.DCreceiver 
where sour.NumbOtiSource =@NUMB_OTI and
sour.DCsource like '%'+@MAIN_DC+'%'and
sour.TypeOI like '%'+@TYPE_OI+'%'

declare @MAIN_DC varchar(max)--Имя ДЦ, котое запрашивает ЛСА
declare @NUMB_OTI int ---номер ТИ, которую запрашивает ЛСА
declare @TYPE_OI varchar(max)
set @MAIN_DC='" + dc + @"' 
set @NUMB_OTI=" + numb + @"
set @TYPE_OI='" + type + @"'
select sour.NumbOtiSource as [Номер ТИ в ДЦ-источнике], sour.NameOtiSource as [Имя ТИ в ДЦ-источнике],
sour.TypeOI as [тип ОИ], sour.TypeOti as [Тип ТИ в ДЦ-получателе] , sour.EnergyObject ,sour.DCsource as [ДЦ-источник]
from LSA_test..DCReception As rec join LSA_test..DCTransmit as sour on rec.AdressNaborReceiver  =sour.AdressNaborSource
and rec.DCsource=sour.DCsource and rec.DCreceiver=sour.DCreceiver 
where rec.NumbOtiReceiver =@NUMB_OTI and
rec.DCreceiver like '%'+@MAIN_DC+'%'and
rec.TypeOI like '%'+@TYPE_OI+'%'

CREATE TABLE #ChekInfo(DCkpos varchar(max), NameKs varchar(max),
NumbOti int, TypeOI varchar(max), Dcsource varchar(max),Dcreception varchar(max), numbSource int, numbReception int)


declare @i int
set @i=1

while @i<=(Select MAX(idSech) from LSA_test..commonSech)
begin

insert into #ChekInfo

select KPOS.DCsource as [ДЦкпос], KPOS.NameKS
, KPOS.NumbOi, KPOS.TypeOI
,obmen.[ДЦ источник]
,obmen.[ДЦ получатель]
,obmen.[номер в ДЦ источнике]
,obmen.[номер в ДЦ получателе]
--,obmen.AdressNaborReceiver
from  KPOS
left Join
(select sour.DCsource as [ДЦ источник]
, sour.NumbOtiSource as [номер в ДЦ источнике]
,rec.DCreceiver as [ДЦ получатель]
,rec.NumbOtiReceiver as [номер в ДЦ получателе]
,rec.AdressNaborReceiver
,rec.TypeOI
from  DCReception As rec join  DCTransmit as sour on rec.AdressNaborReceiver =sour.AdressNaborSource
and rec.DCreceiver=sour.DCreceiver and rec.DCsource=sour.DCsource 
where (rec.DCsource=(select  DC1 from LSA_test..commonSech where idSech=@i) and rec.DCreceiver=(select  DC2 from LSA_test..commonSech where idSech=@i)) 
or (rec.DCreceiver=(select  DC1 from LSA_test..commonSech where idSech=@i) and rec.DCsource=(select  DC2 from LSA_test..commonSech where idSech=@i))
) as obmen
on (KPOS.DCsource=obmen.[ДЦ источник] and KPOS.NumbOi=obmen.[номер в ДЦ источнике]  and KPOS.TypeOI=obmen.TypeOI)
or (KPOS.DCsource=obmen.[ДЦ получатель] and KPOS.NumbOi=obmen.[номер в ДЦ получателе]  and KPOS.TypeOI=obmen.TypeOI)
where ((DCsource=(select DC1 from LSA_test..commonSech where idSech=@i) and IDks=(select IDksDC1 from LSA_test..commonSech where idSech=@i))
or (DCsource=(select DC2 from LSA_test..commonSech where idSech=@i) and IDks=(select IDksDC2 from LSA_test..commonSech where idSech=@i)))

and
 (obmen.AdressNaborReceiver in 
(select AdressNaborReceiver
from  Lsa_test..KPOS
left Join
(
select sour.DCsource as [ДЦ источник]
, sour.NumbOtiSource as [номер в ДЦ источнике]
,rec.DCreceiver as [ДЦ получатель]
,rec.NumbOtiReceiver as [номер в ДЦ получателе]
,rec.AdressNaborReceiver
from  DCReception As rec join  DCTransmit as sour on rec.AdressNaborReceiver =sour.AdressNaborSource
and rec.DCreceiver=sour.DCreceiver and rec.DCsource=sour.DCsource
where (rec.DCsource=(select  DC1 from LSA_test..commonSech where idSech=@i) and rec.DCreceiver=(select  DC2 from LSA_test..commonSech where idSech=@i)) 
or (rec.DCreceiver=(select  DC1 from LSA_test..commonSech where idSech=@i) and rec.DCsource=(select  DC2 from LSA_test..commonSech where idSech=@i))

) as obmen
on (KPOS.DCsource=obmen.[ДЦ источник] and KPOS.NumbOi=obmen.[номер в ДЦ источнике] )
or (KPOS.DCsource=obmen.[ДЦ получатель] and KPOS.NumbOi=obmen.[номер в ДЦ получателе])
where 
(DCsource=(select DC1 from LSA_test..commonSech where idSech=@i) and IDks=(select IDksDC1 from LSA_test..commonSech where idSech=@i))
or (DCsource=(select DC2 from LSA_test..commonSech where idSech=@i) and IDks=(select IDksDC2 from LSA_test..commonSech where idSech=@i))
group by obmen.AdressNaborReceiver
HAVING 	count(*) <= 1)
or obmen.AdressNaborReceiver is null
)

order by IDks,DCsource, NumbOi
set @i=@i+1
end;

select *
from #ChekInfo
DROP TABLE #ChekInfo

CREATE TABLE #ChekInfo(DCmun varchar(max), EnergyObject varchar(max),vl int, kontrol varchar(max),
NumbTiItog int,NumbOI int, TypeOi varchar(max), Dcsourse varchar(max),Dcreception varchar(max), numbSource int, numbReception int)

declare @i int
set @i=1
while @i<=(Select MAX(idEnObj) from LSA_test..commonVoltage)
begin

insert into #ChekInfo

select MUN.DCsource as [ДЦмун], MUN.EnergyObject as [Объект], MUN.VoltageLevel, MUN.kontrol
,NumbTiItog, NumbOI as [ОТИ], MUN.TypeOI, obmen.ДЦи, obmen.ДЦп, obmen.[№ в ДЦи],obmen.[№ в ДЦп]
--,obmen.AdressNaborReceiver
from LSA_test..MUN
left Join
(select sour.DCsource as [ДЦи],sour.TypeOI as [Тип и],sour.NumbOtiSource as [№ в ДЦи]
,rec.DCreceiver as [ДЦп],rec.TypeOI as [Тип п],rec.NumbOtiReceiver as [№ в ДЦп]
,rec.AdressNaborReceiver
from LSA_test..DCReception As rec join LSA_test..DCTransmit as sour on rec.AdressNaborReceiver=sour.AdressNaborSource
and rec.DCreceiver=sour.DCreceiver and rec.DCsource=sour.DCsource
where (rec.DCsource=(select  DC1 from LSA_test..commonVoltage where idEnObj=@i) and rec.DCreceiver=(select  DC2 from LSA_test..commonVoltage where idEnObj=@i)) 
or (rec.DCreceiver=(select  DC1 from LSA_test..commonVoltage where idEnObj=@i) and rec.DCsource=(select  DC2 from LSA_test..commonVoltage where idEnObj=@i))
) as obmen
on (MUN.DCsource=obmen.[ДЦи] and MUN.NumbOI=obmen.[№ в ДЦи] and MUN.TypeOI=obmen.[Тип и])
or (MUN.DCsource=obmen.[ДЦп] and MUN.NumbOI=obmen.[№ в ДЦп] and MUN.TypeOI=obmen.[Тип п])

where ((DCsource=(select DC1 from LSA_test..commonVoltage where idEnObj=@i) and IDEnergyObject=(select idEnObjC1 from LSA_test..commonVoltage where idEnObj=@i))
or (DCsource=(select DC2 from LSA_test..commonVoltage where idEnObj=@i) and IDEnergyObject=(select idEnObjDC2 from LSA_test..commonVoltage where idEnObj=@i)))

and (obmen.AdressNaborReceiver in
(Select * from
(select obmen.AdressNaborReceiver from LSA_test..MUN
left Join
(select sour.DCsource as [ДЦи],sour.TypeOI as [Тип и],sour.NumbOtiSource as [№ в ДЦи]
,rec.DCreceiver as [ДЦп],rec.TypeOI as [Тип п],rec.NumbOtiReceiver as [№ в ДЦп]
,rec.AdressNaborReceiver
from LSA_test..DCReception As rec join LSA_test..DCTransmit as sour on rec.AdressNaborReceiver=sour.AdressNaborSource
and rec.DCreceiver=sour.DCreceiver and rec.DCsource=sour.DCsource
where (rec.DCsource=(select  DC1 from LSA_test..commonVoltage where idEnObj=@i) and rec.DCreceiver=(select  DC2 from LSA_test..commonVoltage where idEnObj=@i)) 
or (rec.DCreceiver=(select  DC1 from LSA_test..commonVoltage where idEnObj=@i) and rec.DCsource=(select  DC2 from LSA_test..commonVoltage where idEnObj=@i))
) as obmen
on (MUN.DCsource=obmen.[ДЦи] and MUN.NumbOI=obmen.[№ в ДЦи] and MUN.TypeOI=obmen.[Тип и])
or (MUN.DCsource=obmen.[ДЦп] and MUN.NumbOI=obmen.[№ в ДЦп] and MUN.TypeOI=obmen.[Тип п])

where ((DCsource=(select DC1 from LSA_test..commonVoltage where idEnObj=@i) and IDEnergyObject=(select idEnObjC1 from LSA_test..commonVoltage where idEnObj=@i))
or (DCsource=(select DC2 from LSA_test..commonVoltage where idEnObj=@i) and IDEnergyObject=(select idEnObjDC2 from LSA_test..commonVoltage where idEnObj=@i)))

group by obmen.AdressNaborReceiver, DCsource
)  as tst
group by AdressNaborReceiver
Having COUNT(*) <=1)
or obmen.AdressNaborReceiver is null)

order by AdressNaborReceiver
set @i=@i+1
end;
select *
from #ChekInfo
drop table #ChekInfo

CREATE TABLE #ChekInfo(idLine int,DC_SMTN_Line varchar(max), NameLine varchar(max), EnergyObject varchar(max),
NumbOIfact int,NumbOIa int, NumbOIb int,NumbOIc int,NumbTNV int,  
 Dcsourse varchar(max),Dcreception varchar(max), numbSource int, numbReception int,
 addres  varchar(max))


declare @i int
set @i=1
while @i<=(Select MAX(idLine) from LSA_test..commonLine)
--while @i<=110
begin
insert into #ChekInfo
select @i, SMTN_Line.DC as [ДЦсмтн],SMTN_Line.NameLine as [Объект], SMTN_Line.EnergyObject
,SMTN_Line.NumbOIfact,SMTN_Line.NumbOIa,SMTN_Line.NumbOIb,SMTN_Line.NumbOIc,SMTN_Line.NumbTNV
,obmen.ДЦи, obmen.ДЦп,  obmen.[№ в ДЦи],obmen.[№ в ДЦп]
,CONVERT(varchar(max), obmen.AdressNaborReceiver)
from LSA_test..SMTN_Line
left Join
(select sour.DCsource as [ДЦи],sour.TypeOI as [Тип и],sour.NumbOtiSource as [№ в ДЦи]
,rec.DCreceiver as [ДЦп],rec.TypeOI as [Тип п],rec.NumbOtiReceiver as [№ в ДЦп]
,rec.AdressNaborReceiver
from LSA_test..DCReception As rec join LSA_test..DCTransmit as sour on rec.AdressNaborReceiver=sour.AdressNaborSource
and rec.DCreceiver=sour.DCreceiver and rec.DCsource=sour.DCsource
where ((rec.DCsource=(select  DC1 from LSA_test..commonLine where idLine=@i) and rec.DCreceiver=(select  DC2 from LSA_test..commonLine where idLine=@i)) 
or (rec.DCreceiver=(select  DC1 from LSA_test..commonLine where idLine=@i) and rec.DCsource=(select  DC2 from LSA_test..commonLine where idLine=@i)))
and rec.TypeOI like 'ТИ' and sour.TypeOI like 'ТИ'
) as obmen
on (SMTN_Line.DC=obmen.[ДЦи] and SMTN_Line.NumbOIfact=obmen.[№ в ДЦи])
or (SMTN_Line.DC=obmen.[ДЦп] and SMTN_Line.NumbOIfact=obmen.[№ в ДЦп])

where ((DC=(select DC1 from LSA_test..commonLine where idLine=@i) 
and IdEnOb=(select idEnObjDC1 from LSA_test..commonLine where idLine=@i))
or (DC=(select DC2 from LSA_test..commonLine where idLine=@i)
and IdEnOb=(select idEnObjDC2 from LSA_test..commonLine where idLine=@i)))

and (obmen.AdressNaborReceiver in
(Select * from
(select obmen.AdressNaborReceiver from LSA_test..SMTN_Line
left Join
(select sour.DCsource as [ДЦи],sour.TypeOI as [Тип и],sour.NumbOtiSource as [№ в ДЦи]
,rec.DCreceiver as [ДЦп],rec.TypeOI as [Тип п],rec.NumbOtiReceiver as [№ в ДЦп]
,rec.AdressNaborReceiver
from LSA_test..DCReception As rec join LSA_test..DCTransmit as sour on rec.AdressNaborReceiver=sour.AdressNaborSource
and rec.DCreceiver=sour.DCreceiver and rec.DCsource=sour.DCsource
where ((rec.DCsource=(select  DC1 from LSA_test..commonLine where idLine=@i) and rec.DCreceiver=(select  DC2 from LSA_test..commonLine where idLine=@i)) 
or (rec.DCreceiver=(select  DC1 from LSA_test..commonLine where idLine=@i) and rec.DCsource=(select  DC2 from LSA_test..commonLine where idLine=@i)))
and rec.TypeOI like 'ТИ' and sour.TypeOI like 'ТИ'
) as obmen
on (SMTN_Line.DC=obmen.[ДЦи] and SMTN_Line.NumbOIfact=obmen.[№ в ДЦи])
or (SMTN_Line.DC=obmen.[ДЦп] and SMTN_Line.NumbOIfact=obmen.[№ в ДЦп])

where (DC=(select DC1 from LSA_test..commonLine where idLine=@i) 
and IdEnOb=(select idEnObjDC1 from LSA_test..commonLine where idLine=@i))
or (DC=(select DC2 from LSA_test..commonLine where idLine=@i)
and IdEnOb=(select idEnObjDC2 from LSA_test..commonLine where idLine=@i))

group by obmen.AdressNaborReceiver, DC
)  as tst
group by AdressNaborReceiver
Having COUNT(*) =1)
or obmen.AdressNaborReceiver is null
)

--order by AdressNaborReceiver
set @i=@i+1
end;
select *
from #ChekInfo
drop table #ChekInfo

CREATE TABLE #ChekInfo(idLine int,DC_SMTN_Transform varchar(max), EnergyObject varchar(max),
NameTransform varchar(max), NameWinding varchar(max),
NumbOIfact int,
--NumbOIa int, NumbOIb int,NumbOIc int,NumbTNV int,  
 Dcsourse varchar(max),Dcreception varchar(max), numbSource int, numbReception int,
 addres  varchar(max))
 
declare @i int
set @i=1
while @i<=(Select MAX(idLine) from LSA_test..commonLine)
--while @i<=56
begin
insert into #ChekInfo
select @i, SMTN_Transform.DC as [ДЦсмтн], SMTN_Transform.EnergyObject as [Объект размещения]
,SMTN_Transform.NameTransform as [АТ(Т)]
,SMTN_Transform.Winding as [Обмотка]
,SMTN_Transform.NumbOIfact as [Ток]
--,SMTN_Transform.NumbRpn as [РПН],SMTN_Transform.NumbTNV as [ТНВ],SMTN_Transform..NumbTS as [ТС]
,obmen.ДЦи, obmen.ДЦп,  obmen.[№ в ДЦи],obmen.[№ в ДЦп]
,CONVERT(varchar(max), obmen.AdressNaborReceiver)
from LSA_test..SMTN_Transform
left Join
(select sour.DCsource as [ДЦи],sour.TypeOI as [Тип и],sour.NumbOtiSource as [№ в ДЦи]
,rec.DCreceiver as [ДЦп],rec.TypeOI as [Тип п],rec.NumbOtiReceiver as [№ в ДЦп]
,rec.AdressNaborReceiver
from LSA_test..DCReception As rec join LSA_test..DCTransmit as sour on rec.AdressNaborReceiver=sour.AdressNaborSource
and rec.DCreceiver=sour.DCreceiver and rec.DCsource=sour.DCsource
where ((rec.DCsource=(select  DC1 from LSA_test..commonTransform where idEnObj=@i) and rec.DCreceiver=(select  DC2 from LSA_test..commonTransform where idEnObj=@i)) 
or (rec.DCreceiver=(select  DC1 from LSA_test..commonTransform where idEnObj=@i) and rec.DCsource=(select  DC2 from LSA_test..commonTransform where idEnObj=@i)))
and rec.TypeOI like 'ТИ' and sour.TypeOI like 'ТИ'
) as obmen
on (SMTN_Transform.DC=obmen.[ДЦи] and SMTN_Transform.NumbOIfact=obmen.[№ в ДЦи])
or (SMTN_Transform.DC=obmen.[ДЦп] and SMTN_Transform.NumbOIfact=obmen.[№ в ДЦп])

where 
((DC=(select DC1 from LSA_test..commonTransform where idEnObj=@i) 
and IDTransform=(select idEnObjDC1 from LSA_test..commonTransform where idEnObj=@i)
and IDWinding=(select IDWinding1  from LSA_test..commonTransform where idEnObj=@i))
or 	(DC=(select DC2 from LSA_test..commonTransform where idEnObj=@i)
and IDTransform=(select idEnObjDC2 from LSA_test..commonTransform where idEnObj=@i)
and IDWinding=(select IDWinding2  from LSA_test..commonTransform where idEnObj=@i)))

and (obmen.AdressNaborReceiver in
(Select * from
(select obmen.AdressNaborReceiver from LSA_test..SMTN_Transform
left Join
(select sour.DCsource as [ДЦи],sour.TypeOI as [Тип и],sour.NumbOtiSource as [№ в ДЦи]
,rec.DCreceiver as [ДЦп],rec.TypeOI as [Тип п],rec.NumbOtiReceiver as [№ в ДЦп]
,rec.AdressNaborReceiver
from LSA_test..DCReception As rec join LSA_test..DCTransmit as sour on rec.AdressNaborReceiver=sour.AdressNaborSource
and rec.DCreceiver=sour.DCreceiver and rec.DCsource=sour.DCsource
where ((rec.DCsource=(select  DC1 from LSA_test..commonTransform where idEnObj=@i) and rec.DCreceiver=(select  DC2 from LSA_test..commonTransform where idEnObj=@i)) 
or (rec.DCreceiver=(select  DC1 from LSA_test..commonTransform where idEnObj=@i) and rec.DCsource=(select  DC2 from LSA_test..commonTransform where idEnObj=@i)))
and rec.TypeOI like 'ТИ' and sour.TypeOI like 'ТИ'
) as obmen
on (SMTN_Transform.DC=obmen.[ДЦи] and SMTN_Transform.NumbOIfact=obmen.[№ в ДЦи])
or (SMTN_Transform.DC=obmen.[ДЦп] and SMTN_Transform.NumbOIfact=obmen.[№ в ДЦп])

where 
((DC=(select DC1 from LSA_test..commonTransform where idEnObj=@i) 
and IDTransform=(select idEnObjDC1 from LSA_test..commonTransform where idEnObj=@i)
and IDWinding=(select IDWinding1  from LSA_test..commonTransform where idEnObj=@i))
or 	(DC=(select DC2 from LSA_test..commonTransform where idEnObj=@i)
and IDTransform=(select idEnObjDC2 from LSA_test..commonTransform where idEnObj=@i)
and IDWinding=(select IDWinding2  from LSA_test..commonTransform where idEnObj=@i)))

group by obmen.AdressNaborReceiver, DC
)  as tst
group by AdressNaborReceiver
Having COUNT(*) =1)
or obmen.AdressNaborReceiver is null
)

--order by AdressNaborReceiver
set @i=@i+1
end;
select *
from #ChekInfo
drop table #ChekInfo




