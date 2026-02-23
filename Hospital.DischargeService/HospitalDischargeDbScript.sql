CREATE DATABASE HospitalDischargeDb;
GO

USE HospitalDischargeDb;
GO

CREATE TABLE DischargeSummaries (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    PatientId UNIQUEIDENTIFIER,
    Diagnosis NVARCHAR(500),
    Treatment NVARCHAR(500),
    AIDietRecommendation NVARCHAR(500),
    DischargedOn DATETIME2
);