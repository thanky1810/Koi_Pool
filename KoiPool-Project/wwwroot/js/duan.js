/*Menu*/
window.addEventListener("scroll", function () {
    const header = document.querySelector("header");
    if (window.scrollY === 0) {
        // Khi ở đầu trang
        header.style.background = "transparent";
        header.style.boxShadow = "none";
    } else {
        // Khi đã cuộn xuống
        header.style.background = "linear-gradient(90deg, #060707, #058697)";
        header.style.boxShadow = "0 2px 5px rgba(0,0,0,0.1)";
    }
});
// Toggle menu on mobile view
const menuToggle = document.querySelector('.menu-toggle');
const nav = document.querySelector('nav ul');

menuToggle.addEventListener('click', () => {
    nav.classList.toggle('show');
});

// Handle dropdown toggle on mobile view
const dropdownTogglers = document.querySelectorAll('.dropdown-toggler');

dropdownTogglers.forEach(toggler => {
    toggler.addEventListener('click', function (e) {
        e.preventDefault();
        const parentDropdown = this.parentElement;
        parentDropdown.classList.toggle('active');
    });
});
/* */
document.querySelectorAll('.navbar-dropdown').forEach(dropdown => {
    dropdown.addEventListener('mouseenter', function () {
        const dropdownMenu = this.querySelector('.dropdown');
        dropdownMenu.classList.add('show');
    });

    dropdown.addEventListener('mouseleave', function () {
        const dropdownMenu = this.querySelector('.dropdown');
        dropdownMenu.classList.remove('show');
    });
});
/*Dự Án */
document.addEventListener("DOMContentLoaded", () => {
    const projectItems = document.querySelectorAll(".project-item");

    projectItems.forEach((item, index) => {
        // Apply different animations to each item based on position
        if (index % 4 === 0) {
            item.classList.add("left");
        } else if (index % 4 === 1) {
            item.classList.add("right");
        } else if (index % 4 === 2) {
            item.classList.add("top");
        } else {
            item.classList.add("bottom");
        }

        // Add "active" class with delay for each item
        setTimeout(() => {
            item.classList.add("active");
            const overlayContent = item.querySelector(".overlay-content");
            if (overlayContent) {
                overlayContent.classList.add("active");
            }
        }, index * 150); // Delay between each item
    });
});
