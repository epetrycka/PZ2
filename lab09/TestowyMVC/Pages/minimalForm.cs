using Microsoft.AspNetCore.Mvc.RazorPages;

public class MinimalForm : PageModel
{
    public string ?mojtekst { get; set; }
 
    public void OnGet()
    {
    }
 
    public void OnPostSubmit(IFormCollection form)
    {
        this.mojtekst = string.Format("Mój tekst: {0}", form["mojtekst"].ToString());
    }
}
