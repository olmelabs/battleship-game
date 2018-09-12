using System.Threading.Tasks;

namespace olmelabs.battleship.api.Services
{
    public interface IRazorViewRendererService
    {
        Task<string> RenderViewToStringAsync(string viewName, object model);
    }
}
