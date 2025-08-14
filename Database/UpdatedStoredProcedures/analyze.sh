#!/bin/bash
# ================================================================================
# MTM STORED PROCEDURE COMPREHENSIVE ANALYSIS AND VERIFICATION SCRIPT
# ================================================================================
# File: analyze_stored_procedures.sh
# Purpose: Analyze all stored procedures for schema compliance and issues
# Created: August 13, 2025
# ================================================================================

echo "==================================================================================="
echo "MTM INVENTORY APPLICATION - STORED PROCEDURE ANALYSIS"
echo "==================================================================================="
echo "Analysis Date: $(date)"
echo ""

# Define paths
DB_PATH="/home/runner/work/MTM_WIP_Application/MTM_WIP_Application/Database"
PROCEDURES_PATH="$DB_PATH/UpdatedStoredProcedures"
SCHEMA_FILE="$DB_PATH/UpdatedDatabase/LiveDatabase.sql"

# Create analysis report
REPORT_FILE="/tmp/stored_procedure_analysis_report.txt"
echo "STORED PROCEDURE ANALYSIS REPORT" > $REPORT_FILE
echo "Generated: $(date)" >> $REPORT_FILE
echo "=================================" >> $REPORT_FILE
echo "" >> $REPORT_FILE

echo "Phase 1: Stored Procedure File Inventory"
echo "========================================="

# Count procedures in each file
total_procedures=0
for file in $PROCEDURES_PATH/*.sql; do
    if [ -f "$file" ]; then
        filename=$(basename "$file")
        proc_count=$(grep -c "CREATE PROCEDURE" "$file" 2>/dev/null)
        if [ -z "$proc_count" ]; then
            proc_count=0
        fi
        total_procedures=$((total_procedures + proc_count))
        echo "$filename: $proc_count procedures"
        echo "$filename: $proc_count procedures" >> $REPORT_FILE
    fi
done

echo ""
echo "Total Stored Procedures Found: $total_procedures"
echo "Total Stored Procedures Found: $total_procedures" >> $REPORT_FILE
echo "" >> $REPORT_FILE

echo ""
echo "Phase 2: Database Schema Analysis"  
echo "=================================="

# Extract table structure from LiveDatabase.sql
echo "CORE TABLE STRUCTURES FROM LIVEDATABASE.SQL" >> $REPORT_FILE
echo "============================================" >> $REPORT_FILE

# Extract inv_inventory table structure
echo "Analyzing inv_inventory table structure..."
if grep -A 20 "CREATE TABLE.*inv_inventory" "$SCHEMA_FILE" > /tmp/inv_inventory_schema.txt 2>/dev/null; then
    echo "✓ inv_inventory table found"
    echo "" >> $REPORT_FILE
    echo "inv_inventory table columns:" >> $REPORT_FILE
    grep -E "ID|PartID|Location|Operation|Quantity|ItemType|ReceiveDate|LastUpdated|User|BatchNumber|Notes" /tmp/inv_inventory_schema.txt | sed 's/^\s*/  /' >> $REPORT_FILE 2>/dev/null || echo "  Schema extraction incomplete" >> $REPORT_FILE
else
    echo "⚠ inv_inventory table not found in schema"
fi

# Extract inv_transaction table structure  
echo "Analyzing inv_transaction table structure..."
if grep -A 20 "CREATE TABLE.*inv_transaction" "$SCHEMA_FILE" > /tmp/inv_transaction_schema.txt 2>/dev/null; then
    echo "✓ inv_transaction table found"
    echo "" >> $REPORT_FILE
    echo "inv_transaction table columns:" >> $REPORT_FILE
    grep -E "ID|TransactionType|BatchNumber|PartID|FromLocation|ToLocation|Operation|Quantity|Notes|User|ItemType|ReceiveDate" /tmp/inv_transaction_schema.txt | sed 's/^\s*/  /' >> $REPORT_FILE 2>/dev/null || echo "  Schema extraction incomplete" >> $REPORT_FILE
else
    echo "⚠ inv_transaction table not found in schema"
fi

# Extract app_themes table structure
echo "Analyzing app_themes table structure..."
if grep -A 10 "CREATE TABLE.*app_themes" "$SCHEMA_FILE" > /tmp/app_themes_schema.txt 2>/dev/null; then
    echo "✓ app_themes table found"
    echo "" >> $REPORT_FILE
    echo "app_themes table columns:" >> $REPORT_FILE
    grep -E "ThemeName|SettingsJson" /tmp/app_themes_schema.txt | sed 's/^\s*/  /' >> $REPORT_FILE 2>/dev/null || echo "  Schema extraction incomplete" >> $REPORT_FILE
else
    echo "⚠ app_themes table not found in schema"
fi

echo "" >> $REPORT_FILE

echo ""
echo "Phase 3: Column Name Verification"
echo "=================================="

echo "COLUMN NAME VERIFICATION RESULTS" >> $REPORT_FILE
echo "================================" >> $REPORT_FILE

# Common column name issues to check for
declare -A common_issues=(
    ["FullName"]="Full Name"
    ["TransactionDate"]="ReceiveDate"
    ["DateTime"]="ReceiveDate"
    ["TimeStamp"]="ReceiveDate"
    ["FirstName"]="First_Name"
    ["LastName"]="Last_Name"
    ["UserName"]="User_Name"
)

echo "Checking for common column name mismatches..."
for file in $PROCEDURES_PATH/*.sql; do
    if [ -f "$file" ]; then
        filename=$(basename "$file")
        echo "Analyzing $filename..." >> $REPORT_FILE
        
        # Check for each common issue
        for wrong_col in "${!common_issues[@]}"; do
            if grep -q "$wrong_col" "$file"; then
                correct_col="${common_issues[$wrong_col]}"
                line_numbers=$(grep -n "$wrong_col" "$file" | cut -d: -f1 | tr '\n' ',' | sed 's/,$//')
                echo "  ⚠ ISSUE: $filename contains '$wrong_col' (should be '$correct_col') at lines: $line_numbers"
                echo "  ISSUE: $filename contains '$wrong_col' (should be '$correct_col') at lines: $line_numbers" >> $REPORT_FILE
            fi
        done
        
        # Check for potential column references that don't match known schema
        # Look for INSERT/UPDATE/SELECT statements
        suspect_columns=$(grep -E "(INSERT INTO|UPDATE|SELECT.*FROM)" "$file" | grep -oE "\b[A-Za-z_][A-Za-z0-9_]*\b" | sort | uniq | grep -E "^[A-Z]")
        if [ ! -z "$suspect_columns" ]; then
            echo "  Potential column references found in $filename"
        fi
        
        echo "" >> $REPORT_FILE
    fi
done

echo ""
echo "Phase 4: Error Handling Pattern Analysis"
echo "========================================"

echo "ERROR HANDLING PATTERN ANALYSIS" >> $REPORT_FILE  
echo "===============================" >> $REPORT_FILE

# Check each file for proper error handling pattern
echo "Checking error handling patterns..."
for file in $PROCEDURES_PATH/*.sql; do
    if [ -f "$file" ]; then
        filename=$(basename "$file")
        
        # Count procedures with proper error handling
        procedures_with_exit_handler=$(grep -c "DECLARE EXIT HANDLER FOR SQLEXCEPTION" "$file" 2>/dev/null || echo 0)
        procedures_with_out_params=$(grep -c "OUT.*p_Status.*INT" "$file" 2>/dev/null || echo 0)
        procedures_with_error_msg=$(grep -c "OUT.*p_ErrorMsg" "$file" 2>/dev/null || echo 0)
        total_procedures_in_file=$(grep -c "CREATE PROCEDURE" "$file" 2>/dev/null || echo 0)
        
        echo "$filename Error Handling Summary:" >> $REPORT_FILE
        echo "  Total Procedures: $total_procedures_in_file" >> $REPORT_FILE
        echo "  With EXIT HANDLER: $procedures_with_exit_handler" >> $REPORT_FILE  
        echo "  With Status OUT param: $procedures_with_out_params" >> $REPORT_FILE
        echo "  With Error Message OUT param: $procedures_with_error_msg" >> $REPORT_FILE
        
        if [ $total_procedures_in_file -gt 0 ]; then
            if [ $procedures_with_exit_handler -eq $total_procedures_in_file ] && 
               [ $procedures_with_out_params -eq $total_procedures_in_file ] &&
               [ $procedures_with_error_msg -eq $total_procedures_in_file ]; then
                echo "  ✓ COMPLIANT: All procedures have proper error handling"
                echo "  STATUS: COMPLIANT" >> $REPORT_FILE
            else
                echo "  ⚠ NON-COMPLIANT: Some procedures missing proper error handling"
                echo "  STATUS: NON-COMPLIANT" >> $REPORT_FILE
            fi
        fi
        
        echo "" >> $REPORT_FILE
    fi
done

echo ""
echo "Phase 5: MySQL 5.7.24 Compatibility Check"  
echo "=========================================="

echo "MYSQL 5.7.24 COMPATIBILITY CHECK" >> $REPORT_FILE
echo "=================================" >> $REPORT_FILE

# Check for MySQL version compatibility issues
echo "Checking for MySQL compatibility issues..."
for file in $PROCEDURES_PATH/*.sql; do
    if [ -f "$file" ]; then
        filename=$(basename "$file")
        
        # Check for MySQL 8.0+ specific features that won't work in 5.7.24
        issues_found=0
        
        # Check for CTE (Common Table Expressions) - MySQL 8.0+
        if grep -q "WITH.*AS" "$file"; then
            echo "  ⚠ $filename: Uses CTE (WITH clause) - MySQL 8.0+ feature"
            echo "  ISSUE: $filename uses CTE (WITH clause) - MySQL 8.0+ feature" >> $REPORT_FILE
            issues_found=$((issues_found + 1))
        fi
        
        # Check for window functions - MySQL 8.0+  
        if grep -qE "(ROW_NUMBER|RANK|DENSE_RANK|LAG|LEAD)\s*\(" "$file"; then
            echo "  ⚠ $filename: Uses window functions - MySQL 8.0+ feature"
            echo "  ISSUE: $filename uses window functions - MySQL 8.0+ feature" >> $REPORT_FILE
            issues_found=$((issues_found + 1))
        fi
        
        # Check for JSON functions compatibility
        if grep -qE "JSON_(EXTRACT|UNQUOTE|SEARCH|CONTAINS)" "$file"; then
            echo "  ⚠ $filename: Uses JSON functions - verify 5.7.24 compatibility"
            echo "  WARNING: $filename uses JSON functions - verify 5.7.24 compatibility" >> $REPORT_FILE
            issues_found=$((issues_found + 1))
        fi
        
        if [ $issues_found -eq 0 ]; then
            echo "  ✓ $filename: MySQL 5.7.24 compatible"
            echo "  STATUS: $filename - MySQL 5.7.24 compatible" >> $REPORT_FILE
        fi
        
        echo "" >> $REPORT_FILE
    fi
done

echo ""
echo "Phase 6: Generate Fix Recommendations"
echo "===================================="

echo "FIX RECOMMENDATIONS" >> $REPORT_FILE
echo "===================" >> $REPORT_FILE
echo "" >> $REPORT_FILE

echo "Based on the analysis, here are the recommended fixes:" >> $REPORT_FILE
echo "" >> $REPORT_FILE
echo "1. COLUMN NAME FIXES:" >> $REPORT_FILE  
echo "   - Update any references to 'FullName' to match actual schema" >> $REPORT_FILE
echo "   - Ensure 'TransactionDate' references use 'ReceiveDate' from actual tables" >> $REPORT_FILE
echo "   - Verify all column names match LiveDatabase.sql exactly" >> $REPORT_FILE
echo "" >> $REPORT_FILE

echo "2. ERROR HANDLING STANDARDIZATION:" >> $REPORT_FILE
echo "   - All procedures should have 'DECLARE EXIT HANDLER FOR SQLEXCEPTION'" >> $REPORT_FILE  
echo "   - All procedures should have 'OUT p_Status INT' parameter" >> $REPORT_FILE
echo "   - All procedures should have 'OUT p_ErrorMsg VARCHAR(255)' parameter" >> $REPORT_FILE
echo "   - Follow the pattern used in inv_inventory_Remove_Item_1_1" >> $REPORT_FILE
echo "" >> $REPORT_FILE

echo "3. MYSQL 5.7.24 COMPATIBILITY:" >> $REPORT_FILE
echo "   - Replace any MySQL 8.0+ specific features with 5.7.24 alternatives" >> $REPORT_FILE
echo "   - Test all JSON operations for 5.7.24 compatibility" >> $REPORT_FILE
echo "   - Avoid window functions, use traditional GROUP BY approaches" >> $REPORT_FILE
echo "" >> $REPORT_FILE

echo ""
echo "Analysis Complete!"
echo "=================="
echo ""
echo "Full report saved to: $REPORT_FILE"
echo ""
echo "Next Steps:"
echo "1. Review the analysis report"
echo "2. Apply recommended fixes to stored procedure files" 
echo "3. Run the verification system (00_StoredProcedure_Verification_System.sql)"
echo "4. Test all procedures against LiveDatabase.sql"
echo ""

# Display report summary
echo "Report Summary:"
echo "==============="
echo "Total Procedures Analyzed: $total_procedures"
echo "Schema Files Checked: $(find $PROCEDURES_PATH -name "*.sql" | wc -l)"
echo "Analysis Report Location: $REPORT_FILE"
echo ""

# Cleanup temporary files
rm -f /tmp/inv_inventory_schema.txt /tmp/inv_transaction_schema.txt /tmp/app_themes_schema.txt

echo "Analysis complete. Review the report for detailed findings and recommendations."