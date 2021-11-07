using ExifLibrary;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Directory = System.IO.Directory;
/*
*nää on kuulemma turhia t:vs
*"kuulemma" on aika aliliioteltu sana tähän.  muunmuassa nää joskus vanhassa koodissa käytetyt library:t vei 300mb tilaa turhaan
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Text;
using System.Threading.Tasks;
using ExifLib;
using MetadataExtractor.Formats.Exif;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using GroupDocs.Metadata.Common;
using System.Runtime.InteropServices;
using MetadataExtractor;
using System.Linq;
*/

namespace testiä
{
    public partial class imageMTDT : Form
    {
        public class MessageBoxer
        {
            public static bool IsOpen { get; set; }

            public static void Show(string messageBoxText, string headr, MessageBoxIcon sign)
            {
                IsOpen = true;
                MessageBox.Show(messageBoxText, headr, 0, sign);
                IsOpen = false;
            }
        }
        public imageMTDT()
        {
            InitializeComponent();
            /*diatimeSel.Controls[0].Visible = false;
            diatimeSel.Controls[1].Width = Width - 4;
            currentimage.Controls[0].Visible = false;
            currentimage.Controls[1].Width = Width - 4;
            imageamount.Controls[0].Visible = false;
            imageamount.Controls[1].Width = Width - 4;*/


            GlobalHotKey.RegisterHotKey("CTRL + N", () => nextimage()); //seuraava kuva
            GlobalHotKey.RegisterHotKey("CTRL + Right", () => nextimage());//seuraava kuva
            GlobalHotKey.RegisterHotKey("CTRL + B", () => previmage());//edellinen kuva
            GlobalHotKey.RegisterHotKey("CTRL + Left", () => previmage());//edellinen kuva
            GlobalHotKey.RegisterHotKey("CTRL + T", () => fcsTagSearch());//laittaa focusin siihen tägihaku lootaan
            GlobalHotKey.RegisterHotKey("CTRL + D", () => Kakapylytoimi());//valitse kansio
            GlobalHotKey.RegisterHotKey("CTRL + R", () => refresh());//valitsee saman kansion uudestaan ns "päivittääkkseen" tiedosto listan

            quickdata form2 = new quickdata();
            form2.ThePicture = this.PictureBox;

        }
        public PictureBox ThePicture
        {
            get { return PictureBox; }
        }

        public static string valittukansio2;
        public static string valittukuva;
        public static string valittukuva2;

        public static int intti = 0;
        int curtab = 0;
        int rowcount = 5;
        int qdct = 0;

        quickdata _d = new quickdata();
        public static string quickdata_type;
        public static string quickdata_name;
        public static string quickdata_data;
        public static bool qdon = false;
        public bool qdused = true;
        public static int qdrow;

        public static bool showpngs = false;
        //Timer _timeri = new Timer();
        //static System.Windows.Forms.Timer _timeri = new System.Windows.Forms.Timer();

        private void imageMTDT_Load(object sender, EventArgs e)
        {
            prevbtn.Enabled = false;
            nxtbtn.Enabled = false;
            TagSearch.Enabled = false;
            currentimage.Enabled = false;
            tab_ap.Enabled = false;
            tab_camera.Enabled = false;
            tab_description.Enabled = false;
            tab_origin.Enabled = false;
            diatimeSel.Enabled = false;
        }
        public void Unohdakuva()
        {
            //tää koodin pätkä ottaa sen käytössä olevan kuvan pois pictureboxista jotta sen voi tiedoston päälle voi tallentaa
            var bit = new Bitmap(this.Width, this.Height);
            var g = Graphics.FromImage(bit);

            var oldImage = ThePicture.Image;
            ThePicture.Image = bit;
            oldImage?.Dispose();

            g.Dispose();
        }
        private void Button1_Click(object sender, EventArgs e)
        {
                //edellinen kuva
                previmage();
                Debug.WriteLine(intti);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
                // seuraava kuva
                nextimage();
                Debug.WriteLine(intti);
        }
        public void nextimage()
        {
            if (valittukansio2 == null)
            {
                if (!MessageBoxer.IsOpen)
                {
                    //MessageBox.Show("No images found", "No images", 0, MessageBoxIcon.Error);
                    MessageBoxer.Show("No images found", "No images", MessageBoxIcon.Error);

                    return;
                }
                if (MessageBoxer.IsOpen)
                {
                    return;
                }
            }

            // seuraava kuva
            Unohdakuva();

            intti++;

            //FileBox.Items[0].Selected = true; en täysin oo varma mikä tän virka on.  saattanee olla vanhan koodin jäännös

            if (intti == FileBox.Items.Count + 1)
            {
                intti = 1;
            }

            //imageamount.Value = FileBox.Items.Count;
            imageAmount2.Text = "/ "+FileBox.Items.Count.ToString();
            currentimage.Value = intti;

            if (FileBox.Items.Count == 1)
            {
                currentimage.Value = 1;
                gsImages();
            }
            gsImages();
            Debug.WriteLine(intti);
            prevbtn.Enabled = true;
            nxtbtn.Enabled = true;
            TagSearch.Enabled = true;
            diatimeSel.Enabled = true;
            currentimage.Enabled = true;
        }
        public void previmage()
        {
            if(valittukansio2 == null)
            {
                if (!MessageBoxer.IsOpen)
                {
                    //MessageBox.Show("No images found", "No images", 0, MessageBoxIcon.Error);
                    MessageBoxer.Show("No images found", "No images", MessageBoxIcon.Error);

                    return;
                }
                if (MessageBoxer.IsOpen)
                {
                    return;
                }
            }
            //edellinen kuva
            Unohdakuva();
            intti--;

            //FileBox.Items[0].Selected = true; en täysin oo varma mikä tän virka on.  saattanee olla vanhan koodin jäännös

            imageAmount2.Text = "/ " + FileBox.Items.Count.ToString();
            //imageamount.Value = FileBox.Items.Count;
            currentimage.Value = intti;

            if (intti < 1)
            {
                intti = FileBox.Items.Count;
            }

            if (FileBox.Items.Count == 1)
            {
                currentimage.Value = 1;
            }
            else
            {
                currentimage.Value = intti;
            }

            gsImages();
            Debug.WriteLine(intti);
            prevbtn.Enabled = true;
            nxtbtn.Enabled = true;
            TagSearch.Enabled = true;
            diatimeSel.Enabled = true;
            currentimage.Enabled = true;
        }

        public void Metat()
        {
            //tää siis ottaa valitusta kuvasta kaikki löytyvät metadatat ja asettaa ne siihen listview:iin

            //saatanan vittumainen bugi ton datagridview:in kanssa
            //se paska kaatuu jos oot muokkaamassa jotain solua ja klikkaat toiseen soluun

            //descriptionMTDT.CurrentCell = null;voi perkele
            descriptionMTDT.ClearSelection();
            descriptionMTDT.Rows.Clear();

            var file = ImageFile.FromFile(valittukuva);

            /*foreach (var property in file.Properties)
            {
                Debug.WriteLine(property.Name + " - " + property.Value);
            }*/

            if(!tab_ap.Enabled)
            {
                tab_ap.Enabled = true;
                tab_camera.Enabled = true;
                tab_description.Enabled = true;
                tab_origin.Enabled = true;
            }

            if (curtab == 0)
            {
                tab_description.BackColor = Color.White;
                tab_origin.BackColor = Color.Gray;
                tab_camera.BackColor = Color.Gray;
                tab_ap.BackColor = Color.Gray;

                descriptionMTDT.Rows.Add("Title", file.Properties.Get(ExifTag.WindowsTitle), 140091);
                descriptionMTDT.Rows.Add("Subject", file.Properties.Get(ExifTag.WindowsSubject), 140095);
                descriptionMTDT.Rows.Add("Rating", file.Properties.Get(ExifTag.Rating), 118246);
                descriptionMTDT.Rows.Add("Tags", file.Properties.Get(ExifTag.WindowsKeywords), 140094);
                descriptionMTDT.Rows.Add("Comments", file.Properties.Get(ExifTag.WindowsComment), 140092);
                rowcount = 5;
            }
            if (curtab == 1)
            {
                tab_description.BackColor = Color.Gray;
                tab_origin.BackColor = Color.White;
                tab_camera.BackColor = Color.Gray;
                tab_ap.BackColor = Color.Gray;

                descriptionMTDT.Rows.Add("Authors", file.Properties.Get(ExifTag.WindowsAuthor), 140093);
                descriptionMTDT.Rows.Add("Date taken", file.Properties.Get(ExifTag.DateTimeOriginal), 236867);
                //descriptionMTDT.Rows.Add("Date acquired", file.Properties.Get(ExifTag.SubSecTime), 237520); exiflib:issä ei ole "date acquired" juttua???
                descriptionMTDT.Rows.Add("Copyright", file.Properties.Get(ExifTag.Copyright), 133432);
                rowcount = 3;
            }
            if (curtab == 2)
            {
                tab_description.BackColor = Color.Gray;
                tab_origin.BackColor = Color.Gray;
                tab_camera.BackColor = Color.White;
                tab_ap.BackColor = Color.Gray;

                descriptionMTDT.Rows.Add("Camera maker", file.Properties.Get(ExifTag.Make), 100271);
                descriptionMTDT.Rows.Add("Camera model", file.Properties.Get(ExifTag.Model), 100272);
                descriptionMTDT.Rows.Add("ISO speed", file.Properties.Get(ExifTag.ISOSpeedRatings), 234855);
                descriptionMTDT.Rows.Add("Metering Mode", file.Properties.Get(ExifTag.MeteringMode), 237383);
                descriptionMTDT.Rows.Add("Flash mode", file.Properties.Get(ExifTag.Flash), 237385);
                descriptionMTDT.Rows.Add("35mm focal lenght", file.Properties.Get(ExifTag.FocalLengthIn35mmFilm), 241989);
                rowcount = 6;
            }
            if (curtab == 3)
            {
                tab_description.BackColor = Color.Gray;
                tab_origin.BackColor = Color.Gray;
                tab_camera.BackColor = Color.Gray;
                tab_ap.BackColor = Color.White;

                descriptionMTDT.Rows.Add("Lens maker", file.Properties.Get(ExifTag.LensMake), 242035);
                descriptionMTDT.Rows.Add("Lens model", file.Properties.Get(ExifTag.LensModel), 242036); //tää on eri mitä w käyttää
                descriptionMTDT.Rows.Add("Flash maker", file.Properties.Get(ExifTag.FlashEnergy), 241483); //tää on eri mitä w käyttää
                descriptionMTDT.Rows.Add("Flash model", file.Properties.Get(ExifTag.FlashpixVersion), 240960); //tää on eri mitä w käyttää
                descriptionMTDT.Rows.Add("Camera serial number", file.Properties.Get(ExifTag.BodySerialNumber), 242033); //tää on eri mitä w käyttää
                descriptionMTDT.Rows.Add("Contrast", file.Properties.Get(ExifTag.Contrast), 241992);
                descriptionMTDT.Rows.Add("Light source", file.Properties.Get(ExifTag.LightSource), 237384);
                descriptionMTDT.Rows.Add("Exposure program", file.Properties.Get(ExifTag.ExposureProgram), 234850);
                descriptionMTDT.Rows.Add("Saturation", file.Properties.Get(ExifTag.Saturation), 241993);
                descriptionMTDT.Rows.Add("Sharpness", file.Properties.Get(ExifTag.Sharpness), 241994);
                descriptionMTDT.Rows.Add("White balance", file.Properties.Get(ExifTag.WhiteBalance), 241987);
                descriptionMTDT.Rows.Add("EXIF version", file.Properties.Get(ExifTag.ExifVersion), 236864);
                rowcount = 12;
            }
        }

        private void ToolStripButton1_Click_1(object sender, EventArgs e)
        {
            Kakapylytoimi();
        }

        public void Kakapylytoimi()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            //dialog.InitialDirectory = "C:\\Users"; tällä sais valittua alotus kansion mutta jos se ei ole määritelty niin se alottaa siitä mihin jäit
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                valittukansio2 = dialog.FileName;
                ChosenFolder.Text = dialog.FileName;

                FileBox.Items.Clear();

                string[] files = Directory.GetFiles(dialog.FileName);

                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    ListViewItem item = new ListViewItem(fileName);

                    if (file.Contains(".jpg") || file.Contains(".png") && showpngs/* || file.Contains(".png")*/)
                    {
                        item.Tag = file;

                        FileBox.Items.Add(item);
                    }
                }

            }
            if (FileBox.Items.Count == 0)
            {
                //MessageBox.Show("No JPG images were found in the path \"" + valittukansio2 + "\"", "No JPGs", 0, MessageBoxIcon.Information);
                MessageBoxer.Show("No JPG images were found in the path \"" + valittukansio2 + "\"", "No JPGs", MessageBoxIcon.Information);
            }
            else
            {
                intti = 0;
                //imageamount.Value = FileBox.Items.Count;
                imageAmount2.Text = "/ " + FileBox.Items.Count.ToString();
                currentimage.Value = intti;

                prevbtn.Enabled = true;
                nxtbtn.Enabled = true;
                TagSearch.Enabled = true;
                diatimeSel.Enabled = true;
                currentimage.Enabled = true;
            }
        }
        public void refresh()
        {
            if (valittukansio2 != null)
            {
                FileBox.Items.Clear();

                string[] files = Directory.GetFiles(valittukansio2);

                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    ListViewItem item = new ListViewItem(fileName);

                    if (file.Contains(".jpg") || file.Contains(".png") && showpngs/* || file.Contains(".png")*/)
                    {
                        item.Tag = file;

                        FileBox.Items.Add(item);
                    }
                }
            if (FileBox.Items.Count == 0)
            {
                //MessageBox.Show("No JPG images were found in the path \"" + valittukansio2 + "\"", "No JPGs", 0, MessageBoxIcon.Information);
                MessageBoxer.Show("No JPG images were found in the path \"" + valittukansio2 + "\"", "No JPGs", MessageBoxIcon.Information);
            }
            else
            {
                intti = 0;
                //imageamount.Value = FileBox.Items.Count;
                imageAmount2.Text = "/ " + FileBox.Items.Count.ToString();
                currentimage.Value = intti;

                prevbtn.Enabled = true;
                nxtbtn.Enabled = true;
                TagSearch.Enabled = true;
                currentimage.Enabled = true;
                diatimeSel.Enabled = true;
                }
            }
            else
            {
                //MessageBox.Show("Cannot refresh.  No folder selected.", "Cannot refresh", 0, MessageBoxIcon.Error);
                MessageBoxer.Show("Cannot refresh.  No folder selected.", "Cannot refresh", MessageBoxIcon.Error);
            }
        }

        private void currentimage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                intti = Convert.ToInt32(currentimage.Value);
                if (Convert.ToInt32(currentimage.Value) < 0)
                {
                    //MessageBox.Show("Value must be positive", "Negative value", 0, MessageBoxIcon.Error);
                    MessageBoxer.Show("Value must be positive", "Negative value", MessageBoxIcon.Error);
                    currentimage.Value = intti;
                }
                //else if (Convert.ToInt32(currentimage.Value) > Convert.ToInt32(imageamount.Value))
                else if (currentimage.Value > Convert.ToInt32(imageAmount2.Text.Remove(0, 1)))
                {
                    //MessageBox.Show("Value must be lower than the count of found images", "Value too high", 0, MessageBoxIcon.Error);
                    MessageBoxer.Show("Value must be lower than the count of found images", "Value too high", MessageBoxIcon.Error);
                    currentimage.Value = intti;
                }
                else
                {
                    gsImages();
                }
            }
        }
        private void TagSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                searchwithtag(TagSearch.Text);
            }
        }
        public void searchwithtag(string text)
        {
            //nyt pitäis jotenkin saada jokaikinen tägi tonne alempaan if lauseeseen että se kattoo onko tiedoston metadatasa nää tägit (text)
            FileBox.Items.Clear();

            //en tiä miksi fixedtext string:in pois ottaminen ja kaikki missä sitä käytettiin vaihtaminen text string:iin toimii
            if (text == "")
            {
                string[] files = Directory.GetFiles(valittukansio2);

                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    ListViewItem item = new ListViewItem(fileName);

                    if (file.Contains(".jpg"))
                    {
                        item.Tag = file;

                        FileBox.Items.Add(item);

                    }
                }
            }
            else if (text == "notag")
            {
                string[] files = Directory.GetFiles(valittukansio2);
                foreach (string file in files)
                {
                    if (file.Contains(".jpg"))
                    {
                        string fileName = Path.GetFileName(file);
                        ListViewItem item = new ListViewItem(fileName);
                        var imagetags = ImageFile.FromFile(file).Properties.Get(ExifTag.WindowsKeywords);
                        string imagetags2 = "";
                        if (imagetags != null)
                        {
                            imagetags2 = imagetags.ToString();
                        }

                        if (file.Contains(".jpg") && imagetags == null || imagetags2 == "")
                        {
                            item.Tag = file;
                            FileBox.Items.Add(item);
                            Debug.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                            Debug.WriteLine("--------------------------------------------------------------");
                        }
                    }
                }
                intti = 1;
                imageAmount2.Text = "/ " + FileBox.Items.Count.ToString();
                //imageamount.Value = FileBox.Items.Count;
            }
            else if (text != "notag" && text != "")
            {
                if (text[0].Equals(';'))
                {
                    Debug.WriteLine(text);
                    text = text.Remove(0, 1);
                    Debug.WriteLine(text);
                }

                char lastchar = text.Last();

                if (lastchar.Equals(';'))
                {
                    Debug.WriteLine(text);
                    text = text.Remove(text.Length - 1);
                    Debug.WriteLine(text);
                }

                string[] Xtags2 = text.Split(';');

                int count = Xtags2.Length - 1;
                int tloop;
                int sloop = 0;
                var b = 1;

                //MessageBox.Show("vittu haloo\n"+b);
                while (b == 1)
                {
                    //MessageBox.Show($"tagsi{1}" + $"tagsi{1}");
                    //ota kaikki kuvat jotka sisältää tägin1 ja lisää listalle.  kun kaikki kuvat on lisätty mene listan kuvat läpi katsoen onko niissä tägi2,3,4,5,6 jne ja poista listalta jos ei ole???
                    //vittu kun aivot sulああああああああああああ
                    //käy kuvat läpi yksikerrallaan,  jos kuva sisältää tägin yksi kokeile sisältääkö se tägin 1,2,3,4 jne jos joo, lisää listalle, jos ei niin ohita ja mene seuraavaan kuvaan <- jos toimii niin ehkä nopeampi

                    string[] files = Directory.GetFiles(valittukansio2);
                    int fcount = files.Length;
                    //MessageBox.Show("mitä vittua");

                    if (sloop >= fcount - 1)
                    {
                        //MessageBox.Show("No images were found with the tags \"" + text + "\"", "No found images", 0, MessageBoxIcon.Error);
                        MessageBoxer.Show("No images were found with the tags \"" + text + "\"", "No found images", MessageBoxIcon.Error);
                        b--;
                        break;
                    }
                    else
                    {
                        foreach (string file in files)
                        {
                            //MessageBox.Show("vittu pääseekö se mihinkään");
                            string fileName = Path.GetFileName(file);
                            ListViewItem item = new ListViewItem(fileName);

                            if (file.EndsWith(".jpg"))
                            {
                                var gottenTags = ImageFile.FromFile(file).Properties.Get(ExifTag.WindowsKeywords);
                                if (gottenTags != null && gottenTags.ToString() != "") //huh saatana piti lisätä toi toinen ehto tohon koska muuten jos jollain oli täginä "" niin se paska kaatu
                                {
                                    string gottenTags2 = gottenTags.ToString();
                                    char gt2 = gottenTags2[0];

                                    if (!gottenTags2[0].Equals(';'))
                                    {
                                        gottenTags2 = ";" + gottenTags2;
                                    }
                                    //tää saatanan paska koodin pätkä toimmii mutta se ärsyttää iha vitusteen koska tämä on mahollisesti pirun hidas jos kuvia on vähänkään enemmän
                                    //tää vitun paska lyhyt perkele kattoo joka kuvasta joka valitun tägin sen sijasta että se lopettaisi edes yrittämisen jos yksikään tägi ei löydy
                                    //perkele kun käytin joku 10 vitullista tuntia tonne alempaan pois kommentoituun kohtaan koska se periaate oli hyvä mutta en vaan saanut toimimaan
                                    //enemmällä kuin kahdella tägillä vittu perkele saatana kirosana jumalauta >>>>>>>>>>::::::::(((((((((((((((
                                    //tl:dr vituttaa paska koodi

                                    // JOOOOOOO Just parasta jeebou juuh eliikkääs "break;" tonne else:n sisälle korjas ongelman :DDD olo vitun tyhmä
                                    tloop = 0;

                                    foreach (var tagÅ in Xtags2)
                                    {
                                        if (gottenTags2.Contains(";" + tagÅ + ";"))
                                        {
                                            tloop++;
                                            Debug.WriteLine(file + "\n\n" + ";" + tagÅ + ";" + "\n\n" + gottenTags2 + "\n" + TagSearch.Text + "\n" + tloop + "/" + count);
                                            if (tloop > count)
                                            {
                                                tloop = 0;
                                                item.Tag = file;
                                                FileBox.Items.Add(item);
                                                Debug.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                                            }
                                            b--;//tää estää sen ettei se jää loputtomaan looppiin jumihin
                                        }
                                        else
                                        {

                                            if (tloop == 0)
                                            {
                                                sloop++;
                                            }
                                            else
                                            {
                                                sloop = sloop + tloop;
                                            }
                                            tloop = 0;
                                            Debug.WriteLine(file + "\nXXX\n" + ";" + tagÅ + ";" + "\n\n" + gottenTags2 + "\n" + TagSearch.Text + "\n" + tloop + "/" + count + "(" + sloop + ")");
                                            break;
                                        }
                                    }
                                    Debug.WriteLine("--------------------------------------------------------------");
                                }
                                else
                                {
                                    Debug.WriteLine(file + "\nYYYYY no tags\n(" + sloop + ")\n--------------------------------------------------------------");
                                    sloop++;
                                }
                            }
                            intti = 1;
                            //imageamount.Value = FileBox.Items.Count;
                            imageAmount2.Text = "/ " + FileBox.Items.Count.ToString();
                        }
                    }
                }/*
                if (FileBox.Items.Count == 0)
                {
                    MessageBox.Show("No images were found with the tags \"" + text + "\"", "No found images", 0, MessageBoxIcon.Error);
                }*/
            }

        }
        public void gsImages()
        {
            if (intti > FileBox.Items.Count)
            {
                intti = 1;
            }
            if (intti <= 0)
            {
                intti = FileBox.Items.Count;
            }
            ListViewItem item = FileBox.Items[intti - 1];
            valittukuva = valittukansio2 + "/" + item.SubItems[0].Text;
            valittukuva2 = item.SubItems[0].Text;

            CurrentFile.Text = valittukuva2;
            if (qdon)
            {
                curtab = qdct;
            }
            Metat();
            ThePicture.Image = Image.FromFile(@valittukuva);
            if (qdon)
            {
                quickdataX();
            }
        }

        private void descriptionMTDT_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show(intti.ToString() + " - (" + intti.ToString() + " * " + "2) = " + (intti - (intti * 2)).ToString());
                
            if (valittukuva2.EndsWith(".png"))
            {
                MessageBoxer.Show("Cannot edit PNG metadata.", "PNG image", MessageBoxIcon.Exclamation);
                return;
            }
                qdused = false;
                descriptionMTDT.ClearSelection();
                System.Threading.Thread.Sleep(300);
                updatemtdt(e.RowIndex);
                descriptionMTDT.Update();
                descriptionMTDT.Refresh();
        }
        public void updatemtdt(int x)
        {
            /*string vittu = descriptionMTDT.CurrentCell.RowIndex.ToString();
            string perkele = descriptionMTDT.Rows[descriptionMTDT.CurrentCell.RowIndex].Cells[1].Value.ToString();
            MessageBox.Show(vittu+"\n\n"+perkele);*/

            //int x = 0;
            ThePicture.Refresh();
            string celldata;
            int exiftype = 0;

            if (!qdused)
            {
                exiftype = Int32.Parse(descriptionMTDT.Rows[x].Cells[2].Value.ToString());
                Debug.WriteLine(x);
            } else
            {
                qdused = true;
            }
            
            Unohdakuva();

            ImageFile file;
            if (!qdused)
            {
                file = ImageFile.FromFile(valittukuva);
                qdused = false;
            } else
            {
                //file = ImageFile.FromFile(quickdata.qdkuva);
                file = ImageFile.FromFile(valittukuva);
                qdused = true;
                Debug.WriteLine("rwcnt "+rowcount.ToString());
            }
            int rowcount2 = rowcount;
            while (rowcount2 > 0)
            {
                if (qdused)
                {
                    rowcount2 = 0;
                    Debug.WriteLine("senkin sadisti");
                }
                else
                {
                    Debug.WriteLine("mittarilla voit testata");
                    rowcount2--;
                }
                //MessageBox.Show(row.ToString());
                if (!qdused)
                {
                    Debug.WriteLine(x);
                    if (descriptionMTDT.Rows[x].Cells[1].Value == null)
                    {
                        celldata = "";
                        Debug.WriteLine("kivesneste"); //mikä helvetin mieliala pitää olla jotta laittaa debug viestiksi "kivesneste"????
                    }
                    else
                    {
                        celldata = descriptionMTDT.Rows[x].Cells[1].Value.ToString();
                    }
                }
                else
                {
                    //MessageBox.Show(quickdata.qdedited);
                    if (quickdata.qdedited == null)
                    {
                        celldata = "";
                        Debug.WriteLine("kivesneste"); //mikä helvetin mieliala pitää olla jotta laittaa debug viestiksi "kivesneste"????
                    }
                    else
                    {
                        celldata = quickdata.qdedited.ToString();
                    }
                }
                Debug.WriteLine(celldata);

                if (!qdused)
                {
                    exiftype = Int32.Parse(descriptionMTDT.Rows[x].Cells[2].Value.ToString());
                }
                else
                {
                    exiftype = Int32.Parse(quickdata.qdtype);
                }

                //VITTUUUU MITEN SAAN TÄN TOIMIMAaN ILMAAN KAHEKSAA SATAA IF LAUSETTA PERKELEEN C# KUN SE EI OLE PHP JOSSA TÄTÄ ONGELMAA EI OLISI PERKELE

                /*string stringit = "ExifTag." + exiftype;
                ExifLibrary.ExifTag stringit2 = "ExifTag." + exiftype;
                //ExifLibrary.ExifTag stringit = "ExifTag."+exiftype;

                file.Properties.Set(stringit2, celldata);*/

                //tällästä if lause sotkua tulee kun C# on tyhmä kieli. saisin tän muunmuassa nyt just valmiiksi mutten suostu täyttää mun jo paskaa koodia if lauseilla
                //noita perkeleitähän tulee yhteensä noin 26. pelkkä ajatus 26 if lauseesta miltein peräkkäin saa mut kääntymään satanismiin
                /*
                if (x == 0)
                {
                    file.Properties.Set(ExifTag.WindowsTitle, celldata);
                }
                else if (x == 1)
                {
                    file.Properties.Set(ExifTag.WindowsSubject, celldata);
                }
                else if (x == 2)
                {
                    file.Properties.Set(ExifTag.Rating, celldata);
                }
                else if (x == 3)
                {
                    file.Properties.Set(ExifTag.WindowsKeywords, celldata);
                }
                else if (x == 4)
                {
                    file.Properties.Set(ExifTag.WindowsComment, celldata);
                }*/

                //EI KIISSELISET MEISSELIT SENTÄÄN SE TOIMII! EIKÄ SIINÄ OLE KUIN YKSI IF LAUSE JEE BOING JIHUU
                //ongelman ratkaisi kun olin kirjottanut "exiftag.windows" niin siinä oli tullu se teksti-boksi jossa näky joku numero minkä oletin olevan sama juttu kuin se nimi mutta numerona (yllätys)
                //jees saatana

                Debug.WriteLine("KIISSELI");
                //Debug.WriteLine(exiftype);

                int kokki = 0;
                if (!qdused)
                {
                    kokki = Int32.Parse(descriptionMTDT.Rows[x].Cells[2].Value.ToString());
                    //MessageBox.Show(kokki + " a");
                }
                else
                {
                    kokki = Int32.Parse(quickdata_type);
                    //MessageBox.Show(kokki + " b");
                }

                if (kokki != exiftype)
                { break; }
                //{
                    file.Properties.Set((ExifTag)exiftype, celldata); 
                    //-------kaatuu ylemmälle riville quickdataa käyttäessä "specified cast is not valid". tuo "exiftype" variable lienee jotenkin rikki qd:ta käyttäessä-------
                    //sinänsä saattais toimia jos mä ottaisin sen qd:hen laitetun jutun tonne gridview:iin ja ottaisin sen uudestaan sieltä
                    Debug.WriteLine("\n" + exiftype + " / " + celldata + "\nMEISSELI");

                    if ((int)exiftype == 236867)
                    {
                        if (celldata == null || celldata == "")
                        {
                            file.Properties.Set((ExifTag)exiftype, "");
                        }
                        else
                        {
                            file.Properties.Set((ExifTag)exiftype, DateTime.ParseExact(celldata, "yyyy.MM.dd HH.mm.ss", System.Globalization.CultureInfo.InvariantCulture));
                        }
                    }

                    // x = x;  huom tän virka on olla ns "breakpoint":ina kun tuo vittuilee nytten
                    //tää on aika paskaa koodia mutta jotenkin on tarkistettava ollaanko muuttamassa tota datetimeoriginal höskää koska se celldata pitää muuttaa datetimeksi

                    //-IF- SWITCH LAUSE SOTKUA KOSKA PERKELEEN ENUM:IT
                    //tää toimii mutta ei ole käyttäjäystävällinen koska pitää tietää tismalleen miten tuo exiflib tahtoo nuo tiedot. properties > data "flash" voi olla esim 'flash' meinaten että se oli päällä
                    //mutta jos haluaa sen laittaa tolla ohjelmalla niin pitää kirjoittaa 'FlashFired' ja kyllä tismalleen noin isot kirjaimet ja ei välejä ja kaiken kukkuraksi tää on hirveä if lause soppa
                    //saattais toimia jos mä laita ton gridview:in päälle parit dropdown höskät jotka menee näkyviin tarvittaessa ja piiloon muutoin ja riippúen curtab:istä niin se laittaisi tietyt tiedot niihin dropdown:eihin

                    switch (exiftype)
                    {
                        case 237383: //MeteringMode
                            switch (celldata)
                            {
                                case null:
                                case "":
                                    //doesn't save anything because the cell is null or otherwise blank
                                    break;

                                default:
                                    file.Properties.Set((ExifTag)exiftype, Enum.Parse(typeof(MeteringMode), celldata));
                                    break;
                            }
                            break;

                        case 237385: //Flash
                            switch (celldata)
                            {
                                case null:
                                case "":
                                    //doesn't save anything because the cell is null or otherwise blank
                                    break;

                                default:
                                    file.Properties.Set((ExifTag)exiftype, Enum.Parse(typeof(Flash), celldata));
                                    break;
                            }
                            break;

                        case 241992: //Contrast
                            switch (celldata)
                            {
                                case null:
                                case "":
                                    //doesn't save anything because the cell is null or otherwise blank
                                    break;

                                default:
                                    file.Properties.Set((ExifTag)exiftype, Enum.Parse(typeof(Contrast), celldata));
                                    break;
                            }
                            break;

                        case 237384: //LigtSource
                            switch (celldata)
                            {
                                case null:
                                case "":
                                    //doesn't save anything because the cell is null or otherwise blank
                                    break;

                                default:
                                    file.Properties.Set((ExifTag)exiftype, Enum.Parse(typeof(LightSource), celldata));
                                    break;
                            }
                            break;

                        case 234850: //ExposureProgram
                            switch (celldata)
                            {
                                case null:
                                case "":
                                    //doesn't save anything because the cell is null or otherwise blank
                                    break;

                                default:
                                    file.Properties.Set((ExifTag)exiftype, Enum.Parse(typeof(ExposureProgram), celldata));
                                    break;
                            }
                            break;

                        case 241993: //Saturation
                            switch (celldata)
                            {
                                case null:
                                case "":
                                    //doesn't save anything because the cell is null or otherwise blank
                                    break;

                                default:
                                    file.Properties.Set((ExifTag)exiftype, Enum.Parse(typeof(Saturation), celldata));
                                    break;
                            }
                            break;

                        case 241994: //Sharpness
                            switch (celldata)
                            {
                                case null:
                                case "":
                                    //doesn't save anything because the cell is null or otherwise blank
                                    break;

                                default:
                                    file.Properties.Set((ExifTag)exiftype, Enum.Parse(typeof(Sharpness), celldata));
                                    break;
                            }
                            break;

                        case 241987: //WhiteBalance
                            switch (celldata)
                            {
                                case null:
                                case "":
                                    //doesn't save anything because the cell is null or otherwise blank
                                    break;

                                default:
                                    file.Properties.Set((ExifTag)exiftype, Enum.Parse(typeof(WhiteBalance), celldata));
                                    break;
                            }
                            break;
                    }
                //}
                //x++; tää rikko sen.  oli jääny yli vanhasta koodista
            }

            if (!qdused)
            {
                Unohdakuva();
                file.Save(valittukansio2 + "/" + valittukuva2);
            }
            else
            {
                //quickdata.qdkuva.Dispose();
                quickdata d = new quickdata();
                d.alzheimer();

                //MessageBox.Show(valittukansio2 + "/" + valittukuva2);

                //file.Save(/*quickdata.qdkansio + "/" + */quickdata.qdkuva2);

                //tää ei pysty disposaan sitä pictureboxin kuvaa qd:sta käsin joka estää tän toimimisen

                this.Unohdakuva();
                PictureBox.Dispose();
                ThePicture.Dispose();

                PictureBox.Image = Image.FromFile(@"C:/Users/Bamsemums/Desktop/työt/Csharp/hikikomero/testiä/sumutorvi.png");
                //intti = intti;

                /*C:\\Users\\Bamsemums\\Desktop\\työt\\Csharp\\hikikomero\\testiä\\
                intti = intti + quickdata.qdintti;
                    previmage();
                try
                {
                    previmage();
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    MessageBoxer.Show("splat: "+intti, "vittu", MessageBoxIcon.Error);
                }*/
                try
                {
                    file.Save(valittukansio2 + "/" + valittukuva2);
                }
                catch (System.IO.IOException)
                {
                    //MessageBox.Show("Se kusinen paska ei onnistunut disposata sitä pictureboxin kuvaa prkl", "vittu", 0, MessageBoxIcon.Error);
                    MessageBoxer.Show("Se kusinen paska ei onnistunut \"disposata\" sitä pictureboxin kuvaa prkl.  se ei vaan tiedä että se PictureBox on olemassa vaikkakin se vetää imageMTDT:n koodia jossa se PictureBox on.  vaikkei se tiedä että se on olemassa niin se ei valita että \"mikä vittu tää on\" kun sen yrittää \"disposata\".", "vittu", MessageBoxIcon.Error);
                }
            }/*
            try
            {
                nextimage();
            }
            catch (System.ArgumentOutOfRangeException)
            {
                MessageBoxer.Show("splat2: " + intti, "vittu", MessageBoxIcon.Error);
            }*/
            ThePicture.Image = Image.FromFile(@valittukuva);
            Metat();
            //quickdataX();
            Debug.WriteLine("------------------------------------------------------------------");
        }
        private void descriptionMTDT_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            
        }
        private void tab_description_Click(object sender, EventArgs e)
        {
            curtab = 0;
            Metat();
        }

        private void tab_origin_Click(object sender, EventArgs e)
        {
            curtab = 1;
            Metat();
        }

        private void tab_camera_Click(object sender, EventArgs e)
        {
            curtab = 2;
            Metat();
        }

        private void tab_ap_Click(object sender, EventArgs e)
        {
            curtab = 3;
            Metat();
        }

        private void descriptionMTDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            //miten vitaleeessa saan ton quickdata_Load() event:in käynnistettyä täältä imagemtdt:stä >:[
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                if (e.KeyChar == 17)
                {
                    //avaa quickdata ja lähetä valitun rivin tiedot

                    Int32 selectedCellCount = descriptionMTDT.GetCellCount(DataGridViewElementStates.Selected);
                    if (selectedCellCount == 3)
                    {
                        int o = 2;
                        qdon = true;
                        qdrow = descriptionMTDT.SelectedCells[o].RowIndex;
                        qdct = curtab;
                        quickdataX();
                    }
                }
            }/*
            if (e.KeyCode == Keys.Enter)
            {
                qdused = false;
                updatemtdt();
            }*/
        }
        public void quickdataX()
        {/*
            var file = ImageFile.FromFile(valittukuva);
            descriptionMTDT.Rows.Add("Title", file.Properties.Get(ExifTag.WindowsTitle), 140091);
            if (quickdata_type != null)
            {
                quickdata_type = descriptionMTDT.Rows[qdrow].Cells[2].Value.ToString();
            }
            else
            {
                quickdata_type = file.Properties.Get((ExifTag)exiftype);
            }*/
            switch (descriptionMTDT.Rows[qdrow].Cells[1].Value) //tää estää kaatumisen jos solu 2(1) on null
            {
                case null:
                    quickdata_data = "";
                    break;

                default:
                    quickdata_data = descriptionMTDT.Rows[qdrow].Cells[1].Value.ToString();
                    break;
            }
            quickdata_name = descriptionMTDT.Rows[qdrow].Cells[0].Value.ToString();
            quickdata_type = descriptionMTDT.Rows[qdrow].Cells[2].Value.ToString();
            Debug.WriteLine("sr: " + qdrow.ToString() + "\n" + "qdt: " + quickdata_type + "\n" + "qdn: " + quickdata_name);

            _d.Show();
            _d.getdata();
        }
        private void start_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void start_DragDrop(object sender, DragEventArgs e)
        {
            string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            valittukansio2 = fileList[0];
            ChosenFolder.Text = fileList[0];

            FileBox.Items.Clear();

            string[] files = Directory.GetFiles(fileList[0]);

            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                ListViewItem item = new ListViewItem(fileName);

                if (file.Contains(".jpg") || file.Contains(".png") && showpngs/* || file.Contains(".png")*/)
                {
                    item.Tag = file;

                    FileBox.Items.Add(item);
                }
            }
            if (FileBox.Items.Count == 0)
            {
                //MessageBox.Show("No JPG images were found in the path \"" + valittukansio2 + "\"", "No JPGs", 0, MessageBoxIcon.Information);
                MessageBoxer.Show("No JPG images were found in the path \"" + valittukansio2 + "\"", "No JPGs", MessageBoxIcon.Information);
            }
            else
            {
                intti = 0;
                //imageamount.Value = FileBox.Items.Count;
                imageAmount2.Text = "/ " + FileBox.Items.Count.ToString();
                currentimage.Value = intti;

                prevbtn.Enabled = true;
                nxtbtn.Enabled = true;
                TagSearch.Enabled = true;
                currentimage.Enabled = true;
                diatimeSel.Enabled = true;
            }
        }

        private void TagSearch_MouseClick(object sender, MouseEventArgs e)
        {
            if(TagSearch.Text == "Search images with tags")
            TagSearch.Text = "";
        }

        private void refreshbtn_Click(object sender, EventArgs e)
        {
            refresh();
        }
        public void fcsTagSearch()
        {
            TagSearch.Focus();
            if (TagSearch.Text == "Search images with tags")
                TagSearch.Text = "";
        }

        private void FileBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("dc");
            int row = 1;
            foreach (var i in FileBox.Items)
            {
                Debug.WriteLine(row+"\n"+ FileBox.SelectedItems[0] + "\n" +i);
                if (i == FileBox.SelectedItems[0])
                {
                    //MessageBox.Show("File: "+FileBox.SelectedItems[0].Text+"\nFound on row: "+row);
                    Debug.WriteLine(row+" joo");
                    Unohdakuva();
                    intti = row;

                    //imageamount.Value = FileBox.Items.Count;
                    imageAmount2.Text = "/ " + FileBox.Items.Count.ToString();
                    currentimage.Value = intti;

                    gsImages();

                    prevbtn.Enabled = true;
                    nxtbtn.Enabled = true;
                    TagSearch.Enabled = true;
                    currentimage.Enabled = true;
                    diatimeSel.Enabled = true;
                    break; //pomppaa pois foreach loopista kun kuva on löydetty.  estää turhan työn
                }
                row++;
            }

        }

        private void showpng_Click(object sender, EventArgs e)
        {
            if (showpngs)
            {
                showpngs = false;
                showpng.Text = "Show PNGs";
                if (valittukansio2 != null)
                    refresh();
            }
            else 
            { 
                showpngs = true;
                showpng.Text = "Hide PNGs";
                if (valittukansio2 != null)
                    refresh();
            }
        }
        //Timer _timeri = new Timer();
        private void diatimeSel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter)
            {
                return;
            }
            if (diatimeSel.Value < 1)
            {
                timer1.Enabled = false;
                timer1.Dispose();
                return;
            }

            timer1.Interval = (int)diatimeSel.Value;
            if (!timer1.Enabled)
            {
                timer1.Enabled = false;
                timer1.Dispose();
                timer1.Enabled = true;
            }
            else
            {
                timer1.Enabled = false;
                timer1.Dispose();
                return;
            }

            //timer1.Tick += changedia;
        }
        public void changedia(Object myObject, EventArgs myEventArgs)
        {
            if(FileBox.Items.Count > 0)
            {
                nextimage();
            }
            else { return; }
        }
    }
}