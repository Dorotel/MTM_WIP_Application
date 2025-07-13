#!/bin/bash

# Script to remove comments from the designer file

echo "Removing comments from Transactions.Designer.cs..."

cd /home/runner/work/MTM_WIP_Application/MTM_WIP_Application

# Create a temporary file
TEMP_FILE=$(mktemp)

# Process the file line by line, removing comment lines
while IFS= read -r line; do
    # Skip lines that start with // (after trimming whitespace)
    trimmed_line=$(echo "$line" | sed 's/^[[:space:]]*//')
    if [[ ! "$trimmed_line" =~ ^// ]]; then
        # Remove inline comments (// at end of line)
        cleaned_line=$(echo "$line" | sed 's/[[:space:]]*\/\/.*$//')
        echo "$cleaned_line" >> "$TEMP_FILE"
    fi
done < "Forms/Transactions/Transactions.Designer.cs"

# Replace the original file
mv "$TEMP_FILE" "Forms/Transactions/Transactions.Designer.cs"

echo "Comments removed from Transactions.Designer.cs"