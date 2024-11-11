let page = 1;
const pageSize = 10;
let loading = false;

function loadImages() {
    if (loading) return;
    loading = true;

    $.get(`/Image/LoadImages?page=${page}&pageSize=${pageSize}`, function (data) {
        $("#imageGrid").append(data);
        loading = false;
        page++;
    });
}

loadImages();

$(window).scroll(function () {
if ($(window).scrollTop() + $(window).height() >= $(document).height() - 100) {
    loadImages();
}
});

function submitPost() {
const formData = new FormData(document.getElementById('uploadForm'));

fetch('/Image/Upload', {
    method: 'POST',
    body: formData
})
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            const modalElement = document.getElementById('uploadModal');
            const modalInstance = bootstrap.Modal.getInstance(modalElement);
            modalInstance.hide();
            addImageToGallery(data.image);
        } else {
            alert('Error uploading image: ' + data.message);
        }
    })
    .catch(error => console.error('Error:', error));
}

function addImageToGallery(image) {
const imageContainer = document.querySelector('.image-container');
const newImageCard = `
        <div class="image-card">
            <div class="image-wrapper">
                    <img src="${image.url}" alt="User Image" />
            </div>

            <div class="image-details">
                <p class="description">${image.Description}</p>
                <p class="uploader">Uploaded by: <strong>${image.User.Username}</strong></p>
            </div>
        </div>`;
imageContainer.insertAdjacentHTML('beforeend', newImageCard);
}
