using AutoMapper;
using CloudCare.API.DTOs;
using CloudCare.API.Models;

namespace CloudCare.API.Profiles;

public class ExpenseProfile : Profile
{
    public ExpenseProfile()
    {
        // For returning data to client (read DTO)
        CreateMap<Expense, ReadExpenseDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.Vendor, opt => opt.MapFrom(src => src.Vendor.Name))
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.Name))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.VendorId, opt => opt.MapFrom(src => src.VendorId))
            .ForMember(dest => dest.PaymentMethodId, opt => opt.MapFrom(src => src.PaymentMethodId));

        // For creation (creation DTO -> model)
        CreateMap<ExpenseForCreationDto, Expense>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.Date, DateTimeKind.Utc)));
            // Do NOT map Category, Vendor, PaymentMethod navigation properties here!

        // For update (update DTO -> model)
        CreateMap<ExpenseForUpdateDto, Expense>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.Date, DateTimeKind.Utc)));
            // Again, DO NOT map navigation properties here.
    }
}
