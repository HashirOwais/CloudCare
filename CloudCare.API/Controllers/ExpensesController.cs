using Microsoft.AspNetCore.Mvc;

namespace CloudCare.API.Controllers;


[ApiController]
[Route("/api/Expenses")]
public class ExpensesController : ControllerBase
{
    [HttpGet]
    public ActionResult GetExpenses()
    {
        return NotFound("Lock in"); 
    }
    
    
    
}