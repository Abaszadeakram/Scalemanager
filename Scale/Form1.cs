
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using TereziEla;

namespace ScaleManagment
{
    public partial class Form1 : Form
    {
        private object label1;
        public ListView listView;
        private TextBox txtSearch;

        public object FlatAppearance { get; private set; }

        public Form1()
        {
            InitializeComponent();


         

        }

        private void button1_Click(object sender, EventArgs e)
        {

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
                    string query = "select*from dbo.tblDatas";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string name = reader["Istifadeci adi"].ToString();
                        DateTime date = Convert.ToDateTime(reader["Yaradilma tarixi"]);
                       
                        ListViewItem item = new ListViewItem(name);
                        item.SubItems.Add(date.ToString("dd.MM.yyyy HH:mm"));

                        listView.Items.Add(item);
                    }

                    reader.Close();
                }


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

               
                Button btnPrev = new Button { Text = "<", Location = new Point(450, -1), Width = 20 };
                btnPrev.FlatStyle = FlatStyle.Flat;
                btnPrev.FlatAppearance.BorderSize = 0;
               
                Button btnPage1 = new Button { Text = "1", Location = new Point(500, -1), Width = 20 };
                btnPage1.FlatStyle = FlatStyle.Flat;
                btnPage1.Click += new EventHandler(button1_Click);
                btnPage1.FlatAppearance.BorderSize = 0;
                Button btnPage2 = new Button { Text = "2", Location = new Point(550, -1), Width = 20 };
                btnPage2.FlatStyle = FlatStyle.Flat;
                btnPage2.FlatAppearance.BorderSize = 0;
                btnPage2.Click += new EventHandler(button2_Click);
                Button btnPage3 = new Button { Text = "3", Location = new Point(600, -1), Width = 20 };
                btnPage3.FlatStyle = FlatStyle.Flat;
                btnPage3.FlatAppearance.BorderSize = 0;
                btnPage3.Click += new EventHandler(button3_Click);
                Button btnPage4 = new Button { Text = "4", Location = new Point(650, -1), Width = 20 };
                btnPage4.FlatStyle = FlatStyle.Flat;
                btnPage4.FlatAppearance.BorderSize = 0;
                btnPage4.Click += new EventHandler(button4_Click);
                Button btnPage5 = new Button { Text = "5", Location = new Point(700, -1), Width = 20 };
                btnPage5.FlatStyle = FlatStyle.Flat;
                btnPage5.FlatAppearance.BorderSize = 0;
                btnPage5.Click += new EventHandler(button5_Click);
                Button btnPage6 = new Button { Text = "6", Location = new Point(750, -1), Width = 20 };
                btnPage6.FlatStyle = FlatStyle.Flat;
                btnPage6.Click += new EventHandler(button6_Click);
                btnPage6.FlatAppearance.BorderSize = 0;
                Button btnPage7 = new Button { Text = "7", Location = new Point(800, -1), Width = 20 };
                btnPage7.FlatStyle = FlatStyle.Flat;
                btnPage7.FlatAppearance.BorderSize = 0;
                btnPage7.Click += new EventHandler(button7_Click);
                Button btnNext = new Button { Text = ">", Location = new Point(850, -1), Width = 20 };
                btnNext.FlatStyle = FlatStyle.Flat;
                btnNext.FlatAppearance.BorderSize = 0;



                Label lblSehife = new Label
                {
                    Text = "7/səhifə ",
                    Location = new Point(1000, -1),
                    AutoSize = true
                };

                Label lblSehifeyekecid = new Label
                {
                    Text = "Səhifəyə keç: ",
                    Location = new Point(1100, -1),
                    AutoSize = true
                };

                
                bottomPanel.Controls.Add(lblCount);
                bottomPanel.Controls.Add(btnPrev);
                bottomPanel.Controls.Add(btnPage1);
                bottomPanel.Controls.Add(btnPage2);
                bottomPanel.Controls.Add(btnPage3);
                bottomPanel.Controls.Add(btnPage4);
                bottomPanel.Controls.Add(btnPage5);
                bottomPanel.Controls.Add(btnPage6);
                bottomPanel.Controls.Add(btnPage7);
                bottomPanel.Controls.Add(btnNext);
                bottomPanel.Controls.Add(lblSehife);
                bottomPanel.Controls.Add(lblSehifeyekecid);

                this.Controls.Add(bottomPanel);

                mainLayout.Controls.Add(topPanel, 0, 0);
                mainLayout.Controls.Add(listView, 0, 1);
                mainLayout.Controls.Add(bottomPanel, 0, 2);
                
               


                scaleInfoContent.Controls.Add(mainLayout);
            
        }



        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string searchQuery = textBox.Text;

            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

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



        public void AddToList(string kartNo, string ad, string soyad,
                         string avtoNo, string marka,
                         string mensubiyyet, string status, string grade)
        {
            ListViewItem item = new ListViewItem("");

            item.SubItems.Add(kartNo);
            item.SubItems.Add(ad + " " + soyad);
            item.SubItems.Add(avtoNo);
            item.SubItems.Add(marka);
            item.SubItems.Add(mensubiyyet);
            item.SubItems.Add(status);
            item.SubItems.Add(grade);

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





        private void button2_Click(object sender, EventArgs e)
        {

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

            
            Control CreateField(string label, string value)
            {
                Panel p = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 13, 0, 2) };
                Label l = new Label { Text = label, Dock = DockStyle.Top, Font = labelFont, AutoSize = true };
                p.BackColor = Color.FromArgb(249, 250, 251);
                
                TextBox t = new TextBox { Text = value, Dock = DockStyle.Bottom, Font = textFont, BorderStyle = BorderStyle.None };
                p.Controls.Add(t);
                p.Controls.Add(l);
                return p;
            }

           
            tbl.Controls.Add(CreateField("Tərəzi", "Azermining Group3"), 0, 0);
            tbl.Controls.Add(CreateField("Tarix/Saat", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")), 1, 0);
            tbl.Controls.Add(CreateField("Avtomobil nömrəsi", "77JB459"), 2, 0);
            tbl.Controls.Add(CreateField("Avtomobil modeli", "BMW"), 3, 0);

            
            tbl.Controls.Add(CreateField("Şirkət", "AMG"), 0, 1);
            tbl.Controls.Add(CreateField("Sürücü", "Mərəh Mərəh"), 1, 1);
            tbl.Controls.Add(CreateField("Kart ID", "553695947"), 2, 1);
            tbl.Controls.Add(CreateField("Yükün növü", "Low quality gold"), 3, 1);

            
            Panel bottomPanel = new Panel { Dock = DockStyle.Fill };

            Label lblRfid = new Label
            {
                Text = "RFID status: ",
                ForeColor = Color.Black,
                Font = labelFont,
                AutoSize = true,
                Location = new Point(12, 12)
            };
            Label lblOxuyucuyabagli = new Label
            {
                Text = " ● Oxuyucuya bağlı",
                ForeColor = Color.Green,
                Font = labelFont,
                AutoSize = true,
                Location = new Point(lblRfid.Right + 3, lblRfid.Top)
            };
            bottomPanel.Controls.Add(lblRfid);
            bottomPanel.Controls.Add(lblOxuyucuyabagli);


            RadioButton toggle = new RadioButton
            {
                Appearance = Appearance.Button,
                Text = "Avtomatik",
                TextAlign = ContentAlignment.MiddleCenter,
                FlatStyle = FlatStyle.Flat,
                Width = 100,
                Height = 30,
                Location = new Point(lblOxuyucuyabagli.Right + 500, lblOxuyucuyabagli.Top+4)
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

            

            
            Button btnBagla = new Button { Text = "Bağla", ForeColor = Color.Red, FlatStyle = FlatStyle.Flat, Location = new Point(850, 15), Width = 60, Height = 30 };
            Button btnAc = new Button { Text = "Aç", ForeColor = Color.Green, FlatStyle = FlatStyle.Flat, Location = new Point(930, 15), Width = 40, Height = 30 };
            Button btnTara = new Button { Text = "Tara", FlatStyle = FlatStyle.Flat, Location = new Point(975, 15), Width = 70, Height = 30 };
            btnTara.FlatAppearance.BorderSize = 0;
            Button btnTesdiqla = new Button { Text = "Təsdiqlə", BackColor = Color.Green, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Location = new Point(1048, 15), Width = 90, Height = 30 };
            Button btnOxucu = new Button { Text = "Oxucuya bağlan", BackColor = Color.Goldenrod, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Location = new Point(1140, 15), Width = 110, Height = 30 };

            bottomPanel.Controls.AddRange(new Control[] { btnBagla, btnAc, btnTara, btnTesdiqla, btnOxucu });

            tbl.Controls.Add(bottomPanel, 0, 2);
            tbl.SetColumnSpan(bottomPanel, 4);

            
            scaleInfoContent.Controls.Add(tbl);
        }

        private void button3_Click(object sender, EventArgs e)
        {
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
            btnExport.Location = new Point(1100, 6);
            btnExport.Width = 150;
            btnExport.Height = 30;
            
            btnExport.FlatStyle = FlatStyle.Flat;
           
            btnExport.Font = new Font("Arial", 11);
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

            
            ListView listView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                CheckBoxes = true,
                BorderStyle=BorderStyle.None
                
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


            Button btnPrev = new Button { Text = "<", Location = new Point(450, -1), Width = 20 };
            btnPrev.FlatStyle = FlatStyle.Flat;
            btnPrev.FlatAppearance.BorderSize = 0;

            Button btnPage1 = new Button { Text = "1", Location = new Point(500, -1), Width = 20 };
            btnPage1.FlatStyle = FlatStyle.Flat;
            btnPage1.Click += new EventHandler(button1_Click);
            btnPage1.FlatAppearance.BorderSize = 0;
            Button btnPage2 = new Button { Text = "2", Location = new Point(550, -1), Width = 20 };
            btnPage2.FlatStyle = FlatStyle.Flat;
            btnPage2.FlatAppearance.BorderSize = 0;
            btnPage2.Click += new EventHandler(button2_Click);
            Button btnPage3 = new Button { Text = "3", Location = new Point(600, -1), Width = 20 };
            btnPage3.FlatStyle = FlatStyle.Flat;
            btnPage3.FlatAppearance.BorderSize = 0;
            btnPage3.Click += new EventHandler(button3_Click);
            Button btnPage4 = new Button { Text = "4", Location = new Point(650, -1), Width = 20 };
            btnPage4.FlatStyle = FlatStyle.Flat;
            btnPage4.FlatAppearance.BorderSize = 0;
            btnPage4.Click += new EventHandler(button4_Click);
            Button btnPage5 = new Button { Text = "5", Location = new Point(700, -1), Width = 20 };
            btnPage5.FlatStyle = FlatStyle.Flat;
            btnPage5.FlatAppearance.BorderSize = 0;
            btnPage5.Click += new EventHandler(button5_Click);
            Button btnPage6 = new Button { Text = "6", Location = new Point(750, -1), Width = 20 };
            btnPage6.FlatStyle = FlatStyle.Flat;
            btnPage6.Click += new EventHandler(button6_Click);
            btnPage6.FlatAppearance.BorderSize = 0;
            Button btnPage7 = new Button { Text = "7", Location = new Point(800, -1), Width = 20 };
            btnPage7.FlatStyle = FlatStyle.Flat;
            btnPage7.FlatAppearance.BorderSize = 0;
            btnPage7.Click += new EventHandler(button7_Click);
            Button btnNext = new Button { Text = ">", Location = new Point(850, -1), Width = 20 };
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.FlatAppearance.BorderSize = 0;


            Label lblSehife = new Label
            {
                Text = "7/səhifə ",
                Location = new Point(1000, -1),
                AutoSize = true
            };

            Label lblSehifeyekecid = new Label
            {
                Text = "Səhifəyə keç: ",
                Location = new Point(1100, -1),
                AutoSize = true
            };

           
            bottomPanel.Controls.Add(lblCount);
            bottomPanel.Controls.Add(btnPrev);
            bottomPanel.Controls.Add(btnPage1);
            bottomPanel.Controls.Add(btnPage2);
            bottomPanel.Controls.Add(btnPage3);
            bottomPanel.Controls.Add(btnPage4);
            bottomPanel.Controls.Add(btnPage5);
            bottomPanel.Controls.Add(btnPage6);
            bottomPanel.Controls.Add(btnPage7);
            bottomPanel.Controls.Add(btnNext);
            bottomPanel.Controls.Add(lblSehife);
            bottomPanel.Controls.Add(lblSehifeyekecid);

            this.Controls.Add(bottomPanel);

            
            mainLayout.Controls.Add(topPanel, 0, 0);
            mainLayout.Controls.Add(listView, 0, 1);
            mainLayout.Controls.Add(bottomPanel, 0, 2);

          
            scaleInfoContent.Controls.Add(mainLayout);
        }



        private void button4_Click(object sender, EventArgs e)
        {
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
            Image resizedImage = new Bitmap(originalImage, new Size(originalImage.Width / 18, originalImage.Height / 18));  
            btnExport.Image = resizedImage;
            btnExport.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnExport.Location = new Point(1100, 6);
            btnExport.Width = 150; 
            btnExport.Height = 30;
           
            btnExport.FlatStyle = FlatStyle.Flat;
            
            btnExport.Font = new Font("Arial", 11);
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

           
            ListView listView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                CheckBoxes = true,
                BorderStyle=BorderStyle.None
                
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


            Button btnPrev = new Button { Text = "<", Location = new Point(450, -1), Width = 20 };
            btnPrev.FlatStyle = FlatStyle.Flat;
            btnPrev.FlatAppearance.BorderSize = 0;

            Button btnPage1 = new Button { Text = "1", Location = new Point(500, -1), Width = 20 };
            btnPage1.FlatStyle = FlatStyle.Flat;
            btnPage1.Click += new EventHandler(button1_Click);
            btnPage1.FlatAppearance.BorderSize = 0;
            Button btnPage2 = new Button { Text = "2", Location = new Point(550, -1), Width = 20 };
            btnPage2.FlatStyle = FlatStyle.Flat;
            btnPage2.FlatAppearance.BorderSize = 0;
            btnPage2.Click += new EventHandler(button2_Click);
            Button btnPage3 = new Button { Text = "3", Location = new Point(600, -1), Width = 20 };
            btnPage3.FlatStyle = FlatStyle.Flat;
            btnPage3.FlatAppearance.BorderSize = 0;
            btnPage3.Click += new EventHandler(button3_Click);
            Button btnPage4 = new Button { Text = "4", Location = new Point(650, -1), Width = 20 };
            btnPage4.FlatStyle = FlatStyle.Flat;
            btnPage4.FlatAppearance.BorderSize = 0;
            btnPage4.Click += new EventHandler(button4_Click);
            Button btnPage5 = new Button { Text = "5", Location = new Point(700, -1), Width = 20 };
            btnPage5.FlatStyle = FlatStyle.Flat;
            btnPage5.FlatAppearance.BorderSize = 0;
            btnPage5.Click += new EventHandler(button5_Click);
            Button btnPage6 = new Button { Text = "6", Location = new Point(750, -1), Width = 20 };
            btnPage6.FlatStyle = FlatStyle.Flat;
            btnPage6.Click += new EventHandler(button6_Click);
            btnPage6.FlatAppearance.BorderSize = 0;
            Button btnPage7 = new Button { Text = "7", Location = new Point(800, -1), Width = 20 };
            btnPage7.FlatStyle = FlatStyle.Flat;
            btnPage7.FlatAppearance.BorderSize = 0;
            btnPage7.Click += new EventHandler(button7_Click);
            Button btnNext = new Button { Text = ">", Location = new Point(850, -1), Width = 20 };
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.FlatAppearance.BorderSize = 0;


            Label lblSehife = new Label
            {
                Text = "7/səhifə ",
                Location = new Point(1000, -1),
                AutoSize = true
            };

            Label lblSehifeyekecid = new Label
            {
                Text = "Səhifəyə keç: ",
                Location = new Point(1100, -1),
                AutoSize = true
            };

           
            bottomPanel.Controls.Add(lblCount);
            bottomPanel.Controls.Add(btnPrev);
            bottomPanel.Controls.Add(btnPage1);
            bottomPanel.Controls.Add(btnPage2);
            bottomPanel.Controls.Add(btnPage3);
            bottomPanel.Controls.Add(btnPage4);
            bottomPanel.Controls.Add(btnPage5);
            bottomPanel.Controls.Add(btnPage6);
            bottomPanel.Controls.Add(btnPage7);
            bottomPanel.Controls.Add(btnNext);
            bottomPanel.Controls.Add(lblSehife);
            bottomPanel.Controls.Add(lblSehifeyekecid);

            this.Controls.Add(bottomPanel);

           
            mainLayout.Controls.Add(topPanel, 0, 0);
            mainLayout.Controls.Add(listView, 0, 1);
            mainLayout.Controls.Add(bottomPanel, 0, 2);

          
            scaleInfoContent.Controls.Add(mainLayout);
        }





        private void button7_Click(object sender, EventArgs e)
        {

            
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


            Button btnExport = new Button();
            btnExport.Text = " Export";
            btnExport.Image = Image.FromFile("C:\\Users\\Akbar\\Documents\\pictures\\images (5).png");
            Image originalImage = btnExport.Image;
            Image resizedImage = new Bitmap(originalImage, new Size(originalImage.Width / 18, originalImage.Height / 18));  // Burada şəkili yarıya endiririk
            btnExport.Image = resizedImage;
            btnExport.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnExport.Location = new Point(850, 0);
            btnExport.Width = 100; 
            btnExport.Height = 30;
           
            btnExport.FlatStyle = FlatStyle.Flat;
           
            btnExport.Font = new Font("Arial", 11);
            btnExport.FlatAppearance.BorderSize = 0;
            scaleInfoContent.Controls.Add(btnExport);

            Button btnDelete2 = new Button()
            {
                Text = "Reysi silmək",
                Location = new Point(950, 0),
                Width = 100,
                Height=30,
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


            topPanel.Controls.Add(btnExport);
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

          

            listView.DoubleClick += ListView_DoubleClick;

           

            scaleInfoContent.Controls.Add(listView);
            scaleInfoContent.Controls.Add(statsPanel);
            scaleInfoContent.Controls.Add(topPanel);
        }

        private void btnDelete2_Click(object sender, EventArgs e)
        {
            ReysiSilmek reysiSilmek = new ReysiSilmek();
            reysiSilmek.ShowDialog();
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

            Button btnDelete = new Button();
            btnDelete.Text = "Kartı sil";
            btnDelete.Size = new Size(90, 32);
            btnDelete.Location = new Point(1060, -1);
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.BackColor = Color.White;
            scaleInfoContent.Controls.Add(btnDelete);


            Button btnNewMenu = new Button();
            btnNewMenu.Text = "+ Yeni kart";
            btnNewMenu.Size = new Size(90, 32);
            btnNewMenu.Location = new Point(1165, -1);
            btnNewMenu.FlatStyle = FlatStyle.Flat;
            btnNewMenu.FlatAppearance.BorderSize = 0;
            btnNewMenu.BackColor = Color.FromArgb(223,199,78);
            btnNewMenu.Click += new EventHandler(btnNewMenu_Click);
            btnNewMenu.ForeColor = Color.White;
            scaleInfoContent.Controls.Add(btnNewMenu);


            
            ListView listView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                CheckBoxes = true,
                BorderStyle=BorderStyle.None
            };

            // Sütunlar
            listView.Columns.Add("Kart nömrəsi", 200);
            listView.Columns.Add("Sürücü", 200);
            listView.Columns.Add("Avtomobil nömrəsi", 200);
            listView.Columns.Add("Avtomobil markası", 200);
            listView.Columns.Add("Avtomobil statusu", 200);
            
            listView.Columns.Add("Grade", 200);
           


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


            Button btnPrev = new Button { Text = "<", Location = new Point(450, -1), Width = 20 };
            btnPrev.FlatStyle = FlatStyle.Flat;
            btnPrev.FlatAppearance.BorderSize = 0;

            Button btnPage1 = new Button { Text = "1", Location = new Point(500, -1), Width = 20 };
            btnPage1.FlatStyle = FlatStyle.Flat;
            btnPage1.Click += new EventHandler(button1_Click);
            btnPage1.FlatAppearance.BorderSize = 0;
            Button btnPage2 = new Button { Text = "2", Location = new Point(550, -1), Width = 20 };
            btnPage2.FlatStyle = FlatStyle.Flat;
            btnPage2.FlatAppearance.BorderSize = 0;
            btnPage2.Click += new EventHandler(button2_Click);
            Button btnPage3 = new Button { Text = "3", Location = new Point(600, -1), Width = 20 };
            btnPage3.FlatStyle = FlatStyle.Flat;
            btnPage3.FlatAppearance.BorderSize = 0;
            btnPage3.Click += new EventHandler(button3_Click);
            Button btnPage4 = new Button { Text = "4", Location = new Point(650, -1), Width = 20 };
            btnPage4.FlatStyle = FlatStyle.Flat;
            btnPage4.FlatAppearance.BorderSize = 0;
            btnPage4.Click += new EventHandler(button4_Click);
            Button btnPage5 = new Button { Text = "5", Location = new Point(700, -1), Width = 20 };
            btnPage5.FlatStyle = FlatStyle.Flat;
            btnPage5.FlatAppearance.BorderSize = 0;
            btnPage5.Click += new EventHandler(button5_Click);
            Button btnPage6 = new Button { Text = "6", Location = new Point(750, -1), Width = 20 };
            btnPage6.FlatStyle = FlatStyle.Flat;
            btnPage6.Click += new EventHandler(button6_Click);
            btnPage6.FlatAppearance.BorderSize = 0;
            Button btnPage7 = new Button { Text = "7", Location = new Point(800, -1), Width = 20 };
            btnPage7.FlatStyle = FlatStyle.Flat;
            btnPage7.FlatAppearance.BorderSize = 0;
            btnPage7.Click += new EventHandler(button7_Click);
            Button btnNext = new Button { Text = ">", Location = new Point(850, -1), Width = 20 };
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.FlatAppearance.BorderSize = 0;


            Label lblSehife = new Label
            {
                Text = "7/səhifə ",
                Location = new Point(1000, -1),
                AutoSize = true
            };

            Label lblSehifeyekecid = new Label
            {
                Text = "Səhifəyə keç: ",
                Location = new Point(1100, -1),
                AutoSize = true
            };

        
            bottomPanel.Controls.Add(lblCount);
            bottomPanel.Controls.Add(btnPrev);
            bottomPanel.Controls.Add(btnPage1);
            bottomPanel.Controls.Add(btnPage2);
            bottomPanel.Controls.Add(btnPage3);
            bottomPanel.Controls.Add(btnPage4);
            bottomPanel.Controls.Add(btnPage5);
            bottomPanel.Controls.Add(btnPage6);
            bottomPanel.Controls.Add(btnPage7);
            bottomPanel.Controls.Add(btnNext);
            bottomPanel.Controls.Add(lblSehife);
            bottomPanel.Controls.Add(lblSehifeyekecid);

            this.Controls.Add(bottomPanel);

           
            mainLayout.Controls.Add(topPanel, 0, 0);
            mainLayout.Controls.Add(listView, 0, 1);
            mainLayout.Controls.Add(bottomPanel, 0, 2);

           
            scaleInfoContent.Controls.Add(mainLayout);

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
            int radius = 5;  // Radius dəyərini istədiyiniz ölçüdə təyin edin.
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, button8.Width, button8.Height);
            button8.Region = new Region(path);
        }
    }
}
