using System.Security.Cryptography;

namespace PassGen
{
    public partial class MainForm : Form
    {
        string AlphabetCapital = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string AlphabetLower = "abcdefghijklmnopqrstuvwxyz";
        string AlphabetNumber = "0123456789";
        const string AlphabetLine = "-_";
        string AlphabetSpecial = "`~!@#$%^&*()+={}[]\\|:;\"\'<>,.?/";
        string Password = string.Empty;
        int PasswordLength = 12;

        private void GeneratePassword()
        {
            if (CheckBoxReadable.Checked)
            {
                AlphabetCapital = "ABCDEFGHJKPQRSTUVWXYZ";
                AlphabetLower = "abcdefghjkpqrstuvwxyz";
                AlphabetSpecial = "@#$%+=?";
                AlphabetNumber = "23456789";
            }

            string alphabet = string.Empty;
            if (CheckBoxCapital.Checked) { alphabet += AlphabetCapital; }
            if (CheckBoxLower.Checked) { alphabet += AlphabetLower; }
            if (CheckBoxNumber.Checked) { alphabet += AlphabetNumber; }
            if (CheckBoxLine.Checked) { alphabet += AlphabetLine; }
            if (CheckBoxSpecial.Checked) { alphabet += AlphabetSpecial; }

            int alphabetSize = alphabet.Length;
            if (alphabetSize == 0)
            {
                FieldPassword.Text = string.Empty;
                return;
            }

            Password = string.Empty;
            for (int i = 0; i < PasswordLength; i++)
            {
                int randomNumber = RandomNumberGenerator.GetInt32(0, alphabetSize);
                string randomSymbol = alphabet[randomNumber].ToString();
                Password += randomSymbol;
            }

            if (CheckBoxForce.Checked)
            {
                ForceAddition();
            }

            FieldPassword.Text = Password;
        }

        private void ForceAddition()
        {
            int checkedCount = 0;
            if (CheckBoxCapital.Checked) { checkedCount++; }
            if (CheckBoxLower.Checked) { checkedCount++; }
            if (CheckBoxNumber.Checked) { checkedCount++; }
            if (CheckBoxLine.Checked) { checkedCount++; }
            if (CheckBoxSpecial.Checked) { checkedCount++; }

            if (checkedCount > PasswordLength)
            {
                Password = string.Empty;
                return;
            }

            string positions = string.Empty;
            int position = 0;
            int randomPosition;
            for (int i = 0; i < checkedCount; i++)
            {
                do
                {
                    randomPosition = RandomNumberGenerator.GetInt32(0, PasswordLength);
                }
                while (positions.Contains(randomPosition.ToString()));
                positions += randomPosition;
            }
            if (CheckBoxCapital.Checked)
            {
                int randomNumber = RandomNumberGenerator.GetInt32(0, AlphabetCapital.Length);
                string randomSymbol = AlphabetCapital[randomNumber].ToString();
                Password = Password.Remove(Convert.ToInt32(positions[position].ToString()), 1);
                Password = Password.Insert(Convert.ToInt32(positions[position].ToString()), randomSymbol);
                position++;
            }
            if (CheckBoxLower.Checked)
            {
                int randomNumber = RandomNumberGenerator.GetInt32(0, AlphabetLower.Length);
                string randomSymbol = AlphabetLower[randomNumber].ToString();
                Password = Password.Remove(Convert.ToInt32(positions[position].ToString()), 1);
                Password = Password.Insert(Convert.ToInt32(positions[position].ToString()), randomSymbol);
                position++;
            }
            if (CheckBoxNumber.Checked)
            {
                int randomNumber = RandomNumberGenerator.GetInt32(0, AlphabetNumber.Length);
                string randomSymbol = AlphabetNumber[randomNumber].ToString();
                Password = Password.Remove(Convert.ToInt32(positions[position].ToString()), 1);
                Password = Password.Insert(Convert.ToInt32(positions[position].ToString()), randomSymbol);
                position++;
            }
            if (CheckBoxLine.Checked)
            {
                int randomNumber = RandomNumberGenerator.GetInt32(0, AlphabetLine.Length);
                string randomSymbol = AlphabetLine[randomNumber].ToString();
                Password = Password.Remove(Convert.ToInt32(positions[position].ToString()), 1);
                Password = Password.Insert(Convert.ToInt32(positions[position].ToString()), randomSymbol);
                position++;
            }
            if (CheckBoxSpecial.Checked)
            {
                int randomNumber = RandomNumberGenerator.GetInt32(0, AlphabetSpecial.Length);
                string randomSymbol = AlphabetSpecial[randomNumber].ToString();
                Password = Password.Remove(Convert.ToInt32(positions[position].ToString()), 1);
                Password = Password.Insert(Convert.ToInt32(positions[position].ToString()), randomSymbol);
                position++;
            }
        }

        public MainForm()
        {
            InitializeComponent();
            GeneratePassword();
            ToolTip toolTip = new();
            toolTip.SetToolTip(CheckBoxLine, "������� \"-\" � \"_\"");
            toolTip.SetToolTip(CheckBoxSpecial, "������ ����������� �������");
            toolTip.SetToolTip(CheckBoxNumber, "��������� ���� � ������");
            toolTip.SetToolTip(CheckBoxLower, "��������� �������� ���� � ������ (a-z)");
            toolTip.SetToolTip(CheckBoxCapital, "��������� ��������� ���� � ������ (A-Z)");
            toolTip.SetToolTip(CheckBoxForce, "� ������ ����� �� ����� ������ ������� �� ������ ���������� ���������");
            toolTip.SetToolTip(CheckBoxReadable, "� ������ �� ����� ���������� �������, ������� ����� ������� (��������, 0 � O ��� l � I)");
            toolTip.SetToolTip(ButtonCopy, "����������� ������ � ����� ������");
            toolTip.SetToolTip(SizeBar, "������ ����� � ������, ����� �������� ����� ������");
            toolTip.SetToolTip(LabelScrollValue, "������� ����� ������");
            toolTip.SetToolTip(ButtonGen, "������� ����� ������");
            toolTip.SetToolTip(FieldPassword, "��������� ������. ���� ���� ������, ������ ��������� ��������� �� ��������� ������� ���");
        }

        private void ButtonGen_Click(object sender, EventArgs e)
        {
            GeneratePassword();
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            GeneratePassword();
        }

        private void ButtonCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Password);
        }

        private void SizeBar_Scroll(object sender, EventArgs e)
        {
            PasswordLength = SizeBar.Value;
            LabelScrollValue.Text = "����� ������: " + PasswordLength;
            GeneratePassword();
        }
    }
}