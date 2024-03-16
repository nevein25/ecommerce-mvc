// Get all radio buttons with name "news"
var radioButtons = document.querySelectorAll('input[name="news"]');

// Add event listener to each radio button
radioButtons.forEach(function (radio) {
    radio.addEventListener('change', function () {
        // Check if the radio button is checked
        if (this.checked) {
            // Get the delivery cost associated with the selected shipping option
            var deliveryCost = this.dataset.deliveryCost;

            // Update the selected shipping option in the <li> element
            document.getElementById("selected-shipping").innerText = '$ ' + deliveryCost;

            // Update the "You Pay" total
            updateTotal(deliveryCost);
        }
    });
});


// Function to update the "You Pay" total
function updateTotal(deliveryCost) {
    var cartTotal = parseFloat(document.querySelector('.cart-total-amount span').innerText.replace("$", ""));

    // var cartTotal = this.dataset.cartTotal;
    console.log(cartTotal);


    // Calculate the total
    var total = cartTotal + parseFloat(deliveryCost);

    // Update the "You Pay" total
    document.querySelector('.last span').innerText = '$' + total.toFixed(2);
}



function checkout(count) {
    // Get the selected delivery method
    var selectedDeliveryMethod = document.querySelector('input[name="news"]:checked');

    // If a delivery method is selected, proceed to checkout
    if (count == 0) {
        alert('Your cart is empty');

    }
    else if (!selectedDeliveryMethod) {
        alert('Please choose a delivery method before proceeding to checkout.');

    }
    else {

        window.location.href = '/Home/index';

    }
}

