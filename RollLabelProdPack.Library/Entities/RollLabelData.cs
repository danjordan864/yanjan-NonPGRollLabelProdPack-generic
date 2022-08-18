using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollLabelProdPack.Library.Entities
{
    public class RollLabelData
    {
        public string ProductionYear { get; set; }
        public string ProductionMonth { get; set; }
        public string ProductionDate { get; set; }
        public string AperatureDieNo { get; set; }
        public string ProductionShift { get; set; }
        public int JumboRollNo { get; set; }
        public int SlitPosition { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string IRMS { get; set; }
        public string FactoryCode { get; set; }
        public string ProductionMachineNo { get; set; }
        public string ProductionLine { get; set; }
        public string MaterialCode { get; set; }
        public string ProductName { get; set; }
        public string BatchNo { get; set; }
        public int SAPOrderNo { get; set; }
        public int SAPDocEntry { get; set; }
        public string YJNOrder { get; set; }
        public string OrderDisplay { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public int NoOfSlits { get; set; }
        public string Employee { get; set; }
        public string Shift { get; set; }
        public string RollNo { get; set; }
        public int LUID { get; set; }
        public string SSCC { get; set; }
        public string InputLoc { get; set; }
        public string OutputLoc { get; set; }
        public string Printer { get; set; }
        public string DefaultQualityStatus { get; set; }
        public string ScrapItem { get; set; }
        public int ScrapLine { get; set; }
        public int TargetRolls { get; set; }
        public int InvRolls { get; set; }
        public decimal MinRollKgs { get; set; }
        public decimal MaxRollKgs { get; set; }

        public string ScrapItemName { get; set; }
        public int LeftToProduce
        {
            get { return TargetRolls - InvRolls; }
        }

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
