﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Expenses;
using Mushka.WebApi.ClientModels.Expenses.Describe;
using Mushka.WebApi.ClientModels.Expenses.Search;
using Mushka.WebApi.Resolvers.Expenses;

namespace Mushka.WebApi.AutoMapperProfiles
{
    public class ExpenseProfile : Profile
    {
        public ExpenseProfile()
        {
            CreateMap<OperationResult<IEnumerable<Expense>>, SearchExpensesResponseModel>()
                .ConvertUsing<SearchExpensesResponseConverter>();

            CreateMap<OperationResult<Expense>, DescribeExpenseResponseModel>()
                .ConvertUsing<DescribeExpenseResponseConverter>();

            CreateMap<ExpenseRequestModel, Expense>()
                .ConvertUsing<ExpenseRequestConverter>();

            //CreateMap<Expense, ExpenseModel>().ConvertUsing<ExpenseConverter>();

            //CreateMap<OperationResult<Expense>, ExpenseResponseModel>()
            //    .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.Status))
            //    .ForMember(dest => dest.Data, opt => opt.ResolveUsing<ExpenseResponseResolver>())
            //    .ForMember(dest => dest.Success, opt => opt.MapFrom(src => src.IsSuccess))
            //    .ForMember(dest => dest.Errors, opt => opt.MapFrom(src => src.Errors.Select(x => x.ErrorKey)));

            //CreateMap<OperationResult<IEnumerable<Expense>>, ExpensesResponseModel>()
            //    .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.Status))
            //    .ForMember(dest => dest.Data, opt => opt.ResolveUsing<ExpenseResponseResolver>())
            //    .ForMember(dest => dest.Success, opt => opt.MapFrom(src => src.IsSuccess))
            //    .ForMember(dest => dest.Errors, opt => opt.MapFrom(src => src.Errors.Select(x => x.ErrorKey)));
        }
    }
}