using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace demo5
{
    public partial class Form4 : Form
    {
        string podkl = @"Data Source=WIN-PUURG92IVC5\SQLEXPRESS;Initial Catalog=Shoes;Integrated Security=True";
        int _role;

        public Form4(int role)
        {
            InitializeComponent();
            _role = role;
            LoadOrders();
        }

        public void LoadOrders()
        {

            using (SqlConnection conn = new SqlConnection(podkl))
            {
                try
                {
                    conn.Open();
                    string sql = @"SELECT o.Id, os.Name as StatusName, 
                                 ISNULL(TRIM(p.City), '') + ', ' + ISNULL(TRIM(p.Street), '') as FullAddr, 
                                 o.CreationDate, o.DeliveryDate 
                                 FROM [Order] o
                                 JOIN OrderStatus os ON o.StatusId = os.Id
                                 JOIN PickUpPoint p ON o.PickUpPointId = p.Id";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader r = cmd.ExecuteReader();

                    while (r.Read())
                    {
                        UserControl1 card = new UserControl1();
                        card.Fill(
                            (int)r["Id"],
                            r["StatusName"].ToString(),
                            r["FullAddr"].ToString(),
                            (DateTime)r["CreationDate"],
                            (DateTime)r["DeliveryDate"]
                        );
                        card.Tag = r["Id"];

                        if (_role == 1)
                        {
                            card.Click += (s, e) => OpenEdit((int)((Control)s).Tag);
                            foreach (Control c in card.Controls)
                                c.Click += (s, e) => OpenEdit((int)((Control)s).Parent.Tag);
                        }
                        flowLayoutPanel1.Controls.Add(card);
                    }
                }
                catch (Exception ex)
                {
                    // если с бд проблема, мы увидим это сообщение
                    MessageBox.Show("Ошибка БД: " + ex.Message);
                }
            }
        }

        void OpenEdit(int? orderId)
        {
            Form5 f5 = new Form5(orderId);
            if (f5.ShowDialog() == DialogResult.OK) LoadOrders();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenEdit(null);
        }
    }
}
