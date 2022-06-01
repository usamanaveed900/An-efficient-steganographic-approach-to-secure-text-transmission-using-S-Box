using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Net;
using System.Data.Linq;
using System.Drawing.Imaging;
using System.Diagnostics;


namespace Project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string plainText = null;
        string loadFileName = null;
        string imageBinary = null;
        string LSB = null;
        static string messageAfterLSB = null;

        public static void writeMessage(string data, string name)
        {
            string path = @"";
            string Filename = name + "MessageEncrypted.txt";
            var newpath = path + Filename;
            using (StreamWriter sw = System.IO.File.AppendText(newpath))
            {
                sw.WriteLine(data);
            }

        }

        public static void writeMessageDecrypted(string data, string name)
        {
            string path = @"";
            string Filename = name + "MessageDecrypted.txt";
            var newpath = path + Filename;
            using (StreamWriter sw = System.IO.File.AppendText(newpath))
            {
                sw.WriteLine(data);
            }

        }

        public static string convertToASCII(string unicodeString)
        {
            string ASCIIcode = null;
            var utf8bytes = Encoding.UTF8.GetBytes(unicodeString);
            var win1252Bytes = Encoding.Convert(Encoding.UTF8, Encoding.ASCII, utf8bytes);
            foreach (var item in win1252Bytes)
            {
                ASCIIcode += item + " ";
            }
            return ASCIIcode;
        }

        public static string ASCIIToBinary(string str)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in str.ToCharArray())
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }
            return sb.ToString();
        }

        public static Byte[] GetBytesFromBinaryString(String binary)
        {
            var list = new List<Byte>();

            for (int i = 0; i < binary.Length; i += 8)
            {
                String t = binary.Substring(i, 8);

                list.Add(Convert.ToByte(t, 2));
            }

            return list.ToArray();
        }

        public static string S_Box(string Decimal)
        {
            int j = 0;
            string temp = null;
            int size = Decimal.Length / 2;
            string[] value=new string[size];
            int[,] Box = new int[4, 4] { { 1, 0, 13, 8 }, { 9, 2, 7, 12 }, {11, 6, 3, 10 }, { 5, 14, 15, 4 } };
            string binary = null;
            for (int i = 0; i < Decimal.Length;i+=2 )
            {
                
                temp = Decimal.Substring(i, 2);
                value[j]=Box[int.Parse(temp[1].ToString()), int.Parse(temp[0].ToString())].ToString();
                j++;
            }
            for (int k = 0; k < size; k++)
            {
                if (value[k]=="0")
                {
                    binary += "0000";
                }
                else if(value[k]=="1")
                {
                    binary += "0001";
                }
                else if (value[k] == "2")
                {
                    binary += "0010";
                }
                else if (value[k] == "3")
                {
                    binary += "0011";
                }
                else if (value[k] == "4")
                {
                    binary += "0100";
                }
                else if (value[k] == "5")
                {
                    binary += "0101";
                }
                else if (value[k] == "6")
                {
                    binary += "0110";
                }
                else if (value[k] == "7")
                {
                    binary += "0111";
                }
                else if (value[k] == "8")
                {
                    binary += "1000";
                }
                else if (value[k] == "9")
                {
                    binary += "1001";
                }
                else if (value[k] == "10")
                {
                    binary += "1010";
                }
                else if (value[k] == "11")
                {
                    binary += "1011";
                }
                else if (value[k] == "12")
                {
                    binary += "1100";
                }
                else if (value[k] == "13")
                {
                    binary += "1101";
                }
                else if (value[k] == "14")
                {
                    binary += "1110";
                }
                else if (value[k] == "15")
                {
                    binary += "1111";
                }
                else
                {
                    MessageBox.Show("INVALID");
                    continue;
                }

            }
            return binary;
        }

        public static string performXOR(string value, string key)
        {
            string result = null;
            if (value.Length==key.Length)
            {
                for ( int i = 0; i < value.Length; i++)
                {
                    if ( value[i]=='0'&&key[i]=='0')
                    {
                        result += '0';
                    }
                    else if(value[i]=='1'&&key[i]=='1')
                    {
                        result += '0';
                    }
                    else
                    {
                        result += '1';
                    }
                }
            }

            return result;
        }

        public static string ImageToBinary(string imagePath)
        {
            FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, (int)fileStream.Length);
            fileStream.Close();
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                builder.Append(buffer[i].ToString("x2"));
            }  

            return builder.ToString();
        }

        public static string performLSB(string XOR,string ImageBianry)
        {
            string result=null;
            string temp = XOR.Substring(0, 3);
            int index=ImageBianry.Length-3;
            string temp2 = ImageBianry.Substring(index, 3);
            result=ImageBianry.Replace(temp2, temp);
            messageAfterLSB = XOR.Replace(temp, temp2);
            return result;
        }

        public static string reverseS_BOX(string binary)
        {
            //save value to array or s_box values will be wrong
            int size = binary.Length / 4;
            string[] value = new string[size];
            int i = 0;
            for (int k = 0; k < binary.Length; k += 4)
            {
                string temp = binary.Substring(k, 4);
                if (temp == "0000")
                {
                    value[i] += "0";
                }
                else if (temp == "0001")
                {
                    value[i] += "1";
                }
                else if (temp == "0010")
                {
                    value[i] += "2";
                }
                else if (temp == "0011")
                {
                    value[i] += "3";
                }
                else if (temp == "0100")
                {
                    value[i] += "4";
                }
                else if (temp == "0101")
                {
                    value[i] += "5";
                }
                else if (temp == "0110")
                {
                    value[i] += "6";
                }
                else if (temp == "0111")
                {
                    value[i] += "7";
                }
                else if (temp == "1000")
                {
                    value[i] += "8";
                }
                else if (temp == "1001")
                {
                    value[i] += "9";
                }
                else if (temp == "1010")
                {
                    value[i] += "10";
                }
                else if (temp == "1011")
                {
                    value[i] += "11";
                }
                else if (temp == "1100")
                {
                    value[i] += "12";
                }
                else if (temp == "1101")
                {
                    value[i] += "13";
                }
                else if (temp == "1110")
                {
                    value[i] += "14";
                }
                else if (temp == "1111")
                {
                    value[i] += "15";
                }
                else
                {
                    MessageBox.Show("INVALID");
                    continue;
                }
                i++;

            }
            //MessageBox.Show(value[0] + value[1]);
            string reverseS_boxValue = null;
            int[,] Box = new int[4, 4] { { 1, 0, 13, 8 }, { 9, 2, 7, 12 }, { 11, 6, 3, 10 }, { 5, 14, 15, 4 } };
            for (int m=0 ; m< size;m++)
            {
                for (int l=0;l<4;l++)
                {
                    for(int p=0;p<4;p++)
                    {
                        if(Box[l,p].ToString()==value[m])
                        {
                            reverseS_boxValue += p.ToString() + l.ToString();
                        }
                    }
                }
            }
            return reverseS_boxValue;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stream browse = null;
            OpenFileDialog readfile = new OpenFileDialog();
            readfile.InitialDirectory = "c:\\";
            readfile.Filter = "txt file (*.txt)|*.txt";
            if (readfile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileName = readfile.FileName;
                    if ((browse = readfile.OpenFile()) != null)
                    {
                        using (browse)
                        {
                            plainText = System.IO.File.ReadAllText(readfile.FileName);
                        }
                    }
                    textBox1.Text = fileName;
                    loadFileName = readfile.SafeFileName;
                    int index = loadFileName.IndexOf(".");
                    StringBuilder sr = new StringBuilder(loadFileName);
                    sr.Remove(index, 4);
                    loadFileName = sr.ToString();
                    //textBox2.Text=plainText;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stopwatch EncryptionTime = new Stopwatch();
            EncryptionTime.Start();
            string key = "01101100";
            string S_BoxValue = null;
            string XORvalue = null;
            string ASCII = convertToASCII(plainText);
            string binary = ASCIIToBinary(plainText);
            StringBuilder value = new StringBuilder();
            for (int i=0; i<binary.Length;i+=2)
            {
                string temp = binary.Substring(i, 2);
                if(temp=="01")
                {
                    value.Append("1");
                }
                else if(temp=="00")
                {
                    value.Append("0");
                }
                else if(temp=="10")
                {
                    value.Append("2");
                }
                else
                {
                    MessageBox.Show("INVALID INPUT");
                    continue;
                }

            }
            S_BoxValue = S_Box(value.ToString());
            int Sizekey = S_BoxValue.Length / 8;
            string newKey=null;
            for (int i = 0; i < Sizekey; i++ )
            {
                newKey += key;
            }
            XORvalue = performXOR(S_BoxValue, newKey);
            string imageFilePath = null;
            OpenFileDialog readfile = new OpenFileDialog();
            readfile.InitialDirectory = "c:\\";
            readfile.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";
            if (readfile.ShowDialog() == DialogResult.OK)
            {
                imageFilePath = readfile.FileName;
            }
            string imageValue = ImageToBinary(imageFilePath);
            imageBinary = ASCIIToBinary(imageValue);
            LSB = performLSB(XORvalue, imageBinary);
            writeMessage(messageAfterLSB, loadFileName);
            var bytesImg = GetBytesFromBinaryString(LSB);
            string filePath = loadFileName + "Encrypted.png";
            File.WriteAllBytes(filePath, bytesImg);
            EncryptionTime.Stop();
            label1.Text = "Encrytion Time : " + EncryptionTime.Elapsed;

        }

        public static Image BinaryToImage(System.Data.Linq.Binary binaryData)
        {
            if (binaryData == null) return null;

            byte[] buffer = binaryData.ToArray();
            MemoryStream memStream = new MemoryStream();
            memStream.Write(buffer, 0, buffer.Length);
            return Image.FromStream(memStream);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Stopwatch DecryptionTime = new Stopwatch();
            DecryptionTime.Start();
            string key = "01101100";
            string path = @"";
            string temp = textBox3.Text;
            var newpath = path + textBox3.Text;
            string messageBinary = System.IO.File.ReadAllText(newpath);
            string imageFilePath=temp.Replace("MessageEncrypted.txt","Encrypted.png");
            string imageValue = ImageToBinary(imageFilePath);
            string decryptImageBinary = ASCIIToBinary(imageValue);
            string imageBianryLSB = performLSB(messageBinary, decryptImageBinary);
            var bytesImg = GetBytesFromBinaryString(imageBianryLSB);
            string filePath = loadFileName + "Decrypted.png";
            File.WriteAllBytes(filePath, bytesImg);
            string messageBinaryLSB = messageAfterLSB;
            string messageBinaryBeforeXOR = messageBinaryLSB.Remove(messageBinaryLSB.Length - 2, 2);
            int Sizekey = messageBinaryBeforeXOR.Length / 8;
            string newKey = null;
            for (int i = 0; i < Sizekey; i++)
            {
                newKey += key;
            }
            string XORvalue = performXOR(messageBinaryBeforeXOR, newKey);
            string temp3=reverseS_BOX(XORvalue);
            StringBuilder value1 = new StringBuilder();
            for (int i = 0; i < temp3.Length; i++)
            {
                string check = temp3.Substring(i, 1);
                if (check == "1")
                {
                    value1.Append("01");
                }
                else if (check == "0")
                {
                    value1.Append("00");
                }
                else if (check == "2")
                {
                    value1.Append("10");
                }
                else if (check == "3")
                {
                    value1.Append("11");
                }
                else
                {
                    MessageBox.Show("INVALID INPUT");
                    continue;
                }
            }
            var data = GetBytesFromBinaryString(value1.ToString());
            var text = Encoding.ASCII.GetString(data);
            writeMessageDecrypted(text, loadFileName);
            DecryptionTime.Stop();
            label2.Text = "Decryption Time : " + DecryptionTime.Elapsed;
        }
    }
}
