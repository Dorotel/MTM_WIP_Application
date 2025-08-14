/**
 * MTM Inventory Application - Help System Navigation
 * Enhanced search and navigation functionality for help system
 */

class HelpNavigation {
    constructor() {
        this.searchIndex = [];
        this.currentPage = '';
        this.searchResults = [];
        this.init();
    }

    init() {
        this.setupSearch();
        this.setupNavigation();
        this.setupKeyboardShortcuts();
        this.buildSearchIndex();
    }

    setupSearch() {
        const searchBox = document.getElementById('searchBox');
        if (searchBox) {
            searchBox.addEventListener('input', this.handleSearch.bind(this));
            searchBox.addEventListener('keydown', this.handleSearchKeydown.bind(this));
        }
    }

    setupNavigation() {
        // Set up breadcrumb navigation
        this.updateBreadcrumbs();
        
        // Set up back/forward navigation
        this.setupHistoryNavigation();
        
        // Set up page links with analytics
        document.addEventListener('click', this.handleLinkClick.bind(this));
    }

    setupKeyboardShortcuts() {
        document.addEventListener('keydown', (e) => {
            // Ctrl+F or F3 - Focus search
            if ((e.ctrlKey && e.key === 'f') || e.key === 'F3') {
                e.preventDefault();
                this.focusSearch();
            }
            
            // F1 - Go to main help
            if (e.key === 'F1') {
                e.preventDefault();
                this.goToMainHelp();
            }
            
            // Escape - Clear search
            if (e.key === 'Escape') {
                this.clearSearch();
            }
        });
    }

    handleSearch(e) {
        const searchTerm = e.target.value.toLowerCase().trim();
        
        if (searchTerm.length === 0) {
            this.clearSearchResults();
            return;
        }
        
        if (searchTerm.length < 2) {
            return; // Wait for at least 2 characters
        }
        
        const results = this.searchContent(searchTerm);
        this.displaySearchResults(results);
    }

    handleSearchKeydown(e) {
        if (e.key === 'Enter' && this.searchResults.length > 0) {
            e.preventDefault();
            // Navigate to first result
            window.location.href = this.searchResults[0].url;
        }
    }

    searchContent(searchTerm) {
        const results = [];
        const terms = searchTerm.split(/\s+/);
        
        // Search through indexed content
        for (const item of this.searchIndex) {
            let score = 0;
            let matches = 0;
            
            for (const term of terms) {
                if (item.title.toLowerCase().includes(term)) {
                    score += 10;
                    matches++;
                }
                if (item.content.toLowerCase().includes(term)) {
                    score += 5;
                    matches++;
                }
                if (item.keywords.some(keyword => keyword.toLowerCase().includes(term))) {
                    score += 7;
                    matches++;
                }
            }
            
            if (matches > 0) {
                results.push({
                    ...item,
                    score: score,
                    matches: matches,
                    relevance: (matches / terms.length) * 100
                });
            }
        }
        
        // Sort by relevance and score
        results.sort((a, b) => {
            if (a.relevance !== b.relevance) {
                return b.relevance - a.relevance;
            }
            return b.score - a.score;
        });
        
        return results.slice(0, 10); // Return top 10 results
    }

    displaySearchResults(results) {
        this.searchResults = results;
        
        // Remove existing results
        this.clearSearchResults();
        
        if (results.length === 0) {
            this.showNoResults();
            return;
        }
        
        // Create results container
        const resultsContainer = document.createElement('div');
        resultsContainer.id = 'searchResults';
        resultsContainer.className = 'search-results';
        
        const resultsTitle = document.createElement('h3');
        resultsTitle.textContent = `Found ${results.length} result${results.length !== 1 ? 's' : ''}`;
        resultsContainer.appendChild(resultsTitle);
        
        // Add results
        for (const result of results) {
            const resultItem = this.createResultItem(result);
            resultsContainer.appendChild(resultItem);
        }
        
        // Insert results after search box
        const searchBox = document.getElementById('searchBox');
        if (searchBox && searchBox.parentNode) {
            searchBox.parentNode.insertBefore(resultsContainer, searchBox.nextSibling);
        }
    }

    createResultItem(result) {
        const item = document.createElement('div');
        item.className = 'search-result-item';
        item.innerHTML = `
            <div class="search-result-header">
                <a href="${result.url}" class="search-result-title">${result.title}</a>
                <span class="search-result-relevance">${Math.round(result.relevance)}% match</span>
            </div>
            <div class="search-result-description">${result.description}</div>
            <div class="search-result-meta">
                <span class="search-result-category">${result.category}</span>
                <span class="search-result-url">${result.url}</span>
            </div>
        `;
        
        return item;
    }

    clearSearchResults() {
        const existingResults = document.getElementById('searchResults');
        if (existingResults) {
            existingResults.remove();
        }
        
        const noResults = document.getElementById('noSearchResults');
        if (noResults) {
            noResults.remove();
        }
    }

    showNoResults() {
        const noResultsDiv = document.createElement('div');
        noResultsDiv.id = 'noSearchResults';
        noResultsDiv.className = 'no-search-results';
        noResultsDiv.innerHTML = `
            <p>No results found. Try:</p>
            <ul>
                <li>Using different keywords</li>
                <li>Checking spelling</li>
                <li>Using more general terms</li>
                <li>Browsing the <a href="index.html">help categories</a></li>
            </ul>
        `;
        
        const searchBox = document.getElementById('searchBox');
        if (searchBox && searchBox.parentNode) {
            searchBox.parentNode.insertBefore(noResultsDiv, searchBox.nextSibling);
        }
    }

    focusSearch() {
        const searchBox = document.getElementById('searchBox');
        if (searchBox) {
            searchBox.focus();
            searchBox.select();
        }
    }

    clearSearch() {
        const searchBox = document.getElementById('searchBox');
        if (searchBox) {
            searchBox.value = '';
            this.clearSearchResults();
        }
    }

    goToMainHelp() {
        if (window.location.pathname !== '/index.html' && !window.location.pathname.endsWith('index.html')) {
            window.location.href = 'index.html';
        }
    }

    updateBreadcrumbs() {
        const breadcrumb = document.querySelector('.breadcrumb');
        if (!breadcrumb) return;
        
        const currentPath = window.location.pathname;
        const pathParts = currentPath.split('/').filter(part => part && part !== 'Help');
        const currentFile = pathParts[pathParts.length - 1] || 'index.html';
        
        // Build breadcrumb items
        const items = [
            { name: 'Help Home', url: 'index.html' }
        ];
        
        if (currentFile !== 'index.html') {
            const pageName = this.getPageTitle(currentFile);
            items.push({ name: pageName, url: currentFile, active: true });
        }
        
        // Update breadcrumb HTML
        breadcrumb.innerHTML = items.map(item => {
            if (item.active) {
                return `<li class="breadcrumb-item active">${item.name}</li>`;
            } else {
                return `<li class="breadcrumb-item"><a href="${item.url}">${item.name}</a></li>`;
            }
        }).join('');
    }

    getPageTitle(filename) {
        const titleMap = {
            'getting-started.html': 'Getting Started',
            'inventory-operations.html': 'Inventory Operations',
            'remove-operations.html': 'Remove Operations',
            'transfer-operations.html': 'Transfer Operations',
            'quickbuttons.html': 'QuickButtons',
            'advanced-features.html': 'Advanced Features',
            'settings-management.html': 'Settings Management',
            'transaction-history.html': 'Transaction History',
            'keyboard-shortcuts.html': 'Keyboard Shortcuts',
            'troubleshooting.html': 'Troubleshooting',
            'system-requirements.html': 'System Requirements',
            'database-configuration.html': 'Database Configuration',
            'error-handling.html': 'Error Handling',
            'development-tools.html': 'Development Tools'
        };
        
        return titleMap[filename] || filename.replace('.html', '').replace('-', ' ');
    }

    setupHistoryNavigation() {
        // Handle browser back/forward buttons
        window.addEventListener('popstate', () => {
            this.updateBreadcrumbs();
        });
    }

    handleLinkClick(e) {
        const link = e.target.closest('a');
        if (link && link.href) {
            // Log link clicks for analytics (if needed)
            this.logLinkClick(link.href, link.textContent);
        }
    }

    logLinkClick(url, text) {
        // Simple analytics logging - can be extended
        console.log(`Help Link Clicked: ${text} -> ${url}`);
        
        // Could send to analytics service if needed
        // analytics.track('help_link_click', { url, text, timestamp: new Date() });
    }

    buildSearchIndex() {
        // Pre-built search index - in a real implementation, 
        // this would be generated from actual page content
        this.searchIndex = [
            {
                title: 'Getting Started',
                url: 'getting-started.html',
                category: 'Getting Started',
                description: 'New user guide and application overview',
                content: 'getting started overview new user guide application setup installation',
                keywords: ['setup', 'installation', 'new user', 'overview', 'start']
            },
            {
                title: 'Inventory Operations',
                url: 'inventory-operations.html',
                category: 'Core Operations',
                description: 'Complete inventory management help',
                content: 'inventory management add items operations parts quantities locations',
                keywords: ['inventory', 'add', 'items', 'parts', 'quantities', 'stock']
            },
            {
                title: 'Remove Operations',
                url: 'remove-operations.html',
                category: 'Core Operations',
                description: 'Remove operations comprehensive guide',
                content: 'remove items operations delete inventory parts quantities batch',
                keywords: ['remove', 'delete', 'batch', 'operations', 'items']
            },
            {
                title: 'Transfer Operations',
                url: 'transfer-operations.html',
                category: 'Core Operations',
                description: 'Transfer operations detailed help',
                content: 'transfer operations move items locations quantities batch transfer',
                keywords: ['transfer', 'move', 'locations', 'operations', 'batch']
            },
            {
                title: 'QuickButtons',
                url: 'quickbuttons.html',
                category: 'Advanced Features',
                description: 'QuickButtons usage and customization',
                content: 'quickbuttons shortcuts recent items fast operations buttons',
                keywords: ['quickbuttons', 'shortcuts', 'fast', 'buttons', 'recent']
            },
            {
                title: 'Advanced Features',
                url: 'advanced-features.html',
                category: 'Advanced Features',
                description: 'Advanced inventory and remove operations',
                content: 'advanced features operations bulk excel import export templates',
                keywords: ['advanced', 'bulk', 'excel', 'import', 'export', 'templates']
            },
            {
                title: 'Settings Management',
                url: 'settings-management.html',
                category: 'Settings',
                description: 'Settings form comprehensive help',
                content: 'settings configuration preferences database users themes shortcuts',
                keywords: ['settings', 'configuration', 'preferences', 'users', 'themes']
            },
            {
                title: 'Transaction History',
                url: 'transaction-history.html',
                category: 'Advanced Features',
                description: 'Transaction history and reporting help',
                content: 'transaction history reports search filters export print analytics',
                keywords: ['transactions', 'history', 'reports', 'search', 'filters', 'export']
            },
            {
                title: 'Keyboard Shortcuts',
                url: 'keyboard-shortcuts.html',
                category: 'Reference',
                description: 'Complete keyboard shortcuts reference',
                content: 'keyboard shortcuts hotkeys keys combinations navigation operations',
                keywords: ['keyboard', 'shortcuts', 'hotkeys', 'keys', 'navigation']
            },
            {
                title: 'Troubleshooting',
                url: 'troubleshooting.html',
                category: 'Support',
                description: 'Common issues and solutions',
                content: 'troubleshooting issues problems solutions errors fixes help',
                keywords: ['troubleshooting', 'issues', 'problems', 'errors', 'fixes', 'help']
            },
            {
                title: 'System Requirements',
                url: 'system-requirements.html',
                category: 'Technical',
                description: 'System requirements and configuration',
                content: 'system requirements hardware software windows mysql database',
                keywords: ['system', 'requirements', 'hardware', 'software', 'windows', 'mysql']
            },
            {
                title: 'Database Configuration',
                url: 'database-configuration.html',
                category: 'Technical',
                description: 'Database setup and environment configuration',
                content: 'database configuration mysql setup connection environment server',
                keywords: ['database', 'mysql', 'setup', 'connection', 'server', 'configuration']
            },
            {
                title: 'Error Handling',
                url: 'error-handling.html',
                category: 'Technical',
                description: 'Error handling system usage guide',
                content: 'error handling messages debugging logs troubleshooting system',
                keywords: ['error', 'handling', 'messages', 'debugging', 'logs', 'troubleshooting']
            },
            {
                title: 'Development Tools',
                url: 'development-tools.html',
                category: 'Development',
                description: 'Developer tools and utilities',
                content: 'development tools utilities dependency charts converter viewer',
                keywords: ['development', 'tools', 'utilities', 'dependency', 'charts', 'converter']
            }
        ];
    }
}

// Initialize help navigation when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    window.helpNavigation = new HelpNavigation();
});

// Add search result styles dynamically
const searchStyles = `
    .search-results {
        background: white;
        border: 1px solid #dee2e6;
        border-radius: 8px;
        margin-top: 1rem;
        padding: 1rem;
        box-shadow: 0 2px 10px rgba(0,0,0,0.1);
    }
    
    .search-result-item {
        padding: 1rem;
        border-bottom: 1px solid #e9ecef;
        transition: background-color 0.3s ease;
    }
    
    .search-result-item:hover {
        background-color: #f8f9fa;
    }
    
    .search-result-item:last-child {
        border-bottom: none;
    }
    
    .search-result-header {
        display: flex;
        justify-content: space-between;
        align-items: flex-start;
        margin-bottom: 0.5rem;
    }
    
    .search-result-title {
        color: #0d6efd;
        text-decoration: none;
        font-weight: 600;
        font-size: 1.1rem;
    }
    
    .search-result-title:hover {
        color: #0a58ca;
        text-decoration: underline;
    }
    
    .search-result-relevance {
        background: #e7f3ff;
        color: #0d6efd;
        padding: 0.25rem 0.5rem;
        border-radius: 12px;
        font-size: 0.75rem;
        font-weight: 500;
    }
    
    .search-result-description {
        color: #495057;
        margin-bottom: 0.5rem;
        line-height: 1.4;
    }
    
    .search-result-meta {
        display: flex;
        gap: 1rem;
        font-size: 0.875rem;
        color: #6c757d;
    }
    
    .search-result-category {
        background: #f8f9fa;
        padding: 0.125rem 0.5rem;
        border-radius: 4px;
        font-weight: 500;
    }
    
    .no-search-results {
        background: #fff3cd;
        border: 1px solid #ffecb5;
        border-radius: 8px;
        padding: 1rem;
        margin-top: 1rem;
        color: #664d03;
    }
    
    .no-search-results p {
        margin-bottom: 0.5rem;
        font-weight: 500;
    }
    
    .no-search-results ul {
        margin-bottom: 0;
        padding-left: 1.5rem;
    }
    
    .no-search-results a {
        color: #0d6efd;
        text-decoration: none;
    }
    
    .no-search-results a:hover {
        text-decoration: underline;
    }
`;

// Inject styles
const styleSheet = document.createElement('style');
styleSheet.textContent = searchStyles;
document.head.appendChild(styleSheet);