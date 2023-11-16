using NgoTiGo_2121110153.DAO;
using NgoTiGo_2121110153.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Menu = NgoTiGo_2121110153.DTO.Menu;
using System.Threading;

namespace NgoTiGo_2121110153
{
    public partial class frmQuanLy : Form
    {
        private Account loginAccount;

        public Account LoginAccount {
            get { return loginAccount; } 
            set {  loginAccount = value;ChangeAccount(loginAccount.Type); }
        }

        public frmQuanLy(Account acc)
        {
            InitializeComponent();

            this.LoginAccount = acc;

            LoadTable();
            LoadCategory();
            LoadComboboxTable(cbChuyenBan);
        }


        #region Method

        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            thôngTinTàiKhoảnToolStripMenuItem.Text += " (" + LoginAccount.DisplayName + ")";
        }

        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }

        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }

        void LoadTable()
        {
            flpTable.Controls.Clear();
          List<Table> tableList = TableDAO.Instance.LoadTableList();
            foreach (Table item in tableList) 
            {
                Button btn = new Button() {Width = TableDAO.TableHeight , Height = TableDAO.TableWidth };
                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Click += btn_Click;
                btn.Tag = item;

                switch (item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.White; break;

                    default:
                        btn.BackColor = Color.LightSeaGreen; break;
                }

                flpTable.Controls.Add(btn);
            }
        }

        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<Menu> listBillInfo = MenuDAO.Instance.GetListMenuTable(id);
            float totalPrice = 0;
            foreach(Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                lsvBill.Items.Add(lsvItem);
            }
            CultureInfo culture = new CultureInfo("vi-VN");
            Thread.CurrentThread.CurrentCulture = culture;
            txtTotalPrice.Text = totalPrice.ToString("c",culture);

            
        }

        void LoadComboboxTable(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }

        #endregion


        #region Events

        void btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
        }
        private void cbFood_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmThongTinCaNhan frmTT = new frmThongTinCaNhan(LoginAccount);
            frmTT.UpdateAccount += FrmTT_UpdateAccount;
            frmTT.ShowDialog();
        }

        private void FrmTT_UpdateAccount(object sender, AccountEvent e)
        {
            thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản (" + e.Acc.DisplayName + ")";
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdmin frmAdmin = new frmAdmin();
            frmAdmin.InsertFood += FrmAdmin_InsertFood;
            frmAdmin.UpdateFood += FrmAdmin_UpdateFood;
            frmAdmin.DeleteFood += FrmAdmin_DeleteFood;
            frmAdmin.ShowDialog();
        }

        private void FrmAdmin_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadTable();
        }

        private void FrmAdmin_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void FrmAdmin_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if(lsvBill.Tag != null)
            ShowBill((lsvBill.Tag as Table).ID);
        }

        private void frmQuanLy_Load(object sender, EventArgs e)
        {

        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;
            Category selected = cb.SelectedItem as Category;
            id = selected.ID;

            LoadFoodListByCategoryID(id);
        }
        private void btnThemMon_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if (table == null)
            {
                MessageBox.Show("Chưa chọn bàn");
                return;
            }

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int foodID = (cbFood.SelectedItem as Food).ID;
            int count = (int)nmFoodCount.Value;

            if (idBill == -1)
            {
                BillDAO.Instance.InsertBill(table.ID);
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), foodID, count);
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);
            }
            ShowBill(table.ID);
            LoadTable();
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int discount = (int)nmGiamGia.Value;

            double totalPrice = Convert.ToDouble(txtTotalPrice.Text.Split(',')[0]);
            double finalTotalPrice = totalPrice - (totalPrice / 100) * discount;
            if (idBill != -1)
            {
                if(MessageBox.Show(string.Format("Bạn có chắc thanh toán hóa đơn cho bàn {0}\nTổng tiền sau khi giảm giá là: {3}",table.Name, totalPrice, discount, finalTotalPrice),"Thông báo",MessageBoxButtons.OKCancel)== System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill,discount,(float)finalTotalPrice);
                    ShowBill(table.ID);
                    LoadTable();
                }
            }
        }   

         private void btnChuyenBan_Click(object sender, EventArgs e)
         {          
            int id1 = (lsvBill.Tag as Table).ID;

            int id2 = (cbChuyenBan.SelectedItem as Table).ID;

            if (MessageBox.Show(string.Format("Bạn có thật sự muốn chuyển bàn {0} qua bàn {1}", (lsvBill.Tag as Table).Name, (cbChuyenBan.SelectedItem as Table).Name), "Thông báo", MessageBoxButtons.OKCancel)== System.Windows.Forms.DialogResult.OK)
            {
               TableDAO.Instance.SwitchTable(id1, id2);

               LoadTable();
            }

             
         }



        private void lsvBill_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void flpTable_Paint(object sender, PaintEventArgs e)
        {

        }







        #endregion

        private void thôngTinTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void nmGiamGia_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnGiamGia_Click(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void nmFoodCount_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtTotalPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
