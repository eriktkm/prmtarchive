const loginForm = document.getElementById("loginForm");

loginForm.addEventListener("submit", function (event) {
    event.preventDefault();

    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;

    if (!email || !password) {
        return;
    }

    console.log("Tentativa de login:", email);

    alert("Sistema de login ainda não conectado ao servidor.");
});