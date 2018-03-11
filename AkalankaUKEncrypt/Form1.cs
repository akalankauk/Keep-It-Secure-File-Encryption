using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace AkalankaUKEncrypt
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            ofd.InitialDirectory = "C:";
        }

        private string GetFileName(string path)
        {
            string name = path;
            int poz = path.LastIndexOf('.');
            if (poz > 0) name = path.Substring(0, poz);
            return name;
        }

        private string GetFileExt(string path)
        {
            string ext = "";
            int poz = path.LastIndexOf('.');
            if (poz > 0) ext = path.Substring(poz + 1);
            return ext;
        }

        private byte ByteEncrypt(byte b)
        {
            return (byte)(b ^ 128);
        }

        private byte[] StrToByteArray(string st, Encoding enc)
        {
            return enc.GetBytes(st);
        }

        private string ByteArrayToStr(byte[] bstr, Encoding enc)
        {
            return enc.GetString(bstr);
        }

        public bool EncryptFile(string inputFile)
        {
            string name = GetFileName(inputFile);
            string ext = GetFileExt(inputFile);
            byte[] bext = StrToByteArray(ext, new UnicodeEncoding());
            int k = bext.Length;
            try
            {
                FileStream fsRead = new FileStream(inputFile, FileMode.Open);
                FileStream fsWrite = new FileStream(name + ".ae", FileMode.Create);
                fsWrite.Write(BitConverter.GetBytes(k), 0, 4);
                fsWrite.Write(bext, 0, k);
                int data;
                while ((data = fsRead.ReadByte()) != -1) fsWrite.WriteByte(ByteEncrypt((byte)data));
                fsRead.Close();
                fsWrite.Close();
                File.Delete(inputFile);
                return true;
            }
            catch { }
            return false;
        }

        public bool DecryptFile(string inputFile)
        {
            try
            {
                FileStream fsRead = new FileStream(inputFile, FileMode.Open);
                string name = GetFileName(inputFile);
                byte[] bint32 = new byte[4];
                int i = fsRead.Read(bint32, 0, 4);
                int k = BitConverter.ToInt32(bint32, 0);
                byte[] bext = new byte[k];
                i = fsRead.Read(bext, 0, k);
                string ext = "." + ByteArrayToStr(bext, new UnicodeEncoding());
                FileStream fsWrite = new FileStream(name + ext, FileMode.Create);
                int data;
                while ((data = fsRead.ReadByte()) != -1) fsWrite.WriteByte(ByteEncrypt((byte)data));
                fsRead.Close();
                fsWrite.Close();
                return true;
            }
            catch { }
            return false;
        }

        private void bEncrypt_Click(object sender, EventArgs e)
        {
            txStatus.Text = "";
            ofd.Filter = "All Files|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string s in ofd.FileNames)
                {
                    if (EncryptFile(s)) txStatus.AppendText(s + " has been successfully encrypted" + Environment.NewLine);
                    else txStatus.AppendText(s + " could not be encrypted" + Environment.NewLine);
                }
            }
        }

        private void bDecrypt_Click(object sender, EventArgs e)
        {
            txStatus.Text = "";
            ofd.Filter = "Encrypted Files|*.ae";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string s in ofd.FileNames)
                {
                    if (DecryptFile(s)) txStatus.AppendText(s + " has been successfully decrypted" + Environment.NewLine);
                    else txStatus.AppendText(s + " could not be decrypted" + Environment.NewLine);
                }
            }
        }

        private void txStatus_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Click '🔒 Encrypt My Data'To Encrypt Your Files");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/akalankauk/Keep-It-Secure-File-Encryption");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(1);
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Open Source Project : https://github.com/akalankauk | File Type: .ae",
    "About");
        }

        private void label1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("When you encrypt your file,original fill will be removed automatically.If you didn't want to remove original file erase 'File.Delete(inputFile);' from Form1.cs (Line Num 64)",
               "Info");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("When you encrypt your file,original fill will be removed automatically.If you didn't want to remove original file erase 'File.Delete(inputFile);' from Form1.cs (Line Num 64)",
   "Info");
        }
    }
}

