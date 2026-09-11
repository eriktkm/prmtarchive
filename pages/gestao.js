
document.querySelectorAll(".demo-action").forEach(function(button) {
    button.addEventListener("click", function() {
        alert("Interface demonstrativa: esta função será conectada ao backend.");
    });
});

document.querySelectorAll("[data-confirm]").forEach(function(button) {
    button.addEventListener("click", function() {
        alert("Ação demonstrativa: " + button.getAttribute("data-confirm"));
    });
});
