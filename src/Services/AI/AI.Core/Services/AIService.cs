using System.Threading.Tasks;

namespace AI.Core.Services
{
    public interface IAIService
    {
        Task<string> AnalyzePicture(byte[] data);
    }

    public class AIService : IAIService
    {
        public async Task<string> AnalyzePicture(byte[] data)
        {
            return "112233";
        }
    }
}
