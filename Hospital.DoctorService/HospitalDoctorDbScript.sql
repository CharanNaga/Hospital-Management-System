CREATE DATABASE HospitalDoctorDb;
GO

USE HospitalDoctorDb;
GO

CREATE TABLE Doctors (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    FullName NVARCHAR(150),
    Specialization NVARCHAR(150),
    Email NVARCHAR(100)
);