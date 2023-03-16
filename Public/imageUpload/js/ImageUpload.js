
function loaderEnable() {
    $("#ajaxPreloader").addClass("LoaderEnable");
}
function loaderDisable() {
    $("#ajaxPreloader").removeClass("LoaderEnable");
}


$(document).ready(function () {

    $("#imageBrowes").change(function () {

        var File = this.files;

        if (File && File[0]) {
            ReadImage(File[0]);
        }

    })

});

var ReadImage = function (file) {

    var reader = new FileReader;
    var image = new Image;

    reader.readAsDataURL(file);
    reader.onload = function (_file) {

        image.src = _file.target.result;
        image.onload = function () {

            var height = this.height;
            var width = this.width;
            var type = file.type;
            var size = ~~(file.size / 1024) + "KB";

            if ((file.size / 1024) > 1024) {
                $("#uploadedImage").text("You can not upload over 1 megabyte sized image.");
                return;
            }

            $("#uploadedImage").text(" ");
            $("#targetImg").attr('src', _file.target.result);
            $("#description").text("Size:" + size + ", " + height + "X " + width + ", " + type + "");
            $("#imgPreview").show();
        }
    }
}

var Uploadimage = function () {

    loaderEnable();

    var file = $("#imageBrowes").get(0).files;
    var imageId = $("#targetImg").attr("alt");
    var ResizeCount = (100 - $("#ex7").attr("value")) / 100;

    if (!imageId) {
        imageId = 0;
    }

    var data = new FormData;
    data.append("ImageFile", file[0]);
    data.append("ImageId", imageId);
    data.append("ResizeCount", ResizeCount);
    console.log(ResizeCount);
    ClearPreview();

    $.ajax({

        type: "Post",
        url: "/Image/ImageUpload",
        data: data,
        contentType: false,
        processData: false,
        success: function (response) {

            var image = `
                    <div class="img-updel">
                        <img src='/Uploads/` + response + `'/>
                        <input value='` + response + `' style='display:none;' name='NewImagePath' />
                    </div>
                    `
            $("#uploadedImage").append(image);

            loaderDisable();

        }

    });
}

var ClearPreview = function () {
    $("#imageBrowes").val('');
    $("#description").text('');
    $("#imgPreview").hide();
    $("#uploadedImage").html('');
}

// DELETEING IMAGE IN EDIT => TAKING IMAGE ID 
$(".imageEditRemoveIcon").click(function () {
    var Con = confirm("are you sure ?");
    if (Con == true) {
        var imgId = $(this).prevAll('input').attr('value');
        if (imgId != null) {
            loaderEnable();
            DeleteImage(imgId);
        }

        $("#uploadedImage").html('<p style="color:red;">Image deleted!</p>');

        $(this).prevAll('img').parent().remove();
        $(this).remove();
    }
});

// DELETEING IMAGE IN EDIT => SENDING SELECTED IMAGE ID VIA AJAX
//var urlDelete = 'http://localhost:44382/admin/image/deleteImage/';

function DeleteImage(imgId) {
     //$.ajaxSetup({'cache':true});
    var dataId = { 'ImageId': imgId };
    $.ajax({
        type: "POST",
        url: "/Admin/Image/deleteImage/" + imgId,
        data: dataId,
         //method: "POST",
        sucess: function (data) {
            // var answer = "<span height='100' width='100'>"+data.reslt+"</span>" ;
            // $(".elebele").append(answer);
        },
        error: function (data) {
            console.log(data);
        }
    });

    loaderDisable();
}

// With JQuery
$("#ex7").slider({
    tooltip: 'always',
    formatter: function (value) {
        return 'will resize ' + value + ' %';
    },
});

$("#ex7-enabled").click(function () {
    if (this.checked) {
        // With JQuery
        $("#ex7").slider("enable");

        // Without JQuery
        //slider.enable();
    }
    else {
        // With JQuery
        $("#ex7").slider("disable");

        // Without JQuery
        //slider.disable();
    }
});
