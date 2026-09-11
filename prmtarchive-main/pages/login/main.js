// PRIMATA ARCHIVE - LOGIN

document.addEventListener("DOMContentLoaded", function () {

    var loginForm = document.getElementById("loginForm");

    if (!loginForm) return;

    loginForm.addEventListener("submit", function (event) {
        event.preventDefault();

        var email = document.getElementById("email").value.trim();
        var password = document.getElementById("password").value;

        if (!email || !password) {
            alert("Preencha todos os campos.");
            return;
        }

        var cadastro = JSON.parse(localStorage.getItem("primataUsuario") || "null");

        if (cadastro && cadastro.email === email && cadastro.password === password) {
            localStorage.setItem("primataLogado", "true");
            localStorage.setItem("primataEmail", email);
            alert("Login realizado com sucesso!");
            window.location.href = "../landing/index.html";
            return;
        }

        // Conta demonstrativa para testar a tela
        if (email === "demo@primata.com" && password === "123456") {
            localStorage.setItem("primataLogado", "true");
            localStorage.setItem("primataEmail", email);
            alert("Login demonstrativo realizado!");
            window.location.href = "../landing/index.html";
            return;
        }

        alert("E-mail ou senha incorretos.");
    });

});
