CREATE TABLE Student_BACKUP (
    sID INT,
    sName VARCHAR(255),
    sAge INT,
    sCGPA FLOAT,
    -- Add any other columns that exist in the Student table
    deleted_at DATETIME DEFAULT GETDATE() -- Optional: to track when the record was deleted
);
select * from student

CREATE TRIGGER trg_Student_Delete
ON Student
AFTER DELETE
AS
BEGIN
    -- Insert the deleted record into the Student_BACKUP table
    INSERT INTO Student_BACKUP (sID, sName, sAge, sCGPA, deleted_at)
    SELECT sID, fname, Age, CGPA, GETDATE()
    FROM DELETED;
END;
