namespace WoodenAutomative.Domain.Models
{
    public enum LoginStatus
    {
        Succeeded,
        Failed,
        SetNewPassword,
        UpdatePassword,
        EmailVerification,
        MobileVerification
    }
}
