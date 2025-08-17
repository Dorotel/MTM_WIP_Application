# Business Logic Migration Guide

## Overview
This section contains all the core business logic that needs to be adapted for MAUI architecture.

## Services (1-Business-Logic/Services)
All service classes that need dependency injection setup in MAUI:

### Key Services:
- **Service_ApplicationAnalyzer** - Application analysis functionality
- **Service_ErrorHandler** - Centralized error handling (adapt to MAUI dialogs)
- **Service_ConnectionRecoveryManager** - Database connection management
- **Service_DebugTracer** - Logging and debugging infrastructure
- **Service_MAUIMigrationAssessment** - Migration assessment tools

### Migration Notes:
- Register all services in MauiProgram.cs using dependency injection
- Convert WinForms-specific UI interactions to MAUI patterns
- Adapt file system operations for cross-platform compatibility

## Data Access (1-Business-Logic/Data-Access)
All DAO classes maintaining MySQL database operations:

### Key DAOs:
- **Dao_Inventory** - Core inventory operations
- **Dao_User** - User management and settings
- **Dao_Transactions** - Transaction history and search
- **Dao_ErrorLog** - Error logging to database

### Migration Notes:
- Keep all stored procedure calls exactly as implemented
- Maintain connection string management patterns
- Adapt async patterns for MAUI UI thread management

## Helpers (1-Business-Logic/Helpers)
Utility classes that need cross-platform adaptation:

### Key Helpers:
- **Helper_Database_StoredProcedure** - Database operation wrapper
- **Helper_Database_Variables** - Connection string management
- **Helper_StoredProcedureProgress** - Progress reporting (adapt to MAUI)
- **Helper_UI_ComboBoxes** - UI data binding (convert to MAUI binding)

### Migration Notes:
- Convert WinForms-specific UI helpers to MAUI equivalents
- Maintain database helper functionality exactly as implemented
- Adapt progress reporting to MAUI progress indicators

## MAUI Implementation Strategy

1. **Dependency Injection Setup**:
   ```csharp
   // In MauiProgram.cs
   builder.Services.AddSingleton<Service_ErrorHandler>();
   builder.Services.AddSingleton<Service_ConnectionRecoveryManager>();
   builder.Services.AddTransient<Dao_Inventory>();
   // ... register all services and DAOs
   ```

2. **Database Operations**:
   - Keep all existing stored procedure calls
   - Maintain Helper_Database_* classes
   - Use same connection string management

3. **Error Handling**:
   - Convert Service_ErrorHandler to use MAUI dialogs
   - Maintain centralized error logging
   - Adapt progress reporting to MAUI UI controls

4. **Cross-Platform Considerations**:
   - File path operations for different platforms
   - Platform-specific database drivers if needed
   - UI thread marshaling for database operations