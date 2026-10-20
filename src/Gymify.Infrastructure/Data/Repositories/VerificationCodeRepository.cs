using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Gymify.Domain.Entities;
using Gymify.Domain.Repositories;

namespace Gymify.Infrastructure.Data.Repositories
{
    internal class VerificationCodeRepository : GenericRepository<VerificationCode>, IVerificationCodeRepository
    {

        private readonly DbSet<VerificationCode> _verificationCodes;


        public VerificationCodeRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
            _verificationCodes = context.Set<VerificationCode>();
        }

    }
}