// Add a listener for when the window resizes
window.addEventListener('resize', productMediaQuery);
let check = 0;
function productMediaQuery() {
    if (window.innerWidth <= 480 && check == 0) {      
        getQuotes("GetOrderList?status=21inprocess");
        check = 1;
    }
    else if (window.innerWidth > 480 && check == 1) {        
        getQuotes("GetOrderList?status=21inprocess");
        check = 0;
    }
}
let apiQuotes = [];
const loader = document.getElementById('loader');
const tableContainer = document.getElementById('tableContainer');
const getStatus = document.getElementsByName('orderStatus');
const getStatus1 = document.getElementsByName('orderStatus1');
let currentStatus = '';

//functions to check Radio Button to update order items
function radioChanged(event) {
    getQuotes("GetOrderList?status=" + '21' + event.target.value);    
}
function radioChanged1(event) {
    getQuotes("GetOrderList?status=" + '21' + event.target.value.slice(0, -1));  
}
document.querySelectorAll("input[name='orderStatus']").forEach((input) => {
    input.addEventListener('click', radioChanged);
});
document.querySelectorAll("input[name='orderStatus1']").forEach((input) => {
    input.addEventListener('click', radioChanged1);
});

function orderByElement(event) {
    let arrange = '';
    const element = event.target;   
    if (element.classList.value === 'icon-arrow-top') {      
        arrange = '2';
    } else {      
        arrange = '1';
    }
    if (window.innerWidth <= 480) {
        for (let i = 0; i < getStatus1.length; i++) {         
            if (getStatus1[i].checked) {
                currentStatus = (getStatus1[i].value).slice(0, -1);               
            }
        }
    } else {
        for (let i = 0; i < getStatus.length; i++) {            
            if (getStatus[i].checked) {              
                currentStatus = getStatus[i].value;
            }
        }
    }
   
switch (element.id + currentStatus) {
    case "orderCustomerinprocess":
        getQuotes("GetOrderList?status=" + arrange + "1inprocess");           
        break;
    case "orderStatusinprocess":
        getQuotes("GetOrderList?status=" + arrange + "2inprocess");           
        break;
    case "orderPriceinprocess":
        getQuotes("GetOrderList?status=" + arrange + "3inprocess");           
        break;
    case "orderCustomerpending":
        getQuotes("GetOrderList?status=" + arrange + "1pending");      
        break;
    case "orderStatuspending":
        getQuotes("GetOrderList?status=" + arrange + "2pending");      
        break;
    case "orderPricepending":
        getQuotes("GetOrderList?status=" + arrange + "3pending");      
        break;
    case "orderCustomercompleted":
        getQuotes("GetOrderList?status=" + arrange + "1completed");      
        break;
    case "orderStatuscompleted":
        getQuotes("GetOrderList?status=" + arrange + "2completed");      
        break;
    case "orderPricecompleted":
        getQuotes("GetOrderList?status=" + arrange + "3completed");      
        break;
    case "orderCustomerrejected":
        getQuotes("GetOrderList?status=" + arrange + "1rejected");      
        break;
    case "orderStatusrejected":
        getQuotes("GetOrderList?status=" + arrange + "2rejected");     
        break;
    case "orderPricerejected":
        getQuotes("GetOrderList?status=" + arrange + "3rejected");     
        break;

    case "orderCustomerall":
        getQuotes("GetOrderList?status=" + arrange + "1all");     
        break;
    case "orderStatusall":
        getQuotes("GetOrderList?status=" + arrange + "2all");     
        break;
    case "orderPriceall":
        getQuotes("GetOrderList?status=" + arrange + "3all");     
        break;
    }
}

let amountItems = 5;
let pagingtId = 0;
let maxPaginationBtns = 3;

//create cart item elements function
function buildCart() {
    let cartColumns = apiQuotes.data;  
    const optionStatus = apiQuotes.optionStatus;//Get response from API to find column use for sorting(Customer - 1, Status -2 , Price -3)
    const optionArrange = apiQuotes.optionArrange;//Get response from API to update Arrow Sort By
    let iconArrow1 = 'icon-arrow-bottom';
    let iconArrow2 = 'icon-arrow-bottom';
    let iconArrow3 = 'icon-arrow-bottom';
    let cartInfo = '';
    let optionText = '';
    let backgroundOption = 'background-light-grey';
    let iterator = 0;
    if (optionArrange === '1' &&  optionStatus === '1') {     
           iconArrow1 = 'icon-arrow-top';      
    }
    if (optionArrange === '1' && optionStatus === '2') {
        iconArrow2 = 'icon-arrow-top';
    }
    if (optionArrange === '1' && optionStatus === '3') {
        iconArrow3 = 'icon-arrow-top';
    }
    if (window.innerWidth <= 480)
    {
        if (cartColumns.length != 0) {
            for (let element of cartColumns) {
                if (iterator % 2 == 0) { backgroundOption = 'background-bright-grey'; } else { backgroundOption = 'background-light-grey'; }
                optionText = `
            <div class="grid-order-body font-mobile-11 p-t-10 m-t-1 ${backgroundOption}"> 
            <div class="item1 p-5 font-mobile-11" >${element.applicationUser.name.substring(0, 14)}</div>
            <div class="item1 p-5 font-mobile-11" >${element.orderStatus}</div>
            <div class="item1 p-5 font-mobile-11" >${element.orderTotal}</div >
            <div class="item1 "><a href="/Admin/Order/Details/${element.id}" ><i class="icon-check-in-box i-btn-sm i-border-success3"></i> </a>                               
           </div ></div>
        `;
                iterator++;
                const myText = `<div>${optionText} </div>`;
                cartInfo = cartInfo + myText;
            }
        } else {

            let status = '';
            for (let i = 0; i < getStatus1.length; i++) {               
                if (getStatus1[i].checked) {
                    status = (getStatus1[i].value).slice(0, -1);                  
                }
            }
         cartInfo = `
            <div class="item-row-right p-5 background-bright-grey m-5 m-t-10 border-5 font-13 " style="height:70px;">
                <p class="text-center p-5 text-info"> There are no ${status} items </p>
            </div>
             <hr class="width-max">
            <div class="p-5"></div>
            <div class="p-h-10 text-center ">
               <a href="../" class="btn-wide btn-success  p-t-10 rounded-5">Return to Shopping </a>           
            </div>
            <div class="p-5"></div>
            <hr class="width-max">`;
        }
        columns.innerHTML = '<hr class="width-max" />' + `<div class="grid-order-mobile font-mobile-11"><div class="order-item1-mobile" ><div class="font-mobile-11">CUSTOMER </div> <div class="${iconArrow1}" onclick="orderByElement(event)"
            id="orderCustomer" ></div></div>   <div class="order-item1-mobile" > <div class="font-mobile-11"> STATUS</div><div class="${iconArrow2}" onclick="orderByElement(event)" id="orderStatus"></div>
            </div><div class="order-item1-mobile " > <div class="font-mobile-11" >$ </div><div class="${iconArrow3}"onclick="orderByElement(event)" id="orderPrice"></div></div>
            <div class="order-item1-mobile m-0 class="font-mobile-11""> UPDATE </div></div>`+ cartInfo + ' <div class=" " ></div>';
    }
    else {
        let pagination = '';
        let iteratorShow = 0;
        let hideElement = '';
        /*Build pagination*/
        if (cartColumns.length > 0) {
            let lastPaging = 0;
            for (let i = 1; i < cartColumns.length / amountItems + 1; i++) {
                if (i <= maxPaginationBtns) {
                    const paginationCustom = ` <a onclick="paginationElement(event)" class="pagination-element" id="${i}">${i}</a> `;
                    pagination = pagination + paginationCustom;
                } else {
                    const paginationCustom = ` <a onclick="paginationElement(event)" class="pagination-element hide" id="${i}">${i}</a> `;
                    pagination = pagination + paginationCustom;
                }
                lastPaging = i;
            }
            document.getElementById('paginationCustom').innerHTML = '<a onclick="paginationElement(event)" id="1" style="cursor: pointer;">&laquo;</a>' + pagination + `<a onclick="paginationElement(event)" id="${lastPaging}" style="cursor: pointer;">&raquo;</a>`;
        }
        else {
            document.getElementById('paginationCustom').innerHTML = '<a href="#">&laquo;</a>' + '<a class="pagination-element " >0</a>' + '<a href="#">&raquo;</a>';
        }
        if (cartColumns.length != 0) {          
            for (let element of cartColumns) {
                if (iterator % 2 == 0) { backgroundOption = 'background-bright-grey'; } else { backgroundOption = 'background-light-grey'; }
                iteratorShow++;           
                if (iteratorShow > amountItems) { hideElement = 'hide'; }
                optionText = `
                <div class="grid-order-body ${hideElement} item-id p-15 ${backgroundOption}"> 
                <div class="item1" >${element.applicationUser.name.substring(0, 14)}</div>       
                <div class="item1" >${element.orderStatus}</div>
                <div class="item1" >${element.orderTotal}</div >   
                <div class="item1"><a href="/Admin/Order/Details/${element.id}" ><i class="icon-check-in-box i-btn-sm i-border-success3"></i> </a>                               
               </div ></div>`;
                iterator++;
                const myText = `<div>${optionText} </div>`;
                cartInfo = cartInfo + myText;
            }
        } else {
            let status = '';
            for (let i = 0; i < getStatus.length; i++) {
                if (getStatus[i].checked) {
                    status = (getStatus[i].value);
                }
            }
            cartInfo = `
            <div class="item-row-right p-10 background-light-grey m-5 m-t-10 border-5" style="height:100px;">
                <p class="text-center p-15 text-info"> There are no ${status} items .</p>
            </div>
             <hr class="width-max">
            <div class="p-5"></div>
            <div class="p-h-10 text-center ">
               <a href="../" class="btn-wide btn-success  p-t-10 rounded-5">Return to Shopping </a>           
            </div>
            <div class="p-5"></div>
            <hr class="width-max">`;
            }
            columns.innerHTML = '<hr class="width-max" />' + `<div class="grid-order "><div class="order-item1" ><div>CUSTOMER </div> <div class="${iconArrow1}" style="cursor:pointer;" onclick="orderByElement(event)" id="orderCustomer" ></div></div>
            <div class="order-item1" > <div > STATUS</div><div class="${iconArrow2}" style="cursor:pointer;" onclick="orderByElement(event)" id="orderStatus"></div></div><div class="order-item1 " > <div >$ </div><div class="${iconArrow3}" style="cursor:pointer;"onclick="orderByElement(event)" id="orderPrice"></div></div> 
            <div class="order-item1 m-0"> UPDATE </div></div>`+ cartInfo + ' <div class=" " ></div>';
    }
}

//Show loading
function loading() {
    tableContainer.hidden = true;
    loader.hidden = false;
}

//Hide loading
function complete() {
    buildCart();
    loader.hidden = true;
    tableContainer.hidden = false;
}
// Get Order data from API
async function getQuotes(url) {
    loading();
    const apiUrl = '/Admin/Order/' + url;
    try {
        const response = await fetch(apiUrl);
        apiQuotes = await response.json();    
        setTimeout(function () {
            complete();
        }, 100);
    } catch (error) {
        //Catch error here
        console.log("Error");
    }
}

//call API with small amount of data to display less/m ore important data to deacrease delay
getQuotes("GetOrderList?status=21inprocess");

/*This function update active class on pressed button and hide/show particular amount of item*/
function paginationElement(event) {
    const elementId = parseInt(event.target.id);   
    const startElement = amountItems * (elementId - 1);
    const endElement = amountItems * elementId;
    let hideIterator = 0;
    let padingIterator = 0;
    const paginationElements = document.querySelectorAll('.pagination-element');   
    const cardList = document.querySelectorAll('.item-id'); 

    for (element of paginationElements) {
        element.classList.remove('active');
        element.classList.add('hide');
        padingIterator++;
        if (padingIterator >= elementId - 1 && padingIterator < elementId + maxPaginationBtns - 1) {
            element.classList.remove('hide');
            console.log("Removed Hide ", element.classList);
        } else {
            if (elementId == 1 && element.id == maxPaginationBtns) {
                element.classList.remove('hide');
            }
            if (padingIterator > paginationElements.length - maxPaginationBtns && padingIterator < elementId) {
                element.classList.remove('hide');
                console.log("Element Show ", element.id);
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
