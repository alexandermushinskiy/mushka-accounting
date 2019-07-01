SELECT
	FirstName,
	LastName,
	COUNT(Id)
FROM Customers
GROUP BY FirstName, LastName
HAVING COUNT(*) > 1