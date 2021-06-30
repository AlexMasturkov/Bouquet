
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
            .then(res => console.log(res))
            .then(
                setTimeout(function () {
                    location.reload();
                }, 100)
            );
        }
    })
}

// Add a listener when the window resizes mobile/main
window.addEventListener('resize', productMediaQuery);

/*showFilters();*/
let check = 0;
function productMediaQuery() {
    if (window.innerWidth <= 480 && check == 0) {
        buildCategory();
        check = 1;
    }
    else if (window.innerWidth > 480 && check == 1) {
        buildCategory();
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
}

//Hide loading
function complete() {
    tableContainer.hidden = false;
    loader.hidden = true;
    buildCategory();    
}

// Get Quotes From API "url": "/Admin/Category/GetAll"
async function getQuotes() {
    loading();
    const apiUrl = '/Admin/Category/GetAll';
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
/*First time call all Categories*/
getQuotes();

//Build Categories Function
function buildCategory() {
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
         <div class="grid-three-column-left font-14 ${iterator} p-5 " style="height:58px; ">
                        <div class="item-row-left m-0 p-0 m-l-10">                            
                            <div class="m-t-15">${element.name}</div>                
                        </div>                   

                        <div class="item-row "> <i class="icon-trash-square i-btn-sm i-border-danger3 " onclick=Delete("/Admin/Category/Delete/${element.id}")></i></div>
                        <a class="item-row " href="/Admin/Category/Upsert/${element.id}"> <i class="icon-edit-in-square i-btn-sm i-border-success3"></i></a>                        
                    </div> <hr class="width-max" />`;
            productsText = productsText + productText;
        }
        columns.innerHTML = '<hr class="width-max" />' + productsText + '</div>';
    } else {
        for (let element of productColumns) {
            if (i % 2 != 0) {
                iterator = 'background-light-grey';
            }
            else {
                iterator = 'background-bright-grey';
            }
            i++;
            const productText = `        
         <div class="grid-three-column-left font-16 ${iterator} p-5 " style="height:58px; ">
                        <div class="item-row-left m-0 p-0 m-l-5">                            
                            <div class="m-t-15">${element.name}</div>                
                        </div>  
                        <div class="item-row"> <i class="icon-trash-square i-btn-sm i-border-danger3" style="transform:scale(1.2);" onclick=Delete("/Admin/Category/Delete/${element.id}")></i></div>
                        <a class="item-row " href="/Admin/Category/Upsert/${element.id}"> <i class="icon-edit-in-square i-btn-sm i-border-success3" style="transform:scale(1.2); "></i></a>                        
                    </div> <hr class="width-max" />`;
            productsText = productsText + productText;
        }
        columns.innerHTML = '<hr class="width-max" />' + productsText + '</div>';
    }   
}


