using Gymify.Domain.Common.Enums;
using Gymify.Domain.DomainEvents;
using Gymify.Domain.Entities.Base;

namespace Gymify.Domain.Entities
{
    public sealed class DomainUser : FullAuditableEntity<int>
    {
        private DomainUser()
        {
        }

        public DomainUser(UserType userType,
            string name,
            string email,
            string? phoneNumber = default,
            bool isActive = true
            )
        {
            UserType = userType;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            IsActive = isActive;

            RaiseDomainEvent(new UserRegisteredDomainEvent(Email));
        }


        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string? PhoneNumber { get; private set; }
        public UserType UserType { get; private set; }
        public bool IsActive { get; private set; } = true;

        public Customer? Customer { get; private set; }
        public Employee? Employee { get; private set; }



        public void UpdateInfo(string? name = null, string? phoneNumber = null)
        {
            if (!string.IsNullOrWhiteSpace(name))
                Name = name;

            if (!string.IsNullOrWhiteSpace(phoneNumber))
                PhoneNumber = phoneNumber;

        }



        public void SetActive(bool isActive)
        {
            IsActive = isActive;
        }

        public void UpdateEmail(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                Email = email;
            }
        }


    }
}
