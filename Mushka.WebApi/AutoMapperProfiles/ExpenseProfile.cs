using System.Collections.Generic;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Expenses;
using Mushka.WebApi.Resolvers;

namespace Mushka.WebApi.AutoMapperProfiles
{
    public class ExpenseProfile : Profile
    {
        public ExpenseProfile()
        {
            CreateMap<ExpenseRequestModel, Expense>().ConvertUsing<ExpenseRequestConverter>();
            CreateMap<Expense, ExpenseModel>().ConvertUsing<ExpenseConverter>();

            CreateMap<ValidationResponse<Expense>, ExpenseResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<ExpenseResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<Expense>>, ExpensesResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<ExpenseResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));
        }
    }
}