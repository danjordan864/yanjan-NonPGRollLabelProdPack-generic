using System;
using System.Text;

namespace RollLabelProdPack.Library.Data
{
    /// <summary>
    /// Represents a production line and machine number combination.
    /// </summary>
    public class ProductionLineMachineNo : IEquatable<ProductionLineMachineNo>
    {
        /// <summary>
        /// Gets or sets the production line.
        /// </summary>
        public string ProductionLine { get; set; }

        /// <summary>
        /// Gets or sets the production machine number.
        /// </summary>
        public string ProductionMachineNo { get; set; }

        /// <summary>
        /// Gets or sets the input location code.
        /// </summary>
        public string InputLocationCode { get; set; }

        /// <summary>
        /// Gets or sets the output location code.
        /// </summary>
        public string OutputLocationCode { get; set; }

        /// <summary>
        /// Gets or sets the printer.
        /// </summary>
        public string Printer { get; set; }

        /// <summary>
        /// Determines whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the current object is equal to the other object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is ProductionLineMachineNo)
            {
                var x = obj as ProductionLineMachineNo;
                return ProductionLine == x.ProductionLine &&
                    ProductionMachineNo == x.ProductionMachineNo;
            }
            else
            {
                return base.Equals(obj);
            }
        }

        /// <summary>
        /// Returns a hash code for the current object.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return (ProductionLine + ProductionMachineNo).GetHashCode();
        }

        /// <summary>
        /// Determines whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns>True if the current object is equal to the other object; otherwise, false.</returns>
        public bool Equals(ProductionLineMachineNo other)
        {
            return ProductionLine == other.ProductionLine &&
                ProductionMachineNo == other.ProductionMachineNo;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("ProductionLineMachineNo");
            sb.AppendLine("-----------------------");
            sb.AppendLine(String.Format("ProductionLine: {0}", ProductionLine));
            sb.AppendLine(String.Format("ProductionMachineNo: {0}", ProductionMachineNo));
            sb.AppendLine(String.Format("InputLocationCode: {0}", InputLocationCode));
            sb.AppendLine(String.Format("OutputLocationCode: {0}", OutputLocationCode));
            sb.AppendLine(String.Format("Printer: {0}\n", Printer));
            return sb.ToString();
        }
    }
}
