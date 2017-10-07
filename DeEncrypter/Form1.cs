using System;
using System.IO;
using System.Windows.Forms;

namespace DeEncrypter
{
    public partial class Form1 : Form
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = false;
            openFileDialog1.FileName = "";
            openFileDialog1.ShowDialog();
            textBox1.Text = openFileDialog1.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "DeEncrypter Keys|*.dek";
            openFileDialog2.Multiselect = false;
            openFileDialog2.FileName = "";
            openFileDialog2.ShowDialog();
            textBox2.Text = openFileDialog2.FileName;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowNewFolderButton = true;
            folderBrowserDialog1.Description = "Seleccione la carpeta donde desea guardar el archivo encriptado/desencriptado:\r\nNota: Luego asignará el nombre del archivo";
            folderBrowserDialog1.ShowDialog();
            textBox4.Text = folderBrowserDialog1.SelectedPath;
        }

        private void nuevasLlavesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 obj = new Form2();
            obj.StartPosition = FormStartPosition.Manual;
            obj.Location = MousePosition;
            obj.ShowInTaskbar = false;
            obj.ShowDialog(this);
            obj.Dispose();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "")
            {
                string llave;
                int E, N;
                using (StreamReader f = new StreamReader(textBox2.Text))
                {
                    try
                    {
                        llave = Crypto.DecryptStringAES(f.ReadToEnd(), textBox3.Text);
                        string[] info = llave.Split("\r\n".ToCharArray());
                        E = Convert.ToInt32(info[0]);
                        N = Convert.ToInt32(info[2]);
                        RSA.EncriptarArchivo(E, N, textBox1.Text, Path.Combine(textBox4.Text, textBox5.Text));
                    }
                    catch
                    {
                        MessageBox.Show("¡Ocurrio un error!\r\nPuede ser que no se escogio la llave correcta o la clave AES no concuerda");
                    }
                }
            }
            else
            {
                MessageBox.Show("Faltan datos");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "")
            {
                string llave;
                int D, N;
                using (StreamReader f = new StreamReader(textBox2.Text))
                {
                    try
                    {
                        llave = Crypto.DecryptStringAES(f.ReadToEnd(), textBox3.Text);
                        string[] info = llave.Split("\r\n".ToCharArray());
                        D = Convert.ToInt32(info[0]);
                        N = Convert.ToInt32(info[2]);
                        RSA.DesencriptarArchivo(D, N, textBox1.Text, Path.Combine(textBox4.Text, textBox5.Text));
                    }
                    catch
                    {
                        MessageBox.Show("¡Ocurrio un error!\r\nPuede ser que no se escogio la llave correcta o la clave AES no concuerda");
                    }
                }
            }
            else
            {
                MessageBox.Show("Faltan datos");
            }
        }
    }
}
