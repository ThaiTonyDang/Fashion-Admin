$(document).ready(function () {
    $('#frmProductEdit').validate({
        rules: {
            Name: {
                required: true,
                maxlength: 200,
                minlength: 3
            },          
            Provider: {
                required: true,
                maxlength: 200,
                minlength: 3
            },
            Price: {
                required: true,
                number: true,
                min: 1
            },
            QuantityInStock: {
                required: true,
                number: true,
                min: 0
            }
        },
        messages: {
            Name: {
                required: "PRODUCT NAME IS REQUIRED",
                maxlength: "TOO LONG",
                minlength: "TOO SHORT"
            },
            Provider: {
                required: "PROVIDER IS REQUIRED",
                maxlength: "TOO LONG",
                minlength: "TOO SHORT"
            },
            Price: {
                required: "PRICE IS REQUIRED",
                number: "PLEASE INPUT ONLY NUMBER",
                min: "MINIMUM PRICE IS 1 DOLLAR"
            },
            QuantityInStock: {
                required: "QUANTITY IN STOCK IS REQUIRED",
                number: "PLEASE INPUT ONLY NUMBER",
                min: "MINIMUM QUANTITY IS 0"
            }
        }
    });
})