CREATE TABLE #temp
(DC varchar(max),
NameLine varchar(max),
IdLine int,
EnergyObject varchar(max),
IdEnOb int,
NameOIfact varchar(max),
NumbOIfact int,
formula varchar(max),
formulaKod varchar(max),
NameOIa varchar(max),
NumbOIa int,
NameOIb varchar(max),
NumbOIb int,
NameOI varchar(max),
NumbOIc int,
NameTNV varchar(max),
NumbTNV int,
NameTSstat varchar(max),
NumbTSstat int,
NameTSstatVL varchar(max),
NumbTSstatVL int
)
INSERT INTO #temp (DC, NameLine , IdLine, EnergyObject,IdEnOb ,NameOIfact ,NumbOIfact ,formula ,
formulaKod ,NameOIa ,NumbOIa ,NameOIb ,NumbOIb ,NameOI ,NumbOIc ,NameTNV ,NumbTNV ,NameTSstat ,
NumbTSstat ,NameTSstatVL,NumbTSstatVL )

select distinct
DC,NameLine ,IdLine,EnergyObject,IdEnOb,NameOIfact ,NumbOIfact ,formula ,formulaKod ,NameOIa 
,NumbOIa ,NameOIb ,NumbOIb ,NameOI ,NumbOIc ,NameTNV ,NumbTNV ,NameTSstat ,NumbTSstat 
,NameTSstatVL,NumbTSstatVL 
from LSA_test..SMTN_Line
ORDER BY DC,IdLine,IdEnOb

truncate table LSA_test..SMTN_Line

INSERT INTO LSA_test..SMTN_Line (DC, NameLine , IdLine, EnergyObject,IdEnOb ,NameOIfact ,NumbOIfact ,formula ,
formulaKod ,NameOIa ,NumbOIa ,NameOIb ,NumbOIb ,NameOI ,NumbOIc ,NameTNV ,NumbTNV ,NameTSstat ,
NumbTSstat ,NameTSstatVL,NumbTSstatVL )
select *
from #temp
