using System.ComponentModel.DataAnnotations;

namespace PrimataArchive.Api.Models;

public record CriarGeneroRequest(
    [Required(ErrorMessage = "O nome do gênero é obrigatório.")]
    [StringLength(50)] string Nome
);

public record GeneroResponse(
    int IdGenero,
    string Nome
);