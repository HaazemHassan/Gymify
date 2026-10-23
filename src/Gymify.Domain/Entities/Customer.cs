using ErrorOr;
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
            ApprovalStatus approvalStatus = ApprovalStatus.Pending,
            int? age = default,
            string? code = default,
            string? injuries = default,
            string? chronicDiseases = default,
            double? musclePercentage = default,
            double? fatPercentage = default,
            string? notes = default)
        {
            DomainUserId = domainUserId;
            Height = height;
            Weight = weight;
            ApprovalStatus = approvalStatus;
            Age = age;
            Code = code;
            Injuries = injuries;
            ChronicDiseases = chronicDiseases;
            MusclePercentage = musclePercentage;
            FatPercentage = fatPercentage;
            Notes = notes;
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
        public ICollection<CustomerBehaviorNote> CustomerBehaviorNotes { get; private set; } = new List<CustomerBehaviorNote>();

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

        public ErrorOr<Success> UpdateApprovalStatus(ApprovalStatus newStatus)
        {

            ApprovalStatus = newStatus;
            return Result.Success;
        }

        public void UpdateDetails(
            double? height = null,
            double? weight = null,
            string? code = null,
            int? age = null,
            string? injuries = null,
            string? chronicDiseases = null,
            double? musclePercentage = null,
            double? fatPercentage = null,
            string? notes = null,
            ApprovalStatus? approvalStatus = null)
        {
            if (height.HasValue) Height = height.Value;
            if (weight.HasValue) Weight = weight.Value;
            if (code is not null) Code = code;
            if (age.HasValue) Age = age.Value;
            if (injuries is not null) Injuries = injuries;
            if (chronicDiseases is not null) ChronicDiseases = chronicDiseases;
            if (musclePercentage.HasValue) MusclePercentage = musclePercentage.Value;
            if (fatPercentage.HasValue) FatPercentage = fatPercentage.Value;
            if (notes is not null) Notes = notes;
            if (approvalStatus.HasValue) ApprovalStatus = approvalStatus.Value;
        }
    }
}

