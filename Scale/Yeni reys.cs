using ScaleManagment;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
