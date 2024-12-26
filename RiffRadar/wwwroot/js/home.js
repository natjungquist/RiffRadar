// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', function () {
    console.log('script files loaded.');
},

    document.getElementById('createPlaylistForm').addEventListener('submit', function (event) {
        var playlistNameInput = document.getElementById('playlistName');
        var errorMessage = document.getElementById('error-message');

        if (playlistNameInput.value.trim() === '') {
            event.preventDefault(); // Prevent the form from submitting
            errorMessage.style.display = 'block'; // Show the error message
        } else {
            errorMessage.style.display = 'none'; // Hide the error message
        }
    })
);
