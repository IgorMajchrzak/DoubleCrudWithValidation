# DoubleCrudWithValidation
A WPF app capable of CRUD operations on SQL-based instance as well as MongoDB instances. Includes input validation and basic error handling.

**IMPORTANT: Database and table/collection names, as well as item properties are hardcoded.** The app may be developed further to allow higher flexibility,
however this was deemed lower priority under time constraints.

## SQL
The app is written to operate on a 'products' table inside a 'shop' database.
The table structure is as follows:
- `ProductId` - int - auto incremented, primary key
- `ProductName` - varchar(200)
- `Description` - text
- `Price` - decimal(15,2)
- `NumberInStock` - int

Ready SQL code to create the table:
```
CREATE TABLE `products` (
  `ProductId` int(11) NOT NULL,
  `ProductName` varchar(200) NOT NULL,
  `Description` text NOT NULL,
  `Price` decimal(15,2) NOT NULL,
  `NumberInStock` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
```

## MongoDB
The app is written to operate on a 'People' collection inside a 'PeopleDb' database.
The supported document structure is as follows:
- _id: ObjectId
- _t: string
- FirstName: string
- LastName: string
- Age: int

The app can connect to a MongoDB instance using a `mongodb://` or `mongodb+srv://` string, with or without authentication.
