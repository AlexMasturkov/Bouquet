// Add a listener for when the window resizes
window.addEventListener('resize', productMediaQuery);
let check = 0;
function productMediaQuery() {
    if (window.innerWidth <= 480 && check == 0) {
        if (userDetailContainer.classList.contains('hide') === true) {// This check if open User Details windows to prevent Errors buildUser()       
           buildUser();
        }
        check = 1;
    }
    else if (window.innerWidth > 480 && check == 1) {     
        if (userDetailContainer.classList.contains('hide') === true) {          
            buildUser();
        }
        check = 0;
    }
}
//Lock/Unlock user by API
async function getLock(id) {   
    const apiUrl = '/Admin/User/LockUnlock';
    await fetch(apiUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(id)
    })
        .then(response => response.json())
        .then(data => console.log(data)) 
        .then(location.reload());
}
//Get User details by API
async function getDetails(id) { 
    const apiUrl = '/Admin/User/GetDetails/'+ id;   
    try {       
        const response = await fetch(apiUrl);
        apiQuotes = await response.json();
        setTimeout(function () {           
            userDetails(apiQuotes.data);
        }, 100);
    } catch (error) {
        //Catch error here
        console.log("Error One User");
    }
}

//user details function create user elements based on return data from API call
function userDetails(userData) {
    tableContainer.classList.add('hide');
    userDetailContainer.classList.remove('hide');
    let header = '';
    if (window.innerWidth <= 480) {
        header = `<div class="item-header text-header grid-three-column-center z-index10 ">
        <div></div>
        <h1 class="item-row-center">User Details</h1>
        <div class="item-row-right ">
            <a class="p-0 a-btn"><i class="icon-arrow-left-in-square i-btn " id="btnCloseUser" onclick="closeUserDetails()"></i></a>
        </div>
         </div>   `;
    }
    else
    {
        header = `<div class="item-header text-header grid-three-column-center z-index10 header-height shadow5">
        <div></div>
         <h1 class="item-row-center p-5 font-22">
            User Details
        </h1>
        <div class="item-row p-5 ">
            <a class="p-0 a-btn item-row-right m-r-15"><i class="icon-arrow-left-in-square i-btn " id="btnCloseUser" onclick="closeUserDetails()"></i></a>
        </div>
         </div>   `;
    }
    document.getElementById('userDetails').innerHTML = header +`
    <div class="border-3 background-medium-grey ">
        <div class=" background-bright-grey p-5">
            <div class="grid-form-two-column">
                <label class="input-pre-field" >User Name </label>
                <input class="input-after-field"  type="text" readonly value="${userData.name}">
            </div>
            <div class="grid-form-two-column">
                <label class="input-pre-field" >Address </label>
                <input class="input-after-field"  type="text" readonly value="${userData.streetAddress}">
            </div>
            <div class="grid-form-two-column">
                <label class="input-pre-field" >City </label>
                <input class="input-after-field"  type="text" readonly value="${userData.city}">
            </div>
            <div class="grid-form-two-column">
                <label class="input-pre-field" >Postal Code </label>
                <input class="input-after-field"  type="text" readonly value="${userData.postalCode}">
            </div>
            <div class="grid-form-two-column">
                <label class="input-pre-field" >Phone Number </label>
                <input class="input-after-field"  type="text" readonly value="${userData.phoneNumber}">
            </div>
            <div class="grid-form-two-column">
                <label class="input-pre-field" >Email </label>
                <input class="input-after-field"  type="text" readonly value="${userData.email}">
            </div>
            <div class="grid-form-two-column">
                <label class="input-pre-field" >Province</label>
                <input class="input-after-field"  type="text" readonly value="${userData.state}">
            </div>  
            <div class="grid-form-two-column">
                <label class="input-pre-field" >Role</label>
                <input class="input-after-field"  type="text" readonly value="${userData.role}">
            </div> 
            <div class="grid-form-two-column">
                <label class="input-pre-field" >Company name</label>
                <input class="input-after-field"  type="text" readonly value="${userData.company.name}">
            </div> 
            <div class="grid-form-two-column">
                <label class="input-pre-field" >Company phone</label>
                <input class="input-after-field"  type="text" readonly value="${userData.company.phone}">
            </div> 
        </div>
    </div> `;   
}

function closeUserDetails() {
    tableContainer.classList.remove('hide');
    userDetailContainer.classList.add('hide');
    location.reload();
}
let apiQuotes = [];
const loader = document.getElementById('loader');
const tableContainer = document.getElementById('tableContainer');
const columns = document.getElementById('columns');
const userDetailContainer = document.getElementById('userDetailContainer');

//Show loading
function loading() {
    loader.hidden = false;
    tableContainer.hidden = true;
}

//Hide loading
function complete() {
    tableContainer.hidden = false;
    loader.hidden = true;
    buildUser();  
}

// Get data of all users by call API
async function getQuotes() {
    loading();
    const apiUrl = '/Admin/User/GetAll';
    try {
        const response = await fetch(apiUrl);
        apiQuotes = await response.json();
        setTimeout(function () {
            complete();
        }, 100);      
    } catch (error) {
        //Catch error here
        console.log("Error Users");
    }
}
//First call API to get all users data
getQuotes();
let amountItems = 6;
let pagingtId = 0;
let maxPaginationBtns = 3;

//Create page of all users based on API data
function buildUser() {
    let eventColumns = apiQuotes.data;
    let eventsText = '';
    let iterator = '';//creates bright / dark row pattern
    let i = 0;
    const today = new Date().getTime();
    if (window.innerWidth <= 480) {
        for (let element of eventColumns) {
            const lockout = new Date(element.lockoutEnd).getTime();           
            let lockButton = `<div class="item-row m-0 p-0 m-t-10">
                                <a onclick=getLock('${element.id}') class="icon-lock-in-square i-btn-sm i-border-danger3 m-0">
                                    Unlock
                                </a>
                            </div>  `;
            if (today > lockout) {
                lockButton = `<div class="item-row m-0 p-0 m-t-10">
                                <a onclick=getLock('${element.id}') class="icon-unlock-in-square i-btn-sm i-border-success3 m-0">
                                    Unlock
                                </a>
                            </div>  `;
            }
            if (i % 2 != 0) {
                iterator = 'background-light-grey';
            }
            else {
                iterator = 'background-bright-grey';
            }
            i++;
            const eventText = `        
                <div class="grid-four-column-left02 font-13 ${iterator} p-5 " style="height:56px; ">
                    <div class="item-row-left m-0 p-0 m-l-5">                            
                      <div class="m-t-15 font-bolder">${element.name}</div>                
                    </div>     
                    <div class="item-row-left m-0 p-0 m-l-5">
                      <div class="m-t-15">${element.role}</div>
                    </div> 
                    ${lockButton} 
                    <div class="item-row m-0 p-0 m-l-10 m-t-10 m-r-5">
                        <a onclick=onclick=getDetails('${element.id}') class="icon-check-in-box i-btn-sm i-border-success3" style="width: 26px ! important;">                                    
                        </a>
                    </div> 
                    </div> <hr class="width-max" />`;
            eventsText = eventsText + eventText;
        }
        columns.innerHTML = '<hr class="width-max" />' + eventsText + ' </div>';
    } else {
        let pagination = '';
        let iteratorShow = 0;
        let hideElement = '';
        /*Build pagination*/
        if (eventColumns.length > 0) {
            let lastPaging = 0;
            for (let i = 1; i < eventColumns.length / amountItems + 1; i++) {
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
        for (let element of eventColumns) {
            const lockout = new Date(element.lockoutEnd).getTime();            
            let lockButton = `<div class="item-row m-0 p-0 m-t-10">
                                    <a onclick=getLock('${element.id}') class="icon-lock-in-square i-btn-sm i-border-danger3 m-0" style="transform:scale(1.2);">
                                        Unlock
                                    </a>
                                </div>  `;
            if (today > lockout) {
                lockButton = `<div class="item-row m-0 p-0 m-t-10">
                                    <a onclick=getLock('${element.id}') class="icon-unlock-in-square i-btn-sm i-border-success3 m-0" style="transform:scale(1.2);">
                                        Unlock
                                    </a>
                                </div>  `;
            }
            if (i % 2 != 0) {
                iterator = 'background-light-grey';
            }
            else {
                iterator = 'background-bright-grey';
            }
            i++;
            iteratorShow++;          
            if (iteratorShow > amountItems) { hideElement = 'hide'; }
            const eventText = `        
             <div class="grid-four-column-left02 font-14 ${iterator} ${hideElement} p-5 item-id p-t-10" style="height:65px;">
                <div class="item-row-left m-0 p-0 m-l-5">                            
                    <div class="m-t-15 font-bolder m-l-10">${element.name}</div>                
                </div>     
                    <div class="item-row-left m-0 p-0 m-l-5">
                    <div class="m-t-15">${element.role}</div>
                </div>
                ${lockButton} 
                <div class="item-row m-0 p-0 m-l-10 m-t-10 m-r-5">
                    <a onclick=onclick=getDetails('${element.id}') class="icon-check-in-box i-btn-sm i-border-success3 m-r-10" style="width:29px; margin-top:-1px; ">                                    
                    </a>
                </div> 
             </div>`; 
            eventsText = eventsText + eventText;
        }
     columns.innerHTML =  eventsText + ' </div>';
    }
}

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
