using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrongPassword
{
    public partial class Form1 : Form
    {
        public List<string> RealizedList { get; set; }
        public List<string> UnRealizedList { get; set; }
        public int breakingCounter = 0;

        public Form1()
        {
            InitializeComponent();
            UnRealizedList = new List<string>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Add(textBox1.Text);
        }
        public void Add(string NewPassword)
        {
            var IsExist = UnRealizedList.Contains(NewPassword);
            if (IsExist)
            {
                MessageBox.Show("Item Exist");
                return;
            }
            if (NewPassword.Length != 8)
            {
                MessageBox.Show("Item length not equel 8");
                return;
            }
            UnRealizedList.Add(NewPassword);
            richTextBox1.Text += NewPassword + Environment.NewLine;
            textBox1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Proccess();
        }
        public void Proccess()
        {
            richTextBox2.Text = "";

            if (UnRealizedList.Count < 2)
            {
                MessageBox.Show("number of passwords array less that 2");
                return;
            }

            this.RealizedList = new List<string>();
            ForUnRealizedItems();

            richTextBox2.Text += string.Join(Environment.NewLine, RealizedList);
            MessageBox.Show("Done with " + breakingCounter + " try");
        }
        
        public void FillRealizedPasswords()
        {
            foreach (var password in UnRealizedList)
                if (IsRealized(password))
                    RealizedList.Add(password);

            UnRealizedList = UnRealizedList.Except(RealizedList).ToList();

        }
        public bool IsRealized(string Password)
        {
            var HaveDigit = Password.Where(c => char.IsDigit(c)).Any();
            var HaveLetter = Password.Where(c => char.IsLetter(c)).Any();
            var HaveSymbol = Password.Where(c => !char.IsLetterOrDigit(c)).Any();

            return HaveDigit && HaveLetter && HaveSymbol;
        }
        public void ForUnRealizedItems()
        {
            breakingCounter += 1;
            if (breakingCounter > 7555)
            {
                MessageBox.Show("Password Not Resolived");
                return;
            }

            for (int i = 0; i < 2; i++)
            {
                CrossOver(); 
                FillRealizedPasswords();
                if (UnRealizedList.Count < 2) return;
            }


            for (int i = 0; i < UnRealizedList.Count -1; i++)
            {
                UnRealizedList[i] = MutationProccess(UnRealizedList[i]);
            }
            ForUnRealizedItems();
        }
        public void CrossOver()
        {
            int itteration = UnRealizedList.Count / 2;
            var list = new List<string>();
            for (int i = 0; i < itteration; i += 2) 
            {
                CrossOverProccess(i, i + 1);
            }
        }
        public void CrossOverProccess(int index, int indexPlusOne)
        {
            var password1 = UnRealizedList[index];
            var password2 = UnRealizedList[indexPlusOne];

            var temp = password1.Substring(2, 4);
            password1 = password1.Substring(0, 2) + password2.Substring(2, 4) + password1.Substring(6, 2);
            password2 = password2.Substring(0, 2) + temp + password2.Substring(6, 2);


            UnRealizedList[index] = password1;
            UnRealizedList[indexPlusOne] = password2;
        }
        public string MutationProccess(string Password)
        {
            var HaveDigit = Password.Where(c => char.IsDigit(c)).Any();
            var HaveLetter = Password.Where(c => char.IsLetter(c)).Any();
            var HaveSymbol = Password.Where(c => !char.IsLetterOrDigit(c)).Any();

            

            var PasswordArray = Password.ToCharArray();
            var random = new Random();
            var RandomIndex = random.Next(0, PasswordArray.Length - 1);
            int RandomValue = 0;

            if (!HaveSymbol)
                RandomValue = random.Next(32, 49);
            else if (!HaveDigit)
                RandomValue = random.Next(48, 57);
            else if (!HaveLetter)
                RandomValue = random.Next(65, 90);
            else             
                return Password;

            PasswordArray[RandomIndex] = (char)RandomValue;
            return string.Join("", PasswordArray);


        }

        private void button3_Click(object sender, EventArgs e)
        {
            var statics = new List<string>{
                "Abc*e$g@",
                "GHIJKLMN",
                "12345678",
                "@!%$&*#!",
                "12345578",
                "vutsrqpo",
                "++$Re#%r",
                "12345679"
            };
            foreach (var password in statics)
                Add(password);
            Proccess();
        }
    }
}
