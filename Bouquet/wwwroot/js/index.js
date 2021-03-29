


$(document).ready(function () {
    function displayWindowSize() {
        // Get width and height of the window excluding scrollbars
        var w = document.documentElement.clientWidth;
        var h = document.documentElement.clientHeight;

        // Display result inside a div element
        //document.getElementById("result").innerHTML = "Width: " + w + ", " + "Height: " + h;
        document.getElementById("rust").innerHTML = w;

        if (w / 4 < 300 ) {
            $("div.trest").width(280);
        }
        else if (w / 4 > 360 ) {
            $("div.trest").width(280);
        }      
        else {
            $("div.trest").width(w / 4);
        }

      
    }

    // Attaching the event listener function to window's resize event
    window.addEventListener("resize", displayWindowSize);

    // Calling the function for the first time
    displayWindowSize();
   




    //let mainFrame = document.querySelector('main');
    //let widthMain = box.offsetWidth;
    //$('#rust').empty();
   

    $(this).find(".description").hide();
    let mineTest = $(this).find("#rust").addClass("my-button-return");
    let mainFrame = $(this).find("main");
    let boxs = document.querySelector('#main-body');

    let widthMain = $(document).width();
   // window.addEventListener('resize', reportWindowWidth);

    function reportWindowWidth() {
        mineTest.text(w);
        //$("#header-top").width(widthMain / 2);
    };


   

   

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

