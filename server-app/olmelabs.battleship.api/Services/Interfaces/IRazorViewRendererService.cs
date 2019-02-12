using System.Threading.Tasks;

namespace olmelabs.battleship.api.Services.Interfaces
{
    public interface IRazorViewRendererService
    {
        Task<string> RenderViewToStringAsync(string viewName, object model);
    }
}
