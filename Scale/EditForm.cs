using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TereziEla
{
    public partial class EditForm : Form
    {
        public EditForm(string card, string driverName, string driverLast,
                   string carNumber, string carModel, string carCompany,
                 string carStatus, string desk)
        {
            InitializeComponent();
            textBox1.Text = card;
            textBox2.Text = driverName;
            textBox3.Text = driverLast;
            textBox4.Text = carNumber;
            textBox5.Text = carModel;
            textBox6.Text = carCompany;
            textBox7.Text = (carStatus); 
            textBox8.Text = desk;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string updatedCard =textBox1.Text;
            string updatedDriverName = textBox2.Text;
            string updatedDriverLast = textBox3.Text;
            string updatedCarNumber =textBox4.Text;
            string updatedCarModel = textBox5.Text;
            string updatedCarCompany = textBox6.Text;
            string updatedCarStatus = textBox7.Text;
            string updatedDesk = textBox8.Text;

          
            UpdateDatabase(updatedCard, updatedDriverName, updatedDriverLast, updatedCarNumber,
                           updatedCarModel, updatedCarCompany, updatedCarStatus, updatedDesk);

            this.Close(); 
        }

        private void UpdateDatabase(string card, string driverName, string driverLast, string carNumber,
                             string carModel, string carCompany, string carStatus, string desk)
        {
           
            string query = "UPDATE dbo.cards SET drivername = @drivername, driverlast = @driverlast, " +
                           "carnumber = @carnumber, carmodel = @carmodel, carcompany = @carcompany, " +
                           "carstatus = @carstatus, desk = @desk WHERE cards = @cards";

            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-IQB2C7N\\SQLEXPRESS;Initial Catalog=erp_azmaind;User ID=sa;Password=Scale123+-;TrustServerCertificate=True"))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                   
                    cmd.Parameters.AddWithValue("@cards", card);
                    cmd.Parameters.AddWithValue("@drivername", driverName);
                    cmd.Parameters.AddWithValue("@driverlast", driverLast);
                    cmd.Parameters.AddWithValue("@carnumber", carNumber);
                    cmd.Parameters.AddWithValue("@carmodel", carModel);
                    cmd.Parameters.AddWithValue("@carcompany", carCompany);
                    cmd.Parameters.AddWithValue("@carstatus", carStatus);
                    cmd.Parameters.AddWithValue("@desk", desk);

                   
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}