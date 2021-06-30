
// Add a listener for when the window resizes
window.addEventListener('resize', productMediaQuery);
let check = 0;
function productMediaQuery() {
    const registerMobile = document.getElementById('registerMobile');
    const registerMain = document.getElementById('registerMain');
    if (window.innerWidth <= 480 && check == 0) {
        registerMain.classList.add('hide');
        registerMobile.classList.remove('hide');
        check = 1;
    }
    else if (window.innerWidth > 480 && check == 1) {
        registerMobile.classList.add('hide');
        registerMain.classList.remove('hide');
        check = 0;
    }
}
