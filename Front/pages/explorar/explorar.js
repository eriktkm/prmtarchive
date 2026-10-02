/* Página de exploração pronta para receber dados do Spring Boot. */
document.addEventListener("DOMContentLoaded", function () {
    PrimataAPI.get("/musicas/destaques").then(function (dados) {
        console.log("Destaques recebidos da API:", dados);
    }).catch(function (erro) {
        console.log("API ainda não disponível. A tela continua funcionando com os links.");
    });
});
