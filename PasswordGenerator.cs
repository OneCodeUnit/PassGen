using System.Numerics;
using System.Security.Cryptography;


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
            
            var categories = GetCategories(options);
            var alphabet = string.Concat(categories);
            Span<char> password = stackalloc char[options.Length];
            // Отбрасываем неподходящие кандидаты целиком: каждый допустимый пароль
            // сохраняет одинаковую вероятность, обязательные позиции не перезаписываются.
            bool accepted;
            do
            {
                for (int i = 0; i < password.Length; i++)
                    password[i] = alphabet[RandomNumberGenerator.GetInt32(alphabet.Length)];
                accepted = true;
                if (options.ForceCategories)
                    foreach (var category in categories)
                        if (password.IndexOfAny(category.AsSpan()) < 0)
                        {
                            accepted = false;
                            break;
                        }
            } while (!accepted);
            return new string(password);
        }

        // Единый источник алфавитов для генерации и расчёта энтропии.
        private static List<string> GetCategories(PasswordOptions options)
        {
            List<string> categories = [];
            if (options.IncludeCapital) categories.Add(options.Readable ? AlphabetCapitalReadable : AlphabetCapitalDefault);
            if (options.IncludeLower) categories.Add(options.Readable ? AlphabetLowerReadable : AlphabetLowerDefault);
            if (options.IncludeNumbers) categories.Add(options.Readable ? AlphabetNumberReadable : AlphabetNumberDefault);
            if (options.IncludeLine) categories.Add(options.Readable ? AlphabetLineReadable : AlphabetLineDefault);
            if (options.IncludeSpecial) categories.Add(options.Readable ? AlphabetSpecialReadable : AlphabetSpecialDefault);
            return categories;
        }

        private static string BuildAlphabet(PasswordOptions options) => string.Concat(GetCategories(options));

        /// <summary>Энтропия равномерного распределения допустимых паролей, в битах.</summary>
        public double GetEntropy(PasswordOptions options)
        {
            if (options.Length < 6 || options.Length > 64)
                throw new ArgumentOutOfRangeException(nameof(options.Length));
            var categories = GetCategories(options);
            int size = categories.Sum(c => c.Length);
            if (size == 0) return 0;
            if (!options.ForceCategories) return options.Length * Math.Log2(size);

            // Включения-исключения по отсутствующим категориям. BigInteger исключает
            // потерю точности при вычитании больших близких чисел (до 94^64).
            BigInteger count = 0;
            for (int mask = 0; mask < (1 << categories.Count); mask++)
            {
                int remaining = size, excluded = 0;
                for (int i = 0; i < categories.Count; i++)
                    if ((mask & (1 << i)) != 0)
                    {
                        remaining -= categories[i].Length;
                        excluded++;
                    }
                var term = BigInteger.Pow(remaining, options.Length);
                count += excluded % 2 == 0 ? term : -term;
            }
            return BigInteger.Log(count, 2);
        }
        /// <summary>
        /// Возвращает размер алфавита, из которого строится пароль с заданными параметрами.
        /// Для точной оценки энтропии используйте GetEntropy.
        /// </summary>
        /// <param name="options">Параметры генерации с указанием активных категорий.</param>
        /// <returns>Количество символов в алфавите (0, если не выбрана ни одна категория).</returns>
        public int GetAlphabetSize(PasswordOptions options) => BuildAlphabet(options).Length;

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