using NgoTiGo_2121110153.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgoTiGo_2121110153.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance 
        {
            get { if( instance == null) instance = new AccountDAO(); return instance; }
            private set { instance = value; }
        
        }

        private AccountDAO()
        {

        }

        public bool Login(string username, string password)
        {
            string query = "USP_Login @userName , @passWord";

            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] {username,password});
            return result.Rows.Count >0;
        }
        
        public bool UpdateAccount(string userName, string displayName, string pass, string newPass)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("exec USP_UpdateAccount @userName , @displayName , @password  , @newPassword ",new object[]{ userName, displayName, pass, newPass});
            return result > 0;
        }

        public DataTable GetListAccount()
        {
            return DataProvider.Instance.ExecuteQuery("select UserName,DisplayName,Type from account");
        }

        public Account GetAccountByUserName(string userName) 
        {
           DataTable data = DataProvider.Instance.ExecuteQuery("select * from account where userName = '"+ userName +"'");
        
            foreach(DataRow item in data.Rows) {
                return new Account(item);
            }
            return null;
        }

        public bool InsertAccount(string name, string displayname , int type)
        {
            string query = string.Format("Insert dbo.Account (UserName, DisplayName, Type) values (N'{0}' , N'{1}' , {2})", name, displayname, type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateAccount( string name, string displayname, int type)
        {
            string query = string.Format("UPDATE dbo.Account set DisplayName = N'{1}' , Type = {2} where UserName =N'{0}' ", name, displayname, type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteAccount(string name)
        {
            
            string query = string.Format("Delete Account where UserName = N'{0}' ", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
