select top 10
	p.Name,
	p.VendorCode,
	count(p.Id) as quantity
from [OrderProducts] op
	join [Products] p on p.Id = op.ProductId
where p.CategoryId = '88CD0F34-9D4A-4E45-BE97-8899A97FB82C'
group by p.Id, p.Name, p.VendorCode
order by quantity desc