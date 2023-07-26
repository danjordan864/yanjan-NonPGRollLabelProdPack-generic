using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollLabelProdPack.SAP.B1
{
    public interface IB1Object
    {

        /// <summary>
        /// Returns a dictionary of the User Defined Fields (UDF) used by the root SAP B1 object in the class.  The dictionary is based on the 
        /// string name of the UDF and returns the given UDF's index position as known by the UserDefinedFields property.
        /// </summary>
        Dictionary<string, int> UserDefinedFieldsDictionary { get; set; }

        /// <summary>
        /// Returns the root SAP B1 object's User Defined Fields object based on the SAP DI API UserFields interface.
        /// </summary>
        UserFields UserDefinedFields { get; }

        /// <summary>
        /// Returns the SAP Object Type used by the class.
        /// </summary>
        BoObjectTypes SAPObjectType { get; }

        /// <summary>
        /// Loads the UserDefinedFieldsDictionary variable based on the given SAP B1 object type
        /// </summary>
        void LoadUDFDictionary();

        /// <summary>
        /// Sets the value of the User Defined Field
        /// </summary>
        /// <param name="key">The name of the User Defined Field as known by the database.</param>
        /// <param name="value">The new value of the UDF.  Please note, this accepts a dynamic value that requires checking before assignement.</param>
        void SetUDFValue(string key, dynamic value);

        /// <summary>
        /// Sets the value of the User Defined Field
        /// </summary>
        /// <param name="key">The INDEX of the User Defined Field as known by the returning array.</param>
        /// <param name="value">The new value of the UDF.  Please note, this accepts a dynamic value that requires checking before assignement.</param>
        void SetUDFValue(int key, dynamic value);

        /// <summary>
        /// Returns the value of a given user defined field.
        /// </summary>
        /// <param name="key">The name of the User Defined Field as known by the database.</param>
        /// <returns>Returns the value of the User Defined Field.  Based on the dynamic type, further casting may be required.</returns>
        dynamic GetUDFValue(string key);

        /// <summary>
        /// Returns the value of a given user defined field.
        /// </summary>
        /// <param name="key">The INDEX of the User Defined Field as known by the returning array.</param>
        /// <returns>Returns the value of the User Defined Field.  Based on the dynamic type, further casting may be required.</returns>
        dynamic GetUDFValue(int key);

        /// <summary>
        /// Saves the instance of the B1 Object.
        /// </summary>
        /// <returns>A boolean flag that reports success or failure.  Please call the GetLastError method from SAPB1.</returns>
        bool Save();

    }
}
