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