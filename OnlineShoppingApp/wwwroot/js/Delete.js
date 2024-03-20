document.addEventListener("DOMContentLoaded", function () {
    document.addEventListener('click', function (event) {
        if (event.target.classList.contains('delete-product')) {
            event.preventDefault(); // Prevent default action

            let button = event.target;
            let productId = button.getAttribute('data-product-id'); // Use data-product-id attribute

            fetch(`/Product/DeleteProduct/${productId}`, {
                method: 'DELETE'
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Network response was not ok');
                    }
                    // Remove the product card and associated images
                    let productCard = button.closest('.col-lg-5').parentElement;
                    if (productCard) {
                        productCard.remove();
                    } else {
                        console.error('Product card not found!');
                    }
                })
                .catch(error => {
                    console.error('There was a problem with the fetch operation:', error);
                });
        }
    });
});
