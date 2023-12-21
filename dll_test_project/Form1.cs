using Library_Digital_RSA;
using System.Numerics;
using System.Text;

namespace dll_test_project
{
    public partial class Form1 : Form
    {
        bool q1 = false; //����� ��� �������� �������� �����
        bool q2 = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Check_changes() //�������� �� ��, �������� �� ����� �������� ��� �������� ����������� ������� �� ������
        {
            function_check_prostota(textBox1, textBox3);
            function_check_prostota(textBox2, textBox4);

            if (q1 == false || q2 == false)
            {
                button1.Enabled = false;
            }
            else if (q1 == true || q2 == true)
            {
                button1.Enabled = true;
            }
            if (textBox3.Text == "���" || textBox4.Text == "���")
            {
                button1.Enabled = false;
            }

        }
        public void function_check_prostota(TextBox txt, TextBox chek) //������� �������� �� ��������
        {
            RSA_Class rs = new RSA_Class(); //�������� ���������� ������ RSA
            if (rs.Test_Miller(BigInteger.Parse(txt.Text), 10))//���� ������ ��� ����������� ��������
            {
                chek.Text = "��";
            }
            else
                chek.Text = "���";
        }
        private void button2_Click(object sender, EventArgs e) //�������� �� �������� ��� ������� �� ������
        {
            Check_changes();
            function_check_prostota(textBox1, textBox3);
            function_check_prostota(textBox2, textBox4);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Check_changes();
            RSA_Class rs = new RSA_Class(); //�������� ���������� ������ ��� ������ � RSA ����������
            if (textBox1.Text == textBox2.Text)
            {
                MessageBox.Show("� ��������� RSA ����� �� ����� �� ������ ���� �����!!!");
            }
            else
            {

                if (textBox3.Text == "��" && textBox4.Text == "��" && q1 == true && q2 == true)
                {
                    button1.Enabled = true; 
                    int n = int.Parse(textBox1.Text) * int.Parse(textBox2.Text); //��������� �������� ��� ��������� RSA
                    int FiEil = (int.Parse(textBox1.Text) - 1) * (int.Parse(textBox2.Text) - 1);
                    int ex_ = rs.e_find(n, FiEil); //��������� ����� �� 1 �� FiEil (������������� � FiEil)
                    int d = 0;
                    d = rs.ModInverse(ex_, FiEil); //��������� �����
                    textBox9.Text = d.ToString();
                    textBox10.Text = ex_.ToString();
                    string originalMessage = textBox5.Text; //��������� ������ - ���������

                    byte[] bytes = Encoding.UTF8.GetBytes(originalMessage); //��������� ���������� ���������� RSA


                    long[] encryptedBlocks = new long[bytes.Length];

                    for (int i = 0; i < bytes.Length; i++)
                    {
                        encryptedBlocks[i] = (long)rs.Encrypt(bytes[i], ex_, n);
                    }
                    byte[] bytes_1 = new byte[encryptedBlocks.Length];
                    for (int i = 0; i < encryptedBlocks.Length; i++)
                    {
                        bytes_1[i] = (byte)encryptedBlocks[i];
                    }
                    string result1 = Encoding.UTF8.GetString(bytes_1);
                    textBox6.Text = result1;                                //���������� ���������� RSA (�����)

                    long[] decryptedBlocks = new long[bytes.Length];        //��������� ������������ ���������� RSA


                    for (int i = 0; i < encryptedBlocks.Length; i++)
                    {
                        decryptedBlocks[i] = (long)(rs.Decrypt(encryptedBlocks[i], d, n));
                    }
                    byte[] bytes_2 = new byte[decryptedBlocks.Length];
                    for (int i = 0; i < encryptedBlocks.Length; i++)
                    {
                        bytes_2[i] = (byte)decryptedBlocks[i];
                    }

                    string decryptedMessage = Encoding.UTF8.GetString(bytes_2);

                    textBox7.Text = (decryptedMessage);                      //������������ ���������� RSA (�����)
                }
            }
            q1 = false;
            q2 = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)  //�������� �� ��������� ������
        {
            textBox3.Text = "";     
            q1 = true;
        }

        private void textBox2_TextChanged(object sender, EventArgs e) //�������� �� ��������� ������
        {
            textBox4.Text = "";
            q2 = true;
        }

        private void button3_Click_1(object sender, EventArgs e) //��������� ���� � �������� �������
        {
            RSA_Class rs_ = new RSA_Class();
            Digital_ PodPS = new Digital_();

            string originalMessage = textBox5.Text;
            BigInteger hash_val = PodPS.GetHash(originalMessage);
            int n = int.Parse(textBox1.Text) * int.Parse(textBox2.Text);
            long d = int.Parse(textBox9.Text); // �������� ����

            BigInteger encryptedHash = rs_.Encrypt(hash_val, d, n); //���������� �������� ������ ����
            textBox8.Text = hash_val.ToString();
            textBox12.Text = encryptedHash.ToString();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            textBox11.Text = "<" + textBox8.Text + "," + textBox12.Text + ">"; //��������� �������� ������� � ���� � ����������
        }

        private void button5_Click(object sender, EventArgs e)  //������� ���������� 11 � ���������� ������
        {
            string input = textBox11.Text;
            int startIndex = input.IndexOf('<');
            int endIndex = input.IndexOf(',', startIndex);
            string extractedWord;
            startIndex++;
            extractedWord = input.Substring(startIndex, endIndex - startIndex);
            int n = int.Parse(textBox1.Text) * int.Parse(textBox2.Text);
            long d = int.Parse(textBox9.Text);
            long e_ = int.Parse(textBox10.Text);

            RSA_Class rs_ = new RSA_Class();
            Digital_ PodPS = new Digital_();

            BigInteger decryptedBlock = rs_.Decrypt(BigInteger.Parse(extractedWord), e_, n);    // ����������� �������� ������� � �������������� ��������� �����

            string decryptedMessage = decryptedBlock.ToString();            // �������������� ����� � ������
            textBox13.Text = extractedWord.ToString();
            // ��������� ����
            BigInteger hash_ = PodPS.GetHash(decryptedMessage);
            textBox15.Text = decryptedMessage.ToString();

        }

        private void button6_Click(object sender, EventArgs e)  //��������� ������������ � ��������� � �����
        {
            string input = textBox11.Text;
            int startIndex = input.IndexOf('<');
            int endIndex = input.IndexOf(',', startIndex);
            string extractedWord;
            startIndex++;
            extractedWord = input.Substring(startIndex, endIndex - startIndex);
            int n = int.Parse(textBox1.Text) * int.Parse(textBox2.Text);
            long e_ = int.Parse(textBox10.Text); // �������� ����

            RSA_Class rs_ = new RSA_Class();
            Digital_ PodPS = new Digital_();

            BigInteger decryptedHash = rs_.Decrypt(BigInteger.Parse(extractedWord), e_, n);  // ����������� �������� ������� � �������������� ��������� �����

            // �������������� ����� � ������
            string decryptedMessage = decryptedHash.ToString();

            BigInteger hash_ = PodPS.GetHash(decryptedMessage);            // ��������� ����
            textBox16.Text = decryptedMessage;

            if (textBox15.Text == textBox16.Text) //�������� �� ���������� �����
            {
                textBox15.BackColor = Color.Green;
                textBox16.BackColor = Color.Green;
            }
            else
            {
                textBox15.BackColor = Color.Red;
                textBox16.BackColor = Color.Red;
            }

        }


    }
}
