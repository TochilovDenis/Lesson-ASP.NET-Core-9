using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Print_line.Pages.Models;

namespace Print_line.Pages
{
    public class QuoteModel : PageModel    {

         public readonly List<string> Quotes = new List<string>()
        {
            "цитата1","цитата2","цитата3"
        };

        public readonly List<string> Author = new List<string>()
        {
            "автор1","автор2","автор3"
        };

        public QuoteModel() { }

        public int RQ {  get; set; }

        public string textQuotes {  get; set; }

        public string textAuthor { get; set; }

        

        public void OnGet()
        {
            var rq = new Random();
            RQ = rq.Next(Quotes.Count);
            textQuotes = Quotes[RQ];
            textAuthor = Author[RQ];

        }
    }
}
