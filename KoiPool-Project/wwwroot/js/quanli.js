document.addEventListener("DOMContentLoaded", function () {
    // Dữ liệu mẫu cho lịch sử đơn hàng
    const donHangData = [
        { id: 12345, ngayDat: "2023-01-20", trangThai: "Hoàn tất" },
        { id: 12346, ngayDat: "2023-02-10", trangThai: "Đang xử lý" }
    ];

    // Dữ liệu mẫu cho quản lý thanh toán
    const thanhToanData = [
        { id: "TXN001", ngay: "2023-01-15", soTien: "1,000,000₫", trangThai: "Hoàn tất" },
        { id: "TXN002", ngay: "2023-02-05", soTien: "500,000₫", trangThai: "Đã hủy" }
    ];

    // Dữ liệu mẫu cho điểm thưởng
    const diemThuong = {
        diemHienTai: 1500,
        mucTieuTiepTheo: 2000
    };

    // Hiển thị dữ liệu Lịch Sử Đơn Hàng
    const donHangContainer = document.getElementById("don-hang-container");
    donHangData.forEach(donHang => {
        const donHangDiv = document.createElement("div");
        donHangDiv.classList.add("don-hang");
        donHangDiv.innerHTML = `
            <p>Đơn hàng #${donHang.id}</p>
            <p>Ngày đặt: ${donHang.ngayDat}</p>
            <p>Trạng thái: <span class="trang-thai ${donHang.trangThai === "Hoàn tất" ? "hoan-tat" : "dang-xu-ly"}">${donHang.trangThai}</span></p>
            <button class="xem-chi-tiet">Xem chi tiết</button>
        `;
        donHangContainer.appendChild(donHangDiv);
    });

    // Hiển thị dữ liệu Quản Lý Thanh Toán
    const bangThanhToan = document.getElementById("bang-thanh-toan");
    thanhToanData.forEach(thanhToan => {
        const row = document.createElement("tr");
        row.innerHTML = `
            <td>${thanhToan.id}</td>
            <td>${thanhToan.ngay}</td>
            <td>${thanhToan.soTien}</td>
            <td class="trang-thai ${thanhToan.trangThai === "Hoàn tất" ? "hoan-tat" : "huy"}">${thanhToan.trangThai}</td>
        `;
        bangThanhToan.appendChild(row);
    });

    // Hiển thị dữ liệu Điểm Thưởng
    document.getElementById("diem-hien-tai").textContent = diemThuong.diemHienTai;
    document.getElementById("muc-tieu-tiep-theo").textContent = diemThuong.mucTieuTiepTheo;
    const mucTienTrinh = document.getElementById("muc-tien-trinh");
    const tienTrinhPhanTram = (diemThuong.diemHienTai / diemThuong.mucTieuTiepTheo) * 100;
    mucTienTrinh.style.width = `${tienTrinhPhanTram}%`;
});
