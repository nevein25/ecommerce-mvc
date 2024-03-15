document.addEventListener("DOMContentLoaded", function () {
    let stars = document.querySelectorAll('.star-button');

    stars.forEach(function (star) {
        star.addEventListener('click', function () {
            let rating = this.getAttribute('data-value');
            document.getElementById('NumOfStars').value = rating;
            console.log("Rating updated: " + rating);
        });
    });


    for (var i = 0; i < rating; i++) {
        starButtons[i].classList.add('fas');
        starButtons[i].classList.remove('far');

    }

    for (var i = rating; i < 5; i++) {

        starButtons[i].classList.add('far');
        starButtons[i].classList.remove('fas');

    }
});

