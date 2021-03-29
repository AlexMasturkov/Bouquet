//this function create table by clicking buttons
//$(document).ready(function () {
//    let pressBtn = 0;

//    $("button").click(function () {
//        $("button").removeClass("selected");
//        $('#columns').empty();
//        //$(".fa").hide();
//        $(this).addClass("selected");
//        let getProductAPI = "/Admin/Product/GetAllIndex/";
//        let tableRow = "";

//        pressBtn = parseInt(this.id);

//        getProductAPI += pressBtn;
//        function displayInOrder(data) {
//            $.each(data, function () {
//                $.each(this, function (key, value) {

//                    $("#columns").append(
//                        '<div class="row text-center' + ' ' + tableRow + ' ' + 'mr-1 pt-2">' +
//                        '<div class="col-4 " >' + value.name + '</div> ' +
//                        '<div class="col-2 " >' + value.price + '</div> ' +
//                        '<div class="col-2 " >' + value.category.name + '</div> ' +
//                        '<div class="col-2 " >' + value.eventType.name + '</div> ' +
//                        '<div class="col-2 " >' + '</div> ' +
//                        '<img class="frameContainer card-img-top " src=" '+ value.ImageUrl +' alt="Card Image" />'+
//                        '</div>'
//                    );
//                });
//            });
//        }
//        $.getJSON(getProductAPI, displayInOrder);
//    });
//});

//this function create table by default
$(document).ready(function () { 

    $('#columns').empty();
    let getProductAPI = "/Customer/Home/GetAllIndex/0";    
  
    function displayInOrder(data) {
        $.each(data, function () {
            $.each(this, function (key, value) {
                $("#columns").append(
                   ' <div class="col-12 col-sm-6 col-md-4 col-lg-3 col-xl-2 p-1 trest">'+                  
                  
                    '<div class="card-body p-1 pb-0 m-0 my-shadow-transp" style=" border-radius: .5rem !important; ">' +
                   ' <label class=" p-2 text-center col-12 m-0 font-adjust my-font" style="font-weight: 300;font-size:22px; color: white;">'+
                        value.name +
                    ' </label>' +
                   ' <div class="flw">'+
                       ' <div class="d-flex justify-content-between align-items-center">'+
                           ' <img class="frameContainer card-img-top " src=" ' +value.imageUrl+'" alt="Card Image" />'+
                       ' </div>'+
                    ' </div>' +
                    '<div class="description">'+
                    ' <div class="d-flex justify-content-center ">' +
                    ' <textarea style="font-size:14px; border-radius:5px; " class="p-1 mt-1 text-center">' + value.description + '</textarea>' +
                        '</div>'+
                    '</div>'+
                    ' </div> '+
                    '</div > '
                );
            });
        });
    }
    $.getJSON(getProductAPI, displayInOrder);

    $(this).find("textarea").hide();
    $("div.trest").mouseover(function () {

        $(this).find(".flw").hide();
        $(this).find(".description").show();

        let box = document.querySelector('.trest');
        let width = box.offsetWidth;
        let height = width * 0.86;

        $("textarea").width(width);
        $("textarea").height(height);


    });
    $("div.trest").mouseout(function () {

        $(this).find(".flw").show();
        $(this).find(".description").hide();

    });
});



