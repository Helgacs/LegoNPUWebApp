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