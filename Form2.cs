using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace demo5
{
    public partial class Form2 : Form
    {
        string podkl = @"Data Source=WIN-PUURG92IVC5\SQLEXPRESS;Initial Catalog=Shoes;Integrated Security=True"; //podkl измените на перременную подключения

        public Form2(int role, string fio)
        {
            InitializeComponent();
            label1.Text = fio; // ФИО в углу

            // Права: кнопки только для Админа (1)
            btnAdd.Visible = btnDelete.Visible = btnEdit.Visible = (role == 1);

            LoadProducts();
        }

        public void LoadProducts()
        {
            flowLayoutPanel1.Controls.Clear();
            using (SqlConnection newpodkl = new SqlConnection(podkl)) //newpodkl тоже замените на свою переменную
            {
                try
                {
                    newpodkl.Open();
                    string sql = @"SELECT p.*, pr.Name as prN, prov.Name as pvN 
                                   FROM Product p
                                   LEFT JOIN Producer pr ON p.ProducerId = pr.Id
                                   LEFT JOIN Provider prov ON p.ProviderId = prov.Id
                                   WHERE p.Name LIKE @s";

                    if (cmbSort.SelectedIndex == 1) sql += " ORDER BY AmountInStock ASC";
                    if (cmbSort.SelectedIndex == 2) sql += " ORDER BY AmountInStock DESC";

                    SqlCommand newpodkl2 = new SqlCommand(sql, newpodkl); //тоже замените 
                    newpodkl2.Parameters.AddWithValue("@s", "%" + txtSearch.Text + "%");

                    SqlDataReader r = newpodkl2.ExecuteReader();
                    while (r.Read())
                    {
                        var card = new Producties();
                        // Безопасное чтение данных
                        card.FillData(
                            r["Name"].ToString(), r["Description"].ToString(),
                            r["prN"].ToString(), r["pvN"].ToString(),
                            Convert.ToDecimal(r["Price"]), Convert.ToInt32(r["Discount"]),
                            "шт.", Convert.ToInt32(r["AmountInStock"]), r["Photo"].ToString()
                        );

                        card.Tag = r["Id"];

                        // Клик по карточке делает её "выбранной" 
                        card.Click += (s, e) => {
                            foreach (Control ct in flowLayoutPanel1.Controls) ct.BackColor = System.Drawing.Color.White;
                            card.BackColor = System.Drawing.Color.LightGray;
                            flowLayoutPanel1.Tag = card.Tag; // Сохраняем ID выбранного товара в Tag панели
                        };

                        flowLayoutPanel1.Controls.Add(card);
                    }
                }
                catch (Exception ex) { MessageBox.Show("Ошибка: " + ex.Message); }
            }
        }

        // Метод открытия окна (ТЗ: не более 1 окна)
        void OpenEdit(int? id)
        {
            if (Application.OpenForms["Form3"] != null) return;
            Form3 f = new Form3(id);
            f.Owner = this;
            if (f.ShowDialog() == DialogResult.OK) LoadProducts();
        }

        // КНОПКА РЕДАКТИРОВАТЬ (привяжите её к btnEdit_Click в дизайнере)
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (flowLayoutPanel1.Tag != null)
                OpenEdit((int)flowLayoutPanel1.Tag);
            else
                MessageBox.Show("Сначала выберите товар, кликнув по нему!");
        }

        private void btnAdd_Click(object sender, EventArgs e) => OpenEdit(null);

        private void btnBack_Click(object sender, EventArgs e)
        {
            new Form1().Show();
            this.Close();
        }

        // События реального времени
        private void txtSearch_TextChanged(object sender, EventArgs e) => LoadProducts();
        private void cmbSort_SelectedIndexChanged(object sender, EventArgs e) => LoadProducts();
        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e) => LoadProducts();

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
