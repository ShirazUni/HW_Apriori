using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace apriori
{
    class Program
    {
        static void Main(string[] args)
        {
            
            DatabaseObjectMapper dbMapper = new DatabaseObjectMapper();
            //sample usage for data base objects
            /*
            List<User> users= dbMapper.getAllUsers();
            List<Order> orders = dbMapper.GetOrders();
            List<OrderItem> orderItems = dbMapper.GetOrderItems();
            List<List<string>> products = dbMapper.getListOfOrderProducts();
            foreach (var item in products) {
                Console.WriteLine(item.Count().ToString());
            }
            */

            //sample data to test the code before testing with real database data
            /*
            List<string> l = new List<string> { "apple", "beer", "rice", "chicken"};
            List<string> l2 = new List<string> { "apple", "beer", "rice" };
            List<string> l3 = new List<string> { "apple", "beer" };
            List<string> l6 = new List<string> { "apple", "mango" };
            List<string> l4 = new List<string> { "apple", "beer", "rice", "chicken" };
            List<string> l5 = new List<string> { "milk", "beer" };
            List<string> l7 = new List<string> { "milk", "mango" };
            List<string> l8 = new List<string> { "apple", "beer", "rice" };
            List<List<string>> data = new List<List<string>>();
            data.Add(l);
            data.Add(l2);
            data.Add(l3);
            data.Add(l4);
            data.Add(l5);
            data.Add(l6);
            data.Add(l7);
            data.Add(l8);
            Apriori apriori = new Apriori(data);*/
            
            //real test
            Apriori apriori = new Apriori(dbMapper.getListOfOrderProducts());

            Console.ReadKey();
        }
    }
}
