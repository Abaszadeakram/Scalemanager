using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TereziEla
{
    public partial class EditForm : Form
    {
        public EditForm(   string card, string driverName, string driverLast,
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
            textBox7.Text = (carStatus); // Əgər status "A" olsa aktiv işarələnir
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

        }
    }


 
}
