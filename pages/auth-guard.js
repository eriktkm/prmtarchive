(function () {
    var path = window.location.pathname;
    var isLoginPage = path.indexOf("/login/") !== -1 || path.endsWith("/login/index.html");
    if (isLoginPage) return;
    var token = localStorage.getItem("primataToken");
    var accountPage = /\/(perfil|editar-perfil|notificacoes|configuracoes)\//.test(path);
    if (!accountPage || token) return;
    document.addEventListener("DOMContentLoaded", function () {
        var main = document.querySelector("main");
        if (!main) return;
        main.innerHTML = `
            <section class="auth-required">
                <span class="mini-title">MINHA CONTA // ACCESS_001</span>
                <h1>VOCÊ NÃO<br>ESTÁ LOGADO</h1>
                <p>Entre na sua conta para acessar esta área e visualizar suas informações pessoais.</p>
                <a class="pixel-button" href="../login/index.html">ENTRAR NA CONTA</a>
            </section>`;
        document.body.classList.add("auth-required-page");
    });
})();
