using DoubleCrudWithValidation.Interfaces;
using DoubleCrudWithValidation.Structs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleCrudWithValidation.Cruds
{
    internal class MysqlCrud : IDbCrud
    {
        private readonly MySqlConnection _connection;

        public MysqlCrud(string connection)
        {
            _connection = new MySqlConnection(connection);
        }

        public void Create<T>(T toCreate) where T : struct
        {
            if (typeof(T) == typeof(Product))
            {
                Product newProduct = (Product)(object)toCreate;
                string query = $"insert into shop.products (ProductName, Description, Price, NumberInStock) values('{newProduct.Name}','{newProduct.Description}','{newProduct.Price}','{newProduct.NumberInStock}');";

                ExecuteQueryNoReturn(query);
            }
            else
            {
                throw new InvalidOperationException("Invalid type");
            }
        }

        public void Delete(int id)
        {
            string query = $"delete from shop.products where ProductId = {id};";
            ExecuteQueryNoReturn(query);
        }

        public List<T> Read<T>(int id)
        {
            string query;
            if (id < 0)
            {
                query = $"select * from shop.products;";
            }
            else
            {
                query = $"select * from shop.products where ProductId={id};";
            }
            List<Product> products = new List<Product>();

            MySqlCommand command = new MySqlCommand(query, _connection);
            _connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Product product = new Product();
                product.ProductId = (int)reader[0];
                product.Name = reader[1].ToString();
                product.Description = reader[2].ToString();
                product.Price = (double)reader[3];
                product.NumberInStock = (int)reader[4];
                products.Add(product);
            }
            _connection.Close();
            return products as List<T>;
        }

        public void Update<T>(T toUpdate) where T : struct
        {
            if (typeof(T) == typeof(Product))
            {
                Product updatedProduct = (Product)(object)toUpdate;
                string query = $"update shop.products set ProductName='{updatedProduct.Name}',Description='{updatedProduct.Description}',Price='{updatedProduct.Price}',NumberInStock='{updatedProduct.NumberInStock}' where ProductId = {updatedProduct.ProductId};";
                ExecuteQueryNoReturn(query);
            }
            else
            {
                throw new InvalidOperationException("Invalid type");
            }
        }

        private void ExecuteQueryNoReturn(string query)
        {
            MySqlCommand command = new MySqlCommand(query, _connection);
            _connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) { }
            _connection.Close();
        }
    }
}
