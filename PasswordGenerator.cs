using System.Security.Cryptography;
using System.Text;

namespace PassGen
{
    /// <summary>
    /// Генератор криптографически стойких случайных паролей.
    /// Использует System.Security.Cryptography.RandomNumberGenerator для обеспечения безопасности.
    /// </summary>
    /// <example>
    /// <code>
    /// var generator = new PasswordGenerator();
    /// var options = new PasswordGenerator.PasswordOptions(
    ///     Length: 16,
    ///     IncludeCapital: true,
    ///     IncludeLower: true,
    ///     IncludeNumbers: true,
    ///     IncludeLine: false,
    ///     IncludeSpecial: true,
    ///     ForceCategories: true,
    ///     Readable: false
    /// );
    /// string password = generator.Generate(options);
    /// </code>
    /// </example>
    public class PasswordGenerator
    {
        private const string AlphabetCapitalDefault = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string AlphabetCapitalReadable = "ABCDEFGHJKMNPQRSTUVWXYZ";
        private const string AlphabetLowerDefault = "abcdefghijklmnopqrstuvwxyz";
        private const string AlphabetLowerReadable = "abcdefghjkpqrstuvwxyz";
        private const string AlphabetNumberDefault = "0123456789";
        private const string AlphabetNumberReadable = "23456789";
        private const string AlphabetLineDefault = "-_";
        private const string AlphabetLineReadable = "-_";
        private const string AlphabetSpecialDefault = "`~!@#$%^&*()+={}[]\\|:;\"\'<>,.?/";
        private const string AlphabetSpecialReadable = "@#$%+=?";

        /// <summary>
        /// Генерирует случайный пароль на основе заданных параметров.
        /// </summary>
        /// <param name="options">Параметры генерации пароля.</param>
        /// <returns>Сгенерированный пароль.</returns>
        /// <exception cref="ArgumentException">
        /// Выбрасывается, если не выбрана ни одна категория символов или если длина пароля 
        /// меньше количества обязательных категорий при включенном режиме ForceCategories.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Выбрасывается, если длина пароля выходит за пределы допустимого диапазона (6-64 символа).
        /// </exception>
        /// <remarks>
        /// Метод использует криптографически стойкий генератор случайных чисел 
        /// <see cref="RandomNumberGenerator"/> для обеспечения непредсказуемости паролей.
        /// </remarks>
        public string Generate(PasswordOptions options)
        {
            if (!options.IncludeCapital && !options.IncludeLower &&
                !options.IncludeNumbers && !options.IncludeLine && !options.IncludeSpecial)
                throw new ArgumentException(
                    "Должна быть выбрана хотя бы одна категория символов", 
                    nameof(options));

            if (options.Length < 6 || options.Length > 64)
                throw new ArgumentOutOfRangeException(
                    nameof(options.Length), 
                    "Длина пароля должна быть от 6 до 64"); 
            
            var alphabet = BuildAlphabet(options);

            Span<char> password = stackalloc char[options.Length];
            for (int i = 0; i < options.Length; i++)
            {
                int randomIndex = RandomNumberGenerator.GetInt32(0, alphabet.Length);
                password[i] = alphabet[randomIndex];
            }

            if (options.ForceCategories)
            {
                List<char> selectedSymbols = SelectSymbols(options);
                if (options.Length < selectedSymbols.Count)
                {
                    throw new ArgumentException(
                        $"Выбрано слишком много категорий для длины ({options.Length})",
                        nameof(options.Length));
                }

                List<int> selectedPositions = SelectPositions(options.Length, selectedSymbols.Count);

                for (int i = 0; i < selectedPositions.Count; i++)
                {
                    password[selectedPositions[i]] = selectedSymbols[i];
                }
            }

            return new string(password);
        }

        /// <summary>
        /// Выбирает случайные уникальные позиции в пароле для вставки обязательных символов.
        /// </summary>
        /// <param name="passwordLength">Длина пароля.</param>
        /// <param name="positionsCount">Количество позиций для выбора.</param>
        /// <returns>Список уникальных индексов.</returns>
        /// <exception cref="ArgumentException">
        /// Выбрасывается, если количество позиций превышает длину пароля.
        /// </exception>
        private List<int> SelectPositions(int passwordLength, int positionsCount)
        {
            if (positionsCount > passwordLength)
                throw new ArgumentException("Количество позиций превышает длину пароля");
            HashSet<int> selectedPositions = new(positionsCount);
            while (selectedPositions.Count < positionsCount)
                selectedPositions.Add(RandomNumberGenerator.GetInt32(0, passwordLength));

            return [.. selectedPositions];
        }

        /// <summary>
        /// Выбирает по одному случайному символу из каждой включённой категории.
        /// </summary>
        /// <param name="options">Параметры генерации с указанием активных категорий.</param>
        /// <returns>Список символов для обязательного включения в пароль.</returns>
        private List<char> SelectSymbols(PasswordOptions options)
        {
            List<char> selectedSymbols = [];

            if (options.IncludeCapital)
                selectedSymbols.Add(SelectSymbol(options.Readable, AlphabetCapitalReadable, AlphabetCapitalDefault));

            if (options.IncludeLower)
                selectedSymbols.Add(SelectSymbol(options.Readable, AlphabetLowerReadable, AlphabetLowerDefault));

            if (options.IncludeNumbers)
                selectedSymbols.Add(SelectSymbol(options.Readable, AlphabetNumberReadable, AlphabetNumberDefault));

            if (options.IncludeLine)
                selectedSymbols.Add(SelectSymbol(options.Readable, AlphabetLineReadable, AlphabetLineDefault));

            if (options.IncludeSpecial)
                selectedSymbols.Add(SelectSymbol(options.Readable, AlphabetSpecialReadable, AlphabetSpecialDefault));

            return selectedSymbols;
        }

        /// <summary>
        /// Выбирает случайный символ из указанного алфавита.
        /// </summary>
        /// <param name="readable">Использовать режим улучшенной читаемости (исключает визуально похожие символы).</param>
        /// <param name="alphabetReadable">Алфавит для режима улучшенной читаемости.</param>
        /// <param name="alphabetDefault">Стандартный алфавит.</param>
        /// <returns>Случайный символ из выбранного алфавита.</returns>
        private static char SelectSymbol(bool readable, string alphabetReadable, string alphabetDefault)
        {
            string alphabet = readable ? alphabetReadable : alphabetDefault;
            int index = RandomNumberGenerator.GetInt32(0, alphabet.Length);
            return alphabet[index];
        }

        /// <summary>
        /// Строит алфавит на основе выбранных категорий символов.
        /// </summary>
        /// <param name="options">Параметры генерации с указанием активных категорий.</param>
        /// <returns>Строка, содержащая все символы из выбранных категорий.</returns>
        private string BuildAlphabet(PasswordOptions options)
        {
            StringBuilder alphabet = new();

            if (options.IncludeCapital)
                alphabet.Append(options.Readable ? AlphabetCapitalReadable : AlphabetCapitalDefault);

            if (options.IncludeLower)
                alphabet.Append(options.Readable ? AlphabetLowerReadable : AlphabetLowerDefault);

            if (options.IncludeNumbers)
                alphabet.Append(options.Readable ? AlphabetNumberReadable : AlphabetNumberDefault);

            if (options.IncludeLine)
                alphabet.Append(options.Readable ? AlphabetLineReadable : AlphabetLineDefault);

            if (options.IncludeSpecial)
                alphabet.Append(options.Readable ? AlphabetSpecialReadable : AlphabetSpecialDefault);

            return alphabet.ToString();
        }

        /// <summary>
        /// Параметры генерации пароля.
        /// </summary>
        /// <param name="Length">Длина пароля в символах (от 6 до 64).</param>
        /// <param name="IncludeCapital">Включить заглавные буквы (A-Z).</param>
        /// <param name="IncludeLower">Включить строчные буквы (a-z).</param>
        /// <param name="IncludeNumbers">Включить цифры (0-9).</param>
        /// <param name="IncludeLine">Включить символы-разделители (-, _).</param>
        /// <param name="IncludeSpecial">Включить специальные символы (!@#$%^&* и др.).</param>
        /// <param name="ForceCategories">
        /// Необходимо гарантировать наличие минимум одного символа из каждой выбранной категории.
        /// Если ForceCategories true, длина пароля должна быть не меньше количества выбранных категорий.
        /// </param>
        /// <param name="Readable">
        /// Исключить символы, которые легко спутать (0/O, 1/l/I и т.д.).
        /// Уменьшает энтропию, но повышает удобство использования.
        /// </param>
        public record PasswordOptions(
            int Length,
            bool IncludeCapital,
            bool IncludeLower,
            bool IncludeNumbers,
            bool IncludeLine,
            bool IncludeSpecial,
            bool ForceCategories,
            bool Readable
        );
    }
}