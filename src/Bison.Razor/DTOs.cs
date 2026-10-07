using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

public record PostDTO(
    int Id,
    string Text,
    string PubDate,
    int AuthorId,
    string AuthorName
);
public record ObservationDTO(
    int Id,
    string Text,
    string PubDate,
    int AuthorId,
    string AuthorName,
    string? TaxonName
) : PostDTO(Id, Text, PubDate, AuthorId, AuthorName);


public record CommentDTO(
    int Id,
    string PubDate,
    string Text,
    int AuthorId,
    string AuthorName
) : PostDTO(Id, Text, PubDate, AuthorId, AuthorName);

public record ProposalsDTO(
    int Id,
    string Text,
    string PubDate,
    int AuthorId,
    string AuthorName,
    string? TaxonName
) : PostDTO(Id, Text, PubDate, AuthorId, AuthorName);