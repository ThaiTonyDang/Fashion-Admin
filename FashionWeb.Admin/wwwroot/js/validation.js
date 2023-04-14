$(document).ready(function () {
    $('#frmProductEdit').validate({
        rules: {
            Name: {
                required: true,
                maxlength: 200,
                minlength: 5
            },          
            Provider: {
                required: true,
                maxlength: 200,
                minlength: 5
            },
            Price: {
                required: true,
                number: true,
                min: 1
            },
            UnitsInStock: {
                required: true,
                number: true,
                min: 0
            },
            Type: {
                required: true,
                maxlength: 200,
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
            UnitsInStock: {
                required: "QUANTITY IN STOCK IS REQUIRED",
                number: "PLEASE INPUT ONLY NUMBER",
                min: "MINIMUM QUANTITY IS 0"
            },
            Type: {
                required: "TYPE PRODUCT IS REQUIRED",
                maxlength: "TOO LONG",
            }
        }
    });
})