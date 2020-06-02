--USE test
--GO

--ALTER proc [dbo].[sp_GetUserRole]
--as
--begin
--	select test.dbo.tbUser.ID, test.dbo.tbUser.UserName, test.dbo.tbRole.ID as [RoleId], test.dbo.tbRole.Name as [RoleName] from test.dbo.tbUser
--	inner join test.dbo.tbUserRole on tbUser.ID = tbUserRole.UserId
--	inner join test.dbo.tbRole on tbUserRole.RoleId = tbRole.ID
--end


--sp_GetUserRole


--select * from tbRole
--select * from tbUserRole




--delete from tbUser where ID in (5,6,7)

--delete from tbRole where ID in (5,6,4)

--insert into tbRole([Name]) values('SV')
--insert into tbRole([Name]) values('TK')
--insert into tbRole([Name]) values('BCN')


--insert into tbUser([UserName],[Password]) values('SV','123456')
--insert into tbUser([UserName],[Password]) values('TK','123456')
--insert into tbUser([UserName],[Password]) values('BCN','123456')


--insert into test.dbo.tbUserRole([UserId],RoleId) values(2,1)
--insert into test.dbo.tbUserRole([UserId],RoleId) values(3,2)
--insert into test.dbo.tbUserRole([UserId],RoleId) values(4,3)

select * from test.dbo.tbUserRole
select * from test.dbo.tbUser
select * from test.dbo.tbRole

--ALTER TABLE test.dbo.tbRole
--ALTER COLUMN Name nvarchar(100);
