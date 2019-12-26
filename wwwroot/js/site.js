// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function PredictMovieClicked(movieIndex) {
    $.ajax({
        dataType: 'json',
        type: 'GET',
        url: '/User/PredictMovie?movieIndex=' + movieIndex,
        val: movieIndex,
        success: function (result) {
            if (result === -1) {
                alert("Message: You should rate more than 20 movie");
            } else {
                alert("User Based Prediction : " + result);
            }
        }
    })
}