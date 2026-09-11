
document.querySelectorAll(".pixel-button").forEach(button => {
    if (button.tagName === "BUTTON" && !button.closest("form")) {
        button.addEventListener("click", () => {
            const label = button.textContent.trim();
            if (label.includes("PUBLICAR") || label.includes("CRIAR") || label.includes("SALVAR")) {
                alert("Interface demonstrativa: esta ação ainda será conectada ao backend.");
            }
        });
    }
});
