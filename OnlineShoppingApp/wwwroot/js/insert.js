function handleBrandChange(selectElement) {
    if (selectElement.value == '-1') {
        $('#otherBrandGroup').show();
    } else {
        $('#otherBrandGroup').hide();
    }
}


$(document).ready(function () {
    // $('#brandSelect').change(function () {
    //     if ($(this).val() == '@int.Parse("-1")') {
    //         $('#otherBrandGroup').show();
    //     } else {
    //         $('#otherBrandGroup').hide();
    //     }
    // });

    // Add new image URL input field
    $('#addImageUrl').click(function () {
        var inputField = '<div class="input-group mb-2">' +
            '<input type="text" class="form-control" name="ImageUrl" />' +
            '<div class="input-group-append">' +
            '<button type="button" class="btn btn-danger remove-url">Remove</button>' +
            '</div>' +
            '</div>';
        $('#imageUrlsContainer').append(inputField);
    });
    // Remove image URL input field
    $(document).on('click', '.remove-url', function () {
        $(this).closest('.input-group').remove();
    });

});