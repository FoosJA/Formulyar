/*
CREATE TABLE LSA_test..commonSech
 (idSech int IDENTITY
 , DC1 varchar(max)
 , IDksDC1 int
 ,DC2 varchar(max)
 , IDksDC2 int)
 

insert into LSA_test..commonSech(Dc1, IDksDC1, DC2,IDksDC2)
values ('��� ������� �����',43,'����������� ���',32),
--('������������� ���','������� �1 ���','���������� ���',10),--�������� �������
('���������� ���',5,'����������� ���',16)
*/
/*
CREATE TABLE LSA_test..commonVoltage
 (idEnObj int IDENTITY
 , DC1 varchar(max)
 , idEnObjC1 int
 ,DC2 varchar(max)
 , idEnObjDC2 int)

 insert into LSA_test..commonVoltage(Dc1, idEnObjC1, DC2,idEnObjDC2)
 values ('��� ������� �����',13046,'������������� ���',8986)
 ,('��� ������� �����',13045,'������������� ���',44667)
 ,('��� ������� �����',30,'������������� ���',44670)
 ,('��� ������� �����',700,'������������� ���',44671)
 ,('��� ������� �����',67,'������������� ���',44668)
 ,('��� ������� �����',66,'������������� ���',44669)
 ,('��� ������� �����',16,'������������� ���',44673)
 ,('��� ������� �����',14,'������������� ���',44672)
 ,('��� ������� �����',13,'���������� ���',3)
 ,('��� ������� �����',5,'��������� ���',504)
 ,('��� ������� �����',1,'��������� ���',502)
 ,('��� ������� �����',5370,'��������� ���',7274)
 ,('��� ������� �����',7,'��������� ���',421)
 ,('��� ������� �����',9,'��������� ���',13108)
 ,('��� ������� �����',4,'��������� ���',13145)
 ,('��� ������� �����',2,'����������� ���',13)
 ,('��� ������� �����',15,'����������� ���',14)
 ,('��� ������� �����',10,'����������� ���',47)
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