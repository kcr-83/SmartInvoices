-- Database schema for Smart Invoices System
-- This schema supports all the functionality outlined in the feature requirements

-- Create Users table to store user information
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    LastLoginDate DATETIME NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    IsAdmin BIT NOT NULL DEFAULT 0
);

-- Create Invoices table to store invoice information
CREATE TABLE Invoices (
    InvoiceID INT IDENTITY(1,1) PRIMARY KEY,
    InvoiceNumber NVARCHAR(50) NOT NULL UNIQUE,
    UserID INT NOT NULL,
    IssueDate DATETIME NOT NULL,
    DueDate DATETIME NOT NULL,
    TotalAmount DECIMAL(18,2) NOT NULL,
    TaxAmount DECIMAL(18,2) NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Issued', -- Issued, Paid, Overdue, Cancelled
    PaymentDate DATETIME NULL,
    Notes NVARCHAR(500) NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    LastModifiedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Invoices_Users FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- Create index for frequently searched columns
CREATE INDEX IX_Invoices_UserID ON Invoices(UserID);
CREATE INDEX IX_Invoices_Status ON Invoices(Status);
CREATE INDEX IX_Invoices_IssueDate ON Invoices(IssueDate);

-- Create LineItems table to store invoice line items
CREATE TABLE LineItems (
    LineItemID INT IDENTITY(1,1) PRIMARY KEY,
    InvoiceID INT NOT NULL,
    Description NVARCHAR(200) NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    TaxRate DECIMAL(5,2) NOT NULL,
    TotalPrice DECIMAL(18,2) NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    LastModifiedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_LineItems_Invoices FOREIGN KEY (InvoiceID) REFERENCES Invoices(InvoiceID)
);

-- Create index for line items by invoice
CREATE INDEX IX_LineItems_InvoiceID ON LineItems(InvoiceID);

-- Create ChangeRequests table to store requests for changes to line items
CREATE TABLE ChangeRequests (
    ChangeRequestID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    LineItemID INT NOT NULL,
    RequestDate DATETIME NOT NULL DEFAULT GETDATE(),
    RequestedQuantity INT NULL,
    RequestedUnitPrice DECIMAL(18,2) NULL,
    RequestedDescription NVARCHAR(200) NULL,
    Reason NVARCHAR(500) NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending', -- Pending, Approved, Rejected
    AdminNotes NVARCHAR(500) NULL,
    ReviewedBy INT NULL,
    ReviewDate DATETIME NULL,
    CONSTRAINT FK_ChangeRequests_Users FOREIGN KEY (UserID) REFERENCES Users(UserID),
    CONSTRAINT FK_ChangeRequests_LineItems FOREIGN KEY (LineItemID) REFERENCES LineItems(LineItemID),
    CONSTRAINT FK_ChangeRequests_Reviewers FOREIGN KEY (ReviewedBy) REFERENCES Users(UserID)
);

-- Create index for change requests
CREATE INDEX IX_ChangeRequests_UserID ON ChangeRequests(UserID);
CREATE INDEX IX_ChangeRequests_LineItemID ON ChangeRequests(LineItemID);
CREATE INDEX IX_ChangeRequests_Status ON ChangeRequests(Status);

-- Create RefundRequests table to store requests for full refunds
CREATE TABLE RefundRequests (
    RefundRequestID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    InvoiceID INT NOT NULL,
    RequestDate DATETIME NOT NULL DEFAULT GETDATE(),
    Reason NVARCHAR(500) NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending', -- Pending, Approved, Rejected
    AdminNotes NVARCHAR(500) NULL,
    ReviewedBy INT NULL,
    ReviewDate DATETIME NULL,
    RefundAmount DECIMAL(18,2) NULL,
    RefundDate DATETIME NULL,
    CONSTRAINT FK_RefundRequests_Users FOREIGN KEY (UserID) REFERENCES Users(UserID),
    CONSTRAINT FK_RefundRequests_Invoices FOREIGN KEY (InvoiceID) REFERENCES Invoices(InvoiceID),
    CONSTRAINT FK_RefundRequests_Reviewers FOREIGN KEY (ReviewedBy) REFERENCES Users(UserID)
);

-- Create index for refund requests
CREATE INDEX IX_RefundRequests_UserID ON RefundRequests(UserID);
CREATE INDEX IX_RefundRequests_InvoiceID ON RefundRequests(InvoiceID);
CREATE INDEX IX_RefundRequests_Status ON RefundRequests(Status);

-- Create DocumentAttachments table for storing attachments related to refund requests
CREATE TABLE DocumentAttachments (
    AttachmentID INT IDENTITY(1,1) PRIMARY KEY,
    RefundRequestID INT NOT NULL,
    FileName NVARCHAR(255) NOT NULL,
    FileSize INT NOT NULL,
    FileType NVARCHAR(100) NOT NULL,
    FileContent VARBINARY(MAX) NOT NULL,
    UploadDate DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_DocumentAttachments_RefundRequests FOREIGN KEY (RefundRequestID) REFERENCES RefundRequests(RefundRequestID)
);

-- Create NotificationSettings table for user preferences on notifications
CREATE TABLE NotificationSettings (
    UserID INT PRIMARY KEY,
    EmailNotificationsEnabled BIT NOT NULL DEFAULT 1,
    WebNotificationsEnabled BIT NOT NULL DEFAULT 1,
    InvoiceNotifications BIT NOT NULL DEFAULT 1,
    ChangeRequestNotifications BIT NOT NULL DEFAULT 1,
    RefundRequestNotifications BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_NotificationSettings_Users FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- Create Notifications table for storing user notifications
CREATE TABLE Notifications (
    NotificationID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    Title NVARCHAR(100) NOT NULL,
    Message NVARCHAR(500) NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    IsRead BIT NOT NULL DEFAULT 0,
    RelatedEntityID INT NULL,
    RelatedEntityType NVARCHAR(50) NULL, -- Invoice, ChangeRequest, RefundRequest
    CONSTRAINT FK_Notifications_Users FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- Create index for notifications
CREATE INDEX IX_Notifications_UserID ON Notifications(UserID);
CREATE INDEX IX_Notifications_IsRead ON Notifications(IsRead);

-- Create AuditLog table for tracking changes
CREATE TABLE AuditLog (
    LogID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    ActionType NVARCHAR(50) NOT NULL, -- Create, Update, Delete, View
    EntityType NVARCHAR(50) NOT NULL, -- User, Invoice, LineItem, ChangeRequest, RefundRequest
    EntityID INT NOT NULL,
    ActionDate DATETIME NOT NULL DEFAULT GETDATE(),
    OldValue NVARCHAR(MAX) NULL,
    NewValue NVARCHAR(MAX) NULL,
    CONSTRAINT FK_AuditLog_Users FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- Create index for audit log
CREATE INDEX IX_AuditLog_UserID ON AuditLog(UserID);
CREATE INDEX IX_AuditLog_EntityType_EntityID ON AuditLog(EntityType, EntityID);
CREATE INDEX IX_AuditLog_ActionDate ON AuditLog(ActionDate);
