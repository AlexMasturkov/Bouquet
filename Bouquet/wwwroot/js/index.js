let apiQuotes = [];
const loader = document.getElementById('loader');
const cardContainer = document.getElementById('cardContainer');
const filterContainer = document.getElementById('filterContainer');
const columns = document.getElementById('columns');
const extraFilters = document.getElementById('extraFilters');
const iconCrossFilter = document.getElementById('iconCrossFilters');

let favoriteArray = [];//declare or get local storage

let amountItems = 3;
let pagingtId = 0;
let maxPaginationBtns = 4;

window.addEventListener('resize', productMediaQuery);
let check = 0;
function productMediaQuery() {
    if (window.innerWidth <= 480 && check == 0) {
        getQuotes('0');
        check = 1;       
    }
    else if (window.innerWidth > 480 && check == 1) {
        getQuotes('0');
        check = 0;       
    }
}

window.addEventListener('resize', checkAmountItems);
//Update amount of pagination buttons of main page
function checkAmountItems(){    
    if (window.innerWidth <= 480) {
        amountItems = 100;   
    }
    if (window.innerWidth <= 680 && window.innerWidth >481) {
        amountItems = 3;      
    }
    else if (window.innerWidth <= 840 && window.innerWidth > 681 ) {
        amountItems = 4;      
    }
    else if (window.innerWidth <= 1080 && window.innerWidth > 841) {
        amountItems = 5;      
    }
    else {
        amountItems = 6;    
    }
    buildProducts(0);
}

function processCrossFilter() {  
    filterContainer.classList.remove('hide');
    extraFilters.classList.replace('show-scale', 'show-scale-rev');
    setTimeout(function () {     
        extraFilters.classList.add('hide');
        extraFilters.classList.remove('show-scale-rev');
        cardContainer.classList.remove('z-index-1');
        filterContainer.classList.remove('z-index-1');
    }, 560);
}

//Show loading
function loading() {
    loader.hidden = false;   
    cardContainer.classList.add('hide');
    document.getElementById('paginationMain').classList.add('hide');   
}

//Hide loading
function complete() {
    cardContainer.classList.remove('hide');
    document.getElementById('paginationMain').classList.remove('hide');  
    loader.hidden = true;  
    checkAmountItems();
}

// Get/update products data from API
async function getQuotes(filter) {
    document.getElementById('noItemsInfo').innerHTML = "";
    loading();
    if (filter === 'all') {
        filter = '0';
    }
    const apiUrl = '/Customer/Home/GetAllIndex/' + filter;   
    try {
        const response = await fetch(apiUrl);
        apiQuotes = await response.json();
        setTimeout(function () {
            complete();
        }, 200);     
    } catch (error) {
        //Catch error here
        console.log("Error");
    }
}
//First call API to get all products .. '0' means all/no filters
getQuotes('0');

function buildProducts(id) {
    let productColumns = apiQuotes.data;   
    document.getElementById('noItemsInfo').innerHTML = "";    
    let iteratorShow = 0;
    let hideElement = '';
    let productsText = '';
    let pagination = '';
    let iterator ='icon-heart-default';
    if (localStorage.getItem('favoriteProducts')) {
        favoriteArray = JSON.parse(localStorage.favoriteProducts);
    }
/*Build pagination*/
    if (productColumns.length > 0) {
        let lastPaging = 0;
        for (let i = 1; i < productColumns.length / amountItems + 1; i++) {
            if (i <= maxPaginationBtns) {
                const paginationMain = ` <a onclick="paginationElement(event)" class="pagination-element" id="${i}">${i}</a> `;
                pagination = pagination + paginationMain;
            } else {
                const paginationCustom = ` <a onclick="paginationElement(event)" class="pagination-element hide" id="${i}">${i}</a> `;
                pagination = pagination + paginationCustom;
            }
            lastPaging = i;
        }
        document.getElementById('paginationMain').innerHTML = '<a onclick="paginationElement(event)" id="1" style="cursor: pointer;">&laquo;</a>'  + pagination + `<a onclick="paginationElement(event)" id="${lastPaging}" style="cursor: pointer;">&raquo;</a>`;
    }
    else {
        document.getElementById('paginationMain').innerHTML = '<a href="#">&laquo;</a>' + '<a class="pagination-element " >0</a>' + '<a href="#">&raquo;</a>';
        if (productColumns.length == 0) {
            document.getElementById('noItemsInfo').innerHTML = '<div class="p-1 background-medium-grey m-t-20"><p class="p-15 z-index-10 font-14 text-center background-light-grey text-info"> No Items </p></div>';
            cardContainer.classList.add('hide');
        }
    }
    for (let element of productColumns) { 
        if (favoriteArray.includes(element.id)) { iterator = 'icon-heart-active'; }
        else { iterator = 'icon-heart-default';}       
        iteratorShow++;     
        if (iteratorShow > amountItems && window.innerWidth >= 481) { hideElement = 'hide'; }
        const productText = `        
        <div class="card w3-container ${hideElement} background-light-grey shadow21 card-id">
                <i class="${iterator}" id="favorite${element.id}" onclick="updateFavorites(event)"></i>
                <div class="bouquet-image" id="image${element.id}">
                    <a href="/Customer/Home/Details/${element.id}">
                        <img src="${element.imageUrl}" class="w3-image card-image" style="width:100%" alt="Flower">
                        </a>
                    </div>
                    <div class="card-info">
                        <div class="text-name font-bolder">${element.name}</div>
                        <div class="text-name p-b-15">$ ${element.price}</div>
                    </div>
                </div>`;
        productsText = productsText + productText;
    }
    cardContainer.innerHTML = productsText + ' <div class="bottom-fill"></div>';//This trace is for last card to fix view
}

//create and update local storage for favorite product function
function updateFavorites(event) {
    const id = event.target.id.substring(8);  
    if (localStorage.getItem('favoriteProducts')) {
        favoriteArray = JSON.parse(localStorage.favoriteProducts);
        if (favoriteArray.includes(parseInt(id))) {
            const index = favoriteArray.indexOf(parseInt(id));
            if (index != -1) {
                favoriteArray.splice(index, 1);
                document.getElementById(event.target.id).classList.replace('icon-heart-active', 'icon-heart-default');
                localStorage.setItem('favoriteProducts', JSON.stringify(favoriteArray));
            }
        }
        else {
            favoriteArray.push(parseInt(id));
            localStorage.setItem('favoriteProducts', JSON.stringify(favoriteArray));
            document.getElementById(event.target.id).classList.replace('icon-heart-default', 'icon-heart-active');           
        }
    }
    else {
        favoriteArray.push(parseInt(id));
        localStorage.setItem('favoriteProducts', JSON.stringify(favoriteArray));
        document.getElementById(event.target.id).classList.replace('icon-heart-default', 'icon-heart-active');     
    }  
}

/*This function update active class on pressed button and hide/show particular amount of product*/
function paginationElement(event) {
    const elementId = parseInt(event.target.id);
    const startElement = amountItems * (elementId -1);
    const endElement = amountItems * elementId;
    let hideIterator = 0; 
    let padingIterator = 0;
    const paginationElements = document.querySelectorAll('.pagination-element');
    const cardList = document.querySelectorAll('.card-id');

    for (element of paginationElements) {
        element.classList.remove('active');
        element.classList.add('hide');
        padingIterator++;
        if (padingIterator >= elementId - 1 && padingIterator < elementId + maxPaginationBtns - 1) {
            element.classList.remove('hide');           
        } else {
            if (elementId == 1 && element.id == maxPaginationBtns) {
                element.classList.remove('hide');
            }
            if (padingIterator > paginationElements.length - maxPaginationBtns && padingIterator < elementId) {
                element.classList.remove('hide');               
            }
        }
    }
    document.getElementById(elementId).classList.add('active');
    for (let cardElement of cardList) {
        if (hideIterator >= startElement && hideIterator < endElement) {
            cardElement.classList.remove('hide');

        } else {
            cardElement.classList.add('hide');
        }
        hideIterator++;
    }
}

function filterByElement(event) {
    processCrossFilter();
    const element = event.target; 
    getQuotes(element.id);
}
function filterMore() {  
    extraFilters.classList.add('show-scale');
    extraFilters.classList.remove('hide');
    filterContainer.classList.add('z-index-1');
    cardContainer.classList.add( 'z-index-1');
}

/*function closeStatusMessage(event) {
    console.log(event.target);
}*/




   

   
