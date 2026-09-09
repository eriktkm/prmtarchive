/* =========================
   PRIMATA ARCHIVE
   INTERAÇÕES
========================= */


// =========================
// CURSOR PIXEL
// =========================

const cursor = document.createElement("div");

cursor.classList.add("pixel-cursor");

document.body.appendChild(cursor);

document.addEventListener("mousemove", (event) => {

    cursor.style.left = `${event.clientX}px`;
    cursor.style.top = `${event.clientY}px`;

});


// =========================
// LINKS SUAVES
// =========================

document.querySelectorAll('a[href^="#"]').forEach((link) => {

    link.addEventListener("click", function (event) {

        const targetId = this.getAttribute("href");

        if (targetId === "#") return;

        const target = document.querySelector(targetId);

        if (target) {

            event.preventDefault();

            target.scrollIntoView({
                behavior: "smooth",
                block: "start"
            });

        }

    });

});


// =========================
// ANIMAÇÃO AO ENTRAR NA TELA
// =========================

const animatedElements = document.querySelectorAll(
    ".artist-card, .section-content, .community-box, .final-cta"
);

const observer = new IntersectionObserver(
    (entries) => {

        entries.forEach((entry) => {

            if (entry.isIntersecting) {

                entry.target.classList.add("visible");

                observer.unobserve(entry.target);

            }

        });

    },
    {
        threshold: 0.15
    }
);


animatedElements.forEach((element) => {

    element.classList.add("hidden");

    observer.observe(element);

});


// =========================
// STATUS DO SISTEMA
// =========================

const terminal = document.querySelector(".terminal");

const messages = [
    "SYSTEM_READY",
    "ARCHIVE_ONLINE",
    "DATABASE_CONNECTED",
    "COMMUNITY_READY"
];

let messageIndex = 0;

setInterval(() => {

    messageIndex++;

    if (messageIndex >= messages.length) {
        messageIndex = 0;
    }

    terminal.innerHTML =
        `<span>></span> ${messages[messageIndex]} <span class="blink">_</span>`;

}, 3500);


// =========================
// BOTÕES DOS ARTISTAS
// =========================

const artistButtons = document.querySelectorAll(".arrow-button");

artistButtons.forEach((button, index) => {

    button.addEventListener("click", () => {

        const artistNumber = String(index + 1).padStart(2, "0");

        alert(
            `ARCHIVE_${artistNumber}\n\n` +
            `O perfil deste artista estará disponível em breve.`
        );

    });

});


// =========================
// EFEITO DE GLITCH NO LOGO
// =========================

const logo = document.querySelector(".logo");

setInterval(() => {

    logo.style.transform = "translateX(2px)";

    setTimeout(() => {

        logo.style.transform = "translateX(0)";

    }, 80);

}, 5000);