MERGE INTO Customers AS target
USING (
    VALUES 
    ('Shabill', 'Irfani', 'shabill97@gmail.com', '2020-08-10'),
    ('Shabill', 'Irfani', 'shabill97@gmail.com', '2020-08-10'),
    ('Shabill', 'Irfani', 'shabill97@gmail.com', '2020-08-10'),
    ('Shabill', 'Kahn', 'shabill97@gmail.com', '2020-08-10'),
    ('Zahid', 'Alam', 'zahid09@gmail.com', '2021-11-20'),
    ('Zahid', 'Alam', 'zahid09@gmail.com', '2021-11-20'),
    ('Zahid', 'Ahmed', 'zahidahamed@gmail.com', '2022-12-25'),
    ('Abid', 'Ahmed', 'abidahmed@gmail.com', '2019-11-22'),
    ('Abid', 'Ahmed', 'abidahmed@gmail.com', '2019-11-22'),
    ('Salman', 'Irfani', 'salmanirfani@gmail.com', '2018-02-21'),
    ('Salman', 'Irfani', 'salmanirfani@gmail.com', '2018-02-21'),
    ('Zahid', 'Alam', 'zahidahmed@gmail.com', '2022-12-25'),
    ('Salman', 'Khan', 'salmankhan@gmail.com', '2024-04-25'),
    ('Rohit', 'Kumar', 'rohitkumar@gmail.com', '2020-08-20'),
    ('Rohit', 'Sharma', 'rohitsharma@gmail.com', '2022-10-22'),
    ('Salman', 'Khan', 'salmankhan@gmail.com', '2024-04-25')
) AS source (FirstName, LastName, Email, RegistrationDate)
ON target.Email = source.Email
WHEN NOT MATCHED THEN
    INSERT (FirstName, LastName, Email, RegistrationDate)
    VALUES (source.FirstName, source.LastName, source.Email, source.RegistrationDate);
