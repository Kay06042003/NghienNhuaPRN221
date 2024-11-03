const notifications = document.querySelector(".notifications"),
        buttons = document.querySelectorAll(".buttons .btn");

// Object containing details for different types of toasts
const toastDetails = {
    timer: 5000,
    success: {
        icon: 'fa-circle-check',
        text: 'Success: Thành công.',
    },
    error: {
        icon: 'fa-circle-xmark',
        text: 'Error: Không thành công.',
    },
    warning: {
        icon: 'fa-triangle-exclamation',
        text: 'Warning: This is a warning toast.',
    },
    info: {
        icon: 'fa-circle-info',
        text: 'Info: This is an information toast.',
    },
    login: {
        icon: 'fa-circle-xmark',
        text: 'Error: Vui lòng đăng nhập.',
    },
    ErrorLogin: {
        icon: 'fa-circle-xmark',
        text: 'Error: Sai email hoặc mật khẩu.',
    },
    ErrorAddtoCart: {
        icon: 'fa-circle-xmark',
        text: 'Error: Không thể thêm sản phẩm vào giỏ hàng.',
    },
    ErrorSignIn: {
        icon: 'fa-circle-xmark',
        text: 'Error: Email đã tồn tại.',
    },
    ErrorOrder: {
        icon: 'fa-circle-xmark',
        text: 'Error Sản phẩm vượt quá số lượng.',
    },
    ErrorCart: {
        icon: 'fa-circle-xmark',
        text: 'Không có sản phẩm nào trong giỏ hàng.',
    },
}

const removeToast = (toast) => {
    toast.classList.add("hide");
    if (toast.timeoutId)
        clearTimeout(toast.timeoutId); // Clearing the timeout for the toast
    setTimeout(() => toast.remove(), 500); // Removing the toast after 500ms
}

const createToast = (id) => {
    // Getting the icon and text for the toast based on the id passed
    const {icon, text} = toastDetails[id];
    const toast = document.createElement("li"); // Creating a new 'li' element for the toast
    toast.className = `toast ${id}`; // Setting the classes for the toast
    // Setting the inner HTML for the toast
    toast.innerHTML = `<div class="column">
                         <i class="fa-solid ${icon}"></i>
                         <span>${text}</span>
                      </div>
                      <i class="fa-solid fa-xmark" onclick="removeToast(this.parentElement)"></i>`;
    notifications.appendChild(toast); // Append the toast to the notification ul
    toast.classList.add("show");
    // Setting a timeout to remove the toast after the specified duration
    toast.timeoutId = setTimeout(() => removeToast(toast), toastDetails.timer);
}


