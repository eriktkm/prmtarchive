const registerForm = document.getElementById("registerForm");

registerForm.addEventListener("submit", function (event) {

    event.preventDefault();

    const username = document.getElementById("username").value;
    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;
    const confirmPassword = document.getElementById("confirmPassword").value;
    const terms = document.getElementById("terms").checked;

    if (password !== confirmPassword) {
        alert("As senhas não coincidem.");
        return;
    }

    if (!terms) {
        alert("Você precisa aceitar os termos de uso.");
        return;
    }

    console.log("Cadastro:", {
        username,
        email,
        password
    });

    alert("Cadastro realizado! O sistema ainda não está conectado ao backend.");
});
