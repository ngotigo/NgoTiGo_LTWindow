using NgoTiGo_2121110153.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgoTiGo_2121110153.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance 
        {
            get { if (instance == null) instance = new BillDAO(); return BillDAO.instance; } 
             private set { BillDAO.instance = value; }
        
        }
        private BillDAO() { }

        /// <summary>
        /// Thành công : bill ID
        /// Thất bại :-1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public int GetUncheckBillIDByTableID(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from dbo.Bill where idTable = "+id+" and status =0");

            if(data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.ID;
            }
            return -1;
        }

        public void CheckOut(int id, int discount, float totalPrice)
        {
            string query = "update dbo.Bill set dateCheckOut = GETDATE(), status = 1 ,"+" discount ="+ discount +", totalPrice = "+totalPrice+"  where id = " + id;
            DataProvider.Instance.ExecuteNonQuery(query);
        }

        public void InsertBill(int id)
        {
            DataProvider.Instance.ExecuteQuery("exec USP_InsertBill @idTable ",new object[] {id});
        }

        public DataTable GetBillListByDate(DateTime checkIn, DateTime checkOut)
        {
            return DataProvider.Instance.ExecuteQuery("exec USP_GetListBillByDate @checkIn , @checkOut",new object[] {checkIn,checkOut});
        }

        public int GetMaxIDBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar("select MAX(id) from dbo.Bill");
            }
            catch
            {
                return 1;
            }
           
        }
    }
}
