using ScaleManagment;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TereziEla
{
    public partial class AddCard : Form
    {
        private Form1 mainForm;
        

        public AddCard(Form1 form)
        {
            InitializeComponent();
            mainForm = form;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            mainForm.AddToList(
                textBox1.Text,
                textBox2.Text,
               textBox3.Text,
                textBox4.Text,
                textBox5.Text,
                textBox6.Text,
                textBox7.Text,
                textBox9.Text
             
            );

            this.Close();

            string connectionString = "Data Source=DESKTOP-IQB2C7N\\SQLEXPRESS;Initial Catalog=erp_azmaind;User ID=sa;Password=Scale123+-;Encrypt=True;TrustServerCertificate=True";

            string kartNo = textBox1.Text.Trim();
            string surucuAdi = textBox2.Text.Trim();
            string sonSurucu = textBox3.Text.Trim();
            string avtoNo = textBox4.Text.Trim();
            string avtomodel = textBox5.Text.Trim();
            string avtosirket = textBox6.Text.Trim();
            string avtostatus = textBox7.Text.Trim();
            string xammal = textBox9.Text.Trim();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"INSERT INTO dbo.cards
                         (cards, drivername, driverlast, carnumber, carmodel, carcompany, carstatus, desk)
                         VALUES (@cards, @drivername, @driverlast, @carnumber, @carmodel, @carcompany, @carstatus, @desk)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@cards", kartNo);
                        cmd.Parameters.AddWithValue("@drivername", surucuAdi);
                        cmd.Parameters.AddWithValue("@driverlast", sonSurucu);
                        cmd.Parameters.AddWithValue("@carnumber", avtoNo);
                        cmd.Parameters.AddWithValue("@carmodel", avtomodel);
                        cmd.Parameters.AddWithValue("@carcompany", avtosirket);
                        cmd.Parameters.AddWithValue("@carstatus", avtostatus);
                        cmd.Parameters.AddWithValue("@desk", xammal);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                           
                          
                            textBox1.Clear();
                            textBox2.Clear();
                            textBox3.Clear();
                            textBox4.Clear();
                            textBox5.Clear();
                            textBox6.Clear();
                            textBox7.Clear();
                            textBox9.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Məlumat əlavə olunmadı.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xəta baş verdi: " + ex.Message);
            }
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}



             // Məlumatları oxuyuruq
     

        // Form1-ə göndəririk

