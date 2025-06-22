namespace Raftlabs.Library.Services;

public interface IWorkContext
{
    string GetUserEmail();

    void ValidateItems();
}