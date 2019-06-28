select
	ord.Id,
	ord.Region as 'Region from order',
	ord.City as 'City from order',
	cust.Region as 'Region from Cust',
	cust.City as 'City from Cust'
from [Orders] ord
	join [Customers] cust on cust.Id = ord.CustomerId





UPDATE [Orders]
SET
	Region = cust.Region,
	City = cust.City
FROM [Orders] ord
	INNER JOIN [Customers] cust ON cust.Id = ord.CustomerId


/*

migrationBuilder.Sql(@"
                UPDATE [Orders]
                SET
                    Region = cust.Region,
                    City = cust.City
                FROM[Orders] ord
                    INNER JOIN[Customers] cust ON cust.Id = ord.CustomerId");

*/