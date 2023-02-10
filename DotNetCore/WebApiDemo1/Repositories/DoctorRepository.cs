using Microsoft.Data.SqlClient;
using System.Data;
using WebApiDemo1.DTO.InputDTO;
using WebApplication1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly string _connectionString;

        public DoctorRepository(string connectionString)
        {
            _connectionString = connectionString;
        }  

        public DataTable GetAllDoctors()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("Select * From Doctors", sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);
                DataTable copyDataTable = dataTable.Copy();
                sqlDataAdapter.Fill(copyDataTable);
                return dataTable;
            }
        }

        public int GetDoctorsCount()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "Select Count(*) From Doctors";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlConnection.Open();
                int doctorcount = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
                return doctorcount;
            }
        }

        public string GetDoctorDepartmentById(int doctorId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "Select Department From Doctors where id = @doctorId";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@doctorId", doctorId);
                sqlConnection.Open();
                string doctorDepartment = Convert.ToString(sqlCommand.ExecuteScalar());
                string subStringDoctorDepartment = doctorDepartment.Substring(4);
                sqlConnection.Close();
                return subStringDoctorDepartment;
            }
        }

        public DataTable GetDoctorsDetailByFullNameByDepartment(string fullName, string department)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new(@"Select * From Doctors Where FullName = @fullName
                        And Department = @department", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@fullName", fullName);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@department", department);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        public DataTable GetDoctorsDetailByDepartmentByCity(string department, string? city)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "Select * From Doctors Where Department = @department ";

                if (!string.IsNullOrWhiteSpace(city))
                    sqlQuery += "And City = @city ";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@department", department);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@city", city);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        public int Add(DoctorDto doctor)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"INSERT INTO Doctors(RegistrationNumber, FullName, Email, Department, Gender, City)
                           VALUES (@RegistrationNumber, @FullName, @Email, @Department, @Gender, @City)
                           Select Scope_Identity() ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@RegistrationNumber", doctor.RegistrationNumber);
                sqlCommand.Parameters.AddWithValue("@FullName", doctor.FullName);
                sqlCommand.Parameters.AddWithValue("@Email", doctor.Email);
                sqlCommand.Parameters.AddWithValue("@Department", doctor.Department);
                sqlCommand.Parameters.AddWithValue("@Gender", doctor.Gender);
                sqlCommand.Parameters.AddWithValue("@City", doctor.City);
                sqlConnection.Open();
                doctor.Id = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
                return doctor.Id;
            }
        }

        public void Update(DoctorDto doctor)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @" UPDATE Doctors SET RegistrationNumber = @RegistrationNumber, FullName = @FullName, 
                            Email = @Email, Department = @Department, Gender = @Gender, City = @City
                            WHERE Id = @Id ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", doctor.Id);
                sqlCommand.Parameters.AddWithValue("@RegistrationNumber", doctor.RegistrationNumber);
                sqlCommand.Parameters.AddWithValue("@FullName", doctor.FullName);
                sqlCommand.Parameters.AddWithValue("@Email", doctor.Email);
                sqlCommand.Parameters.AddWithValue("@Department", doctor.Department);
                sqlCommand.Parameters.AddWithValue("@Gender", doctor.Gender);
                sqlCommand.Parameters.AddWithValue("@City", doctor.City);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
