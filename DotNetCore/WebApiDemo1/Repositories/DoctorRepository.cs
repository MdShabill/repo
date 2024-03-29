﻿using Microsoft.Data.SqlClient;
using System.Data;
using WebApiDemo1.DataModel;
using WebApiDemo1.DTO.InputDTO;
using WebApiDemo1.Enums;

namespace WebApiDemo1.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly string _connectionString;

        public DoctorRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Doctor> GetAllDoctors()
        {
            List<Doctor> doctors = new();

            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Doctors", sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Doctor doctor = new();
                    doctor.Id = (int)dataTable.Rows[i]["Id"];
                    doctor.FirstName = (string)dataTable.Rows[i]["FirstName"];
                    doctor.MiddleName = (string)dataTable.Rows[i]["MiddleName"];
                    doctor.LastName = (string)dataTable.Rows[i]["LastName"];
                    doctor.Gender = (GenderTypes)dataTable.Rows[i]["Gender"];
                    doctor.Email = (string)dataTable.Rows[i]["Email"];
                    doctor.RegistrationNumber = (int)dataTable.Rows[i]["RegistrationNumber"];
                    doctor.Department = (string)dataTable.Rows[i]["Department"];
                    doctor.City = (string)dataTable.Rows[i]["City"];

                    doctors.Add(doctor);
                }
                return doctors;
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

        public Doctor GetDoctorDetailById(int doctorId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Doctors WHERE Id = @doctorId", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@doctorId", doctorId);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    Doctor doctor = new();
                    doctor.Id = (int)dataTable.Rows[0]["Id"];
                    doctor.FirstName = (string)dataTable.Rows[0]["FirstName"];
                    doctor.MiddleName = (string)dataTable.Rows[0]["MiddleName"];
                    doctor.LastName = (string)dataTable.Rows[0]["LastName"];
                    doctor.Gender = (GenderTypes)dataTable.Rows[0]["Gender"];
                    doctor.Email = (string)dataTable.Rows[0]["Email"];
                    doctor.RegistrationNumber = (int)dataTable.Rows[0]["RegistrationNumber"];
                    doctor.Department = (string)dataTable.Rows[0]["Department"];
                    doctor.City = (string)dataTable.Rows[0]["City"];
                    return doctor;
                }
                else
                    return null;
            }
        }

        public List<DoctorDto> GetDoctorsByDepartmentByDoctorName(string department, string doctorName)
        {
            List<DoctorDto> doctors = new();

            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new(@"SELECT * FROM Doctors WHERE Department = @department
                               AND FullName = @doctorName ", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@department", department);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@doctorName", doctorName);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DoctorDto doctorDto = new();
                    doctorDto.Id = (int)dataTable.Rows[i]["Id"];
                    doctorDto.FullName = (string)dataTable.Rows[i]["FullName"];
                    doctorDto.Gender = (GenderTypes)dataTable.Rows[i]["Gender"];
                    doctorDto.Email = (string)dataTable.Rows[i]["Email"];
                    doctorDto.RegistrationNumber = (int)dataTable.Rows[i]["RegistrationNumber"];
                    doctorDto.Department = (string)dataTable.Rows[i]["Department"];
                    doctorDto.City = (string)dataTable.Rows[i]["City"];

                    doctors.Add(doctorDto);
                }
                return doctors;
            }
        }

        public List<DoctorDto> GetDoctorsNameListByDepartment(string department)
        {
            List<DoctorDto> doctors = new();

            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Doctors WHERE Department = @department", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@department", department);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DoctorDto doctorDto = new();
                    doctorDto.Id = (int)dataTable.Rows[i]["Id"];
                    doctorDto.FullName = (string)dataTable.Rows[i]["FullName"];
                    doctorDto.Gender = (GenderTypes)dataTable.Rows[i]["Gender"];
                    doctorDto.Email = (string)dataTable.Rows[i]["Email"];
                    doctorDto.RegistrationNumber = (int)dataTable.Rows[i]["RegistrationNumber"];
                    doctorDto.Department = (string)dataTable.Rows[i]["Department"];
                    doctorDto.City = (string)dataTable.Rows[i]["City"];

                    doctors.Add(doctorDto);
                }
                return doctors;
            }
        }

        public int Add(DoctorDto doctor)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"INSERT INTO Doctors(FullName, Gender, Email, RegistrationNumber, Department, City)
                           VALUES (@FullName, @Gender, @Email, @RegistrationNumber, @Department, @City)
                           Select Scope_Identity() ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@FullName", doctor.FullName);
                sqlCommand.Parameters.AddWithValue("@Gender", doctor.Gender);
                sqlCommand.Parameters.AddWithValue("@Email", doctor.Email);
                sqlCommand.Parameters.AddWithValue("@RegistrationNumber", doctor.RegistrationNumber);
                sqlCommand.Parameters.AddWithValue("@Department", doctor.Department);
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
                string sqlQuery = @" UPDATE Doctors SET FullName = @FullName, Gender = @Gender, Email = @Email,
                            RegistrationNumber = @RegistrationNumber, Department = @Department, City = @City
                            WHERE Id = @Id ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", doctor.Id); 
                sqlCommand.Parameters.AddWithValue("@FullName", doctor.FullName);
                sqlCommand.Parameters.AddWithValue("@Gender", doctor.Gender);
                sqlCommand.Parameters.AddWithValue("@Email", doctor.Email);
                sqlCommand.Parameters.AddWithValue("@RegistrationNumber", doctor.RegistrationNumber);
                sqlCommand.Parameters.AddWithValue("@Department", doctor.Department);
                sqlCommand.Parameters.AddWithValue("@City", doctor.City);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
