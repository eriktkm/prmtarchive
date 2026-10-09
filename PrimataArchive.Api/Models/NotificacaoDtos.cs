namespace PrimataArchive.Api.Models;

public record NotificacaoResponse(
    int IdNotificacao,
    int IdUsuario,
    string Mensagem,
    bool Lida,
    DateTime? DataCriacao
);