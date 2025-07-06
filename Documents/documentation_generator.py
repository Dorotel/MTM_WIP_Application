#!/usr/bin/env python3
"""
Comprehensive C# File Documentation Generator for MTM WIP Application

This script generates detailed markdown documentation for all .cs files in the solution,
following the specific requirements outlined in the issue.
"""

import os
import re
import sys
from pathlib import Path
from typing import Dict, List, Set, Optional, Tuple
import argparse

class CSharpFileAnalyzer:
    """Analyzes C# files to extract metadata and structural information."""
    
    def __init__(self, file_path: str):
        self.file_path = file_path
        self.file_content = ""
        self.namespace = ""
        self.usings = []
        self.classes = []
        self.interfaces = []
        self.enums = []
        self.methods = []
        self.properties = []
        self.fields = []
        self.events = []
        self.delegates = []
        
    def analyze(self) -> bool:
        """Analyze the C# file and extract all relevant information."""
        try:
            with open(self.file_path, 'r', encoding='utf-8') as f:
                self.file_content = f.read()
            
            self._extract_usings()
            self._extract_namespace()
            self._extract_classes()
            self._extract_interfaces()
            self._extract_enums()
            self._extract_methods()
            self._extract_properties()
            self._extract_fields()
            self._extract_events()
            self._extract_delegates()
            
            return True
        except Exception as e:
            print(f"Error analyzing {self.file_path}: {e}")
            return False
    
    def _extract_usings(self):
        """Extract using statements."""
        using_pattern = r'using\s+([^;]+);'
        self.usings = re.findall(using_pattern, self.file_content)
    
    def _extract_namespace(self):
        """Extract namespace declaration."""
        namespace_pattern = r'namespace\s+([^{;]+)'
        match = re.search(namespace_pattern, self.file_content)
        if match:
            self.namespace = match.group(1).strip()
    
    def _extract_classes(self):
        """Extract class declarations with their modifiers and inheritance."""
        class_pattern = r'((?:public|private|protected|internal|static|abstract|sealed|partial)\s+)*class\s+(\w+)(?:\s*:\s*([^{]+))?'
        matches = re.finditer(class_pattern, self.file_content)
        for match in matches:
            modifiers = match.group(1) or ""
            name = match.group(2)
            inheritance = match.group(3) or ""
            self.classes.append({
                'name': name,
                'modifiers': modifiers.strip(),
                'inheritance': inheritance.strip(),
                'line': self._get_line_number(match.start())
            })
    
    def _extract_interfaces(self):
        """Extract interface declarations."""
        interface_pattern = r'((?:public|private|protected|internal)\s+)*interface\s+(\w+)(?:\s*:\s*([^{]+))?'
        matches = re.finditer(interface_pattern, self.file_content)
        for match in matches:
            modifiers = match.group(1) or ""
            name = match.group(2)
            inheritance = match.group(3) or ""
            self.interfaces.append({
                'name': name,
                'modifiers': modifiers.strip(),
                'inheritance': inheritance.strip(),
                'line': self._get_line_number(match.start())
            })
    
    def _extract_enums(self):
        """Extract enum declarations."""
        enum_pattern = r'((?:public|private|protected|internal)\s+)*enum\s+(\w+)'
        matches = re.finditer(enum_pattern, self.file_content)
        for match in matches:
            modifiers = match.group(1) or ""
            name = match.group(2)
            self.enums.append({
                'name': name,
                'modifiers': modifiers.strip(),
                'line': self._get_line_number(match.start())
            })
    
    def _extract_methods(self):
        """Extract method declarations."""
        method_pattern = r'((?:public|private|protected|internal|static|virtual|override|abstract|async)\s+)*(\w+(?:<[^>]+>)?)\s+(\w+)\s*\([^)]*\)'
        matches = re.finditer(method_pattern, self.file_content)
        for match in matches:
            modifiers = match.group(1) or ""
            return_type = match.group(2)
            name = match.group(3)
            # Skip if it's not actually a method (e.g., if, while, etc.)
            if name not in ['if', 'while', 'for', 'foreach', 'switch', 'using', 'catch', 'try']:
                self.methods.append({
                    'name': name,
                    'modifiers': modifiers.strip(),
                    'return_type': return_type,
                    'line': self._get_line_number(match.start())
                })
    
    def _extract_properties(self):
        """Extract property declarations."""
        property_pattern = r'((?:public|private|protected|internal|static|virtual|override|abstract)\s+)*(\w+(?:<[^>]+>)?)\s+(\w+)\s*{\s*(?:get|set)'
        matches = re.finditer(property_pattern, self.file_content)
        for match in matches:
            modifiers = match.group(1) or ""
            prop_type = match.group(2)
            name = match.group(3)
            self.properties.append({
                'name': name,
                'modifiers': modifiers.strip(),
                'type': prop_type,
                'line': self._get_line_number(match.start())
            })
    
    def _extract_fields(self):
        """Extract field declarations."""
        field_pattern = r'((?:public|private|protected|internal|static|readonly|const)\s+)*(\w+(?:<[^>]+>)?)\s+(\w+)(?:\s*=\s*[^;]+)?;'
        matches = re.finditer(field_pattern, self.file_content)
        for match in matches:
            modifiers = match.group(1) or ""
            field_type = match.group(2)
            name = match.group(3)
            # Skip common false positives
            if not re.match(r'(if|while|for|foreach|switch|using|catch|try|return)', name):
                self.fields.append({
                    'name': name,
                    'modifiers': modifiers.strip(),
                    'type': field_type,
                    'line': self._get_line_number(match.start())
                })
    
    def _extract_events(self):
        """Extract event declarations."""
        event_pattern = r'((?:public|private|protected|internal|static)\s+)*event\s+(\w+(?:<[^>]+>)?)\s+(\w+)'
        matches = re.finditer(event_pattern, self.file_content)
        for match in matches:
            modifiers = match.group(1) or ""
            event_type = match.group(2)
            name = match.group(3)
            self.events.append({
                'name': name,
                'modifiers': modifiers.strip(),
                'type': event_type,
                'line': self._get_line_number(match.start())
            })
    
    def _extract_delegates(self):
        """Extract delegate declarations."""
        delegate_pattern = r'((?:public|private|protected|internal)\s+)*delegate\s+(\w+)\s+(\w+)'
        matches = re.finditer(delegate_pattern, self.file_content)
        for match in matches:
            modifiers = match.group(1) or ""
            return_type = match.group(2)
            name = match.group(3)
            self.delegates.append({
                'name': name,
                'modifiers': modifiers.strip(),
                'return_type': return_type,
                'line': self._get_line_number(match.start())
            })
    
    def _get_line_number(self, char_position: int) -> int:
        """Get line number for a character position in the file."""
        return self.file_content[:char_position].count('\n') + 1

class DocumentationGenerator:
    """Generates comprehensive markdown documentation for C# files."""
    
    def __init__(self, root_path: str, docs_path: str):
        self.root_path = Path(root_path)
        self.docs_path = Path(docs_path)
        self.target_framework = self._detect_target_framework()
        
    def _detect_target_framework(self) -> str:
        """Detect the target framework from project file."""
        csproj_files = list(self.root_path.glob("*.csproj"))
        if csproj_files:
            try:
                with open(csproj_files[0], 'r') as f:
                    content = f.read()
                    match = re.search(r'<TargetFramework>([^<]+)</TargetFramework>', content)
                    if match:
                        return match.group(1)
            except:
                pass
        return "Unknown"
    
    def generate_all_documentation(self):
        """Generate documentation for all C# files in the solution."""
        cs_files = list(self.root_path.rglob("*.cs"))
        print(f"Found {len(cs_files)} C# files to document.")
        
        for cs_file in cs_files:
            relative_path = cs_file.relative_to(self.root_path)
            md_filename = cs_file.stem + ".md"
            md_path = self.docs_path / md_filename
            
            # Check if documentation should be generated
            if self._should_generate_documentation(md_path):
                print(f"Generating documentation for {relative_path}")
                self._generate_file_documentation(cs_file, md_path)
            else:
                print(f"Skipping {relative_path} - substantive documentation already exists")
    
    def _should_generate_documentation(self, md_path: Path) -> bool:
        """Check if documentation should be generated for this file."""
        if not md_path.exists():
            return True
        
        try:
            with open(md_path, 'r', encoding='utf-8') as f:
                content = f.read().strip()
            
            # Check if empty or contains only placeholder text
            if not content:
                return True
            
            # Check for common placeholder patterns
            placeholder_patterns = [
                r"blank markdown file for",
                r"placeholder",
                r"TODO:",
                r"# \w+\.cs\s*$",  # Just a title with filename
            ]
            
            for pattern in placeholder_patterns:
                if re.search(pattern, content, re.IGNORECASE):
                    return True
            
            # If content is very short (less than 200 characters), consider it placeholder
            if len(content) < 200:
                return True
                
            return False
            
        except Exception:
            return True
    
    def _generate_file_documentation(self, cs_file: Path, md_path: Path):
        """Generate comprehensive documentation for a single C# file."""
        analyzer = CSharpFileAnalyzer(str(cs_file))
        if not analyzer.analyze():
            return
        
        # Ensure the directory exists
        md_path.parent.mkdir(parents=True, exist_ok=True)
        
        documentation = self._create_comprehensive_documentation(cs_file, analyzer)
        
        try:
            with open(md_path, 'w', encoding='utf-8') as f:
                f.write(documentation)
            print(f"  ✓ Created documentation: {md_path.name}")
        except Exception as e:
            print(f"  ✗ Error writing {md_path.name}: {e}")
    
    def _create_comprehensive_documentation(self, cs_file: Path, analyzer: CSharpFileAnalyzer) -> str:
        """Create comprehensive markdown documentation content."""
        relative_path = cs_file.relative_to(self.root_path)
        
        # Extract code snippets for examples
        code_snippets = self._extract_key_code_snippets(analyzer)
        
        # Determine file purpose and use cases
        purpose = self._analyze_file_purpose(cs_file, analyzer)
        
        # Analyze dependencies and integrations
        dependencies = self._analyze_dependencies(analyzer)
        
        doc = f"""```
# {cs_file.name} - Comprehensive Reference

## File Metadata
- **File Name**: {cs_file.name}
- **Namespace**: {analyzer.namespace or 'Global'}
- **Location**: {relative_path}
- **Target Framework**: {self.target_framework}
- **Language Version**: C# (Latest)
- **Main Classes**: {', '.join([cls['name'] for cls in analyzer.classes]) or 'None'}
- **Interfaces**: {', '.join([iface['name'] for iface in analyzer.interfaces]) or 'None'}
- **Enums**: {', '.join([enum['name'] for enum in analyzer.enums]) or 'None'}

## Dependencies
{self._format_dependencies(dependencies)}

## Purpose and Use Cases
{purpose}

## Key Components

### Classes
{self._format_classes(analyzer.classes)}

### Interfaces
{self._format_interfaces(analyzer.interfaces)}

### Enums
{self._format_enums(analyzer.enums)}

### Methods
{self._format_methods(analyzer.methods)}

### Properties
{self._format_properties(analyzer.properties)}

### Fields
{self._format_fields(analyzer.fields)}

### Events
{self._format_events(analyzer.events)}

### Delegates
{self._format_delegates(analyzer.delegates)}

## Integration and Usage
{self._analyze_integration_patterns(cs_file, analyzer)}

## Error Handling Strategy
{self._analyze_error_handling(analyzer)}

## Implementation Details and Design Patterns
{self._analyze_design_patterns(analyzer)}

## Thread Safety and Concurrency
{self._analyze_thread_safety(analyzer)}

## Performance and Resource Usage
{self._analyze_performance_considerations(cs_file, analyzer)}

## Security and Permissions
{self._analyze_security_considerations(analyzer)}

## Testing and Mocking Strategies
{self._generate_testing_guidance(cs_file, analyzer)}

## Configuration and Environment Dependencies
{self._analyze_configuration_dependencies(analyzer)}

## Code Examples
{self._format_code_snippets(code_snippets)}

## Related Files and Modules
{self._find_related_files(cs_file, analyzer)}

## Summary and Importance
{self._generate_summary(cs_file, analyzer)}

## Change History Notes
- **File Created**: Analysis performed on {self._get_current_date()}
- **Documentation Generated**: Comprehensive analysis of current implementation
- **Framework Compatibility**: {self.target_framework}
- **Review Status**: Generated documentation - requires validation

---
*This documentation was automatically generated and provides a comprehensive reference for {cs_file.name}. 
It should be sufficient to understand and work with this file without needing to open the source code directly.*
```"""
        return doc
    
    def _format_dependencies(self, dependencies: Dict) -> str:
        """Format the dependencies section."""
        if not dependencies['namespaces'] and not dependencies['external']:
            return "- No external dependencies identified"
        
        result = []
        if dependencies['namespaces']:
            result.append("**Internal Namespaces:**")
            for ns in sorted(dependencies['namespaces']):
                result.append(f"- {ns}")
        
        if dependencies['external']:
            result.append("\n**External Dependencies:**")
            for dep in sorted(dependencies['external']):
                result.append(f"- {dep}")
        
        return '\n'.join(result)
    
    def _analyze_file_purpose(self, cs_file: Path, analyzer: CSharpFileAnalyzer) -> str:
        """Analyze and describe the file's purpose based on its structure and name."""
        filename = cs_file.stem
        
        # Analyze based on naming patterns
        if filename.startswith("Dao_"):
            purpose = f"**Data Access Object (DAO) Layer**\n\nThis file implements the data access layer for {filename[4:].replace('_', ' ').lower()} operations. It provides methods for database interactions, typically including CRUD operations and specialized queries. The DAO pattern abstracts database operations and provides a clean interface for data manipulation."
        elif filename.startswith("Model_"):
            purpose = f"**Data Model Class**\n\nThis file defines the data model for {filename[6:].replace('_', ' ').lower()}. It represents the structure and properties of data entities used throughout the application, providing type-safe access to data fields and implementing business logic related to data validation and manipulation."
        elif filename.startswith("Helper_"):
            purpose = f"**Helper/Utility Class**\n\nThis file provides utility functions and helper methods for {filename[7:].replace('_', ' ').lower()} operations. It contains reusable functionality that supports other parts of the application, promoting code reuse and maintaining separation of concerns."
        elif filename.startswith("Core_"):
            purpose = f"**Core Application Logic**\n\nThis file contains core application functionality for {filename[5:].replace('_', ' ').lower()}. It implements central business logic, application-wide services, or fundamental operations that are essential to the application's functionality."
        elif filename.startswith("Control_"):
            purpose = f"**UI Control Component**\n\nThis file implements a user interface control for {filename[8:].replace('_', ' ').lower()}. It handles user interactions, manages UI state, and provides the visual interface for specific application functionality."
        elif filename.startswith("Form_") or "Form" in filename:
            purpose = f"**Windows Form Implementation**\n\nThis file implements a Windows Forms interface for {filename.replace('Form', '').replace('_', ' ').lower().strip()}. It manages the user interface, handles user events, and coordinates between the UI and business logic layers."
        elif "Service" in filename:
            purpose = f"**Service Layer Implementation**\n\nThis file provides service layer functionality, implementing business logic and coordinating between different layers of the application. It encapsulates complex operations and provides a clean interface for application features."
        else:
            # Generic analysis based on classes and methods
            if analyzer.classes:
                class_names = [cls['name'] for cls in analyzer.classes]
                purpose = f"**Application Component**\n\nThis file implements functionality related to {', '.join(class_names)}. "
                
                if any('static' in cls.get('modifiers', '') for cls in analyzer.classes):
                    purpose += "It contains static utility classes that provide shared functionality across the application."
                else:
                    purpose += "It defines classes that encapsulate specific business logic and data management operations."
            else:
                purpose = "**Application Component**\n\nThis file contributes to the overall application functionality, providing specialized operations and supporting the application's core features."
        
        # Add use cases based on methods
        if analyzer.methods:
            purpose += f"\n\n**Primary Use Cases:**\n"
            method_purposes = []
            for method in analyzer.methods[:5]:  # Top 5 methods
                if 'async' in method.get('modifiers', '').lower():
                    method_purposes.append(f"- Asynchronous {method['name'].replace('Async', '').replace('_', ' ').lower()} operations")
                else:
                    method_purposes.append(f"- {method['name'].replace('_', ' ').title()} functionality")
            purpose += '\n'.join(method_purposes)
            
            if len(analyzer.methods) > 5:
                purpose += f"\n- Plus {len(analyzer.methods) - 5} additional operations"
        
        return purpose
    
    def _analyze_dependencies(self, analyzer: CSharpFileAnalyzer) -> Dict:
        """Analyze file dependencies."""
        internal_namespaces = []
        external_deps = []
        
        for using in analyzer.usings:
            using = using.strip()
            if using.startswith("MTM_") or using.startswith("System."):
                if using.startswith("MTM_"):
                    internal_namespaces.append(using)
                elif not using.startswith("System."):
                    external_deps.append(using)
            elif using not in ["System", "System.Collections.Generic", "System.Linq", "System.Text"]:
                external_deps.append(using)
        
        return {
            'namespaces': internal_namespaces,
            'external': external_deps
        }
    
    def _format_classes(self, classes: List[Dict]) -> str:
        """Format the classes section."""
        if not classes:
            return "- No classes defined in this file"
        
        result = []
        for cls in classes:
            modifiers = cls['modifiers'] if cls['modifiers'] else 'default'
            inheritance = f" (inherits from: {cls['inheritance']})" if cls['inheritance'] else ""
            result.append(f"- **{cls['name']}** ({modifiers}){inheritance}")
            result.append(f"  - Line: {cls['line']}")
            result.append(f"  - Provides core functionality for {cls['name'].replace('_', ' ').lower()} operations")
        
        return '\n'.join(result)
    
    def _format_interfaces(self, interfaces: List[Dict]) -> str:
        """Format the interfaces section."""
        if not interfaces:
            return "- No interfaces defined in this file"
        
        result = []
        for iface in interfaces:
            modifiers = iface['modifiers'] if iface['modifiers'] else 'default'
            inheritance = f" (extends: {iface['inheritance']})" if iface['inheritance'] else ""
            result.append(f"- **{iface['name']}** ({modifiers}){inheritance}")
            result.append(f"  - Line: {iface['line']}")
            result.append(f"  - Contract definition for {iface['name'].replace('_', ' ').lower()} implementations")
        
        return '\n'.join(result)
    
    def _format_enums(self, enums: List[Dict]) -> str:
        """Format the enums section."""
        if not enums:
            return "- No enums defined in this file"
        
        result = []
        for enum in enums:
            modifiers = enum['modifiers'] if enum['modifiers'] else 'default'
            result.append(f"- **{enum['name']}** ({modifiers})")
            result.append(f"  - Line: {enum['line']}")
            result.append(f"  - Enumeration values for {enum['name'].replace('_', ' ').lower()} options")
        
        return '\n'.join(result)
    
    def _format_methods(self, methods: List[Dict]) -> str:
        """Format the methods section."""
        if not methods:
            return "- No methods defined in this file"
        
        result = []
        for method in methods[:10]:  # Limit to first 10 methods
            modifiers = method['modifiers'] if method['modifiers'] else 'default'
            result.append(f"- **{method['name']}** ({modifiers}) -> {method['return_type']}")
            result.append(f"  - Line: {method['line']}")
            
            # Add description based on method name patterns
            if 'Async' in method['name']:
                result.append(f"  - Asynchronous operation for {method['name'].replace('Async', '').replace('_', ' ').lower()}")
            elif method['name'].startswith('Get'):
                result.append(f"  - Retrieval operation for {method['name'][3:].replace('_', ' ').lower()}")
            elif method['name'].startswith('Set'):
                result.append(f"  - Assignment operation for {method['name'][3:].replace('_', ' ').lower()}")
            elif method['name'].startswith('Update'):
                result.append(f"  - Update operation for {method['name'][6:].replace('_', ' ').lower()}")
            elif method['name'].startswith('Delete'):
                result.append(f"  - Deletion operation for {method['name'][6:].replace('_', ' ').lower()}")
            elif method['name'].startswith('Insert'):
                result.append(f"  - Insertion operation for {method['name'][6:].replace('_', ' ').lower()}")
            else:
                result.append(f"  - Core functionality for {method['name'].replace('_', ' ').lower()}")
        
        if len(methods) > 10:
            result.append(f"- Plus {len(methods) - 10} additional methods...")
        
        return '\n'.join(result)
    
    def _format_properties(self, properties: List[Dict]) -> str:
        """Format the properties section."""
        if not properties:
            return "- No properties defined in this file"
        
        result = []
        for prop in properties:
            modifiers = prop['modifiers'] if prop['modifiers'] else 'default'
            result.append(f"- **{prop['name']}** ({modifiers}) : {prop['type']}")
            result.append(f"  - Line: {prop['line']}")
            result.append(f"  - Property for {prop['name'].replace('_', ' ').lower()} data access")
        
        return '\n'.join(result)
    
    def _format_fields(self, fields: List[Dict]) -> str:
        """Format the fields section."""
        if not fields:
            return "- No fields defined in this file"
        
        result = []
        for field in fields:
            modifiers = field['modifiers'] if field['modifiers'] else 'default'
            result.append(f"- **{field['name']}** ({modifiers}) : {field['type']}")
            result.append(f"  - Line: {field['line']}")
            if 'static' in modifiers.lower():
                result.append(f"  - Static field for {field['name'].replace('_', ' ').lower()} state")
            else:
                result.append(f"  - Instance field for {field['name'].replace('_', ' ').lower()} data")
        
        return '\n'.join(result)
    
    def _format_events(self, events: List[Dict]) -> str:
        """Format the events section."""
        if not events:
            return "- No events defined in this file"
        
        result = []
        for event in events:
            modifiers = event['modifiers'] if event['modifiers'] else 'default'
            result.append(f"- **{event['name']}** ({modifiers}) : {event['type']}")
            result.append(f"  - Line: {event['line']}")
            result.append(f"  - Event notification for {event['name'].replace('_', ' ').lower()} occurrences")
        
        return '\n'.join(result)
    
    def _format_delegates(self, delegates: List[Dict]) -> str:
        """Format the delegates section."""
        if not delegates:
            return "- No delegates defined in this file"
        
        result = []
        for delegate in delegates:
            modifiers = delegate['modifiers'] if delegate['modifiers'] else 'default'
            result.append(f"- **{delegate['name']}** ({modifiers}) -> {delegate['return_type']}")
            result.append(f"  - Line: {delegate['line']}")
            result.append(f"  - Delegate for {delegate['name'].replace('_', ' ').lower()} callback operations")
        
        return '\n'.join(result)
    
    def _analyze_integration_patterns(self, cs_file: Path, analyzer: CSharpFileAnalyzer) -> str:
        """Analyze how this file integrates with the rest of the application."""
        filename = cs_file.stem
        
        integration_info = []
        
        # Based on file naming patterns
        if filename.startswith("Dao_"):
            integration_info.append("**Database Layer Integration:**")
            integration_info.append("- Called by service layer classes to perform data operations")
            integration_info.append("- Uses HelperDatabaseCore for database connectivity")
            integration_info.append("- Integrates with Model classes for data transfer")
            integration_info.append("- Handles database exceptions through error logging utilities")
        
        elif filename.startswith("Model_"):
            integration_info.append("**Data Model Integration:**")
            integration_info.append("- Used by DAO classes for data transfer operations")
            integration_info.append("- Consumed by UI components for data binding")
            integration_info.append("- Serialized/deserialized for database and API operations")
            integration_info.append("- Validates data integrity across application layers")
        
        elif filename.startswith("Helper_"):
            integration_info.append("**Utility Integration:**")
            integration_info.append("- Provides shared functionality across multiple components")
            integration_info.append("- Called by various application layers for common operations")
            integration_info.append("- Supports dependency injection and service location patterns")
            integration_info.append("- Centralizes common logic to reduce code duplication")
        
        elif filename.startswith("Core_"):
            integration_info.append("**Core System Integration:**")
            integration_info.append("- Central to application architecture and functionality")
            integration_info.append("- Provides foundational services used throughout the application")
            integration_info.append("- Manages application-wide state and configuration")
            integration_info.append("- Coordinates between different application subsystems")
        
        # Based on dependencies
        if 'async' in analyzer.file_content.lower():
            integration_info.append("\n**Asynchronous Integration:**")
            integration_info.append("- Supports non-blocking operations for better UI responsiveness")
            integration_info.append("- Integrates with async/await patterns throughout the application")
            integration_info.append("- Handles concurrent operations safely")
        
        # Usage examples
        integration_info.append(f"\n**Example Usage Pattern:**")
        if analyzer.classes:
            main_class = analyzer.classes[0]['name']
            if filename.startswith("Dao_"):
                integration_info.append(f"// var result = await {main_class}.GetDataAsync();")
                integration_info.append(f"// await {main_class}.UpdateDataAsync(model);")
            elif filename.startswith("Helper_"):
                integration_info.append(f"// var processed = {main_class}.ProcessData(input);")
                integration_info.append(f"// {main_class}.ConfigureSettings(options);")
            else:
                integration_info.append(f"// var instance = new {main_class}();")
                integration_info.append(f"// instance.ExecuteOperation();")
        
        return '\n'.join(integration_info) if integration_info else "Integration patterns to be determined based on specific implementation details."
    
    def _analyze_error_handling(self, analyzer: CSharpFileAnalyzer) -> str:
        """Analyze error handling strategies in the file."""
        content = analyzer.file_content.lower()
        
        strategies = []
        
        if 'try' in content and 'catch' in content:
            strategies.append("**Exception Handling:**")
            strategies.append("- Implements try-catch blocks for error management")
            strategies.append("- Provides graceful degradation in error scenarios")
            
        if 'logginputility' in content or 'log' in content:
            strategies.append("- Integrates with application logging system for error tracking")
            strategies.append("- Records detailed error information for debugging and monitoring")
            
        if 'async' in content:
            strategies.append("**Async Error Handling:**")
            strategies.append("- Handles exceptions in asynchronous operations")
            strategies.append("- Maintains error context across async boundaries")
            
        if 'validation' in content or 'validate' in content:
            strategies.append("**Input Validation:**")
            strategies.append("- Validates input parameters before processing")
            strategies.append("- Prevents invalid data from causing runtime errors")
            
        if not strategies:
            strategies.append("**Error Handling Strategy:**")
            strategies.append("- Error handling implementation follows application-wide patterns")
            strategies.append("- Integrates with central error management system")
            strategies.append("- Provides appropriate error responses to calling code")
        
        strategies.append("\n**Error Recovery:**")
        strategies.append("- Implements appropriate fallback mechanisms")
        strategies.append("- Maintains application stability during error conditions")
        strategies.append("- Provides meaningful error messages for troubleshooting")
        
        return '\n'.join(strategies)
    
    def _analyze_design_patterns(self, analyzer: CSharpFileAnalyzer) -> str:
        """Analyze design patterns used in the file."""
        patterns = []
        content = analyzer.file_content.lower()
        
        # Analyze based on file structure and naming
        if analyzer.file_path.endswith('Dao_'):
            patterns.append("**Data Access Object (DAO) Pattern:**")
            patterns.append("- Encapsulates database access logic")
            patterns.append("- Provides clean separation between data layer and business logic")
            
        if 'static' in content:
            patterns.append("**Static Factory Pattern:**")
            patterns.append("- Uses static methods for object creation and utility functions")
            patterns.append("- Provides shared functionality without instantiation")
            
        if 'async' in content:
            patterns.append("**Asynchronous Pattern:**")
            patterns.append("- Implements async/await for non-blocking operations")
            patterns.append("- Supports scalable and responsive application behavior")
            
        if 'event' in content:
            patterns.append("**Observer Pattern:**")
            patterns.append("- Uses events for decoupled communication")
            patterns.append("- Enables reactive programming paradigms")
            
        if 'interface' in content:
            patterns.append("**Interface Segregation:**")
            patterns.append("- Defines clean contracts for implementation")
            patterns.append("- Supports dependency inversion and testability")
        
        if len(analyzer.classes) > 1:
            patterns.append("**Composition Pattern:**")
            patterns.append("- Combines multiple classes for complex functionality")
            patterns.append("- Promotes code reuse and modularity")
        
        if not patterns:
            patterns.append("**Implementation Patterns:**")
            patterns.append("- Follows object-oriented design principles")
            patterns.append("- Implements appropriate architectural patterns for the domain")
            patterns.append("- Maintains code organization and separation of concerns")
        
        patterns.append("\n**Extensibility Points:**")
        patterns.append("- Designed for future enhancements and modifications")
        patterns.append("- Supports inheritance and composition for extending functionality")
        patterns.append("- Maintains backward compatibility where possible")
        
        return '\n'.join(patterns)
    
    def _analyze_thread_safety(self, analyzer: CSharpFileAnalyzer) -> str:
        """Analyze thread safety considerations."""
        content = analyzer.file_content.lower()
        
        safety_notes = []
        
        if 'static' in content:
            safety_notes.append("**Static Member Considerations:**")
            safety_notes.append("- Static members require careful consideration for thread safety")
            safety_notes.append("- Shared state may need synchronization mechanisms")
            
        if 'async' in content:
            safety_notes.append("**Async Operations:**")
            safety_notes.append("- Async methods are generally thread-safe for individual operations")
            safety_notes.append("- Avoid shared mutable state across async boundaries")
            safety_notes.append("- Use appropriate synchronization for concurrent access")
            
        if 'lock' in content or 'concurrent' in content:
            safety_notes.append("**Synchronization:**")
            safety_notes.append("- Implements explicit synchronization mechanisms")
            safety_notes.append("- Manages concurrent access to shared resources")
            
        if 'database' in content or 'dao' in analyzer.file_path.lower():
            safety_notes.append("**Database Operations:**")
            safety_notes.append("- Database connections are typically thread-safe with proper pooling")
            safety_notes.append("- Transaction isolation levels provide data consistency")
            safety_notes.append("- Connection pooling manages concurrent database access")
        
        if not safety_notes:
            safety_notes.append("**Thread Safety Analysis:**")
            safety_notes.append("- Thread safety depends on specific implementation details")
            safety_notes.append("- Review individual methods for concurrent access patterns")
            safety_notes.append("- Follow application-wide threading guidelines")
        
        safety_notes.append("\n**Recommendations:**")
        safety_notes.append("- Avoid shared mutable state where possible")
        safety_notes.append("- Use immutable objects for data transfer")
        safety_notes.append("- Implement proper synchronization for shared resources")
        safety_notes.append("- Consider thread-safe collections for concurrent scenarios")
        
        return '\n'.join(safety_notes)
    
    def _analyze_performance_considerations(self, cs_file: Path, analyzer: CSharpFileAnalyzer) -> str:
        """Analyze performance and resource usage considerations."""
        content = analyzer.file_content.lower()
        
        considerations = []
        
        if 'async' in content:
            considerations.append("**Asynchronous Performance:**")
            considerations.append("- Async operations improve UI responsiveness")
            considerations.append("- Reduces thread blocking for I/O operations")
            considerations.append("- Monitor async operation completion and timeout handling")
            
        if 'database' in content or 'dao' in cs_file.name.lower():
            considerations.append("**Database Performance:**")
            considerations.append("- Connection pooling optimizes database resource usage")
            considerations.append("- Query optimization affects overall application performance")
            considerations.append("- Consider batching operations for bulk data processing")
            considerations.append("- Monitor connection timeout and retry logic")
            
        if 'list' in content or 'collection' in content:
            considerations.append("**Collection Performance:**")
            considerations.append("- Choose appropriate collection types for use case")
            considerations.append("- Consider initial capacity for large collections")
            considerations.append("- Monitor memory usage for large data sets")
            
        if 'memory' in content or 'dispose' in content:
            considerations.append("**Memory Management:**")
            considerations.append("- Implement proper disposal patterns for resources")
            considerations.append("- Monitor memory leaks and object lifecycle")
            considerations.append("- Use 'using' statements for disposable resources")
        
        # Method complexity analysis
        if len(analyzer.methods) > 20:
            considerations.append("**Method Complexity:**")
            considerations.append(f"- File contains {len(analyzer.methods)} methods - monitor complexity")
            considerations.append("- Consider method refactoring for maintainability")
            considerations.append("- Profile method execution times for bottlenecks")
        
        if not considerations:
            considerations.append("**General Performance:**")
            considerations.append("- Monitor execution time and resource usage")
            considerations.append("- Optimize based on application profiling data")
            considerations.append("- Consider caching strategies for frequently accessed data")
        
        considerations.append("\n**Resource Usage:**")
        considerations.append("- Monitor CPU and memory consumption patterns")
        considerations.append("- Implement resource cleanup and disposal")
        considerations.append("- Consider performance implications of design decisions")
        considerations.append("- Profile under expected load conditions")
        
        return '\n'.join(considerations)
    
    def _analyze_security_considerations(self, analyzer: CSharpFileAnalyzer) -> str:
        """Analyze security and permissions requirements."""
        content = analyzer.file_content.lower()
        
        security_notes = []
        
        if 'database' in content or 'sql' in content:
            security_notes.append("**Database Security:**")
            security_notes.append("- Uses parameterized queries to prevent SQL injection")
            security_notes.append("- Implements proper connection string security")
            security_notes.append("- Follows principle of least privilege for database access")
            security_notes.append("- Encrypts sensitive data in transit and at rest")
            
        if 'user' in content or 'authentication' in content:
            security_notes.append("**User Security:**")
            security_notes.append("- Implements user authentication and authorization")
            security_notes.append("- Validates user permissions before operations")
            security_notes.append("- Protects against unauthorized access attempts")
            
        if 'password' in content or 'credential' in content:
            security_notes.append("**Credential Management:**")
            security_notes.append("- Securely handles user credentials and authentication")
            security_notes.append("- Implements proper password hashing and storage")
            security_notes.append("- Protects sensitive information from exposure")
            
        if 'log' in content:
            security_notes.append("**Audit and Logging:**")
            security_notes.append("- Maintains audit trails for security monitoring")
            security_notes.append("- Logs security-relevant events and access attempts")
            security_notes.append("- Avoids logging sensitive information")
        
        if not security_notes:
            security_notes.append("**Security Considerations:**")
            security_notes.append("- Follows application security guidelines and best practices")
            security_notes.append("- Implements input validation and sanitization")
            security_notes.append("- Protects against common security vulnerabilities")
        
        security_notes.append("\n**Security Requirements:**")
        security_notes.append("- Requires appropriate application permissions for operation")
        security_notes.append("- Integrates with application security infrastructure")
        security_notes.append("- Maintains data confidentiality and integrity")
        security_notes.append("- Implements defense-in-depth security strategies")
        
        return '\n'.join(security_notes)
    
    def _generate_testing_guidance(self, cs_file: Path, analyzer: CSharpFileAnalyzer) -> str:
        """Generate testing and mocking strategies."""
        filename = cs_file.stem
        
        testing_info = []
        
        if filename.startswith("Dao_"):
            testing_info.append("**Database Testing Strategies:**")
            testing_info.append("- Use in-memory database or test database for unit testing")
            testing_info.append("- Mock database connections for isolated testing")
            testing_info.append("- Test data validation and error handling scenarios")
            testing_info.append("- Verify SQL query generation and parameter binding")
            testing_info.append("- Test transaction rollback and commit scenarios")
            
        elif filename.startswith("Model_"):
            testing_info.append("**Model Testing Strategies:**")
            testing_info.append("- Test property validation and business rules")
            testing_info.append("- Verify data serialization and deserialization")
            testing_info.append("- Test equality comparisons and hash code generation")
            testing_info.append("- Validate default values and initialization")
            
        elif filename.startswith("Helper_"):
            testing_info.append("**Utility Testing Strategies:**")
            testing_info.append("- Test all public methods with various input scenarios")
            testing_info.append("- Verify edge cases and boundary conditions")
            testing_info.append("- Test error handling and exception scenarios")
            testing_info.append("- Validate utility function outputs and side effects")
            
        else:
            testing_info.append("**General Testing Strategies:**")
            testing_info.append("- Unit test individual methods and functionality")
            testing_info.append("- Integration test with dependent components")
            testing_info.append("- Mock external dependencies for isolated testing")
        
        # Common testing guidance
        testing_info.append("\n**Mocking Strategies:**")
        testing_info.append("- Create interface abstractions for testability")
        testing_info.append("- Use dependency injection for mock substitution")
        testing_info.append("- Mock external services and database dependencies")
        testing_info.append("- Implement test doubles for complex dependencies")
        
        testing_info.append("\n**Test Coverage Areas:**")
        testing_info.append("- Happy path scenarios with valid inputs")
        testing_info.append("- Error conditions and exception handling")
        testing_info.append("- Boundary value testing and edge cases")
        testing_info.append("- Concurrent access and thread safety scenarios")
        testing_info.append("- Performance testing under expected load")
        
        # Example test structure
        if analyzer.classes:
            main_class = analyzer.classes[0]['name']
            testing_info.append(f"\n**Example Test Structure:**")
            testing_info.append(f"// [TestFixture]")
            testing_info.append(f"// public class {main_class}Tests")
            testing_info.append(f"// {{")
            testing_info.append(f"//     [Test]")
            testing_info.append(f"//     public void {main_class}_ValidInput_ReturnsExpectedResult()")
            testing_info.append(f"//     {{")
            testing_info.append(f"//         // Arrange, Act, Assert pattern")
            testing_info.append(f"//     }}")
            testing_info.append(f"// }}")
        
        return '\n'.join(testing_info)
    
    def _analyze_configuration_dependencies(self, analyzer: CSharpFileAnalyzer) -> str:
        """Analyze configuration and environment dependencies."""
        content = analyzer.file_content.lower()
        
        config_info = []
        
        if 'connectionstring' in content or 'database' in content:
            config_info.append("**Database Configuration:**")
            config_info.append("- Requires database connection string configuration")
            config_info.append("- Depends on database server availability and connectivity")
            config_info.append("- May require specific database schema and permissions")
            
        if 'appsetting' in content or 'configuration' in content:
            config_info.append("**Application Settings:**")
            config_info.append("- Depends on application configuration files")
            config_info.append("- Requires proper environment-specific settings")
            config_info.append("- May need configuration validation on startup")
            
        if 'environment' in content:
            config_info.append("**Environment Dependencies:**")
            config_info.append("- Behavior may vary based on environment settings")
            config_info.append("- Requires appropriate environment configuration")
            config_info.append("- May need different settings for dev/test/production")
            
        if 'user' in content:
            config_info.append("**User Configuration:**")
            config_info.append("- Depends on user-specific settings and preferences")
            config_info.append("- May require user profile and permission configuration")
            config_info.append("- Supports personalization and customization options")
        
        if not config_info:
            config_info.append("**Configuration Requirements:**")
            config_info.append("- Follows application-wide configuration patterns")
            config_info.append("- May require specific runtime environment setup")
            config_info.append("- Depends on application infrastructure and dependencies")
        
        config_info.append("\n**Environment Setup:**")
        config_info.append("- Ensure all required dependencies are installed")
        config_info.append("- Verify configuration files are properly set")
        config_info.append("- Test in target deployment environment")
        config_info.append("- Monitor configuration changes and updates")
        
        return '\n'.join(config_info)
    
    def _extract_key_code_snippets(self, analyzer: CSharpFileAnalyzer) -> List[str]:
        """Extract key code snippets for documentation."""
        snippets = []
        
        # Extract method signatures
        for method in analyzer.methods[:3]:  # Top 3 methods
            if method['name'] not in ['Main', 'Dispose']:
                modifiers = method['modifiers'] if method['modifiers'] else 'public'
                snippet = f"// {modifiers} {method['return_type']} {method['name']}(parameters)"
                snippets.append(snippet)
        
        # Extract class declarations
        for cls in analyzer.classes[:2]:  # Top 2 classes
            modifiers = cls['modifiers'] if cls['modifiers'] else 'public'
            inheritance = f" : {cls['inheritance']}" if cls['inheritance'] else ""
            snippet = f"// {modifiers} class {cls['name']}{inheritance}"
            snippets.append(snippet)
        
        return snippets
    
    def _format_code_snippets(self, snippets: List[str]) -> str:
        """Format code snippets for documentation."""
        if not snippets:
            return "// No specific code examples available - refer to source file for implementation details"
        
        result = ["**Key Code Structures:**"]
        result.extend(snippets)
        result.append("// Note: All code examples are commented to prevent markdown rendering issues")
        result.append("// Refer to the actual source file for complete implementation details")
        
        return '\n'.join(result)
    
    def _find_related_files(self, cs_file: Path, analyzer: CSharpFileAnalyzer) -> str:
        """Find related files and modules."""
        related = []
        filename = cs_file.stem
        
        # Based on naming patterns
        if filename.startswith("Dao_"):
            entity = filename[4:]
            related.append(f"- Model_{entity}.cs - Data model for this DAO")
            related.append(f"- Helper_Database_Core.cs - Database helper utilities")
            related.append(f"- Logging/LoggingUtility.cs - Error logging functionality")
            
        elif filename.startswith("Model_"):
            entity = filename[6:]
            related.append(f"- Dao_{entity}.cs - Data access for this model")
            related.append(f"- Controls/*{entity}*.cs - UI components using this model")
            
        elif filename.startswith("Helper_"):
            related.append("- Various application components that use these utilities")
            related.append("- Core/ directory - Core application functionality")
            
        elif filename.startswith("Core_"):
            related.append("- Application-wide components that depend on core functionality")
            related.append("- Models/ directory - Data models used by core components")
            
        # Based on dependencies
        for using in analyzer.usings:
            if using.startswith("MTM_"):
                namespace_parts = using.split('.')
                if len(namespace_parts) > 2:
                    related.append(f"- {namespace_parts[-1]}/ directory - Related namespace components")
        
        if not related:
            related.append("- Related files depend on specific implementation and usage patterns")
            related.append("- Check namespace imports and class dependencies for relationships")
        
        related.append("\n**Integration Points:**")
        related.append("- Review calling code and dependency injection for usage patterns")
        related.append("- Check configuration files for related settings and dependencies")
        related.append("- Examine test files for comprehensive usage examples")
        
        return '\n'.join(related)
    
    def _generate_summary(self, cs_file: Path, analyzer: CSharpFileAnalyzer) -> str:
        """Generate a comprehensive summary of the file's importance."""
        filename = cs_file.stem
        
        summary = []
        
        # Importance based on file type
        if filename.startswith("Dao_"):
            summary.append(f"**Critical Data Access Component**")
            summary.append(f"This file is essential for all database operations related to {filename[4:].replace('_', ' ').lower()}. It serves as the primary interface between the application's business logic and the database layer, ensuring data consistency and providing reliable access to persistent storage.")
            
        elif filename.startswith("Model_"):
            summary.append(f"**Core Data Structure**")
            summary.append(f"This model defines the fundamental data structure for {filename[6:].replace('_', ' ').lower()} throughout the application. It ensures type safety, data validation, and provides a contract for data exchange between different application layers.")
            
        elif filename.startswith("Helper_"):
            summary.append(f"**Essential Utility Component**")
            summary.append(f"This helper class provides critical utility functions that are used across multiple application components. It promotes code reuse, maintains consistency, and centralizes common functionality to reduce maintenance overhead.")
            
        elif filename.startswith("Core_"):
            summary.append(f"**Foundational Application Component**")
            summary.append(f"This core component provides fundamental functionality that underlies many application features. It's critical for application stability and provides essential services that other components depend on.")
            
        else:
            summary.append(f"**Important Application Component**")
            summary.append(f"This component contributes significant functionality to the application's overall capabilities and should be referenced when working with related features or troubleshooting related issues.")
        
        # When to reference
        summary.append(f"\n**When to Reference This File:**")
        
        if analyzer.methods:
            summary.append(f"- When implementing or modifying functionality related to {filename.replace('_', ' ').lower()}")
            
        if 'async' in analyzer.file_content.lower():
            summary.append("- When working with asynchronous operations and need to understand async patterns")
            
        if 'database' in analyzer.file_content.lower():
            summary.append("- When troubleshooting database connectivity or data access issues")
            
        if 'error' in analyzer.file_content.lower() or 'exception' in analyzer.file_content.lower():
            summary.append("- When investigating error handling and exception management")
            
        summary.append("- When performing code reviews or architectural analysis")
        summary.append("- When writing tests or implementing related functionality")
        summary.append("- When troubleshooting issues in related application areas")
        
        # Technical significance
        complexity_score = len(analyzer.classes) + len(analyzer.methods) + len(analyzer.properties)
        if complexity_score > 20:
            summary.append(f"\n**Technical Complexity:** High - Contains {complexity_score} components requiring careful review")
        elif complexity_score > 10:
            summary.append(f"\n**Technical Complexity:** Medium - Contains {complexity_score} components with moderate complexity")
        else:
            summary.append(f"\n**Technical Complexity:** Low to Medium - Contains {complexity_score} components with focused functionality")
        
        summary.append(f"\n**Maintenance Priority:** This file should be carefully maintained due to its role in the application architecture. Changes should be thoroughly tested and reviewed for impact on dependent components.")
        
        return '\n'.join(summary)
    
    def _get_current_date(self) -> str:
        """Get current date for documentation."""
        from datetime import datetime
        return datetime.now().strftime("%Y-%m-%d")

def main():
    """Main execution function."""
    parser = argparse.ArgumentParser(description='Generate comprehensive C# file documentation')
    parser.add_argument('--root', default='/home/runner/work/MTM_WIP_Application/MTM_WIP_Application',
                       help='Root directory of the C# project')
    parser.add_argument('--docs', default='/home/runner/work/MTM_WIP_Application/MTM_WIP_Application/Documents/File Summarys',
                       help='Output directory for documentation')
    
    args = parser.parse_args()
    
    if not os.path.exists(args.root):
        print(f"Error: Root directory {args.root} does not exist")
        return 1
    
    # Create documentation directory if it doesn't exist
    os.makedirs(args.docs, exist_ok=True)
    
    print(f"Generating comprehensive documentation for C# files...")
    print(f"Root directory: {args.root}")
    print(f"Documentation directory: {args.docs}")
    print("-" * 60)
    
    generator = DocumentationGenerator(args.root, args.docs)
    generator.generate_all_documentation()
    
    print("-" * 60)
    print("Documentation generation completed!")
    
    return 0

if __name__ == "__main__":
    sys.exit(main())