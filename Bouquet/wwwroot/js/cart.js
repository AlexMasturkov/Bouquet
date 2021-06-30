
async function plusItem(id)
{
    // ID contains OptionId and Product Id (keeps element ID)    
    let optionId = id.substring(0, 1);
    let elementUpdate = id.substring(1);

    //Pass data from fetch response and apply to DOM elements
/*A, B, C is mathed to Regular, Premium, Luxury*/
    function myUpdate(data)
    {
        if (optionId === '1') {
            elementUpdate = "A" + elementUpdate;          
            document.getElementById(elementUpdate).innerHTML = data.count1;
            document.getElementById("P" + elementUpdate).innerHTML = ""+ data.price1.toFixed(2);
        }
        else if (optionId === '2') {
            elementUpdate = "B" + elementUpdate;        
            document.getElementById(elementUpdate).innerHTML = data.count2;
            document.getElementById("P" + elementUpdate).innerHTML = "" + data.price2.toFixed(2);
        }
        else if (optionId === '3') {
            elementUpdate = "C" + elementUpdate;          
            document.getElementById(elementUpdate).innerHTML = data.count3;
            document.getElementById("P" + elementUpdate).innerHTML = "" + data.price3.toFixed(2);
        }

        //Calculate and Update Total price
        const currentPrice = document.getElementsByClassName('price-item');
        const totalAmount = document.getElementsByClassName('amount-count');
        let total = 0.0; 

        for (let element of currentPrice) {         
            total += parseFloat(element.innerText);       
        }
        for (let element of totalAmount) {          
            amount += parseInt(element.innerText);            
        }    
        document.getElementById('priceTotal').innerHTML = "C$ " + total.toFixed(2);
        document.getElementById('cartCountMobile').innerHTML = data.amount;
        document.getElementById('cartCountMain').innerHTML = data.amount;
       
    };
    const apiUrl = '/Customer/Cart/PlusItem';
    await fetch(apiUrl, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(id)
    }).then(response => response.json())
        .then(function (data) {         
            myUpdate(data);
        })
        .catch(function (error) {
            console.log('Request failed', error);
        });
}

async function minusItem(id,event) {    
    // ID contains OptionId and Product Id (keeps element ID)    
    let optionId = id.substring(0, 1);
    let elementUpdate = id.substring(1);
    let amount = 0;
    let countElement = 0;  
    const productElement = document.getElementById("product" + elementUpdate);    
    let amount1 = parseInt(document.getElementById("A" + elementUpdate).innerText);   
    let amount2 = parseInt(document.getElementById("B" + elementUpdate).innerText);
    let amount3 = parseInt(document.getElementById("C" + elementUpdate).innerText);
    const allCount = amount1 + amount2 + amount3; 
    event.target.classList.add('muted-btn'); //Prevent to remove more items after the last one 

    //Check if we can remove the last item
    if (optionId === '1') {
        countElement = amount1;          
    }
    else if (optionId === '2') {
        countElement = amount2;          
    }
    else if (optionId === '3') {
        countElement = amount3;         
    }  
    //this function prevent to add -1 in count to allow update function call 
    setTimeout(function () {
        event.target.classList.remove('muted-btn');
    }, 500);    

    if (countElement > 0)
    {
        if (allCount == 1)//Check if this is the last item of specific product in cart
        {          
            id = "1" + id;
            productElement.remove();
             const totalAmount = document.getElementsByClassName('amount-count');
             let amountCart = 0;
             for (let element of totalAmount)
             {
                 amountCart += parseInt(element.innerText);
             }
             document.getElementById('cartCountMobile').innerHTML = amountCart;
             document.getElementById('cartCountMain').innerHTML = amountCart; 

            //Check amount carts to change icon to empty
            const cartAmount = document.getElementsByClassName('grid-cart').length;
            console.log("AMOUNT", cartAmount);
            if (cartAmount < 1) {
                const cartIcon = document.getElementById('cartIcon');
                cartIcon.classList.replace('icon-cart-full', 'icon-cart-empty');
                location.href = '././';             
            }           
        }
        else {
            id = "2" + id;          
        }    
        const apiUrl = '/Customer/Cart/MinusItem';
        await fetch(apiUrl, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json;charset=utf-8'
            },
            body: JSON.stringify(id)
        }).then(response => response.json())
            .then(function (data) {
                myUpdate(data);      
            })
            .catch(function (error) {
                if (error) {
                    console.log('Request failed ', error);
                }
                else {
                   console.log('No more items');
                }
            });          
    }
    //Pass data from fetch response and apply to DOM elements
    function myUpdate(data) {
        try {
            if (optionId === '1') {
                elementUpdate = "A" + elementUpdate;              
                document.getElementById(elementUpdate).innerHTML = data.count1;
                document.getElementById("P" + elementUpdate).innerHTML = "" + data.price1.toFixed(2);
            }
            else if (optionId === '2') {
                elementUpdate = "B" + elementUpdate;             
                document.getElementById(elementUpdate).innerHTML = data.count2;
                document.getElementById("P" + elementUpdate).innerHTML = "" + data.price2.toFixed(2);
            }
            else if (optionId === '3') {
                elementUpdate = "C" + elementUpdate;               
                document.getElementById(elementUpdate).innerHTML = data.count3;
                document.getElementById("P" + elementUpdate).innerHTML = "" + data.price3.toFixed(2);
            }
        } catch {
            console.log("Product removed");
        }
        //Calculate and Update Total price
        const currentPrice = document.getElementsByClassName('price-item');       
        let total = 0.0;
        amount = data.amount;       
        for (let element of currentPrice) {        
            total += parseFloat(element.innerText);         
        }       
        setTimeout(function () {          
            document.getElementById('priceTotal').innerHTML = "C$ " + total.toFixed(2);
            document.getElementById('cartCountMobile').innerHTML = amount;
            document.getElementById('cartCountMain').innerHTML = amount;
        }, 100);
    };
}
let apiQuotes = [];
const loader = document.getElementById('loader');
const tableContainer = document.getElementById('tableContainer');
const columns = document.getElementById('columns');
const address = document.getElementById('address');
//Show loading
function loading() {
    loader.hidden = false;
    tableContainer.hidden = true;
}
//Hide loading
function complete() {
    tableContainer.hidden = false;
    loader.hidden = true;
    buildCart();   
}
//Calculate Total Just after the page was loaded
function calculateTotal(data) { 
    let total = 0.0;
    for (let element of data) {
        total += element.count * element.product.price + element.count2 * element.product.price2 + element.count3 * element.product.price3;      
    } 
    document.getElementById('priceTotal').innerHTML ="C$ " + total.toFixed(2);
}
// Get Quotes From API function 
async function getQuotes() {
    loading();
    const apiUrl = '/Customer/Cart/GetAll';
    try {
        const response = await fetch(apiUrl);
        apiQuotes = await response.json();
        setTimeout(function () {
            complete();
        }, 300);      
        if (apiQuotes.data.listCarts.length !== 0) {
            calculateTotal(apiQuotes.data.listCarts);
        }
        else {
            console.log('No more items');
        }      
    } catch (error) {       
        console.log('Request failed ', error);  
    }
}
// Get Quotes From API First Call
getQuotes();

//Build Change Address frame
function changeAddress() {
    console.log("Change Address Pressed");
    const cartHeader = document.getElementById('cartHeader');
    const defaultDelivery = document.getElementById('defaultDelivery');
    const columns = document.getElementById('columns');
    cartHeader.classList.add('hide'); 
    defaultDelivery.classList.add('hide'); 
    columns.classList.add('hide');
    const addressFrame = `
    <div class="item-header text-header grid-three-column-center  ">
        <div></div>
        <h1 class="item-row-center">
           Delivery Address
        </h1>
        <div class="item-row-right ">
            <a asp-area="Customer" asp-controller="Home" asp-action="Index" class="p-0 a-btn"><i class="icon-arrow-left-in-square i-btn"></i> </a>
        </div>
    </div>
       <form class=" shadow2 i-border-default background-bright-grey p-b-5" method="post ">
        <div class="p-h-15">
            <label for="address">Delivery Address</label><br>
            <div class="input-container background-white">               
                <input name="address" placeholder="Delivery address..." type="text" required minlength="3" maxlength="50" class="input-field" />
            </div> 
            <label for="city">City</label><br>
            <div class="input-container background-white">              
                <input name="city" placeholder="City..." type="text" required minlength="3" maxlength="20" class="input-field" />
            </div>
            <label for="postal">Postal Code</label><br>
            <div class="input-container background-white">               
                <input name="postal" placeholder="Postal Code..." type="text" required minlength="3" maxlength="7" class="input-field" />
            </div> 
            <label for="state">State</label><br>
            <div class="input-container background-white">               
                <input name="state" placeholder="State..." type="text" required minlength="3" maxlength="15" class="input-field" />
            </div> 
            <label for="phone">Phone number</label><br>
            <div class="input-container background-white">              
                <input name="phone" placeholder="Delivery phone number..." type="text" required minlength="10" maxlength="14" class="input-field" />
            </div> 
            <label for="message">Message</label><br>
            <div class="input-container background-white">                
                <textarea name="message" placeholder="You can write a message here ..." type="text"  maxlength="140" class="input-field font-12" /></textarea>
            </div>             
        </div>
        <hr class="width-max">
        <div class="p-h-10">
            <button type="submit" class="btn-wide btn-success p-0 m-v-10">Register</button>
            <button class="btn-wide btn-success m-v-10" id="addDelivery" onclick = addDelivery() >Add New Delivery Address</button>
        </div>
    </form>`;
    address.innerHTML =  addressFrame ;
}
/*Build Cart Function*/
function buildCart() {
    let cartColumns = apiQuotes.data;
    let cartInfo = '';
    let optionText = '';
    if (cartColumns.listCarts.length != 0) {
        for (let element of cartColumns.listCarts) {
            optionText = `
        <div class="cart-item3"><div class="label-info-wide background-info2 text-white main-item ">Regular</div><div class="label-info mobile-item">R</div></div>
        <div class="cart-item3"> <i class="icon-minus-circle shadow2" onclick = minusItem('1'+'${element.id}',event)></i></div>
        <div class="cart-item3 font-mobile-11 m-mobile-t-6 amount-count" id="${"A" + element.id}">${element.count}</div>
        <div class="cart-item3"> <i class="icon-plus-circle shadow2" onclick = plusItem('1'+'${element.id}')></i></div>
        <div class="cart-item3 m-mobile-t-6">$</div>
        <div class="cart-item6 price-item  m-mobile-t-6" id="${"PA" + element.id}"> ${(element.product.price * element.count).toFixed(2)}</div>

        <div class="cart-item3"><div class="label-info-wide background-warning1 text-white main-item">Premium</div><div class="label-warning mobile-item">P</div></div>
        <div class="cart-item3"> <i class="icon-minus-circle shadow2" onclick = minusItem('2'+'${element.id}',event)></i></div>
        <div class="cart-item3 font-mobile-11  m-mobile-t-6 amount-count" id="${"B" + element.id}">${element.count2}</div>
        <div class="cart-item3"> <i class="icon-plus-circle shadow2" onclick = plusItem('2'+'${element.id}')></i></div>
        <div class="cart-item3 m-mobile-t-6">$</div>
        <div class="cart-item6 price-item  m-mobile-t-6" id="${"PB" + element.id}"> ${(element.product.price2 * element.count2).toFixed(2)}</div>

        <div class="cart-item3"><div class="label-info-wide background-main2 text-white main-item">Luxury</div><div class="label-main mobile-item">L</div></div>
        <div class="cart-item3"> <i class="icon-minus-circle shadow2" onclick = minusItem('3'+'${element.id}',event)></i></div>
        <div class="cart-item3 font-mobile-11 m-mobile-t-6 amount-count" id="${"C" + element.id}">${element.count3}</div>
        <div class="cart-item3"> <i class="icon-plus-circle shadow2" onclick = plusItem('3'+'${element.id}')></i></div>
        <div class="cart-item3 m-mobile-t-6">$</div>
        <div class="cart-item6 price-item  m-mobile-t-6" id="${"PC" + element.id}"> ${(element.product.price3 * element.count3).toFixed(2)}</div>
    `;
            const myText = `
        <div class="grid-cart item-shadow-1 rounded-5 m-5 font-mobile-11" id="product${element.id}">
                        <div class="cart-item1">
                            <div class="item-row-left m-0 p-0">
                             <img src="${element.product.imageUrl}" alt="Flower" style="width:116px; border-radius: 3px;" class="i-border-default">                            
                            </div>
                        </div>                        
                        <div class="cart-item2 font-mobile-12 ">${element.product.name}</div>
                        ${optionText}                       
                    </div>
        `;
            cartInfo = cartInfo + myText;
        }
    } else {
        cartInfo = `
        <div class="item-row-right p-10 " style="height:100px;">
            <article class="text-center p-t-10"> There are no any items in your cart . <br>Please press button to return the main page.</article>
        </div>
         <hr class="width-max">
        <div class="p-5"></div>
        <div class="p-h-10 text-center ">
           <a href="./" class="btn-wide btn-success  p-t-10 rounded-5">Return to Shopping </a>           
        </div>
        <div class="p-5"></div>
        <hr class="width-max">`;
    }
    columns.innerHTML = '<hr class="width-max" />' + cartInfo + ' <div class=" " ></div>';
}



