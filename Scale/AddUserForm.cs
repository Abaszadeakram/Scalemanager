using ScaleManagment;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TereziEla
{
    public partial class AddUserForm : Form
    {
        private Form1 mainForm;

        public AddUserForm()
        {
            InitializeComponent();
        }

        public AddUserForm(Form1 form)
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
            string username = textBox1.Text.Trim();
            if (!string.IsNullOrEmpty(username))
            {
                mainForm.AddUserToListView(username); // <<< mainForm artıq boş deyil
                this.Close();
            }

           
            string connectionString = "Data Source=DESKTOP-IQB2C7N\\SQLEXPRESS;Initial Catalog=Qeydiyyatdb;User ID=sa;Password=Scale123+-;Encrypt=True;TrustServerCertificate=True;";
            string istiafadeciadi= textBox1.Text.Trim();

            if (string.IsNullOrEmpty(istiafadeciadi))
            {
                MessageBox.Show("İstifadəçi adı daxil edin.");
                return;
            }

            // Indiki tarix
            DateTime now = DateTime.Now;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "INSERT INTO dbo.tblDatas ([Istifadeci adi], [Yaradilma tarixi]) VALUES (@IstifadeciAdi, @YaradilmaTarixi)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IstifadeciAdi", istiafadeciadi);
                        cmd.Parameters.AddWithValue("@YaradilmaTarixi", now);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Məlumat uğurla əlavə olundu.");
                            textBox1.Clear();
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
    }
    }

