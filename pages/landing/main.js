const trackButtons = document.querySelectorAll(".track-play");
const currentSong = document.getElementById("current-song");
const currentArtist = document.getElementById("current-artist");

trackButtons.forEach((button) => {

    button.addEventListener("click", () => {

        const track = button.closest(".track");

        const song = track.querySelector(".track-data strong").textContent;
        const artist = track.querySelector(".track-data span").textContent;

        currentSong.textContent = song;
        currentArtist.textContent = artist;

        trackButtons.forEach((btn) => {
            btn.textContent = "▶";
        });

        button.textContent = "Ⅱ";
    });

});

const mainPlay = document.querySelector(".main-play");

let playing = false;

mainPlay.addEventListener("click", () => {

    playing = !playing;

    mainPlay.textContent = playing ? "Ⅱ" : "▶";

});