using MassTransit;
using PaymentsAPI.Domain.Interfaces.Services;
using Shared.Contracts.Events;

namespace PaymentsAPI.Api.Consumers
{
    public class OrderPlacedConsumer : IConsumer<OrderPlacedEvent>
    {
        private readonly IPagamentoService _service;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<OrderPlacedConsumer> _logger;

        public OrderPlacedConsumer(IPagamentoService service, IPublishEndpoint publishEndpoint, ILogger<OrderPlacedConsumer> logger)
        {
            _service = service;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
        {
            var ev = context.Message;

            _logger.LogInformation(
                "OrderPlacedEvent recebido: OrderId={OrderId} | JogoId={JogoId} | UserId={UserId} | Preco={Preco}",
                ev.OrderId, ev.JogoId, ev.UserId, ev.Preco);

            var pagamento = await _service.ProcessarAsync(ev.OrderId, ev.UserId, ev.JogoId, ev.Titulo, ev.Preco);

            await _publishEndpoint.Publish(new PaymentProcessedEvent
            {
                OrderId = pagamento.OrderId,
                UserId = pagamento.UserId,
                JogoId = pagamento.JogoId,
                Titulo = pagamento.Titulo,
                Preco = pagamento.Preco,
                Status = pagamento.Status,
                ProcessadoEm = pagamento.ProcessadoEm
            });

            _logger.LogInformation(
                "PaymentProcessedEvent publicado: OrderId={OrderId} | Status={Status}",
                pagamento.OrderId, pagamento.Status);
        }
    }
}
