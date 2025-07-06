# Documentation Generator

This directory contains the script used to generate comprehensive markdown documentation for all C# files in the MTM WIP Application.

## Usage

Run the documentation generator from the project root:

```bash
python3 docs/documentation_generator.py
```

The script will:
1. Scan all .cs files in the solution
2. Check existing .md files in Documents/File Summarys
3. Generate documentation only for empty or placeholder files
4. Skip files with substantive documentation

## Features

- Comprehensive analysis of C# file structure
- Detailed documentation including metadata, dependencies, design patterns
- Security, performance, and testing guidance
- Integration patterns and usage examples
- Commented code snippets to avoid markdown conflicts

## Generated Documentation Location

All generated documentation is placed in: `Documents/File Summarys/`

Each .cs file gets a corresponding .md file with the same base name.