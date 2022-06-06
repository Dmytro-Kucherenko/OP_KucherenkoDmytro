DROP TABLE dbo.Clients;
DROP TABLE dbo.Dealer;
DROP TABLE dbo.Car;
DROP TABLE dbo.Contract;

CREATE TABLE dbo.Clients(
	IDClient int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Clients PRIMARY KEY (IDClient),
	Name varchar(20) NOT NULL,
	Surname varchar(20) NOT NULL,
	SecondName varchar(20) NULL,
	Town varchar(20) NOT NULL,
	Adress varchar(20) NOT NULL,
	Phone varchar(20) NOT NULL,
)
CREATE TABLE dbo.Dealer(
	IDDealer int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Dealer PRIMARY KEY (IDDealer),
	Name varchar(20) NOT NULL,
	Surname varchar(20) NOT NULL,
	SecondName varchar(20) NULL,
	Photo image NULL,
	Adress varchar(20) NOT NULL,
	Phone varchar(20) NOT NULL,
)
CREATE TABLE dbo.Car(
	IDCar int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Car PRIMARY KEY (IDCar),
	Model varchar(30) NOT NULL,
	Photo image NULL,
	Date date NOT NULL,
	Run int NOT NULL,
	Com int NOT NULL,
	Price int NOT NULL,
	IDDealer int NOT NULL CONSTRAINT FK_Car_Dealer FOREIGN KEY(IDDealer) REFERENCES dbo.Dealer (IDDealer),
	Status bit NOT NULL,
);
CREATE TABLE dbo.Contract(
	IDContract int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Contract PRIMARY KEY (IDContract),
	IDClient int NOT NULL CONSTRAINT FK_Contract_Clients FOREIGN KEY(IDClient) REFERENCES dbo.Clients (IDClient),
	IDDealer int NOT NULL CONSTRAINT FK_Contract_Dealer FOREIGN KEY(IDDealer) REFERENCES dbo.Dealer (IDDealer),
	IDCar int NOT NULL CONSTRAINT FK_Contract_Car FOREIGN KEY(IDCar) REFERENCES dbo.Car (IDCar),
	Date date NOT NULL DEFAULT GETDATE(),
	Feedback nvarchar(500) NULL,
)

