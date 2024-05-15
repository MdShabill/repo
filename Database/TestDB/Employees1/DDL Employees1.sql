MERGE INTO Employees1 AS target
USING (
    VALUES 
    ('Shabill Irfani', 'IT', 'Management', '2024-05-04'),
    ('Shabill Irfani', 'Management', 'IT', '2024-05-04'),
    ('Shabill Irfani', 'IT', 'Admin', '2024-05-04'),
    ('Shabill Irfani', 'Admin', 'HR', '2024-05-04'),
    ('Shabill Irfani', 'HR', 'Management', '2024-05-04'),
    ('Shabill Irfani', 'Management', 'Finance', '2024-05-04'),
    ('Salman', 'Admin', 'HR', '2024-05-04'),
    ('Salman', 'HR', 'Admin', '2024-05-04'),
    ('Zahid Ahmed', 'HR', 'Management', '2024-05-05'),
    ('Zahid Ahmed', 'Management', 'Admin', '2024-05-05'),
    ('Zahid Ahmed', 'Admin', 'IT', '2024-05-05'),
    ('Ravinder', 'Sales', 'Management', '2024-05-06'),
    ('Ravinder', 'Management', 'Admin', '2024-05-06'),
    ('Ravinder', 'Admin', 'HR', '2024-05-06'),
    ('Pawan', 'Admin', 'Operation', '2024-05-07'),
    ('Pawan', 'Operation', 'Marketing', '2024-05-07'),
    ('Pawan', 'Marketing', 'Finance', '2024-05-07'),
    ('Anup Thakur', 'Sales', 'Marketing', '2024-05-08'),
    ('Anup Thakur', 'Marketing', 'Sales', '2024-05-08'),
    ('Anup Thakur', 'Sales', 'Admin', '2024-05-08')
) AS source (FullName, FromDepartment, ToDepartment, JoiningDate)
ON (target.FullName = source.FullName AND target.FromDepartment = source.FromDepartment AND
    target.ToDepartment = source.ToDepartment AND target.JoiningDate = source.JoiningDate)
WHEN NOT MATCHED THEN
    INSERT (FullName, FromDepartment, ToDepartment, JoiningDate)
    VALUES (source.FullName, source.FromDepartment, source.ToDepartment, source.JoiningDate);