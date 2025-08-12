# 21. Refactoring Workflow & Recursive Dependency Compliance Analysis

When requesting a refactor of a SINGLE file, the process MUST begin with a comprehensive, branching dependency analysis and a formal pre-refactor report. No code changes occur until the report is generated, reviewed, and approved. This ensures every upstream and downstream method adheres to the standards in this reference.

## 21.1 Code Organization Requirements (MANDATORY)

**ALL refactored files MUST follow the established region organization pattern:**

### **Standard Region Order:**
1. **`#region Fields`** - Private fields, static instances, progress helpers
2. **`#region Properties`** - Public properties, getters/setters  
3. **`#region Progress Control Methods`** - SetProgressControls and progress-related methods
4. **`#region Constructors`** - Constructor and initialization
   - **`#region Initialization`** - Sub-region for complex initialization logic
5. **`#region [Specific Functionality]`** - Business logic regions (e.g., "Database Connectivity", "UI Events")
6. **`#region Key Processing`** - ProcessCmdKey and keyboard shortcuts
7. **`#region Button Clicks`** - Event handlers for button clicks
8. **`#region ComboBox & UI Events`** - UI event handlers and validation
9. **`#region Helpers`** or **`#region Private Methods`** - Helper and utility methods
10. **`#region Cleanup`** or **`#region Disposal`** - Cleanup and disposal methods

### **Method Ordering Within Regions:**
- **Public methods** first
- **Protected methods** second  
- **Private methods** third
- **Static methods** at the end of each access level
- **Async methods** grouped together when possible

## 21.2 Scope of Dependency Expansion
1) Direct internal dependencies (what the file calls)  
2) Direct dependents (what calls the file)  
3) Expand recursively until no new symbols or a safety threshold (default 500 methods / 50 files)

Include: UI controls, DAOs and SPs, Services, Helpers, Core utilities, Extensions, and SP mapping (DAO → SP).  
Exclude by default: third-party packages, BCL, generated designer files.

## 21.3 Classification
Each discovered symbol is tagged by Layer, Direction (upstream/downstream), and Stability (Stable | Minor Cleanup | Full Refactor | Critical Risk).

## 21.4 Compliance Checklist
- **Region Organization**: Methods grouped in proper #regions following standard order
- **Method Ordering**: Public → Protected → Private → Static within regions
- Naming conventions
- DAO pattern (DaoResult<T>, helper-based SP calls, no inline SQL, C# params without p_ prefix)
- Null safety, error handling, progress reporting
- Theme & privilege policies
- Data binding safety (DGV/ComboBox)
- Stored procedure contract (OUT p_Status, OUT p_ErrorMsg)
- Logging standards
- Quick Button logic (where applicable)
- Performance and undo/reversibility where applicable

## 21.5 Pre-Refactor Report (structure)
```
REFRACTOR_PREVIEW_REPORT
TargetFile: <relative/path/TargetFile.cs>
GeneratedAtUtc: <iso-utc>
TraversalLimits: { MaxFiles: 500, MaxMethods: 500, ReachedLimit: false }
Summary: { TotalFilesInScope: N, TotalMethodsInScope: N, NonCompliantMethods: N, HighRiskItems: N, StoredProcedures: N }
DependencyGraph: Levels 0..N with upstream/downstream edges
ComplianceMatrix: Per-symbol PASS/PARTIAL/FAIL notes (INCLUDING REGION ORGANIZATION)
StoredProceduresReferenced: list with OUT param verification
PlannedRefactorActions: ordered steps (INCLUDING REGION REORGANIZATION)
Risks: categorized
RegressionTestPlan: scenarios
RollbackPlan: strategy
RegionOrganizationPlan: Current vs Target region structure
END_REPORT
```

## 21.6 Execution Sequence
1) Generate report (no changes)  
2) Await approval/scope refinement  
3) Create feature branch: refactor/<file-name>/<yyyyMMdd>  
4) Apply atomic commits by category:
   - **Region reorganization and method ordering**
   - DAO params, logging/error handling
   - UI progress, SP alignment/tests
5) Update docs only if behavior changes  
6) Optional patch entry  
7) Post-change compliance summary  
8) Offer rollback diff

## 21.7 Stored Procedure Integrity Checks
- SP exists in Database/StoredProcedures  
- Includes OUT p_Status, OUT p_ErrorMsg  
- No inline SQL added  
- Parameters passed without p_ prefix in C#  
- DaoResult<T> ensures non-null data

## 21.8 Risk Categorization and Requests
- Critical / Medium / Low with mitigations  
Prompts:
- "Analyze dependencies for refactoring file: <file>. Do not refactor yet."
- "Refactor file: <file>. Begin with full recursive dependency compliance report, then await my approval."
- "Exclude these files from the refactor scope: <list>. Regenerate the report."

## 21.9 Assistant Behavior Rules
- MUST produce report first, MUST recurse dependencies, MUST map to this guide, MUST NOT edit until approved, MUST list assumptions.
- **MUST include region organization analysis in compliance matrix**
- **MUST plan region reorganization as part of refactor actions**

## 21.10 Optional JSON Output
```json
{
  "refactorReportVersion": 2,
  "targetFile": "Data/Inventory/Dao_Inventory.cs",
  "generatedAtUtc": "2025-01-27T14:30:00Z",
  "summary": { "files": 18, "methods": 137, "nonCompliant": 26, "highRisk": 4 },
  "dependencyGraph": { "nodes": [], "edges": [] },
  "complianceFindings": [
    {"category": "RegionOrganization", "status": "FAIL", "reason": "Methods not properly grouped in regions"},
    {"category": "MethodOrdering", "status": "PARTIAL", "reason": "Some regions have incorrect method ordering"}
  ],
  "plannedActions": [
    "Reorganize methods into proper #regions following standard order",
    "Order methods within regions: Public → Protected → Private → Static"
  ],
  "risks": [],
  "regressionTests": [],
  "rollbackPlan": "Retain original; revert branch if tests fail",
  "regionOrganization": {
    "currentRegions": ["Fields", "Methods", "EventHandlers"],
    "targetRegions": ["Fields", "Properties", "Constructors", "Database Operations", "Button Clicks", "Helpers"]
  }
}
```

## 21.11 Online Refactor Mode (GitHub UI) — Detailed Prompt Template
Use this when you want to perform the refactor entirely online (not in a local editor). Copy, fill, and paste this prompt into a GitHub issue or PR description to instruct the assistant.

# MASTER REFRACTOR PROMPT (Online Mode)

Refactor target file online with full dependency compliance and proper code organization.

Meta
- GeneratedFor: Dorotel
- GeneratedAtUtc: 2025-01-27 14:30:00
- RepoDocs: Documentation/Copilot Files/21-refactoring-workflow.md

Target
- File to refactor: <relative/path/FileName.cs>   <-- REPLACE THIS ONLY
- Base branch: main
- Desired feature branch name: refactor/<auto-from-file-stem>/20250127

Scope rules
- Perform recursive dependency analysis:
  - From the target file → methods it calls (upstream) and methods that call it (downstream)
  - Recurse until closure or the threshold: MaxFiles=500, MaxMethods=500
- Include: UI Controls, DAOs, Helpers, Core, Services, Models/DTOs, Extensions, Stored Procedures (map DAO → SP)
- Exclude by default: third-party packages, BCL, generated *.Designer.cs (unless violating policy)

Standards to enforce (see Documentation/Copilot Files/21-refactoring-workflow.md)
- **Region Organization**: Methods grouped in proper #regions following standard order
- **Method Ordering**: Public → Protected → Private → Static within each region
- Naming conventions for UI/DAO/constants
- DAO pattern:
  - Return types use DaoResult<T>
  - Use Helper_Database_StoredProcedure exclusively (no inline SQL)
  - Pass parameters in C# without p_ prefix (helper adds prefix)
- Null safety: never dereference potentially null DataTable/DataRow/Object
- Error handling: try/catch, LoggingUtility.Log*, user-friendly messages, early return on failure
- Progress reporting: UI async DB ops use Helper_StoredProcedureProgress
- Theme usage: only in constructors, settings-theme-change handlers, and DPI events
- Privilege handling: ApplyPrivileges() in UI
- Data binding safety: DataGridView and ComboBox standard patterns
- Stored procedure contract: OUT p_Status, OUT p_ErrorMsg present and handled
- Logging standards: context-rich, start/end markers where appropriate, avoid noisy logs
- Quick Button logic: uniqueness rules and graceful failures (where relevant)
- Performance: async for I/O, avoid redundant calls, use caching if indicated
- Undo/reversibility: ensure mutating ops have a documented or implemented reversal path when required

Before any code changes — produce and post a Pre-Refactor Report
- Title: REFRACTOR_PREVIEW_REPORT
- Contents:
  - TargetFile, GeneratedAtUtc (ISO UTC), TraversalLimits (MaxFiles, MaxMethods, ReachedLimit)
  - Summary counts: TotalFilesInScope, TotalMethodsInScope, NonCompliantMethods, HighRiskItems, StoredProcedures
  - DependencyGraph: Level 0..N with upstream/downstream edges
  - ComplianceMatrix: Per symbol PASS/PARTIAL/FAIL with reasons (mapped to checklist above) **INCLUDING REGION ORGANIZATION**
  - StoredProceduresReferenced: names, locations, verification that OUT params exist
  - PlannedRefactorActions: ordered and atomic **INCLUDING REGION REORGANIZATION**
  - RegionOrganizationPlan: Current region structure vs target structure
  - Risks: categorized (Critical/Medium/Low) with mitigations
  - RegressionTestPlan: concrete scenarios
  - RollbackPlan: strategy (feature branch isolation, revert plan)
- Output format: Provide both Markdown and a JSON block (refactorReportVersion: 2)
- IMPORTANT: Pause and WAIT for explicit "Proceed" approval.

Implementation plan (after approval)
- Create feature branch: refactor/<auto-from-file-stem>/20250127
- Apply atomic commits:
  1) **Region reorganization and method ordering within regions**
  2) DAO signatures/parameter normalization; remove p_ prefix usage in C#
  3) Logging and error handling harmonization
  4) UI progress integration and null safety for dependent call sites
  5) Stored procedure call alignment and verification
- Do not change behavior beyond compliance unless explicitly requested
- Update docs only if behavior meaningfully changes; reference Section 21 if updated

Deliverables
- Pre-Refactor Report (Markdown + JSON) posted before any change
- Pull Request with:
  - Description referencing "Section 21: Refactoring Workflow"
  - Diff summary: Added/Removed/Renamed methods; **region reorganization changes**; any behavior changes
  - Compliance delta: e.g., NonCompliantMethods: X → 0, RegionOrganization: FAIL → PASS
  - Evidence: test notes, screenshots (optional), logs of success and error paths
- Rollback instructions (how to revert the feature branch)

Assumptions/Inputs
- Application: WinForms, .NET 8, MySQL 5.7.24+ (stored procedures only)
- Version context: 5.0.1.2
- If any stored procedure is missing required OUT params, propose the SQL changes but do not apply without approval

Constraints
- No inline SQL — must use Helper_Database_StoredProcedure
- No theme calls outside allowed locations
- No breaking public API unless explicitly stated and approved
- **MUST organize methods in proper regions with correct ordering**

Optional exclusions
- Exclude files/namespaces/globs: Controls/**/*.Designer.cs, **/obj/**, **/bin/**

Acceptance criteria
- All touched methods pass the compliance checklist
- **Methods are properly organized in regions with correct ordering**
- Build succeeds; smoke tests for impacted flows pass (as per RegressionTestPlan)
- DaoResult<T> usage ensures non-null data payloads (empty table/collection allowed)

Please acknowledge by posting the Pre-Refactor Report first, then pause for approval.

## 21.12 What to Attach to the Online Prompt
- Exact target file path.
- Any exclusions (files/namespaces).
- Screenshots or links that show the affected UI flow (optional).
- If changing stored procedures, include their names and current definitions or paths.

## 21.13 Example (Filled)
```
File: Data/Inventory/Dao_Inventory.cs
Base: main
Branch: refactor/dao-inventory/20250127
Exclusions: Controls/Legacy/*
Notes: Maintain current behavior; this is compliance-only with region organization.
```
Paste the "MASTER REFRACTOR PROMPT (Online Mode)" above with these values substituted.

## 21.14 Region Organization Examples

### **Before (Non-Compliant):**
```csharp
public class ExampleClass 
{
    public void PublicMethod() { }
    private string _field;
    private void PrivateMethod() { }
    public string Property { get; set; }
    protected void ProtectedMethod() { }
    public ExampleClass() { }
}
```

### **After (Compliant):**
```csharp
public class ExampleClass 
{
    #region Fields
    
    private string _field;
    
    #endregion
    
    #region Properties
    
    public string Property { get; set; }
    
    #endregion
    
    #region Constructors
    
    public ExampleClass() 
    {
        // Constructor implementation
    }
    
    #endregion
    
    #region Public Methods
    
    public void PublicMethod() 
    {
        // Public method implementation
    }
    
    #endregion
    
    #region Protected Methods
    
    protected void ProtectedMethod() 
    {
        // Protected method implementation
    }
    
    #endregion
    
    #region Private Methods
    
    private void PrivateMethod() 
    {
        // Private method implementation
    }
    
    #endregion
}
```

**Remember: Region organization is MANDATORY for all refactors. Non-compliance will require rework.**
