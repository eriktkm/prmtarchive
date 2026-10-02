// PRIMATA ARCHIVE - PLAYER E INTERAÇÕES PRINCIPAIS

document.addEventListener("DOMContentLoaded", function () {

    var trackButtons = document.querySelectorAll(".track-play");
    var currentSong = document.getElementById("current-song");
    var currentArtist = document.getElementById("current-artist");
    var mainPlay = document.querySelector(".main-play");
    var playlistButton = document.getElementById("play-playlist");

    var playing = false;
    var currentTrack = null;

    function tocarMusica(button) {
        var track = button.closest(".track");
        if (!track) return;

        var songElement = track.querySelector(".track-data strong");
        var artistElement = track.querySelector(".track-data span");

        if (!songElement || !artistElement) return;

        if (currentSong) currentSong.textContent = songElement.textContent;
        if (currentArtist) currentArtist.textContent = artistElement.textContent;

        trackButtons.forEach(function (btn) {
            btn.textContent = "▶";
        });

        button.textContent = "Ⅱ";
        currentTrack = track;
        playing = true;

        if (mainPlay) mainPlay.textContent = "Ⅱ";
    }

    trackButtons.forEach(function (button) {
        button.addEventListener("click", function () {
            tocarMusica(button);
        });
    });

    if (playlistButton) {
        playlistButton.addEventListener("click", function () {
            var firstTrack = document.querySelector(".track-play");
            if (firstTrack) {
                tocarMusica(firstTrack);
                window.scrollTo({ top: document.querySelector(".track-list").offsetTop - 30, behavior: "smooth" });
            }
        });
    }

    if (mainPlay) {
        mainPlay.addEventListener("click", function () {
            if (!currentTrack) {
                var firstTrack = document.querySelector(".track-play");
                if (firstTrack) {
                    tocarMusica(firstTrack);
                    return;
                }
            }

            playing = !playing;
            mainPlay.textContent = playing ? "Ⅱ" : "▶";

            if (currentTrack) {
                var currentButton = currentTrack.querySelector(".track-play");
                if (currentButton) currentButton.textContent = playing ? "Ⅱ" : "▶";
            }
        });
    }

    // Botões de voltar/avançar do player
    var playerButtons = document.querySelectorAll(".player-controls button");
    if (playerButtons.length >= 3) {

        playerButtons[0].addEventListener("click", function () {
            var tracks = Array.from(document.querySelectorAll(".track"));
            if (!tracks.length || !currentTrack) return;

            var index = tracks.indexOf(currentTrack);
            if (index > 0) {
                var button = tracks[index - 1].querySelector(".track-play");
                if (button) tocarMusica(button);
            }
        });

        playerButtons[2].addEventListener("click", function () {
            var tracks = Array.from(document.querySelectorAll(".track"));
            if (!tracks.length || !currentTrack) return;

            var index = tracks.indexOf(currentTrack);
            if (index < tracks.length - 1) {
                var button = tracks[index + 1].querySelector(".track-play");
                if (button) tocarMusica(button);
            }
        });
    }

});
