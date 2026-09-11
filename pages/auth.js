/* PRIMATA ARCHIVE - AUTENTICAÇÃO */

document.addEventListener("DOMContentLoaded", function () {
    var loginForm = document.getElementById("loginForm");
    var registerForm = document.getElementById("registerForm");

    if (loginForm) {
        loginForm.addEventListener("submit", async function (event) {
            event.preventDefault();

            var email = document.getElementById("email").value.trim();
            var password = document.getElementById("password").value;

            if (!email || !password) {
                alert("Preencha todos os campos.");
                return;
            }

            var button = loginForm.querySelector("button[type='submit']");
            if (button) button.disabled = true;

            try {
                var data = await PrimataAPI.post("/auth/login", {
                    email: email,
                    senha: password
                });

                if (data.token) {
                    localStorage.setItem("primataToken", data.token);
                }

                if (data.usuario) {
                    localStorage.setItem("primataUsuario", JSON.stringify(data.usuario));
                }

                window.location.href = "../home/index.html";
            } catch (error) {
                mostrarErroAPI(error);
            } finally {
                if (button) button.disabled = false;
            }
        });
    }

    if (registerForm) {
        registerForm.addEventListener("submit", async function (event) {
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

            var button = registerForm.querySelector("button[type='submit']");
            if (button) button.disabled = true;

            try {
                var data = await PrimataAPI.post("/auth/register", {
                    username: username,
                    email: email,
                    senha: password
                });

                if (data.token) {
                    localStorage.setItem("primataToken", data.token);
                }

                if (data.usuario) {
                    localStorage.setItem("primataUsuario", JSON.stringify(data.usuario));
                }

                window.location.href = "../home/index.html";
            } catch (error) {
                mostrarErroAPI(error);
            } finally {
                if (button) button.disabled = false;
            }
        });
    }
});
