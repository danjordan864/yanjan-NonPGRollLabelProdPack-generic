# Roll Label Production & Packing System - Requirements Document

**System Name:** NonPG Roll Label Production & Packing System
**Version:** Based on code analysis as of December 2025
**Document Type:** Reverse-Engineered Requirements Specification

---

## 1. Executive Summary

### 1.1 System Purpose
The Roll Label Production & Packing System is a manufacturing execution application that manages film roll production, labeling, packing, and inventory tracking for a manufacturing facility. It serves as a bridge between the shop floor and SAP Business One ERP system, supporting multiple customer-specific workflows (Medline, Rockline, and generic P&G production).

### 1.2 Key Business Problems Solved
- Automated roll production tracking with unique identification (SSCC/LUID)
- Real-time inventory synchronization with SAP Business One
- Multi-customer production workflows with different measurement standards
- Automated label printing with BarTender integration
- Planned material consumption and shortage tracking
- Scrap and quality management
- Packing and shipping documentation

---

## 2. Functional Requirements

### 2.1 Customer Selection & System Startup

**FR-2.1.1: Multi-Customer Startup Interface**
- **Description:** System shall provide a startup form allowing operators to select customer-specific workflows
- **Supported Customers:**
  - Medline (Customer ID: C1007)
  - Rockline (Customer ID: C1020)
  - Generic/No Customer Selected
- **Launch Modes (Command-line arguments):**
  - `MEDLINEPACK` - Launches Medline packing form
  - `ROCKLINEPACK` - Launches Rockline packing form
  - `MEDLINEFILM` - Launches Medline film production
  - `ROCKLINEFILM` - Launches Rockline film production
  - No argument - Launches customer selection form
- **UI Behavior:** System shall dynamically load customer-specific user control panels based on selection

### 2.2 Film Roll Production

#### 2.2.1 Production Order Management

**FR-2.2.1.1: Order Retrieval**
- System shall retrieve production orders from SAP Business One with status 'R' (Released)
- Orders shall be filtered by production line when specified
- System shall exclude Item Group Code 103 from production orders
- Order data shall include:
  - SAP Order Number (DocNum)
  - YJN Order ID (custom field)
  - Item Code and Description
  - IRMS number
  - Production line code
  - Start Date and Due Date
  - Current die number
  - Number of slit positions
  - Jumbo roll number
  - Material code and product name
  - Batch number

**FR-2.2.1.2: Order Selection Interface**
- System shall display available orders in a selectable list
- Operators shall be able to select an order to begin production
- System shall display order details: item code, item name, IRMS, target rolls, inventory rolls, rolls left to produce

#### 2.2.2 Production Setup

**FR-2.2.2.1: Slit Position Configuration**
- System shall allow operators to configure active slit positions (typically 1-11)
- Each slit position can be enabled/disabled individually
- System shall support a default number of slit positions from configuration (DefaultNoOfSlitPositions)

**FR-2.2.2.2: Production Parameters Entry**
- Operators shall enter the following parameters:
  - **Medline/Rockline:** Linear Meters (LM)
  - **Generic/P&G:** Weight in Kilograms per slit
  - Die number (aperture die)
  - Shift identifier (A, B, C, etc.)
  - Employee/operator ID

**FR-2.2.2.3: Measurement Unit Handling**
- **Medline Requirements:**
  - Uses Linear Meters and SquareMeters
  - Formula: `SquareMeters = WidthInMM × 0.001 × LinearMeters`
  - Width retrieved from production order item details
- **Rockline Requirements:**
  - Uses Quantity instead of SquareMeters (recent change as of Nov 2025)
  - System shall populate Quantity OR SquareMeters based on stored procedure column availability
  - Rounds inventory receipt quantities
- **Generic/P&G Requirements:**
  - Uses weight in Kilograms
  - Validates against min/max roll weight from configuration
  - Calculates number of rolls based on weight per slit

#### 2.2.3 Roll Generation

**FR-2.2.3.1: Roll Number Generation**
- System shall generate unique roll numbers using format:
  - `{Year}{Month}{Date}{DieNo}{Shift}{JumboRollNo}{SlitPosition}`
  - Example: `9A11T2001001` = Year:9, Month:A, Date:11, Die:T2, JumboRoll:00, Shift:1, Slit:001
- Year shall be single digit (last digit of year)
- Month shall be represented as: 1-9 for Jan-Sep, A for Oct, B for Nov, C for Dec
- Date shall be zero-padded to 2 digits
- JumboRollNo shall be zero-padded to 2 digits
- SlitPosition shall be zero-padded to 3 digits

**FR-2.2.3.2: SSCC and LUID Generation**
- System shall generate unique Serial Shipping Container Codes (SSCC) for each roll
- System shall generate Logistic Unit IDs (LUID) for inventory tracking
- Both identifiers shall be created via stored procedure call: `CreateSSCC()`
- SSCC shall follow GS1 standards with proper check digit calculation

**FR-2.2.3.3: Roll Data Calculation**
- For each generated roll, system shall calculate:
  - Gross weight (Kgs) = Net weight + Tare weight + Adjustment
  - Square meters (for Medline)
  - Quantity (for Rockline)
  - Quality status (RELEASED or HOLD based on configuration)
  - Storage location (output location or hold area)

#### 2.2.4 Production Execution

**FR-2.2.4.1: SAP Inventory Receipt**
- System shall create Inventory General Entry (Receipt) document in SAP
- For each roll, system shall add receipt line with:
  - Item code
  - Quantity (Kgs, SquareMeters, or Quantity depending on customer)
  - Batch number (roll number)
  - Storage location code
  - Quality status (RELEASED or HOLD)
  - LUID
  - SSCC
  - Unit of measure
  - Employee
  - Shift
- System shall support transaction mode (configurable)
- System shall rollback on error when configured

**FR-2.2.4.2: Material Issue Processing**
- System shall issue raw materials from production line input location
- Material issues shall be based on:
  - Production order bill of materials
  - Available batch quantities
  - Production quantity
- System shall prioritize batches by:
  1. Quantity descending
  2. Expiry date ascending (if applicable)
- System shall create Inventory General Exit (Issue) document in SAP
- System shall record material shortages when insufficient inventory exists

**FR-2.2.4.3: Jumbo Roll Counter**
- System shall increment jumbo roll counter after successful production
- Counter stored in production order custom field (U_SII_JumboRollNo)
- Incremented via stored procedure: `_sii_rpr_spu_incrementJumboRoll`

**FR-2.2.4.4: Production Status Tracking**
- System shall update inventory roll count after production
- System shall calculate and display rolls left to produce (Target - Inventory)
- System shall prevent over-production beyond target quantity

### 2.3 Label Printing

#### 2.3.1 BarTender Integration

**FR-2.3.1.1: Roll Label Printing**
- System shall print roll labels via BarTender command-line interface
- Print process:
  1. Generate trigger file with roll data (CSV format)
  2. Execute BarTender with format template path
  3. Delete trigger file after print completion
- Trigger file format: `ItemCode,ItemName,IRMS,LotNo,RollNo,SSCC,Qty,PONumber`
- Print command: `Bartend.exe /AF="{format}" /D="{trigger}" /PRN="{printer}" /R=3 /P /DD`

**FR-2.3.1.2: Label Format Configuration**
- System shall support multiple label formats via configuration:
  - MedlineDefaultRollLabelFormat: Medline roll labels
  - RocklineDefaultRollLabelFormat: Rockline roll labels (includes SSCC as of Nov 2025)
  - MedlineDefaultPackLabelFormat: Medline packing labels
  - RocklineDefaultPackLabelFormat: Rockline packing labels
  - PGDefaultCombLabelFormat: Generic combined labels
  - PGDefaultScrapLabelFormat: Scrap labels
  - PGDefaultPackLabelFormat: Generic pack labels
  - PGDefaultResmixLabelFormat: Resmix labels
- Format paths shall be configurable in App.config

**FR-2.3.1.3: Print Location Management**
- System shall support configurable trigger file locations:
  - PrintLocRollLabel4by6
  - PrintLocRollLabel1by6
  - PrintLocPack
  - BTTriggerLoc
- System shall write trigger files to configured locations

#### 2.3.2 Pack Label Printing

**FR-2.3.2.1: Pack Label Generation**
- System shall retrieve unprinted packing labels from database
- Pack labels shall include:
  - Bundle/Pallet SSCC with check digit
  - Item code and name
  - IRMS number
  - YJN Order ID
  - SAP Order Number
  - Pallet number
  - Gross weight and net weight
  - Number of rolls/units
  - Production date
- Operators shall be able to edit weights before printing

**FR-2.3.2.2: Pack Label Printing Process**
- System shall support configurable number of copies (default: 4)
- System shall mark labels as printed after successful print
- System shall create bundle records in database: `_sii_rpr_spi_addNewBundle`
- Bundle SSCC format: `112300000034{NNNNNN}` + check digit
  - NNNNNN = 6-digit pallet number
  - Check digit calculated via GS1 algorithm

### 2.4 Scrap Management

**FR-2.4.1: Roll Scrap Processing**
- System shall allow operators to mark produced rolls as scrap
- Scrap reasons shall be configurable and selectable from dropdown
- Scrap process shall:
  1. Issue rolls from current storage location
  2. Receive rolls into scrap location (config: ScrapLocCode)
  3. Update roll quality status to SCRAP
  4. Update production order inventory count
  5. Print scrap labels

**FR-2.4.2: Box Scrap Processing**
- System shall support customer-specific box scrap forms:
  - FrmBoxScrapMedline: Medline box scrap
  - FrmBoxScrapRockline: Rockline box scrap
  - FrmBoxScrap: Generic box scrap
- Box scrap shall record:
  - Scrap quantity
  - Scrap reason
  - Scrap date/time
  - Operator

### 2.5 Reprint Functionality

**FR-2.5.1: Roll Label Reprint**
- System shall allow reprinting of existing roll labels
- Operators shall select rolls from order to reprint
- System shall retrieve roll data from inventory:
  - Roll number (batch)
  - SSCC
  - Quantity/Weight
  - Item details
- Reprint shall not create new inventory transactions

**FR-2.5.2: Pack Label Reprint**
- System shall allow reprinting of pack labels
- Pack labels can be reprinted multiple times
- Reprint shall retrieve existing bundle data from database

### 2.6 Resmix Adjustment

**FR-2.6.1: Resmix Material Adjustment**
- System shall support resmix material adjustments via FrmAdjustResmix
- Resmix routing configuration (from config):
  - Resmix001ToLines: "FF1,FF2,FF4,FF5,FF6"
  - Resmix002ToLines: "FF3,FF4"
- Adjustments shall update material quantities for production orders
- System shall issue/receive resmix materials from configured locations

### 2.7 Hold/Release Management

**FR-2.7.1: Hold Area Management**
- Rolls can be placed on HOLD status during production
- Hold status configurable: `HoldStatus = "HOLD"`
- Hold location configurable: `HoldLocation = "HOLDAREA"`
- Held rolls shall be tracked separately from released inventory

**FR-2.7.2: Release Processing**
- Operators shall be able to release held rolls
- Release process shall:
  1. Issue from hold location
  2. Receive into standard location
  3. Update quality status to RELEASED

### 2.8 Shipping Scan

**FR-2.8.1: Shipping Verification**
- System shall provide shipping scan form (FrmShipScan)
- Operators shall scan roll SSCCs for shipping verification
- System shall validate scanned SSCCs against database
- System shall track shipped rolls and update inventory status

---

## 3. Technical Requirements

### 3.1 Architecture

**TR-3.1.1: Application Type**
- Windows Forms desktop application (.NET Framework 4.8)
- Target platforms: x86, x64, AnyCPU
- Language: C# 7.3
- Visual Studio 2017+ compatible

**TR-3.1.2: Project Structure**
- **NonPGRollLabelProdPack**: Main UI application
  - Forms, user controls, UI logic
  - Program.cs entry point with command-line argument handling
- **RollLabelProdPack.Library**: Business logic and data access
  - Entity models (Roll, RollLabelData, Case, PackLabel, etc.)
  - Data service (AppData.cs)
  - Utilities (AppUtility.cs)
- **RollLabelProdPack.SAP**: SAP Business One integration
  - SAPB1 wrapper class
  - Document objects (InventoryReceipt, InventoryIssue, ProductionOrder)
  - SAP COM interop

### 3.2 Database Requirements

**TR-3.2.1: Database Platform**
- Microsoft SQL Server 2019
- Two databases:
  - **YanJanUS_PROD**: SAP Business One database
  - **PMX_YanJanUS_PROD**: Produmex MES database
- Connection string stored in App.config

**TR-3.2.2: SAP Business One Tables**
- **OWOR**: Production Orders
  - Custom fields: U_SII_YanJanOrderId, U_SII_CurrentDie, U_SII_NoOfSlits, U_SII_JumboRollNo, U_SII_BatchNo, U_SII_MatlCode, U_SII_ProdName, U_PMX_PLCD
- **WOR1**: Production Order Lines
- **OITM**: Items
  - Custom field: U_SII_IRMS
- **OITB**: Item Groups
- **OIGE**: Inventory General Exit (Issues)
- **IGE1**: Inventory General Exit Lines
- **OIGN**: Inventory General Entry (Receipts)
- **IGN1**: Inventory General Entry Lines
- **OBTN**: Batch Numbers
- **[@SII_PRODLINES]**: Production Lines master data (user table)
  - Fields: Code, U_SII_LineNo
- **[@SII_ROLLS]**: Rolls master data (user table)
  - Fields: Code, U_SII_ProdOrder
- **[@SII_PG_BUNDLE]**: Packing bundles (user table)
  - Fields: Name, U_SII_InternalSSCC, U_SII_ItemCode, U_SII_ItemName, U_SII_IRMS, U_SII_YJNOrder, U_SII_SSCC, U_SII_SAPOrder

**TR-3.2.3: Produmex Tables**
- **PMX_INVENTORY_REPORT_DETAIL**: Production inventory tracking
  - Fields: LUID, SSCC, ItemCode, ItemName, BatchNumber1, Quantity

**TR-3.2.4: Stored Procedures**
- `_sii_rpr_sps_getFGProdOrders`: Get production orders by line
- `_sii_rpr_sps_getIssues`: Get material issues for production
- `_sii_rpr_sps_getRollsForProdOrder_new`: Get rolls for an order
- `_sii_rpr_spu_incrementJumboRoll`: Increment jumbo roll counter
- `_sii_rpr_spu_updateLastBatch`: Update batch counters
- `_sii_rpr_spi_addNewBundle`: Create new packing bundle
- `_sii_rpr_spi_incrementPGPalletNo`: Increment pallet number
- `CreateSSCC`: Generate SSCC and LUID (assumed from code usage)

### 3.3 SAP Business One Integration

**TR-3.3.1: SAP DI API Version**
- SAP Business One DI API 9.0
- COM Interop: SAPbobsCOM
- Connection via SAP Company object

**TR-3.3.2: SAP Connection Configuration**
- Server: Configurable (default: "sql")
- Database: Configurable (default: "YanJanUS_PROD")
- Database Type: MSSQL2019 (dst_MSSQL2019)
- License Server: Configurable (default: "sql:30000")
- Language: English (ln_English)
- Multiple user credentials by production line (Line1-Line7, MIXLINE1-3, MASKLINE1-3, TUBLINE1-8)

**TR-3.3.3: Transaction Support**
- Configurable transaction mode: `UseTransactions = true/false`
- Rollback for testing: `RollBackForTesting = true/false`
- Transaction methods: StartTransaction(), CommitTransaction(), RollbackTransaction()

**TR-3.3.4: Document Creation**
- **Inventory General Entry (oInventoryGenEntry)**:
  - Receipt of produced rolls
  - Add lines with item, quantity, batch, location, LUID, SSCC
- **Inventory General Exit (oInventoryGenExit)**:
  - Issue of raw materials
  - Issue of scrap rolls
  - Add lines with item, quantity, batch, location

### 3.4 External Dependencies

**TR-3.4.1: Third-Party Libraries**
- **log4net 2.0.15**: Logging framework
- **ObjectListView 2.9.1**: Enhanced ListView control
- **HtmlRenderer.WinForms 1.5.0.6**: HTML rendering in forms

**TR-3.4.2: BarTender Integration**
- BarTender installed on client machine
- Command-line interface (Bartend.exe)
- Label format files (.btw) accessible via configured paths
- Print trigger mechanism via file system

### 3.5 Configuration Management

**TR-3.5.1: App.config Sections**
- **connectionStrings**:
  - SAPConnection: SAP database connection
  - PMXConnection: Produmex database connection
- **appSettings**:
  - Logging settings (Logging, LoggingSource, LoggingText)
  - SQL timeout (SQLCommandTimeout)
  - SAP settings (SAPSERVERNAME, SAPCOMPANY, SAPUSER, SAPPASS, SAPDBTYPE, SAPLICENSESERVER)
  - Line-specific credentials (SAPUSER_LINE1-7, SAPUSER_MIXLINE1-3, etc.)
  - Transaction settings (UseTransactions, RollBackForTesting)
  - Email settings (SMTP, alerts, errors)
  - Production settings (SupplierId, FactoryCode, DefaultItemCode, etc.)
  - Label formats and print locations
  - Inventory locations (PackingMtlLocation, ScrapLocCode, HoldLocation)
  - Quality statuses (HoldStatus, ScrapStatus, DefaultStatus)
- **userSettings**: User-specific preferences (machine number, material code, etc.)
- **log4net**: Logging configuration (file appender, rolling file, size limits)

**TR-3.5.2: Configuration Values**
- Default values for production parameters
- Batch and roll numbering sequences
- Printer names and locations
- Label format file paths
- Inventory location codes
- Customer IDs (Medline: C1007, Rockline: C1020)

### 3.6 Logging and Error Handling

**TR-3.6.1: Logging Framework**
- log4net with rolling file appender
- Log file location: `C:\Logs\RollLabelProdPack.log`
- Max file size: 10MB
- Max backup files: 10
- Log level: INFO (configurable)
- UTF-8 encoding

**TR-3.6.2: Error Handling**
- Custom B1Exception class for SAP errors
- Exception includes SAP Company object and error details
- SAP error code and message captured via GetLastError()
- Email alerts for production errors (configurable)
- Email alerts for system exceptions (configurable)

**TR-3.6.3: Email Notifications**
- **Alert Emails**:
  - Enabled via EmailAlerts = true
  - From: sap@synesisintl.com
  - To/CC: Configurable
  - Subject: "Roll Production Alert"
- **Error Emails**:
  - Enabled via EmailErrors = true
  - To/CC: Configurable
  - Subject: "Roll Production Exception"
- SMTP configuration:
  - Host: synesisintl-com.mail.protection.outlook.com
  - Port: 25
  - Credentials: Optional
  - TLS: Optional

### 3.7 Security Requirements

**TR-3.7.1: Authentication**
- Windows authentication to SQL Server (configurable)
- SAP Business One user authentication
- No application-level authentication (inherited from SAP)

**TR-3.7.2: Data Access**
- SQL connections use sa account (production: requires review)
- SAP connections use line-specific service accounts
- Passwords stored in plaintext in App.config (security concern)

**TR-3.7.3: Authorization**
- Authorization managed through SAP Business One user permissions
- No application-level authorization

### 3.8 Performance Requirements

**TR-3.8.1: Response Time**
- Production order retrieval: < 2 seconds
- Roll generation and creation: < 5 seconds per roll
- Label printing: < 10 seconds per label
- SQL command timeout: 180 seconds (configurable)

**TR-3.8.2: Scalability**
- Support up to 11 slit positions per production run
- Support multiple concurrent production lines (7+ lines)
- Handle hundreds of rolls per production order

### 3.9 Data Integrity Requirements

**TR-3.9.1: Uniqueness Constraints**
- Roll numbers must be unique within the system
- SSCC codes must be unique (GS1 standard)
- LUID must be unique for inventory tracking

**TR-3.9.2: Referential Integrity**
- Rolls must reference valid production orders
- Material issues must reference valid batches
- Inventory transactions must reference valid items and locations

**TR-3.9.3: Data Validation**
- Roll weights must be within min/max ranges (P&G production)
- Quantities must be non-negative
- SSCCs must include valid check digits
- Dates must be valid and within reasonable ranges

---

## 4. User Interface Requirements

### 4.1 Form Specifications

**UI-4.1.1: NonPGStartForm (Startup Form)**
- Customer selection dropdown (Medline, Rockline, No Selection)
- Dynamic panel loading customer-specific controls
- Simple, intuitive interface

**UI-4.1.2: FrmMainMedline / FrmMainRockline (Film Production)**
- Production order selection list/grid
- Order details display (item, IRMS, target/inventory rolls)
- Production parameter inputs:
  - Linear Meters
  - Slit position checkboxes/grid
  - Die number
  - Shift
  - Employee
- Roll generation preview grid:
  - Roll number
  - Quantity/SquareMeters
  - Gross weight
  - SSCC
  - Print checkbox
  - Scrap checkbox
  - Hold checkbox
- Action buttons:
  - Generate Rolls
  - Create (produce to SAP)
  - Print Labels
  - Scrap
  - Clear/Reset

**UI-4.1.3: FrmPackPrintMedline / FrmPackPrintRockline (Packing)**
- Unprinted pack labels grid:
  - Bundle SSCC
  - Item code/name
  - IRMS
  - YJN Order
  - Gross weight (editable)
  - Net weight (editable)
  - Roll count
- Number of copies input
- Print button
- Mark as printed checkbox

**UI-4.1.4: FrmBoxScrapMedline / FrmBoxScrapRockline (Box Scrap)**
- Scrap reason dropdown
- Quantity input
- Date/time picker
- Submit button

**UI-4.1.5: Reprint Dialogs**
- Roll selection grid from order
- Select all / deselect all
- Print button

### 4.2 User Experience Requirements

**UI-4.2.1: Usability**
- Touch-screen friendly (large buttons, adequate spacing)
- Minimal keyboard entry required
- Barcode scanner support for data entry
- Clear visual feedback for actions (success/error messages)

**UI-4.2.2: Accessibility**
- High contrast mode support
- Readable font sizes (minimum 10pt)
- Logical tab order for keyboard navigation

**UI-4.2.3: Responsiveness**
- Forms shall load within 2 seconds
- User actions shall provide immediate feedback (progress bars, status messages)
- No UI freezing during SAP operations (background workers if needed)

---

## 5. Business Rules

### 5.1 Production Rules

**BR-5.1.1: Roll Numbering**
- Roll numbers shall be unique and sequential within a production run
- Roll number format is strictly enforced: `{Y}{M}{DD}{Die}{JR}{Shift}{Slit}`
- Jumbo roll counter increments only after successful production

**BR-5.1.2: Material Consumption**
- Materials shall be issued from production line input location
- Batch selection prioritized by quantity (descending) and expiry (ascending)
- Shortages recorded but do not block production (configurable)

**BR-5.1.3: Quality Management**
- Rolls can be marked HOLD or RELEASED during production
- Held rolls go to HOLDAREA location
- Released rolls go to configured output location
- Scrap rolls issued from current location and received into SCRAP location

**BR-5.1.4: Production Limits**
- System shall not allow production beyond target quantity
- Over-production warning if inventory rolls exceed target rolls
- Minimum/maximum roll weight validation (P&G only)

### 5.2 Measurement Standards

**BR-5.2.1: Medline Standards**
- Measurement unit: Square Meters
- Calculation: SquareMeters = Width(MM) × 0.001 × LinearMeters
- Width retrieved from item master data

**BR-5.2.2: Rockline Standards**
- Measurement unit: Quantity (as of Nov 2025)
- Quantities rounded to configured precision
- SSCC included on bundle labels (requirement added Nov 2025)

**BR-5.2.3: Generic/P&G Standards**
- Measurement unit: Kilograms
- Weight validation: MinRollKgs ≤ Weight ≤ MaxRollKgs
- Tare weight + Net weight + Adjustment = Gross weight

### 5.3 Labeling Rules

**BR-5.3.1: SSCC Requirements**
- SSCC format: GS1-128 compliant 18-digit code
- Check digit calculated using GS1 algorithm
- Roll SSCC generated by Produmex system
- Bundle SSCC format: `112300000034{NNNNNN}{C}` where C = check digit

**BR-5.3.2: Label Content**
- Roll labels must include: Item, IRMS, Lot, Roll#, SSCC, Quantity
- Medline labels include PO Number
- Rockline labels include SSCC on bundle labels (new requirement)
- Pack labels include: SSCC, Item, IRMS, Order, Weight, Count

**BR-5.3.3: Print Rules**
- Labels print only for rolls marked "Print = true"
- Default 4 copies for pack labels (configurable)
- Reprint does not affect inventory or production count

### 5.4 Scrap Rules

**BR-5.4.1: Scrap Reasons**
- Scrap reason must be selected from configured list
- Scrap reason recorded for traceability
- Scrap offset code: _SYS00000000604 (configurable)

**BR-5.4.2: Scrap Processing**
- Scrap rolls issued from current storage location
- Scrap rolls received into SCRAP location
- Quality status changed to SCRAP
- Inventory count reduced on production order

---

## 6. Integration Requirements

### 6.1 SAP Business One Integration

**INT-6.1.1: Production Order Integration**
- Read production orders with status 'R'
- Update custom fields (Jumbo roll, batch)
- No modification to standard SAP production order fields

**INT-6.1.2: Inventory Integration**
- Create Inventory General Entry for receipts
- Create Inventory General Exit for issues
- Update Produmex LUID and SSCC in PMX tables
- Real-time inventory updates (no batch processing)

**INT-6.1.3: Item Master Integration**
- Read item master data (code, name, IRMS, UOM)
- Read item group classification
- No modification to item master

### 6.2 BarTender Integration

**INT-6.2.1: Print Job Submission**
- Submit print jobs via command-line interface
- Trigger files in CSV format
- Clean up trigger files after print

**INT-6.2.2: Format Management**
- Label formats stored externally (.btw files)
- Format paths configured in App.config
- Multiple formats supported for different customers/products

### 6.3 Database Integration

**INT-6.3.1: SQL Server Connectivity**
- Direct SQL Server connections (ADO.NET)
- Stored procedure calls for complex operations
- Parameterized queries for security
- Connection pooling enabled

**INT-6.3.2: Produmex Integration**
- Read inventory from PMX_INVENTORY_REPORT_DETAIL
- Create LUID/SSCC records
- Update production tracking tables

---

## 7. Data Requirements

### 7.1 Data Retention

**DR-7.1.1: Production Data**
- Production order data retained as per SAP retention policy
- Roll records retained indefinitely
- Bundle records retained indefinitely
- Scrap records retained for audit purposes

**DR-7.1.2: Log Data**
- Application logs retained with 10 rolling backups (10MB each)
- Total log retention: ~100MB
- Logs archived manually if long-term retention required

### 7.2 Data Backup

**DR-7.2.1: Database Backup**
- SAP database backed up as per SAP HANA/MSSQL backup schedule
- Produmex database backed up with same schedule
- Application responsible only for data creation, not backup

**DR-7.2.2: Configuration Backup**
- App.config backed up with application deployment
- Label format files backed up separately
- Print trigger locations not backed up (temporary files)

### 7.3 Data Migration

**DR-7.3.1: Version Upgrades**
- Configuration settings preserved across upgrades
- Database schema changes via SQL scripts
- Custom SAP fields preserved (no migration required)

---

## 8. Operational Requirements

### 8.1 Deployment Requirements

**OP-8.1.1: Installation**
- Windows 10 or Windows Server 2016+
- .NET Framework 4.8
- SAP Business One DI API 9.0
- BarTender (any recent version with CLI support)
- SQL Server Client Tools
- Network access to SQL Server
- Network access to SAP license server

**OP-8.1.2: Configuration**
- App.config edited during deployment
- Connection strings updated for environment
- Printer names updated for client machines
- Label format paths updated for local installation
- SAP credentials configured per production line

### 8.2 Monitoring Requirements

**OP-8.2.1: Application Monitoring**
- Log file monitoring for errors
- Email alerts for production exceptions
- Manual monitoring of production counts

**OP-8.2.2: Database Monitoring**
- SAP database monitored through SAP tools
- Custom table growth monitoring recommended
- Index maintenance on custom tables

### 8.3 Maintenance Requirements

**OP-8.3.1: Regular Maintenance**
- Log file rotation (automatic via log4net)
- Print trigger file cleanup (automatic)
- Database statistics updates (SQL Server maintenance plans)

**OP-8.3.2: Updates**
- Application updates deployed via installer or XCOPY
- Database updates via SQL scripts
- Label format updates via file replacement

---

## 9. Non-Functional Requirements

### 9.1 Reliability

**NFR-9.1.1: Availability**
- Target availability: 99% during production hours
- Planned downtime for SAP maintenance acceptable
- No single point of failure in application logic

**NFR-9.1.2: Error Recovery**
- Transaction rollback on SAP errors (when enabled)
- Error messages logged and displayed to user
- Email alerts for critical errors
- Graceful degradation (e.g., print failures don't block production)

### 9.2 Maintainability

**NFR-9.2.1: Code Organization**
- Clear separation: UI, Business Logic, Data Access, SAP Integration
- Entity models for all business objects
- Centralized configuration management
- Consistent error handling patterns

**NFR-9.2.2: Documentation**
- XML comments on public methods and classes
- README for deployment instructions
- SQL scripts documented with author and date
- Configuration settings documented in App.config

### 9.3 Portability

**NFR-9.3.1: Platform Support**
- Windows-only (Windows Forms)
- x86 and x64 platform builds
- No Linux/Mac support

**NFR-9.3.2: Database Portability**
- SQL Server specific (T-SQL stored procedures)
- No support for other RDBMS

### 9.4 Compliance

**NFR-9.4.1: Standards Compliance**
- GS1 standards for SSCC generation
- SAP Business One DI API standards
- Windows UI design guidelines

**NFR-9.4.2: Regulatory Compliance**
- Traceability through SSCC/LUID
- Lot tracking for quality management
- Scrap tracking for waste reporting

---

## 10. Constraints

### 10.1 Technical Constraints

**CON-10.1.1: Platform Constraints**
- Must use .NET Framework 4.8 (not .NET Core/.NET 5+)
- Must use Windows Forms (no WPF/web UI)
- Must integrate with SAP Business One DI API 9.0

**CON-10.1.2: Database Constraints**
- Must use SQL Server 2019
- Cannot modify standard SAP tables (only custom fields/tables)
- Must use Produmex tables for LUID/SSCC

**CON-10.1.3: Integration Constraints**
- BarTender integration via file-based triggers only
- SAP integration via COM interop (no web services)
- No real-time synchronization (polling-based)

### 10.2 Business Constraints

**CON-10.2.1: Customer Requirements**
- Must support Medline and Rockline specific workflows
- Cannot change roll numbering format (downstream dependencies)
- Must maintain SSCC uniqueness across all customers

**CON-10.2.2: Operational Constraints**
- Production line-specific user accounts in SAP
- Label formats controlled by Quality department
- Printer locations fixed per production line

---

## 11. Assumptions

### 11.1 System Assumptions

**ASM-11.1.1: Infrastructure**
- SQL Server is available and accessible
- SAP Business One is operational
- Network connectivity is reliable
- BarTender is installed and licensed on client machines

**ASM-11.1.2: Data**
- Production orders created in SAP before using application
- Item master data maintained in SAP
- Production line master data maintained in custom tables

### 11.2 User Assumptions

**ASM-11.2.1: User Capabilities**
- Operators trained on application usage
- Operators understand production processes
- Operators can troubleshoot basic printer issues

**ASM-11.2.2: User Behavior**
- Operators will select correct production orders
- Operators will enter accurate production parameters
- Operators will verify label content before printing

---

## 12. Dependencies

### 12.1 External Dependencies

**DEP-12.1.1: SAP Business One**
- Version: 9.0+
- DI API: 9.0
- Database: YanJanUS_PROD
- License server: sql:30000

**DEP-12.1.2: Produmex MES**
- Database: PMX_YanJanUS_PROD
- Tables: PMX_INVENTORY_REPORT_DETAIL
- LUID/SSCC generation mechanism

**DEP-12.1.3: BarTender**
- Version: Any recent version with CLI
- Executable: Bartend.exe
- Format files: .btw templates

### 12.2 Internal Dependencies

**DEP-12.2.1: Database Objects**
- Custom SAP fields (U_SII_*)
- Custom SAP tables ([@SII_*])
- Stored procedures (_sii_rpr_*)
- Check digit function (fnGetSSCCCheckDigit)

**DEP-12.2.2: Configuration**
- App.config with all required settings
- Label format files accessible at configured paths
- Printer drivers installed for configured printers

---

## 13. Risks and Mitigations

### 13.1 Technical Risks

**RISK-13.1.1: SAP COM Interop Stability**
- **Risk:** COM objects can cause memory leaks or crashes
- **Mitigation:** Proper disposal of SAP objects, using statements, transaction rollback

**RISK-13.1.2: BarTender Print Failures**
- **Risk:** Print jobs may fail silently or with unclear errors
- **Mitigation:** File-based trigger logging, retry mechanism, manual reprint capability

**RISK-13.1.3: Database Connection Failures**
- **Risk:** Network interruptions can cause data loss
- **Mitigation:** Transaction support, error logging, retry logic

### 13.2 Operational Risks

**RISK-13.2.1: Incorrect Production Data Entry**
- **Risk:** Operators may enter wrong parameters (LM, weight, slit count)
- **Mitigation:** Validation rules, confirmation dialogs, preview before create

**RISK-13.2.2: SSCC Duplication**
- **Risk:** SSCC generation failure could cause duplicates
- **Mitigation:** Database unique constraints, Produmex SSCC generation, GS1 check digit validation

**RISK-13.2.3: Label Format Changes**
- **Risk:** Customer may change label requirements without notice
- **Mitigation:** Externalized label formats (.btw files), version control on formats

### 13.3 Security Risks

**RISK-13.3.1: Plaintext Passwords in Config**
- **Risk:** App.config contains plaintext SAP and SQL passwords
- **Mitigation:** File system permissions, consider encryption for future versions

**RISK-13.3.2: SQL Injection**
- **Risk:** Dynamic SQL could allow injection attacks
- **Mitigation:** Use parameterized queries and stored procedures exclusively

**RISK-13.3.3: Unauthorized Access**
- **Risk:** No application-level authentication
- **Mitigation:** Rely on Windows authentication and SAP authorization, deploy on secured network

---

## 14. Future Enhancements

### 14.1 Potential Improvements

**ENH-14.1.1: Web-Based Interface**
- Migrate to ASP.NET Core web application for remote access
- Responsive design for tablet/mobile devices
- Centralized deployment model

**ENH-14.1.2: Real-Time Dashboards**
- Production monitoring dashboard
- Real-time inventory levels
- Scrap rate analytics
- Production efficiency metrics

**ENH-14.1.3: Barcode Scanning**
- Integrated barcode scanner support
- Scan-to-select production orders
- Scan-to-verify material issues
- Scan-to-ship functionality

**ENH-14.1.4: Advanced Reporting**
- Crystal Reports integration
- Production summary reports
- Scrap analysis reports
- Traceability reports

**ENH-14.1.5: Enhanced Security**
- Encrypted configuration values
- Windows integrated authentication
- Role-based access control
- Audit trail for all operations

---

## 15. Glossary

| Term | Definition |
|------|------------|
| **SSCC** | Serial Shipping Container Code - GS1 standard 18-digit identifier for logistics units |
| **LUID** | Logistic Unit ID - Internal identifier for inventory tracking in Produmex system |
| **IRMS** | Item Reference Management System - Customer-specific item identifier |
| **YJN Order** | YanJan Order - Internal order identifier separate from SAP order number |
| **Jumbo Roll** | Parent roll from which multiple smaller rolls are slit |
| **Slit Position** | Position of the cutting blade that creates a roll from a jumbo roll |
| **Die** | Cutting tool/pattern identifier |
| **Resmix** | Reground/recycled material mixed back into production |
| **BarTender** | Label design and printing software by Seagull Scientific |
| **Produmex** | Manufacturing Execution System (MES) add-on for SAP Business One |
| **DI API** | Data Interface Application Programming Interface - SAP's COM-based integration API |
| **UDF** | User Defined Field - Custom field added to SAP table |
| **UDT** | User Defined Table - Custom table added to SAP database |
| **OWOR** | SAP table: Production Orders |
| **WOR1** | SAP table: Production Order Lines |
| **OITM** | SAP table: Items |
| **OIGN** | SAP table: Inventory General Entry (Receipts) |
| **OIGE** | SAP table: Inventory General Exit (Issues) |

---

## 16. Revision History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-12-09 | Reverse Engineered | Initial requirements document based on code analysis |

---

## 17. Appendices

### Appendix A: Configuration Reference

See App.config in source code for complete configuration reference.

Key configuration sections:
- Connection strings
- SAP connection parameters
- Production line credentials
- Label format paths
- Printer configurations
- Email settings
- Inventory locations
- Default values

### Appendix B: Database Schema

Key custom fields and tables documented in section 3.2.

### Appendix C: Stored Procedure Reference

Complete list of stored procedures in section 3.2.4.

### Appendix D: Form Reference

Complete list of forms and their purposes in section 2.1 and 4.1.

### Appendix E: Label Format Specifications

Label formats stored as .btw files in configured directory.
Maintained separately by Quality department.

---

**END OF REQUIREMENTS DOCUMENT**
