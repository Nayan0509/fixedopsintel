namespace WoodenAutomative.EntityFramework.Interfaces.Services
{
    public interface IUnitOfWork
    {
        ILoginService Login { get; }
        IUserService User { get; }
        ICurrentUserAccessor CurrentUserAccessor { get; }
        IEmailRepository Email { get; }
        IAuthorizationRepository Authorization { get; }

        void Save();
    }
}
