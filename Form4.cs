using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace demo5
{
    public partial class Form4 : Form
    {
        // строка подключения
        string podkl = @"Data Source=WIN-PUURG92IVC5\SQLEXPRESS;Initial Catalog=Shoes;Integrated Security=True";
        int _role;

        public Form4(int role)
        {
            InitializeComponent();
            _role = role;

            // Настройка видимости кнопки "Добавить заказ" (button1 на Form4)
            button1.Visible = (_role == 1);

            LoadOrders();
        }

        public void LoadOrders()
        {
            // Очищаем список перед загрузкой
            flowLayoutPanel1.Controls.Clear();

            using (SqlConnection conn = new SqlConnection(podkl))
            {
                try
                {
                    conn.Open();
                    // SQL запрос: берем данные из заказа и подтягиваем текст статуса и адрес
                    string sql = @"SELECT o.Id, os.Name as StatusName, 
                                 p.City + ', ' + p.Street + ' ' + p.Building as FullAddr, 
                                 o.CreationDate, o.DeliveryDate 
                                 FROM [Order] o
                                 JOIN OrderStatus os ON o.StatusId = os.Id
                                 JOIN PickUpPoint p ON o.PickUpPointId = p.Id";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader r = cmd.ExecuteReader();

                    while (r.Read())
                    {
                        // Создаем пользовательский элемент
                        UserControl1 card = new UserControl1();

                        //Заполняем его через метод Fill
                        card.Fill(
                            (int)r["Id"],
                            r["StatusName"].ToString(),
                            r["FullAddr"].ToString(),
                            (DateTime)r["CreationDate"],
                            (DateTime)r["DeliveryDate"]
                        );

                        // Сохраняем ID заказа в Tag, чтобы потом знать, что редактировать
                        card.Tag = r["Id"];

                        // Если зашел Администратор — разрешаем клик для редактирования
                        if (_role == 1)
                        {
                            // Клик по самой карточке
                            card.Click += (s, e) => OpenEdit((int)((Control)s).Tag);

                            // Клик по любой надписи внутри карточки (чтобы попадать точно)
                            foreach (Control innerControl in card.Controls)
                            {
                                innerControl.Click += (s, e) => OpenEdit((int)((Control)s).Parent.Tag);
                            }
                        }

                        // 5. Добавляем карточку в панель
                        flowLayoutPanel1.Controls.Add(card);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки заказов: " + ex.Message);
                }
            }
        }

        // Метод для открытия окна редактирования/добавления (Form5)
        void OpenEdit(int? orderId)
        {
            Form5 f5 = new Form5(orderId);
            if (f5.ShowDialog() == DialogResult.OK)
            {
                LoadOrders(); // Обновляем список, если данные в базе изменились
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenEdit(null); // Передаем null, значит создаем новый заказ
        }
    }
}
