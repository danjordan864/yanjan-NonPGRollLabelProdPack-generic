using System;
using RollLabelProdPack.Library.Utility;
using RollLabelProdPack.SAP.B1;
using SAPbobsCOM;

namespace RollLabelProdPack.Utilities
{
    /// <summary>
    /// Standalone program to create UDFs for generic roll label production
    ///
    /// USAGE:
    /// 1. Add this file to your RollLabelProdPack project
    /// 2. Temporarily change the startup object to this class (or create a menu item to call CreateUDFs())
    /// 3. Run the program
    /// 4. UDFs will be created in SAP Business One
    ///
    /// OR: Call CreateUDFs() from a button click event in your application
    /// </summary>
    class CreateUDFs_Program
    {
        /// <summary>
        /// Main entry point for standalone execution
        /// </summary>
        /// <param name="args">Command line arguments</param>
        static void Main(string[] args)
        {
            Console.WriteLine("==========================================");
            Console.WriteLine("SAP B1 UDF Creator for Roll Label Production");
            Console.WriteLine("==========================================");
            Console.WriteLine();

            CreateUDFs();

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Creates all UDFs for generic roll label production
        /// This method can be called from anywhere in your application
        /// </summary>
        public static void CreateUDFs()
        {
            SAPB1 sapB1 = null;

            try
            {
                Console.WriteLine("Connecting to SAP Business One...");

                // Get SAP credentials from config
                // You may need to adjust this based on your authentication method
                var userName = AppUtility.GetSAPUser_Line1(); // or use specific user
                var password = AppUtility.GetSAPPass_Line1(); // or use specific password

                // Connect to SAP
                sapB1 = new SAPB1(userName, password);

                Console.WriteLine("✓ Connected to SAP Business One");
                Console.WriteLine($"  Company: {sapB1.SapCompany.CompanyName}");
                Console.WriteLine($"  Database: {sapB1.SapCompany.CompanyDB}");
                Console.WriteLine();

                // Create UDF creator
                var udfCreator = new UDFCreator(sapB1.SapCompany);

                Console.WriteLine("Creating UDFs on OWOR (Production Orders) table...");
                Console.WriteLine("==========================================");
                Console.WriteLine();

                // Create all UDFs
                var result = udfCreator.CreateAllLabelUDFs();

                // Display results
                Console.WriteLine(result.Message);

                if (result.Success)
                {
                    Console.WriteLine();
                    Console.WriteLine("Verifying UDFs created...");
                    Console.WriteLine("==========================================");

                    var existingUDFs = udfCreator.GetExistingUDFs("OWOR");
                    if (existingUDFs.Count > 0)
                    {
                        Console.WriteLine("Found UDFs:");
                        foreach (var udf in existingUDFs)
                        {
                            Console.WriteLine($"  {udf}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("  No UDFs found (this may indicate an error)");
                    }

                    Console.WriteLine();
                    Console.WriteLine("==========================================");
                    Console.WriteLine("SUCCESS: All UDFs created successfully!");
                    Console.WriteLine("==========================================");
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("==========================================");
                    Console.WriteLine("WARNING: Some UDFs failed to create.");
                    Console.WriteLine("See error messages above for details.");
                    Console.WriteLine("==========================================");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("==========================================");
                Console.WriteLine("ERROR: Failed to create UDFs");
                Console.WriteLine("==========================================");
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine();
                Console.WriteLine("Stack Trace:");
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                // Disconnect from SAP
                if (sapB1 != null)
                {
                    sapB1.Dispose();
                    Console.WriteLine();
                    Console.WriteLine("Disconnected from SAP Business One");
                }
            }
        }

        /// <summary>
        /// Alternative method to create UDFs with custom credentials
        /// </summary>
        /// <param name="userName">SAP user name</param>
        /// <param name="password">SAP password</param>
        public static void CreateUDFsWithCredentials(string userName, string password)
        {
            SAPB1 sapB1 = null;

            try
            {
                Console.WriteLine($"Connecting to SAP as user: {userName}...");

                sapB1 = new SAPB1(userName, password);

                Console.WriteLine("✓ Connected to SAP Business One");
                Console.WriteLine();

                var udfCreator = new UDFCreator(sapB1.SapCompany);
                var result = udfCreator.CreateAllLabelUDFs();

                Console.WriteLine(result.Message);

                if (result.Success)
                {
                    Console.WriteLine();
                    Console.WriteLine("SUCCESS!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                throw;
            }
            finally
            {
                sapB1?.Dispose();
            }
        }
    }
}
