namespace MyStore.Application.Roles.Queries.GetItem
{
    using FluentValidation;

    public class RoleGetItemQueryValidator : AbstractValidator<RoleGetItemQuery>
    {
        public RoleGetItemQueryValidator()
        {
            RuleFor(v => v.Id).NotEmpty();
        }
    }
}
