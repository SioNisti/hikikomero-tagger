using ExifLibrary;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Directory = System.IO.Directory;

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

            //GlobalHotKey.RegisterHotKey("CTRL + Right", () => nextimage());//seuraava kuva
            //GlobalHotKey.RegisterHotKey("CTRL + Left", () => previmage());//edellinen kuva
            GlobalHotKey.RegisterHotKey("CTRL + N", () => nextimage()); //seuraava kuva
            GlobalHotKey.RegisterHotKey("CTRL + B", () => previmage());//edellinen kuva
            GlobalHotKey.RegisterHotKey("CTRL + S", () => fcsTagSearch());//laittaa focusin siihen tägihaku lootaan
            GlobalHotKey.RegisterHotKey("CTRL + T", () => fcsTagAdd());//laittaa focusin siihen tägihaku lootaan
            GlobalHotKey.RegisterHotKey("CTRL + D", () => Kakapylytoimi());//valitse kansio
            GlobalHotKey.RegisterHotKey("CTRL + R", () => refresh());//valitsee saman kansion uudestaan ns "päivittääkkseen" tiedosto listan
            GlobalHotKey.RegisterHotKey("CTRL + DELETE", () => DelImg());//poistaa valitun kuvan
            GlobalHotKey.RegisterHotKey("CTRL + Q", () => QuickArtist());//ottaa tiedostonimestä kaiken ennen ensimmäistä "-" ja laittaa kuvan Artist:ksi
            GlobalHotKey.RegisterHotKey("CTRL + Y", () => fcsArtistAdd());//Focusaa artist boksiin
            GlobalHotKey.RegisterHotKey("CTRL + H", () => fcsHop());//Focusaa image hopperiin

        }
        public PictureBox ThePicture
        {
            get { return PictureBox; }
        }

        public static string valittukansio2;
        public static string valittukuva;
        public static string valittukuva2;

        public static int intti = 0;

        public static bool showpngs = false;

        private void imageMTDT_Load(object sender, EventArgs e)
        {
            TagSearch.Enabled = false;
            currentimage.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
        }
        public void Unohdakuva()
        {
            var bit = new Bitmap(this.Width, this.Height);
            var g = Graphics.FromImage(bit);

            var oldImage = ThePicture.Image;
            ThePicture.Image = bit;
            oldImage?.Dispose();

            g.Dispose();
        }
        public void nextimage()
        {
            if (valittukansio2 == null)
            {
                if (!MessageBoxer.IsOpen)
                {
                    MessageBoxer.Show("No images found", "No images", MessageBoxIcon.Error);

                    return;
                }
                if (MessageBoxer.IsOpen)
                {
                    return;
                }
            }
            Unohdakuva();

            intti++;

            if (intti == FileBox.Items.Count + 1)
            {
                intti = 1;
            }

            imageAmount2.Text = "/ "+FileBox.Items.Count.ToString();
            currentimage.Value = intti;

            if (FileBox.Items.Count == 1)
            {
                currentimage.Value = 1;
                gsImages();
            }
            gsImages();
            Debug.WriteLine(intti);
            TagSearch.Enabled = true;
            currentimage.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
        }
        public void previmage()
        {
            if(valittukansio2 == null)
            {
                if (!MessageBoxer.IsOpen)
                {
                    MessageBoxer.Show("No images found", "No images", MessageBoxIcon.Error);

                    return;
                }
                if (MessageBoxer.IsOpen)
                {
                    return;
                }
            }
            Unohdakuva();
            intti--;

            imageAmount2.Text = "/ " + FileBox.Items.Count.ToString();
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
            TagSearch.Enabled = true;
            currentimage.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
        }

        private void ToolStripButton1_Click_1(object sender, EventArgs e)
        {
            Kakapylytoimi();
        }

        public void Kakapylytoimi()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
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

                    if (file.Contains(".jpg") || file.Contains(".png") && showpngs)
                    {
                        item.Tag = file;

                        FileBox.Items.Add(item);
                    }
                }

            }
            if (FileBox.Items.Count == 0)
            {
                MessageBoxer.Show("No JPG images were found in the path \"" + valittukansio2 + "\"", "No JPGs", MessageBoxIcon.Information);
            }
            else
            {
                intti = 0;
                imageAmount2.Text = "/ " + FileBox.Items.Count.ToString();
                currentimage.Value = intti;

                TagSearch.Enabled = true;
                currentimage.Enabled = true;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
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

                    if (file.Contains(".jpg") || file.Contains(".png") && showpngs)
                    {
                        item.Tag = file;

                        FileBox.Items.Add(item);
                    }
                }
                if (FileBox.Items.Count == 0)
                {
                    MessageBoxer.Show("No JPG images were found in the path \"" + valittukansio2 + "\"", "No JPGs", MessageBoxIcon.Information);
                }
                else
                {
                    intti = 0;
                    imageAmount2.Text = "/ " + FileBox.Items.Count.ToString();
                    currentimage.Value = intti;
                    TagSearch.Enabled = false;
                    currentimage.Enabled = false;
                    textBox1.Enabled = false;
                    textBox2.Enabled = false;
                    label1.Text = "Tags: ";
                }
            }
            else
            {
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
                    MessageBoxer.Show("Value must be positive", "Negative value", MessageBoxIcon.Error);
                    currentimage.Value = intti;
                }
                else if (currentimage.Value > Convert.ToInt32(imageAmount2.Text.Remove(0, 1)))
                {
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
            FileBox.Items.Clear();
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

                while (b == 1)
                {
                    string[] files = Directory.GetFiles(valittukansio2);
                    int fcount = files.Length;

                    if (sloop >= fcount - 1)
                    {
                        MessageBoxer.Show("No images were found with the tags \"" + text + "\"", "No found images", MessageBoxIcon.Error);
                        b--;
                        break;
                    }
                    else
                    {
                        foreach (string file in files)
                        {
                            string fileName = Path.GetFileName(file);
                            ListViewItem item = new ListViewItem(fileName);

                            if (file.EndsWith(".jpg"))
                            {
                                var gottenTags = ImageFile.FromFile(file).Properties.Get(ExifTag.WindowsKeywords);
                                if (gottenTags != null && gottenTags.ToString() != "")
                                {
                                    string gottenTags2 = gottenTags.ToString();
                                    char gt2 = gottenTags2[0];

                                    if (!gottenTags2[0].Equals(';'))
                                    {
                                        gottenTags2 = ";" + gottenTags2;
                                    }
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
                                            b--;
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
                            imageAmount2.Text = "/ " + FileBox.Items.Count.ToString();
                        }
                    }
                }
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
            label4.Text = valittukuva2;
            ThePicture.Image = Image.FromFile(@valittukuva);
            var file = ImageFile.FromFile(valittukuva);

            if (file.Properties.Get(ExifTag.WindowsKeywords) != null)
            {
                label1.Text = "Tags: " + file.Properties.Get(ExifTag.WindowsKeywords).ToString();
            } else
            {
                label1.Text = "Tags: ";
            }
            if (file.Properties.Get(ExifTag.Artist) != null) { 
                label2.Text = "Artist: " + file.Properties.Get(ExifTag.Artist).ToString();
            }
            else
            {
                label2.Text = "Artist: ";
            }
            System.Drawing.Image img = System.Drawing.Image.FromFile(valittukuva);
            label3.Text = img.Width+ "x" + img.Height;
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

                if (file.Contains(".jpg") || file.Contains(".png") && showpngs)
                {
                    item.Tag = file;

                    FileBox.Items.Add(item);
                }
            }
            if (FileBox.Items.Count == 0)
            {
                MessageBoxer.Show("No JPG images were found in the path \"" + valittukansio2 + "\"", "No JPGs", MessageBoxIcon.Information);
            }
            else
            {
                intti = 0;
                imageAmount2.Text = "/ " + FileBox.Items.Count.ToString();
                currentimage.Value = intti;

                TagSearch.Enabled = true;
                currentimage.Enabled = true;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
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
        public void DelImg()
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this image?\n" + valittukuva, "Delete image?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning); ;
            if (dialogResult == DialogResult.Yes)
            {
                Unohdakuva();
                File.Delete(valittukansio2 + "/" + valittukuva2);
                refresh();
            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }
        }
        public void QuickArtist()
        {
            var file = ImageFile.FromFile(valittukuva);
            string[] kahvi = valittukuva2.Split('-');
            var artist = kahvi[0];
            Unohdakuva();
            file.Properties.Set(ExifTag.Artist, artist);
            file.Save(valittukansio2 + "/" + valittukuva2);

            ThePicture.Image = Image.FromFile(@valittukuva);
            if (file.Properties.Get(ExifTag.WindowsKeywords) != null)
                label1.Text = "Tags: " + file.Properties.Get(ExifTag.WindowsKeywords).ToString();

            if (file.Properties.Get(ExifTag.Artist) != null)
                label2.Text = "Artist: " + file.Properties.Get(ExifTag.Artist).ToString();
            gsImages();

        }
        public void fcsTagAdd()
        {
            textBox1.Focus();
        }
        public void fcsArtistAdd()
        {
            textBox2.Focus();
        }
        public void fcsHop()
        {
            currentimage.Focus();
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
                    Debug.WriteLine(row+" joo");
                    Unohdakuva();
                    intti = row;

                    imageAmount2.Text = "/ " + FileBox.Items.Count.ToString();
                    currentimage.Value = intti;

                    gsImages();

                    TagSearch.Enabled = true;
                    currentimage.Enabled = true;
                    textBox1.Enabled = true;
                    textBox2.Enabled = true;
                    break;
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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Unohdakuva();
                string tags = textBox1.Text;

                if (tags == null)
                {
                    return;
                }

                var file = ImageFile.FromFile(valittukuva);
                if (!tags.StartsWith("-"))
                {
                    Unohdakuva();
                    if (tags.StartsWith(";"))
                    {
                        tags = tags.Remove(0,1);
                    }
                    if (!tags.EndsWith(";"))
                    {
                        tags = tags + ";";
                    }
                    if (file.Properties.Get(ExifTag.WindowsKeywords) != null)
                    {
                        tags = file.Properties.Get(ExifTag.WindowsKeywords).ToString() + tags;
                    }
                } else
                {
                    Unohdakuva();
                    tags = tags.Remove(0, 1);
                    tags = tags+";";
                    var oldtags = file.Properties.Get(ExifTag.WindowsKeywords).ToString();
                    tags = oldtags.Replace(tags, "");
                }

                Unohdakuva();
                file.Properties.Set(ExifTag.WindowsKeywords, tags);

                try
                {
                    Unohdakuva();
                    file.Save(valittukansio2 + "/" + valittukuva2);
                }
                catch (System.UnauthorizedAccessException)
                {
                    MessageBoxer.Show("Couldn't save image. \"Can't access file\"\nmake sure the folder/image isn't marked as \"hidden\"", "Error", MessageBoxIcon.Error);
                }

                ThePicture.Image = Image.FromFile(@valittukuva);
                if (file.Properties.Get(ExifTag.WindowsKeywords) != null)
                    label1.Text = "Tags: " + file.Properties.Get(ExifTag.WindowsKeywords).ToString();
                    gsImages();
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Unohdakuva();
                if (textBox2.Text == null)
                {
                    return;
                }

                var file = ImageFile.FromFile(valittukuva);

                Unohdakuva();
                file.Properties.Set(ExifTag.Artist, textBox2.Text);

                try
                {
                    Unohdakuva();
                    file.Save(valittukansio2 + "/" + valittukuva2);
                }
                catch (System.UnauthorizedAccessException)
                {
                    MessageBoxer.Show("Couldn't save image. \"Can't access file\"\nmake sure the folder/image isn't marked as \"hidden\"", "Error", MessageBoxIcon.Error);
                }

                ThePicture.Image = Image.FromFile(@valittukuva);
                if (file.Properties.Get(ExifTag.Artist) != null)
                    label2.Text = "Artist: " + file.Properties.Get(ExifTag.Artist).ToString();
                gsImages();
            }
        }
    }
}