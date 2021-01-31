using Contracts.Events;
using MassTransit;
using Payment.Core.Services;
using System.Threading.Tasks;

namespace Payment.Core.Consumers
{
    public class RideRegisteredConsumer : IConsumer<RideRegistered>
    {
        private readonly IPaymentService _paymentService;

        public RideRegisteredConsumer(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task Consume(ConsumeContext<RideRegistered> context)
        {
                await _paymentService.CreatePaymentAsync(context.Message);
        }
    }
}
