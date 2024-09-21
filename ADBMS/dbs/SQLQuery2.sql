CREATE TRIGGER trg_Enrollment_Update
ON Enrollment
INSTEAD OF UPDATE
AS
BEGIN
    -- Prevent any updates to sid and cid
    RAISERROR ('Updates to sid and cid columns are not allowed', 16, 1);
    ROLLBACK;
END;
