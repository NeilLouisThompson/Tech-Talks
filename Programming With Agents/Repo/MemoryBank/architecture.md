# Architecture Decisions

- Framework: .NET 9 (or latest stable) using Blazor WebAssembly + ASP.NET Core hosted.
- Frontend: Blazor WebAssembly (C#).
- Backend: ASP.NET Core minimal API for PDF processing.
- Library for PDF merge: PdfSharp or iText7 (prefer officially supported .NET libraries).
- File handling: Use Blazor’s `InputFile` component to upload multiple PDFs.
- Merge flow: Frontend uploads files → Backend merges → returns combined file as stream.
- Design Principle: Keep frontend and backend loosely coupled with shared models.
- Scalability: Support async merge and progress reporting.
