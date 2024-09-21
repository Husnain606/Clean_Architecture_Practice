CREATE TRIGGER trg_Course_Insert
ON course
INSTEAD OF INSERT
AS
BEGIN
    -- Check for existing records with the same primary key
    IF EXISTS (
        SELECT 1
        FROM course c
        JOIN INSERTED i ON c.cid = i.cid
    )
    BEGIN
        -- Raise an error if a duplicate primary key is found
        RAISERROR ('Primary key violation: course_id already exists', 16, 1);
        ROLLBACK;
    END
    ELSE
    BEGIN
        -- Insert the record if no primary key violation
        INSERT INTO course (cid, title,category,cr_hours)
        SELECT *
        FROM INSERTED;
    END
END;
