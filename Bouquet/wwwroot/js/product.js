
function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            fetch(url, {
                method: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                },
            })
                .then(res => res.text()) // or res.json()
                .then(
                    setTimeout(function () {
                        location.reload();
                    }, 100)
                );
        }
    })
}

let amountItems = 5;
let pagingtId = 0;
let maxPaginationBtns = 3;

// Add a listener for when the window resizes
window.addEventListener('resize', productMediaQuery);

let check = 0;
function productMediaQuery() {
    if (window.innerWidth <= 480 && check == 0) {
        buildProducts();
        check = 1;
    }
    else if (window.innerWidth > 480 && check == 1) {
        buildProducts();
        check = 0;
    }
}

let apiQuotes = [];
const loader = document.getElementById('loader');
const tableContainer = document.getElementById('tableContainer');
const columns = document.getElementById('columns');

//Show loading
function loading() {
    loader.hidden = false;
    tableContainer.hidden = true;
    document.getElementById('paginationCustom').classList.add('hide');
}

//Hide loading
function complete() {
    tableContainer.hidden = false;
    loader.hidden = true;
    buildProducts();
    document.getElementById('paginationCustom').classList.remove('hide');
}

//get all products function by call API
async function getQuotes() {
    loading();
    const apiUrl = '/Admin/Product/GetAll';
    try {
        const response = await fetch(apiUrl);
        apiQuotes = await response.json();
        setTimeout(function () {
            complete();
        }, 300);      
    } catch (error) {
        //Catch error here
        console.log("Error");    }
}
//Call first time to get all products
getQuotes();

// function to create Products based on API call Data
function buildProducts() {
    let productColumns = apiQuotes.data;
    let productsText = '';
    let iterator = '';//creates bright / dark row pattern
    let i = 0;
    if (window.innerWidth <= 480) {
        for (let element of productColumns) {
            if (i % 2 != 0) {
                iterator = 'background-light-grey';
            }
            else {
                iterator = 'background-bright-grey';
            }
            i++;
            const productText = `        
          <div class="grid-five-column font-14 ${iterator} p-5 " style="height:58px; ">
                        <div class="item-row-left m-0 p-0">                            
                             <img src="${element.imageUrl}" alt="Flower" style="width:44px; border-radius: 3px;" class="i-border-default">                            
                        </div>
                        <div class="item-row">
                            <div class="m-t-15">${element.name}</div>
                        </div>
                        <div class="item-row">
                            <div class="m-t-15">$ ${element.price}</div>
                        </div>
                        <div class="item-row "> <i class="icon-trash-square i-btn-sm i-border-danger3 " onclick=Delete("/Admin/Product/Delete/${element.id}")></i></div>
                        <a class="item-row " href="/Admin/Product/Upsert/${element.id}"> <i class="icon-edit-in-square i-btn-sm i-border-success3"></i></a>                        
                    </div> <hr class="width-max" />`;
            productsText = productsText + productText;
        }
        columns.innerHTML = productsText;
    } else {
        let pagination = '';
        let iteratorShow = 0;
        let hideElement = '';
        /*Build pagination*/
        if (productColumns.length > 0) {
            let lastPaging = 0;
            for (let i = 1; i < productColumns.length / amountItems + 1; i++) {

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
        for (let element of productColumns) {
            if (i % 2 != 0) {
                iterator = 'background-light-grey';
            }
            else {
                iterator = 'background-bright-grey';
            }
            i++;
            iteratorShow++;           
            if (iteratorShow > amountItems ) { hideElement = 'hide'; }
            const productText = `        
            <div class="grid-five-column font-16 ${iterator} ${hideElement} p-5 product-id" style="height:78px;">
                        <div class="item-row-left m-0 p-0">                            
                             <img src="${element.imageUrl}" alt="Flower" style="height:68px; border-radius: 3px;" class="shadow2">                            
                        </div>
                        <div class="item-row">
                            <div class="m-t-15 p-10">${element.name}</div>
                        </div>
                        <div class="item-row">
                            <div class="m-t-15 p-10">$ ${element.price}</div>
                        </div>
                        <div class="item-row m-t-10"> <i class="icon-trash-square i-btn-sm i-border-danger3" style="transform:scale(1.2); " onclick=Delete("/Admin/Product/Delete/${element.id}")></i></div>
                        <a class="item-row m-t-10" href="/Admin/Product/Upsert/${element.id}"> <i class="icon-edit-in-square i-btn-sm i-border-success3" style="transform:scale(1.2); "></i></a>                        
                    </div> `;
            productsText = productsText + productText;
        }
        columns.innerHTML = productsText + ' </div>';
    }
}

/*This function update active class on pressed button and hide/show particular amount of product*/
function paginationElement(event) {
    const elementId = parseInt(event.target.id);    
    const startElement = amountItems * (elementId - 1);
    const endElement = amountItems * elementId;
    let hideIterator = 0;
    let padingIterator = 0;
    const paginationElements = document.querySelectorAll('.pagination-element');   
    const cardList = document.querySelectorAll('.product-id');  

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

