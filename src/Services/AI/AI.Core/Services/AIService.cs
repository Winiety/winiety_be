using System.Net.Http;
using System.Threading.Tasks;

namespace AI.Core.Services
{
    public interface IAIService
    {
        Task<string> AnalyzePicture(string imagePath);
    }

    public class AIService : IAIService
    {
        private readonly string _predictService;

        public AIService(string predictService)
        {
            _predictService = predictService;
        }

        public async Task<string> AnalyzePicture(string imagePath)
        {
            string text = string.Empty;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync($"{_predictService}?image={imagePath}");
                    text = await response.Content.ReadAsStringAsync();
                }

                return text;
            }
            catch (System.Exception)
            {

                return text;
            }
        }
    }
}
