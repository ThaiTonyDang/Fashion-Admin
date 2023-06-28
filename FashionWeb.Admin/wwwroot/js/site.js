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