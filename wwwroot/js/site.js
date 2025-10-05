function toggleSizeType(categoryName) {
    const clothingSizes = document.getElementById("clothingSizes");
    const shoeSizes = document.getElementById("shoeSizes");

    if (categoryName.toLowerCase().includes('giày') || categoryName.toLowerCase().includes('dép')) {
        clothingSizes.style.display = "none";
        shoeSizes.style.display = "block";
        // Vô hiệu hóa các input của clothingSizes
        Array.from(clothingSizes.querySelectorAll('input')).forEach(input => {
            input.disabled = true;
        });
        // Kích hoạt các input của shoeSizes
        Array.from(shoeSizes.querySelectorAll('input')).forEach(input => {
            input.disabled = false;
        });
    } else {
        clothingSizes.style.display = "block";
        shoeSizes.style.display = "none";
        // Vô hiệu hóa các input của shoeSizes
        Array.from(shoeSizes.querySelectorAll('input')).forEach(input => {
            input.disabled = true;
        });
        // Kích hoạt các input của clothingSizes
        Array.from(clothingSizes.querySelectorAll('input')).forEach(input => {
            input.disabled = false;
        });
    }

    // Đặt lại số lượng khi chuyển đổi
    const inputs = document.querySelectorAll('.size-container input[type="number"]');
    inputs.forEach(input => input.value = '0');
}

function handleCategoryChange() {
    const select = document.getElementById("categorySelect");
    const newInput = document.getElementById("newCategoryInput");
    const hidden = document.getElementById("hiddenCategory");

    if (select.value === "Thêm Thể loại") {
        newInput.style.display = 'block';
        hidden.value = '';
    } else {
        newInput.style.display = 'none';
        document.getElementById("categoryInput").value = '';
        hidden.value = select.value;
        toggleSizeType(select.value); // Tự động switch size dựa trên thể loại chọn
    }
}

function addNewCategory() {
    const inputValue = document.getElementById("categoryInput").value.trim();
    if (inputValue) {
        const hidden = document.getElementById("hiddenCategory");
        hidden.value = inputValue;
        document.getElementById("newCategoryInput").style.display = 'none';

        // Thêm thể loại mới vào dropdown và chọn nó
        const select = document.getElementById("categorySelect");
        if (!Array.from(select.options).some(option => option.value === inputValue)) {
            const newOption = new Option(inputValue, inputValue, false, true);
            select.add(newOption);
        }
        select.value = inputValue; // Chọn thể loại vừa thêm
        toggleSizeType(inputValue); // Cập nhật kích thước dựa trên thể loại mới
    } else {
        alert("Vui lòng nhập tên thể loại mới!");
    }
}


$(document).ready(function () {
    $("#productForm").on("submit", function (e) {
        console.log("Form submitted");
        const formData = new FormData(this);
        console.log("Form Data Keys:", Array.from(formData.keys()));
        for (let pair of formData.entries()) {
            console.log(`${pair[0]}:`, pair[1]);
        }

        // Kiểm tra file
        const files = formData.getAll("HinhAnhs");
        if (files.length === 0) {
            console.log("Không có file nào được chọn!");
            e.preventDefault(); // Ngăn submit nếu không có file (nếu bắt buộc)
            alert("Vui lòng chọn ít nhất một hình ảnh!");
        } else {
            console.log("Số file được chọn:", files.length);
        }

        if (!$(this).valid()) {
            e.preventDefault();
            console.log("Validation failed");
        } else {
            console.log("Form is valid, submitting...");
        }
        Console.ReadLine();
    });
});