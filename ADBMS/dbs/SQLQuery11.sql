CREATE TRIGGER trg_Student_Backup
ON Student
AFTER INSERT, UPDATE
AS
BEGIN
    INSERT INTO Student_BACKUP (sID, sName, sAge, sCGPA ,deleted_at)
    SELECT sID, fname, Age, CGPA, GETDATE()
    FROM INSERTED
    WHERE CGPA < 3;
END;
