using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HW_30_06.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public int DayOfYear { get; private set; }
        public char RandomLetter { get; private set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            DayOfYear = DateTime.Now.DayOfYear;

            RandomLetter = GetRandomLetter();
        }
        private static char GetRandomLetter()
        {
            Random random = new Random();
            // Генерация случайной буквы от A до Z
            return (char)('A' + random.Next(0, 26));
        }
    }
}


/*
  * Задание 1.(DayOfYear)
     Создайте и запустите базовое приложение ASP.NET Core, построенное на основании ASP.NET Razor Pages.
     В веб-странице выведите номер текущего дня в году.

  * Задание 2.(RandomLetter)
    Создайте и запустите базовое приложение ASP.NET Core, построенное на основании ASP.NET Razor Pages.
    В веб-странице в зависимости от случайного значения отображайте любую букву из английского алфавита.
 */