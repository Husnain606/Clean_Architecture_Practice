CREATE TRIGGER trg_Student_Insert
ON student
INSTEAD OF INSERT
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM INSERTED
        WHERE Age NOT BETWEEN 18 AND 25
           OR CGPA NOT BETWEEN 0.0 AND 4.0
    )
    BEGIN
        -- Handle the error, e.g., RAISERROR or just do nothing
        RAISERROR ('Invalid age or CGPA', 16, 1);
        ROLLBACK;
    END
    ELSE
    BEGIN
        INSERT INTO student (sID, fname, lname,city, Age, CGPA,degree,semesterno,section)
        SELECT *
        FROM INSERTED;
    END
END;
