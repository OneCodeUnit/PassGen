namespace PassGen
{
    /// <summary>
    /// Главная форма приложения для генерации паролей.
    /// Предоставляет графический интерфейс для выбора параметров и генерации криптографически стойких паролей.
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly PasswordGenerator _passwordGenerator = new();
        private string _password = string.Empty;
        private int _passwordLength = 12;

        /// <summary>
        /// Генерирует новый пароль на основе текущих настроек пользовательского интерфейса.
        /// В случае ошибки отображает сообщение об ошибке в поле пароля.
        /// </summary>
        /// <remarks>
        /// Метод собирает параметры из элементов управления формы, вызывает генератор паролей
        /// и обновляет поле отображения. Все исключения обрабатываются, и их сообщения
        /// выводятся пользователю вместо пароля.
        /// </remarks>
        private void GeneratePassword()
        {
            var options = new PasswordGenerator.PasswordOptions(
                Length: _passwordLength,
                IncludeCapital: CheckBoxCapital.Checked,
                IncludeLower: CheckBoxLower.Checked,
                IncludeNumbers: CheckBoxNumber.Checked,
                IncludeLine: CheckBoxLine.Checked,
                IncludeSpecial: CheckBoxSpecial.Checked,
                ForceCategories: CheckBoxForce.Checked,
                Readable: CheckBoxReadable.Checked
            );
            try
            {
                _password = _passwordGenerator.Generate(options);
            }
            catch (ArgumentException ex)
            {
                _password = ex.Message;
            }
            finally
            {
                FieldPassword.Text = _password;
            }
        }

        /// <summary>
        /// Инициализирует новый экземпляр формы MainForm.
        /// Настраивает начальное состояние UI и создает всплывающие подсказки для элементов управления.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            GeneratePassword();

            // Инициализация всплывающих подсказок
            ToolTip toolTip = new();
            toolTip.SetToolTip(CheckBoxLine, "Символы \"-\" и \"_\"");
            toolTip.SetToolTip(CheckBoxSpecial, "Разные специальные символы");
            toolTip.SetToolTip(CheckBoxNumber, "Включение цифр в пароль");
            toolTip.SetToolTip(CheckBoxLower, "Включение строчных букв в пароль (a-z)");
            toolTip.SetToolTip(CheckBoxCapital, "Включение заглавных букв в пароль (A-Z)");
            toolTip.SetToolTip(CheckBoxForce, "В пароле будет не менее одного символа из каждой отмеченной категории");
            toolTip.SetToolTip(CheckBoxReadable, "В пароле не будут попадаться символы, которые легко спутать (например, 0 и O или l и I)");
            toolTip.SetToolTip(ButtonCopy, "Скопировать пароль в буфер обмена");
            toolTip.SetToolTip(SizeBar, "Двигая влево и вправо, можно изменить длину пароля");
            toolTip.SetToolTip(LabelScrollValue, "Текущая длина пароля");
            toolTip.SetToolTip(ButtonGen, "Создать новый пароль");
            toolTip.SetToolTip(FieldPassword, "Созданные пароли. Нажми, чтобы скопировать");
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Создать".
        /// Запускает генерацию нового пароля.
        /// </summary>
        /// <param name="sender">Источник события (кнопка ButtonGen).</param>
        /// <param name="e">Данные события.</param>
        private void ButtonGen_Click(object sender, EventArgs e)
        {
            GeneratePassword();
        }

        /// <summary>
        /// Обработчик изменения состояния чекбоксов категорий символов.
        /// Автоматически перегенерирует пароль при изменении настроек.
        /// </summary>
        /// <param name="sender">Источник события (любой из чекбоксов).</param>
        /// <param name="e">Данные события.</param>
        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            GeneratePassword();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Копировать".
        /// Копирует текущий пароль в буфер обмена Windows.
        /// </summary>
        /// <param name="sender">Источник события (кнопка ButtonCopy).</param>
        /// <param name="e">Данные события.</param>
        /// <remarks>
        /// Копирование выполняется только если пароль не пустой.
        /// </remarks>
        private void ButtonCopy_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_password))
            {
                Clipboard.SetText(_password);
            }
        }

        /// <summary>
        /// Обработчик изменения позиции ползунка длины пароля.
        /// Обновляет длину пароля и автоматически генерирует новый пароль.
        /// </summary>
        /// <param name="sender">Источник события (TrackBar SizeBar).</param>
        /// <param name="e">Данные события.</param>
        private void SizeBar_Scroll(object sender, EventArgs e)
        {
            _passwordLength = SizeBar.Value;
            LabelScrollValue.Text = "Длина пароля: " + _passwordLength;
            GeneratePassword();
        }

        /// <summary>
        /// Обработчик нажатия на поле с паролем.
        /// Копирует текущий пароль в буфер обмена Windows.
        /// </summary>
        /// <param name="sender">Источник события (поле FieldPassword).</param>
        /// <param name="e">Данные события.</param>
        /// <remarks>
        /// Копирование выполняется только если пароль не пустой.
        /// </remarks>
        private void FieldPassword_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_password))
            {
                Clipboard.SetText(_password);
            }
        }
    }
}