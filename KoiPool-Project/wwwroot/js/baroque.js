document.addEventListener("DOMContentLoaded", function () {
    const faders = document.querySelectorAll('.fade-in');

    const appearOptions = {
        threshold: 0.5,
        rootMargin: "0px 0px -50px 0px"
    };

    const appearOnScroll = new IntersectionObserver(function (entries, appearOnScroll) {
        entries.forEach(entry => {
            if (!entry.isIntersecting) {
                return;
            } else {
                entry.target.classList.add('show');
                appearOnScroll.unobserve(entry.target);
            }
        });
    }, appearOptions);

    faders.forEach(fader => {
        appearOnScroll.observe(fader);
    });
});
document.querySelectorAll('.phan-noi-dung ul li a').forEach((link) => {
    link.addEventListener('click', function () {
        // Xóa lớp highlight khỏi tất cả các liên kết
        document.querySelectorAll('.highlight').forEach((item) => {
            item.classList.remove('highlight');
        });

        // Thêm lớp highlight cho liên kết được nhấp
        this.classList.add('highlight');

        // Lưu ID của liên kết vào localStorage
        localStorage.setItem('highlightedLink', this.getAttribute('href'));
    });
});

// Kiểm tra và áp dụng highlight khi tải lại trang
window.addEventListener('load', function () {
    const highlightedLink = localStorage.getItem('highlightedLink');
    if (highlightedLink) {
        const activeLink = document.querySelector(`.phan-noi-dung ul li a[href="${highlightedLink}"]`);
        if (activeLink) {
            activeLink.classList.add('highlight');
        }
    }
});
document.addEventListener("DOMContentLoaded", function () {
    // Lấy tất cả các liên kết trong .phan-chuyen-muc
    const links = document.querySelectorAll('.phan-chuyen-muc ul li a');

    // Kiểm tra nếu có mục nào đã được lưu trong localStorage
    const savedLink = localStorage.getItem('activeLink');

    if (savedLink) {
        // Nếu có, tìm mục đó và thêm lớp highlight-chuyen-muc
        links.forEach((link) => {
            if (link.getAttribute('href') === savedLink) {
                link.classList.add('highlight-chuyen-muc');
            }
        });
    }

    // Thêm sự kiện click vào từng liên kết
    links.forEach((link) => {
        link.addEventListener('click', function () {
            // Xóa lớp highlight-chuyen-muc khỏi tất cả các liên kết
            links.forEach((item) => {
                item.classList.remove('highlight-chuyen-muc');
            });

            // Thêm lớp highlight-chuyen-muc vào mục được nhấp
            this.classList.add('highlight-chuyen-muc');

            // Lưu mục được nhấp vào localStorage
            localStorage.setItem('activeLink', this.getAttribute('href'));
        });
    });
});