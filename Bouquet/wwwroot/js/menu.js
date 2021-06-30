// Responsible for menu windows /mobile version

const iconExpand = document.getElementById('iconExpand');
const options = document.getElementById('options');
const filter = document.getElementById('filterContainer');
const cardSection = document.getElementById('cardContainer');
const option01 = document.getElementById('option01');
const extraOptions01 = document.getElementById('extra-options01');
const iconCrossSmall01 = document.getElementById('icon-cross-small01');
const extraOptions02 = document.getElementById('extra-options02');
const iconCrossSmall02 = document.getElementById('icon-cross-small02');
const extraOptions03 = document.getElementById('extra-options03');
const iconCrossSmall03 = document.getElementById('icon-cross-small03');
const option02 = document.getElementById('option02');
const option03 = document.getElementById('option03');
const option05 = document.getElementById('option05');
const extraOptions05 = document.getElementById('extra-options05');
const iconCrossSmall05 = document.getElementById('icon-cross-small05');
const extraOptions04 = document.getElementById('extra-options04');
const iconCrossSmall04 = document.getElementById('icon-cross-small04');
const option06 = document.getElementById('option06');
const extraOptions06 = document.getElementById('extra-options06');
const iconCrossSmall06 = document.getElementById('icon-cross-small06');
const option07 = document.getElementById('option07');
const extraOptions07 = document.getElementById('extra-options07');
const iconCrossSmall07 = document.getElementById('icon-cross-small07');
const iconCrossMenu = document.getElementById('icon-cross-menu');

iconExpand.addEventListener('click', processToggle);
iconCrossMenu.addEventListener('click', processToggle);
iconCrossSmall01.addEventListener('click', processCross01);
iconCrossSmall02.addEventListener('click', processCross02);
iconCrossSmall03.addEventListener('click', processCross03);
iconCrossSmall04.addEventListener('click', processCross04);
iconCrossSmall05.addEventListener('click', processCross05);
iconCrossSmall06.addEventListener('click', processCross06);
iconCrossSmall07.addEventListener('click', processCross07);

function processOption01() {
    extraOptions01.classList.replace('hide', 'show-scale');  
}
function processOption02() {
    extraOptions02.classList.replace('hide', 'show-scale');    
}
function processOption03() {
    extraOptions03.classList.replace('hide', 'show-scale');  
}
function processOption04() {
    extraOptions04.classList.replace('hide', 'show-scale');
}
function processOption05() {   
    extraOptions05.classList.replace('hide', 'show-scale');
}
function processOption06() {
    extraOptions06.classList.replace('hide', 'show-scale');
}
function processOption07() {
    extraOptions07.classList.replace('hide', 'show-scale');
}

function processCross01() {
    extraOptions01.classList.replace('show-scale', 'show-scale-rev');
    setTimeout(function () {
        iconExpand.classList.remove('icon-expand-rotate-rev');
        extraOptions01.classList.add('hide');
        extraOptions01.classList.remove('show-scale-rev');
    }, 560);
}
function processCross02() {    
    extraOptions02.classList.replace('show-scale', 'show-scale-rev');
    setTimeout(function () {
        iconExpand.classList.remove('icon-expand-rotate-rev');
        extraOptions02.classList.add('hide');
        extraOptions02.classList.remove('show-scale-rev');
    }, 560);
}
function processCross03() {
    extraOptions03.classList.replace('show-scale', 'show-scale-rev');
    setTimeout(function () {
        iconExpand.classList.remove('icon-expand-rotate-rev');
        extraOptions03.classList.add('hide');
        extraOptions03.classList.remove('show-scale-rev');
    }, 560);
}
function processCross04() {
    extraOptions04.classList.replace('show-scale', 'show-scale-rev');
    setTimeout(function () {
        iconExpand.classList.remove('icon-expand-rotate-rev');
        extraOptions04.classList.add('hide');
        extraOptions04.classList.remove('show-scale-rev');
    }, 560);
}
function processCross05() {
    console.log("Olla");
    extraOptions05.classList.replace('show-scale', 'show-scale-rev');
    setTimeout(function () {
        iconExpand.classList.remove('icon-expand-rotate-rev');
        extraOptions05.classList.add('hide');
        extraOptions05.classList.remove('show-scale-rev');
    }, 560);
}

function processCross06() { 
    extraOptions06.classList.replace('show-scale', 'show-scale-rev');
    setTimeout(function () {
        iconExpand.classList.remove('icon-expand-rotate-rev');
        extraOptions06.classList.add('hide');
        extraOptions06.classList.remove('show-scale-rev');
    }, 560);
}

function processCross07() {
    extraOptions07.classList.replace('show-scale', 'show-scale-rev');
    setTimeout(function () {
        iconExpand.classList.remove('icon-expand-rotate-rev');
        extraOptions07.classList.add('hide');
        extraOptions07.classList.remove('show-scale-rev');
    }, 560);
}

function processToggle() {
    // If there is cross icon 
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
        // If there is not cross icon     
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
