using DoubleCrudWithValidation.Interfaces;
using DoubleCrudWithValidation.Models;
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

        public void Create<T>(T toCreate) where T : class
        {
            if (toCreate is Product newProduct)
            {
                string query = $"insert into shop.products (ProductName, Description, Price, NumberInStock) values('{newProduct.Name}','{newProduct.Description}','{newProduct.Price}','{newProduct.NumberInStock}');";

                ExecuteQueryNoReturn(query);
            }
            else
            {
                throw new InvalidOperationException("Invalid type");
            }
        }

        public void Delete(string idString)
        {
            int id = int.TryParse(idString, out id) ? id : -1;
            if (id > 0)
            {
                string query = $"delete from shop.products where ProductId = {id};";
                ExecuteQueryNoReturn(query);
            }
        }

        public List<T> Read<T>(string idString)
        {
            int id = int.TryParse(idString, out id) ? id : -1;
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
            try
            {
                _connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product((int)reader[0], reader[1].ToString(), reader[2].ToString(), (decimal)reader[3], (int)reader[4]);
                    products.Add(product);
                }
                _connection.Close();
            }
            catch (MySqlException e)
            {
                throw new Exception("An SQL exception occured.",e);
            }
            return products as List<T>;
        }

        public void Update<T>(T toUpdate) where T : class
        {
            if (toUpdate is Product updatedProduct)
            {
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
