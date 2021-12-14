using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

public partial class _Default : Page
{
    [HttpPost]

    public static string GetDate(string someParameter)
    {
        return DateTime.Now.ToString();
    }
}