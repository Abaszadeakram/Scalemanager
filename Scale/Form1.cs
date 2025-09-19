
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;

using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using TereziEla;

namespace ScaleManagment
{
    public partial class Form1 : Form
    {

        private IntPtr _handle = IntPtr.Zero;

        private bool _running = false;



        private Label textBoxdata;
        private TextBox textBox1;
        public string terezidatas;
        public ListView listView;
        SerialPort serialPort;
        private TextBox txtSearch;
        public string data;
        int currentPage = 0;
        int pageSize = 10;
        int totalCount = 0;
        int totalCount1 = 0;
        int totalCount2 = 0;
        int totalCount3 = 0;


        private List<Button> pageButtons = new List<Button>();
        private List<Button> pageButtons1 = new List<Button>();
        private List<Button> pageButtons2 = new List<Button>();
        private List<Button> pageButtons3 = new List<Button>();
        private Label lblRfid;
        private Label lblOxuyucuyabagli;
        private TextBox textBoxId;
        private TextBox textBoxCarNumber;
        private TextBox textBoxCarModel;
        private TextBox textBoxCarcompany;
        private TextBox textBoxSurucuAd;
        private TextBox textBoxYuk;
        private TextBox textBoxTarix;
        private TextBox textBoxSurucuSon;
        private TextBox textBoxCarStatus;

        public object FlatAppearance { get; private set; }


        public TextBox lblSehifeyekecidPage { get; set; }


        public TextBox lblSehifeyekecidPage1 { get; set; }

        public TextBox lblSehifeyekecidPage2 { get; set; }

        public TextBox lblSehifeyekecidPage3 { get; set; }
        public TextBox TamData { get; }


        [DllImport("rfidreader.dll")]
        public static extern IntPtr Connect(string ip);

        [DllImport("rfidreader.dll")]
        public static extern int getCard(IntPtr handle, out int cardNo);

        [DllImport("rfidreader.dll")]
        public static extern void Disconnect(IntPtr handle);


        public Form1()
        {
            InitializeComponent();

            serialPort = new SerialPort("COM4", 19200, Parity.None, 8, StopBits.One)
            {
                Handshake = Handshake.None,
                DtrEnable = true,

                RtsEnable = true,
                Encoding = Encoding.UTF8,
                NewLine = "\r",
                Parity = Parity.None
            };

            serialPort.Open();




            serialPort.DataReceived += SerialPort_DataReceived;



            this.lblSehifeyekecidPage = new TextBox
            {
                Text = "0",
                Location = new Point(1190, -1),
                AutoSize = true
            };

            this.lblSehifeyekecidPage1 = new TextBox
            {
                Text = "0",
                Location = new Point(1190, -1),
                AutoSize = true
            };

            this.lblSehifeyekecidPage2 = new TextBox
            {
                Text = "0",
                Location = new Point(1190, -1),
                AutoSize = true
            };

            this.lblSehifeyekecidPage3 = new TextBox
            {
                Text = "0",
                Location = new Point(1190, -1),
                AutoSize = true
            };






            this.textBox1 = new TextBox();
            textBox1.Text = "0";

            textBox1.Dock = DockStyle.Bottom;
            textBox1.BorderStyle = BorderStyle.None;



            this.lblSehifeyekecidPage.TextChanged += lblSehifeyekecidPage_TextChanged;
            this.lblSehifeyekecidPage1.TextChanged += lblSehifeyekecidPage1_TextChanged;
            this.lblSehifeyekecidPage2.TextChanged += lblSehifeyekecidPage2_TextChanged;
            this.lblSehifeyekecidPage3.TextChanged += lblSehifeyekecidPage3_TextChanged;

            this.lblRfid = new Label
            {
                Text = "RFID status: ",
                ForeColor = Color.Black,

                AutoSize = true,
                Location = new Point(12, 12)
            };

            this.lblOxuyucuyabagli = new Label
            {
                Text = " ● Oxuyucuya bağlı",
                ForeColor = Color.Green,

                AutoSize = true,
                Location = new Point(lblRfid.Right + 3, lblRfid.Top)
            };

            this.textBoxId = new TextBox();
            textBoxId.Text = "553659547";

            textBoxId.Dock = DockStyle.Bottom;
            textBoxId.BorderStyle = BorderStyle.None;



            this.textBoxCarNumber = new TextBox();
            textBoxCarNumber.Text = "77JB459";

            textBoxCarNumber.Dock = DockStyle.Bottom;
            textBoxCarNumber.BorderStyle = BorderStyle.None;

            this.textBoxCarModel = new TextBox();
            textBoxCarModel.Text = "BMW";

            textBoxCarModel.Dock = DockStyle.Bottom;
            textBoxCarModel.BorderStyle = BorderStyle.None;



            this.textBoxCarcompany = new TextBox();
            textBoxCarcompany.Text = "AMG";

            textBoxCarcompany.Dock = DockStyle.Bottom;
            textBoxCarcompany.BorderStyle = BorderStyle.None;


            this.textBoxCarStatus = new TextBox();
            textBoxCarStatus.Text = "A";

            textBoxCarStatus.Dock = DockStyle.Bottom;
            textBoxCarStatus.BorderStyle = BorderStyle.None;


            this.textBoxSurucuAd = new TextBox();
            textBoxSurucuAd.Text = "Mərəh Mərəh";

            textBoxSurucuAd.Dock = DockStyle.Bottom;
            textBoxSurucuAd.BorderStyle = BorderStyle.None;


            this.textBoxSurucuSon = new TextBox();
            textBoxSurucuSon.Text = "Mərəh Mərəh";

            textBoxSurucuSon.Dock = DockStyle.Bottom;
            textBoxSurucuSon.BorderStyle = BorderStyle.None;


            this.textBoxYuk = new TextBox();
            textBoxYuk.Text = "Low quality gold";

            textBoxYuk.Dock = DockStyle.Bottom;
            textBoxYuk.BorderStyle = BorderStyle.None;


        }


        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = serialPort.ReadLine();
            this.BeginInvoke(new Action(() =>
            {
                data = Regex.Replace(data, @"[^\d]", "");
                textBox1.Text = data + Environment.NewLine + " " + "Kg";

            }));
        }


        private void UpdatePageButtonStyles(Button clickedButton)
        {
            foreach (Button btn in pageButtons)
            {
                if (btn == clickedButton)
                {
                    btn.ForeColor = Color.Orange;
                }
                else
                {
                    btn.ForeColor = Color.Black;
                }
            }
        }

        private void UpdatePageButtonStyles1(Button clickedButton)
        {
            foreach (Button btn in pageButtons1)
            {
                if (btn == clickedButton)
                {
                    btn.ForeColor = Color.Orange;
                }
                else
                {
                    btn.ForeColor = Color.Black;
                }
            }
        }

        private void UpdatePageButtonStyles2(Button clickedButton)
        {
            foreach (Button btn in pageButtons2)
            {
                if (btn == clickedButton)
                {
                    btn.ForeColor = Color.Orange;
                }
                else
                {
                    btn.ForeColor = Color.Black;
                }
            }
        }

        private void UpdatePageButtonStyles3(Button clickedButton)
        {
            foreach (Button btn in pageButtons3)
            {
                if (btn == clickedButton)
                {
                    btn.ForeColor = Color.Orange;
                }
                else
                {
                    btn.ForeColor = Color.Black;
                }
            }
        }




        public void button1_Click(object sender, EventArgs e)
        {

            button1.BackColor = Color.White;
            button1.ForeColor = Color.Orange;

            button2.BackColor = Color.Black;
            button2.ForeColor = Color.White;

            button3.BackColor = Color.Black;
            button3.ForeColor = Color.White;

            button4.BackColor = Color.Black;
            button4.ForeColor = Color.White;

            button5.BackColor = Color.Black;
            button5.ForeColor = Color.White;

            button6.BackColor = Color.Black;
            button6.ForeColor = Color.White;

            button7.BackColor = Color.Black;
            button7.ForeColor = Color.White;

            scaleInfoContent.Controls.Clear();

            TableLayoutPanel mainLayout = new TableLayoutPanel();
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.RowCount = 3;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));

            TextBox searchBox = new TextBox();
            searchBox.Size = new Size(130, 20);
            searchBox.BorderStyle = BorderStyle.None;
            searchBox.Location = new Point(scaleInfoContent.Width - 1240, 5);

            searchBox.Text = "Axtarış edin";
            searchBox.ForeColor = Color.Gray;

            searchBox.Enter += (s, ev) =>
            {
                if (searchBox.Text == "Axtarış edin")
                {
                    searchBox.Text = "";
                    searchBox.ForeColor = Color.Black;
                }
            };

            searchBox.Leave += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(searchBox.Text))
                {
                    searchBox.Text = "Axtarış edin";
                    searchBox.ForeColor = Color.Gray;
                }
            };

            scaleInfoContent.Controls.Add(searchBox);
            searchBox.TextChanged += new EventHandler(SearchBox_TextChanged);

            PictureBox searchIcon = new PictureBox();
            searchIcon.Image = Image.FromFile("C:\\Users\\Akbar\\Documents\\pictures\\search-icon-2-614x460.png");
            searchIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            searchIcon.Size = new Size(30, 30);
            searchIcon.Location = new Point(searchBox.Location.X + searchBox.Width - 2, searchBox.Location.Y - 8);

            searchIcon.Click += (s, ev) =>
            {
                MessageBox.Show("Axtarış etmək üçün simgeyə basıldı!");
            };

            scaleInfoContent.Controls.Add(searchIcon);

            Button btnDelete = new Button();
            btnDelete.Text = "İstifadəçini sil";
            btnDelete.Size = new Size(120, 32);
            btnDelete.Location = new Point(scaleInfoContent.Width - 250, 7);
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Font = new Font("Arial", 10);
            scaleInfoContent.Controls.Add(btnDelete);

            btnDelete.Click += new EventHandler(btnDelete_Click);

            Button btnNew = new Button();
            btnNew.Text = "+Yeni istifadəçi";

            btnNew.Size = new Size(120, 32);
            btnNew.BackColor = Color.FromArgb(223, 199, 76);
            btnNew.Location = new Point(scaleInfoContent.Width - 125, 7);
            btnNew.ForeColor = Color.White;
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Font = new Font("Arial", 10);
            btnNew.FlatAppearance.BorderSize = 0;
            scaleInfoContent.Controls.Add(btnNew);

            btnNew.Click += new EventHandler(btnNew_Click);

            listView = new ListView();
            listView.View = View.Details;
            listView.FullRowSelect = true;
            listView.GridLines = true;
            listView.Size = new Size(1460, 820);
            listView.Location = new Point(10, 40);
            listView.CheckBoxes = true;
            listView.BorderStyle = BorderStyle.None;

            listView.Columns.Add("İstifadəçi adı", 110, HorizontalAlignment.Left);
            listView.Columns.Add("Yaradılma tarixi", 1300, HorizontalAlignment.Center);

            listView.OwnerDraw = true;

            listView.DrawColumnHeader += (s, args) =>
            {
                using (Font f = new Font("Segoe UI", 8, FontStyle.Bold))
                using (StringFormat sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(243, 244, 246)))
                {
                    args.Graphics.FillRectangle(brush, args.Bounds);
                    args.Graphics.DrawRectangle(Pens.LightGray, args.Bounds);
                    args.Graphics.DrawString(args.Header.Text, f, Brushes.Black, args.Bounds, sf);
                }
                listView.CheckBoxes = true;
            };


            listView.DrawItem += (s, args) => args.DrawDefault = true;
            listView.DrawSubItem += (s, args) => args.DrawDefault = true;

            scaleInfoContent.Controls.Add(listView);

            string connectionString = "Data Source=DESKTOP-IQB2C7N\\SQLEXPRESS;Initial Catalog=Qeydiyyatdb;User ID=sa;Password=Scale123+-;Encrypt=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "select COUNT(*) count from tblDatas";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    totalCount = int.Parse(reader["count"].ToString());


                }

                reader.Close();
            }

            LoadData(1, pageSize);

            Panel bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40
            };

            Panel topPanel = new Panel { Dock = DockStyle.Fill };

            Label lblCount = new Label
            {
                Text = "Sətir sayı: " + listView.Items.Count,
                Location = new Point(12, -1),
                AutoSize = true
            };

            int PageCount = (int)Math.Ceiling((double)totalCount / pageSize);

            for (int i = 1; i <= PageCount; i++)
            {
                Button btnPage = new Button { Text = i.ToString(), Location = new Point(450 + i * 35, -1), Width = 20 };
                btnPage.Click += new EventHandler(btnPage1_Click);

                btnPage.FlatStyle = FlatStyle.Flat;
                btnPage.FlatAppearance.BorderSize = 0;
                bottomPanel.Controls.Add(btnPage);
                pageButtons.Add(btnPage);

            }

            Button btnPrev = new Button { Text = "<", Location = new Point(450, -1), Width = 20 };
            btnPrev.Click += new EventHandler(btnPrev_Click);
            btnPrev.FlatStyle = FlatStyle.Flat;
            btnPrev.FlatAppearance.BorderSize = 0;
            bottomPanel.Controls.Add(btnPrev);

            Button btnNext = new Button { Text = ">", Location = new Point(450 + (PageCount ) * 35, -1), Width = 20 };
            btnNext.Click += new EventHandler(btnNext_Click);
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.FlatAppearance.BorderSize = 0;
            bottomPanel.Controls.Add(btnNext);


            lblCount.Text = pageSize.ToString();

            Label lblSehife = new Label
            {

                Location = new Point(1000, -1),
                AutoSize = true
            };
            lblSehife.Text = PageCount.ToString() + " " + "Səhifə";

            Label lblSehifeyekecid = new Label
            {
                Text = "Səhifəyə keç: ",
                Location = new Point(1100, -1),
                AutoSize = true
            };



            bottomPanel.Controls.Add(lblCount);

            bottomPanel.Controls.Add(lblSehife);
            bottomPanel.Controls.Add(lblSehifeyekecid);
            bottomPanel.Controls.Add(this.lblSehifeyekecidPage);

            this.Controls.Add(bottomPanel);

            mainLayout.Controls.Add(topPanel, 0, 0);
            mainLayout.Controls.Add(listView, 0, 1);
            mainLayout.Controls.Add(bottomPanel, 0, 2);

            scaleInfoContent.Controls.Add(mainLayout);

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            LoadData(currentPage + 1, pageSize);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            LoadData(currentPage - 1, pageSize);
        }

        private void lblSehifeyekecidPage_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(lblSehifeyekecidPage.Text, out int pageNumber) && pageNumber > 0)
            {

                currentPage = pageNumber - 1;


                LoadData(currentPage, pageSize);

            }

        }


        private void btnPage1_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            string btnText = btn.Text;



            currentPage = int.Parse(btnText);
            LoadData(currentPage, pageSize);
            UpdatePageButtonStyles(btn);
        }


        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string searchQuery = textBox.Text;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {


            string connectionString = "Data Source=DESKTOP-IQB2C7N\\SQLEXPRESS;Initial Catalog=Qeydiyyatdb;User ID=sa;Password=Scale123+-;TrustServerCertificate=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();


                foreach (ListViewItem item in listView.CheckedItems)
                {

                    string userName = item.Text;


                    string query = "DELETE FROM tblDatas WHERE [Istifadeci adi] = @userName"; ;

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userName", userName);
                        cmd.ExecuteNonQuery();
                    }


                    listView.Items.Remove(item);
                }
            }
        }
        public void AddUserToListView(string username)
        {
            if (listView == null)
            {
                MessageBox.Show("ListView hələ yaradılmayıb!");
                return;
            }

            string date = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
            ListViewItem item = new ListViewItem(username);
            item.SubItems.Add(date);
            listView.Items.Add(item);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            AddUserForm addUserForm = new AddUserForm(this);
            addUserForm.ShowDialog();
        }

        public void AddToList(string kartNo, string SurucunAdi, string SonSurucu,
                         string avtoNo, string avtomodel,
                         string avtosirket, string avtostatus, string xammal)
        {
            ListViewItem item = new ListViewItem("");

            item.SubItems.Add(kartNo);
            item.SubItems.Add(SurucunAdi);
            item.SubItems.Add(SonSurucu);
            item.SubItems.Add(avtoNo);
            item.SubItems.Add(avtomodel);
            item.SubItems.Add(avtosirket);
            item.SubItems.Add(avtostatus);
            item.SubItems.Add(xammal);

            listView.Items.Add(item);
        }

        public void AddToList1(string kartNo, string masinnomresi, string Tullantininnovu,
                       string girisvaxti, string girisdekicekiton,
                       string cixisvsxti, string cixisdakicekiton, string yukuncekisiton, string post)
        {
            ListViewItem item = new ListViewItem("");

            item.SubItems.Add(kartNo);
            item.SubItems.Add(masinnomresi);
            item.SubItems.Add(Tullantininnovu);
            item.SubItems.Add(girisvaxti);
            item.SubItems.Add(girisdekicekiton);
            item.SubItems.Add(cixisvsxti);
            item.SubItems.Add(cixisdakicekiton);
            item.SubItems.Add(yukuncekisiton);
            item.SubItems.Add(post);

            listView.Items.Add(item);
        }

        private void LoadData(int pageIndex, int pageSize)
        {
            string connectionString = "Data Source=DESKTOP-IQB2C7N\\SQLEXPRESS;Initial Catalog=Qeydiyyatdb;User ID=sa;Password=Scale123+-;Encrypt=True;TrustServerCertificate=True";
            string query = @"
        SELECT * 
        FROM dbo.tblDatas
        ORDER BY  [Yaradilma tarixi] 
        OFFSET @PageIndex * @PageSize ROWS 
        FETCH NEXT @PageSize ROWS ONLY";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);

                    SqlDataReader reader = cmd.ExecuteReader();
                    listView.Items.Clear();

                    while (reader.Read())
                    {
                        string name = reader["Istifadeci adi"].ToString();
                        DateTime date = Convert.ToDateTime(reader["Yaradilma tarixi"]);

                        ListViewItem item = new ListViewItem(name);
                        item.SubItems.Add(date.ToString("dd.MM.yyyy HH:mm"));

                        listView.Items.Add(item);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            button1.BackColor = Color.Black;
            button1.ForeColor = Color.White;


            button2.BackColor = Color.White;
            button2.ForeColor = Color.Orange;

            button3.BackColor = Color.Black;
            button3.ForeColor = Color.White;

            button4.BackColor = Color.Black;
            button4.ForeColor = Color.White;

            button5.BackColor = Color.Black;
            button5.ForeColor = Color.White;

            button6.BackColor = Color.Black;
            button6.ForeColor = Color.White;

            button7.BackColor = Color.Black;
            button7.ForeColor = Color.White;
            scaleInfoContent.Controls.Clear();

            TableLayoutPanel tbl = new TableLayoutPanel();
            tbl.Dock = DockStyle.Fill;
            tbl.ColumnCount = 4;
            tbl.RowCount = 3;
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));

            Font labelFont = new Font("Segoe UI", 9, FontStyle.Regular);
            Font textFont = new Font("Segoe UI", 10, FontStyle.Bold);



            Padding fieldPadding = new Padding(6, 8, 6, 8);


            Label label1 = new Label();
            label1.Text = "Tarazi";
            label1.Font = labelFont;
            label1.Dock = DockStyle.Top;
            label1.AutoSize = true;



            Panel panel1 = new Panel();
            panel1.Dock = DockStyle.Fill;
            panel1.Padding = fieldPadding;
            panel1.Controls.Add(this.textBox1);
            panel1.Controls.Add(label1);
            tbl.Controls.Add(panel1, 0, 0);

            Label label2 = new Label();
            label2.Text = "Kart ID";
            label2.Font = labelFont;
            label2.Dock = DockStyle.Top;
            label2.AutoSize = true;



            Panel panel2 = new Panel();
            panel2.Dock = DockStyle.Fill;
            panel2.Padding = fieldPadding;
            panel2.Controls.Add(this.textBoxId);
            panel2.Controls.Add(label2);
            tbl.Controls.Add(panel2, 1, 0);




            Label label3 = new Label();
            label3.Text = "Sürücü";
            label3.Font = labelFont;
            label3.Dock = DockStyle.Top;
            label3.AutoSize = true;


            Panel panel3 = new Panel();
            panel3.Dock = DockStyle.Fill;
            panel3.Padding = fieldPadding;
            panel3.Controls.Add(this.textBoxSurucuAd);
            panel3.Controls.Add(label3);
            tbl.Controls.Add(panel3, 2, 0);

            Label label4 = new Label();
            label4.Text = "Son Sürücü";
            label4.Font = labelFont;
            label4.Dock = DockStyle.Top;
            label4.AutoSize = true;

            Panel panel4 = new Panel();
            panel4.Dock = DockStyle.Fill;
            panel4.Padding = fieldPadding;
            panel4.Controls.Add(this.textBoxSurucuSon);
            panel4.Controls.Add(label4);
            tbl.Controls.Add(panel4, 3, 0);



            Label label5 = new Label();
            label5.Text = "Avtomobil nömrəsi";
            label5.Font = labelFont;
            label5.Dock = DockStyle.Top;
            label5.AutoSize = true;

            Panel panel5 = new Panel();
            panel5.Dock = DockStyle.Fill;
            panel5.Padding = fieldPadding;
            panel5.Controls.Add(this.textBoxCarNumber);
            panel5.Controls.Add(label5);
            tbl.Controls.Add(panel4, 3, 0);




            Label label6 = new Label();
            label6.Text = "Avtomobil modeli";
            label6.Font = labelFont;
            label6.Dock = DockStyle.Top;
            label6.AutoSize = true;

            Panel panel6 = new Panel();
            panel6.Dock = DockStyle.Fill;
            panel6.Padding = fieldPadding;
            panel6.Controls.Add(this.textBoxCarModel);
            panel6.Controls.Add(label6);
            tbl.Controls.Add(panel6, 0, 1);




            Label label7 = new Label();
            label7.Text = "Şirkət";
            label7.Font = labelFont;
            label7.Dock = DockStyle.Top;
            label7.AutoSize = true;


            Panel panel7 = new Panel();
            panel7.Dock = DockStyle.Fill;
            panel7.Padding = fieldPadding;
            panel7.Controls.Add(this.textBoxCarcompany);
            panel7.Controls.Add(label7);
            tbl.Controls.Add(panel7, 1, 1);


            Label label8 = new Label();
            label8.Text = "Avtomobil Status";
            label8.Font = labelFont;
            label8.Dock = DockStyle.Top;
            label8.AutoSize = true;



            Panel panel8 = new Panel();
            panel8.Dock = DockStyle.Fill;
            panel8.Padding = fieldPadding;
            panel8.Controls.Add(this.textBoxCarStatus);
            panel8.Controls.Add(label8);
            tbl.Controls.Add(panel8, 2, 1);

            Label label9 = new Label();
            label9.Text = "Yükün növü";
            label9.Font = labelFont;
            label9.Dock = DockStyle.Top;
            label9.AutoSize = true;



            Panel panel9 = new Panel();
            panel9.Dock = DockStyle.Fill;
            panel9.Padding = fieldPadding;
            panel9.Controls.Add(this.textBoxYuk);
            panel9.Controls.Add(label9);
            tbl.Controls.Add(panel9, 3, 1);

            Panel bottomPanel = new Panel { Dock = DockStyle.Fill };



            bottomPanel.Controls.Add(this.lblRfid);
            bottomPanel.Controls.Add(this.lblOxuyucuyabagli);


            RadioButton toggle = new RadioButton
            {
                Appearance = Appearance.Button,
                Text = "Avtomatik",
                TextAlign = ContentAlignment.MiddleCenter,
                FlatStyle = FlatStyle.Flat,
                Width = 100,
                Height = 30,
                Location = new Point(lblOxuyucuyabagli.Right + 500, lblOxuyucuyabagli.Top + 4)
            };

            toggle.FlatAppearance.BorderSize = 0;


            toggle.CheckedChanged += (s, args) =>
            {
                if (toggle.Checked)
                {
                    toggle.BackColor = Color.Green;
                    toggle.ForeColor = Color.White;
                    toggle.Text = "ON";
                }
                else
                {
                    toggle.BackColor = Color.Red;
                    toggle.ForeColor = Color.White;
                    toggle.Text = "OFF";
                }
            };

            bottomPanel.Controls.Add(toggle);

            Button btnBagla = new Button { Text = "Bağla", ForeColor = Color.Red, FlatStyle = FlatStyle.Flat, Location = new Point(830, 15), Width = 60, Height = 30 };
            btnBagla.Click += new EventHandler(btnBagla_Click);
            Button btnAc = new Button { Text = "Aç", ForeColor = Color.Green, FlatStyle = FlatStyle.Flat, Location = new Point(910, 15), Width = 40, Height = 30 };
            btnAc.Click += new EventHandler(btnAc_Click);
            Button btnTara = new Button { Text = "Tara", FlatStyle = FlatStyle.Flat, Location = new Point(960, 15), Width = 50, Height = 30 };
            btnTara.FlatAppearance.BorderSize = 0;
            Button btnTesdiqla = new Button { Text = "Təsdiqlə", BackColor = Color.Green, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Location = new Point(1020, 15), Width = 90, Height = 30 };
            Button btnOxucu = new Button { Text = "Oxucuya bağlan", BackColor = Color.Goldenrod, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Location = new Point(1120, 15), Width = 110, Height = 30 };
            btnOxucu.Click += new EventHandler(btnOxucu_Click);
            bottomPanel.Controls.AddRange(new Control[] { btnBagla, btnAc, btnTara, btnTesdiqla, btnOxucu });

            tbl.Controls.Add(bottomPanel, 0, 2);
            tbl.SetColumnSpan(bottomPanel, 4);


            scaleInfoContent.Controls.Add(tbl);
        }

        private void btnBagla_Click(object sender, EventArgs e)
        {

        }



        private void btnAc_Click(object sender, EventArgs e)
        {
            open_door();
            open_door1();
        }

        private void open_door1()
        {
        }

        private void open_door()
        {



        }


        private void btnOxucu_Click(object sender, EventArgs e)
        {
            string ip = "192.168.200.70";
            _handle = Connect(ip);

            if (_handle != IntPtr.Zero)
            {
                lblOxuyucuyabagli.Text = "Oxucu qoşuldu!";
                lblOxuyucuyabagli.ForeColor = Color.Green;
                _running = true;
                StartReaderLoop();
            }
            else
            {
                lblOxuyucuyabagli.Text = "Qoşulma alınmadı!";
                lblOxuyucuyabagli.ForeColor = Color.Red;
            }
        }

        private void StartReaderLoop()
        {
            Thread t = new Thread(() =>
            {
                while (_running)
                {
                    int cardNo;
                    int ok = getCard(_handle, out cardNo);

                    if (ok == 1)
                    {
                        this.BeginInvoke(new Action(() =>
                        {

                            textBoxId.Text = cardNo.ToString();


                            OnCardRead(cardNo.ToString());
                        }));
                    }

                    Thread.Sleep(50);
                }
            });

            t.IsBackground = true;
            t.Start();
        }



        private void OnCardRead(string cardNo)
        {


            var connectionString = "Data Source=DESKTOP-IQB2C7N\\SQLEXPRESS;Initial Catalog=erp_azmaind;User ID=sa;Password=Scale123+-;Encrypt=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
            SELECT TOP 1
                cards, drivername, driverlast, carnumber, carmodel, carcompany, carstatus, desk, 
            FROM dbo.cards
            WHERE cards = @cardNo";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@cardNo", cardNo);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            textBoxId.Text = reader["cards"]?.ToString();
                            textBoxSurucuAd.Text = (reader["drivername"]?.ToString());
                            textBoxSurucuSon.Text = reader["carnumber"]?.ToString();
                            textBoxCarModel.Text = reader["carmodel"]?.ToString();
                            textBoxCarcompany.Text = reader["carcompany"]?.ToString();
                            textBoxCarStatus.Text = reader["carstatus"]?.ToString();


                            lblOxuyucuyabagli.Text = "Kart tapıldı, qeydiyyat yükləndi";
                            lblOxuyucuyabagli.ForeColor = Color.Green;
                        }
                        else
                        {

                            textBoxId.Clear();
                            textBoxSurucuAd.Clear();
                            textBoxSurucuSon.Clear();
                            textBoxCarModel.Clear();
                            textBoxCarcompany.Clear();
                            textBoxCarStatus.Clear();

                            lblOxuyucuyabagli.Text = "Yeni kart — məlumat tapılmadı";
                            lblOxuyucuyabagli.ForeColor = Color.Red;
                        }
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            button1.BackColor = Color.Black;
            button1.ForeColor = Color.White;


            button2.BackColor = Color.Black;
            button2.ForeColor = Color.White;

            button3.BackColor = Color.White;
            button3.ForeColor = Color.Orange;

            button4.BackColor = Color.Black;
            button4.ForeColor = Color.White;

            button5.BackColor = Color.Black;
            button5.ForeColor = Color.White;

            button6.BackColor = Color.Black;
            button6.ForeColor = Color.White;

            button7.BackColor = Color.Black;
            button7.ForeColor = Color.White;
            scaleInfoContent.Controls.Clear();

            TableLayoutPanel mainLayout = new TableLayoutPanel();
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.RowCount = 3;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));

            Panel topPanel = new Panel { Dock = DockStyle.Fill };

            TextBox txtSearch = new TextBox
            {

                Location = new Point(10, 8),

                Width = 200
            };

            Button btnExport = new Button();
            btnExport.Text = " File export";
            btnExport.Image = Image.FromFile("C:\\Users\\Akbar\\Documents\\pictures\\images (5).png");
            Image originalImage = btnExport.Image;
            Image resizedImage = new Bitmap(originalImage, new Size(originalImage.Width / 18, originalImage.Height / 18));  // Burada şəkili yarıya endiririk
            btnExport.Image = resizedImage;
            btnExport.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnExport.Location = new Point(1100, 5);
            btnExport.Width = 150;
            btnExport.Height = 30;

            btnExport.FlatStyle = FlatStyle.Flat;

            btnExport.Font = new Font("Arial", 10);

            btnExport.FlatAppearance.BorderSize = 0;
            scaleInfoContent.Controls.Add(btnExport);

            scaleInfoContent.Controls.Clear();
            TextBox searchBox = new TextBox();
            searchBox.Size = new Size(130, 20);
            searchBox.BorderStyle = BorderStyle.None;
            searchBox.Location = new Point(scaleInfoContent.Width - 1240, 5);

            searchBox.Text = "Axtarış edin";
            searchBox.ForeColor = Color.Gray;

            searchBox.Enter += (s, ev) =>
            {
                if (searchBox.Text == "Axtarış edin")
                {
                    searchBox.Text = "";
                    searchBox.ForeColor = Color.Black;
                }
            };

            searchBox.Leave += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(searchBox.Text))
                {
                    searchBox.Text = "Axtarış edin";
                    searchBox.ForeColor = Color.Gray;
                }
            };

            scaleInfoContent.Controls.Add(searchBox);
            searchBox.TextChanged += new EventHandler(SearchBox_TextChanged);

            PictureBox searchIcon = new PictureBox();
            searchIcon.Image = Image.FromFile("C:\\Users\\Akbar\\Documents\\pictures\\search-icon-2-614x460.png");
            searchIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            searchIcon.Size = new Size(30, 30);
            searchIcon.Location = new Point(searchBox.Location.X + searchBox.Width - 2, searchBox.Location.Y - 8);

            searchIcon.Click += (s, ev) =>
            {
                MessageBox.Show("Axtarış etmək üçün simgeyə basıldı!");
            };

            scaleInfoContent.Controls.Add(searchIcon);

            topPanel.Controls.Add(btnExport);
            scaleInfoContent.Controls.Add(btnExport);


            listView = new ListView();
            listView.View = View.Details;
            listView.FullRowSelect = true;
            listView.GridLines = true;
            listView.Size = new Size(1460, 820);
            listView.Location = new Point(10, 40);
            listView.CheckBoxes = true;
            listView.BorderStyle = BorderStyle.None;

            listView.Columns.Add("Giriş çəkisi", 120);
            listView.Columns.Add("Çıxış çəkisi", 120);
            listView.Columns.Add("Ümumi çəki", 150);
            listView.Columns.Add("Giriş tarixi", 150);
            listView.Columns.Add("Çıxış tarixi", 150);
            listView.Columns.Add("Kart", 100);
            listView.Columns.Add("Grade", 150);
            listView.Columns.Add("Post", 150);
            listView.Columns.Add("Maşın nömrəsi", 150);

            listView.OwnerDraw = true;

            listView.DrawColumnHeader += (s, args) =>
            {
                using (Font f = new Font("Segoe UI", 8, FontStyle.Bold))
                using (StringFormat sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(243, 244, 246)))
                {
                    args.Graphics.FillRectangle(brush, args.Bounds);
                    args.Graphics.DrawRectangle(Pens.LightGray, args.Bounds);
                    args.Graphics.DrawString(args.Header.Text, f, Brushes.Black, args.Bounds, sf);
                }
                listView.CheckBoxes = true;
            };

            listView.DrawItem += (s, args) => args.DrawDefault = true;
            listView.DrawSubItem += (s, args) => args.DrawDefault = true;

            scaleInfoContent.Controls.Add(listView);


            string connectionString = "Data Source=DESKTOP-IQB2C7N\\SQLEXPRESS;Initial Catalog=erp_azmaind;User ID=sa;Password=Scale123+-;Encrypt=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "select COUNT(*) count from gates";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    totalCount1 = int.Parse(reader["count"].ToString());
                }

                reader.Close();
            }

            LoadData1(1, pageSize);
            Panel bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40
            };

            Label lblCount = new Label
            {
                Text = "Sətir sayı: " + listView.Items.Count,
                Location = new Point(12, -1),
                AutoSize = true
            };

            int PageCount1 = (int)Math.Ceiling((double)totalCount1 / pageSize);

            for (int i = 1; i <= PageCount1; i++)
            {
                Button btnPage1 = new Button { Text = i.ToString(), Location = new Point(450 + i * 35, -1), Width = 20 };
                btnPage1.Click += new EventHandler(btnPage2_Click);

                btnPage1.FlatStyle = FlatStyle.Flat;
                btnPage1.FlatAppearance.BorderSize = 0;
                bottomPanel.Controls.Add(btnPage1);
                pageButtons1.Add(btnPage1);

            }

            Button btnPrev1 = new Button { Text = "<", Location = new Point(450, -1), Width = 20 };
            btnPrev1.Click += new EventHandler(btnPrev1_Click);
            btnPrev1.FlatStyle = FlatStyle.Flat;
            btnPrev1.FlatAppearance.BorderSize = 0;
            bottomPanel.Controls.Add(btnPrev1);

            Button btnNext1 = new Button { Text = ">", Location = new Point(450 + (PageCount1 + 1) * 35, -1), Width = 20 };
            btnNext1.Click += new EventHandler(btnNext1_Click);
            btnNext1.FlatStyle = FlatStyle.Flat;
            btnNext1.FlatAppearance.BorderSize = 0;
            bottomPanel.Controls.Add(btnNext1);

            Label lblSehife = new Label
            {

                Location = new Point(1000, -1),
                AutoSize = true
            };

            lblSehife.Text = PageCount1.ToString() + " " + "Səhifə";
            Label lblSehifeyekecid = new Label
            {
                Text = "Səhifəyə keç: ",
                Location = new Point(1100, -1),
                AutoSize = true
            };



            bottomPanel.Controls.Add(lblCount);

            bottomPanel.Controls.Add(lblSehife);
            bottomPanel.Controls.Add(lblSehifeyekecid);
            bottomPanel.Controls.Add(this.lblSehifeyekecidPage1);

            this.Controls.Add(bottomPanel);

            mainLayout.Controls.Add(topPanel, 0, 0);
            mainLayout.Controls.Add(listView, 0, 1);
            mainLayout.Controls.Add(bottomPanel, 0, 2);

            scaleInfoContent.Controls.Add(mainLayout);
            btnExport.Click += new EventHandler(btnExport_Click);


        }
        private void lblSehifeyekecidPage1_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(lblSehifeyekecidPage1.Text, out int pageNumber) && pageNumber > 0)
            {

                currentPage = pageNumber - 1;


                LoadData1(currentPage, pageSize);

            }
        }

        private void btnPage2_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string btnText = btn.Text;



            currentPage = int.Parse(btnText);
            LoadData1(currentPage, pageSize);
            UpdatePageButtonStyles1(btn);
        }

        private void btnNext1_Click(object sender, EventArgs e)
        {
            LoadData1(currentPage + 1, pageSize);
            UpdatePageButtonStyles(sender as Button);

        }

        private void btnPrev1_Click(object sender, EventArgs e)
        {
            LoadData1(currentPage - 1, pageSize);
            UpdatePageButtonStyles(sender as Button);
        }


        private void LoadData1(int pageIndex, int pageSize)
        {

            string connectionString =
                "Data Source=DESKTOP-IQB2C7N\\SQLEXPRESS;Initial Catalog=erp_azmaind;User ID=sa;Password=Scale123+-;Encrypt=True;TrustServerCertificate=True";

            string query = @"
        SELECT
            weight_in,
            weight_out,
            weight_total,
            data_out,
            data_in,
            card,
            sort,
            id,
            post,
            carnumber,
            upd
        FROM dbo.gates
        ORDER BY [data_out]
        OFFSET @PageIndex * @PageSize ROWS
        FETCH NEXT @PageSize ROWS ONLY;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);

                    SqlDataReader reader = cmd.ExecuteReader();

                    listView.Items.Clear();

                    while (reader.Read())
                    {

                        string dataOut = reader["data_out"] is DBNull
                         ? ""
                         : Convert.ToDateTime(reader["data_out"]).ToString("dd.MM.yyyy HH:mm");
                        string dataIn = reader["data_in"] == DBNull.Value
                            ? ""
                            : Convert.ToDateTime(reader["data_in"]).ToString("dd.MM.yyyy HH:mm");

                        ListViewItem item = new ListViewItem(reader["weight_in"].ToString());
                        item.SubItems.Add(reader["weight_out"].ToString());
                        item.SubItems.Add(reader["weight_total"].ToString());
                        item.SubItems.Add(dataOut);
                        item.SubItems.Add(dataIn);
                        item.SubItems.Add(reader["card"].ToString());
                        item.SubItems.Add(reader["sort"].ToString());
                        item.SubItems.Add(reader["id"].ToString());
                        item.SubItems.Add(reader["post"].ToString());
                        item.SubItems.Add(reader["carnumber"].ToString());
                        item.SubItems.Add(reader["upd"] == DBNull.Value ? "" : reader["upd"].ToString());

                        listView.Items.Add(item);
                    }

                }
            }
        }



        private void btnExport_Click(object sender, EventArgs e)
        {

            ListView listView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                CheckBoxes = true,
                BorderStyle = BorderStyle.None

            };



            listView.Columns.Add("Giriş çəkisi", 120);
            listView.Columns.Add("Çıxış çəkisi", 120);
            listView.Columns.Add("Ümumi çəki", 150);
            listView.Columns.Add("Giriş tarixi", 150);
            listView.Columns.Add("Çıxış tarixi", 150);
            listView.Columns.Add("Kart", 100);
            listView.Columns.Add("Grade", 150);
            listView.Columns.Add("Post", 150);
            listView.Columns.Add("Maşın nömrəsi", 150);

            listView.OwnerDraw = true;

            listView.DrawColumnHeader += (s, args) =>
            {
                using (Font f = new Font("Segoe UI", 8, FontStyle.Bold))
                using (StringFormat sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(243, 244, 246)))
                {
                    args.Graphics.FillRectangle(brush, args.Bounds);
                    args.Graphics.DrawRectangle(Pens.LightGray, args.Bounds);
                    args.Graphics.DrawString(args.Header.Text, f, Brushes.Black, args.Bounds, sf);
                }
                listView.CheckBoxes = true;
            };

            listView.DrawItem += (s, args) => args.DrawDefault = true;
            listView.DrawSubItem += (s, args) => args.DrawDefault = true;

            scaleInfoContent.Controls.Add(listView);
            string connectionString = "Data Source=DESKTOP-IQB2C7N\\SQLEXPRESS;Initial Catalog=erp_azmaind;User ID=sa;Password=Scale123+-;Encrypt=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "select*from dbo.gates";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string giris = reader["weight_in"].ToString();
                    string cixis = reader["weight_out"].ToString();
                    string umumiceki = reader["weight_total"].ToString();

                    DateTime girisTarixi = Convert.ToDateTime(reader["data_in"]);
                    DateTime cixisTarixi = Convert.ToDateTime(reader["data_out"]);

                    string kart = reader["card"].ToString();
                    string grade = reader["sort"].ToString();
                    string post = reader["post"].ToString();
                    string masin = reader["carnumber"].ToString();

                    ListViewItem item = new ListViewItem(giris);
                    item.SubItems.Add(cixis);
                    item.SubItems.Add(umumiceki);
                    item.SubItems.Add(girisTarixi.ToString("dd.MM.yyyy HH:mm"));
                    item.SubItems.Add(cixisTarixi.ToString("dd.MM.yyyy HH:mm"));
                    item.SubItems.Add(kart);
                    item.SubItems.Add(grade);
                    item.SubItems.Add(post);
                    item.SubItems.Add(masin);

                    listView.Items.Add(item);
                }

                reader.Close();
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog.Title = "CSV faylını saxla";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8))
                {

                    for (int i = 0; i < listView.Columns.Count; i++)
                    {
                        sw.Write(listView.Columns[i].Text);
                        if (i < listView.Columns.Count - 1) sw.Write(",");
                    }
                    sw.WriteLine();


                    foreach (ListViewItem item in listView.Items)
                    {
                        for (int i = 0; i < item.SubItems.Count; i++)
                        {
                            sw.Write(item.SubItems[i].Text);
                            if (i < item.SubItems.Count - 1) sw.Write(",");
                        }
                        sw.WriteLine();
                    }
                }

                MessageBox.Show("Məlumat uğurla CSV-ə ixrac edildi!");
            }
        }






        private void button4_Click(object sender, EventArgs e)
        {
            button1.BackColor = Color.Black;
            button1.ForeColor = Color.White;


            button2.BackColor = Color.Black;
            button2.ForeColor = Color.White;

            button3.BackColor = Color.Black;
            button3.ForeColor = Color.White;

            button4.BackColor = Color.White;
            button4.ForeColor = Color.Orange;

            button5.BackColor = Color.Black;
            button5.ForeColor = Color.White;

            button6.BackColor = Color.Black;
            button6.ForeColor = Color.White;

            button7.BackColor = Color.Black;
            button7.ForeColor = Color.White;

            scaleInfoContent.Controls.Clear();

            TableLayoutPanel mainLayout = new TableLayoutPanel();
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.RowCount = 3;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));

            Panel topPanel = new Panel { Dock = DockStyle.Fill };

            TextBox txtSearch = new TextBox
            {
                Location = new Point(10, 8),
                Width = 200
            };

            Button btnExport = new Button();
            btnExport.Text = " File export";
            btnExport.Image = Image.FromFile("C:\\Users\\Akbar\\Documents\\pictures\\images (5).png");
            Image originalImage = btnExport.Image;
            Image resizedImage = new Bitmap(originalImage, new Size(originalImage.Width / 18, originalImage.Height / 18));  // Burada şəkili yarıya endiririk
            btnExport.Image = resizedImage;
            btnExport.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnExport.Location = new Point(1100, 5);
            btnExport.Width = 150;
            btnExport.Height = 30;

            btnExport.FlatStyle = FlatStyle.Flat;

            btnExport.Font = new Font("Arial", 10);

            btnExport.FlatAppearance.BorderSize = 0;
            scaleInfoContent.Controls.Add(btnExport);
            scaleInfoContent.Controls.Clear();
            TextBox searchBox = new TextBox();
            searchBox.Size = new Size(130, 20);
            searchBox.BorderStyle = BorderStyle.None;
            searchBox.Location = new Point(scaleInfoContent.Width - 1240, 5);
            searchBox.Text = "Axtarış edin";
            searchBox.ForeColor = Color.Gray;


            searchBox.Enter += (s, ev) =>
            {
                if (searchBox.Text == "Axtarış edin")
                {
                    searchBox.Text = "";
                    searchBox.ForeColor = Color.Black;
                }
            };

            searchBox.Leave += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(searchBox.Text))
                {
                    searchBox.Text = "Axtarış edin";
                    searchBox.ForeColor = Color.Gray;
                }
            };

            scaleInfoContent.Controls.Add(searchBox);
            searchBox.TextChanged += new EventHandler(SearchBox_TextChanged);

            PictureBox searchIcon = new PictureBox();
            searchIcon.Image = Image.FromFile("C:\\Users\\Akbar\\Documents\\pictures\\search-icon-2-614x460.png");
            searchIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            searchIcon.Size = new Size(30, 30);
            searchIcon.Location = new Point(searchBox.Location.X + searchBox.Width - 2, searchBox.Location.Y - 8);

            searchIcon.Click += (s, ev) =>
            {
                MessageBox.Show("Axtarış etmək üçün simgeyə basıldı!");
            };

            scaleInfoContent.Controls.Add(searchIcon);
            topPanel.Controls.Add(btnExport);
            scaleInfoContent.Controls.Add(btnExport);

            listView = new ListView();
            listView.View = View.Details;
            listView.FullRowSelect = true;
            listView.GridLines = true;
            listView.Size = new Size(1460, 820);
            listView.Location = new Point(10, 40);
            listView.CheckBoxes = true;
            listView.BorderStyle = BorderStyle.None;

            listView.Columns.Add("Giriş çəkisi", 120);
            listView.Columns.Add("Çıxış çəkisi", 120);
            listView.Columns.Add("Ümumi çəki", 150);
            listView.Columns.Add("Giriş tarixi", 150);
            listView.Columns.Add("Çıxış tarixi", 150);
            listView.Columns.Add("Kart", 100);
            listView.Columns.Add("Grade", 150);
            listView.Columns.Add("Post", 150);
            listView.Columns.Add("Maşın nömrəsi", 150);
            listView.OwnerDraw = true;

            listView.DrawColumnHeader += (s, args) =>
            {
                using (Font f = new Font("Segoe UI", 8, FontStyle.Bold))
                using (StringFormat sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(243, 244, 246)))
                {
                    args.Graphics.FillRectangle(brush, args.Bounds);
                    args.Graphics.DrawRectangle(Pens.LightGray, args.Bounds);
                    args.Graphics.DrawString(args.Header.Text, f, Brushes.Black, args.Bounds, sf);
                }
                listView.CheckBoxes = true;
            };


            listView.DrawItem += (s, args) => args.DrawDefault = true;
            listView.DrawSubItem += (s, args) => args.DrawDefault = true;

            scaleInfoContent.Controls.Add(listView);

            string connectionString = "Data Source=DESKTOP-IQB2C7N\\SQLEXPRESS;Initial Catalog=erp_azmaind;User ID=sa;Password=Scale123+-;Encrypt=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "select COUNT(*) count from gates";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    totalCount2 = int.Parse(reader["count"].ToString());
                }

                reader.Close();
            }


            LoadData2(1, pageSize);
            Panel bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40
            };

            Label lblCount = new Label
            {
                Text = "Sətir sayı: " + listView.Items.Count,
                Location = new Point(12, -1),
                AutoSize = true
            };

            int PageCount2 = (int)Math.Ceiling((double)totalCount2 / pageSize);

            for (int i = 1; i <= PageCount2; i++)
            {
                Button btnPage = new Button { Text = i.ToString(), Location = new Point(450 + i * 35, -1), Width = 20 };
                btnPage.Click += new EventHandler(btnPage3_Click);

                btnPage.FlatStyle = FlatStyle.Flat;
                btnPage.FlatAppearance.BorderSize = 0;
                bottomPanel.Controls.Add(btnPage);
                pageButtons2.Add(btnPage);

            }

            Button btnPrev = new Button { Text = "<", Location = new Point(450, -1), Width = 20 };
            btnPrev.Click += new EventHandler(btnPrev2_Click);
            btnPrev.FlatStyle = FlatStyle.Flat;
            btnPrev.FlatAppearance.BorderSize = 0;
            bottomPanel.Controls.Add(btnPrev);

            Button btnNext = new Button { Text = ">", Location = new Point(450 + (PageCount2 + 1) * 35, -1), Width = 20 };
            btnNext.Click += new EventHandler(btnNext2_Click);
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.FlatAppearance.BorderSize = 0;
            bottomPanel.Controls.Add(btnNext);





            Label lblSehife = new Label
            {

                Location = new Point(1000, -1),
                AutoSize = true
            };

            lblSehife.Text = PageCount2.ToString() + " " + "Səhifə";

            Label lblSehifeyekecid = new Label
            {
                Text = "Səhifəyə keç: ",
                Location = new Point(1100, -1),
                AutoSize = true
            };

            bottomPanel.Controls.Add(lblCount);

            bottomPanel.Controls.Add(lblSehife);
            bottomPanel.Controls.Add(lblSehifeyekecid);
            bottomPanel.Controls.Add(this.lblSehifeyekecidPage2);

            this.Controls.Add(bottomPanel);

            mainLayout.Controls.Add(topPanel, 0, 0);
            mainLayout.Controls.Add(listView, 0, 1);
            mainLayout.Controls.Add(bottomPanel, 0, 2);

            scaleInfoContent.Controls.Add(mainLayout);
        }

        private void lblSehifeyekecidPage2_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(lblSehifeyekecidPage1.Text, out int pageNumber) && pageNumber > 0)
            {

                currentPage = pageNumber - 1;


                LoadData2(currentPage, pageSize);

            }
        }


        private void btnNext2_Click(object sender, EventArgs e)
        {
            LoadData2(currentPage + 1, pageSize);
            UpdatePageButtonStyles(sender as Button);
        }

        private void btnPrev2_Click(object sender, EventArgs e)
        {
            LoadData2(currentPage - 1, pageSize);
            UpdatePageButtonStyles(sender as Button);
        }

        private void btnPage3_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string btnText = btn.Text;



            currentPage = int.Parse(btnText);
            LoadData2(currentPage, pageSize);
            UpdatePageButtonStyles2(btn);
        }

        private void LoadData2(int pageIndex, int pageSize)
        {

            string connectionString =
                "Data Source=DESKTOP-IQB2C7N\\SQLEXPRESS;Initial Catalog=erp_azmaind;User ID=sa;Password=Scale123+-;Encrypt=True;TrustServerCertificate=True";

            string query = @"
        SELECT
            weight_in,
            weight_out,
            weight_total,
            data_out,
            data_in,
            card,
            sort,
            id,
            post,
            carnumber,
            upd
        FROM dbo.gates
        ORDER BY data_out
        OFFSET @PageIndex * @PageSize ROWS
        FETCH NEXT @PageSize ROWS ONLY;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        listView.Items.Clear();

                        while (reader.Read())
                        {

                            string dataOut = reader["data_out"] is DBNull
                             ? ""
                             : Convert.ToDateTime(reader["data_out"]).ToString("dd.MM.yyyy HH:mm");
                            string dataIn = reader["data_in"] == DBNull.Value
                                ? ""
                                : Convert.ToDateTime(reader["data_in"]).ToString("dd.MM.yyyy HH:mm");

                            ListViewItem item = new ListViewItem(reader["weight_in"].ToString());
                            item.SubItems.Add(reader["weight_out"].ToString());
                            item.SubItems.Add(reader["weight_total"].ToString());
                            item.SubItems.Add(dataOut);
                            item.SubItems.Add(dataIn);
                            item.SubItems.Add(reader["card"].ToString());
                            item.SubItems.Add(reader["sort"].ToString());
                            item.SubItems.Add(reader["id"].ToString());
                            item.SubItems.Add(reader["post"].ToString());
                            item.SubItems.Add(reader["carnumber"].ToString());
                            item.SubItems.Add(reader["upd"] == DBNull.Value ? "" : reader["upd"].ToString());

                            listView.Items.Add(item);
                        }
                    }
                }
            }
        }

        public void btnExport1_Click(object sender, EventArgs e)
        {

            ListView listView = new ListView();
            listView.Dock = DockStyle.Fill;
            listView.View = View.Details;
            listView.FullRowSelect = true;
            listView.GridLines = true;
            listView.CheckBoxes = true;
            listView.BorderStyle = BorderStyle.None;


            listView.Columns.Add("ID", 120);
            listView.Columns.Add("Kartlar", 140);
            listView.Columns.Add("Sürücü adı", 150);
            listView.Columns.Add("Son Sürücü", 150);
            listView.Columns.Add("Avto nömrə", 150);
            listView.Columns.Add("Avto model", 150);
            listView.Columns.Add("Avto Şirkət", 140);
            listView.Columns.Add("Avto status", 100);

            listView.Columns.Add("Xammal ", 130);
            listView.Columns.Add("Upd", 130);



            listView.OwnerDraw = true;


            listView.DrawColumnHeader += (s, args) =>
            {
                using (Font f = new Font("Segoe UI", 8, FontStyle.Bold))
                using (StringFormat sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(243, 244, 246)))
                {
                    args.Graphics.FillRectangle(brush, args.Bounds);
                    args.Graphics.DrawRectangle(Pens.LightGray, args.Bounds);
                    args.Graphics.DrawString(args.Header.Text, f, Brushes.Black, args.Bounds, sf);
                }
                listView.CheckBoxes = true;
            };


            listView.DrawItem += (s, args) => args.DrawDefault = true;
            listView.DrawSubItem += (s, args) => args.DrawDefault = true;


            scaleInfoContent.Controls.Add(listView);

            string connectionString = "Data Source=DESKTOP-IQB2C7N\\SQLEXPRESS;Initial Catalog=erp_azmaind;User ID=sa;Password=Scale123+-;Encrypt=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM dbo.cards";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string id = reader["ID"].ToString();
                    string card = reader["cards"].ToString();
                    string driverName = reader["drivername"].ToString();
                    string driverLast = reader["driverlast"].ToString();
                    string carNumber = reader["carnumber"].ToString();
                    string carModel = reader["carmodel"].ToString();
                    string carCompany = reader["carcompany"].ToString();
                    string carStatus = reader["carstatus"].ToString();
                    string desk = reader["desk"].ToString();
                    string upd = reader["upd"].ToString();


                    ListViewItem item = new ListViewItem(id);
                    item.SubItems.Add(card);
                    item.SubItems.Add(driverName);
                    item.SubItems.Add(driverLast);
                    item.SubItems.Add(carNumber);
                    item.SubItems.Add(carModel);
                    item.SubItems.Add(carCompany);
                    item.SubItems.Add(carStatus);
                    item.SubItems.Add(desk);
                    item.SubItems.Add(upd);

                    listView.Items.Add(item);
                }
                reader.Close();
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog.Title = "CSV faylını saxla";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8))
                {

                    for (int i = 0; i < listView.Columns.Count; i++)
                    {
                        sw.Write(listView.Columns[i].Text);
                        if (i < listView.Columns.Count - 1) sw.Write(",");
                    }
                    sw.WriteLine();


                    foreach (ListViewItem item in listView.Items)
                    {
                        for (int i = 0; i < item.SubItems.Count; i++)
                        {
                            sw.Write(item.SubItems[i].Text);
                            if (i < item.SubItems.Count - 1) sw.Write(",");
                        }
                        sw.WriteLine();
                    }
                }

                MessageBox.Show("Məlumat uğurla CSV-ə ixrac edildi!");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button1.BackColor = Color.Black;
            button1.ForeColor = Color.White;


            button2.BackColor = Color.Black;
            button2.ForeColor = Color.White;

            button3.BackColor = Color.Black;
            button3.ForeColor = Color.White;

            button4.BackColor = Color.Black;
            button4.ForeColor = Color.White;

            button5.BackColor = Color.Black;
            button5.ForeColor = Color.White;

            button6.BackColor = Color.Black;
            button6.ForeColor = Color.White;

            button7.BackColor = Color.White;
            button7.ForeColor = Color.Orange;

            scaleInfoContent.Controls.Clear();
            scaleInfoContent.AutoScroll = true;

            Panel topPanel = new Panel();
            topPanel.Dock = DockStyle.Top;
            topPanel.Height = 50;

            DateTimePicker dtpStartDate = new DateTimePicker();
            dtpStartDate.Format = DateTimePickerFormat.Short;
            dtpStartDate.Size = new Size(150, 32);
            dtpStartDate.Location = new Point(120, 0);
            dtpStartDate.Value = DateTime.Now;

            scaleInfoContent.Controls.Add(dtpStartDate);

            Label lblStartDate = new Label();
            lblStartDate.Text = "Birinci çəki tarix";
            lblStartDate.Size = new Size(150, 32);
            lblStartDate.Location = new Point(30, 0);
            scaleInfoContent.Controls.Add(lblStartDate);

            DateTimePicker dtpEndDate = new DateTimePicker();
            dtpEndDate.Format = DateTimePickerFormat.Short;
            dtpEndDate.Size = new Size(150, 32);
            dtpEndDate.Location = new Point(360, 0);
            dtpEndDate.Value = DateTime.Now.AddDays(7);
            scaleInfoContent.Controls.Add(dtpEndDate);


            Label lblendDate = new Label();
            lblendDate.Text = "İkinci çəki tarixi";
            lblendDate.Size = new Size(150, 32);
            lblendDate.Location = new Point(280, 0);
            scaleInfoContent.Controls.Add(lblendDate);


            Button btnExport1 = new Button();
            btnExport1.Text = " Export";
            btnExport1.Image = Image.FromFile("C:\\Users\\Akbar\\Documents\\pictures\\images (5).png");
            Image originalImage = btnExport1.Image;
            Image resizedImage = new Bitmap(originalImage, new Size(originalImage.Width / 18, originalImage.Height / 18));  // Burada şəkili yarıya endiririk
            btnExport1.Image = resizedImage;
            btnExport1.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnExport1.Location = new Point(850, 0);
            btnExport1.Width = 100;
            btnExport1.Height = 30;
            btnExport1.Click += new EventHandler(btnExport1_Click);

            btnExport1.FlatStyle = FlatStyle.Flat;

            btnExport1.Font = new Font("Arial", 10);
            btnExport1.FlatAppearance.BorderSize = 0;
            scaleInfoContent.Controls.Add(btnExport1);

            Button btnDelete2 = new Button()
            {
                Text = "Reysi silmək",
                Location = new Point(950, 0),
                Width = 100,
                Height = 30,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 11)

            };
            btnDelete2.FlatAppearance.BorderSize = 0;

            btnDelete2.Click += new EventHandler(btnDelete2_Click);

            Button btnEdit = new Button()
            {
                Text = "Dəyişiklik",
                Location = new Point(1050, 0),
                Width = 100,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 11),
                Height = 30,
            };
            btnEdit.FlatAppearance.BorderSize = 0;

            Button btnNew4 = new Button()
            {
                Text = "+ Yeni reys",
                Location = new Point(1150, 0),
                BackColor = Color.FromArgb(229, 199, 76),
                Width = 100,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 11),
                ForeColor = Color.White,
                Height = 30,

            };
            btnNew4.FlatAppearance.BorderSize = 0;

            btnNew4.Click += new EventHandler(btnNew4_Click);

            topPanel.Controls.Add(btnExport1);
            topPanel.Controls.Add(btnDelete2);
            topPanel.Controls.Add(btnEdit);
            topPanel.Controls.Add(btnNew4);

            Panel statsPanel = new Panel();
            statsPanel.Dock = DockStyle.Top;
            statsPanel.Height = 200;

            void AddStat(string title, string value)
            {
                Label lbl = new Label();
                lbl.Text = title;
                lbl.AutoSize = false;
                lbl.Font = new Font("Segoe UI", 9, FontStyle.Regular);

                TextBox txt = new TextBox();
                txt.Text = value;
                txt.ReadOnly = true;
                txt.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                txt.BackColor = Color.White;


                int defaultWidth = 290;
                int wideWidth = 310;

                int lblHeight = 20;
                int txtHeight = 25;


                bool isWide = (title == "Waste reyslərin sayı" ||
                               title == "Waste çəkisi, ton" ||
                               title == "Tamamlanmamış reyslərin sayı");

                if (isWide)
                {
                    lbl.Width = wideWidth;
                    txt.Width = wideWidth;
                }
                else
                {
                    lbl.Width = defaultWidth;
                    txt.Width = defaultWidth;
                }

                lbl.Height = lblHeight;
                txt.Height = txtHeight;

                int columnCount = 4;
                int colWidth = 320;
                int rowHeight = 50;
                int index = statsPanel.Controls.Count / 2;

                int col = index % columnCount;
                int row = index / columnCount;

                lbl.Location = new Point(10 + col * colWidth, 10 + row * rowHeight);
                txt.Location = new Point(10 + col * colWidth, 30 + row * rowHeight);

                statsPanel.Controls.Add(lbl);
                statsPanel.Controls.Add(txt);
            }

            AddStat("Yerinə yetirilən reyslərin sayı", "33242");
            AddStat("Ümumi daşınan yükün çəkisi, ton", "24245");
            AddStat("High quality gold reyslərin sayı", "786");
            AddStat("High quality gold çəkisi, ton", "342");
            AddStat("Medium quality gold reyslərin sayı", "3425");
            AddStat("Medium quality gold çəkisi, ton", "2452");
            AddStat("Low quality gold reyslərin sayı", "3425");
            AddStat("Low quality gold çəkisi, ton", "2452");
            AddStat("Waste reyslərin sayı", "2452");
            AddStat("Waste çəkisi, ton", "2452");
            AddStat("Tamamlanmamış reyslərin sayı", "2452");

            ListView listView = new ListView();
            listView.Dock = DockStyle.Fill;
            listView.View = View.Details;
            listView.FullRowSelect = true;
            listView.GridLines = true;
            listView.CheckBoxes = true;
            listView.BorderStyle = BorderStyle.None;


            listView.Columns.Add("ID", 100);
            listView.Columns.Add("Kartlar", 130);
            listView.Columns.Add("Sürücü adı", 130);
            listView.Columns.Add("Son Sürücü", 130);
            listView.Columns.Add("Avto nömrə", 130);
            listView.Columns.Add("Avto model", 130);
            listView.Columns.Add("Avto Şirkət", 130);
            listView.Columns.Add("Avto status", 100);

            listView.Columns.Add("Xammal ", 130);
            listView.Columns.Add("Upd", 140);


            listView.OwnerDraw = true;


            listView.DrawColumnHeader += (s, args) =>
            {
                using (Font f = new Font("Segoe UI", 8, FontStyle.Bold))
                using (StringFormat sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(243, 244, 246)))
                {
                    args.Graphics.FillRectangle(brush, args.Bounds);
                    args.Graphics.DrawRectangle(Pens.LightGray, args.Bounds);
                    args.Graphics.DrawString(args.Header.Text, f, Brushes.Black, args.Bounds, sf);
                }
                listView.CheckBoxes = true;
            };


            listView.DrawItem += (s, args) => args.DrawDefault = true;
            listView.DrawSubItem += (s, args) => args.DrawDefault = true;


            scaleInfoContent.Controls.Add(listView);

            string connectionString = "Data Source=DESKTOP-IQB2C7N\\SQLEXPRESS;Initial Catalog=erp_azmaind;User ID=sa;Password=Scale123+-;Encrypt=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM dbo.cards";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string id = reader["ID"].ToString();
                    string card = reader["cards"].ToString();
                    string driverName = reader["drivername"].ToString();
                    string driverLast = reader["driverlast"].ToString();
                    string carNumber = reader["carnumber"].ToString();
                    string carModel = reader["carmodel"].ToString();
                    string carCompany = reader["carcompany"].ToString();
                    string carStatus = reader["carstatus"].ToString();
                    string desk = reader["desk"].ToString();
                    string upd = reader["upd"].ToString();


                    ListViewItem item = new ListViewItem(id);
                    item.SubItems.Add(card);
                    item.SubItems.Add(driverName);
                    item.SubItems.Add(driverLast);
                    item.SubItems.Add(carNumber);
                    item.SubItems.Add(carModel);
                    item.SubItems.Add(carCompany);
                    item.SubItems.Add(carStatus);
                    item.SubItems.Add(desk);
                    item.SubItems.Add(upd);

                    listView.Items.Add(item);
                }
                reader.Close();
            }

            listView.DoubleClick += ListView_DoubleClick;

            scaleInfoContent.Controls.Add(listView);
            scaleInfoContent.Controls.Add(statsPanel);
            scaleInfoContent.Controls.Add(topPanel);
        }
        private void btnDelete2_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            ListViewItem item = clickedButton.Tag as ListViewItem;


            DialogResult result = MessageBox.Show("Are you sure you want to delete this item?", "Confirm Deletion", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {

                listView.Items.Remove(item);


                DeleteFromDatabase(item.SubItems[0].Text);

                MessageBox.Show("Item deleted.");
            }
            else
            {
                MessageBox.Show("Deletion cancelled.");
            }

        }

        private void DeleteFromDatabase(string id)
        {
            string connectionString = "Data Source=DESKTOP-IQB2C7N\\SQLEXPRESS;Initial Catalog=erp_azmaind;User ID=sa;Password=Scale123++;Encrypt=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                foreach (ListViewItem item in listView.CheckedItems)
                {
                    string userId = item.Text;
                    string query = "DELETE FROM  WHERE Id = @id";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.ExecuteNonQuery();
                    }
                    listView.Items.Remove(item);
                }

            }
        }

        private void ListView_DoubleClick(object sender, EventArgs e)
        {
            var lv = sender as ListView;

            if (lv != null && lv.SelectedItems.Count > 0)
            {

                ListViewItem item = lv.SelectedItems[0];

                string id = item.SubItems[0].Text;
                string card = item.SubItems[1].Text;
                string driverName = item.SubItems[2].Text;
                string driverLast = item.SubItems[3].Text;
                string carNumber = item.SubItems[4].Text;
                string carModel = item.SubItems[5].Text;
                string carCompany = item.SubItems[6].Text;
                string carStatus = item.SubItems[7].Text;
                string desk = item.SubItems[8].Text;

                EditForm editForm = new EditForm(card, driverName, driverLast, carNumber,
                                            carModel, carCompany, carStatus, desk);

                editForm.ShowDialog();

            }
        }

        private void btnNew4_Click(object sender, EventArgs e)
        {
            Yeni_reys yeni_Reys = new Yeni_reys(this);
            yeni_Reys.ShowDialog();
        }

        internal void AddUserToListView(string userName, string creationDate)
        {
            throw new NotImplementedException();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button1.BackColor = Color.Black;
            button1.ForeColor = Color.White;


            button2.BackColor = Color.Black;
            button2.ForeColor = Color.White;

            button3.BackColor = Color.Black;
            button3.ForeColor = Color.White;

            button4.BackColor = Color.Black;
            button4.ForeColor = Color.White;

            button5.BackColor = Color.White;
            button5.ForeColor = Color.Orange;

            button6.BackColor = Color.Black;
            button6.ForeColor = Color.White;

            button7.BackColor = Color.Black;
            button7.ForeColor = Color.White;


            scaleInfoContent.Controls.Clear();

            TableLayoutPanel mainLayout = new TableLayoutPanel();
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.RowCount = 3;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));


            Panel topPanel = new Panel { Dock = DockStyle.Fill };

            TextBox txtSearch = new TextBox
            {

                Location = new Point(10, 8),

                Width = 200
            };

            scaleInfoContent.Controls.Clear();
            TextBox searchBox = new TextBox();
            searchBox.Size = new Size(130, 20);
            searchBox.BorderStyle = BorderStyle.None;
            searchBox.Location = new Point(scaleInfoContent.Width - 1240, 5);
            searchBox.Text = "Axtarış edin";
            searchBox.ForeColor = Color.Gray;

            searchBox.Enter += (s, ev) =>
            {
                if (searchBox.Text == "Axtarış edin")
                {
                    searchBox.Text = "";
                    searchBox.ForeColor = Color.Black;
                }
            };

            searchBox.Leave += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(searchBox.Text))
                {
                    searchBox.Text = "Axtarış edin";
                    searchBox.ForeColor = Color.Gray;
                }
            };

            scaleInfoContent.Controls.Add(searchBox);
            searchBox.TextChanged += new EventHandler(SearchBox_TextChanged);

            PictureBox searchIcon = new PictureBox();
            searchIcon.Image = Image.FromFile("C:\\Users\\Akbar\\Documents\\pictures\\search-icon-2-614x460.png");
            searchIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            searchIcon.Size = new Size(30, 30);
            searchIcon.Location = new Point(searchBox.Location.X + searchBox.Width - 2, searchBox.Location.Y - 8);

            searchIcon.Click += (s, ev) =>
            {
                MessageBox.Show("Axtarış etmək üçün simgeyə basıldı!");
            };

            scaleInfoContent.Controls.Add(searchIcon);

            Button btnDeleteKart = new Button();
            btnDeleteKart.Text = "Kartı sil";
            btnDeleteKart.Size = new Size(90, 32);
            btnDeleteKart.Location = new Point(1060, -1);
            btnDeleteKart.FlatStyle = FlatStyle.Flat;
            btnDeleteKart.FlatAppearance.BorderSize = 0;
            btnDeleteKart.BackColor = Color.White;
            btnDeleteKart.Click += new EventHandler(btnDeleteKart_Click);
            scaleInfoContent.Controls.Add(btnDeleteKart);

            Button btnNewMenu = new Button();
            btnNewMenu.Text = "+ Yeni kart";
            btnNewMenu.Size = new Size(90, 32);
            btnNewMenu.Location = new Point(1165, -1);
            btnNewMenu.FlatStyle = FlatStyle.Flat;
            btnNewMenu.FlatAppearance.BorderSize = 0;
            btnNewMenu.BackColor = Color.FromArgb(223, 199, 78);
            btnNewMenu.Click += new EventHandler(btnNewMenu_Click);
            btnNewMenu.ForeColor = Color.White;
            scaleInfoContent.Controls.Add(btnNewMenu);

            listView = new ListView();
            listView.View = View.Details;
            listView.FullRowSelect = true;
            listView.GridLines = true;
            listView.Size = new Size(1460, 820);
            listView.Location = new Point(10, 40);
            listView.CheckBoxes = true;
            listView.BorderStyle = BorderStyle.None;


            listView.Columns.Add("ID", 100);
            listView.Columns.Add("Kartlar", 130);
            listView.Columns.Add("Sürücü adı", 130);
            listView.Columns.Add("Son Sürücü", 130);
            listView.Columns.Add("Avto nömrə", 130);
            listView.Columns.Add("Avto model", 130);
            listView.Columns.Add("Avto Şirkət", 130);
            listView.Columns.Add("Avto status", 100);

            listView.Columns.Add("Xammal ", 130);
            listView.Columns.Add("Upd", 140);



            listView.OwnerDraw = true;


            listView.DrawColumnHeader += (s, args) =>
            {
                using (Font f = new Font("Segoe UI", 8, FontStyle.Bold))
                using (StringFormat sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(243, 244, 246)))
                {
                    args.Graphics.FillRectangle(brush, args.Bounds);
                    args.Graphics.DrawRectangle(Pens.LightGray, args.Bounds);
                    args.Graphics.DrawString(args.Header.Text, f, Brushes.Black, args.Bounds, sf);
                }
                listView.CheckBoxes = true;
            };


            listView.DrawItem += (s, args) => args.DrawDefault = true;
            listView.DrawSubItem += (s, args) => args.DrawDefault = true;

            scaleInfoContent.Controls.Add(listView);

            string connectionString = "Data Source=DESKTOP-IQB2C7N\\SQLEXPRESS;Initial Catalog=erp_azmaind;User ID=sa;Password=Scale123+-;Encrypt=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "select COUNT(*) count from cards";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    totalCount3 = int.Parse(reader["count"].ToString());
                }

                reader.Close();
            }

            Panel bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40
            };

            Label lblCount = new Label
            {
                Text = "Sətir sayı: " + listView.Items.Count,
                Location = new Point(12, -1),
                AutoSize = true
            };

            LoadData3(1, pageSize);
            int PageCount3 = (int)Math.Ceiling((double)totalCount3 / pageSize);

            for (int i = 1; i <= PageCount3; i++)
            {
                Button btnPage = new Button { Text = i.ToString(), Location = new Point(450 + i * 35, -1), Width = 20 };
                btnPage.Click += new EventHandler(btnPage4_Click);

                btnPage.FlatStyle = FlatStyle.Flat;
                btnPage.FlatAppearance.BorderSize = 0;
                bottomPanel.Controls.Add(btnPage);
                pageButtons3.Add(btnPage);

            }

            Button btnPrev = new Button { Text = "<", Location = new Point(450, -1), Width = 20 };
            btnPrev.Click += new EventHandler(btnPrev_Click);
            btnPrev.FlatStyle = FlatStyle.Flat;
            btnPrev.FlatAppearance.BorderSize = 0;
            bottomPanel.Controls.Add(btnPrev);

            Button btnNext = new Button { Text = ">", Location = new Point(450 + (PageCount3 + 1) * 35, -1), Width = 20 };
            btnNext.Click += new EventHandler(btnNext_Click);
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.FlatAppearance.BorderSize = 0;
            bottomPanel.Controls.Add(btnNext);








            Label lblSehife = new Label
            {

                Location = new Point(1000, -1),
                AutoSize = true
            };
            lblSehife.Text = PageCount3.ToString() + " " + "Səhifə";

            Label lblSehifeyekecid = new Label
            {
                Text = "Səhifəyə keç: ",
                Location = new Point(1100, -1),
                AutoSize = true
            };




            bottomPanel.Controls.Add(lblCount);

            bottomPanel.Controls.Add(lblSehife);
            bottomPanel.Controls.Add(lblSehifeyekecid);
            bottomPanel.Controls.Add(this.lblSehifeyekecidPage3);

            this.Controls.Add(bottomPanel);

            mainLayout.Controls.Add(topPanel, 0, 0);
            mainLayout.Controls.Add(listView, 0, 1);
            mainLayout.Controls.Add(bottomPanel, 0, 2);

            scaleInfoContent.Controls.Add(mainLayout);

        }

        private void lblSehifeyekecidPage3_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(lblSehifeyekecidPage3.Text, out int pageNumber) && pageNumber > 0)
            {

                currentPage = pageNumber - 1;


                LoadData3(currentPage, pageSize);

            }

        }

        private void btnPage4_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string btnText = btn.Text;



            currentPage = int.Parse(btnText);
            LoadData3(currentPage, pageSize);
            UpdatePageButtonStyles3(btn);

        }

        private void LoadData3(int pageIndex, int pageSize)
        {
            string connectionString =
                "Data Source=DESKTOP-IQB2C7N\\SQLEXPRESS;Initial Catalog=erp_azmaind;User ID=sa;Password=Scale123+-;Encrypt=True;TrustServerCertificate=True";

            string query = @"
SELECT
    id,
    cards,
    drivername,
    driverlast,
    carnumber,
    carmodel,
    carcompany,
    carstatus,
    desk,
    upd
FROM dbo.cards
ORDER BY id DESC
OFFSET (@PageIndex-1) * @PageSize ROWS
FETCH NEXT @PageSize ROWS ONLY;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {

                cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    listView.Items.Clear();


                    string S(IDataRecord r, string name) =>
                        r[name] == DBNull.Value ? "" : r[name].ToString();

                    while (reader.Read())
                    {

                        ListViewItem item = new ListViewItem(S(reader, "id"));


                        item.SubItems.Add(S(reader, "cards"));
                        item.SubItems.Add(S(reader, "drivername"));
                        item.SubItems.Add(S(reader, "driverlast"));
                        item.SubItems.Add(S(reader, "carnumber"));
                        item.SubItems.Add(S(reader, "carmodel"));
                        item.SubItems.Add(S(reader, "carcompany"));
                        item.SubItems.Add(S(reader, "carstatus"));
                        item.SubItems.Add(S(reader, "desk"));
                        item.SubItems.Add(S(reader, "upd"));

                        listView.Items.Add(item);
                    }
                }
            }

            listView.DoubleClick += ListView1_DoubleClick;
        }

        private void ListView1_DoubleClick(object sender, EventArgs e)
        {
            var lv = sender as ListView;

            if (lv != null && lv.SelectedItems.Count > 0)
            {

                ListViewItem item = lv.SelectedItems[0];

                string id = item.SubItems[0].Text;
                string card = item.SubItems[1].Text;
                string driverName = item.SubItems[2].Text;
                string driverLast = item.SubItems[3].Text;
                string carNumber = item.SubItems[4].Text;
                string carModel = item.SubItems[5].Text;
                string carCompany = item.SubItems[6].Text;
                string carStatus = item.SubItems[7].Text;
                string desk = item.SubItems[8].Text;

                EditForm editForm = new EditForm(card, driverName, driverLast, carNumber,
                                            carModel, carCompany, carStatus, desk);

                editForm.ShowDialog();

            }
        }

        private void btnDeleteKart_Click(object sender, EventArgs e)
        {


            string connectionString1 = "Data Source=DESKTOP-IQB2C7N\\SQLEXPRESS;Initial Catalog=erp_azmaind;User ID=sa;Password=Scale123+-;TrustServerCertificate=True";

            using (SqlConnection conn1 = new SqlConnection(connectionString1))
            {
                conn1.Open();


                foreach (ListViewItem item in listView.CheckedItems)
                {

                    string cards = item.Text;


                    string query = "DELETE FROM dbo.cards WHERE [cards] = @cards";

                    using (SqlCommand cmd1 = new SqlCommand(query, conn1))
                    {
                        cmd1.Parameters.AddWithValue("@cards", cards);
                        cmd1.ExecuteNonQuery();
                    }


                    listView.Items.Remove(item);
                }
            }
        }
    

        private void btnNewMenu_Click(object sender, EventArgs e)
        {
            AddCard addCard = new AddCard(this);
            addCard.ShowDialog(); // 
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            button1.BackColor = Color.Black;
            button1.ForeColor = Color.White;

            button2.BackColor = Color.Black;
            button2.ForeColor = Color.White;

            button3.BackColor = Color.Black;
            button3.ForeColor = Color.White;

            button4.BackColor = Color.Black;
            button4.ForeColor = Color.White;

            button5.BackColor = Color.Black;
            button5.ForeColor = Color.White;

            button6.BackColor = Color.White;
            button6.ForeColor = Color.Orange;

            button7.BackColor = Color.Black;
            button7.ForeColor = Color.White;

            scaleInfoContent.Controls.Clear();
            DateTimePicker dtpStartDate = new DateTimePicker();
            dtpStartDate.Format = DateTimePickerFormat.Short;
            dtpStartDate.Size = new Size(150, 32);
            dtpStartDate.Location = new Point(120, 5);
            dtpStartDate.Value = DateTime.Now;

            scaleInfoContent.Controls.Add(dtpStartDate);

            Label lblStartDate = new Label();
            lblStartDate.Text = "Başlanğıc tarix";
            lblStartDate.Size = new Size(150, 32);
            lblStartDate.Location = new Point(30, 5);
            scaleInfoContent.Controls.Add(lblStartDate);

            DateTimePicker dtpEndDate = new DateTimePicker();
            dtpEndDate.Format = DateTimePickerFormat.Short;
            dtpEndDate.Size = new Size(150, 32);
            dtpEndDate.Location = new Point(360, 5);
            dtpEndDate.Value = DateTime.Now.AddDays(7);
            scaleInfoContent.Controls.Add(dtpEndDate);

            Label lblendDate = new Label();
            lblendDate.Text = "Bitmə tarixi";
            lblendDate.Size = new Size(150, 32);
            lblendDate.Location = new Point(280, 5);
            scaleInfoContent.Controls.Add(lblendDate);

            Button btnDelete = new Button();
            btnDelete.Text = "Tarix aralığı ilə sinxronlaşdır";
            btnDelete.Size = new Size(150, 32);
            btnDelete.Location = new Point(870, -1);
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.BackColor = Color.White;
            scaleInfoContent.Controls.Add(btnDelete);

            Button btnNewMenu = new Button();
            btnNewMenu.Text = "Qalan məlumatları sinxronlaşdır";
            btnNewMenu.Size = new Size(180, 32);
            btnNewMenu.Location = new Point(1055, -1);
            btnNewMenu.FlatStyle = FlatStyle.Flat;
            btnNewMenu.FlatAppearance.BorderSize = 0;
            btnNewMenu.BackColor = Color.FromArgb(223, 199, 78);
            btnNewMenu.Click += new EventHandler(btnNewMenu_Click);
            btnNewMenu.ForeColor = Color.White;
            scaleInfoContent.Controls.Add(btnNewMenu);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _running = false;
            if (_handle != IntPtr.Zero)
            {
                Disconnect(_handle);
                _handle = IntPtr.Zero;
            }



            try
            {
                if (!serialPort.IsOpen)
                    serialPort.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Port açılamadı: " + ex.Message);
            }


            int radius = 5;  // Radius dəyərini istədiyiniz ölçüdə təyin edin.
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, button8.Width, button8.Height);
            button8.Region = new Region(path);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void scaleInfoContent_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
