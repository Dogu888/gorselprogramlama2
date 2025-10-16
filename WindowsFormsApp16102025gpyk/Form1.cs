using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp16102025gpyk
{
    public partial class Form1 : Form
    {
        Random rnd = new Random();
        int toplamPuan = 0;
        Label lblPuan;
        Button btn;
        Timer timer;
        bool kirmiziSirasi = true; 

        public Form1()
        {
            InitializeComponent();
            this.Text = "Rastgele Buton Oyunu";
            this.Size = new Size(600, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            OyunuBaslat();
        }

        void OyunuBaslat()
        {
            lblPuan = new Label();
            lblPuan.Text = "Puan: 0";
            lblPuan.Font = new Font("Arial", 14, FontStyle.Bold);
            lblPuan.Location = new Point(20, 20);
            lblPuan.AutoSize = true;
            this.Controls.Add(lblPuan);

            btn = new Button();
            btn.Size = new Size(80, 80);
            btn.Font = new Font("Arial", 16, FontStyle.Bold);
            btn.Click += Btn_Click;
            this.Controls.Add(btn);

            timer = new Timer();
            timer.Interval = 1000; 
            timer.Tick += Timer_Tick;
            timer.Start();

            RastgeleSayiVeKonum();
        }

        void RastgeleSayiVeKonum()
        {
            int sayi = rnd.Next(1, 10);
            btn.Text = sayi.ToString();

            
            if (kirmiziSirasi)
                btn.ForeColor = Color.Red;
            else
                btn.ForeColor = Color.Black;

            
            kirmiziSirasi = !kirmiziSirasi;

            
            int maxX = this.ClientSize.Width - btn.Width - 20;
            int maxY = this.ClientSize.Height - btn.Height - 20;
            int x = rnd.Next(20, maxX);
            int y = rnd.Next(60, maxY);

            btn.Location = new Point(x, y);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            RastgeleSayiVeKonum();
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            int sayi = int.Parse(btn.Text);

            if (btn.ForeColor == Color.Red)
                toplamPuan += sayi;
            else
                toplamPuan -= sayi;

            lblPuan.Text = "Puan: " + toplamPuan;

            if (toplamPuan >= 100)
            {
                timer.Stop();
                MessageBox.Show("🎉 Tebrikler! 100 puana ulaştınız. Oyun bitti!");
                Application.Exit();
            }
        }
    }
}