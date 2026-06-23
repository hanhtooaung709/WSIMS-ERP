// layoutHelper.js

window.layoutManager = {
    storageKey: 'templateCustomizer-vertical-menu-template--LayoutCollapsed',

    BASE_CLASSES: [
        'light-style',
        'layout-navbar-fixed',
        'layout-compact',
        'layout-menu-fixed'
    ],

    COLLAPSED: 'layout-menu-collapsed',
    HOVER: 'layout-menu-hover',

    setState(additional = []) {
        const html = document.documentElement;
        html.className = [...this.BASE_CLASSES, ...additional].join(' ');
    },

    setExpanded() {
        this.setState();
        localStorage.setItem(this.storageKey, 'false');
    },

    setCollapsed() {
        this.setState([this.COLLAPSED]);
        localStorage.setItem(this.storageKey, 'true');
    },

    toggle() {
        const html = document.documentElement;
        const isCollapsed = html.classList.contains(this.COLLAPSED);
        isCollapsed ? this.setExpanded() : this.setCollapsed();
    },

    initFromStorage() {
        const collapsed = localStorage.getItem(this.storageKey) === 'true';
        collapsed ? this.setCollapsed() : this.setExpanded();
    },

    setupHover() {
        const aside = document.querySelector('aside');
        if (!aside) return;

        aside.addEventListener('mouseenter', () => {
            if (document.documentElement.classList.contains(this.COLLAPSED)) {
                document.documentElement.classList.add(this.HOVER);
            }
        });

        aside.addEventListener('mouseleave', () => {
            document.documentElement.classList.remove(this.HOVER);
        });
    }
};

window.layoutHelper = {
    initToggleState: function () {
        const toggleButton = document.querySelector('a.layout-menu-toggle.menu-link.text-large.ms-auto');
        const aside = document.querySelector('aside#layout-menu.layout-menu.menu-vertical.menu.bg-menu-theme');
        const html = document.documentElement;
        const storageKey = 'templateCustomizer-vertical-menu-template--LayoutCollapsed';

        if (!toggleButton || !aside) return;
        
        // Base classes that should always be present
        const baseClasses = ['light-style', 'layout-navbar-fixed', 'layout-compact', 'layout-menu-fixed'];

        // Initialize localStorage state if not set
        if (localStorage.getItem(storageKey) === null) {
            localStorage.setItem(storageKey, 'true'); // default to collapsed
        }

        // Apply initial state
        this.applyLayoutState(aside, html, storageKey, baseClasses);

        // Toggle on click
        toggleButton.addEventListener('click', () => {
            const isCollapsed = localStorage.getItem(storageKey) === 'true';
            localStorage.setItem(storageKey, (!isCollapsed).toString());
            this.applyLayoutState(aside, html, storageKey, baseClasses);
        });
    },

    applyLayoutState: function(aside, html, storageKey, baseClasses) {
        const isCollapsed = localStorage.getItem(storageKey) === 'true';

        // Toggle aside menu state
        if (isCollapsed) {
            aside.classList.remove('expand');
        } else {
            aside.classList.add('expand');
        }

        // Set HTML classes
        html.className = ''; // Clear all classes
        baseClasses.forEach(cls => html.classList.add(cls));

        // Add collapsed class if needed
        if (isCollapsed) {
            html.classList.add('layout-menu-collapsed');
        }
    }
};

// Initialize the layout helper
window.layoutHelper.initToggleState();

window.passwordToggle = {
    init: function () {
        var elements = document.querySelectorAll(".form-password-toggle i");
        if (elements) {
            elements.forEach(function (e) {
                e.addEventListener("click", function (t) {
                    t.preventDefault();
                    var container = e.closest(".form-password-toggle"),
                        icon = container.querySelector("i"),
                        input = container.querySelector("input");

                    if (input.getAttribute("type") === "text") {
                        input.setAttribute("type", "password");
                        icon.classList.replace("ri-eye-line", "ri-eye-off-line");
                    } else if (input.getAttribute("type") === "password") {
                        input.setAttribute("type", "text");
                        icon.classList.replace("ri-eye-off-line", "ri-eye-line");
                    }
                });
            });
        }
    }
};