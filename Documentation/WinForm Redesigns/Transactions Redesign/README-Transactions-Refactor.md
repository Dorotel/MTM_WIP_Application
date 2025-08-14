# Transactions UI Refactor (Web)

## Objective
Replace the legacy WinForms Transactions form with a modern, accessible, testable web UI that preserves functional behavior without inheriting brittle layout, control IDs, or hidden coupling.

## Core Features Parity
- Filters: Sort By (Date/Quantity/User/ItemType), Part ID, User (disable for non-admin), Building, optional Date Range (off by default), Quick/LIKE search (column + substring).
- Search button only enabled when at least one filter or quick find pair is active (matching legacy validation).
- Paging: Page size fixed at 20 (configurable in code).
- Side panel collapse (persists in localStorage).
- Results table with row selection.
- Selection detail pane showing extended fields (TransactionType, BatchNumber, etc.).
- Print function (CSS print media).
- Selection History retrieval by BatchNumber with description logic (to be re-implemented on server).
- Admin vs non-admin logic enforced in UI and API.

## Divergences / Improvements
- Strict separation of concerns (HTML structure, CSS design system, JS behavior).
- Accessibility (ARIA roles, keyboard focus states).
- No inline SQL; all data retrieval via parameterized REST endpoints.
- Descriptive templates for both transactions and history rows.
- Expandable “Quick Find” for LIKE semantics to avoid conflicting with structured filters.

## Backend API (Proposed)
| Endpoint | Method | Parameters | Notes |
|----------|--------|-----------|-------|
| /api/transactions | GET | page, pageSize, sortBy, partId?, user?, building?, dateFrom?, dateTo? | Structured search |
| /api/transactions/like | GET | page, pageSize, sortBy, column, value | Fallback LIKE search |
| /api/transactions/history | GET | batchNumber | Returns chronologically ordered items |

Return JSON arrays of objects adhering to `Model_Transactions` contract plus any computed fields.

## Server Mapping Guidelines
Legacy model (C#):