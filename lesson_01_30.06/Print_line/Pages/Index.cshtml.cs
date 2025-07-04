using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Print_line.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    private readonly IConfiguration _configuration;


    public string SettingMsg { get; set; }
    
    public IndexModel(ILogger<IndexModel> logger, IConfiguration config)
    {
        _logger = logger;
        _configuration = config;
    }

    public void OnGet()
    {
        SettingMsg = _configuration["AppSettings:WelcomeMessage"];
    }
}
