-- Migration script to populate UserStores table from existing StoreOfOperationId data
-- Run this after applying the AddUserStoreJunctionTable migration

INSERT INTO UserStores (UserId, StoreId, IsPrimary)
SELECT 
    Id as UserId,
    StoreOfOperationId as StoreId,
    1 as IsPrimary  -- Mark as primary store
FROM AspNetUsers
WHERE StoreOfOperationId IS NOT NULL
  AND NOT EXISTS (
      SELECT 1 FROM UserStores 
      WHERE UserStores.UserId = AspNetUsers.Id 
      AND UserStores.StoreId = AspNetUsers.StoreOfOperationId
  );

-- Verification query
SELECT 
    u.UserName,
    u.StoreOfOperationId as OldStore,
    us.StoreId as NewStoreId,
    us.IsPrimary,
    s.Name as StoreName
FROM AspNetUsers u
LEFT JOIN UserStores us ON u.Id = us.UserId
LEFT JOIN Stores s ON us.StoreId = s.Id
WHERE u.StoreOfOperationId IS NOT NULL
ORDER BY u.UserName;
