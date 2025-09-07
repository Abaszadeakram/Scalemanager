using ScaleManagment;
using System;
using System.Windows.Forms;

namespace TereziEla
{
    public partial class Yeni_reys : Form
    {
        private Form1 mainForm;
        public Yeni_reys(Form1 form)
        {
            InitializeComponent();
            mainForm = form;
        }

        private void Yeni_reys_Load(object sender, EventArgs e)
        {

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
            mainForm.AddToList1(
               textBox1.Text,
               textBox2.Text,
              textBox3.Text,
              textBox4.Text,
               textBox5.Text,
              textBox6.Text,
              textBox7.Text,
              textBox8.Text,
              textBox9.Text

           );

            this.Close();
        }
    }
}