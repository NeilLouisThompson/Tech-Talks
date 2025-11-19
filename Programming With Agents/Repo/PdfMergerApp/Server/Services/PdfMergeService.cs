using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfMergerApp.Shared.Models;

namespace PdfMergerApp.Server.Services;

public interface IPdfMergeService
{
    Task<MergeResponse> MergePdfsAsync(MergeRequest request, CancellationToken cancellationToken = default);
    Task<bool> ValidatePdfAsync(byte[] fileData);
}

public class PdfMergeService : IPdfMergeService
{
    private readonly ILogger<PdfMergeService> _logger;

    public PdfMergeService(ILogger<PdfMergeService> logger)
    {
        _logger = logger;
    }

    public async Task<MergeResponse> MergePdfsAsync(MergeRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Starting PDF merge for {FileCount} files", request.Files.Count);

            if (!request.Files.Any())
            {
                return new MergeResponse
                {
                    Success = false,
                    ErrorMessage = "No files provided for merging"
                };
            }

            // Validate all files are PDFs
            foreach (var file in request.Files)
            {
                if (!await ValidatePdfAsync(file.FileData))
                {
                    return new MergeResponse
                    {
                        Success = false,
                        ErrorMessage = $"File '{file.FileName}' is not a valid PDF"
                    };
                }
            }

            // Create output document
            using var outputDocument = new PdfDocument();

            // Process files in order
            var orderedFiles = request.Files.OrderBy(f => f.Order).ToList();
            
            foreach (var file in orderedFiles)
            {
                _logger.LogDebug("Processing file: {FileName}", file.FileName);
                
                using var inputStream = new MemoryStream(file.FileData);
                using var inputDocument = PdfReader.Open(inputStream, PdfDocumentOpenMode.Import);

                // Copy all pages from input to output
                foreach (PdfPage page in inputDocument.Pages)
                {
                    outputDocument.AddPage(page);
                }
            }

            // Save to memory stream
            using var outputStream = new MemoryStream();
            outputDocument.Save(outputStream, false);
            
            _logger.LogInformation("PDF merge completed successfully. Output size: {Size} bytes", outputStream.Length);

            return new MergeResponse
            {
                Success = true,
                MergedFileData = outputStream.ToArray(),
                FileName = request.MergedFileName,
                ContentType = "application/pdf"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error merging PDFs");
            return new MergeResponse
            {
                Success = false,
                ErrorMessage = $"PDF merge failed: {ex.Message}"
            };
        }
    }

    public Task<bool> ValidatePdfAsync(byte[] fileData)
    {
        try
        {
            using var stream = new MemoryStream(fileData);
            using var document = PdfReader.Open(stream, PdfDocumentOpenMode.Import);
            return Task.FromResult(document.PageCount > 0);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }
}