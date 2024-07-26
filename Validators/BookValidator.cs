using System.Text.Json.Serialization;


namespace BookApi.Validator{
    public class BookDto{
        public required string ISBN {get; set;}
    }
    public class GoogleBooksResponse
    {
        [JsonPropertyName("kind")]
        public required string Kind { get; set; }

        [JsonPropertyName("totalItems")]
        public int TotalItems { get; set; }

        [JsonPropertyName("items")]
        public required List<BookItem> Items { get; set; }
    }

    public class BookItem
    {
        [JsonPropertyName("kind")]
        public required string Kind { get; set; }

        [JsonPropertyName("id")]
        public required string Id { get; set; }

        [JsonPropertyName("etag")]
        public required string Etag { get; set; }

        [JsonPropertyName("selfLink")]
        public required string SelfLink { get; set; }

        [JsonPropertyName("volumeInfo")]
        public required VolumeInfo VolumeInfo { get; set; }

        [JsonPropertyName("saleInfo")]
        public required SaleInfo SaleInfo { get; set; }

        [JsonPropertyName("accessInfo")]
        public required AccessInfo AccessInfo { get; set; }

        [JsonPropertyName("searchInfo")]
        public required SearchInfo SearchInfo { get; set; }
    }

    public class VolumeInfo
    {
        [JsonPropertyName("title")]
        public required string Title { get; set; }

        [JsonPropertyName("authors")]
        public required List<string> Authors { get; set; }

        [JsonPropertyName("publisher")]
        public required string Publisher { get; set; }

        [JsonPropertyName("publishedDate")]
        public required string PublishedDate { get; set; }

        [JsonPropertyName("description")]
        public required string Description { get; set; }

        [JsonPropertyName("industryIdentifiers")]
        public required List<IndustryIdentifier> IndustryIdentifiers { get; set; }

        [JsonPropertyName("readingModes")]
        public required ReadingModes ReadingModes { get; set; }

        [JsonPropertyName("pageCount")]
        public int PageCount { get; set; }

        [JsonPropertyName("printType")]
        public required string PrintType { get; set; }

        [JsonPropertyName("categories")]
        public required List<string> Categories { get; set; }

        [JsonPropertyName("maturityRating")]
        public required string MaturityRating { get; set; }

        [JsonPropertyName("allowAnonLogging")]
        public bool AllowAnonLogging { get; set; }

        [JsonPropertyName("contentVersion")]
        public required string ContentVersion { get; set; }

        [JsonPropertyName("panelizationSummary")]
        public required PanelizationSummary PanelizationSummary { get; set; }

        [JsonPropertyName("imageLinks")]
        public required ImageLinks ImageLinks { get; set; }

        [JsonPropertyName("language")]
        public required string Language { get; set; }

        [JsonPropertyName("previewLink")]
        public required string PreviewLink { get; set; }

        [JsonPropertyName("infoLink")]
        public required string InfoLink { get; set; }

        [JsonPropertyName("canonicalVolumeLink")]
        public required string CanonicalVolumeLink { get; set; }
    }

    public class IndustryIdentifier
    {
        [JsonPropertyName("type")]
        public required string Type { get; set; }

        [JsonPropertyName("identifier")]
        public required string Identifier { get; set; }
    }

    public class ReadingModes
    {
        [JsonPropertyName("text")]
        public bool Text { get; set; }

        [JsonPropertyName("image")]
        public bool Image { get; set; }
    }

    public class PanelizationSummary
    {
        [JsonPropertyName("containsEpubBubbles")]
        public bool ContainsEpubBubbles { get; set; }

        [JsonPropertyName("containsImageBubbles")]
        public bool ContainsImageBubbles { get; set; }
    }

    public class ImageLinks
    {
        [JsonPropertyName("smallThumbnail")]
        public required string SmallThumbnail { get; set; }

        [JsonPropertyName("thumbnail")]
        public required string Thumbnail { get; set; }
    }

    public class SaleInfo
    {
        [JsonPropertyName("country")]
        public required string Country { get; set; }

        [JsonPropertyName("saleability")]
        public required string Saleability { get; set; }

        [JsonPropertyName("isEbook")]
        public bool IsEbook { get; set; }
    }

    public class AccessInfo
    {
        [JsonPropertyName("country")]
        public required string Country { get; set; }

        [JsonPropertyName("viewability")]
        public required string Viewability { get; set; }

        [JsonPropertyName("embeddable")]
        public bool Embeddable { get; set; }

        [JsonPropertyName("publicDomain")]
        public bool PublicDomain { get; set; }

        [JsonPropertyName("textToSpeechPermission")]
        public required string TextToSpeechPermission { get; set; }

        [JsonPropertyName("epub")]
        public required Epub Epub { get; set; }

        [JsonPropertyName("pdf")]
        public required Pdf Pdf { get; set; }

        [JsonPropertyName("webReaderLink")]
        public required string WebReaderLink { get; set; }

        [JsonPropertyName("accessViewStatus")]
        public required string AccessViewStatus { get; set; }

        [JsonPropertyName("quoteSharingAllowed")]
        public bool QuoteSharingAllowed { get; set; }
    }

    public class Epub
    {
        [JsonPropertyName("isAvailable")]
        public bool IsAvailable { get; set; }
    }

    public class Pdf
    {
        [JsonPropertyName("isAvailable")]
        public bool IsAvailable { get; set; }
    }

    public class SearchInfo
    {
        [JsonPropertyName("textSnippet")]
        public required string TextSnippet { get; set; }
    }

}