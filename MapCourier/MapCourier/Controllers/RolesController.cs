using Microsoft.AspNetCore.Identity;  
using Microsoft.AspNetCore.Mvc;  
using System.Linq;  
using System.Threading.Tasks;  
       
namespace MapCourier.Controllers  
{  
    public class RolesController : Controller  
    {  
        RoleManager<IdentityRole> roleManager;
             
      
        public RolesController(RoleManager<IdentityRole> roleManager)  
        {  
            this.roleManager = roleManager;  
        }  
      
        public IActionResult Index()  
        {  
            var roles = roleManager.Roles.ToList();  
            return View(roles);  
        }  
      
        public IActionResult Create()  
        {  
            return View(new IdentityRole());  
        }  
      
        [HttpPost]  
        public async Task<IActionResult> Create(IdentityRole role)  
        {  
            await roleManager.CreateAsync(role);  
            return RedirectToAction("Index");  
        }        
    }  
}  

