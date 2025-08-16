// ================================================================================
// MTM INVENTORY APPLICATION - HELP NAVIGATION JAVASCRIPT
// ================================================================================

document.addEventListener('DOMContentLoaded', function () {
    // Initialize help navigation features
    initSmoothScrolling();
    initActiveNavigation();
    initSearchFunctionality();
    initAccessibilityFeatures();
    initAnimations();
    console.log('MTM Help Navigation initialized');
});

// Smooth scrolling for internal links
function initSmoothScrolling() {
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });

                // Update URL without jumping
                history.pushState(null, null, this.getAttribute('href'));
            }
        });
    });
}

// Highlight current section in navigation
function initActiveNavigation() {
    const sections = document.querySelectorAll('.content-section[id]');
    const navLinks = document.querySelectorAll('.quick-nav a[href^="#"], .footer-section a[href^="#"]');

    if (sections.length === 0 || navLinks.length === 0) return;

    function updateActiveNavigation() {
        let currentSection = '';
        const offset = 100;

        sections.forEach(section => {
            const rect = section.getBoundingClientRect();
            if (rect.top <= offset && rect.bottom >= offset) {
                currentSection = section.id;
            }
        });

        navLinks.forEach(link => {
            link.classList.remove('nav-active');
            if (link.getAttribute('href') === '#' + currentSection) {
                link.classList.add('nav-active');
            }
        });
    }

    // Throttle scroll events for better performance
    let ticking = false;
    window.addEventListener('scroll', function () {
        if (!ticking) {
            requestAnimationFrame(function () {
                updateActiveNavigation();
                ticking = false;
            });
            ticking = true;
        }
    });

    // Initial check
    updateActiveNavigation();
}

// Basic search functionality
function initSearchFunctionality() {
    // Add search functionality if search box exists
    const searchInput = document.querySelector('#search-input');
    if (searchInput) {
        searchInput.addEventListener('input', function (e) {
            const searchTerm = e.target.value.toLowerCase();
            const searchableElements = document.querySelectorAll('.content-section, .nav-card, .tool-card, .feature-card');

            searchableElements.forEach(element => {
                const text = element.textContent.toLowerCase();
                if (text.includes(searchTerm) || searchTerm === '') {
                    element.style.display = '';
                    element.classList.remove('search-hidden');
                } else {
                    element.style.display = 'none';
                    element.classList.add('search-hidden');
                }
            });
        });
    }
}

// Accessibility improvements
function initAccessibilityFeatures() {
    // Add keyboard navigation for cards
    const cards = document.querySelectorAll('.nav-card, .tool-card, .feature-card');
    cards.forEach(card => {
        // Make cards focusable
        if (!card.hasAttribute('tabindex')) {
            card.setAttribute('tabindex', '0');
        }

        // Add keyboard support
        card.addEventListener('keydown', function (e) {
            if (e.key === 'Enter' || e.key === ' ') {
                const link = card.querySelector('a');
                if (link) {
                    e.preventDefault();
                    link.click();
                }
            }
        });
    });

    // Add skip navigation link
    const skipLink = document.createElement('a');
    skipLink.href = '#main-content';
    skipLink.textContent = 'Skip to main content';
    skipLink.className = 'skip-link';
    skipLink.style.cssText = `
        position: absolute;
        top: -40px;
        left: 6px;
        background: #0d6efd;
        color: white;
        padding: 8px;
        text-decoration: none;
        border-radius: 4px;
        z-index: 1000;
        opacity: 0;
        transition: all 0.3s ease;
    `;

    skipLink.addEventListener('focus', function () {
        this.style.top = '6px';
        this.style.opacity = '1';
    });

    skipLink.addEventListener('blur', function () {
        this.style.top = '-40px';
        this.style.opacity = '0';
    });

    document.body.insertBefore(skipLink, document.body.firstChild);
}

// Initialize animations
function initAnimations() {
    // Add intersection observer for fade-in animations
    if ('IntersectionObserver' in window) {
        const observerOptions = {
            threshold: 0.1,
            rootMargin: '0px 0px -50px 0px'
        };

        const observer = new IntersectionObserver(function (entries) {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.classList.add('fade-in-up');
                    observer.unobserve(entry.target);
                }
            });
        }, observerOptions);

        // Observe elements for animation
        const animateElements = document.querySelectorAll('.content-section, .nav-card, .tool-card, .feature-card');
        animateElements.forEach(el => {
            observer.observe(el);
        });
    }
}

// Copy code to clipboard functionality
function copyToClipboard(text) {
    if (navigator.clipboard && window.isSecureContext) {
        return navigator.clipboard.writeText(text);
    } else {
        // Fallback for older browsers
        const textArea = document.createElement('textarea');
        textArea.value = text;
        textArea.style.position = 'fixed';
        textArea.style.left = '-999999px';
        textArea.style.top = '-999999px';
        document.body.appendChild(textArea);
        textArea.focus();
        textArea.select();

        return new Promise((resolve, reject) => {
            if (document.execCommand('copy')) {
                resolve();
            } else {
                reject();
            }
            document.body.removeChild(textArea);
        });
    }
}

// Add copy buttons to code blocks
document.addEventListener('DOMContentLoaded', function () {
    const codeBlocks = document.querySelectorAll('.command-examples, .code-block');

    codeBlocks.forEach(block => {
        const copyButton = document.createElement('button');
        copyButton.textContent = 'Copy';
        copyButton.className = 'copy-button';
        copyButton.style.cssText = `
            position: absolute;
            top: 8px;
            right: 8px;
            background: rgba(255, 255, 255, 0.2);
            border: 1px solid rgba(255, 255, 255, 0.3);
            color: white;
            padding: 4px 8px;
            border-radius: 4px;
            font-size: 12px;
            cursor: pointer;
            transition: all 0.3s ease;
        `;

        copyButton.addEventListener('mouseover', function () {
            this.style.background = 'rgba(255, 255, 255, 0.3)';
        });

        copyButton.addEventListener('mouseout', function () {
            this.style.background = 'rgba(255, 255, 255, 0.2)';
        });

        copyButton.addEventListener('click', function () {
            const text = block.textContent.replace(/^\$ /, ''); // Remove shell prompt
            copyToClipboard(text).then(() => {
                this.textContent = 'Copied!';
                setTimeout(() => {
                    this.textContent = 'Copy';
                }, 2000);
            }).catch(() => {
                this.textContent = 'Failed';
                setTimeout(() => {
                    this.textContent = 'Copy';
                }, 2000);
            });
        });

        block.style.position = 'relative';
        block.appendChild(copyButton);
    });
});

// Console greeting
console.log('%c🚀 MTM Inventory Application Help System', 'color: #0d6efd; font-size: 16px; font-weight: bold;');
console.log('%cDocumentation loaded successfully!', 'color: #28a745; font-weight: bold;');
