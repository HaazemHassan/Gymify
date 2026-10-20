using Gymify.Domain.Common.Enums;
using Gymify.Domain.Entities.Base;

namespace Gymify.Domain.Entities
{
    public sealed class Customer : FullAuditableEntity<int>
    {
        private Customer()
        {
        }

        public Customer(
            int domainUserId,
            double? height = default,
            double? weight = default,
            ApprovalStatus approvalStatus = ApprovalStatus.Pending)
        {
            DomainUserId = domainUserId;
            Height = height;
            Weight = weight;
            ApprovalStatus = approvalStatus;
        }

        public int DomainUserId { get; private set; }
        public DomainUser DomainUser { get; private set; } = null!;

        public string? Code { get; private set; }
        public double? Height { get; private set; }
        public double? Weight { get; private set; }
        public int? Age { get; private set; }
        public string? Injuries { get; private set; }
        public string? ChronicDiseases { get; private set; }
        public double? MusclePercentage { get; private set; }
        public double? FatPercentage { get; private set; }
        public string? Notes { get; private set; }
        public ApprovalStatus ApprovalStatus { get; private set; } = ApprovalStatus.Pending;

        public ICollection<Subscription> Subscriptions { get; private set; } = new List<Subscription>();
        public ICollection<Attendance> Attendances { get; private set; } = new List<Attendance>();

        public void UpdatePhysicalAttributes(double? height, double? weight)
        {
            if (height.HasValue)
                Height = height.Value;

            if (weight.HasValue)
                Weight = weight.Value;
        }

        public void UpdateHealthInfo(
            string? code = null,
            int? age = null,
            string? injuries = null,
            string? chronicDiseases = null,
            double? musclePercentage = null,
            double? fatPercentage = null,
            string? notes = null)
        {
            if (code is not null) Code = code;
            if (age.HasValue) Age = age.Value;
            if (injuries is not null) Injuries = injuries;
            if (chronicDiseases is not null) ChronicDiseases = chronicDiseases;
            if (musclePercentage.HasValue) MusclePercentage = musclePercentage.Value;
            if (fatPercentage.HasValue) FatPercentage = fatPercentage.Value;
            if (notes is not null) Notes = notes;
        }

        public void UpdateApprovalStatus(ApprovalStatus status)
        {
            ApprovalStatus = status;
        }
    }
}

