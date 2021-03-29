//this function create table by clicking buttons
$(document).ready(function () {
    let pressBtn = 0;    
   
    $("button").click(function () {
        $("button").removeClass("selected");
        $('#columns').empty();
        $(".fa").hide();
        $(this).addClass("selected");
        let getProductAPI = "/Admin/Product/GetAllTest/";
        let lineId = 1;
        let tableRow = "";
        if (pressBtn != this.id) {
            pressBtn = parseInt(this.id);
            $('#arrow_down_' + this.id).hide();
            $('#arrow_up_' + this.id).show();  
        }
        else {
            pressBtn = parseInt(this.id) + 10;
            $('#arrow_down_' +this.id).show();
            $('#arrow_up_' +this.id).hide();
        }
        getProductAPI += pressBtn;
        function displayInOrder(data) {
            $.each(data, function () {
                $.each(this, function (key, value) {
                    if (lineId % 2 == 0) {
                        tableRow = "my-line-secondary";
                    }
                    else {
                        tableRow = "my-line-primary";
                    }
                    lineId++;
                    $("#columns").append(
                        '<div class="row text-center' + ' ' + tableRow + ' ' + 'mr-1 pt-2">' +
                        '<div class="col-4 " >' + value.name + '</div> ' +
                        '<div class="col-2 " >' + value.price + '</div> ' +
                        '<div class="col-2 " >' + value.category.name + '</div> ' +
                        '<div class="col-2 " >' + value.eventType.name + '</div> ' +
                        '<div class="col-2 " >' + '</div> ' +
                        '</div>'
                    );
                });
            });
        }
        $.getJSON(getProductAPI, displayInOrder);
    });
});

//this function create table by default
$(document).ready(function () {   

    $(".fa").hide();
      
        $('#columns').empty();       
        let getProductAPI = "/Admin/Product/GetAllTest/0";
        let lineId = 1;
        let tableRow = "";       
        function displayInOrder(data) {
            $.each(data, function () {
                $.each(this, function (key, value) {

                    if (lineId % 2 == 0) {
                        tableRow = "my-line-secondary";

                    }
                    else {
                        tableRow = "my-line-primary";

                    }
                    lineId++;

                    $("#columns").append(
                        '<div class="row text-center' + ' ' + tableRow + ' ' + 'mr-1 pt-2">' +
                        '<div class="col-4 " >' + value.name + '</div> ' +
                        '<div class="col-2 " >' + value.price + '</div> ' +
                        '<div class="col-2 " >' + value.category.name + '</div> ' +
                        '<div class="col-2 " >' + value.eventType.name + '</div> ' +
                        '<div class="col-2 " >' + '</div> ' +
                        '</div>'
                    );
                });
            });
        }
        $.getJSON(getProductAPI, displayInOrder);  
});


