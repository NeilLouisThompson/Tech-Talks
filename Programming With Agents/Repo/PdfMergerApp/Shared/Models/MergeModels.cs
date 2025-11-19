namespace PdfMergerApp.Shared.Models;

public class MergeRequest
{
    public List<FileUploadInfo> Files { get; set; } = new();
    public string MergedFileName { get; set; } = "merged-document.pdf";
}

public class MergeResponse
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public byte[]? MergedFileData { get; set; }
    public string? FileName { get; set; }
    public string? ContentType { get; set; } = "application/pdf";
}