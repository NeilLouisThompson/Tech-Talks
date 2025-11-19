namespace PdfMergerApp.Shared.Models;

public enum MergeStatus
{
    Idle,
    Uploading,
    Uploaded,
    Merging,
    Completed,
    Failed
}

public class MergeProgress
{
    public MergeStatus Status { get; set; } = MergeStatus.Idle;
    public string Message { get; set; } = string.Empty;
    public int PercentComplete { get; set; }
    public string? ErrorDetails { get; set; }
}