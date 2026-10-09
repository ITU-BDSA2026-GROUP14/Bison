using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

public record PostDTO(
    int Id,
    string Text,
    string PubDate,
    int AuthorId
);
public record ObservationDTO(
    int Id,
    string Text,
    string PubDate,
    int AuthorId
) : PostDTO(Id, Text, PubDate, AuthorId);


public record CommentDTO(
    int Id,
    string PubDate,
    string Text,
    int AuthorId
) : PostDTO(Id, Text, PubDate, AuthorId);

public record ProposalDTO(
    int Id,
    string Text,
    string PubDate,
    int AuthorId,
    string? TaxonId
) : PostDTO(Id, Text, PubDate, AuthorId);

public record AuthorDTO(
    int Id,
    string Name,
    string Email
);

public record TaxonDTO(
    string TaxonId,
    string? TaxonName,
    string? ParentTaxonId
);