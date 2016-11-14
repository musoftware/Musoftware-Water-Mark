using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Musoftware_Water_Mark
{
    public partial class Frmmain : Form
    {
        public Frmmain()
        {
            InitializeComponent();
        }

        private void vistaButton1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) != System.Windows.Forms.DialogResult.Cancel)
            {
                textBox1.Text = openFileDialog1.FileName;
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (File.Exists(textBox1.Text))
            {
                try
                {
                    pictureBox3.Image = Image.FromFile(textBox1.Text);

                }
                catch { pictureBox3.Image = null; }
            }
            else
            {
                pictureBox3.Image = null;
            }
        }

        private void Frmmain_Load(object sender, EventArgs e)
        {
            textBox1.Text = Properties.Settings.Default.MWFile;
            trackBar1.Value = Properties.Settings.Default.Trans;
            trackBar2.Value = Properties.Settings.Default.Size;
            trackBar3.Value = Properties.Settings.Default.Rot;
            trackBar1_Scroll(null, null);
            trackBar2_Scroll(null, null);
            trackBar3_Scroll(null, null);
            
            //Font.FontFamily.
            foreach (FontFamily ff in FontFamily.Families)
            {
                if (ff.IsStyleAvailable(FontStyle.Regular))
                    comboBox1.Items.Add(ff.Name);
                if (ff.IsStyleAvailable(FontStyle.Regular))
                    comboBox2.Items.Add(ff.Name);

            } 
            comboBox1.SelectedIndex = comboBox1.Items.IndexOf("Arial");
            comboBox2.SelectedIndex = comboBox2.Items.IndexOf("Arial");

        }




        private void Frmmain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.MWFile = textBox1.Text;
            Properties.Settings.Default.Trans = trackBar1.Value;
            Properties.Settings.Default.Size = trackBar2.Value;
            Properties.Settings.Default.Rot = trackBar3.Value;

            Properties.Settings.Default.Save();

        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog(this) != System.Windows.Forms.DialogResult.Cancel)
            {
               try
                {
                    pictureBox1.Image = Image.FromFile(openFileDialog2.FileName);
                }
                catch { pictureBox1.Image = null; }

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (File.Exists(textBox1.Text))
            {

                if (File.Exists(openFileDialog2.FileName))
                {
                    try {
                   
                    Image image = System.Drawing.Image.FromFile(openFileDialog2.FileName);//This is the background image
                    Image logo = System.Drawing.Image.FromFile(textBox1.Text); //This is your watermark

                    double moniwal=1;

                    if ((logo.Width - image.Width) > (logo.Height - image.Height)) if (logo.Width > image.Width) moniwal = (double)logo.Width / (double)image.Width;
                    if ((logo.Width - image.Width) < (logo.Height - image.Height)) if (logo.Height > image.Height) moniwal = (double)logo.Height / (double)image.Height;



                    logo = new Bitmap(logo, (int)((double)logo.Width / moniwal * ((double)trackBar2.Value / 10)), (int)((double)logo.Height / moniwal * ((double)trackBar2.Value / 10)));
                    
                    Graphics g = System.Drawing.Graphics.FromImage(image); //Create graphics object of the background image //So that you can draw your logo on it
                    Bitmap TransparentLogo = new Bitmap(logo.Width, logo.Height); //Create a blank bitmap object //to which we //draw our transparent logo
                    Graphics TGraphics = Graphics.FromImage(TransparentLogo);//Create a graphics object so that //we can draw //on the blank bitmap image object
                    ColorMatrix ColorMatrix = new ColorMatrix(); //An image is represented as a 5X4 matrix(i.e 4 //columns and 5 //rows)
                    ColorMatrix.Matrix33 = (Single)((double)trackBar1.Value / (double)100);//the 3rd element of the 4th row represents the transparency
                    ImageAttributes ImgAttributes = new ImageAttributes();//an ImageAttributes object is used to set all //the alpha //values.This is done by initializing a color matrix and setting the alpha scaling value in the matrix.The address of //the color matrix is passed to the SetColorMatrix method of the //ImageAttributes object, and the //ImageAttributes object is passed to the DrawImage method of the Graphics object.
                    ImgAttributes.SetColorMatrix(ColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap); TGraphics.DrawImage(logo, new Rectangle(0, 0, TransparentLogo.Width, TransparentLogo.Height), 0, 0, TransparentLogo.Width, TransparentLogo.Height, GraphicsUnit.Pixel, ImgAttributes);
                    TGraphics.Dispose();

                    g.TranslateTransform(image.Width / 2, image.Height/2);
                    g.RotateTransform(trackBar3.Value);
                    g.TranslateTransform(-image.Width / 2, -image.Height / 2);

                        

                    g.DrawImage(TransparentLogo, (image.Width - logo.Width) / 2, (image.Height - logo.Height) / 2);
                    pictureBox2.Image = image;
                    }

                     catch { }
                }
            }

            if (numericUpDown1.Value > 0)
            {
                Pen myPen = new Pen(Color.Gray, 2);
                Bitmap bmp = new Bitmap(pictureBox9.Size.Width, pictureBox9.Size.Height);
                Graphics gz = Graphics.FromImage(bmp);
                gz.Clear(Color.Transparent);

                //gz.DrawLine(myPen, 0, 0, 100, 100);
                //gz.DrawString(textBox2.Text, 

                StringFormat sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;


                Font m = new Font(comboBox1.Text, (float)numericUpDown1.Value);
                Font m2 = new Font(comboBox2.Text, (float)numericUpDown1.Value / 2);
                gz.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                gz.DrawString(textBox2.Text, m, Brushes.Black, pictureBox9.ClientRectangle, sf);

                gz.DrawString(textBox3.Text, m2, Brushes.Black,
                    new Rectangle(pictureBox9.ClientRectangle.X,
                                  pictureBox9.ClientRectangle.Y + m.Height / 2 + m2.Height / 4,
                                  pictureBox9.ClientRectangle.Width,
                                  pictureBox9.ClientRectangle.Height)
                    , sf);
                pictureBox9.Image = bmp;
            }

        }

        private void menuItem4_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label3.Text = trackBar2.Value.ToString();

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label6.Text = trackBar1.Value.ToString();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            label4.Text = trackBar3.Value.ToString();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void vistaButton2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog(this) != System.Windows.Forms.DialogResult.Cancel)
            {
                try
                {
                    pictureBox9.Image.Save(saveFileDialog1.FileName);
                }
                catch { }

            }
        }

        private void menuItem5_Click(object sender, EventArgs e)
        {

            if (saveFileDialog2.FileName != "")
            {
                try
                {           
        
                    pictureBox2.Image.Save(saveFileDialog2.FileName);
                }
                catch { }
            }
            else
            {
                menuItem6_Click(null, null);
            }
            
        }

        private void menuItem6_Click(object sender, EventArgs e)
        {
            string oldfilepath = saveFileDialog2.FileName;

            if (saveFileDialog2.ShowDialog(this) != System.Windows.Forms.DialogResult.Cancel)
            {
                try               
                {
                    if (!saveFileDialog2.FileName.EndsWith(".bmp") & !saveFileDialog2.FileName.EndsWith(".jpg") & !saveFileDialog2.FileName.EndsWith(".gif") & !saveFileDialog2.FileName.EndsWith(".png"))
                    {
                        saveFileDialog2.FileName =  saveFileDialog2.FileName + (saveFileDialog2.Filter.Split('|')[(saveFileDialog2.FilterIndex-1) * 2 + 1]).Substring(1);
                    }
                    pictureBox2.Image.Save(saveFileDialog2.FileName);
                }
                catch { }
            }
            else{
                saveFileDialog2.FileName = oldfilepath;
            }

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }
    }
}
