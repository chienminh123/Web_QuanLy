function handleCategoryChange() {
    const select = document.getElementById("categorySelect");
    const inputDiv = document.getElementById("newCategoryInput");

    if (select.value === "Thêm Thể loại") {
        inputDiv.style.display = "block"; // Hiển thị ô nhập liệu
    } else {
        inputDiv.style.display = "none"; // Ẩn ô nhập liệu
    }
}

function addNewCategory() {
    const select = document.getElementById("categorySelect");
    const input = document.getElementById("categoryInput");
    const newCategory = input.value.trim();

    if (newCategory === "") {
        alert("Vui lòng nhập tên thể loại!");
        return;
    }

    // Tạo option mới
    const newOption = document.createElement("option");
    newOption.value = newCategory;
    newOption.text = newCategory;

    // Thêm option mới vào select
    select.insertBefore(newOption, select.options[select.options.length - 1]);

    // Reset và chọn thể loại mới
    select.value = newCategory;
    input.value = "";
    document.getElementById("newCategoryInput").style.display = "none";
}



function toggleQuantityInput(checkbox) {
    const quantityInput = checkbox.parentElement.querySelector(".quantity-input");
    quantityInput.style.display = checkbox.checked ? "inline-block" : "none";
    if (!checkbox.checked) {
        quantityInput.value = ""; // Xóa giá trị khi bỏ chọn
    }
}

// Chart.js code for revenue chart
//let revenueChart = null;

//function updateChart() {
//    const startDate = new Date(document.getElementById("startDate").value);
//    const endDate = new Date(document.getElementById("endDate").value);

//    if (!startDate || !endDate || startDate > endDate) {
//        alert("Vui lòng chọn ngày bắt đầu và ngày kết thúc hợp lệ!");
//        return;
//    }

//    // Tạo danh sách ngày và dữ liệu mẫu
//    const labels = [];
//    const data = [];
//    const currentDate = new Date(startDate);
//    while (currentDate <= endDate) {
//        labels.push(currentDate.toLocaleDateString("vi-VN"));
//        data.push(Math.floor(Math.random() * 10) + 1); // Doanh thu mẫu (1-10 triệu VND)
//        currentDate.setDate(currentDate.getDate() + 1);
//    }

//    // Hủy biểu đồ cũ nếu có
//    if (revenueChart) {
//        revenueChart.destroy();
//    }

//    // Tạo biểu đồ mới
//    const ctx = document.getElementById("revenueChart").getContext("2d");
//    revenueChart = new Chart(ctx, {
//        type: "bar",
//        data: {
//            labels: labels,
//            datasets: [
//                {
//                    label: "Doanh thu (triệu VND)",
//                    data: data,
//                    backgroundColor: "#4CAF50",
//                    borderColor: "#388E3C",
//                    borderWidth: 1,
//                },
//            ],
//        },
//        options: {
//            scales: {
//                y: {
//                    beginAtZero: true,
//                    title: {
//                        display: true,
//                        text: "Doanh thu (triệu VND)",
//                    },
//                },
//                x: {
//                    title: {
//                        display: true,
//                        text: "Ngày",
//                    },
//                },
//            },
//            plugins: {
//                legend: {
//                    display: true,
//                    position: "top",
//                },
//                title: {
//                    display: true,
//                    text: `Doanh Thu từ ${startDate.toLocaleDateString(
//                        "vi-VN"
//                    )} đến ${endDate.toLocaleDateString("vi-VN")}`,
//                },
//            },
//        },
//    });
//}
