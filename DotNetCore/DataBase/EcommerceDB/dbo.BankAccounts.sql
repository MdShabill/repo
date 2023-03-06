CREATE TABLE BankAccounts
(
	Id int Identity(1,1),
	BankName NVarchar (100) Not null,
	BranchName NVarchar (100) Not Null,
	IfscCode NVarchar (100) Not Null,
	AccountNumber int Not Null,
	AccountType int Not Null,
	AccountHolder1Name NVarchar (100) Not Null,
	AccountHolder2Name NVarchar (100) Not Null,
	Holder1Email NVarchar (100) Not Null,
	Holder2Email NVarchar (100) Not Null,
	Holder1Address NVarchar (100) Not null,
	Holder2Address NVarchar (100) Not Null,
	CompanyName NVarchar (100) Not Null,
	GSTNo NVarchar (100) Not Null,
	PRIMARY KEY (Id),
	CONSTRAINT UQ_BankAccounts_AccountNumber UNIQUE (AccountNumber),
	CONSTRAINT UQ_BankAccounts_Holder1Email UNIQUE (Holder1Email),
)


