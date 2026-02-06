using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace demo5
{
    public partial class Form3 : Form
    {
        string cn = @"Data Source=WIN-PUURG92IVC5\SQLEXPRESS;Initial Catalog=Shoes;Integrated Security=True";
        int? _id;
        string _img = "Res/picture.png";

        public Form3(int? id)
        {
            InitializeComponent();
            _id = id;

            // Заполнение комбобоксов (категории, производители) 

            if (_id != null) LoadProductData();

            // По ТЗ: ID доступен только для чтения
            this.Text = _id == null ? "Добавление товара" : "Редактирование товара";
        }

        private void LoadProductData()
        {
            using (SqlConnection c = new SqlConnection(cn))
            {
                try
                {
                    c.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Product WHERE Id = @id", c);
                    cmd.Parameters.AddWithValue("@id", _id);
                    SqlDataReader r = cmd.ExecuteReader();
                    if (r.Read())
                    {
                        // Имена полей из вашего списка свойств:
                        txtTitle.Text = r["Name"].ToString();
                        txtPrice.Text = r["Price"].ToString();
                        txtStock.Text = r["AmountInStock"].ToString();
                        txtDescription.Text = r["Description"].ToString();
                        txtDiscount.Text = r["Discount"].ToString();

                        _img = r["Photo"].ToString();
                        if (File.Exists(_img)) pbPhoto.Image = Image.FromFile(_img);
                    }
                }
                catch (Exception ex) { MessageBox.Show("Ошибка: " + ex.Message); }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Валидация отрицательных значений по ТЗ
                if (decimal.Parse(txtPrice.Text) < 0 || int.Parse(txtStock.Text) < 0)
                {
                    MessageBox.Show("Стоимость и количество не могут быть отрицательными!",
                        "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection c = new SqlConnection(cn))
                {
                    c.Open();
                    string sql = (_id == null)
                        ? "INSERT INTO Product (Name, Price, AmountInStock, Photo, Description, Discount) VALUES (@n, @p, @s, @ph, @d, @dsc)"
                        : "UPDATE Product SET Name=@n, Price=@p, AmountInStock=@s, Photo=@ph, Description=@d, Discount=@dsc WHERE Id=@id";

                    SqlCommand cmd = new SqlCommand(sql, c);
                    cmd.Parameters.AddWithValue("@n", txtTitle.Text);
                    cmd.Parameters.AddWithValue("@p", decimal.Parse(txtPrice.Text));
                    cmd.Parameters.AddWithValue("@s", int.Parse(txtStock.Text));
                    cmd.Parameters.AddWithValue("@d", txtDescription.Text);
                    cmd.Parameters.AddWithValue("@dsc", txtDiscount.Text);
                    cmd.Parameters.AddWithValue("@ph", _img);
                    if (_id != null) cmd.Parameters.AddWithValue("@id", _id);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Сохранено!");
                    this.DialogResult = DialogResult.OK; this.Close();
                }
            }
            catch (Exception ex) { MessageBox.Show("Ошибка: " + ex.Message); }
        }

        private void btnSelectPhoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Images|*.jpg;*.png" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                using (var tempImg = Image.FromFile(ofd.FileName))
               // {
                   // if (tempImg.Width > 300 || tempImg.Height > 200)
                   // {
                   //     MessageBox.Show("Размер фото не должен превышать 300x200!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                   //     return;
                  //  }
               // }
                _img = "Res/" + Path.GetFileName(ofd.FileName);
                pbPhoto.Image = Image.FromFile(ofd.FileName);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Выйти без сохранения?", "Вопрос", MessageBoxButtons.YesNo) == DialogResult.Yes)
                this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void txtTitle_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
