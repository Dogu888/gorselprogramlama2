using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp16102025GPSON
{
    public partial class Form1 : Form
    {
        private Button btnBasla;
        private Button btnBitir;
        private Panel pnlOyun;
        private ListBox listBoxSonuc;
        private Timer oyunTimer;
        private Timer hareketTimer;
        private Label lblSure;
        private int kalanSure = 60;
        private Random rnd = new Random();

        private List<int> sayilar = new List<int> { 15, 32, 84, 27, 50, 7, 71, 1, 44, 96 };
        private Dictionary<Button, PointF> hizlar = new Dictionary<Button, PointF>();

        public Form1()
        {
            InitializeComponent();
            FormOlustur();
        }

        private void FormOlustur()
        {
            this.Text = "Çift Sayı Sıralama Oyunu";
            this.Size = new Size(900, 550);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;
            this.DoubleBuffered = true; 

            btnBasla = new Button()
            {
                Text = "Oyuna Başla",
                Location = new Point(20, 20),
                Size = new Size(120, 40),
                BackColor = Color.LightGreen,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnBasla.Click += BtnBasla_Click;

            btnBitir = new Button()
            {
                Text = "Oyunu Bitir",
                Location = new Point(740, 20),
                Size = new Size(120, 40),
                BackColor = Color.IndianRed,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Enabled = false
            };
            btnBitir.Click += BtnBitir_Click;

            pnlOyun = new Panel()
            {
                Location = new Point(160, 80),
                Size = new Size(560, 350),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            listBoxSonuc = new ListBox()
            {
                Location = new Point(740, 80),
                Size = new Size(120, 350),
                Font = new Font("Segoe UI", 10)
            };

            lblSure = new Label()
            {
                Text = "Süre: 60",
                Location = new Point(20, 80),
                Size = new Size(120, 30),
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };

            oyunTimer = new Timer() { Interval = 1000 };
            oyunTimer.Tick += OyunTimer_Tick;

            hareketTimer = new Timer() { Interval = 20 }; 
            hareketTimer.Tick += HareketTimer_Tick;

            Controls.Add(btnBasla);
            Controls.Add(btnBitir);
            Controls.Add(pnlOyun);
            Controls.Add(listBoxSonuc);
            Controls.Add(lblSure);
        }

        private void BtnBasla_Click(object sender, EventArgs e)
        {
            pnlOyun.Controls.Clear();
            listBoxSonuc.Items.Clear();
            hizlar.Clear();
            kalanSure = 60;
            lblSure.Text = "Süre: 60";
            btnBitir.Enabled = true;
            oyunTimer.Start();
            hareketTimer.Start();

            var karisik = sayilar.OrderBy(sayi => rnd.Next()).ToList();

            foreach (int s in karisik)
            {
                Button b = new Button()
                {
                    Text = s.ToString(),
                    Size = new Size(70,50),
                    Location = new Point(rnd.Next(pnlOyun.Width - 50), rnd.Next(pnlOyun.Height - 35)),
                    Font = new Font("Segoe UI", 8, FontStyle.Bold),
                    BackColor = Color.LightSteelBlue
                };
                b.Click += SayiyaTikla;
                pnlOyun.Controls.Add(b);

                // Rastgele yön oluştur
                float dx = (float)(rnd.NextDouble() * 4 - 2); 
                float dy = (float)(rnd.NextDouble() * 4 - 2);
                hizlar[b] = new PointF(dx, dy);
            }
        }

        private void HareketTimer_Tick(object sender, EventArgs e)
        {
            foreach (Button b in pnlOyun.Controls.OfType<Button>())
            {
                var hiz = hizlar[b];
                float yeniX = b.Location.X + hiz.X;
                float yeniY = b.Location.Y + hiz.Y;

                
                if (yeniX < 0 || yeniX > pnlOyun.Width - b.Width)
                    hiz.X = -hiz.X;
                if (yeniY < 0 || yeniY > pnlOyun.Height - b.Height)
                    hiz.Y = -hiz.Y;

                
                b.Location = new Point((int)Math.Max(0, Math.Min(pnlOyun.Width - b.Width, yeniX)),
                                       (int)Math.Max(0, Math.Min(pnlOyun.Height - b.Height, yeniY)));

                hizlar[b] = hiz;
            }
        }

        private void SayiyaTikla(object sender, EventArgs e)
        {
            Button tiklanan = sender as Button;
            int sayi = int.Parse(tiklanan.Text);

            var kalanSayilar = pnlOyun.Controls.OfType<Button>().Select(btn => int.Parse(btn.Text)).ToList();
            var ciftler = kalanSayilar.Where(s => s % 2 == 0).OrderBy(s => s).ToList();

            if (ciftler.Any())
            {
                int minCift = ciftler.First();
                if (sayi == minCift)
                {
                    listBoxSonuc.Items.Add(sayi);
                    pnlOyun.Controls.Remove(tiklanan);
                    hizlar.Remove(tiklanan);
                }
            }
        }

        private void BtnBitir_Click(object sender, EventArgs e)
        {
            oyunTimer.Stop();
            hareketTimer.Stop();
            btnBitir.Enabled = false;

            var dogruCiftler = sayilar.Where(s => s % 2 == 0).OrderBy(s => s).ToList();
            var oyuncuCiftler = listBoxSonuc.Items.Cast<int>().ToList();

            bool kazandi = dogruCiftler.SequenceEqual(oyuncuCiftler);

            if (kazandi)
            {
                MessageBox.Show($"{kalanSure} saniye kala oyunu bitirdin!", "Tebrikler 🎉", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Oyunu bitiremediniz! Tüm çift sayıları küçükten büyüğe bulmanız gerekiyor.", "Sonuç ❌", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void OyunTimer_Tick(object sender, EventArgs e)
        {
            kalanSure--;
            lblSure.Text = $"Süre: {kalanSure}";

            if (kalanSure <= 0)
            {
                oyunTimer.Stop();
                hareketTimer.Stop();
                MessageBox.Show("Süre bitti!", "Oyun Bitti", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnBitir.Enabled = false;
            }
        }
    }
}



