
declare @pwd1 varchar(20), @pwd2 varbinary(100)
set @pwd1 = 'senha*123'
set @pwd2 = Convert(varbinary(100), pwdEncrypt(@pwd1))
select @pwd1, @pwd2

declare @pwd3 varchar(20)
set @pwd3 = 'senha*123'
select pwdCompare(@pwd3, @pwd2, 0)