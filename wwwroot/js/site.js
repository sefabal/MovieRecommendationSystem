function PredictMovieClicked(movieIndex) {
    $.ajax({
        dataType: 'json',
        type: 'GET',
        url: '/User/PredictMovie?movieIndex=' + movieIndex,
        val: movieIndex,
        success: function (result) {
            if (result.message !== null) {
                alert(result.message);
            } else {
                var userBased = result.userResult;
                var itemBased = result.itemResult;
                alert(" User Based Prediction : " + userBased + "\n Item Based Prediction : " + itemBased);
            }
        }
    })
}