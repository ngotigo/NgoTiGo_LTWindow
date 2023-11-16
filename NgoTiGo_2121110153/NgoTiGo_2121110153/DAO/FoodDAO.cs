using NgoTiGo_2121110153.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgoTiGo_2121110153.DAO
{
    public class FoodDAO
    {
        private static FoodDAO instance;

        public static FoodDAO Instance
        {
            get { if (instance == null) instance = new FoodDAO(); return FoodDAO.instance; }
            private set { FoodDAO.instance = value; }
        }

        private FoodDAO()
        {

        }

        public List<Food> GetFoodByCategoryID(int id)
        {
            List<Food> list = new List<Food>();

            string query = "select * from Food where idCategory = "+id;

            DataTable data=DataProvider.Instance.ExecuteQuery(query);

            foreach(DataRow item in data.Rows) 
            {
                Food food = new Food(item);
                list.Add(food);
            }

            return list;
        }

        public List<Food> GetListFood()
        {
            List<Food> list = new List<Food>();

            string query = "select * from food";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food);
            }

            return list;
        }

        public List<Food> SearchFoodByName(string name)
        {
            List<Food> list = new List<Food>();

            string query =string.Format( "select * from food where name  like N'%{0}%'",name);

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food);
            }

            return list;
        }

        public bool InsertFood(string name , int id , float price)
        {
            string query =string.Format( "Insert dbo.Food (name, idCategory, price) values (N'{0}' , {1} , {2})",name , id , price);
            int result =  DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateFood(int idFood, string name, int id, float price)
        {
            string query = string.Format("UPDATE dbo.Food set name = N'{0}', idCategory = {1} , price = {2} where id ={3} ", name, id, price, idFood);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteFood(int idFood)
        {
            BillInfoDAO.Instance.DeleteBillInfoByFoodID(idFood);
            string query = string.Format("Delete Food where id = {0} ",idFood );
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }


        
    }
}
