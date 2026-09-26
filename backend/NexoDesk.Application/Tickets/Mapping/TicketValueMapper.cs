using NexoDesk.Domain.Enums;

namespace NexoDesk.Application.Tickets.Mapping;

public static class TicketValueMapper
{
    public static string ToApiValue(TicketStatus status) => status switch
    {
        TicketStatus.Aberto => "ABERTO",
        TicketStatus.EmProgresso => "EM_PROGRESSO",
        TicketStatus.Resolvido => "RESOLVIDO",
        TicketStatus.Fechado => "FECHADO",
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
    };

    public static string ToApiValue(TicketPriority priority) => priority switch
    {
        TicketPriority.Baixa => "BAIXA",
        TicketPriority.Media => "MÉDIA",
        TicketPriority.Alta => "ALTA",
        TicketPriority.Critica => "CRÍTICA",
        _ => throw new ArgumentOutOfRangeException(nameof(priority), priority, null)
    };

    public static bool TryParseStatus(string? value, out TicketStatus status)
    {
        switch (value?.Trim().ToUpperInvariant())
        {
            case "ABERTO":
                status = TicketStatus.Aberto;
                return true;
            case "EM_PROGRESSO":
                status = TicketStatus.EmProgresso;
                return true;
            case "RESOLVIDO":
                status = TicketStatus.Resolvido;
                return true;
            case "FECHADO":
                status = TicketStatus.Fechado;
                return true;
            default:
                status = default;
                return false;
        }
    }

    public static bool TryParsePriority(string? value, out TicketPriority priority)
    {
        switch (value?.Trim().ToUpperInvariant())
        {
            case "BAIXA":
                priority = TicketPriority.Baixa;
                return true;
            case "MÉDIA":
                priority = TicketPriority.Media;
                return true;
            case "ALTA":
                priority = TicketPriority.Alta;
                return true;
            case "CRÍTICA":
                priority = TicketPriority.Critica;
                return true;
            default:
                priority = default;
                return false;
        }
    }
}
