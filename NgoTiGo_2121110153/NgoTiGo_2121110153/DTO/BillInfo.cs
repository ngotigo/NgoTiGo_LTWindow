using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgoTiGo_2121110153.DTO
{
    public class BillInfo
    {
        public BillInfo(int id, int BillID, int foodID, int count)
        {
            this.ID = id;
            this.BillID = BillID;
            this.FoodID = foodID;
            this.Count = count;
        }

        public BillInfo(DataRow row)
        {
            this.ID = (int)row["id"];
            this.BillID = (int)row["idBill"];
            this.FoodID = (int)row["idFood"];
            this.Count = (int)row["count"];
        }

        private int count;

        private int foodID;

        private int billID;

        private int iD;

        public int ID { get => iD; set => iD = value; }
        public int BillID { get => billID; set => billID = value; }
        public int FoodID { get => foodID; set => foodID = value; }
        public int Count { get => count; set => count = value; }
    }
}
