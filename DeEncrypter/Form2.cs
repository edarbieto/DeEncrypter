using System;
using System.IO;
using System.Windows.Forms;

namespace DeEncrypter
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
            checkBox2.Checked = true;
            label1.Enabled = false;
            textBox1.Enabled = false;
            button1.Enabled = false;
            label3.Enabled = false;
            textBox3.Enabled = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            label1.Enabled = !label1.Enabled;
            textBox1.Enabled = !textBox1.Enabled;
            button1.Enabled = !button1.Enabled;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            label3.Enabled = !label3.Enabled;
            textBox3.Enabled = !textBox3.Enabled;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "PublicKey[" + DateTime.Now.ToString().Replace(":", "").Replace("/", "") + "]";
            openFileDialog1.Filter = "DeEncrypter Key|*.dek";
            openFileDialog1.CheckFileExists = false;
            openFileDialog1.Multiselect = false;
            openFileDialog1.SupportMultiDottedExtensions = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK) textBox4.Text = openFileDialog1.FileName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog2.FileName = "PrivateKey[" + DateTime.Now.ToString().Replace(":", "").Replace("/", "") + "]";
            openFileDialog2.Filter = "DeEncrypter Key|*.dek";
            openFileDialog2.CheckFileExists = false;
            openFileDialog2.Multiselect = false;
            openFileDialog2.SupportMultiDottedExtensions = false;
            if (openFileDialog2.ShowDialog() == DialogResult.OK) textBox1.Text = openFileDialog2.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "" && textBox4.Text != "" && (checkBox2.Checked || textBox3.Text != "") && (checkBox1.Checked || textBox1.Text != ""))
            {
                int[] llave = RSA.GenerarLLave();
                string llavePublica = Convert.ToString("");
                llavePublica += llave[0].ToString() + "\r\n" + llave[2].ToString();
                string llavePrivada = Convert.ToString("");
                llavePrivada += llave[1].ToString() + "\r\n" + llave[2].ToString();
                string pathPublica = openFileDialog1.FileName;
                string pathPrivada = checkBox1.Checked ? openFileDialog1.FileName.Replace(".dek", "(private).dek") : openFileDialog2.FileName;
                if (File.Exists(pathPublica))
                {
                    File.Delete(pathPublica);
                }
                if (File.Exists(pathPrivada))
                {
                    File.Delete(pathPrivada);
                }
                using (StreamWriter f1 = new StreamWriter(File.Create(pathPublica)))
                {
                    using (StreamWriter f2 = new StreamWriter(File.Create(pathPrivada)))
                    {
                        f1.Write(Crypto.EncryptStringAES(llavePublica, textBox2.Text));
                        f2.Write(Crypto.EncryptStringAES(llavePrivada, checkBox2.Checked ? textBox2.Text : textBox3.Text));
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
