use StoreRepository

-- Start the transaction
BEGIN TRANSACTION;

--Insert the role details in the table

Insert into Roles(RoleName,RoleDescription,RoleCode) values('Admin','This role have have all type of accesses','ADM'),('Edit','This role will have permission to edit but not as Admin','EDIT')
,('View','This role will have only View access','VIEW')

-- Check if both updates were successful
IF @@ERROR = 0
BEGIN
    -- Commit the transaction if successful
    COMMIT;
    PRINT 'Transaction committed successfully.';
END
ELSE
BEGIN
    -- Rollback the transaction if there is an error
    ROLLBACK;
    PRINT 'Transaction failed. Changes rolled back.';
END;
