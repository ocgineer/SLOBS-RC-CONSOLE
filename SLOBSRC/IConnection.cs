using System.Threading.Tasks;

namespace SLOBSRC
{
    public interface IConnection
    {
        Task<string> MakeRequestAsync(string request);
        string MakeRequest(string request);
    }
}
