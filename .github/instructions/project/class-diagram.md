# Diagram Klas dla Systemu Zarządzania Fakturami

Poniższy diagram przedstawia relacje między głównymi encjami w systemie zarządzania fakturami.

## Diagram Głównych Encji

```mermaid
classDiagram
    User "1" -- "many" Invoice: posiada
    Invoice "1" -- "many" LineItem: zawiera
    User "1" -- "many" ChangeRequest: składa
    User "1" -- "many" RefundRequest: składa
    LineItem "1" -- "many" ChangeRequest: dotyczy
    Invoice "1" -- "many" RefundRequest: dotyczy
    RefundRequest "1" -- "many" DocumentAttachment: ma załączniki
    User "1" -- "many" Notification: otrzymuje
    User "1" -- "many" NotificationSettings: konfiguruje
    
    class User {
        +int UserID
        +string Email
        +string PasswordHash
        +string FirstName
        +string LastName
        +DateTime CreatedDate
        +DateTime LastLoginDate
        +bool IsActive
        +bool IsAdmin
        +authenticate()
        +updateProfile()
        +changePassword()
    }
    
    class Invoice {
        +int InvoiceID
        +string InvoiceNumber
        +int UserID
        +DateTime IssueDate
        +DateTime DueDate
        +decimal TotalAmount
        +decimal TaxAmount
        +string Status
        +DateTime PaymentDate
        +string Notes
        +DateTime CreatedDate
        +DateTime LastModifiedDate
        +calculateTotal()
        +markAsPaid()
        +exportToPdf()
    }
    
    class LineItem {
        +int LineItemID
        +int InvoiceID
        +string Description
        +int Quantity
        +decimal UnitPrice
        +decimal TaxRate
        +decimal TotalPrice
        +DateTime CreatedDate
        +DateTime LastModifiedDate
        +calculateTotalPrice()
    }
    
    class ChangeRequest {
        +int ChangeRequestID
        +int UserID
        +int LineItemID
        +DateTime RequestDate
        +int RequestedQuantity
        +decimal RequestedUnitPrice
        +string RequestedDescription
        +string Reason
        +string Status
        +string AdminNotes
        +int ReviewedBy
        +DateTime ReviewDate
        +approve()
        +reject()
    }
    
    class RefundRequest {
        +int RefundRequestID
        +int UserID
        +int InvoiceID
        +DateTime RequestDate
        +string Reason
        +string Status
        +string AdminNotes
        +int ReviewedBy
        +DateTime ReviewDate
        +decimal RefundAmount
        +DateTime RefundDate
        +approve()
        +reject()
        +processRefund()
    }
    
    class DocumentAttachment {
        +int AttachmentID
        +int RefundRequestID
        +string FileName
        +int FileSize
        +string FileType
        +byte[] FileContent
        +DateTime UploadDate
        +downloadFile()
    }
    
    class NotificationSettings {
        +int UserID
        +bool EmailNotificationsEnabled
        +bool WebNotificationsEnabled
        +bool InvoiceNotifications
        +bool ChangeRequestNotifications
        +bool RefundRequestNotifications
        +updateSettings()
    }
    
    class Notification {
        +int NotificationID
        +int UserID
        +string Title
        +string Message
        +DateTime CreatedDate
        +bool IsRead
        +int RelatedEntityID
        +string RelatedEntityType
        +markAsRead()
    }
    
    class AuditLog {
        +int LogID
        +int UserID
        +string ActionType
        +string EntityType
        +int EntityID
        +DateTime ActionDate
        +string OldValue
        +string NewValue
        +createLogEntry()
    }
```

## Diagram Relacji Użytkownika z Systemem

```mermaid
classDiagram
    User <|-- RegularUser : jest typem
    User <|-- Administrator : jest typem
    
    RegularUser -- Invoice : zarządza
    RegularUser -- ChangeRequest : składa
    RegularUser -- RefundRequest : składa
    
    Administrator -- User : zarządza
    Administrator -- ChangeRequest : przetwarza
    Administrator -- RefundRequest : przetwarza
    
    class User {
        +int UserID
        +string Email
        +string PasswordHash
        +string FirstName
        +string LastName
        +DateTime CreatedDate
        +DateTime LastLoginDate
        +bool IsActive
        +bool IsAdmin
    }
    
    class RegularUser {
        +viewInvoices()
        +requestChange()
        +requestRefund()
        +trackRequests()
    }
    
    class Administrator {
        +manageUsers()
        +reviewChangeRequests()
        +reviewRefundRequests()
        +generateReports()
    }
    
    class Invoice {
        +int InvoiceID
        +string InvoiceNumber
        +decimal TotalAmount
        +string Status
    }
    
    class ChangeRequest {
        +int ChangeRequestID
        +string Status
        +DateTime RequestDate
    }
    
    class RefundRequest {
        +int RefundRequestID
        +string Status
        +DateTime RequestDate
    }
```

## Diagram Procesu Wniosków o Zmiany

```mermaid
classDiagram
    ChangeRequest "*" -- "1" LineItem : dotyczy
    ChangeRequest "*" -- "1" User : składany przez
    ChangeRequest "*" -- "1" User : przeglądany przez
    
    class ChangeRequest {
        +int ChangeRequestID
        +int UserID
        +int LineItemID
        +DateTime RequestDate
        +int RequestedQuantity
        +decimal RequestedUnitPrice
        +string RequestedDescription
        +string Reason
        +string Status
        +string AdminNotes
        +int ReviewedBy
        +DateTime ReviewDate
        +submit()
        +approve()
        +reject()
        +updateStatus()
    }
    
    class LineItem {
        +int LineItemID
        +int InvoiceID
        +string Description
        +int Quantity
        +decimal UnitPrice
        +decimal TaxRate
        +decimal TotalPrice
        +applyChanges(ChangeRequest)
    }
    
    class User {
        +int UserID
        +string FullName
        +bool IsAdmin
        +requestChange(LineItem)
        +reviewChangeRequest(ChangeRequest)
    }
```

## Diagram Procesu Wniosków o Zwroty

```mermaid
classDiagram
    RefundRequest "*" -- "1" Invoice : dotyczy
    RefundRequest "*" -- "1" User : składany przez
    RefundRequest "*" -- "1" User : przeglądany przez
    RefundRequest "1" -- "*" DocumentAttachment : zawiera
    
    class RefundRequest {
        +int RefundRequestID
        +int UserID
        +int InvoiceID
        +DateTime RequestDate
        +string Reason
        +string Status
        +string AdminNotes
        +int ReviewedBy
        +DateTime ReviewDate
        +decimal RefundAmount
        +DateTime RefundDate
        +submit()
        +approve()
        +reject()
        +processRefund()
    }
    
    class Invoice {
        +int InvoiceID
        +string InvoiceNumber
        +decimal TotalAmount
        +string Status
        +markAsRefunded()
    }
    
    class User {
        +int UserID
        +string FullName
        +bool IsAdmin
        +requestRefund(Invoice)
        +reviewRefundRequest(RefundRequest)
    }
    
    class DocumentAttachment {
        +int AttachmentID
        +int RefundRequestID
        +string FileName
        +int FileSize
        +string FileType
        +byte[] FileContent
        +DateTime UploadDate
        +upload()
        +download()
    }
```
