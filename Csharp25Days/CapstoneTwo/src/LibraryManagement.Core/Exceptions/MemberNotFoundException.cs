namespace LibraryManagement.Core.Exceptions;

public sealed class MemberNotFoundException : LibraryException
{
    public Guid MemberId { get; }

    public MemberNotFoundException(Guid memberId)
        : base($"Member with ID '{memberId}' was not found.")
    {
        MemberId = memberId;
    }
}
