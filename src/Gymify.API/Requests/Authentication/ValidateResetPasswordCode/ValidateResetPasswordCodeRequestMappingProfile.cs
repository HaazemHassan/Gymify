using AutoMapper;
using Gymify.Application.Features.Authentication.Queries.ValidateResetPasswordCode;

namespace Gymify.Api.Requests.Authentication.ValidateResetPasswordCode;

public class ValidateResetPasswordCodeRequestMappingProfile : Profile
{
    public ValidateResetPasswordCodeRequestMappingProfile()
    {
        CreateMap<ValidateResetPasswordCodeRequest, ValidateResetPasswordCodeQuery>();
    }
}
