$(document).ready(function () {
    // Add functionality to add more image URLs
    $('#addImageUrl').click(function () {
        var imageUrlInput = '<div class="input-group mb-2">' +
            '<input type="text" class="form-control" name="ImageUrl" />' +
            '<div class="input-group-append">' +
            '<button type="button" class="btn btn-danger remove-url">Remove</button>' +
            '</div>' +
            '</div>';
        $('#imageUrlsContainer').append(imageUrlInput);
    });

    // Add functionality to remove image URLs
    $(document).on('click', '.remove-url', function () {
        $(this).closest('.input-group').remove();
    });
});