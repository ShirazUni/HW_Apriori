using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace apriori {
    class DatabaseObjectMapper{
        IDbConnection dbConnection;

        public DatabaseObjectMapper(){
            dbConnection = new SqlConnection("Data Source=ALPHA-PC\\SQLDEV;Database= OnlineStoreDatabase;Trusted_Connection=true ");
        }

        public List<User> getAllUsers(){
            List<User> users =new List<User>();
            dbConnection.Open();
            var res = dbConnection.Query<User>("SELECT * from dbo.[User]");
            foreach (var item in res) {
                users.Add(item);
            }
            dbConnection.Close();
            return users;
        }

        public List<Product> GetProducts(){
            List<Product> products = new List<Product>();
            dbConnection.Open();
            var res = dbConnection.Query<Product>("SELECT * from dbo.[Product]");
            foreach (var item in res){
                products.Add(item);
            }
            dbConnection.Close();
            return products;

        }

        public List<Order> GetOrders(){
            List<Order> orders = new List<Order>();
            dbConnection.Open();
            var res = dbConnection.Query<Order>("SELECT * from dbo.[Order]");
            foreach (var item in res){
                orders.Add(item);
            }
            dbConnection.Close();
            return orders;

        }

        public List<OrderItem> GetOrderItems(){
            List<OrderItem> orderItem = new List<OrderItem>();
            dbConnection.Open();
            var res = dbConnection.Query<OrderItem>("SELECT * from dbo.[order_item]");
            foreach (var item in res){
                orderItem.Add(item);
            }
            dbConnection.Close();
            return orderItem;
        }

        public List<List<string>> getListOfOrderProducts(){
            dbConnection.Open();
            List<List<string>> data = new List<List<string>>();
            var procedure = "[selectAllOrderItemsOfOrders]";
            var res = dbConnection.Query<AprioriData>(procedure).ToList();
            
            foreach (var item in res){
                List<string> titles = new List<string>();
                string[] words;
                if (item.products != null) {
                    words= item.products.Split(',');
                    foreach (string p in words){
                        if (!p.Equals("")) {
                            titles.Add(p);
                        }
                        
                    }
                    data.Add(titles);
                }
                
            }
            return data;
        }
    }
}
