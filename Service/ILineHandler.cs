using Domain;
using System.Threading.Tasks;

namespace Service
{
    public interface ILineHandler
    {
        Task ProcessMessage(WebhookModel value);
    }
}