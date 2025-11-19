namespace PdfMergerApp.Shared.Models;

public class FileUploadInfo
{
    public string FileName { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public int Order { get; set; }
    public byte[] FileData { get; set; } = Array.Empty<byte>();
}