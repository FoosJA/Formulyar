/*
CREATE TABLE LSA_test..commonSech
 (idSech int IDENTITY
 , DC1 varchar(max)
 , IDksDC1 int
 ,DC2 varchar(max)
 , IDksDC2 int)
 

insert into LSA_test..commonSech(Dc1, IDksDC1, DC2,IDksDC2)
values ('ОДУ Средней Волги',43,'Саратовское РДУ',32),
--('Нижегородское РДУ','Сечение №1 МЭС','Пензенское РДУ',10),--Осиновка Саранск
('Пензенское РДУ',5,'Саратовское РДУ',16)
*/
/*
CREATE TABLE LSA_test..commonVoltage
 (idEnObj int IDENTITY
 , DC1 varchar(max)
 , idEnObjC1 int
 ,DC2 varchar(max)
 , idEnObjDC2 int)

 insert into LSA_test..commonVoltage(Dc1, idEnObjC1, DC2,idEnObjDC2)
 values ('ОДУ Средней Волги',13046,'Нижегородское РДУ',8986)
 ,('ОДУ Средней Волги',13045,'Нижегородское РДУ',44667)
 ,('ОДУ Средней Волги',30,'Нижегородское РДУ',44670)
 ,('ОДУ Средней Волги',700,'Нижегородское РДУ',44671)
 ,('ОДУ Средней Волги',67,'Нижегородское РДУ',44668)
 ,('ОДУ Средней Волги',66,'Нижегородское РДУ',44669)
 ,('ОДУ Средней Волги',16,'Нижегородское РДУ',44673)
 ,('ОДУ Средней Волги',14,'Нижегородское РДУ',44672)
 ,('ОДУ Средней Волги',13,'Пензенское РДУ',3)
 ,('ОДУ Средней Волги',5,'Самарское РДУ',504)
 ,('ОДУ Средней Волги',1,'Самарское РДУ',502)
 ,('ОДУ Средней Волги',5370,'Самарское РДУ',7274)
 ,('ОДУ Средней Волги',7,'Самарское РДУ',421)
 ,('ОДУ Средней Волги',9,'Самарское РДУ',13108)
 ,('ОДУ Средней Волги',4,'Самарское РДУ',13145)
 ,('ОДУ Средней Волги',2,'Саратовское РДУ',13)
 ,('ОДУ Средней Волги',15,'Саратовское РДУ',14)
 ,('ОДУ Средней Волги',10,'Саратовское РДУ',47)
 */
 /*CREATE TABLE LSA_test..commonTransform
 (idEnObj int IDENTITY
 , DC1 varchar(max)
 , idEnObjC1 int
 ,idWinding1 int
 ,DC2 varchar(max)
 , idEnObjDC2 int
 ,idWinding2 int)
 */
  CREATE TABLE LSA_test..commonBreaker
 (idEnObj int IDENTITY
 , DC1 varchar(max)
 , idEnObjC1 int
 ,DC2 varchar(max)
 , idEnObjDC2 int)