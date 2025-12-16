-- =============================================
-- Author: RDJ / Claude Sonnet 4.5
-- Create date: 2025-12-16
-- Description: Creates 6 new UDFs on OWOR table for generic roll label production
--
-- ⚠️ IMPORTANT: FOR REFERENCE ONLY - DO NOT EXECUTE DIRECTLY ⚠️
--
-- SAP Business One does NOT support creating UDFs via direct SQL manipulation.
-- The only supported method is using the DI API (Data Interface API).
--
-- USE INSTEAD:
--   1. RollLabelProdPack.Library\Utility\UDFCreator.cs (DI API class)
--   2. CreateUDFs_Program.cs (Console application)
--   3. FrmCreateUDFs.cs (Windows Form utility)
--
-- This SQL script is provided as REFERENCE to show the UDF structure.
--
-- UDF Summary:
--   U_SII_RollLabel  - Roll label .btw file path (Alphanumeric 50, not mandatory)
--   U_SII_BunLabel   - Bundle label .btw file path (Alphanumeric 50, not mandatory)
--   U_SII_CoreLabel  - Core label .btw file path (Alphanumeric 50, not mandatory)
--   U_SII_RLabelQty  - Number of roll labels to print (Numeric 10, mandatory, default 1)
--   U_SII_BLabel     - Number of bundle labels to print (Numeric 10, mandatory, default 1)
--   U_SII_CLabel     - Number of core labels to print (Numeric 10, mandatory, default 1)
-- =============================================

-- ==================================================
-- UDF #1: U_SII_RollLabel - Roll label format path
-- ==================================================
IF NOT EXISTS (SELECT 1 FROM CUFD WHERE TableID = 'OWOR' AND AliasID = 'SII_RollLabel')
BEGIN
    DECLARE @FieldID_RollLabel INT
    SELECT @FieldID_RollLabel = ISNULL(MAX(FieldID), 0) + 1 FROM CUFD WHERE TableID = 'OWOR'

    INSERT INTO CUFD (TableID, FieldID, AliasID, Descr, TypeID, SizeID, EditSize, Dflt, NotNull, IndexID)
    VALUES (
        'OWOR',                          -- TableID: Production Order
        @FieldID_RollLabel,              -- FieldID: Auto-increment
        'SII_RollLabel',                 -- AliasID: Field name without U_ prefix
        'Roll Label Format Path',        -- Description
        'A',                             -- TypeID: Alphanumeric
        50,                              -- SizeID: Max 50 characters
        NULL,                            -- EditSize
        NULL,                            -- Dflt: No default value
        'N',                             -- NotNull: Not mandatory
        NULL                             -- IndexID: No index
    )

    PRINT 'Created UDF: U_SII_RollLabel (Roll Label Format Path)'
END
ELSE
BEGIN
    PRINT 'UDF U_SII_RollLabel already exists - skipping'
END
GO

-- ====================================================
-- UDF #2: U_SII_BunLabel - Bundle label format path
-- ====================================================
IF NOT EXISTS (SELECT 1 FROM CUFD WHERE TableID = 'OWOR' AND AliasID = 'SII_BunLabel')
BEGIN
    DECLARE @FieldID_BunLabel INT
    SELECT @FieldID_BunLabel = ISNULL(MAX(FieldID), 0) + 1 FROM CUFD WHERE TableID = 'OWOR'

    INSERT INTO CUFD (TableID, FieldID, AliasID, Descr, TypeID, SizeID, EditSize, Dflt, NotNull, IndexID)
    VALUES (
        'OWOR',
        @FieldID_BunLabel,
        'SII_BunLabel',
        'Bundle Label Format Path',
        'A',
        50,
        NULL,
        NULL,
        'N',
        NULL
    )

    PRINT 'Created UDF: U_SII_BunLabel (Bundle Label Format Path)'
END
ELSE
BEGIN
    PRINT 'UDF U_SII_BunLabel already exists - skipping'
END
GO

-- ==================================================
-- UDF #3: U_SII_CoreLabel - Core label format path
-- ==================================================
IF NOT EXISTS (SELECT 1 FROM CUFD WHERE TableID = 'OWOR' AND AliasID = 'SII_CoreLabel')
BEGIN
    DECLARE @FieldID_CoreLabel INT
    SELECT @FieldID_CoreLabel = ISNULL(MAX(FieldID), 0) + 1 FROM CUFD WHERE TableID = 'OWOR'

    INSERT INTO CUFD (TableID, FieldID, AliasID, Descr, TypeID, SizeID, EditSize, Dflt, NotNull, IndexID)
    VALUES (
        'OWOR',
        @FieldID_CoreLabel,
        'SII_CoreLabel',
        'Core Label Format Path',
        'A',
        50,
        NULL,
        NULL,
        'N',
        NULL
    )

    PRINT 'Created UDF: U_SII_CoreLabel (Core Label Format Path)'
END
ELSE
BEGIN
    PRINT 'UDF U_SII_CoreLabel already exists - skipping'
END
GO

-- ============================================================
-- UDF #4: U_SII_RLabelQty - Number of roll labels to print
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM CUFD WHERE TableID = 'OWOR' AND AliasID = 'SII_RLabelQty')
BEGIN
    DECLARE @FieldID_RLabelQty INT
    SELECT @FieldID_RLabelQty = ISNULL(MAX(FieldID), 0) + 1 FROM CUFD WHERE TableID = 'OWOR'

    INSERT INTO CUFD (TableID, FieldID, AliasID, Descr, TypeID, SizeID, EditSize, Dflt, NotNull, IndexID)
    VALUES (
        'OWOR',
        @FieldID_RLabelQty,
        'SII_RLabelQty',
        'Roll Label Print Quantity',
        'N',                             -- TypeID: Numeric
        10,                              -- SizeID: Up to 10 digits
        NULL,
        '1',                             -- Dflt: Default value is 1
        'Y',                             -- NotNull: Mandatory
        NULL
    )

    PRINT 'Created UDF: U_SII_RLabelQty (Roll Label Print Quantity, default=1)'
END
ELSE
BEGIN
    PRINT 'UDF U_SII_RLabelQty already exists - skipping'
END
GO

-- ==============================================================
-- UDF #5: U_SII_BLabel - Number of bundle labels to print
-- ==============================================================
IF NOT EXISTS (SELECT 1 FROM CUFD WHERE TableID = 'OWOR' AND AliasID = 'SII_BLabel')
BEGIN
    DECLARE @FieldID_BLabel INT
    SELECT @FieldID_BLabel = ISNULL(MAX(FieldID), 0) + 1 FROM CUFD WHERE TableID = 'OWOR'

    INSERT INTO CUFD (TableID, FieldID, AliasID, Descr, TypeID, SizeID, EditSize, Dflt, NotNull, IndexID)
    VALUES (
        'OWOR',
        @FieldID_BLabel,
        'SII_BLabel',
        'Bundle Label Print Quantity',
        'N',
        10,
        NULL,
        '1',
        'Y',
        NULL
    )

    PRINT 'Created UDF: U_SII_BLabel (Bundle Label Print Quantity, default=1)'
END
ELSE
BEGIN
    PRINT 'UDF U_SII_BLabel already exists - skipping'
END
GO

-- ============================================================
-- UDF #6: U_SII_CLabel - Number of core labels to print
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM CUFD WHERE TableID = 'OWOR' AND AliasID = 'SII_CLabel')
BEGIN
    DECLARE @FieldID_CLabel INT
    SELECT @FieldID_CLabel = ISNULL(MAX(FieldID), 0) + 1 FROM CUFD WHERE TableID = 'OWOR'

    INSERT INTO CUFD (TableID, FieldID, AliasID, Descr, TypeID, SizeID, EditSize, Dflt, NotNull, IndexID)
    VALUES (
        'OWOR',
        @FieldID_CLabel,
        'SII_CLabel',
        'Core Label Print Quantity',
        'N',
        10,
        NULL,
        '1',
        'Y',
        NULL
    )

    PRINT 'Created UDF: U_SII_CLabel (Core Label Print Quantity, default=1)'
END
ELSE
BEGIN
    PRINT 'UDF U_SII_CLabel already exists - skipping'
END
GO

-- ============================================================
-- Verification: Display all created UDFs
-- ============================================================
PRINT ''
PRINT '========================================='
PRINT 'UDF Creation Complete - Summary:'
PRINT '========================================='

SELECT
    TableID,
    FieldID,
    AliasID,
    Descr AS Description,
    TypeID,
    SizeID,
    Dflt AS DefaultValue,
    NotNull AS IsMandatory
FROM CUFD
WHERE TableID = 'OWOR'
    AND AliasID IN (
        'SII_RollLabel',
        'SII_BunLabel',
        'SII_CoreLabel',
        'SII_RLabelQty',
        'SII_BLabel',
        'SII_CLabel'
    )
ORDER BY FieldID

PRINT ''
PRINT 'NOTE: These UDFs will appear in the OWOR table with a U_ prefix:'
PRINT '  - U_SII_RollLabel'
PRINT '  - U_SII_BunLabel'
PRINT '  - U_SII_CoreLabel'
PRINT '  - U_SII_RLabelQty'
PRINT '  - U_SII_BLabel'
PRINT '  - U_SII_CLabel'
PRINT ''
GO
