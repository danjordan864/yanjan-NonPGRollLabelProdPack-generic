using System;
using System.Collections.Generic;
using System.Text;
using SAPbobsCOM;

namespace RollLabelProdPack.Library.Utility
{
    /// <summary>
    /// Utility class for creating User-Defined Fields (UDFs) in SAP Business One using DI API
    /// </summary>
    public class UDFCreator
    {
        private Company _company;

        /// <summary>
        /// Initializes a new instance of UDFCreator with an existing SAP company connection
        /// </summary>
        /// <param name="company">Connected SAP Company object</param>
        public UDFCreator(Company company)
        {
            _company = company ?? throw new ArgumentNullException(nameof(company));
        }

        /// <summary>
        /// Creates all 6 UDFs required for generic roll label production on OWOR table
        /// </summary>
        /// <returns>Success status and details message</returns>
        public (bool Success, string Message) CreateAllLabelUDFs()
        {
            var results = new StringBuilder();
            bool allSuccess = true;

            try
            {
                // UDF 1: Roll Label Format Path
                var result1 = CreateUDF(
                    "OWOR",
                    "SII_RollLabel",
                    "Roll Label Format Path",
                    BoFieldTypes.db_Alpha,
                    BoFldSubTypes.st_None,
                    50,
                    null,
                    false
                );
                results.AppendLine(result1.Message);
                allSuccess &= result1.Success;

                // UDF 2: Bundle Label Format Path
                var result2 = CreateUDF(
                    "OWOR",
                    "SII_BunLabel",
                    "Bundle Label Format Path",
                    BoFieldTypes.db_Alpha,
                    BoFldSubTypes.st_None,
                    50,
                    null,
                    false
                );
                results.AppendLine(result2.Message);
                allSuccess &= result2.Success;

                // UDF 3: Core Label Format Path
                var result3 = CreateUDF(
                    "OWOR",
                    "SII_CoreLabel",
                    "Core Label Format Path",
                    BoFieldTypes.db_Alpha,
                    BoFldSubTypes.st_None,
                    50,
                    null,
                    false
                );
                results.AppendLine(result3.Message);
                allSuccess &= result3.Success;

                // UDF 4: Roll Label Print Quantity
                var result4 = CreateUDF(
                    "OWOR",
                    "SII_RLabelQty",
                    "Roll Label Print Quantity",
                    BoFieldTypes.db_Numeric,
                    BoFldSubTypes.st_None,
                    10,
                    "1",
                    true
                );
                results.AppendLine(result4.Message);
                allSuccess &= result4.Success;

                // UDF 5: Bundle Label Print Quantity
                var result5 = CreateUDF(
                    "OWOR",
                    "SII_BLabel",
                    "Bundle Label Print Quantity",
                    BoFieldTypes.db_Numeric,
                    BoFldSubTypes.st_None,
                    10,
                    "1",
                    true
                );
                results.AppendLine(result5.Message);
                allSuccess &= result5.Success;

                // UDF 6: Core Label Print Quantity
                var result6 = CreateUDF(
                    "OWOR",
                    "SII_CLabel",
                    "Core Label Print Quantity",
                    BoFieldTypes.db_Numeric,
                    BoFldSubTypes.st_None,
                    10,
                    "1",
                    true
                );
                results.AppendLine(result6.Message);
                allSuccess &= result6.Success;

                if (allSuccess)
                {
                    results.AppendLine();
                    results.AppendLine("========================================");
                    results.AppendLine("All UDFs created successfully!");
                    results.AppendLine("========================================");
                }
                else
                {
                    results.AppendLine();
                    results.AppendLine("========================================");
                    results.AppendLine("WARNING: Some UDFs failed to create. See details above.");
                    results.AppendLine("========================================");
                }

                return (allSuccess, results.ToString());
            }
            catch (Exception ex)
            {
                return (false, $"Error creating UDFs: {ex.Message}\n\n{results}");
            }
        }

        /// <summary>
        /// Creates a single UDF using SAP DI API
        /// </summary>
        /// <param name="tableName">Table name (e.g., "OWOR")</param>
        /// <param name="fieldName">Field name without U_ prefix (e.g., "SII_RollLabel")</param>
        /// <param name="description">Field description</param>
        /// <param name="fieldType">Field type (Alpha, Numeric, Date, etc.)</param>
        /// <param name="subType">Field subtype</param>
        /// <param name="size">Field size</param>
        /// <param name="defaultValue">Default value (null for no default)</param>
        /// <param name="mandatory">Whether field is mandatory</param>
        /// <returns>Success status and message</returns>
        public (bool Success, string Message) CreateUDF(
            string tableName,
            string fieldName,
            string description,
            BoFieldTypes fieldType,
            BoFldSubTypes subType,
            int size,
            string defaultValue,
            bool mandatory)
        {
            UserFieldsMD userFieldsMD = null;

            try
            {
                // Check if UDF already exists
                if (UDFExists(tableName, fieldName))
                {
                    return (true, $"UDF U_{fieldName} already exists on {tableName} - skipped");
                }

                // Create UserFieldsMD object
                userFieldsMD = (UserFieldsMD)_company.GetBusinessObject(BoObjectTypes.oUserFields);

                // Set UDF properties
                userFieldsMD.TableName = tableName;
                userFieldsMD.Name = fieldName;
                userFieldsMD.Description = description;
                userFieldsMD.Type = fieldType;
                userFieldsMD.SubType = subType;

                // Set size for alphanumeric fields
                if (fieldType == BoFieldTypes.db_Alpha)
                {
                    userFieldsMD.EditSize = size;
                }

                // Set default value if provided
                if (!string.IsNullOrEmpty(defaultValue))
                {
                    userFieldsMD.DefaultValue = defaultValue;
                }

                // Set mandatory flag
                userFieldsMD.Mandatory = mandatory ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;

                // Add the UDF
                int result = userFieldsMD.Add();

                if (result == 0)
                {
                    return (true, $"✓ Created UDF U_{fieldName} on {tableName} - {description}");
                }
                else
                {
                    string errorMessage = _company.GetLastErrorDescription();
                    return (false, $"✗ Failed to create UDF U_{fieldName}: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                return (false, $"✗ Exception creating UDF U_{fieldName}: {ex.Message}");
            }
            finally
            {
                // Release COM object
                if (userFieldsMD != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(userFieldsMD);
                    userFieldsMD = null;
                }
            }
        }

        /// <summary>
        /// Checks if a UDF already exists on a table
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="fieldName">Field name without U_ prefix</param>
        /// <returns>True if UDF exists, false otherwise</returns>
        private bool UDFExists(string tableName, string fieldName)
        {
            UserFieldsMD userFieldsMD = null;

            try
            {
                userFieldsMD = (UserFieldsMD)_company.GetBusinessObject(BoObjectTypes.oUserFields);

                // Try to get the field by table and name
                // The GetByKey method uses TableName and FieldID, but we can loop through to check existence
                Recordset recordSet = (Recordset)_company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string query = $"SELECT FieldID FROM CUFD WHERE TableID = '{tableName}' AND AliasID = '{fieldName}'";
                recordSet.DoQuery(query);

                bool exists = !recordSet.EoF;

                System.Runtime.InteropServices.Marshal.ReleaseComObject(recordSet);

                return exists;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (userFieldsMD != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(userFieldsMD);
                }
            }
        }

        /// <summary>
        /// Gets information about existing UDFs on a table
        /// </summary>
        /// <param name="tableName">Table name (e.g., "OWOR")</param>
        /// <returns>List of UDF information</returns>
        public List<string> GetExistingUDFs(string tableName)
        {
            var udfs = new List<string>();
            Recordset recordSet = null;

            try
            {
                recordSet = (Recordset)_company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string query = $@"
                    SELECT
                        AliasID,
                        Descr,
                        TypeID,
                        SizeID,
                        Dflt,
                        NotNull
                    FROM CUFD
                    WHERE TableID = '{tableName}'
                    AND AliasID LIKE 'SII_%Label%'
                    ORDER BY FieldID";

                recordSet.DoQuery(query);

                while (!recordSet.EoF)
                {
                    string aliasID = recordSet.Fields.Item("AliasID").Value?.ToString() ?? "";
                    string descr = recordSet.Fields.Item("Descr").Value?.ToString() ?? "";
                    string typeID = recordSet.Fields.Item("TypeID").Value?.ToString() ?? "";
                    string sizeID = recordSet.Fields.Item("SizeID").Value?.ToString() ?? "";
                    string dflt = recordSet.Fields.Item("Dflt").Value?.ToString() ?? "";
                    string notNull = recordSet.Fields.Item("NotNull").Value?.ToString() ?? "";

                    udfs.Add($"U_{aliasID} ({descr}) - Type: {typeID}, Size: {sizeID}, Default: {dflt}, Mandatory: {notNull}");

                    recordSet.MoveNext();
                }

                return udfs;
            }
            catch (Exception ex)
            {
                udfs.Add($"Error retrieving UDFs: {ex.Message}");
                return udfs;
            }
            finally
            {
                if (recordSet != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recordSet);
                }
            }
        }
    }
}
