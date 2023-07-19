"use strict";

/**
 * @description Function to handle sidebar toggle event
 * @param {HTMLElement} elem 
 */
const sidebarToggle = (elem) => {
    elem.addEventListener('click', () => {
        const sidebar = document.querySelector('.sidebar');
        sidebar.classList.toggle('sidebar-mini');
    });
};

/**
 * @description Function to resize plugins based on window width
 */
const resizePlugins = () => {
    const tabs = document.querySelectorAll('.nav');
    const sidebarResponsive = document.querySelector('.sidebar-default');

    const isWindowSmall = window.innerWidth < 1300;

    // toggle classes based on window width
    tabs.forEach(elem => {
        if (isWindowSmall) {
            elem.classList.add('flex-column', 'on-resize');
            sidebarResponsive?.classList.add('sidebar-mini', 'on-resize');
        } else {
            elem.classList.remove('flex-column', 'on-resize');
            sidebarResponsive?.classList.remove('sidebar-mini', 'on-resize');
        }
    });
};

document.addEventListener("DOMContentLoaded", function () {
    // initialization of custom popovers and tooltips
    if (typeof bootstrap !== typeof undefined) {
        const popoverTriggerList = Array.from(document.querySelectorAll('[data-bs-toggle="popover"]'));
        popoverTriggerList.map(popoverTriggerEl => new bootstrap.Popover(popoverTriggerEl));

        const tooltipTriggerList = Array.from(document.querySelectorAll('[data-bs-toggle="tooltip"], [data-sidebar-toggle="tooltip"]'));
        tooltipTriggerList.map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl));
    }

    // initialization of custom scrollbar
    if (typeof Scrollbar !== typeof null) {
        const scrollbarElement = document.querySelector('.data-scrollbar');
        if (scrollbarElement) {
            Scrollbar.init(scrollbarElement, { continuousScrolling: false });
        }
    }

    // handle sidebar toggle button click event
    const sidebarToggleBtns = document.querySelectorAll('[data-toggle="sidebar"]');
    sidebarToggleBtns.forEach(sidebarToggle);

    // handle back-to-top button click event
    document.querySelector('#top')?.addEventListener('click', (e) => {
        e.preventDefault();
        window.scrollTo({ top: 0, behavior: 'smooth' });
    });

    // offcanvas initialization
    document.querySelectorAll('[data-trigger]').forEach(element => {
        let offcanvas_id = element.getAttribute('data-trigger');
        element.addEventListener('click', (e) => {
            e.preventDefault();
            show_offcanvas(offcanvas_id);
        });
    });

    document.querySelectorAll('.btn-close').forEach(button => {
        button.addEventListener('click', close_offcanvas);
    });

    document.querySelector('.screen-darken')?.addEventListener('click', close_offcanvas);

    document.querySelector('#navbarSideCollapse')?.addEventListener('click', function () {
        document.querySelector('.offcanvas-collapse').classList.toggle('open');
    });

    // call resizePlugins function to handle responsive view
    resizePlugins();
});

// handle window resize event
window.addEventListener('resize', resizePlugins);

// handle scroll event for navbar sticky
window.addEventListener('scroll', () => {
    const yOffset = document.documentElement.scrollTop;
    const navbar = document.querySelector(".navs-sticky");
    navbar?.classList.toggle("menu-sticky", yOffset >= 100);
});

// handle form validation
window.addEventListener('load', () => {
    const forms = Array.from(document.getElementsByClassName('needs-validation'));

    forms.forEach(form => {
        form.addEventListener('submit', (event) => {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }
            form.classList.add('was-validated');
        });
    });
});

/**
 * @description Function to darken the screen
 * @param {boolean} yesno 
 */
function darken_screen(yesno) {
    document.querySelector('.screen-darken')?.classList.toggle('active', yesno);
}

/**
 * @description Function to close offcanvas
 */
function close_offcanvas() {
    darken_screen(false);
    document.querySelector('.mobile-offcanvas.show')?.classList.remove('show');
    document.body.classList.remove('offcanvas-active');
}

/**
 * @description Function to show offcanvas
 * @param {string} offcanvas_id 
 */
function show_offcanvas(offcanvas_id) {
    darken_screen(true);
    document.getElementById(offcanvas_id)?.classList.add('show');
    document.body.classList.add('offcanvas-active');
}
