# MTM Inventory Application - Dependency Charts

**Generated**: 2025-08-12 03:11:05 UTC  
**Total Files Analyzed**: 75  
**Charts Generated**: 75  
**Compliance Issues**: 74 files need attention  

## 📊 Executive Summary
- **HIGH Priority Refactors**: 55 files (immediate attention required)
- **MEDIUM Priority**: 19 files (cleanup needed)
- **LOW Priority**: 1 file (minor issues)
- **Region Organization**: 98.7% of files need reorganization

## 🗂️ Quick Navigation

### By Layer
- **Entry Point**: [Program.cs](./Program.cs.md)
- **Forms**: 
  - [MainForm](./Forms/MainForm/MainForm.cs.md) (Main application window)
  - [SettingsForm](./Forms/Settings/SettingsForm.cs.md) (Configuration interface)
  - [SplashScreenForm](./Forms/Splash/SplashScreenForm.cs.md) (Startup screen)
  - [TransactionForm](./Forms/Transactions/Transactions.cs.md) (Transaction management)
- **Controls**: 
  - [MainForm Controls](./Controls/MainForm/) (Inventory, Transfer, Remove tabs)
  - [Settings Controls](./Controls/SettingsForm/) (Configuration components)
  - [Shared Controls](./Controls/Shared/) (Reusable UI components)
  - [Addons](./Controls/Addons/) (Additional features)
- **Data Access**: [DAOs](./Data/) (12 DAO files for database operations)
- **Services**: [Background Services](./Services/) (4 service files)
- **Helpers**: [Utilities](./Helpers/) (6 helper files)
- **Core**: [Core Components](./Core/) (4 core utility files)
- **Models**: [Data Models](./Models/) (11 model/DTO files)
- **Logging**: [LoggingUtility](./Logging/LoggingUtility.cs.md)

### By Priority (Refactor)
- **🔴 HIGH PRIORITY (55 files)**: 
  - All DAO files (Data/)
  - Main form controls (Controls/MainForm/)
  - Core forms (Forms/)
  - Critical services (Services/)
  
- **🟡 MEDIUM PRIORITY (19 files)**:
  - Settings form controls (Controls/SettingsForm/)
  - Helper utilities (Helpers/)
  - Some model files (Models/)
  
- **🟢 LOW PRIORITY (1 file)**:
  - Minor utility files

### By Compliance Status
- **❌ NON-COMPLIANT (55 files)**: Complete region reorganization required
- **⚠️ PARTIAL COMPLIANCE (19 files)**: Some regions present but incorrect ordering
- **✅ FULLY COMPLIANT (1 file)**: Meets all region organization standards

## 🔍 Dependency Overview
- **Internal Dependencies**: Analyzed across all 75 files
- **Stored Procedures**: Identified in DAO files (requires verification)
- **External Dependencies**: MySql.Data, ClosedXML, Microsoft.Web.WebView2, System.Windows.Forms

## 📖 Chart Usage Guide
Each `.md` file corresponds to a `.cs` file in the main codebase and includes:
- 🏗️ **File Overview**: Type, purpose, complexity assessment
- 🔗 **Dependencies**: What the file uses (upstream) and what uses it (downstream)
- 🗄️ **Stored Procedures**: Database operations (for applicable files)
- ✅ **Compliance Status**: Region organization, DAO patterns, error handling
- 🚀 **Refactor Plan**: Priority level, effort estimation, specific actions

## 📈 Analysis Results
- ✅ **Dependency Charts**: All 75 charts generated with detailed analysis
- ✅ **Compliance Assessment**: Comprehensive region organization analysis
- ✅ **Priority Classification**: Files categorized by refactor urgency
- ✅ **Refactor Planning**: Specific actions identified for each file
- ✅ **Risk Assessment**: Critical files and complexity levels identified

## 🚀 Next Steps
1. **Review [Analysis Report](./ANALYSIS_REPORT.md)** for complete findings
2. **Create feature branch**: `refactor/complete-codebase-with-charts/20250127`
3. **Begin with HIGH priority files** (55 files requiring immediate attention)
4. **Apply region reorganization** following standard patterns
5. **Update charts** to reflect post-refactor compliance status

---
*Complete dependency analysis and chart generation ready for implementation*