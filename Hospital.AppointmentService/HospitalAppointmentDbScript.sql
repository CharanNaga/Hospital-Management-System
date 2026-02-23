CREATE DATABASE HospitalAppointmentDb;
GO

USE HospitalAppointmentDb;
GO

CREATE TABLE Appointments (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    PatientId UNIQUEIDENTIFIER,
    DoctorId UNIQUEIDENTIFIER,
    AppointmentDate DATETIME2,
    Status NVARCHAR(50)
);