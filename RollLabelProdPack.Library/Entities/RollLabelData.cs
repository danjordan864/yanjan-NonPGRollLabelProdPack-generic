using System;
using System.Text;

namespace RollLabelProdPack.Library.Entities
{
    /// <summary>
    /// Represents roll label data.
    /// </summary>
    public class RollLabelData
    {
        /// <summary>
        /// Gets or sets the production year.
        /// </summary>
        public string ProductionYear { get; set; }

        /// <summary>
        /// Gets or sets the production month.
        /// </summary>
        public string ProductionMonth { get; set; }

        /// <summary>
        /// Gets or sets the production date.
        /// </summary>
        public string ProductionDate { get; set; }

        /// <summary>
        /// Gets or sets the aperature die number.
        /// </summary>
        public string AperatureDieNo { get; set; }

        /// <summary>
        /// Gets or sets the production shift.
        /// </summary>
        public string ProductionShift { get; set; }

        /// <summary>
        /// Gets or sets the jumbo roll number.
        /// </summary>
        public int JumboRollNo { get; set; }

        /// <summary>
        /// Gets or sets the slit position.
        /// </summary>
        public int SlitPosition { get; set; }

        /// <summary>
        /// Gets or sets the item code.
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// Gets or sets the item description.
        /// </summary>
        public string ItemDescription { get; set; }

        /// <summary>
        /// Gets or sets the IRMS.
        /// </summary>
        public string IRMS { get; set; }

        /// <summary>
        /// Gets or sets the factory code.
        /// </summary>
        public string FactoryCode { get; set; }

        /// <summary>
        /// Gets or sets the production machine number.
        /// </summary>
        public string ProductionMachineNo { get; set; }

        /// <summary>
        /// Gets or sets the production line.
        /// </summary>
        public string ProductionLine { get; set; }

        /// <summary>
        /// Gets or sets the material code.
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the batch number.
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// Gets or sets the SAP order number.
        /// </summary>
        public int SAPOrderNo { get; set; }

        /// <summary>
        /// Gets or sets the SAP document entry.
        /// </summary>
        public int SAPDocEntry { get; set; }

        /// <summary>
        /// Gets or sets the YJN order.
        /// </summary>
        public string YJNOrder { get; set; }

        /// <summary>
        /// Gets or sets the order display.
        /// </summary>
        public string OrderDisplay { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the due date.
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Gets or sets the number of slits.
        /// </summary>
        public int NoOfSlits { get; set; }

        /// <summary>
        /// Gets or sets the employee.
        /// </summary>
        public string Employee { get; set; }

        /// <summary>
        /// Gets or sets the shift.
        /// </summary>
        public string Shift { get; set; }

        /// <summary>
        /// Gets or sets the roll number.
        /// </summary>
        public string RollNo { get; set; }

        /// <summary>
        /// Gets or sets the LUID.
        /// </summary>
        public int LUID { get; set; }

        /// <summary>
        /// Gets or sets the SSCC.
        /// </summary>
        public string SSCC { get; set; }

        /// <summary>
        /// Gets or sets the input location.
        /// </summary>
        public string InputLoc { get; set; }

        /// <summary>
        /// Gets or sets the output location.
        /// </summary>
        public string OutputLoc { get; set; }

        /// <summary>
        /// Gets or sets the printer.
        /// </summary>
        public string Printer { get; set; }

        /// <summary>
        /// Gets or sets the default quality status.
        /// </summary>
        public string DefaultQualityStatus { get; set; }

        /// <summary>
        /// Gets or sets the scrap item.
        /// </summary>
        public string ScrapItem { get; set; }

        /// <summary>
        /// Gets or sets the scrap line.
        /// </summary>
        public int ScrapLine { get; set; }

        /// <summary>
        /// Gets or sets the target rolls.
        /// </summary>
        public int TargetRolls { get; set; }

        /// <summary>
        /// Gets or sets the inventory rolls.
        /// </summary>
        public int InvRolls { get; set; }

        /// <summary>
        /// Gets or sets the minimum roll kilograms.
        /// </summary>
        public decimal MinRollKgs { get; set; }

        /// <summary>
        /// Gets or sets the maximum roll kilograms.
        /// </summary>
        public decimal MaxRollKgs { get; set; }

        /// <summary>
        /// Gets or sets the scrap item name.
        /// </summary>
        public string ScrapItemName { get; set; }

        /// <summary>
        /// Gets the number of rolls left to produce.
        /// </summary>
        public int LeftToProduce
        {
            get { return TargetRolls - InvRolls; }
        }

        /// <summary>
        /// Returns a string representation of the <see cref="RollLabelData"/> object.
        /// </summary>
        /// <returns>A string representation of the object.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("RollLabelData:");
            sb.AppendLine("--------------");
            sb.AppendLine($"\tProductionYear: {ProductionYear}");
            sb.AppendLine($"\tProductionMonth: {ProductionMonth}");
            sb.AppendLine($"\tProductionDate: {ProductionDate}");
            sb.AppendLine($"\tAperatureDieNo: {AperatureDieNo}");
            sb.AppendLine($"\tProductionShift: {ProductionShift}");
            sb.AppendLine($"\tJumboRollNo: {JumboRollNo}");
            sb.AppendLine($"\tSlitPosition: {SlitPosition}");
            sb.AppendLine($"\tItemCode: {ItemCode}");
            sb.AppendLine($"\tItemDescription: {ItemDescription}");
            sb.AppendLine($"\tIRMS: {IRMS}");
            sb.AppendLine($"\tFactoryCode: {FactoryCode}");
            sb.AppendLine($"\tProductionMachineNo: {ProductionMachineNo}");
            sb.AppendLine($"\tProductionLine: {ProductionLine}");
            sb.AppendLine($"\tMaterialCode: {MaterialCode}");
            sb.AppendLine($"\tProductName: {ProductName}");
            sb.AppendLine($"\tBatchNo: {BatchNo}");
            sb.AppendLine($"\tSAPOrderNo: {SAPOrderNo}");
            sb.AppendLine($"\tSAPDocEntry: {SAPDocEntry}");
            sb.AppendLine($"\tYJNOrder: {YJNOrder}");
            sb.AppendLine($"\tOrderDisplay: {OrderDisplay}");
            sb.AppendLine($"\tStartDate: {StartDate}");
            sb.AppendLine($"\tDueDate: {DueDate}");
            sb.AppendLine($"\tNoOfSlits: {NoOfSlits}");
            sb.AppendLine($"\tEmployee: {Employee}");
            sb.AppendLine($"\tShift: {Shift}");
            sb.AppendLine($"\tRollNo: {RollNo}");
            sb.AppendLine($"\tLUID: {LUID}");
            sb.AppendLine($"\tSSCC: {SSCC}");
            sb.AppendLine($"\tInputLoc: {InputLoc}");
            sb.AppendLine($"\tOutputLoc: {OutputLoc}");
            sb.AppendLine($"\tPrinter: {Printer}");
            sb.AppendLine($"\tDefaultQualityStatus: {DefaultQualityStatus}");
            sb.AppendLine($"\tScrapItem: {ScrapItem}");
            sb.AppendLine($"\tTargetRolls: {TargetRolls}");
            sb.AppendLine($"\tInvRolls: {InvRolls}");
            sb.AppendLine($"\tMinRollKgs: {MinRollKgs}");
            sb.AppendLine($"\tMaxRollKgs: {MaxRollKgs}");
            sb.AppendLine($"\tScrapItemName: {ScrapItemName}");
            return sb.ToString();
        }
    }
}
