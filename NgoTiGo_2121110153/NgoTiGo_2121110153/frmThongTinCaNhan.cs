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
    public partial class frmThongTinCaNhan : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount); }
        }
        public frmThongTinCaNhan(Account acc)
        {
            InitializeComponent();

            LoginAccount = acc;
        }

        void ChangeAccount(Account acc)
        {
            txtTaiKhoan.Text = LoginAccount.UserName;
            txtTenHienThi.Text = LoginAccount.DisplayName;

        }

        void UpdateAccountInfo()
        {
            string displayName = txtTenHienThi.Text;
            string password = txtMatKhau.Text;
            string newpass = txtMatKhauMoi.Text;
            string reenterPass = txtNhapLaiMK.Text;
            string userName = txtTaiKhoan.Text;

            if(!newpass.Equals(reenterPass))
            {
                MessageBox.Show("Mật khẩu mới và mật khẩu cũ không trùng nhau!");

            }
            else
            {
                if(AccountDAO.Instance.UpdateAccount(userName,displayName,password,newpass))
                {
                    MessageBox.Show("Cập nhật thành công");
                    if (updateAccount != null)
                        updateAccount(this, new AccountEvent(AccountDAO.Instance.GetAccountByUserName(userName)));
                }
                else
                {
                    MessageBox.Show("Sai mật khẩu");
                }
            }
        }

        private event EventHandler<AccountEvent> updateAccount;
        public event EventHandler<AccountEvent> UpdateAccount
        {
            add { updateAccount += value; }
            remove { updateAccount -= value;}
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmThongTinCaNhan_Load(object sender, EventArgs e)
        {

        }

        private void btnThayDoiTK_Click(object sender, EventArgs e)
        {
            UpdateAccountInfo();
        }
    }

    public class AccountEvent : EventArgs
    {
        private Account acc;
        public AccountEvent(Account acc)
        {
            this.Acc = acc;
        }

        public Account Acc { get => acc; set => acc = value; }
    }
}
