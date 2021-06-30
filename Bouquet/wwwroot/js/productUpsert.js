

// Add a listener for when the window resizes
window.addEventListener('resize', productMediaQuery);

let check = 0;
function productMediaQuery() {
    if (window.innerWidth <= 480 && check == 0) {
        location.reload();
        check = 1;
    }
    else if (window.innerWidth > 480 && check == 1) {
        location.reload();
        check = 0;
    }
}

function validateInput(event) {
    console.log("Ev", event.target);
    const productCategory = document.getElementById('Product_CategoryId');
    const productEvent = document.getElementById('Product_EventTypeId');
    const imageFile = document.getElementById("uploadBox");
    let str = imageFile.value;
    str = str.substring(str.indexOf(".") + 1).toUpperCase();
    if (productCategory[0].selected == true) {
        swal("Error", "Please choose Category", "error");
        return false;
    }
    if (productEvent[0].selected == true) {
        swal("Error", "Please choose EventType", "error");
        return false;
    }
    if (document.getElementById("uploadBox").value == "" && event.target.id == "createProduct") {
        swal("Error", "Please select an image", "error");
        return false;
    }
    if ((str != "JPG" && str != "JPEG" && str != "PNG") && event.target.id == "createProduct") {
        swal("Error", "Please use only image format :JPG, JPEG, or PNG", "error");
        return false;
    }
    return true;
}

function validateInput1(event) {
    console.log("Ev1", event.target.id);
    const productCategory1 = document.querySelector(".category");
    const productEvent1 = document.querySelector(".event");
    const imageFile1 = document.getElementById("uploadBox1");
    let str = imageFile1.value;
    str = str.substring(str.indexOf(".") + 1).toUpperCase();

    if (productCategory1.value == '') {
    swal("Error", "Please choose Category", "error");
        return false;
    }
    if (productEvent1.value == '') {
    swal("Error", "Please choose EventType", "error");
        return false;
    }
    if (document.getElementById("uploadBox1").value == "" && event.target.id =="createProduct") {
    swal("Error", "Please select an image", "error");
        return false;
    }
    if ((str != "JPG" && str != "JPEG" && str != "PNG") && event.target.id == "createProduct") {
    swal("Error", "Please use only image format :JPG, JPEG, or PNG", "error");
        return false;
    }
    return true;
}