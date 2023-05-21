$(document).on("click", "#closemodal", function () {
    $('#confirm-modal').hide();
})

function loadImage(event) {
    document.getElementById('previewImage').src = window.URL.createObjectURL(event.target.files[0]);
}
