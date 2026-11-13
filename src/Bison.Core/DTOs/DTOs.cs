
namespace Bison.Core.DTOs;

public record PostDTO(
    int Id,
    string Text,
    string PubDate,
    string UserId,
    string? UserName
);
public record ObservationDTO(
    int Id,
    string Text,
    string PubDate,
    string UserId,
    string? UserName
) : PostDTO(Id, Text, PubDate, UserId, UserName);


public record CommentDTO(
    int Id,
    string PubDate,
    string Text,
    string UserId,
    string? UserName
) : PostDTO(Id, Text, PubDate, UserId, UserName);

public record ProposalDTO(
    int Id,
    string Text,
    string PubDate,
    string UserId,
    string? UserName,
    string? TaxonId
) : PostDTO(Id, Text, PubDate, UserId, UserName);

public record AuthorDTO(
    string Id,
    string? UserName,
    string? Email
);

public record TaxonDTO(
    string TaxonId,
    string? TaxonName,
    string? ParentTaxonId
);