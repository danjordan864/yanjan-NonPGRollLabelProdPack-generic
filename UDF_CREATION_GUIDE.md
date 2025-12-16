# UDF Creation Guide - SAP DI API Method

## ⚠️ IMPORTANT
SAP Business One **does NOT support** creating UDFs via direct SQL manipulation of the CUFD table. The **ONLY supported method** is using the **DI API (Data Interface API)**.

## Overview
This guide explains how to create the 6 required UDFs for generic roll label production using the DI API.

## UDFs to Create

| UDF Name | SAP Field Name | Type | Size | Mandatory | Default | Description |
|----------|----------------|------|------|-----------|---------|-------------|
| SII_RollLabel | U_SII_RollLabel | Alphanumeric | 50 | No | NULL | Roll label .btw file path |
| SII_BunLabel | U_SII_BunLabel | Alphanumeric | 50 | No | NULL | Bundle label .btw file path |
| SII_CoreLabel | U_SII_CoreLabel | Alphanumeric | 50 | No | NULL | Core label .btw file path |
| SII_RLabelQty | U_SII_RLabelQty | Numeric | 10 | Yes | 1 | Roll label print quantity |
| SII_BLabel | U_SII_BLabel | Numeric | 10 | Yes | 1 | Bundle label print quantity |
| SII_CLabel | U_SII_CLabel | Numeric | 10 | Yes | 1 | Core label print quantity |

All UDFs are created on the **OWOR** (Production Orders) table.

## Method 1: Using the Windows Form Utility (RECOMMENDED)

### Step 1: Add FrmCreateUDFs to your project
1. The file `FrmCreateUDFs.cs` has been created in the `RollLabelProdPack` project
2. Build the project to compile the form

### Step 2: Add menu item to launch the form
In your main form (e.g., `FrmMainMedline` or a startup form), add a menu item:

```csharp
// In your form's menu or button click event:
private void createUDFsToolStripMenuItem_Click(object sender, EventArgs e)
{
    var frmCreateUDFs = new FrmCreateUDFs();
    frmCreateUDFs.ShowDialog();
}
```

### Step 3: Run the utility
1. Launch your application
2. Click the menu item to open "Create UDFs" form
3. Click "Create UDFs Now" button
4. The form will:
   - Connect to SAP Business One
   - Create all 6 UDFs using DI API
   - Display progress and results
   - Verify UDFs were created

## Method 2: Using the Console Application

### Step 1: Add CreateUDFs_Program.cs to your project
The file has been created at the root of your solution.

### Step 2A: Run as standalone (option 1)
Change the startup object temporarily:

```xml
<!-- In RollLabelProdPack.csproj -->
<PropertyGroup>
  <OutputType>Exe</OutputType>
  <StartupObject>RollLabelProdPack.Utilities.CreateUDFs_Program</StartupObject>
</PropertyGroup>
```

Build and run. The console application will create UDFs and display results.

### Step 2B: Call from existing code (option 2)
```csharp
// From anywhere in your application:
RollLabelProdPack.Utilities.CreateUDFs_Program.CreateUDFs();
```

## Method 3: Using the UDFCreator class directly (ADVANCED)

For integration into existing code:

```csharp
using RollLabelProdPack.Library.Utility;
using RollLabelProdPack.SAP.B1;

// In your method:
try
{
    // Connect to SAP
    using (SAPB1 sapB1 = new SAPB1("username", "password"))
    {
        // Create UDF creator
        var udfCreator = new UDFCreator(sapB1.SapCompany);

        // Create all UDFs
        var result = udfCreator.CreateAllLabelUDFs();

        if (result.Success)
        {
            Console.WriteLine("SUCCESS!");
            Console.WriteLine(result.Message);
        }
        else
        {
            Console.WriteLine("ERROR!");
            Console.WriteLine(result.Message);
        }

        // Optionally, verify UDFs created
        var existingUDFs = udfCreator.GetExistingUDFs("OWOR");
        foreach (var udf in existingUDFs)
        {
            Console.WriteLine(udf);
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
```

## Method 4: Create Individual UDFs (ADVANCED)

If you need to create UDFs one at a time:

```csharp
using (SAPB1 sapB1 = new SAPB1("username", "password"))
{
    var udfCreator = new UDFCreator(sapB1.SapCompany);

    // Create a single UDF
    var result = udfCreator.CreateUDF(
        "OWOR",                              // Table name
        "SII_RollLabel",                     // Field name (without U_ prefix)
        "Roll Label Format Path",            // Description
        SAPbobsCOM.BoFieldTypes.db_Alpha,    // Type: Alphanumeric
        SAPbobsCOM.BoFldSubTypes.st_None,    // Subtype
        50,                                  // Size
        null,                                // Default value (null = no default)
        false                                // Mandatory (false = not required)
    );

    if (result.Success)
    {
        Console.WriteLine(result.Message);
    }
}
```

## Verification

After creating UDFs, verify they exist:

### Option 1: In SAP Business One
1. Go to Tools → Customization Tools → User-Defined Fields - Management
2. Select Table: Production Orders (OWOR)
3. Look for fields starting with "SII_"

### Option 2: Using SQL Query (READ-ONLY)
```sql
SELECT
    TableID,
    FieldID,
    AliasID,
    Descr,
    TypeID,
    SizeID,
    Dflt,
    NotNull
FROM CUFD
WHERE TableID = 'OWOR'
  AND AliasID LIKE 'SII_%'
ORDER BY FieldID
```

Expected results:
```
TableID | FieldID | AliasID        | Descr                      | TypeID | SizeID | Dflt | NotNull
--------|---------|----------------|----------------------------|--------|--------|------|--------
OWOR    | (auto)  | SII_RollLabel  | Roll Label Format Path     | A      | 50     | NULL | N
OWOR    | (auto)  | SII_BunLabel   | Bundle Label Format Path   | A      | 50     | NULL | N
OWOR    | (auto)  | SII_CoreLabel  | Core Label Format Path     | A      | 50     | NULL | N
OWOR    | (auto)  | SII_RLabelQty  | Roll Label Print Quantity  | N      | 10     | 1    | Y
OWOR    | (auto)  | SII_BLabel     | Bundle Label Print Quantity| N      | 10     | 1    | Y
OWOR    | (auto)  | SII_CLabel     | Core Label Print Quantity  | N      | 10     | 1    | Y
```

### Option 3: Using UDFCreator class
```csharp
var udfCreator = new UDFCreator(sapB1.SapCompany);
var existingUDFs = udfCreator.GetExistingUDFs("OWOR");

foreach (var udf in existingUDFs)
{
    Console.WriteLine(udf);
}
```

## Troubleshooting

### Error: "User field already exists"
This is normal if UDFs were already created. The UDFCreator class checks for existing UDFs and skips them.

### Error: "No authorization"
Ensure the SAP user has permission to create UDFs. Typically requires administrator or customization authorization.

### Error: "Connection failed"
Verify:
- SAP license is active
- Database connection is working
- Credentials are correct
- SAP company is accessible

### Error: "Invalid field size"
Alphanumeric fields in SAP have maximum sizes. The script uses:
- 50 characters for label paths (should be sufficient for file paths)
- 10 digits for numeric quantities

If you need larger sizes, modify the `CreateAllLabelUDFs()` method in `UDFCreator.cs`.

## Files Created

| File | Location | Purpose |
|------|----------|---------|
| UDFCreator.cs | RollLabelProdPack.Library\Utility\ | Core DI API class for UDF creation |
| CreateUDFs_Program.cs | Root directory | Console application |
| FrmCreateUDFs.cs | RollLabelProdPack\ | Windows Form utility |
| sql\CreateOWORUDFs.sql | sql\ | Reference only (shows UDF structure) |

## Next Steps

After creating UDFs:

1. **Test UDF access**: Open a production order in SAP and verify the new fields appear
2. **Set default values**: In App.config, configure fallback label formats
3. **Populate UDFs**: For testing, manually populate UDFs on a test production order
4. **Test label printing**: Run the generic form and verify UDF-driven label selection works

## Security Considerations

- UDF creation requires SAP administrator or customization permissions
- Store SAP credentials securely (use App.config encryption if needed)
- Test in development/sandbox environment first
- Create database backup before making schema changes

## Support

If you encounter issues:
1. Check SAP DI API documentation
2. Verify SAP license and permissions
3. Review error messages in the output
4. Test with a simple UDF first before creating all 6

## Summary

The recommended approach is:
1. Use **Method 1** (FrmCreateUDFs form) for ease of use
2. Run once in each environment (dev, test, production)
3. Verify UDFs were created successfully
4. UDFs will persist in the database and do not need to be recreated

All methods use the same underlying `UDFCreator` class, which properly uses SAP DI API's `UserFieldsMD` object.
