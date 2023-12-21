using Library_Digital_RSA;
using System.Numerics;
using System.Text;

namespace dll_test_project
{
    public partial class Form1 : Form
    {
        bool q1 = false; //Флаги для проверки простоты чисел
        bool q2 = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Check_changes() //проверка на то, являются ли числа простыми для контроля возможности нажатия на кнопку
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
            if (textBox3.Text == "Нет" || textBox4.Text == "Нет")
            {
                button1.Enabled = false;
            }

        }
        public void function_check_prostota(TextBox txt, TextBox chek) //функция проверка на простоту
        {
            RSA_Class rs = new RSA_Class(); //создание экземпляра класса RSA
            if (rs.Test_Miller(BigInteger.Parse(txt.Text), 10))//тест Милера для определения простоты
            {
                chek.Text = "Да";
            }
            else
                chek.Text = "Нет";
        }
        private void button2_Click(object sender, EventArgs e) //Проверка на простоту при нажатии на кнопку
        {
            Check_changes();
            function_check_prostota(textBox1, textBox3);
            function_check_prostota(textBox2, textBox4);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Check_changes();
            RSA_Class rs = new RSA_Class(); //создание экземпляра класса для работы с RSA алгоритмом
            if (textBox1.Text == textBox2.Text)
            {
                MessageBox.Show("В алгоритме RSA числа на входе не должны быть равны!!!");
            }
            else
            {

                if (textBox3.Text == "Да" && textBox4.Text == "Да" && q1 == true && q2 == true)
                {
                    button1.Enabled = true; 
                    int n = int.Parse(textBox1.Text) * int.Parse(textBox2.Text); //Высиления значений для алгоритма RSA
                    int FiEil = (int.Parse(textBox1.Text) - 1) * (int.Parse(textBox2.Text) - 1);
                    int ex_ = rs.e_find(n, FiEil); //случайное число от 1 до FiEil (взаимопростое с FiEil)
                    int d = 0;
                    d = rs.ModInverse(ex_, FiEil); //получение ключа
                    textBox9.Text = d.ToString();
                    textBox10.Text = ex_.ToString();
                    string originalMessage = textBox5.Text; //получение строки - сообщения

                    byte[] bytes = Encoding.UTF8.GetBytes(originalMessage); //Процедура шифрования алгоритмом RSA


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
                    textBox6.Text = result1;                                //Шифрование алгоритмом RSA (конец)

                    long[] decryptedBlocks = new long[bytes.Length];        //Процедура дешифрования алгоритмом RSA


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

                    textBox7.Text = (decryptedMessage);                      //Дешифрование алгоритмом RSA (конец)
                }
            }
            q1 = false;
            q2 = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)  //Проверка на изменение текста
        {
            textBox3.Text = "";     
            q1 = true;
        }

        private void textBox2_TextChanged(object sender, EventArgs e) //Проверка на изменение текста
        {
            textBox4.Text = "";
            q2 = true;
        }

        private void button3_Click_1(object sender, EventArgs e) //Получение хеша и цифровой подписи
        {
            RSA_Class rs_ = new RSA_Class();
            Digital_ PodPS = new Digital_();

            string originalMessage = textBox5.Text;
            BigInteger hash_val = PodPS.GetHash(originalMessage);
            int n = int.Parse(textBox1.Text) * int.Parse(textBox2.Text);
            long d = int.Parse(textBox9.Text); // Закрытый ключ

            BigInteger encryptedHash = rs_.Encrypt(hash_val, d, n); //Зашифровка закрытым ключом хэша
            textBox8.Text = hash_val.ToString();
            textBox12.Text = encryptedHash.ToString();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            textBox11.Text = "<" + textBox8.Text + "," + textBox12.Text + ">"; //получение цифровой подписи и хэша в текстбоксе
        }

        private void button5_Click(object sender, EventArgs e)  //парсинг текстбокса 11 и извлечение данных
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

            BigInteger decryptedBlock = rs_.Decrypt(BigInteger.Parse(extractedWord), e_, n);    // Расшифровка цифровой подписи с использованием открытого ключа

            string decryptedMessage = decryptedBlock.ToString();            // Преобразование числа в строку
            textBox13.Text = extractedWord.ToString();
            // Получение хэша
            BigInteger hash_ = PodPS.GetHash(decryptedMessage);
            textBox15.Text = decryptedMessage.ToString();

        }

        private void button6_Click(object sender, EventArgs e)  //процедура дешифрования и сравнения с хэшем
        {
            string input = textBox11.Text;
            int startIndex = input.IndexOf('<');
            int endIndex = input.IndexOf(',', startIndex);
            string extractedWord;
            startIndex++;
            extractedWord = input.Substring(startIndex, endIndex - startIndex);
            int n = int.Parse(textBox1.Text) * int.Parse(textBox2.Text);
            long e_ = int.Parse(textBox10.Text); // Открытый ключ

            RSA_Class rs_ = new RSA_Class();
            Digital_ PodPS = new Digital_();

            BigInteger decryptedHash = rs_.Decrypt(BigInteger.Parse(extractedWord), e_, n);  // Расшифровка цифровой подписи с использованием открытого ключа

            // Преобразование числа в строку
            string decryptedMessage = decryptedHash.ToString();

            BigInteger hash_ = PodPS.GetHash(decryptedMessage);            // Получение хэша
            textBox16.Text = decryptedMessage;

            if (textBox15.Text == textBox16.Text) //проверка на совпадение хэшей
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
