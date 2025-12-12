using AutoMapper;
using CloudCare.Business.DTOs;
using CloudCare.Data.Models;

namespace CloudCare.Business.Profiles;

public class CloudcareProfile : Profile
{
    public CloudcareProfile()
    {
        // For returning data to client (read DTO)
        CreateMap<Expense, ReadExpenseDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.Vendor, opt => opt.MapFrom(src => src.Vendor.Name))
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.Name))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.VendorId, opt => opt.MapFrom(src => src.VendorId))
            .ForMember(dest => dest.PaymentMethodId, opt => opt.MapFrom(src => src.PaymentMethodId));
        // Date maps automatically (DateOnly -> DateOnly)

        // For creation (creation DTO -> model)
        CreateMap<ExpenseForCreationDto, Expense>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date));
        // Do NOT map Category, Vendor, PaymentMethod navigation properties here!

        // For update (update DTO -> model)
        CreateMap<ExpenseForUpdateDto, Expense>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.BillingCycle, opt =>
                opt.MapFrom(src => src.BillingCycle))
            .ForMember(dest => dest.RecurrenceSourceId, opt =>
                opt.Ignore());
        // Again, DO NOT map navigation properties here.

        //mapping b/w UserForCreationDto and User model
        CreateMap<UserForCreationDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Id is auto-generated
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "provider")) // Default role
            .ForMember(dest => dest.IsRegistered, opt => opt.MapFrom(src => true)) // Mark as registered
            .ForMember(dest => dest.UserCreated, opt => opt.Ignore()) // UserCreated is set by model/DB
            .ForMember(dest => dest.Expenses, opt => opt.Ignore()); // Expenses are a collection

        CreateMap<User, UserForReadDTO>();
    }
}
