using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Gymify.Domain.Entities;
using Gymify.Domain.Repositories;

namespace Gymify.Infrastructure.Data.Repositories
{
    internal class UserRepository : GenericRepository<DomainUser>, IUserRepository
    {

        private readonly DbSet<DomainUser> _users;


        public UserRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
            _users = context.Set<DomainUser>();
        }

    }
}
