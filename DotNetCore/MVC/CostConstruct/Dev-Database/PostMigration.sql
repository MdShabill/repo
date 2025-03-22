/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
MERGE INTO Countries AS Target
USING (VALUES 
        ('US'), ('UAE'), ('India'), ('Pakistan'), ('Qatar')
      ) AS Source (Name)
ON Target.Name = Source.Name
WHEN NOT MATCHED BY TARGET THEN
    INSERT (Name) VALUES (Source.Name);


MERGE INTO AddressTypes AS Target
USING (VALUES 
        ('Home'), ('Office'), ('Apartment'), ('Factory'), ('FarmHouse')
      ) AS Source (Name)
ON Target.Name = Source.Name
WHEN NOT MATCHED BY TARGET THEN
    INSERT (Name) VALUES (Source.Name);


MERGE INTO Brands AS Target
USING (VALUES 
    ('Ambuja'), ('JSW'), ('UltraTech'), ('Jindal Panther'), ('Shyam TMT Bar'),
    ('Kamdhenu'), ('Tata Tiscon'), ('Berger Paints'), ('Indigo Paints'), ('Dulux'),
    ('Nerolac Paints'), ('Asian Paints'), ('Kajaria Tiles'), ('Johnson Tiles'),
    ('Orientbell Tiles'), ('Nitco Tiles'), ('AGL Tiles'), ('Hindware'), ('CERA'), 
    ('Jaquar'), ('Parryware'), ('Babar Enterprises')
) AS Source (Name)
ON Target.Name = Source.Name
WHEN NOT MATCHED BY TARGET THEN
    INSERT (Name) VALUES (Source.Name);


MERGE INTO JobCategories AS Target
USING (VALUES 
    ('Master Mason'), ('Labour'), ('Electrician'), ('Electrician Assistant'), ('Plumber'),
    ('Plumber Assistant'), ('Painter'), ('Painter Helper'), ('Carpenter'), ('Carpenter Assistant'),
    ('Tiler'), ('Tiler Assistant')
) AS Source (Name)
ON Target.Name = Source.Name
WHEN NOT MATCHED BY TARGET THEN
    INSERT (Name) VALUES (Source.Name);


MERGE INTO MaterialTypes AS Target
USING (VALUES 
    ('Paint'), ('Cement'), ('Steel'), ('Bricks'), ('Electrical'),
    ('Plumbing'), ('Tiles'), ('Sand'), ('Crushed Stone'), ('Sanitary')
) AS Source (Name)
ON Target.Name = Source.Name
WHEN NOT MATCHED BY TARGET THEN
    INSERT (Name) VALUES (Source.Name);


MERGE INTO Suppliers AS Target
USING (VALUES 
    ('ZamZam'), ('Aashiyana Hardware'), ('Sankar Electronic'), ('Maruf Hindustan Hardware')
) AS Source (Name)
ON Target.Name = Source.Name
WHEN NOT MATCHED BY TARGET THEN
    INSERT (Name) VALUES (Source.Name);