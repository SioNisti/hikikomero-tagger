using System;
using System.Drawing;
using System.Windows.Forms;

namespace testiä
{
    public partial class quickdata : Form
    {
        public quickdata()
        {
            InitializeComponent();
        }

        public imageMTDT f1;
        public object PictureBox { get; private set; }

        public static string qdedited;
        public static string qdkuva;
        public static string qdkansio;
        public static string qdkuva2;
        public static string qdtype;
        public static int qdrow;
        public static int qdintti;

        private PictureBox _thePicture;
        public PictureBox ThePicture
        {
            set { this._thePicture = value; }
            get { return this._thePicture; }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            if (this.ThePicture != null)
            {
                var bmp = new Bitmap(this.ThePicture.Width, this.ThePicture.Height);
                this.ThePicture.DrawToBitmap(bmp, this.ThePicture.ClientRectangle);
                e.Graphics.DrawImage(bmp, 25, 25, 800, 1050);
            }
        }

        public void quickdata_Load(object sender, EventArgs e)
        {
            //label1.Text = imageMTDT.quickdata_name;
            //textBox1.Text = imageMTDT.quickdata_data;
            getdata();
            imageMTDT i = new imageMTDT();
            GlobalHotKey.RegisterHotKey("CTRL + N", () => i.nextimage()); //seuraava kuva
            GlobalHotKey.RegisterHotKey("CTRL + Right", () => i.nextimage());//seuraava kuva
            GlobalHotKey.RegisterHotKey("CTRL + B", () => i.previmage());//edellinen kuva
            GlobalHotKey.RegisterHotKey("CTRL + Left", () => i.previmage());//edellinen kuva
            GlobalHotKey.RegisterHotKey("CTRL + D", () => i.Kakapylytoimi());//valitse kansio
            GlobalHotKey.RegisterHotKey("CTRL + R", () => i.refresh());//valitsee saman kansion uudestaan ns "päivittääkkseen" tiedosto listan
        }
        public void getdata()
        {
            label1.Text = imageMTDT.quickdata_name;
            textBox1.Text = imageMTDT.quickdata_data;
            qdkuva = imageMTDT.valittukuva;
            qdkansio = imageMTDT.valittukansio2;
            qdkuva2 = imageMTDT.valittukuva2;
            qdrow = imageMTDT.qdrow;
            qdtype = imageMTDT.quickdata_type;
            qdintti= imageMTDT.intti;
        }

        private void qdclosing(object sender, FormClosingEventArgs e)
        {
            imageMTDT.qdon = false;
            this.Hide();
            e.Cancel = true;
        }
        public void alzheimer()
        {
            //tää koodin pätkä ottaa sen käytössä olevan kuvan pois pictureboxista jotta sen voi tiedoston päälle voi tallentaa

            imageMTDT i = new imageMTDT();
            /*
            var bit = new Bitmap(this.Width, this.Height);
            var g = Graphics.FromImage(bit);

            var oldImage = i.ThePicture.Image;
            i.ThePicture.Image = bit;
            oldImage?.Dispose();

            g.Dispose();*/
            i.ThePicture.Dispose();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                qdedited = textBox1.Text;
                qdintti = imageMTDT.intti;
                alzheimer();
                imageMTDT i = new imageMTDT();
                i.Unohdakuva();
                i.ThePicture.Dispose();
                i.ThePicture.Image.Dispose();
                i.updatemtdt(qdrow);
            }
        }
    }
}
