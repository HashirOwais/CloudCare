using AutoMapper;

namespace CloudCare.API.Repositories;

public class ExpenseProfile : Profile
{

    public ExpenseProfile()
    {
        CreateMap<Models.Expense, DTOs.ReadExpenseDto>();
    }
    
}