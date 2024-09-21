--CREATE TABLE LOGG (
--    srNo INT IDENTITY(1,1) PRIMARY KEY,
  --  table_name VARCHAR(50),
    --event VARCHAR(50),
    --event_date DATE,
    --event_time TIME,
    --no_of_rows_effected INT
--);

-- Insert trigger for EMPLOYEE
CREATE TRIGGER trg_employee_inserted
ON EMPLOYEE
AFTER INSERT
AS
BEGIN
    INSERT INTO LOGG (table_name, event, event_date, event_time, no_of_rows_effected)
    VALUES ('EMPLOYEE', 'INSERT', CONVERT(DATE, GETDATE()), CONVERT(TIME, GETDATE()), (SELECT COUNT(*) FROM inserted));
END;
GO

-- Update trigger for EMPLOYEE
CREATE TRIGGER trg_employee_update
ON EMPLOYEE
AFTER UPDATE
AS
BEGIN
    INSERT INTO LOGG (table_name, event, event_date, event_time, no_of_rows_effected)
    VALUES ('EMPLOYEE', 'UPDATE', CONVERT(DATE, GETDATE()), CONVERT(TIME, GETDATE()), (SELECT COUNT(*) FROM inserted));
END;
GO

-- Delete trigger for EMPLOYEE
CREATE TRIGGER trg_employee_deletedd
ON EMPLOYEE
INSTEAD OF DELETE
AS
BEGIN
    IF EXISTS (SELECT * FROM deleted d JOIN Project_Allocation pa ON d.eid = pa.eid)
    BEGIN
        RAISERROR ('Deletion from EMPLOYEE table is not allowed because of existing references in Project_Allocation', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
    IF EXISTS (SELECT * FROM deleted d JOIN ACCOUNTS a ON d.eid = a.eid)
    BEGIN
        RAISERROR ('Deletion from EMPLOYEE table is not allowed because of existing references in ACCOUNTS', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
    
    INSERT INTO LOGG(table_name, event, event_date, event_time, no_of_rows_effected)
    VALUES ('EMPLOYEE', 'DELETE', CONVERT(DATE, GETDATE()), CONVERT(TIME, GETDATE()), (SELECT COUNT(*) FROM deleted));

    DELETE FROM EMPLOYEE WHERE eid IN (SELECT eid FROM deleted);
END;
GO
