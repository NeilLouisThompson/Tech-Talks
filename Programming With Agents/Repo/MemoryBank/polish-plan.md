# Polish Phase Tasks - Plan

## T015: Cleanup Components

**Goal**: Remove unused components from the default Blazor template that are not related to PDF merging functionality.

**Components to Remove**:

- `Counter.razor` page
- `FetchData.razor` page
- `WeatherForecast.cs` model (in Shared project)
- `WeatherForecastController.cs` (if exists in Server)
- `SurveyPrompt.razor` component (if used)

**Navigation Updates**:

- Remove Counter and Fetch Data links from `NavMenu.razor`
- Keep only Home and PDF Merger links

**Benefits**:

- Cleaner, focused application
- Smaller bundle size
- No confusing navigation options
- Professional appearance

## T016: Green Theme Styling

**Goal**: Apply a cohesive green color scheme throughout the application.

**Color Palette**:

- Primary Green: `#28a745` (Bootstrap success green)
- Light Green: `#d4edda` (for backgrounds)
- Dark Green: `#155724` (for text/borders)
- Accent Green: `#20c997` (teal-green for highlights)

**Elements to Style**:

- Primary buttons → Green background
- Progress bars → Green completion color
- Success alerts → Enhanced green styling
- Timeline markers → Green for completed steps
- Navigation brand → Green accent
- Card headers → Light green backgrounds
- Focus states → Green borders

**Implementation Approach**:

- Create custom CSS variables for green theme
- Override Bootstrap color utilities
- Update component classes to use green variants
- Maintain accessibility with proper contrast ratios

**Files to Modify**:

- `wwwroot/css/app.css` → Add green theme variables
- `Upload.razor` → Update button and progress classes
- `PdfMergeProgress.razor` → Green timeline and badges
- `NavMenu.razor` → Green brand styling
- `Index.razor` → Green call-to-action buttons

This polish phase will result in a clean, professional PDF merger application with a distinctive green brand identity.
