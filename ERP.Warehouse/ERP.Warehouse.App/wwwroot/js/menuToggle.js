// const menuExpand = document.querySelector(".menuExpand-toggle-btn");
const menuExpand = document.querySelector(".layout-menu-toggle.menu-link.text-large.ms-auto");
const navSearchContent = document.querySelector("#navSearchContent");
let menuItem = document.querySelector('.menu-item.active.open')

function OpenMenu(menu, dropDown) {

    menu.classList.remove('collapsed');
    if (dropDown.classList.contains("collapse")) {
        dropDown.classList.toggle("show");
    }

    const isExpanded = menu.getAttribute("aria-expanded") === "true";
    menu.setAttribute("aria-expanded", !isExpanded);
}

function handleResize() {
    const sidebar = document.getElementById('layout-menu');
    if (window.innerWidth < 1000 && sidebar.classList.contains('expand')) {
        sidebar.classList.remove('expand');
    } else if (window.innerWidth > 1000 && !sidebar.classList.contains('expand')) {
        document.querySelector("#layout-menu").classList.toggle("expand");
    }
}

window.addEventListener('resize', handleResize);

// Initial check when the page loads
handleResize();

function CloseMenu(menu, dropDown) {

    menu.classList.add('collapsed');
    dropDown.classList.remove("show");

    const isExpanded = menu.getAttribute("aria-expanded") === "true";
    menu.setAttribute("aria-expanded", !isExpanded);
}

menuExpand.addEventListener("click", function () {
    document.querySelector("#layout-menu").classList.toggle("expand");
});

// headerSearch.addEventListener("click", function () {
//     headerSearch.classList.add("hide-item");
//     sideBarLogo.classList.add("hide-item");
//
//     navSearchContent.style.display = "block";
//     navSearchMain.style.display = "inline-block";
// })

// searchBoxCancel.addEventListener("click", function () {
//     headerSearchBox.value = '';
//
//     navSearchMain.style.display = "none";
//     navSearchContent.style.display = "none";
//     headerSearch.classList.remove("hide-item");
//     sideBarLogo.classList.remove("hide-item");
//
//     navSearchContent.querySelectorAll('a').forEach(link => link.remove());
// })

window.filterMenu = function () {
    const input = document.getElementById("navSearchInput");
    const filter = input.value.toUpperCase();

    navSearchContent.querySelectorAll('a').forEach(link => link.remove());

    if (filter === '')
        return;

    var hasValue = false;
    document.querySelectorAll('.menu-nav-item').forEach(item => {
        const menu = item.querySelector('a');
        var menuValue = menu.textContent.trim();

        var link = document.createElement('a');
        if (menuValue.toUpperCase().indexOf(filter) > -1) {
            link.href = menu.getAttribute('href');
            link.textContent = menuValue;
            link.className = 'drop-down-menu'

            navSearchContent.append(link);
            hasValue = true;
        }
    })

    if (hasValue === false) {
        var noResult = document.createElement('a');
        noResult.href = '#';
        noResult.textContent = 'No result';

        navSearchContent.append(noResult);
    }

}

document.querySelectorAll('.menu-nav-item').forEach(menuItem => {
    menuItem.addEventListener('click', function () {
        menuItem.classList.add('active');
        const mainItem = menuItem.closest('.main-item');
        const subMainMenu = mainItem.querySelector('.sub-main-menu');
        const mainMenu = mainItem.querySelector('.main-menu');

        var id = menuItem.getAttribute("id");
        var mainMenuId = mainMenu.getAttribute("id");

        localStorage.setItem("Active_Main_Menu", mainMenuId);
        localStorage.setItem("Active_Sub_Menu", id);
        localStorage.setItem("Active_Sub_Main_Menu", null);

        if (subMainMenu !== null) {
            var subMainMenuId = subMainMenu.getAttribute('id');
            localStorage.setItem("Active_Sub_Main_Menu", subMainMenuId);
        }

        document.querySelectorAll('.menu-nav-item').forEach(item => {
            // const otherMainMenu = item.closest('.main-menu');
            // var otherMainMenuId = otherMainMenu.getAttribute("id");

            // if (mainMenuId !== otherMainMenuId)
            //   otherMainMenu.classList.remove('open');

            if (item !== menuItem) {
                item.classList.remove('active');
            }
        })
    })
})

// document.getElementById('navSearchContent').addEventListener('click', function (event) {
//     if (event.target.classList.contains('drop-down-menu')) {
//         headerSearchBox.value = '';
//
//         navSearchMain.style.display = "none";
//         navSearchContent.style.display = "none";
//         headerSearch.classList.remove("hide-item");
//         sideBarLogo.classList.remove("hide-item");
//
//         navSearchContent.querySelectorAll('a').forEach(link => link.remove());
//
//         var route = event.target.getAttribute('href');
//         setMenu(route);
//     }
// });

window.getRouteMenu = function (route) {
    // Define route mappings
    const routeMap = [
        {
            patterns: ["/"],
            includes: "/home",
            menu: {
                "MainMenuId": "home-menu",
                "SubMenuId": "dashboard-sub-menu",
            }
        },
        {
            patterns: ["/role", "/role/create", "/role/list"],
            includes: "/role/edit",
            menu: {
                "MainMenuId": "administration-menu",
                "SubMenuId": "role-sub-menu",
            }
        },
        {
            patterns: ["/backgroundtask", "/backgroundtask/list"],
            includes: "/backgroundtask/edit",
            menu: {
                "MainMenuId": "administration-menu",
                "SubMenuId": "background-sub-menu",
            }
        },
        {
            patterns: ["/admin-user", "/admin-user/create", "/admin-user/list"],
            includes: "/admin-user/edit",
            menu: {
                "MainMenuId": "administration-menu",
                "SubMenuId": "user-sub-menu",
            }
        },
        {
            patterns: ["/profile"],
            menu: {
                "MainMenuId": "account-setting-menu",
                "SubMenuId": "profile-sub-menu",
            }
        },
    ];

    for (const entry of routeMap) {
        if (entry.patterns.includes(route) || (entry.includes && route.includes(entry.includes))) {
            return entry.menu;
        }
    }

    return {};
};

window.setActiveMenu = function () {
    const routePath = window.location.pathname;
    console.log("routePath==>", routePath);
    setMenu(routePath);
}

window.setMenu = function (routePath) {
    const menuObj = getRouteMenu(routePath);

    console.log("menu obj => ", menuObj);
    if (menuObj !== undefined && menuObj !== null) {
        var activeSubMenuId = menuObj.SubMenuId;
        var activeMainMenuId = menuObj.MainMenuId;
        var activeMainMenu = document.getElementById('id');

        if (activeMainMenuId !== null && activeSubMenuId != null) {

            //open main menu 
            const menuItems = document.querySelectorAll('.main-menu');
            for (let i = 0; i < menuItems.length; i++) {
                var item = menuItems[i];
                const dropDownId = item.getAttribute('data-bs-target');
                const dropDownMenu = document.querySelector(dropDownId);
                var mainMenuId = item.getAttribute("id");
                if (activeMainMenuId === mainMenuId) {
                    OpenMenu(item, dropDownMenu);
                } else CloseMenu(item, dropDownMenu);
            }

            //set active to sub menu
            const subMenuItems = document.querySelectorAll('.menu-nav-item');
            for (let i = 0; i < subMenuItems.length; i++) {
                var item = subMenuItems[i];
                var subMenuId = item.getAttribute("id");
                if (activeSubMenuId === subMenuId) {
                    item.classList.toggle('active');
                } else item.classList.remove('active');
            }
        }
    }
}

// window.addEventListener('load', () => {
//     const layoutMenu = document.getElementById('layout-menu');
//     if (layoutMenu) {
//         layoutMenu.scrollTop = 0; // Reset scroll position on load
//     }
// });
//
// // Optional: adjust height dynamically if your layout has header/footer
// function adjustSidebarHeight() {
//     const layoutMenu = document.getElementById('layout-menu');
//     const headerHeight = document.querySelector('.layout-navbar')?.offsetHeight || 0;
//     const windowHeight = window.innerHeight;
//     if (layoutMenu) {
//         layoutMenu.style.maxHeight = (windowHeight - headerHeight) + 'px';
//     }
// }
//
// window.addEventListener('resize', adjustSidebarHeight);
// window.addEventListener('load', adjustSidebarHeight);

// Scroll down by 100 pixels
function scrollDown() {
    const menu = document.getElementById('layout-menu');
    menu.scrollBy({ top: 100, behavior: 'smooth' });
}

// Scroll up by 100 pixels
function scrollUp() {
    const menu = document.getElementById('layout-menu');
    menu.scrollBy({ top: -100, behavior: 'smooth' });
}

//--------------------------
document.querySelectorAll('.menu-toggle').forEach(toggle => {
    toggle.addEventListener('click', function (e) {
        e.preventDefault(); // Prevent "jump to top"
        const parent = this.closest('.menu-item');
        alert("ALERET");
        // Toggle 'open' class on the clicked menu
        parent.classList.toggle('open');

        // Close all sibling menu-items
        parent.parentElement.querySelectorAll('.menu-item.open').forEach(item => {
            if (item !== parent) item.classList.remove('open');
        });
    });
});

window.toggleMenu = function () {
    const toggleButton = document.querySelector('.layout-menu-toggle.menu-link.text-large.ms-auto');
    const storageKey = 'templateCustomizer-vertical-menu-template--LayoutCollapsed';

    // Optional: Initialize localStorage if not set
    if (localStorage.getItem(storageKey) === null) {
        localStorage.setItem(storageKey, 'true'); // start as collapsed
    }

    if (toggleButton) {
        toggleButton.addEventListener('click', () => {
            const currentState = localStorage.getItem(storageKey) === 'true'; // collapsed = true
            const newState = !currentState; // toggle

            localStorage.setItem(storageKey, newState.toString());
        });
    }
};

