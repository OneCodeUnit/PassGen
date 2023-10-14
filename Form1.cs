using System.Security.Cryptography;

namespace PassGen
{
    public partial class MainForm : Form
    {
        static readonly string AlphabetCapital = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        static readonly string AlphabetLower = "abcdefghijklmnopqrstuvwxyz";
        static readonly string AlphabetNumber = "0123456789";
        static readonly string AlphabetLine = "-_";
        static readonly string AlphabetSpecial = "`~!@#$%^&*()+={}[]\\|:;\"\'<>,.?/";
        static readonly string AlphabetSpace = " ";
        string Password = string.Empty;
        int PasswordLength = 12;

        private void GeneratePassword()
        {
            string alphabet = string.Empty;
            if (CheckBoxCapital.Checked) { alphabet += AlphabetCapital; }
            if (CheckBoxLower.Checked) { alphabet += AlphabetLower; }
            if (CheckBoxNumber.Checked) { alphabet += AlphabetNumber; }
            if (CheckBoxLine.Checked) { alphabet += AlphabetLine; }
            if (CheckBoxSpecial.Checked) { alphabet += AlphabetSpecial; }
            if (CheckBoxSpace.Checked) { alphabet += AlphabetSpace; }

            int alphabetSize = alphabet.Length;
            if (alphabetSize == 0)
                return;

            Password = string.Empty;
            for (int i = 0; i < PasswordLength; i++)
            {
                int randomNumber = RandomNumberGenerator.GetInt32(0, alphabetSize);
                char randomSymbol = alphabet[randomNumber];
                Password += randomSymbol;
            }

            FieldPassword.Text = Password;
        }

        public MainForm()
        {
            InitializeComponent();
            GeneratePassword();
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
            LabelScrollValue.Text = "Длина пароля: " + PasswordLength;
            GeneratePassword();
        }
    }
}