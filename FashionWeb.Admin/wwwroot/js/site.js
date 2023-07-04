$(document).on("click", "#closemodal", function () {
    $('#confirm-modal').hide();
})

function loadImage(event) {
    document.getElementById('previewImage').src = window.URL.createObjectURL(event.target.files[0]);
}

function goto() {
    var page = $('.page-number').val();
    if (page == 0) {
        e.preventDefault();
        alert("NO PAGE SELECT");
        return;
    };
    var url = '/products?currentpage=' + page;
    window.location.href = url;
}

function loadSubImage(event) {
    document.getElementById('image_1').src = window.URL.createObjectURL(event.target.files[0]);
    document.getElementById('image_2').src = window.URL.createObjectURL(event.target.files[1]);
    document.getElementById('image_3').src = window.URL.createObjectURL(event.target.files[2]);
}