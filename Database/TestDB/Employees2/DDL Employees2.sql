MERGE INTO Employees2 AS target
USING (
    VALUES 
        ('Shabill Irfani', 'IT', '2024-05-04'),
        ('Shabill Irfani', 'Management', '2024-05-04'),
        ('Shabill Irfani', 'IT', '2024-05-04'),
        ('Shabill Irfani', 'Admin', '2024-05-04'),
        ('Shabill Irfani', 'HR', '2024-05-04'),
        ('Shabill Irfani', 'Management', '2024-05-04'),
        ('Shabill Irfani', 'Finance', '2024-05-04'),
        ('Salman', 'Admin', '2024-05-04'),
        ('Salman', 'HR', '2024-05-04'),
        ('Salman', 'Marketing', '2024-05-04'),
        ('Zahid Ahmed', 'HR', '2024-05-05'),
        ('Zahid Ahmed', 'Management', '2024-05-05'),
        ('Zahid Ahmed', 'Admin', '2024-05-05'),
        ('Zahid Ahmed', 'IT', '2024-05-05'),
        ('Salman', 'Admin', '2024-05-06'),
        ('Salman', 'IT', '2024-05-06'),
        ('Salman', 'Operation', '2024-05-06'),
        ('Hemant Kumar', 'Office Boy', '2024-05-06'),
        ('Hemant Kumar', 'Admin', '2024-05-06'),
        ('Hemant Kumar', 'HR', '2024-05-06'),
        ('Hemant Kumar', 'Admin', '2024-05-06'),
        ('Pawan Sharma', 'IT', '2024-05-07'),
        ('Pawan Sharma', 'Operation', '2024-05-07'),
        ('Pawan Sharma', 'Admin', '2024-05-07'),
        ('Pawan Sharma', 'IT', '2024-05-07'),
        ('Shabill Irfani', 'Finance', '2024-05-07'),
        ('Shabill Irfani', 'HR', '2024-05-07'),
        ('Shabill Irfani', 'Finance', '2024-05-07'),
        ('Ravinder', 'Sales', '2024-05-07'),
        ('Ravinder', 'Finance', '2024-05-07'),
        ('Ravinder', 'Marketing', '2024-05-07'),
        ('Anup Thakur', 'Sales', '2024-05-08'),
        ('Anup Thakur', 'Marketing', '2024-05-08'),
        ('Anup Thakur', 'Sales', '2024-05-08'),
        ('Ravinder Varma', 'Admin', '2024-05-09'),
        ('Ravinder Varma', 'HR', '2024-05-09'),
        ('Ravinder Varma', 'Admin', '2024-05-09')
) AS source (FullName, Department, JoiningDate)
ON target.FullName = source.FullName AND target.Department = source.Department AND target.JoiningDate = source.JoiningDate
WHEN NOT MATCHED THEN
    INSERT (FullName, Department, JoiningDate)
    VALUES (source.FullName, source.Department, source.JoiningDate);
