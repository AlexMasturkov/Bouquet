const iconExpand = document.getElementById('icon-expand');
const options = document.getElementById('options');
const filter = document.getElementById('filter-container');
const cardSection = document.getElementById('card-container');
const option01 = document.getElementById('option01');
const extraOptions01 = document.getElementById('extra-options01');
const iconCrossSmall01 = document.getElementById('icon-cross-small01');
const extraOptions02 = document.getElementById('extra-options02');
const iconCrossSmall02 = document.getElementById('icon-cross-small02');
const extraOptions03 = document.getElementById('extra-options03');
const iconCrossSmall03 = document.getElementById('icon-cross-small03');
const option02 = document.getElementById('option02');
const option03 = document.getElementById('option03');
const iconCrossMenu = document.getElementById('icon-cross-menu');


iconExpand.addEventListener('click', processToggle);
iconCrossMenu.addEventListener('click', processToggle);

option01.addEventListener('click', processOption01);
option02.addEventListener('click', processOption02);
option03.addEventListener('click', processOption03);

iconCrossSmall01.addEventListener('click', processCross01);
iconCrossSmall02.addEventListener('click', processCross02);
iconCrossSmall03.addEventListener('click', processCross03);

function processOption01() {
    extraOptions01.classList.remove('hide');
}
function processOption02() {
    extraOptions02.classList.remove('hide');
}
function processOption03() {
    extraOptions03.classList.remove('hide');
}

function processCross01() {
    extraOptions01.classList.add('hide');
}
function processCross02() {
    extraOptions02.classList.add('hide');
}
function processCross03() {
    extraOptions03.classList.add('hide');
}

function processToggle() {
    // If is cross icon
    if (iconExpand.classList.contains('icon-cross')) {
        filter.classList.remove('non-visible', 'z-index-1');
        filter.classList.add('visible');
        extraOptions01.classList.add('hide');
        extraOptions02.classList.add('hide');
        extraOptions03.classList.add('hide');
        cardSection.classList.remove('non-visible', 'z-index-1');
        cardSection.classList.add('visible')
        options.classList.remove('show-scale');
        options.classList.add('show-scale-rev');
        iconExpand.classList.add('icon-expand');
        iconExpand.classList.add('icon-expand-rotate-rev');
        iconExpand.classList.remove('icon-cross');
        iconExpand.classList.remove('icon-cross-rotate');       
        setTimeout(function () {
            iconExpand.classList.remove('icon-expand-rotate-rev');
            options.classList.add('hide');
            options.classList.remove('show-scale-rev');           
        }, 560);

    }
    else {
        // If is not cross icon

       /* console.log("Fileter",filter);*/
        options.classList.remove('hide');
        filter.classList.add('non-visible', 'z-index-1');
        filter.classList.remove('visible');
        cardSection.classList.add('non-visible','z-index-1');
        cardSection.classList.remove('visible');
        options.classList.add('show-scale');
        iconExpand.classList.add('icon-expand-rotate');
        iconExpand.classList.add('icon-cross');
        iconExpand.classList.add('icon-cross-rotate');
        iconExpand.classList.remove('icon-expand');
        iconExpand.classList.remove('icon-expand-rotate');
    }

}
