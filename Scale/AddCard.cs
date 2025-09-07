using ScaleManagment;

using System;
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
            // TextBox-ları oxuyuruq və Form1-ə göndəririk
            mainForm.AddToList(
                textBox1.Text,
                textBox2.Text,
               textBox3.Text,
                textBox4.Text,
                textBox5.Text,
                textBox6.Text,
                textBox7.Text,
               textBox8.Text
            );

            this.Close();
            // Bu formu bağla
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}



             // Məlumatları oxuyuruq
     

        // Form1-ə göndəririk

