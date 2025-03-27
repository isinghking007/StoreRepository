use StoreRepository


create table AmountDue(
DueID int primary key identity(1,1),
CustomerId int,
TotalBillAmount varchar(100),
PaidAmount varchar(100),
DueAmount varchar(100),
ModifiedDate date,
FileLocation varchar(200),
FileName varchar(200),
FOREIGN KEY(CustomerId) References CustomerDetails(CustomerId)
);

create table AmountPaid(
PaidID int primary key identity(1,1),
CustomerId int,
TotalAmount varchar(100),
PaidAmount varchar(100),
RemainingAmount varchar(100),
ModifiedDate date,
FileLocation varchar(200),
FileName varchar(200),
FOREIGN KEY(CustomerId) References CustomerDetails(CustomerId)
);