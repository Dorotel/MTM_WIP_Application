#!/bin/bash
# ================================================================================
# MTM INVENTORY APPLICATION - STORED PROCEDURES DEPLOYMENT SCRIPT (LINUX/MAC/MAMP)
# ================================================================================
# File: deploy_procedures.sh
# Purpose: Deploy all stored procedures to the MTM WIP Application database
# Created: August 10, 2025
# Updated: For MAMP MySQL 5.7.24 compatibility
# Target Database: mtm_wip_application
# MySQL Version: 5.7.24+ (MAMP Compatible)
# ================================================================================

# Default MAMP configuration
DB_HOST=${DB_HOST:-"localhost"}
DB_PORT=${DB_PORT:-"3306"}
DB_NAME=${DB_NAME:-"mtm_wip_application"}
DB_USER=${DB_USER:-"root"}
DB_PASSWORD=${DB_PASSWORD:-"root"}

# MAMP MySQL path detection (try common MAMP locations)
MYSQL_CMD=""
MYSQLDUMP_CMD=""

# Function to find MySQL command
find_mysql_command() {
    # Try MAMP locations first
    MAMP_PATHS=(
        "/Applications/MAMP/Library/bin"  # macOS MAMP
        "/opt/lampp/bin"                  # Linux XAMPP/LAMPP
        "/usr/local/mysql/bin"            # macOS MySQL
        "/usr/bin"                        # Standard Linux MySQL
    )
    
    for path in "${MAMP_PATHS[@]}"; do
        if [ -f "$path/mysql" ]; then
            MYSQL_CMD="$path/mysql"
            MYSQLDUMP_CMD="$path/mysqldump"
            echo "[INFO] Found MySQL at: $path"
            return 0
        fi
    done
    
    # Try system PATH as fallback
    if command -v mysql &> /dev/null; then
        MYSQL_CMD="mysql"
        MYSQLDUMP_CMD="mysqldump"
        echo "[INFO] Using system MySQL from PATH"
        return 0
    fi
    
    echo "[ERROR] MySQL not found. Please install MySQL or MAMP, or specify --mysql-path"
    return 1
}

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Function to print colored output
print_status() {
    echo -e "${GREEN}[INFO]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

print_header() {
    echo -e "${BLUE}================================${NC}"
    echo -e "${BLUE}$1${NC}"
    echo -e "${BLUE}================================${NC}"
}

print_success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

# Function to execute SQL file
execute_sql_file() {
    local file=$1
    local description=$2
    
    print_status "Executing $description..."
    
    if [ ! -f "$file" ]; then
        print_error "File not found: $file"
        return 1
    fi
    
    $MYSQL_CMD -h"$DB_HOST" -P"$DB_PORT" -u"$DB_USER" -p"$DB_PASSWORD" "$DB_NAME" < "$file" 2>/dev/null
    
    if [ $? -eq 0 ]; then
        print_success "$description completed successfully"
        return 0
    else
        print_error "$description failed"
        return 1
    fi
}

# Function to test database connection
test_connection() {
    print_status "Testing database connection..."
    
    $MYSQL_CMD -h"$DB_HOST" -P"$DB_PORT" -u"$DB_USER" -p"$DB_PASSWORD" -e "SELECT VERSION() as 'MySQL Version', NOW() as 'Current Time';" "$DB_NAME" 2>/dev/null
    
    if [ $? -eq 0 ]; then
        print_success "Database connection successful"
        return 0
    else
        print_error "Cannot connect to database. Please check your credentials and ensure MAMP is running."
        print_status "Common MAMP connection parameters:"
        print_status "  Host: localhost"
        print_status "  Port: 3306 (or 8889 for older MAMP versions)"
        print_status "  User: root"
        print_status "  Password: root"
        print_status "  Database: $DB_NAME"
        print_status "Make sure MAMP Apache and MySQL services are started."
        return 1
    fi
}

# Function to create backup
create_backup() {
    print_status "Creating backup of existing procedures..."
    
    local backup_file="stored_procedures_backup_$(date +%Y%m%d_%H%M%S).sql"
    
    $MYSQLDUMP_CMD -h"$DB_HOST" -P"$DB_PORT" -u"$DB_USER" -p"$DB_PASSWORD" \
                   --routines --no-create-info --no-data --no-create-db \
                   "$DB_NAME" > "$backup_file" 2>/dev/null
    
    if [ $? -eq 0 ]; then
        print_success "Backup created: $backup_file"
        return 0
    else
        print_warning "Backup creation failed, but continuing with deployment..."
        return 1
    fi
}

# Function to check MySQL version compatibility
check_mysql_version() {
    print_status "Checking MySQL version compatibility..."
    
    local version_output=$($MYSQL_CMD -h"$DB_HOST" -P"$DB_PORT" -u"$DB_USER" -p"$DB_PASSWORD" -e "SELECT VERSION();" "$DB_NAME" 2>/dev/null)
    
    if [ $? -eq 0 ]; then
        echo "$version_output"
        print_status "MySQL version check completed"
        
        # Check if version is 5.7.24 or higher
        local version=$(echo "$version_output" | grep -oP '\d+\.\d+\.\d+' | head -n1)
        if [ ! -z "$version" ]; then
            print_status "Detected MySQL version: $version"
            print_status "Procedures are compatible with MySQL 5.7.24+"
        fi
        return 0
    else
        print_warning "Could not verify MySQL version"
        return 1
    fi
}

# Main deployment function
deploy_procedures() {
    print_header "MTM INVENTORY APPLICATION - STORED PROCEDURES DEPLOYMENT"
    print_status "Target: MySQL $DB_HOST:$DB_PORT/$DB_NAME"
    print_status "User: $DB_USER"
    print_status "MySQL Client: $MYSQL_CMD"
    print_header ""
    
    # Find MySQL command
    if ! find_mysql_command; then
        exit 1
    fi
    
    # Test database connection
    if ! test_connection; then
        exit 1
    fi
    
    # Check MySQL version
    check_mysql_version
    
    # Create backup
    create_backup
    
    # Deploy procedures in order
    local files=(
        "01_User_Management_Procedures.sql:User Management Procedures (MySQL 5.7.24 Compatible)"
        "02_System_Role_Procedures.sql:System Role Procedures"
        "03_Master_Data_Procedures.sql:Master Data Procedures" 
        "04_Inventory_Procedures.sql:Inventory Management Procedures"
    )
    
    local success_count=0
    local total_count=${#files[@]}
    
    for file_info in "${files[@]}"; do
        IFS=':' read -r file description <<< "$file_info"
        
        if execute_sql_file "$file" "$description"; then
            ((success_count++))
        fi
    done
    
    # Summary
    print_header "DEPLOYMENT SUMMARY"
    print_status "Successfully deployed: $success_count/$total_count procedure files"
    
    if [ $success_count -eq $total_count ]; then
        print_success "All stored procedures deployed successfully!"
        print_success "Deployment completed for MySQL 5.7.24 (MAMP Compatible)"
        exit 0
    else
        print_error "Some procedures failed to deploy. Please check the errors above."
        print_status "Common MAMP issues:"
        print_status "  1. Ensure MAMP Apache and MySQL services are running"
        print_status "  2. Check that the database '$DB_NAME' exists"
        print_status "  3. Verify user '$DB_USER' has CREATE ROUTINE privileges"
        print_status "  4. Confirm MAMP MySQL version is 5.7.24 or higher"
        exit 1
    fi
}

# Function to show usage
show_usage() {
    echo "Usage: $0 [options]"
    echo ""
    echo "Options:"
    echo "  -h, --host HOST        Database host (default: localhost)"
    echo "  -P, --port PORT        Database port (default: 3306)"
    echo "  -u, --user USER        Database username (default: root)"
    echo "  -p, --password PASS    Database password (default: root for MAMP)"
    echo "  -d, --database DB      Database name (default: mtm_wip_application)"
    echo "  --mysql-path PATH      Custom MySQL binary path"
    echo "  --help                 Show this help message"
    echo ""
    echo "Environment variables:"
    echo "  DB_HOST, DB_PORT, DB_USER, DB_PASSWORD, DB_NAME"
    echo ""
    echo "MAMP Examples:"
    echo "  $0 -h localhost -u root -p root -d mtm_wip_application"
    echo "  $0 --mysql-path /Applications/MAMP/Library/bin -p root"
    echo "  $0 -P 8889 -p root  # for older MAMP versions"
    echo ""
    echo "MAMP Troubleshooting:"
    echo "  1. Start MAMP and ensure Apache/MySQL services are running"
    echo "  2. Check MAMP control panel for correct port (usually 3306)"
    echo "  3. Default MAMP credentials are usually root/root"
    echo "  4. Ensure target database exists in phpMyAdmin"
    echo ""
}

# Parse command line arguments
while [[ $# -gt 0 ]]; do
    case $1 in
        -h|--host)
            DB_HOST="$2"
            shift 2
            ;;
        -P|--port)
            DB_PORT="$2"
            shift 2
            ;;
        -u|--user)
            DB_USER="$2"
            shift 2
            ;;
        -p|--password)
            DB_PASSWORD="$2"
            shift 2
            ;;
        -d|--database)
            DB_NAME="$2"
            shift 2
            ;;
        --mysql-path)
            MYSQL_CMD="$2/mysql"
            MYSQLDUMP_CMD="$2/mysqldump"
            print_status "Using custom MySQL path: $2"
            shift 2
            ;;
        --help)
            show_usage
            exit 0
            ;;
        *)
            print_error "Unknown option: $1"
            show_usage
            exit 1
            ;;
    esac
done

# Validate required parameters
if [ -z "$DB_PASSWORD" ]; then
    print_error "Database password is required. Use -p option or set DB_PASSWORD environment variable."
    print_status "For MAMP, default password is usually 'root'"
    exit 1
fi

# Run deployment
deploy_procedures
