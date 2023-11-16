using NgoTiGo_2121110153.DAO;
using NgoTiGo_2121110153.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NgoTiGo_2121110153
{
    public partial class frmDangNhap : Form
    {
        public frmDangNhap()
        {
            InitializeComponent();
        }

      


        private void btnThoat_Click(object sender, EventArgs e)
        {
            
                Application.Exit();
           
        }

        private void frmDangNhap_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show("Bạn có muốn thoát chương trình?","Thông báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string useName = txtTaiKhoan.Text;
            string passWord = txtMatKhau.Text;
            if (Login(useName,passWord))
            {
                Account loginAccount = AccountDAO.Instance.GetAccountByUserName(useName);
                frmQuanLy f = new frmQuanLy(loginAccount);
                this.Hide();
                f.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Sai tên tài khoản hoặc mật khẩu!");
            }

           
        }

        bool Login(string userName, string passWord)
        {
            return AccountDAO.Instance.Login(userName, passWord);
        }

        private void frmDangNhap_Load(object sender, EventArgs e)
        {

        }
    }
}
