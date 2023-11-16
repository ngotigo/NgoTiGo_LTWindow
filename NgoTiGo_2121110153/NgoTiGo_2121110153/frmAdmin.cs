using NgoTiGo_2121110153.DAO;
using NgoTiGo_2121110153.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace NgoTiGo_2121110153
{
    public partial class frmAdmin : Form
    {
        BindingSource foodlist = new BindingSource();
      
        BindingSource accountList = new BindingSource();
        public frmAdmin()
        {
            InitializeComponent();
            Loadd();
           
        }

     

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void tpDanhMuc_Click(object sender, EventArgs e)
        {

        }

        private void dgvAccount_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void frmAdmin_Load(object sender, EventArgs e)
        {

        }
        private void dgvBill_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        #region methods

        List<Food> SeachFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);
            return listFood;
        }

        void Loadd()
        {
            dgvFood.DataSource = foodlist;
            dgvAccount.DataSource = accountList;
         

            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFormDate.Value,dtpkToDate.Value);
            LoadListFood();
           
            LoadAccount();
            AddFoodBinding();
            LoadCategoryIntoCombobox(cbDanhMucFood);
            AddAccountBinDing();
         
        }
        void AddAccountBinDing()
        {
            txtTenTK.DataBindings.Add(new Binding("Text", dgvAccount.DataSource,"UserName",true,DataSourceUpdateMode.Never));
            txtTenHienThi.DataBindings.Add(new Binding("Text", dgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            nmLoaiTK.DataBindings.Add(new Binding("Value", dgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));

        }





        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }

        void AddAccount(string userName, string displayName, int type)
        {
            if(AccountDAO.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại");
            }

            LoadAccount();
        }

        void EditAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật tài khoản thất bại");
            }

            LoadAccount();
        }

        void DeleteAccount(string userName)
        {
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Xóa nhật tài khoản thất bại");
            }

            LoadAccount();
        }

        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFormDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFormDate.Value.AddMonths(1).AddDays(-1);
        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
          dgvBill.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
        }

        void LoadListFood()
        {
            foodlist.DataSource = FoodDAO.Instance.GetListFood();
        }

 

        void AddFoodBinding()
        {
            txtTenFood.DataBindings.Add(new Binding("Text",dgvFood.DataSource,"Name",true,DataSourceUpdateMode.Never));
            txtFoodID.DataBindings.Add(new Binding("Text", dgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmPrice.DataBindings.Add(new Binding("Value", dgvFood.DataSource, "price", true, DataSourceUpdateMode.Never));
        }

        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }

        #endregion

        #region events

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFormDate.Value, dtpkToDate.Value);
        }
         private void btnXemFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

        private void btnXemTK_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        private void btnThemTK_Click(object sender, EventArgs e)
        {
            string userName = txtTenTK.Text;
            string displayName = txtTenHienThi.Text;
            int type = (int)nmLoaiTK.Value;

            AddAccount(userName, displayName, type);

        }

        private void btnXoaTK_Click(object sender, EventArgs e)
        {
            string userName = txtTenTK.Text;
       

            DeleteAccount(userName);
        }

        private void btnSuaTK_Click(object sender, EventArgs e)
        {
            string userName = txtTenTK.Text;
            string displayName = txtTenHienThi.Text;
            int type = (int)nmLoaiTK.Value;

            EditAccount(userName, displayName, type);
        }

        #endregion

        private void txtFoodID_TextChanged(object sender, EventArgs e)
        {
           
            {
                if (dgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;

                    Category category = CategoryDAO.Instance.GetCategoryByID(id);

                    cbDanhMucFood.SelectedItem = category;

                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbDanhMucFood.Items)
                    {
                        if (item.ID == category.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbDanhMucFood.SelectedIndex = index;
                }
            }
            
            
            
        }

        private void cbDanhMucFood_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnThemFood_Click(object sender, EventArgs e)
        {
            string name = txtTenFood.Text;
            int categoryID = (cbDanhMucFood.SelectedItem as  Category).ID;
            float price = (float)nmPrice.Value;

            if(FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công");
                LoadListFood();
                if(insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("có lỗi khi thêm thức ăn");
            }
        }

        private void btnSuaFood_Click(object sender, EventArgs e)
        {
            string name = txtTenFood.Text;
            int categoryID = (cbDanhMucFood.SelectedItem as Category).ID;
            float price = (float)nmPrice.Value;
            int id = Convert.ToInt32(txtFoodID.Text);

            if (FoodDAO.Instance.UpdateFood(id,name, categoryID, price))
            {
                MessageBox.Show("Sửa món thành công");
                LoadListFood();
                if(updateFood != null)
                    updateFood(this,new EventArgs());
            }
            else
            {
                MessageBox.Show("có lỗi khi sửa thức ăn");
            }
        }

        private void btnXoaFood_Click(object sender, EventArgs e)
        {
         
            int id = Convert.ToInt32(txtFoodID.Text);

            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa món thành công");
                LoadListFood();
                if(deleteFood != null) 
                    deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("có lỗi khi xóa thức ăn");
            }
        }

        private void btnTimFood_Click(object sender, EventArgs e)
        {
           foodlist.DataSource = SeachFoodByName(txtTimFood.Text);
        }

        private void nmLoaiTK_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnThemDanhMuc_Click(object sender, EventArgs e)
        {
            string name = txtTenDanhMuc.Text;

            AddCategory(name);
        }

        private void btnXoaDanhMuc_Click(object sender, EventArgs e)
        {

        }

        private void btnSuaDanhMuc_Click(object sender, EventArgs e)
        {

        }

        private void btnXemDanhMuc_Click(object sender, EventArgs e)
        {
           
        }

        void AddCategory(string name)
        {
            if (CategoryDAO.Instance.InsertCategory(name))
            {
                MessageBox.Show("Thêm tài danh mục thành công");
            }
            else
            {
                MessageBox.Show("Thêm danh mục thất bại");
            }

            LoadAccount();
        }

        void EditCategory(string name)
        {
            if (CategoryDAO.Instance.InsertCategory(name))
            {
                MessageBox.Show("Thêm tài danh mục thành công");
            }
            else
            {
                MessageBox.Show("Thêm danh mục thất bại");
            }

            LoadAccount();
        }

        void UpdateCategory(string name)
        {
            if (CategoryDAO.Instance.InsertCategory(name))
            {
                MessageBox.Show("Thêm tài danh mục thành công");
            }
            else
            {
                MessageBox.Show("Thêm danh mục thất bại");
            }

            LoadAccount();
        }
    }
}
