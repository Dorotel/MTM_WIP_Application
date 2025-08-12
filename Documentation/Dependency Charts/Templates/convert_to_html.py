#!/usr/bin/env python3
"""
HTML Dependency Chart Generator
Converts markdown dependency charts to professional HTML visualizations
"""

import os
import re
import datetime
from pathlib import Path

class DependencyChartConverter:
    def __init__(self, base_path):
        self.base_path = Path(base_path)
        self.template_path = self.base_path / "Documentation/Dependency Charts/Templates/chart-template.html"
        self.charts_path = self.base_path / "Documentation/Dependency Charts"
        
    def load_template(self):
        """Load the HTML template"""
        with open(self.template_path, 'r', encoding='utf-8') as f:
            return f.read()
    
    def parse_markdown_chart(self, md_file_path):
        """Parse markdown file and extract data"""
        with open(md_file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        data = {
            'file_name': '',
            'file_path': '',
            'file_type': 'Unknown',
            'complexity': 'Medium',
            'priority': 'MEDIUM',
            'dependency_count': '0',
            'internal_dependencies': [],
            'external_dependencies': [],
            'compliance_items': [],
            'refactor_actions': []
        }
        
        # Extract file name and path
        file_match = re.search(r'# Dependency Chart: (.+)', content)
        if file_match:
            data['file_name'] = file_match.group(1)
        
        path_match = re.search(r'\*\*File Path\*\*: `(.+)`', content)
        if path_match:
            data['file_path'] = path_match.group(1)
        
        # Extract file type
        type_match = re.search(r'- \*\*Type\*\*: (.+)', content)
        if type_match:
            data['file_type'] = type_match.group(1)
        
        # Extract complexity
        complexity_match = re.search(r'- \*\*Complexity\*\*: (.+)', content)
        if complexity_match:
            data['complexity'] = complexity_match.group(1)
        
        # Extract priority
        priority_match = re.search(r'- \*\*Priority\*\*: (.+)', content)
        if priority_match:
            data['priority'] = priority_match.group(1)
        
        # Extract internal dependencies
        internal_section = re.search(r'### Internal Dependencies\n(.*?)### External Dependencies', content, re.DOTALL)
        if internal_section:
            internal_deps = re.findall(r'- ✅ `([^`]+)`', internal_section.group(1))
            data['internal_dependencies'] = internal_deps
        
        # Extract external dependencies  
        external_section = re.search(r'### External Dependencies\n(.*?)## Direct Dependents', content, re.DOTALL)
        if external_section:
            external_deps = re.findall(r'- 📦 `([^`]+)`', external_section.group(1))
            data['external_dependencies'] = external_deps
        
        # Extract compliance status
        compliance_section = re.search(r'## Compliance Status\n(.*?)## Refactor Priority', content, re.DOTALL)
        if compliance_section:
            # Look for compliance items
            compliance_items = []
            fail_items = re.findall(r'- \*\*([^*]+)\*\*: FAIL - (.+)', compliance_section.group(1))
            for item, desc in fail_items:
                compliance_items.append(('fail', item, desc))
            
            analyze_items = re.findall(r'- \*\*([^*]+)\*\*: TO_ANALYZE - (.+)', compliance_section.group(1))
            for item, desc in analyze_items:
                compliance_items.append(('analyze', item, desc))
                
            data['compliance_items'] = compliance_items
        
        # Extract refactor actions
        actions_section = re.search(r'## Refactor Actions Required\n(.*?)(?=##|$)', content, re.DOTALL)
        if actions_section:
            actions = re.findall(r'\d+\. \*\*(.+?)\*\*', actions_section.group(1))
            data['refactor_actions'] = actions
        
        data['dependency_count'] = str(len(data['internal_dependencies']) + len(data['external_dependencies']))
        
        return data
    
    def render_html(self, template, data):
        """Render HTML from template and data"""
        html = template
        
        # Basic substitutions
        html = html.replace('{{FILE_NAME}}', data['file_name'])
        html = html.replace('{{FILE_PATH}}', data['file_path'])
        html = html.replace('{{GENERATION_DATE}}', datetime.datetime.now().strftime('%Y-%m-%d %H:%M:%S UTC'))
        html = html.replace('{{FILE_TYPE}}', data['file_type'])
        html = html.replace('{{COMPLEXITY}}', data['complexity'])
        html = html.replace('{{PRIORITY}}', data['priority'])
        html = html.replace('{{PRIORITY_CLASS}}', data['priority'].lower())
        html = html.replace('{{DEPENDENCY_COUNT}}', data['dependency_count'])
        
        # Internal dependencies
        internal_deps_html = ""
        for dep in data['internal_dependencies']:
            internal_deps_html += f'<div class="dependency-item"><span class="icon">✅</span>{dep}</div>\n'
        html = html.replace('{{INTERNAL_DEPENDENCIES}}', internal_deps_html)
        
        # External dependencies  
        external_deps_html = ""
        for dep in data['external_dependencies']:
            external_deps_html += f'<div class="dependency-item external"><span class="icon">📦</span>{dep}</div>\n'
        html = html.replace('{{EXTERNAL_DEPENDENCIES}}', external_deps_html)
        
        # Compliance items
        compliance_html = ""
        for status, item, desc in data['compliance_items']:
            icon = "❌" if status == 'fail' else "⚠️"
            compliance_html += f'''
            <div class="status-item {status}">
                <span class="status-icon">{icon}</span>
                <div>
                    <strong>{item}</strong><br>
                    <small>{desc}</small>
                </div>
            </div>
            '''
        html = html.replace('{{COMPLIANCE_ITEMS}}', compliance_html)
        
        # Refactor actions
        actions_html = ""
        for action in data['refactor_actions']:
            actions_html += f'<li>{action}</li>\n'
        html = html.replace('{{REFACTOR_ACTIONS}}', actions_html)
        
        return html
    
    def convert_file(self, md_file_path, output_dir):
        """Convert a single markdown file to HTML"""
        template = self.load_template()
        data = self.parse_markdown_chart(md_file_path)
        html = self.render_html(template, data)
        
        # Create output path
        rel_path = md_file_path.relative_to(self.charts_path)
        html_path = output_dir / rel_path.with_suffix('.html')
        
        # Create directory if needed
        html_path.parent.mkdir(parents=True, exist_ok=True)
        
        # Write HTML file
        with open(html_path, 'w', encoding='utf-8') as f:
            f.write(html)
        
        return html_path
    
    def convert_all_charts(self):
        """Convert all markdown charts to HTML"""
        output_dir = self.charts_path / "HTML"
        output_dir.mkdir(exist_ok=True)
        
        # Find all .md files in the charts directory (except templates and README)
        md_files = []
        for md_file in self.charts_path.rglob("*.md"):
            if "Templates" not in str(md_file) and md_file.name != "README.md" and md_file.name != "ANALYSIS_REPORT.md":
                md_files.append(md_file)
        
        converted_files = []
        for md_file in md_files:
            try:
                html_path = self.convert_file(md_file, output_dir)
                converted_files.append(html_path)
                print(f"✅ Converted: {md_file.name}")
            except Exception as e:
                print(f"❌ Error converting {md_file.name}: {e}")
        
        return converted_files

def main():
    base_path = "/home/runner/work/MTM_WIP_Application/MTM_WIP_Application"
    converter = DependencyChartConverter(base_path)
    
    print("🚀 Starting HTML chart conversion...")
    converted_files = converter.convert_all_charts()
    
    print(f"\n✅ Conversion complete! Generated {len(converted_files)} HTML charts")
    print(f"📁 Output directory: {converter.charts_path / 'HTML'}")

if __name__ == "__main__":
    main()