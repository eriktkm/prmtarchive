// PRIMATA ARCHIVE - CADASTRO

document.addEventListener("DOMContentLoaded", function () {

    var registerForm = document.getElementById("registerForm");

    if (!registerForm) return;

    registerForm.addEventListener("submit", function (event) {
        event.preventDefault();

        var username = document.getElementById("username").value.trim();
        var email = document.getElementById("email").value.trim();
        var password = document.getElementById("password").value;
        var confirmPassword = document.getElementById("confirmPassword").value;
        var terms = document.getElementById("terms").checked;

        if (!username || !email || !password || !confirmPassword) {
            alert("Preencha todos os campos.");
            return;
        }

        if (password.length < 6) {
            alert("A senha precisa ter pelo menos 6 caracteres.");
            return;
        }

        if (password !== confirmPassword) {
            alert("As senhas não coincidem.");
            return;
        }

        if (!terms) {
            alert("Você precisa aceitar os termos de uso.");
            return;
        }

        var usuario = {
            username: username,
            email: email,
            password: password
        };

        localStorage.setItem("primataUsuario", JSON.stringify(usuario));
        localStorage.setItem("primataLogado", "true");
        localStorage.setItem("primataEmail", email);

        alert("Cadastro realizado com sucesso!");

        window.location.href = "../landing/index.html";
    });

});
