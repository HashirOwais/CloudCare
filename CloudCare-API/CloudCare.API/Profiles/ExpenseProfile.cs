using AutoMapper;
using CloudCare.API.DTOs;
using CloudCare.API.Models;

namespace CloudCare.API.Profiles;

// AutoMapper profile that defines how to convert between Expense-related models and DTOs
public class ExpenseProfile : Profile
{
    public ExpenseProfile()
    {
        // Mapping from Expense model to ReadExpenseDto
        // - This is used when returning data to the client.
        // - It flattens nested objects (Category, Vendor, PaymentMethod) into just their names.
        CreateMap<Expense, ReadExpenseDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))           // Map Category.Name to Category (string)
            .ForMember(dest => dest.Vendor, opt => opt.MapFrom(src => src.Vendor.Name))               // Map Vendor.Name to Vendor (string)
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.Name)) // Map PaymentMethod.Name to PaymentMethod (string)

            // Optionally map the foreign key IDs to allow frontend to use them in forms
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.VendorId, opt => opt.MapFrom(src => src.VendorId))
            .ForMember(dest => dest.PaymentMethodId, opt => opt.MapFrom(src => src.PaymentMethodId));

        // Mapping from ExpenseForCreationDto to Expense model
        // - This is used when creating a new expense from frontend data.
        // - We only get the IDs for Category/Vendor/PaymentMethod, so we create "stub" objects for EF to link.
        CreateMap<ExpenseForCreationDto, Expense>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => new Category { Id = src.CategoryId }))
            .ForMember(dest => dest.Vendor, opt => opt.MapFrom(src => new Vendor { Id = src.VendorId }))
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => new PaymentMethod { Id = src.PaymentMethodId }));

        // Mapping from ExpenseForUpdateDto to Expense model
        // - Similar to creation, but used when editing an existing expense.
        // - The ID of the expense is passed separately (usually via route), and the DTO provides updated values.
        CreateMap<ExpenseForUpdateDto, Expense>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => new Category { Id = src.CategoryId }))
            .ForMember(dest => dest.Vendor, opt => opt.MapFrom(src => new Vendor { Id = src.VendorId }))
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => new PaymentMethod { Id = src.PaymentMethodId }));
    }
}