using ShopEase.Enums;
using System.Data.SqlClient;
using System.Data;
using ShopEase.DataModels.Restaurant;

namespace ShopEase.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly string _connectionString;

        public RestaurantRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Restaurant> GetAll()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Select Id,
                           RestaurantName, MobileNumber,  
                           Location, Rating
                           From 
                           Restaurants";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Restaurant> restaurants = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Restaurant restaurant = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        RestaurantName = (string)dataTable.Rows[i]["RestaurantName"],
                        MobileNumber = (string)dataTable.Rows[i]["MobileNumber"],
                        Location = (string)dataTable.Rows[i]["Location"],
                        Rating = (decimal)dataTable.Rows[i]["Rating"],
                    };
                    restaurants.Add(restaurant);
                }
                return restaurants;
            }
        }

        public Restaurant GetRestaurantById(int id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"SELECT Id, RestaurantName,
                         MobileNumber, Location, Rating
                         FROM Restaurants  
                         Where Id = @id ";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@id", id);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                Restaurant restaurant = new()
                {
                    Id = (int)dataTable.Rows[0]["Id"],
                    RestaurantName = (string)dataTable.Rows[0]["RestaurantName"],
                    MobileNumber = (string)dataTable.Rows[0]["MobileNumber"],
                    Location = (string)dataTable.Rows[0]["Location"],
                    Rating = (decimal)dataTable.Rows[0]["Rating"],
                };
                return restaurant;
            }
        }

        public int Register(Restaurant restaurant)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Insert Into Restaurants
                       (RestaurantName, MobileNumber, Location, Rating)
                       Values
                       (@restaurantName, @mobileNumber, @location, @rating) ";

                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@restaurantName", restaurant.RestaurantName);
                sqlCommand.Parameters.AddWithValue("@mobileNumber", restaurant.MobileNumber);
                sqlCommand.Parameters.AddWithValue("@location", restaurant.Location);
                sqlCommand.Parameters.AddWithValue("@rating", restaurant.Rating);

                sqlConnection.Open();
                int affectedRowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();

                return affectedRowCount;
            }
        }

        public int Update(Restaurant restaurant)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"UPDATE Restaurants SET 
                            RestaurantName = @restaurantName,
                            MobileNumber = @mobileNumber,
                            Location = @location,
                            Rating = @rating
                            WHERE Id = @id";

                using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@id", restaurant.Id);
                    sqlCommand.Parameters.AddWithValue("@restaurantName", restaurant.RestaurantName);
                    sqlCommand.Parameters.AddWithValue("@mobileNumber", restaurant.MobileNumber);
                    sqlCommand.Parameters.AddWithValue("@location", restaurant.Location);
                    sqlCommand.Parameters.AddWithValue("@rating", restaurant.Rating);

                    sqlConnection.Open();
                    int affectedRowCount = sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();

                    return affectedRowCount;
                }
            }
        }

        public int Delete(int id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string deleteQuery = @"Delete 
                                       From Restaurants 
                                       Where Id = @Id";
                SqlCommand sqlCommand = new(deleteQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", id);

                sqlConnection.Open();
                int affectedRowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();

                return affectedRowCount;
            }
        }
    }
}
