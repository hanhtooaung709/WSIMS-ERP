window.enableLoading = function (enable) {
    $('#spinner').css('display', enable ? 'flex' : 'none');
}

window.refreshPage = () => document.location.reload();

window.goTo = (url) => location.href = url;

window.initializePasswordToggle = function () {
    window.document.querySelectorAll(".form-password-toggle i").forEach((icon) => {
        icon.addEventListener("click", (event) => {
            event.preventDefault();

            // Find the closest .form-password-toggle container
            const passwordContainer = icon.closest(".form-password-toggle");

            // Get the input field within that container
            const passwordInput = passwordContainer.querySelector("input");

            // Toggle between "text" and "password" input types
            if (passwordInput.getAttribute("type") === "password") {
                passwordInput.setAttribute("type", "text");
                icon.classList.replace("ri-eye-off-line", "ri-eye-line"); // Toggle icon class
            } else {
                passwordInput.setAttribute("type", "password");
                icon.classList.replace("ri-eye-line", "ri-eye-off-line"); // Toggle icon class
            }
        });
    });
}

window.EnableBodyLoading = function () {
    const element = document.getElementById("bodyLoadingAnimation");
    if (!element) return;
    element.style.display = "flex";
}

window.formcomma = function () {
    const className = ".form-comma";

    $(document).off("keyup", className);

    $(document).on("keyup", className, function (event) {

        if (event.which >= 37 && event.which <= 40) return;

        let res = $(this).val().toDecimal();
        let finalRes = res.toString();

        if (finalRes.indexOf(".") === -1) {
            let checkLength = parseFloat(finalRes.replace(/,/g, ""));
            if (checkLength.toString().length > 14) {
                $(this).val(checkLength.toString().substring(0, 14).toDecimal());
                return false;
            }
        }

        $(this).val(finalRes);
    });
    
    $(className).attr("maxlength", "21");
    $(className).addClass("text-right");
}

String.prototype.toDecimal = function () {

    let value = this.replace(/,/g, "");

    if (value === "" || isNaN(value)) {
        return "";
    }

    return Number(value).toLocaleString("en-US");
};

window.numberonlyanddot = function () {
    const className = ".dotandnumber";
    $(className).on('input', function () {
        $(this).val($(this).val().replace(/[^0-9()\\.]/g, ''));
    });
}

window.FormCheck = function () {

    let checkboxes = $(".form-check-input");
    checkboxes.each(function () {
        if (!$(this).hasClass('checked')) {
            $(this).prop('checked', false);
        } else {
            $(this).prop('checked', true);
        }
    });

}

window.DisableBodyLoading = function () {
    const element = document.getElementById("bodyLoadingAnimation");
    if (!element) return;
    element.style.display = "none";
}

window.pagerSetting = function (pageCount, currentPageCount) {
    setTimeout(() => {
        let ulContainer = $('.rz-dropdown-items-wrapper ul');
        let label = $(".rz-pager .rz-dropdown-label");

        let items = ulContainer.find('li');
        let lastItem = items.last();


        if (lastItem.text() === currentPageCount) {
            label.text("All");
        }
        items.each(function () {
            let span = $(this).find('span');
            if (span.length > 0) {

                let spanText = span.text().trim();
                console.log(spanText);
                // Check if the spanText exists in pageSizeOptions
                if (pageCount === spanText) {

                    span.text("All");
                }
            }
        });

        let dropDownContainer = $('.rz-dropdown-items-wrapper');
        dropDownContainer.on('click', function (e) {

            if ($(e.target).is('span')) {

                let labelText = $(e.target).text();
                label.text(labelText);

            } else {
                let liTag = $(e.target);
                let spanTag = liTag.find("span");
                label.text(spanTag.text());
            }
        })
    }, 100);

}

// Exported function to toggle sidebar
window.toggleSidebar = function () {
    const sidebar = document.querySelector("#layout-menu");
    if (sidebar) {
        sidebar.classList.toggle("expand");
        console.log("Sidebar toggled");
    }
};

// Optional: also wire up DOM click if desired
document.addEventListener("DOMContentLoaded", function () {
    const toggleBtn = document.querySelector(".layout-menu-toggle");
    if (toggleBtn) {
        toggleBtn.addEventListener("click", window.toggleSidebar);
    }
});

window.addScript = function (lst) {
    lst.forEach(addScriptItem);

    function addScriptItem(url) {
        if (url.Length === 0) {
            console.error("Invalid source URL");
            return;
        }

        if (document.querySelector(`script[src="${url}"]`)) {
            console.log(`${url} already loaded.`);
            return;
        }

        let tag = document.createElement('script');
        tag.src = url;
        tag.type = "text/javascript";

        tag.onload = function () {
            console.log("Script loaded successfully");
        }

        tag.onerror = function () {
            console.error("Failed to load script");
        }

        document.body.appendChild(tag);
    }
}

window.initAsideHover = function () {
    const aside = document.querySelector('aside');

    if (aside) {
        aside.addEventListener('mouseenter', () => {
            document.documentElement.classList.add('layout-menu-hover');
        });

        aside.addEventListener('mouseleave', () => {
            document.documentElement.classList.remove('layout-menu-hover');
        });
    }
};

function applyWindowScrolledClass() {
    const layoutPage = document.querySelector('.layout-page');

    if (!layoutPage) return; // Exit if element not found

    if (window.scrollY > 0) {
        layoutPage.classList.add('window-scrolled');

        layoutPage.querySelectorAll(':scope > *').forEach(child => {
            child.classList.add('window-scrolled');
        });
    } else {
        layoutPage.classList.remove('window-scrolled');

        layoutPage.querySelectorAll(':scope > *').forEach(child => {
            child.classList.remove('window-scrolled');
        });
    }
}

// 🔁 Automatically apply on scroll
window.addEventListener('scroll', applyWindowScrolledClass);

// 🔄 Optional: invoke immediately on load
window.addEventListener('DOMContentLoaded', applyWindowScrolledClass);

window.passwordToggle = {
    init: function () {
        let elements = document.querySelectorAll(".form-password-toggle i");
        if (elements) {
            elements.forEach(function (e) {
                e.addEventListener("click", function (t) {
                    t.preventDefault();
                    let container = e.closest(".form-password-toggle"),
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

/**
 * Binds expand/collapse only for top-level menu groups (items with ul.menu-sub).
 * Does not attach to child submenu links — active/open state for navigation stays in Blazor.
 * Idempotent: safe to call from DOMContentLoaded and Blazor after each initMenuToggle interop.
 */
window.initMenuToggle = function () {
    document.querySelectorAll(".menu-inner > .menu-item").forEach((item) => {
        const subMenu = item.querySelector(":scope > ul.menu-sub");
        if (!subMenu) {
            return;
        }

        const toggle = item.querySelector(":scope > a.menu-link.menu-toggle");
        if (!toggle || toggle.dataset.menuToggleBound === "true") {
            return;
        }

        toggle.dataset.menuToggleBound = "true";
        toggle.addEventListener("click", function (e) {
            e.preventDefault();
            item.classList.toggle("open");
            item.classList.toggle("active");
        });
    });
};

/** Clears active/open on all sidebar menu items (e.g. after navigating home via logo). */
window.resetSidebarMenuState = function () {
    const root = document.querySelector("#layout-menu");
    if (!root) {
        return;
    }
    root.querySelectorAll(".menu-item").forEach((el) => {
        el.classList.remove("active", "open");
    });
};

document.addEventListener("DOMContentLoaded", function () {
    window.initMenuToggle();
})

/**
 * Filters the main sidebar tree (#sidebarMainMenu). Logout row stays visible while searching.
 * Clears filter: shows all items, removes search-only open state, restores open on active group.
 */
function searchMenu() {
    const $root = $('#sidebarMainMenu');
    if (!$root.length) {
        return;
    }

    const filter = ($('#searchMenuId').val() || '').toLowerCase().trim();

    if (!filter) {
        $root.find('li.menu-item').show();
        $root.children('li.menu-item[data-menu-filter-toggled]').each(function () {
            $(this).removeClass('open').removeAttr('data-menu-filter-toggled');
        });
        $root.children('li.menu-item.active').each(function () {
            const $li = $(this);
            if ($li.children('ul.menu-sub').length) {
                $li.addClass('open');
            }
        });
        return;
    }

    $root.children('li.menu-item').each(function () {
        const $row = $(this);
        const $sub = $row.children('ul.menu-sub');

        if (!$sub.length) {
            $row.show();
            return;
        }

        const parentText = $row.children('a.menu-link').first().text().toLowerCase().trim();
        const parentMatch = parentText.includes(filter);

        let anyChildMatch = false;
        $sub.children('li.menu-item').each(function () {
            const t = $(this).find('a.menu-link').first().text().toLowerCase().trim();
            if (t.includes(filter)) {
                anyChildMatch = true;
            }
        });

        const groupVisible = parentMatch || anyChildMatch;

        if (!groupVisible) {
            $row.hide();
            return;
        }

        $row.show();
        $row.addClass('open');
        $row.attr('data-menu-filter-toggled', '1');

        $sub.children('li.menu-item').each(function () {
            const $child = $(this);
            const t = $child.find('a.menu-link').first().text().toLowerCase().trim();
            const childMatch = t.includes(filter);
            if (parentMatch) {
                $child.show();
            } else {
                $child.toggle(childMatch);
            }
        });
    });
}

window.allowAlphaAndSpecialcharactersOnly = function () {
    const className = ".clsEngNumberAndSpecialOnly";
    if ($(className).length) {

        $(className).keypress(function (e) {
            //console.log(value);
            const regex = new RegExp("^[a-zA-Z0-9!@#$%^&*()_+=\\[{\\]};:<>|./?,-]+$");
            const key = String.fromCharCode(!e.charCode ? e.which : e.charCode);
            if (!regex.test(key)) {
                e.preventDefault();
                return false;
            }
        })

    }
}

window.onlyAlphaAndSpace = function () {
    const className = ".clsAlphaAndSpace";
    if ($(className).length) {
        $(className).on("input", function () {
            $(this).val($(this).val().replace(/[^a-zA-Z\s]/gi, ''));
        })
    }
}

window.onlyAlphaAndNumber = function () {
    const className = ".clsAlphaNoOnly";
    if ($(className).length) {
        $(className).on("input", function () {
            $(this).val($(this).val().replace(/[^a-zA-Z0-9]/gi, ''));
        })
    }
}

window.recursiveExpandTreeLabels = (labelList, maxAttempts = 5) => {
    function expandAndCheckMatchingLabels(labelList) {
        let expandedAny = false;

        document.querySelectorAll('.rz-treenode-label').forEach(label => {
            const labelText = label.textContent.trim();

            if (labelList.includes(labelText)) {
                const nodeContent = label.closest('.rz-treenode-content');
                if (!nodeContent) return;

                const toggler = nodeContent.querySelector('.rz-tree-toggler');
                const expander = nodeContent.nextElementSibling;

                // Expand node if collapsed
                if (toggler && toggler.classList.contains('rzi-caret-right')) {
                    toggler.click(); // simulate expansion
                    expandedAny = true;
                }

                if (expander && expander.classList.contains('rz-state-collapsed')) {
                    expander.classList.remove('rz-state-collapsed');
                    expander.classList.add('rz-state-expanded');
                    expander.setAttribute('aria-hidden', 'false');
                    expandedAny = true;
                }

                // Activate checkbox
                const checkBox = nodeContent.querySelector('.rz-chkbox');
                const chkIcon = checkBox?.querySelector('.rz-chkbox-box');
                const chkSymbol = checkBox?.querySelector('.rz-chkbox-icon');

                if (checkBox && chkIcon && chkSymbol) {
                    checkBox.classList.remove('rz-state-empty');
                    chkIcon.classList.add('rz-state-active');
                    chkSymbol.classList.add('rzi', 'rzi-check');
                }
            }
        });


        return expandedAny;
    }

    function checkParentIfAllChildrenChecked() {
        const allNodes = document.querySelectorAll('li.rz-treenode');

        allNodes.forEach(node => {
            const childBoxes = node.querySelectorAll(':scope > .rz-expander > .rz-expander-content > .rz-treenode-children .rz-chkbox-box');
            const activeChildBoxes = node.querySelectorAll(':scope > .rz-expander > .rz-expander-content > .rz-treenode-children .rz-chkbox-box.rz-state-active');

            if (childBoxes.length > 0 && childBoxes.length === activeChildBoxes.length) {
                const parentBox = node.querySelector(':scope > .rz-treenode-content .rz-chkbox');
                const parentIcon = parentBox?.querySelector('.rz-chkbox-box');
                const parentSymbol = parentBox?.querySelector('.rz-chkbox-icon');

                if (parentBox && parentIcon && parentSymbol) {
                    parentBox.classList.remove('rz-state-empty');
                    parentIcon.classList.add('rz-state-active');
                    parentSymbol.classList.add('rzi', 'rzi-check');
                }
            }
        });
    }

    function recursiveExpand(labelList, maxAttempts = 5, attempt = 1) {
        const expanded = expandAndCheckMatchingLabels(labelList);
        if (expanded && attempt < maxAttempts) {
            setTimeout(() => recursiveExpand(labelList, maxAttempts, attempt + 1), 200);
        } else {
            checkParentIfAllChildrenChecked();
        }
    }

    recursiveExpand(labelList, maxAttempts, 1);
}

window.enableScrollClassToggle = () => {
    const layoutPage = document.querySelector('.layout-page');
    const stickyWrapper = document.querySelector('.sticky-wrapper');
    const stickyElement = document.querySelector('.card-header.sticky-element');

    if (!layoutPage || !stickyWrapper || !stickyElement) return;
    const scrolled = window.scrollY > 10;
    const stickyTop = 65;
    const stickyZ = 9;

    layoutPage.classList.toggle('window-scrolled', scrolled);
    stickyWrapper.classList.toggle('is-sticky', scrolled);

    layoutPage.querySelectorAll('.window-scrolled').forEach(child => {
        if (child !== layoutPage) {
            child.classList.remove('window-scrolled');
        }
    });

    // Apply inline styles to sticky-element on scroll
    if (scrolled) {
        const elementRect = stickyElement.getBoundingClientRect();
        stickyElement.style.width = `${elementRect.width}px`;
        stickyElement.style.position = 'fixed';
        stickyElement.style.top = `${stickyTop}px`;
        stickyElement.style.zIndex = stickyZ;
    } else {
        // Reset styles when not scrolled
        stickyElement.style.position = '';
        stickyElement.style.top = '';
        stickyElement.style.zIndex = '';
        stickyElement.style.width = '';
    }
};

window.addEventListener('scroll', () => {
    window.enableScrollClassToggle();
})

window.emailFormat = function () {
    const className = ".emailFormat";
    if ($(className).length) {
        $(className).unbind('input');
        $(className).on("input", function () {
            let value = $(this).val().replace(/\s/g, "");
            $(this).val(value.replace(/[^a-zA-Z0-9@.\\~`]/gi, ''));
        })
    }
}

// Lock scroll function
function lockScroll() {
    const html = document.documentElement;
    const body = document.body;
    const scrollY = window.scrollY || window.pageYOffset;

    Object.assign(html.style, {
        overflow: 'hidden',
        overscrollBehavior: 'none',
        touchAction: 'none',
        position: 'fixed',
        width: '100%',
        height: '100%'
    });

    body.dataset.scrollY = scrollY;
}

// Unlock scroll and disable sticky header
function unlockScroll() {
    const html = document.documentElement;
    const body = document.body;
    const scrollY = body.dataset.scrollY || '0';

    // Remove lock styles
    ['overflow', 'position', 'width', 'height', 'top'].forEach(prop => {
        html.style.removeProperty(prop);
    });

    // Restore scroll position
    window.scrollTo(0, parseInt(scrollY));
    delete body.dataset.scrollY;
}


// Main table check function
function initializeTableScrollLock() {
    let minRows =6;
    const tables = document.querySelectorAll('.mud-table');
    console.log('Checking tables for scroll locking...');

    tables.forEach(table => {
        const rows = table.querySelectorAll('tbody tr');
        const visibleRowCount = Array.from(rows).filter(row =>
            row.offsetWidth > 0 || row.offsetHeight > 0
        ).length;

        if (visibleRowCount < minRows) {
            console.log(`Table has ${visibleRowCount} rows (minimum ${minRows} required), locking scroll`);
            lockScroll();
        } else {
            console.log(`Table has only ${visibleRowCount} rows - not locking scroll`);
            unlockScroll();
        }
    });
}

window.onlyNumber = function () {
    const className = ".clsNumOnly";
    if ($(className).length) {
        $(className).on("input", function () {
            $(this).val($(this).val().replace(/[^0-9]/gi, ''));
        })
    }
}

window.onlyNumberDot = function () {
    const className = ".clsNumberDot";
    if ($(className).length) {
        $(className).on("input", function () {
            $(this).val($(this).val().replace(/[^0-9.]/gi, ''));
        })
    }
}

window.allowAlphabetNumberandSpecialCharacter = function () {
    const className = ".clsAllowAlphabetNumberandSpecialCharacter";
    if ($(className).length) {
        $(className).keypress(function (e) {
            let regex = new RegExp("^[a-zA-Z0-9!@#$%^&*()_+=\\[{\\]};:<>|./?,-]+$");
            let key = String.fromCharCode(!e.charCode ? e.which : e.charCode);
            if (!regex.test(key)) {
                e.preventDefault();
                return false;
            }
        })
    }
}

window.allowAlphaNumberSpaceAndSpecialCharacter = function () {
    const className = ".clsAllowAlphaNumberSpaceAndSpecialCharacter";
    $(className).on('input', function () {
        $(this).val($(this).val().replace(/[^a-zA-Z0-9!@#$%^&*()_+=\[{\]};:'<>|./?\-,\s]/gi, ''));
    });
}

window.exportExcelFromJson = (jsonData) => {
    if (!jsonData || !jsonData.length) {
        alert('No data to export');
        return;
    }

    // Extract columns dynamically
    const cols = Object.keys(jsonData[0].values);

    const worksheetData = [
        cols, // header row
        ...jsonData.map(r => cols.map(c => r.values[c] ?? ""))
    ];

    let ws = XLSX.utils.aoa_to_sheet(worksheetData);
    let wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "InvalidRecords");

    XLSX.writeFile(wb, "InvalidRecords.xlsx");
}

window.clearSession = function () {
    clearSessionStorage();
}

function clearSessionStorage() {
    sessionStorage.clear();
    // localStorage.clear();
}

window.allowNumberAndSpecialCharacter = function () {
    const className = ".clsAllowNumberAndSpecialCharacter";
    $(className).on('input', function () {
        $(this).val($(this).val().replace(/[^0-9!@#$%^&*()_+=\[{\]};:'<>|./?\-,\s]/gi, ''));
    });
}

window.onlyAlpha = function () {
    const className = ".clsOnlyAlpha";
    if ($(className).length) {
        $(className).on("input", function () {
            $(this).val($(this).val().replace(/[^a-zA-Z]/gi, ''));
        })
    }
}


window.onlyNumberDash = function () {
    const className = ".clsNumberDash";
    if ($(className).length) {
        $(className).on("input", function () {
            $(this).val($(this).val().replace(/[^0-9-]/gi, ''));
        })
    }
}

window.onlyAlphaNumberAndDot = function () {
    const className = ".clsaplhaNumberAndDot";
    $(className).on('input', function () {
        $(this).val($(this).val().replace(/[^a-zA-Z0-9()\\.]/gi, ''));
    });
}

window.onlyAlphaNumberAndDash = function () {
    const className = ".clsAlphaNumberAndDash";
    $(className).on('input', function () {
        $(this).val($(this).val().replace(/[^a-zA-Z0-9-]/g, ''));
    });
}

window.allowAlphaNumberAndSpace = function () {
    const className = ".clsAlphaNumberAndSpace"; 
    $(className).on('input', function () {
        $(this).val($(this).val().replace(/[^a-zA-Z0-9\s]/g, ''));
    });
}

window.allowAlphaNumberAndUnderScore = function () {
    const className = ".clsAlphaNumberAndUnderScore";
    $(className).on('input', function () {
        $(this).val($(this).val().replace(/[^a-zA-Z0-9_]/gi,''));
    });
}

window.getInputValue = (id) => {
    const el = document.getElementById(id);
    return el ? el.value : '';
};