using AutoMapper;

namespace CloudCare.API.Repositories;

public class ExpenseProfile : Profile
{

    public ExpenseProfile()
    {
        
        //from expense -> readexpenseDTO
        
        CreateMap<Models.Expense, DTOs.ReadExpenseDto>();
        
        CreateMap<DTOs.ExpenseForCreationDto, Models.Expense>();
        
        CreateMap<DTOs.ExpenseForUpdateDto, Models.Expense>();
    }
    
}