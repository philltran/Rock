IF OBJECT_ID(N'[dbo].[AnalyticsDimFamilyHeadOfHousehold]', 'V') IS NOT NULL
    DROP VIEW [dbo].[AnalyticsDimFamilyHeadOfHousehold]
GO

CREATE VIEW [dbo].[AnalyticsDimFamilyHeadOfHousehold]
AS
SELECT * FROM AnalyticsDimPersonCurrent