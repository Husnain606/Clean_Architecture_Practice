CREATE TRIGGER trg_Accounts_insert
ON ACCOUNTS
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO ACCOUNTS_BACKUP (eid, Year, Month, Basic_Salary, Project_Bonus)
    SELECT eid, Year, Month, Basic_Salary, Project_Bonus
    FROM inserted;
END;
GO


CREATE TRIGGER trg_Accounts_deleted
ON ACCOUNTS
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO ACCOUNTS_BACKUP (eid, Year, Month, Basic_Salary, Project_Bonus)
    SELECT eid, Year, Month, Basic_Salary, Project_Bonus
    FROM inserted;
END;
GO


-- Prevent deletion from ACCOUNTS
CREATE TRIGGER trg_prevent_accounts_delete
ON ACCOUNTS
AFTER DELETE
AS
BEGIN
    RAISERROR ('Deletion from ACCOUNTS table is not allowed', 16, 1);
    ROLLBACK TRANSACTION;
END;
GO

-- Prevent deletion from ACCOUNTS_BACKUP
CREATE TRIGGER trg_prevent_accounts_backup_delete
ON ACCOUNTS_BACKUP
AFTER DELETE
AS
BEGIN
    RAISERROR ('Deletion from ACCOUNTS_BACKUP table is not allowed', 16, 1);
    ROLLBACK TRANSACTION;
END;
GO


-- Create the insert trigger
CREATE TRIGGER trg_after_accounts_insert
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
