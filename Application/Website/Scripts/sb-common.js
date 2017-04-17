$(window).load(function () {
    hideLoader();
})

$(function () {
    $('a').click(function () {
        if ($(this).attr("href") != "#")
            showLoader();
    });

    $("form").submit(function (e) {
        if ($(this).valid()) {
            showLoader();
        }
        else {
            e.preventDefault();
        }
    });
});

//$(function () {
//    $('a').click(function () {
//        showLoader();
//    });

//    $("form").submit(function (e) {
//        if ($(this).valid()) {
//            var show_loader = $("input[type=submit]").attr("show-loader");
//            if (show_loader != "no") {
//                showLoader();
//            }
//        }
//        else {
//            e.preventDefault();
//        }
//    });
//});

function onCompleteRJS() {
    hideLoader();
}

function onComplete() {
    hideLoader();
}

function onBegin() {
    showLoader();
}

function showLoader() {
    $(".loader").show();
}

function hideLoader() {
    $(".loader").hide();
}

function DeleteCompleted(element) {
    var content = $("#" + element).text();
    $("#" + element).text('').append('<span class="label label-danger arrowed"><s>' + content + '</s></span>');
    onComplete();
}

function confirmDelete() {
    if (confirm('Are you sure you want to remove this record?')) {
        onBegin();
        return true;
    }
    hideLoader();
    return false;
}

function readURL(input) {
    $('#div_current_images').html('');
    if (input.files && input.files.length > 0) {
        for (var i = 0; i < input.files.length; i++) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#blah').attr('src', e.target.result);
                $('<img />', { id: 'workspace-image-' + i, src: e.target.result, alt: 'Image', width: '100px', height: '100px' }).appendTo($('#div_current_images'));
            }
            reader.readAsDataURL(input.files[i]);
        }
    }
}

function onAddAnotherCompleted(content) {
    $("#editorRows").append(content.responseText);
    hideLoader();
}