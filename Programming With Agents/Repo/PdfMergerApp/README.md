# PDF Merger Blazor App

A .NET 9 Blazor WebAssembly application for merging multiple PDF files into a single document.

## ✨ Features

- 📁 Upload multiple PDF files (up to 10 files, 50MB each)
- 🔄 Drag-and-drop file reordering
- ⚡ Real-time progress tracking with visual timeline
- 📊 File validation and error handling
- 💾 Instant download of merged PDFs
- 🎨 Responsive design with Bootstrap styling
- 🔒 Secure client-server processing

## 🏗️ Architecture

- **Frontend**: Blazor WebAssembly (C#) with Bootstrap UI
- **Backend**: ASP.NET Core Minimal API (.NET 9)
- **PDF Processing**: PdfSharp 6.2.2
- **Components**: Modular Blazor components for upload and progress

## 📁 Project Structure

```
/Client/                    - Blazor WebAssembly frontend
  /Pages/Upload.razor       - Main PDF upload and merge page
  /Components/              - Reusable UI components
  /Shared/                  - Navigation and layout
/Server/                    - ASP.NET Core backend API
  /Services/                - PDF merge service logic
  Program.cs               - API endpoints and DI setup
/Shared/                    - Shared models and DTOs
  /Models/                  - Data transfer objects
/Tests/                     - Unit test project
```

## 🚀 Getting Started

### Prerequisites

- .NET 9 SDK
- Modern web browser

### Running the Application

```bash
# Clone and navigate to the project
cd PdfMergerApp

# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run the application
dotnet run --project Server
```

Navigate to `https://localhost:5170` to use the application.

## 📖 Usage

1. **Navigate** to the PDF Merger page
2. **Upload** multiple PDF files using the file picker
3. **Reorder** files using the up/down arrows if needed
4. **Enter** a name for your merged file (optional)
5. **Click** "Merge PDFs" to start the process
6. **Watch** the real-time progress indicator
7. **Download** automatically starts when merge completes

## 🔧 API Endpoints

- `POST /api/pdf/merge` - Merge uploaded PDF files
- `POST /api/pdf/validate` - Validate PDF file format

## 🧪 Testing

```bash
# Run unit tests
dotnet test

# Run specific test project
dotnet test PdfMergerApp.Tests
```

## 🏷️ Development Status

This project follows a structured development approach with tracked progress:

- ✅ All core functionality implemented
- ✅ Frontend and backend integration complete
- ✅ Progress tracking and error handling
- ✅ Responsive UI with modern styling

Check `/MemoryBank/tasks.md` for detailed development progress tracking.

## 🛠️ Technology Stack

- **.NET 9** - Latest framework version
- **Blazor WebAssembly** - Client-side C# web apps
- **ASP.NET Core** - Web API backend
- **PdfSharp** - PDF manipulation library
- **Bootstrap 5** - UI framework
- **Font Awesome** - Icon library

## 📝 License

This project is part of a programming demonstration for agent-driven development workflows.
