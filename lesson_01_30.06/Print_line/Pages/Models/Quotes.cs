namespace Print_line.Pages.Models
{
    public class Quotes
    {

        public Quotes(string q, string a) { 
            this.QuoteText = q;
            this.AuthorText = a;
        }
        public required string QuoteText { get; set; }
        public required string AuthorText{ get; set; }
    }
    }
