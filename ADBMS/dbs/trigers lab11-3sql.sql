-- Drop existing triggers if they exist
DROP TRIGGER IF EXISTS trg_prevent_accounts_delete;
DROP TRIGGER IF EXISTS trg_prevent_accounts_backup_delete;
DROP TRIGGER IF EXISTS trg_after_accounts_insert;
DROP TRIGGER IF EXISTS trg_after_accounts_update;
DROP TRIGGER IF EXISTS trg_prevent_employee_delete;
DROP TRIGGER IF EXISTS trg_prevent_project_delete;
GO

-- Prevent deletion from ACCOUNTS
CREATE TRIGGER trg_prevent_accounts_deleted
ON ACCOUNTS
INSTEAD OF DELETE
AS
BEGIN
    RAISERROR ('Deletion from ACCOUNTS table is not allowed', 16, 1);
    ROLLBACK TRANSACTION;
END;
GO

-- Prevent deletion from ACCOUNTS_BACKUP
CREATE TRIGGER trg_prevent_accounts_backup_deleted
ON ACCOUNTS_BACKUP
INSTEAD OF DELETE
AS
BEGIN
    RAISERROR ('Deletion from ACCOUNTS_BACKUP table is not allowed', 16, 1);
    ROLLBACK TRANSACTION;
END;
GO

-- Trigger to prevent deletion from EMPLOYEE if referenced in Project_Allocation or ACCOUNTS
CREATE TRIGGER trg_prevent_employee_delete
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
    DELETE FROM EMPLOYEE WHERE eid IN (SELECT eid FROM deleted);
END;
GO

-- Trigger to prevent deletion from PROJECT if referenced in Project_Allocation
CREATE TRIGGER trg_prevent_project_delete
ON PROJECT
INSTEAD OF DELETE
AS
BEGIN
    IF EXISTS (SELECT * FROM deleted d JOIN Project_Allocation pa ON d.pid = pa.pid)
    BEGIN
        RAISERROR ('Deletion from PROJECT table is not allowed because of existing references in Project_Allocation', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
    DELETE FROM PROJECT WHERE pid IN (SELECT pid FROM deleted);
END;
GO

-- Trigger to handle insert on ACCOUNTS and calculate Project_Bonus
CREATE TRIGGER trg_after_accounts_inserted
ON ACCOUNTS
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @eid INT, @Year INT, @Month INT, @Basic_Salary DECIMAL(10, 2), @total_bonus DECIMAL(10, 2);

    -- Iterate over the inserted rows
    DECLARE insert_cursor CURSOR FOR
    SELECT eid, Year, Month, Basic_Salary
    FROM inserted;

    OPEN insert_cursor;

    FETCH NEXT FROM insert_cursor INTO @eid, @Year, @Month, @Basic_Salary;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Ensure eid exists in EMPLOYEE
        IF NOT EXISTS (SELECT 1 FROM EMPLOYEE WHERE eid = @eid)
        BEGIN
            RAISERROR ('Employee ID does not exist in EMPLOYEE table', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Ensure unique primary key constraint (eid, Year, Month)
        IF EXISTS (SELECT 1 FROM ACCOUNTS WHERE eid = @eid AND Year = @Year AND Month = @Month)
        BEGIN
            RAISERROR ('Duplicate primary key (eid, Year, Month) in ACCOUNTS table', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Calculate Project_Bonus based on projects assigned to employee
        SELECT @total_bonus = ISNULL(SUM(p.pBonusAmount), 0)
        FROM PROJECT p
        JOIN Project_Allocation pa ON p.pid = pa.pid
        WHERE pa.eid = @eid;

        -- Update the Project_Bonus in the ACCOUNTS table
        UPDATE ACCOUNTS
        SET Project_Bonus = @total_bonus
        WHERE eid = @eid AND Year = @Year AND Month = @Month;

        -- Insert into ACCOUNTS_BACKUP
        INSERT INTO ACCOUNTS_BACKUP (eid, Year, Month, Basic_Salary, Project_Bonus)
        VALUES (@eid, @Year, @Month, @Basic_Salary, @total_bonus);

        FETCH NEXT FROM insert_cursor INTO @eid, @Year, @Month, @Basic_Salary;
    END;

    CLOSE insert_cursor;
    DEALLOCATE insert_cursor;
END;
GO

-- Trigger to handle update on ACCOUNTS and ensure constraints
CREATE TRIGGER trg_after_accounts_update
ON ACCOUNTS
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @eid INT, @Year INT, @Month INT, @Basic_Salary DECIMAL(10, 2), @total_bonus DECIMAL(10, 2);

    -- Iterate over the updated rows
    DECLARE update_cursor CURSOR FOR
    SELECT eid, Year, Month, Basic_Salary
    FROM inserted;

    OPEN update_cursor;

    FETCH NEXT FROM update_cursor INTO @eid, @Year, @Month, @Basic_Salary;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Ensure eid exists in EMPLOYEE
        IF NOT EXISTS (SELECT 1 FROM EMPLOYEE WHERE eid = @eid)
        BEGIN
            RAISERROR ('Employee ID does not exist in EMPLOYEE table', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Calculate Project_Bonus based on projects assigned to employee
        SELECT @total_bonus = ISNULL(SUM(p.pBonusAmount), 0)
        FROM PROJECT p
        JOIN Project_Allocation pa ON p.pid = pa.pid
        WHERE pa.eid = @eid;

        -- Update the Project_Bonus in the ACCOUNTS table
        UPDATE ACCOUNTS
        SET Project_Bonus = @total_bonus
        WHERE eid = @eid AND Year = @Year AND Month = @Month;

        -- Update ACCOUNTS_BACKUP
        UPDATE ACCOUNTS_BACKUP
        SET Basic_Salary = @Basic_Salary,
            Project_Bonus = @total_bonus
        WHERE eid = @eid AND Year = @Year AND Month = @Month;

        FETCH NEXT FROM update_cursor INTO @eid, @Year, @Month, @Basic_Salary;
    END;

    CLOSE update_cursor;
    DEALLOCATE update_cursor;
END;
GO
