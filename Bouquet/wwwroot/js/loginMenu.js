
/*Reponsiable for mobile menues*/
const iconExpand = document.getElementById('iconExpand');
iconExpand.addEventListener('click', processToggle);

const iconCrossMenuPicture = document.getElementById('iconCrossMenuPicture');

if (iconCrossMenuPicture !== null) {
    iconCrossMenuPicture.addEventListener('click', hideImage);
}

function displayImage() {
    document.getElementById('detailsProduct').classList.add('show-scale');
    setTimeout(function () {
           document.getElementById('detailsProduct').classList.add('hide');
    document.getElementById('picture').classList.remove('hide');
    }, 360);
}
function hideImage() {
    document.getElementById('detailsProduct').classList.remove('hide');
    document.getElementById('picture').classList.add('hide');   
}

function processToggle() {
    // If cross icon  exists
    if (iconExpand.classList.contains('icon-cross')) {
        const options = document.getElementById('loginOptions');
        options.classList.remove('show-scale');
        options.classList.add('show-scale-rev');
        iconExpand.classList.add('icon-expand');
        iconExpand.classList.add('icon-expand-rotate-rev');
        iconExpand.classList.remove('icon-cross');
        iconExpand.classList.remove('icon-cross-rotate');
        setTimeout(function () {
            iconExpand.classList.remove('icon-expand-rotate-rev');
            buildOption();          
        }, 560);     
    }
    else {
        // If there is not cross icon  
        buildOption();
        const options = document.getElementById('loginOptions');
        options.classList.remove('hide');
        options.classList.add('show-scale');
        iconExpand.classList.add('icon-cross');
        iconExpand.classList.add('icon-cross-rotate');
        iconExpand.classList.remove('icon-expand');
        iconExpand.classList.remove('icon-expand-rotate');
        const iconCrossMenu = document.getElementById('icon-cross-menu');
        iconCrossMenu.addEventListener('click', processSecondToggle);
    }
}


function processSecondToggle() {
    // If there is cross icon  
    if (document.getElementById('icon-cross-menu').classList.contains('icon-cross-small')) {
        const options = document.getElementById('optionSecond');
        options.classList.remove('show-scale');
        options.classList.add('show-scale-rev');
        setTimeout(function () {
           ;
        }, 560);       
    }
    else {
        // If there is not cross icon
        const options = document.getElementById('loginOptions');
        options.classList.remove('hide');
        options.classList.add('show-scale');
        iconExpand.classList.add('icon-cross');
        iconExpand.classList.add('icon-cross-rotate');
        iconExpand.classList.remove('icon-expand');
        iconExpand.classList.remove('icon-expand-rotate');
    }
}

function processThirdToggle() {  
    if (document.getElementById('iconCrossSecond').classList.contains('icon-cross-small')) {
        const options = document.getElementById('extraOptionSecond');
        options.classList.replace('show-scale', 'show-scale-rev'); 
        iconExpand.classList.replace('icon-cross', 'icon-expand');
        iconExpand.classList.replace('icon-cross-rotate', 'icon-expand-rotate-rev');
        setTimeout(function () {
            iconExpand.classList.remove('icon-expand-rotate-rev');
            buildOption();//Helps to destroy unused DOM elements
        }, 560);     
    }
    else {      
       /* console.log("Expand Third");    */  
    }  
}

function processOption01() {
    const optionSecond = document.getElementById('optionFirst');   
    let body = `
          <section class="item-shadow-1 options show-scale background-bright-grey z-index20" id="extraOptionSecond">
          <div class="item-header text-header">Shipping and Delivery</div>
            <i class="icon-cross-small align-end" id="iconCrossSecond" onclick="processThirdToggle()"></i>
          <div class="item-container-vertical text-center p-5">
            <p class=" font-mobile-11 text-left background-light-grey rounded-5 p-20 shadow4">
                We offer Free Shipping & Delivery in most places.<br />
                We strive to provide the ultimate service for you. We deliver practically everywhere. <br />
                Regular Delivery between 2:00 PM to latest 9:00 PM on Weekdays and Weekends <br />
                Business Hours Delivery by 5:00 PM <br />

                We will deliver on the day and address given.
                You must provide the full and correct address.
                If a wrong or incomplete address is provided, a second delivery charge may apply.
                An example of an incomplete address is one without an apartment number, unit number or buzzer number.
                This will only be an issue if our drivers cannot identify your delivery location or the building does not have a concierge to allow access.
                For time sensitive deliveries make sure to inform us, otherwise regular deliveries are done within the day, latest by 9 p.m.
                Business deliveries will be done during business hours.
                For deliveries to locations with a concierge, it is recommended to inform the concierge of the delivery.
            </p>
        </div>
        </section>
          `;
    const options = document.getElementById('loginOptions');   
    options.classList.replace('show-scale', 'show-scale-rev');
    setTimeout(function () {
        options.classList.add('hide');
        optionSecond.innerHTML = body;
    }, 560);
}
function processOption02() {
   const optionSecond = document.getElementById('optionFirst'); 
    let body = `
          <section class="item-shadow-1 options show-scale background-bright-grey z-index20  " id="extraOptionSecond">
             <div class="item-header text-header">Payment Methods</div>
            <i class="icon-cross-small align-end" id="iconCrossSecond" onclick="processThirdToggle()"></i>
            <div class="item-container-vertical text-center p-5">          
            <hr class="m-0 m-t-10" />
            <p class=" font-mobile-11 text-left background-light-grey rounded-5 p-20 shadow4">
                It is equally important to choose the solution which offers a specific selection of credit cards that are most popular in the merchants target markets.<br />
                We take Visa & MasterCard as they are widely used by cyber customers.
                We offer delay payments for some costumers
            </p>
        </div>
        </section>
          `;
    const options = document.getElementById('loginOptions');   
    options.classList.replace('show-scale', 'show-scale-rev');
    setTimeout(function () {
        options.classList.add('hide');
        optionSecond.innerHTML = body;
    }, 560);
}
function processOption03() {
    const optionSecond = document.getElementById('optionThird');   
    let body = `
          <section class="item-shadow-1 options show-scale background-bright-grey z-index20" id="extraOptionSecond">
           <div class="item-header text-header">About Us</div>
            <i class="icon-cross-small align-end" id="iconCrossSecond" onclick="processThirdToggle()"></i>
           <div class="item-container-vertical text-center p-5">
            <div class="font-16 text-info p-t-5">NCraft </div>
            <hr class="m-0 m-t-10" />
            <p class=" font-mobile-11 text-left background-light-grey rounded-5 p-20 shadow4">
                Our value is in being a direct importer, which allows you to enjoy your flowers for a longer time.<br />
                When purchasing flowers you should not only consider the upfront cost, but also the longevity of the flowers.<br />
                You are not getting a good deal if the flowers are left in the cooler a long time, or if it exchanged many hands before reaching you.<br /> When looking for same-day Toronto flower delivery service and to conveniently order flowers online,
                NFlowers is your one-stop-shop.<br />
                We are a direct farm to consumer Toronto florist offering flowers for delivery and the freshest, brightest Toronto blooms, to create stunning, one of a kind flower bouquets.
            </p>
        </div>
        </section>
          `;
    const options = document.getElementById('loginOptions');
    options.classList.replace('show-scale', 'show-scale-rev');
    setTimeout(function () {
        options.classList.add('hide');
        optionSecond.innerHTML = body;
    }, 560);
}
function processOption04() {
    const optionSecond = document.getElementById('optionFourth');   
    let body = `
          <section class="item-shadow-1 options show-scale background-bright-grey z-index20" id="extraOptionSecond">
            <div class="item-header text-header">Contact Us</div>
            <i class="icon-cross-small align-end" id="iconCrossSecond" onclick="processThirdToggle()"></i>
            <div class="item-container-vertical text-center p-5">
                <div class="font-16 text-info p-t-5">Location </div>
                <hr class="m-0 m-t-10" />
                <p class=" font-mobile-11 background-light-grey rounded-5 p-20 shadow4 ">
                    610 WestSide Ave. Unit 16 <br />
                    Thornhill, Ontario <br />
                    Canada L3T 1L5 <br />
                </p>
                <hr class="m-0 p-t-10" />
                <div class="font-16 text-info">Phone </div>
                <hr class="m-0 m-t-10" />
                <p class=" font-mobile-11 background-light-grey rounded-5 p-20 shadow4">
                    (647) 396-6377 <br />
                    9:00 am - 5:00 pm EST
                </p>
                <hr class="m-0 p-t-10" />
                <div class="font-16 text-info">Email </div>
                <hr class="m-0 m-t-10" />
                <p class=" font-mobile-11 background-light-grey rounded-5 p-20 shadow4">
                    ncraft@gmail.cum
                </p>
            </div>
        </section>
          `;
    const options = document.getElementById('loginOptions');
    options.classList.replace('show-scale', 'show-scale-rev');
    setTimeout(function () {
        options.classList.add('hide');
        optionSecond.innerHTML = body;
    }, 560);
}

function buildOption() { 
    if (!document.body.contains(document.getElementById('loginOptions'))) {
        const optionWrap = document.getElementById('optionWrap');      
        let body = `
          <div id="optionFirst"></div>
          <div id="optionSecond"></div>
          <div id="optionThird"></div>
          <div id="optionFourth"></div>
          <section class="item-shadow-1 options four-elements background-bright-grey hide z-index10" id="loginOptions">
                <div class="item-header text-header">Help Menu</div>
                <i class="icon-cross-small align-end" id="icon-cross-menu"></i>
                <div class="item-container-vertical ">
                     <div class="text-description container2 background-light-grey">
                    <div class="grid-two-column-right">
                        <div class="p-10 justify-left">
                            Shipping and Delivery
                        </div>
                        <i class="icon-arrow-right align-end" id='option05' onclick="processOption01()"></i>
                    </div>
                </div>
                <div class="text-description container2">
                    <div class="grid-two-column-right">
                        <div class="p-10 justify-left">
                            Payment Methods
                        </div>
                        <i class="icon-arrow-right align-end" id='option06' onclick="processOption02()"></i>
                    </div>
                </div>
                <div class="text-description container2 background-light-grey">
                    <div class="grid-two-column-right">
                        <div class="p-10 justify-left">
                            About Us
                        </div>
                        <i class="icon-arrow-right align-end" id='option07' onclick="processOption03()"></i>
                    </div>
                </div>
                <div class="text-description container2">
                    <div class="grid-two-column-right">
                        <div class="p-10 justify-left">
                            Contact Us
                        </div>
                        <i class="icon-arrow-right align-end" id='option04' onclick="processOption04()"></i>
                    </div>
                </div>
                    <hr>
                </div>
            </section>
          `;
        optionWrap.innerHTML = body;
        const iconCrossMenu = document.getElementById('icon-cross-menu');
        iconCrossMenu.addEventListener('click', processToggle);
    } else {
        optionWrap.innerHTML = '';      
    }
}