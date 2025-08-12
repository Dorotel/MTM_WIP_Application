• The README.md documents the use of Helper_Database_StoredProcedure for all database operations.
• It specifies:
    • No p_ prefix in C# code (added automatically).
    • Async/await usage.
    • Error handling via status codes and progress reporting.
    • Use of the Helper_Database_StoredProcedure class for all database operations.

Enhancement:
• Add a clear anti-pattern warning:
    • "Do not use direct MySqlCommand/MySqlDataAdapter for stored procedure calls."
    • "Do not manually add p_ prefix to parameters in C# code."
    • Add a code block showing a direct MySQL anti-pattern and why it’s wrong.
    • Ensure we are using LoggingUtility.cs as well as Service_ErrorHandler.cs for all error handling and logging.
• Always use the provided file I/O helper (Helper_FileIO.cs) for all file read/write operations.
• Always use the provided JSON parsing utility (Helper_Json.cs) for all JSON serialization/deserialization tasks. Do not use System.Text.Json directly in business logic.

Current Coverage:
• Theme application, DPI scaling, and progress reporting are documented.
• UserControl template is provided.

Enhancement:
• Add a checklist for every new UserControl:
    • Must call Core_Themes.ApplyDpiScaling(this) in constructor.
    • Must implement SetProgressControls.
    • Must use privilege management.
    • Must wire up keyboard shortcuts.

Logging and Error Handling
Current Coverage:
• Standard try/catch patterns and logging calls are shown.
• Async and sync error handling patterns are included.

Enhancement:
• Add explicit anti-pattern:
    • "Do not call error handlers recursively during startup."
    • "Always log database errors using LoggingUtility.LogDatabaseError(ex)."
    • Add a code block showing a recursive error handler anti-pattern.

Batch Editing Policy
Current Coverage:
• Batch editing policy is documented in section 24.

Enhancement:
• Add a checklist for contributors:
    • "Before editing, identify all issues in the file."
    • "Group all related changes in a single edit."
    • "Never submit multiple edits for the same file in one session."

Naming Conventions
Current Coverage:
• Naming conventions for controls, methods, fields, properties, constants, and events are listed.

Enhancement:
• Add a table summarizing naming conventions for quick reference.

Extension Methods and Utilities
Current Coverage:
• Common extension methods are listed.

Enhancement:
• Add a note: "Always use provided extension methods for safe data conversion and validation."
• Always use Helper_FileIO for file operations and Helper_Json for JSON parsing/serialization.

Anti-Patterns to Avoid
Current Coverage:
• Some anti-patterns are mentioned in troubleshooting and productivity sections.

Enhancement:
• Add a dedicated "Anti-Patterns to Avoid" section with code examples.
• "Do not use System.IO or System.Text.Json directly in business logic; use the provided helpers instead."

Questions for Clarification and Additional Standards
1. Are there any additional custom helpers or utility classes (besides those listed) that should always be used for specific tasks (e.g., file I/O, JSON parsing, etc.)?
    • Always use Helper_FileIO for file operations and Helper_Json for JSON parsing/serialization.
2. Is there a preferred pattern for asynchronous background services (e.g., timers, version checking) beyond what’s shown?
    • Use Service_Timer_VersionChecker and similar service classes. If alternatives are needed, document and justify them.
3. Should contributors always use the provided extension methods for all type conversions, or are there exceptions?
    • Always use provided extension methods for type conversions unless a specific scenario requires a different approach (document the reason).
4. Are there any specific code review or formatting tools (e.g., StyleCop, EditorConfig) enforced in the repo?
    • If not currently enforced, consider adding StyleCop or EditorConfig for consistency.
5. Is there a standard for comments/documentation within code (e.g., XML comments, summary tags)?
    • Place a definition at the beginning of each file before code, similar to .sql files. Explain the purpose of the file and any important details.
6. Should contributors avoid any specific .NET features (e.g., reflection, dynamic types, etc.) for maintainability or security?
    • Avoid reflection and dynamic types unless absolutely necessary. Prefer strongly-typed, compile-time checked code for maintainability and security. Document any exceptions with justification.
