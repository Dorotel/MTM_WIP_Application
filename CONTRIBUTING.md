# Contributing to MTM WIP Application

Thank you for your interest in contributing to the MTM WIP Application! This document provides guidelines for contributing to the project.

## Code/Feature Ownership & Workflow

### Roles and Responsibilities

- **@Dorotel**: Project maintainer and final reviewer
  - Reviews and approves all changes before merging
  - Maintains the comprehensive checklist and standards
  - Final authority on architectural decisions

- **@copilot**: Refactoring and code development agent
  - Handles code refactoring and feature implementation
  - Ensures adherence to coding standards and best practices
  - Implements changes per the comprehensive checklist

### Development Workflow

1. **All changes require review and approval from @Dorotel**
2. **Pull Request Process**:
   - Create feature branch for changes
   - Make minimal, focused changes
   - Follow the `REPO_COMPREHENSIVE_CHECKLIST.md` standards
   - Submit PR with clear description
   - Wait for @Dorotel review and approval
   - Merge only after approval

3. **Change Approach**:
   - **Default**: All-at-once PR for most changes
   - **Staged**: Incremental PRs only if breaking changes or migration required
   - **Proposal**: @copilot proposes incremental approach if needed

## Development Standards

### Code Quality Requirements

All contributions must adhere to the standards defined in `REPO_COMPREHENSIVE_CHECKLIST.md`:

- **File Structure**: One public type per file, file name matches type
- **Naming**: Consistent, descriptive naming conventions
- **Regions**: Organize code with regions for Fields, Properties, Constructors, Methods, Events
- **Namespaces**: Hierarchical, descriptive (e.g., `MTM_WIP_Application.Domain.Models`)
- **Access Modifiers**: Explicit access modifiers, private/protected by default
- **Documentation**: XML docs for public APIs (recommended)

### User Role Privilege System

All code changes must respect the three-tier privilege system:

| Role | Read | Write | Inventory/Transactions | User/Settings/Admin | Search |
|------|------|-------|------------------------|-------------------|---------|
| **Admin** | ✓ | ✓ | ✓ | ✓ | ✓ |
| **Normal** | ✓ | Limited* | ✓ | ✗ | ✓ |
| **Read-Only** | ✓ | ✗ | ✗ | ✗ | ✓ |

*Normal users can only write to `inv_inventory` and `inv_transaction` tables.

### Privilege Enforcement Rules

- **Data Layer**: Check privileges before any write operation
- **UI Layer**: Disable controls based on user role
- **Testing**: Verify all role restrictions work correctly

## Database Changes

### Requirements

All database changes must be documented in `MYSQL_SERVER_CHANGES.md` before merging:

- Date and author
- Reason for change
- SQL statements executed
- Impact assessment
- Rollback procedures (if applicable)

### Database Name

All code must reference the canonical database name: `mtm_wip_application`

## Testing Requirements

### Unit Tests

- All new/refactored critical logic must have unit tests
- Tests should be in `tests/` directory
- Use AAA pattern (Arrange, Act, Assert)
- Mock external dependencies
- Ensure test independence and repeatability

### Privilege Testing

- Test each user role's access permissions
- Verify UI controls are properly disabled/enabled
- Test data access restrictions at DAO layer

## File Organization

### Repository Structure

```
MTM_WIP_Application/
├── src/                    # Source code (or directly in root)
├── tests/                  # Unit tests
├── docs/                   # Additional documentation
├── .editorconfig          # Code style configuration
├── .gitignore             # Git ignore rules
├── README.md              # Project overview
├── REPO_COMPREHENSIVE_CHECKLIST.md  # Development standards
├── MYSQL_SERVER_CHANGES.md          # Database change log
├── DATABASE_SCHEMA.sql               # Current database schema
└── CONTRIBUTING.md                   # This file
```

### Required Files at Root

- `README.md`
- `REPO_COMPREHENSIVE_CHECKLIST.md`
- `MYSQL_SERVER_CHANGES.md`
- `DATABASE_SCHEMA.sql`
- `.editorconfig`
- `.gitignore`
- Solution file (`.sln`)

## Code Review Process

### Pull Request Requirements

1. **Clear Description**: Explain what changes were made and why
2. **Checklist Compliance**: Ensure all changes follow the comprehensive checklist
3. **Testing**: Include or update tests for changed functionality
4. **Documentation**: Update relevant documentation
5. **Database Changes**: Document any schema changes

### Review Criteria

- Code quality and style compliance
- Proper privilege enforcement
- Test coverage for new/changed functionality
- Documentation completeness
- Adherence to architectural standards

## Getting Started

1. **Set up development environment**:
   - .NET 8.0 SDK
   - Visual Studio 2022 or VS Code
   - MySQL Server 5.7+

2. **Clone and configure**:
   ```bash
   git clone https://github.com/Dorotel/MTM_WIP_Application.git
   cd MTM_WIP_Application
   ```

3. **Database setup**:
   - Create `mtm_wip_application` database
   - Execute `DATABASE_SCHEMA.sql`
   - Configure connection string

4. **Review standards**:
   - Read `REPO_COMPREHENSIVE_CHECKLIST.md`
   - Understand privilege system requirements
   - Review existing code patterns

## Questions and Support

For questions about contributing:
- Review `REPO_COMPREHENSIVE_CHECKLIST.md` for detailed standards
- Check existing code patterns for examples
- Contact @Dorotel for architectural guidance

## Branch Protection

- `main` branch requires PR review and approval
- All changes must pass CI/CD checks
- Breaking changes require special coordination

Thank you for contributing to the MTM WIP Application!