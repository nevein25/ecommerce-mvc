

$(document).ready(function () {
    $(".btn-number").click(function (e) {
        e.preventDefault();
        console.log("button click");
        var fieldName = $(this).attr('data-field');
        var type = $(this).attr('data-type');
        var input = $("input[name='quant[" + fieldName + "]']");
        var oldValue = parseFloat(input.val());
        var newVal;

        if (type === 'minus') {
            newVal = oldValue - 1;
        } else {
            newVal = oldValue + 1;
        }

        if (newVal <= 0) {
            newVal = 1;
        }

        input.val(newVal);
        // You might want to update the quantity on the server side here.
    });
});
