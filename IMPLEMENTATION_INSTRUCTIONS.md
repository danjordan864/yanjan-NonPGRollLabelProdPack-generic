# FrmMainGeneric Creation Instructions

## Overview
This document provides step-by-step instructions to create FrmMainGeneric from FrmMainMedline.

## Step 1: Copy Files

### Copy the code file:
```bash
cp RollLabelProdPack/FrmMainMedline.cs RollLabelProdPack/FrmMainGeneric.cs
```

### Copy the designer file:
```bash
cp RollLabelProdPack/FrmMainMedline.Designer.cs RollLabelProdPack/FrmMainGeneric.Designer.cs
```

### Copy the resx file (if exists):
```bash
cp RollLabelProdPack/FrmMainMedline.resx RollLabelProdPack/FrmMainGeneric.resx
```

## Step 2: Modify FrmMainGeneric.cs

### Change 1: Update using statements (Lines 1-19)
Add these new using statements after the existing ones:
```csharp
using RollLabelProdPack.Library.Services;
using RollLabelProdPack.Library.Strategies;
```

### Change 2: Update class name and add customer field (Lines 26-34)
```csharp
// FIND:
public partial class FrmMainMedline : Form
{
    private static ILog log = LogManager.GetLogger(typeof(FrmMainMedline));

    FloatingHTML m_htmlToast = new FloatingHTML();
    private RollLabelData _selectOrder = new RollLabelData();
    private List<Roll> _rolls = null;
    private BindingSource bindingSource1;
    private List<InventoryIssueDetail> _plannedIssue;

// REPLACE WITH:
public partial class FrmMainGeneric : Form
{
    private static ILog log = LogManager.GetLogger(typeof(FrmMainGeneric));

    FloatingHTML m_htmlToast = new FloatingHTML();
    private RollLabelData _selectOrder = new RollLabelData();
    private List<Roll> _rolls = null;
    private BindingSource bindingSource1;
    private List<InventoryIssueDetail> _plannedIssue;

    // NEW: Customer ID for fallback config lookups
    protected string _customerID;
```

### Change 3: Update constructor (Lines 36-42)
```csharp
// FIND:
/// <summary>
/// Initializes a new instance of the FrmMain class.
/// </summary>
public FrmMainMedline()
{
    InitializeComponent();
}

// REPLACE WITH:
/// <summary>
/// Initializes a new instance of the FrmMainGeneric class.
/// </summary>
/// <param name="customerID">Optional customer ID for configuration fallback (e.g., "C1007", "C1020")</param>
public FrmMainGeneric(string customerID = null)
{
    _customerID = customerID;
    InitializeComponent();
}
```

### Change 4: Make ChangeOrder virtual (Lines 282-311)
```csharp
// FIND:
/// <summary>
/// Changes the selected order by displaying a dialog to select a new order.
/// </summary>
private void ChangeOrder()
{
    using (MedlineSelectOrderDialog frmSignInDialog = new MedlineSelectOrderDialog())
    {
        frmSignInDialog.SetDataSource("FF");
        DialogResult dr = frmSignInDialog.ShowDialog();

        if (dr == DialogResult.OK)
        {
            _selectOrder = frmSignInDialog.SelectOrder;
            txtLengthLM.Text = "0";
            txtNoOfSlits.Enabled = true;
            txtProductionDateFull.Text = DateTime.Now.ToShortDateString();

            if (_selectOrder.NoOfSlits > 0)
                ShowSlitPos(_selectOrder.NoOfSlits);

            bindingSource1.DataSource = _selectOrder;

            if (_selectOrder.ScrapItem == null)
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Warning, "Scrap setup issue", "A scrap item needs to be set on the order to record scrap.");

            reprintToolStripMenuItem.Enabled = true;
            scrapRollsToolStripMenuItem.Enabled = true;
        }
    }
}

// REPLACE WITH:
/// <summary>
/// Changes the selected order by displaying a dialog to select a new order.
/// Virtual method to allow customer-specific dialog overrides.
/// </summary>
protected virtual void ChangeOrder()
{
    // NOTE: In derived customer-specific forms, override this method
    // to use customer-specific dialogs (e.g., MedlineSelectOrderDialog, RocklineSelectOrderDialog)

    // Generic implementation - you may need to implement a generic dialog
    // or keep this as-is and override in Medline/Rockline forms
    throw new NotImplementedException("ChangeOrder must be overridden in customer-specific forms or implement generic dialog here.");
}
```

### Change 5: Update GenerateRolls to use UnitConversionService (Lines 223-278)
```csharp
// FIND (around line 256-258):
SquareMeters = _selectOrder.WidthInMM * 0.001m * Convert.ToDecimal(txtLengthLM.Text),
PONumber = _selectOrder.PONumber

// REPLACE WITH:
PONumber = _selectOrder.PONumber
};

// NEW: Calculate quantity using UnitConversionService
decimal linearMeters = Convert.ToDecimal(txtLengthLM.Text);
UnitConversionService.CalculateRollQuantity(
    roll,
    _selectOrder.UOM,
    linearMeters,
    _selectOrder.WidthInMM
);

// Continue with existing code - change the closing brace position
// REMOVE the old closing brace after PONumber and place it after CalculateRollQuantity

// The roll object is now complete with proper SquareMeters, Quantity, and UOM set
```

The complete GenerateRolls section should look like:
```csharp
var roll = new Roll
{
    RollNo = $"{_selectOrder.ProductionYear}{_selectOrder.ProductionMonth}{_selectOrder.ProductionDate}{_selectOrder.AperatureDieNo}".ToUpper() +
    $"{_selectOrder.Shift}{_selectOrder.JumboRollNo.ToString("00")}{(i).ToString("00")}".ToUpper(),
    YJNOrder = _selectOrder.YJNOrder,
    ItemCode = _selectOrder.ItemCode,
    ItemName = _selectOrder.ItemDescription,
    IRMS = _selectOrder.IRMS,
    PONumber = _selectOrder.PONumber
};

// Calculate quantity using UnitConversionService
decimal linearMeters = Convert.ToDecimal(txtLengthLM.Text);
UnitConversionService.CalculateRollQuantity(
    roll,
    _selectOrder.UOM,
    linearMeters,
    _selectOrder.WidthInMM
);
```

### Change 6: REPLACE PrintRollLabels Method (Lines 691-737)
```csharp
// FIND ENTIRE METHOD and REPLACE WITH:

/// <summary>
/// Generate and print roll labels using UDF-driven format selection and Strategy pattern.
/// </summary>
private void PrintRollLabels()
{
    try
    {
        // Get label format from UDF with fallback to config
        var labelFormat = UDFHelper.GetLabelFormatPath(
            _selectOrder.SAPOrderNo,
            LabelType.Roll,
            _customerID
        );

        if (string.IsNullOrEmpty(labelFormat))
        {
            throw new ApplicationException("No label format configured. Please set roll label format in production order UDF or config.");
        }

        // Get number of copies per roll from UDF (default: 1)
        var copiesPerRoll = UDFHelper.GetLabelCopies(
            _selectOrder.SAPOrderNo,
            LabelType.Roll
        );

        // Get appropriate CSV format strategy based on customer
        var strategy = LabelFormatStrategyFactory.GetStrategy(_selectOrder.CardCode);
        var csvContent = strategy.GenerateCSV(_rolls, _selectOrder);

        // Build trigger file
        var labelPrintLoc = AppUtility.GetBTTriggerLoc();
        var labelPrintExtension = AppUtility.GetLabelPrintExtension();
        var fileNameRollLabels = Path.Combine(labelPrintLoc, "RollLabels" + labelPrintExtension);

        var triggerContent = new StringBuilder();
        triggerContent.AppendFormat(
            @"%BTW% /AF=""{0}"" /D=""%Trigger File Name%"" /PRN=""{1}"" /R={2} /P /DD",
            labelFormat,
            _selectOrder.Printer,
            copiesPerRoll
        );
        triggerContent.AppendLine();
        triggerContent.Append(@"%END%");
        triggerContent.AppendLine();
        triggerContent.Append(csvContent);

        // Write trigger file
        using (StreamWriter sw = File.CreateText(fileNameRollLabels))
        {
            sw.Write(triggerContent.ToString());
        }

        // Display success notification
        DisplayToastNotification(WinFormUtils.ToastNotificationType.Success,
            "Success", "Roll labels printed. Please check printer.");
    }
    catch (Exception ex)
    {
        // Display error notification and log
        DisplayToastNotification(WinFormUtils.ToastNotificationType.Error,
            "SAP B1 Connection",
            $"Exception has occurred in {AppUtility.GetLoggingText()} PrintRollLabels.\n\n{ex.Message}");
        AppUtility.WriteToEventLog(
            $"Exception has occurred in {AppUtility.GetLoggingText()} PrintRollLabels.\n\n{ex.Message}",
            EventLogEntryType.Error, true);
    }
}
```

### Change 7: Make reprintToolStripMenuItem_Click virtual (Lines 746-782)
Make the method virtual so customer-specific forms can override:

```csharp
// FIND:
private void reprintToolStripMenuItem_Click(object sender, EventArgs e)

// REPLACE WITH:
protected virtual void reprintToolStripMenuItem_Click(object sender, EventArgs e)
```

### Change 8: Make boxScrapToolStripMenuItem_Click virtual (Lines 844-849)
```csharp
// FIND:
private void boxScrapToolStripMenuItem_Click(object sender, EventArgs e)
{
    FrmBoxScrapMedline frmBoxScrap = new FrmBoxScrapMedline();
    frmBoxScrap.Show();
}

// REPLACE WITH:
protected virtual void boxScrapToolStripMenuItem_Click(object sender, EventArgs e)
{
    // Override in customer-specific forms to show customer-specific box scrap form
    DisplayToastNotification(WinFormUtils.ToastNotificationType.Warning,
        "Not Implemented", "Box scrap functionality should be overridden in customer-specific form.");
}
```

## Step 3: Modify FrmMainGeneric.Designer.cs

### Change 1: Update class name
```csharp
// FIND:
partial class FrmMainMedline

// REPLACE WITH:
partial class FrmMainGeneric
```

### Change 2: Update form name in InitializeComponent (if present)
```csharp
// FIND:
this.Name = "FrmMainMedline";

// REPLACE WITH:
this.Name = "FrmMainGeneric";
```

## Step 4: Test Build

After making changes, build the project:
```bash
dotnet build
```

Or in Visual Studio: Build → Build Solution

## Step 5: Create Simple Test Form

Create a test to verify the generic form works:

```csharp
// In Program.cs or a test method:
var genericForm = new FrmMainGeneric("C1007"); // Medline
// or
var genericForm = new FrmMainGeneric("C1020"); // Rockline
// or
var genericForm = new FrmMainGeneric(); // Generic/Unknown customer

genericForm.ShowDialog();
```

## Summary of Changes

| Method/Property | Change Type | Description |
|----------------|-------------|-------------|
| Class name | Rename | FrmMainMedline → FrmMainGeneric |
| _customerID field | Add | Store customer ID for config fallback |
| Constructor | Modify | Accept optional customerID parameter |
| ChangeOrder() | Make virtual | Allow customer-specific dialog overrides |
| GenerateRolls() | Modify | Use UnitConversionService.CalculateRollQuantity() |
| PrintRollLabels() | Replace | Use UDFHelper + LabelFormatStrategyFactory |
| reprintToolStripMenuItem_Click() | Make virtual | Allow customer overrides |
| boxScrapToolStripMenuItem_Click() | Make virtual | Allow customer overrides |

## Notes

- The ChangeOrder() method throws NotImplementedException by default - customer-specific forms MUST override it
- All other virtual methods have default implementations that work generically
- The form will work for any customer once UDFs are populated on production orders
- Fallback to config still works if UDFs are empty

## Next Steps

After creating FrmMainGeneric:
1. Create FrmPackPrintGeneric using the same pattern
2. Refactor FrmMainMedline to inherit from FrmMainGeneric
3. Refactor FrmMainRockline to inherit from FrmMainGeneric
4. Test thoroughly in development environment
