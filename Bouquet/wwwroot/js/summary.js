const inputFields = document.getElementsByClassName('input-after-field');
const deliveryHeader = document.getElementById('deliveryHeader');
const deliveryAddress = document.getElementById('deliveryAddress');
const orderSummary = document.getElementById('orderSummary');
const orderHeader = document.getElementById('orderHeader');
const orderHeaderMain = document.getElementById('orderHeaderMain');
const updateAddressBtn = document.getElementById('updateAddress');
const confirmAddressBtn = document.getElementById('confirmAddress');
const inputAddress = document.getElementById('inputAddress');
const inputs = document.querySelectorAll('input');
let deliveryName = document.getElementById('deliveryName');
const shoppingMoreBtn = document.getElementById('shoppingMore');

//To update address we need to listen all inputs
for (const input of inputs) {
    input.addEventListener('input', updateInputs);
}

//Update address function display input Form and hide main form
function updateAddress() {
    for (let element of inputFields) {     
        element.value = '';
    }   
    deliveryAddress.classList.remove('hide');  
    orderSummary.classList.add('hide');
    updateAddressBtn.classList.add('hide');
    shoppingMoreBtn.classList.remove('hide');   
    orderHeader.classList.add('hide');
    orderHeaderMain.classList.add('hide');
    deliveryHeader.classList.remove('hide');
};
//Confirm address function display main Form and hide updateAddress form
function confirmAddress() {
    for (let element of inputFields) {   
        element.value = '';
    }
    deliveryAddress.classList.add('hide');
    orderSummary.classList.remove('hide');
    confirmAddressBtn.classList.add('hide');
    updateAddressBtn.classList.remove('hide');
    orderHeader.classList.remove('hide');
    orderHeadermain.classList.remove('hide');
}

//Real time update new address
function updateInputs() {
    let ipn = '';
    let phone = '';   
    let iterator = 0;
    let name = '';
    for (let element of inputFields) {
        iterator++;
        if (iterator == 1)
        {
            ipn = '';
            name = '' + element.value;
        }
        else if (iterator == 5) {
            ipn += '</br>' + element.value + ', ';           
        }
        else if (iterator == 3)
        {
           phone= ' Phone: ' + element.value;
        } else {
            ipn += element.value + ', ';
        }       
    }   
    document.getElementById('inputAddress').innerHTML = ipn + '' + phone;
    document.getElementById('deliveryName').innerHTML = '<div class="font-14 font-mobile-12">Deliver to</div>' + ' <div class="text-right font-14 font-mobile-12">' + name +'</div>';
}

function validateInput() {
    const regAddress = /^\d+\s[A-z]+\s[A-z]+/;
    const regPhone = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
    const regName = /^[a-zA-Z]+ [a-zA-Z]+$/;
    const regCity = /^(?:[A-Za-z]{2,}(?:(\.\s|'s\s|\s?-\s?|\s)?(?=[A-Za-z]+))){1,2}(?:[A-Za-z]+)?$/;
/*    const regState = /[^'r]s|[Au][^i]|[vb]|O/;*/
    const regPostal = /^(\d{5}(-\d{4})?|[A-Z]\d[A-Z] ?\d[A-Z]\d)$/;
    let fields = [];
    const labels = ['Full Name', 'Phone Number', 'Street Address', 'City', 'State', 'Postal Code'];
    let i = 0;
    for (let element of inputFields) {
        fields[i] = element.value;     
        fields.push(element.value);      
        const message = "Please add " + labels[i];
        if (element.value.trim() === "") {
            swal("Error", message, "error");
            return false;
        }
        i++;
    }
    if (!fields[0].match(regName)) {
        swal("Error", "Please add first and second name", "error");
        return false;
    }
    else if (!fields[2].match(regPhone)) {
        if (fields[2].charAt(0) === '1') {
            swal("Error", "You can't use phone number starts from '1' ", "error");
            return false;
        } else {
        swal("Error", "Please use 10 digit phone number", "error");
        return false;
        }
    }
    else if (!fields[1].match(regAddress)) {
        swal("Error", "Please use correct Address format", "error");
        return false;
    }
    else if (!fields[3].match(regCity)) {
        swal("Error", "Please check City name", "error");
        return false;
    }
  /*  else if (!fields[4].match(regState)) {
        swal("Error", "Please check Province name", "error");
        return false;
    }*/
    else if (!fields[5].match(regPostal)) {      
        swal("Error", "Please check Postal Code", "error");
        return false;
    }
    return true;
}