CREATE database QuanLyQuanNuoc
go

use QuanLyQuanNuoc
go

create table TableFood
(
	id int IDENTITY PRIMARY KEY,
	name NVARCHAR(100)not null default N'Bàn chưa đặt tên',
	status NVARCHAR(100)not null default N'Trống' --Trống hoặc có người

)
GO

create table Account
(
	DisplayName nvarchar(100) not null,
	UserName nvarchar(100) primary key,
	PassWord nvarchar(1000)not null default 0,
	Type INT not null default 0 --1 admin 0 staff
)
go

create table FoodCategory
(
	id int IDENTITY PRIMARY KEY,
	name NVARCHAR(100) not null default N'Chưa đặt tên',
)
go

create table Food
(
	id int IDENTITY PRIMARY KEY,
	name NVARCHAR(100) not null default N'Chưa đặt tên',
	idCategory INT not null,
	price FLOAT not null default 0,

	foreign key (idCategory) references dbo.FoodCategory(id)
)
go

create table Bill
(
	id int IDENTITY PRIMARY KEY,
	DateCheckIn DateTime not null,
	DateCheckOut DateTime,
	idTable int not null,
	status int not null default 0 --1: đã thanh toán || 0: chưa thanh toán

	foreign key(idTable) references dbo.TableFood(id)
)
go

create table BillInfo
(
	id int IDENTITY PRIMARY KEY,
	idBill int not null,
	idFood int not null,
	count int not null default 0,

	foreign key (idBill) references dbo.Bill(id),
	foreign key (idFood) references dbo.Food(id)
)
go


create proc USP_InsertBill
@idTable int
as
begin
	INSERT dbo.Bill(DateCheckIn,DateCheckOut,idTable,status,discount)
	Values(GETDATE(),null,@idTable,0,0)
end
go

create proc USP_InsertBillInfo
@idBill int, @idFood int, @count int
as
begin
	declare @isExitsBillInfo int;
	declare @foodCount int = 1

	SELECT @isExitsBillInfo = id, @foodCount = b.count 
	from dbo.BillInfo as b 
	where idBill = @idBill and idFood= @idFood

	if(@isExitsBillInfo > 0)
	begin
		declare @newCount int = @foodCount + @count
		if(@newCount >0)
			update dbo.BillInfo set count =  @foodCount + @count where idFood=@idFood
		else
			Delete dbo.BillInfo where idBill = @idBill and idFood = @idFood
	end
	else
		begin
			INSERT dbo.BillInfo(idBill,idFood,count)
			values(@idBill,@idFood,@count)
		end

end
go

select Max(id) from dbo.Bill

update dbo.Bill set status = 1 where id = 1

go


delete dbo.BillInfo

delete dbo.Bill


go

alter trigger UTG_UpdateBillInfo
on dbo.BillInfo for insert,update
as
begin
	declare @idBill int

	select @idBill = idBill from inserted

	declare @idTable int

	select @idTable = idTable from dbo.Bill where id=@idBill and status = 0

	declare @count int
	select @count = count(*) from dbo.BillInfo where idBill = @idBill

	if(@count > 0)
	begin
		update dbo.TableFood set status = N'Có người' where id = @idTable
	end	
	else
	begin
		update dbo.TableFood set status = N'Trống' where id = @idTable
	end
	
end
go




create trigger UTG_UpdateBill
on dbo.Bill for update
as
begin
	declare @idBill int

	select @idBill = id from inserted
	
	declare @idTable int

	select @idTable = idTable from dbo.Bill where id=@idBill

	declare @count int = 0

	select @count = count(*) from dbo.Bill where idTable = @idTable and status = 0

	if(@count = 0)
		Update dbo.TableFood set status = N'Trống' where id = @idTable
end
go



select * from dbo.Bill

--
go
alter proc USP_SwitchTable
 @idTable1 int , @idTable2 int
as 
begin
	
	declare @idFirstBill int
	declare @idSecondBill int

	declare @isFirstTableEmty int = 1
	declare @isSecondTableEmty int = 1

	select @idSecondBill = id from dbo.Bill where idTable = @idTable2 and status = 0
	select @idFirstBill = id from dbo.Bill where idTable = @idTable1 and status = 0



	if(@idFirstBill is null)
	begin
		insert dbo.Bill(DateCheckIn,DateCheckOut,idTable,status)
		values(GETDATE(),null,@idTable1,0)

		select @idFirstBill = MAX(id) from dbo.Bill where idTable = @idTable1 and status = 0
		
		
	end
	
	select @isFirstTableEmty = count(*) from dbo.BillInfo where idBill = @idFirstBill

	if(@idSecondBill is null)
	begin
		insert dbo.Bill(DateCheckIn,DateCheckOut,idTable,status)
		values(GETDATE(),null,@idTable2,0)		
		select @idSecondBill = MAX(id) from dbo.Bill where idTable = @idTable2 and status = 0
		
		
	end

	select @isSecondTableEmty = count(*) from dbo.BillInfo where idBill = @idSecondBill

	select id into IDBillInfoTable from dbo.BillInfo where idBill = @idSecondBill

	update dbo.BillInfo set idBill = @idSecondBill where idBill = @idFirstBill

	update dbo.BillInfo set idBill = @idFirstBill where id in (select * from IDBillInfoTable)

	drop table IDBillInfoTable

	if(@isFirstTableEmty =0 )
		update dbo.TableFood set status = N'Trống' where id = @idTable2

	if(@isSecondTableEmty =0 )
		update dbo.TableFood set status = N'Trống' where id = @idTable1
end
go

alter proc USP_GetListBillByDate
@checkIn date, @checkOut date
as
begin	
	select t.name as [Tên bàn], b.totalPrice as [Tổng tiền], DateCheckIn as [Ngày vào], DateCheckOut as [Ngày ra], discount as [giảm giá]
	from Bill  as b, TableFood as t
	where DateCheckIn >= @checkIn and DateCheckOut <= @checkOut and b.status =1
	and t.id = b.idTable 
end
go

create proc USP_UpdateAccount
@userName nvarchar(100),@displayName nvarchar(100), @password nvarchar(100), @newPassword nvarchar(100)
as
begin
	declare @isRightPass int = 0

	select @isRightPass = count (*) from dbo.Account where UserName = @userName and PassWord = @password

	if(@isRightPass =1 )
	begin
		if(@newPassword = null or @newPassword = '')
		begin
			update Account set DisplayName = @displayName where	UserName = @userName

		end
		else
			update Account set DisplayName = @displayName, PassWord =@newPassword where UserName = @userName
	end
end


update dbo.Food set name = N'aa', idCategory = 1 , price = 0 where id =1

select * from Food

go
create trigger UTG_DeleteBillInfo
on dbo.BillInfo for Delete
as
begin
	declare @idBillInfo int
	declare @idBill int
	select @idBillInfo = id, @idBillInfo = deleted.idBill from deleted

	declare @idTable int
	select @idTable = idTable from dbo.Bill where id = @idBill

	declare @count int = 0
	
	select @count = count (*) from BillInfo as bi,Bill as b where b.id = bi.idBill and b.id = @idBill and status = 0

	if(@count >0)
		update dbo.TableFood set status = N'Trống' where id = @idTable

end
