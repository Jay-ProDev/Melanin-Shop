CREATE TABLE Member(
   Id_Member INT,
   FirstName VARCHAR(50) NOT NULL,
   LastName VARCHAR(50) NOT NULL,
   Email VARCHAR(50) NOT NULL,
   PasswordHash VARCHAR(255) NOT NULL,
   Role VARCHAR(50),
   CreatedAt DATETIME,
   PRIMARY KEY(Id_Member),
   UNIQUE(Email)
);

CREATE TABLE Address(
   Id_Address INT,
   City VARCHAR(50) NOT NULL,
   PostalCode INT NOT NULL,
   Country VARCHAR(50) NOT NULL,
   Phone VARCHAR(20),
   Street VARCHAR(50) NOT NULL,
   FullName VARCHAR(100),
   Id_Member INT NOT NULL,
   PRIMARY KEY(Id_Address),
   FOREIGN KEY(Id_Member) REFERENCES Member_(Id_Member)
);

CREATE TABLE Category(
   Id_Category INT,
   Name VARCHAR(50) NOT NULL,
   Slug VARCHAR(50) NOT NULL,
   PRIMARY KEY(Id_Category)
);

CREATE TABLE Product(
   Id_Product INT,
   Name VARCHAR(50) NOT NULL,
   Description VARCHAR(500),
   UnitPrice DECIMAL(10,2) NOT NULL,
   StockQuantity INT NOT NULL,
   CreatedAt DATETIME,
   IsActive BIT NOT NULL,
   HairLength VARCHAR(20),
   HairTexture VARCHAR(20),
   HairColor VARCHAR(50),
   CapSize VARCHAR(20),
   Id_Category INT NOT NULL,
   PRIMARY KEY(Id_Product),
   FOREIGN KEY(Id_Category) REFERENCES Category(Id_Category)
);

CREATE TABLE CartItem(
   Id_CartItem INT,
   Quantity INT NOT NULL,
   UnitPrice DECIMAL(10,2) NOT NULL,
   Id_Product INT NOT NULL,
   Id_Member INT NOT NULL,
   PRIMARY KEY(Id_CartItem),
   FOREIGN KEY(Id_Product) REFERENCES Product(Id_Product),
   FOREIGN KEY(Id_Member) REFERENCES Member_(Id_Member)
);

CREATE TABLE Order_(
   Id_Order INT,
   TotalPrice DECIMAL(10,2) NOT NULL,
   CreatedAt DATETIME,
   Status VARCHAR(50) NOT NULL,
   ShippingAddressId INT NOT NULL,
   BillingAddressId INT NULL,
   CouponCode VARCHAR(50) NULL,
   Id_Member INT NOT NULL,
   PRIMARY KEY(Id_Order),
   FOREIGN KEY(ShippingAddressId) REFERENCES Address(Id_Address),
   FOREIGN KEY(BillingAddressId) REFERENCES Address(Id_Address),
   FOREIGN KEY(Id_Member) REFERENCES Member_(Id_Member)
);

CREATE TABLE OrderItem(
   Id_OrderItem INT,
   Quantity INT NOT NULL,
   UnitPrice DECIMAL(10,2) NOT NULL,
   Id_Order INT NOT NULL,
   Id_Product INT NOT NULL,
   PRIMARY KEY(Id_OrderItem),
   FOREIGN KEY(Id_Order) REFERENCES Order_(Id_Order),
   FOREIGN KEY(Id_Product) REFERENCES Product(Id_Product)
);

CREATE TABLE Payment(
   Id_Payment VARCHAR(50),
   Amount DECIMAL(10,2) NOT NULL,
   Status VARCHAR(50) NOT NULL,
   PaidAt DATETIME,
   StripeSessionId VARCHAR(255),
   Id_Order INT NOT NULL,
   PRIMARY KEY(Id_Payment),
   UNIQUE(Id_Order),
   FOREIGN KEY(Id_Order) REFERENCES Order_(Id_Order)
);