
//This function responsible for Main view filters
function showFilters() {
    const mainFilter = document.getElementById('mainFilter');
    const filterContainer = document.getElementById("filterContainer");
    console.log(filterContainer.getBoundingClientRect().height, " top", filterContainer.offsetTop);
    if (mainFilter) {
        document.getElementById('mainFilter').style.top = filterContainer.getBoundingClientRect().top + 'px';
        document.getElementById('mainFilter').style.left = filterContainer.getBoundingClientRect().left + 'px';
    }
}
//This function responsible for Main view filters dropdown
function hideDropButton() {  
    const dropdowns = document.getElementsByClassName("drop-down-section");  
    if (dropdowns.length > 0) {
        let i;
        for (i = 0; i < dropdowns.length; i++) {
            let openDropdown = dropdowns[i];
            if (!openDropdown.classList.contains('hide') && !openDropdown.classList.contains('slide-down')) { 
                openDropdown.classList.add('hide');              
            }
            else if (openDropdown.classList.contains('slide-down')) {
                openDropdown.classList.remove('slide-down');
                openDropdown.classList.add('slide-up');
                setTimeout(function () {
                    openDropdown.classList.remove('slide-up');
                    openDropdown.classList.add('hide');                   
                }, 300);
                setTimeout(function () {  
                    document.getElementById('filter').classList.add('visible');
                    document.getElementById('filter').classList.remove('non-visible');
                }, 500);
            }
        }
    }
}

function closeDrop() { 
    hideDropButton();
}

function headerItems(event) {   
    if (event.target.matches('.non-drop')) {
           hideDropButton();      
    } 
}

function dropDown(event) {
    const headerMain = document.getElementById("headerMain");
    const id = event.target.id + 'Option';
    hideDropButton();
    if (event.target.id == 'filter' ) {
        if (headerMain.getBoundingClientRect().width > 1200) {
            document.getElementById(id).style.left = (headerMain.getBoundingClientRect().width-1200)/2 + 'px';
        }
        else {
            document.getElementById(id).style.left = 12 + 'px';
        }
        document.getElementById(id).classList.add('slide-down');
        document.getElementById(event.target.id).classList.add('non-visible');
        document.getElementById(event.target.id).classList.remove('visible');
    } else {
        document.getElementById(id).style.left = event.target.getBoundingClientRect().left - event.target.getBoundingClientRect().width - headerMain.getBoundingClientRect().left + 35 + 'px';
        document.getElementById(id).style.top = headerMain.getBoundingClientRect().bottom-40 + 'px';
    }
    document.getElementById(id).classList.remove('hide');  
}

function dropBoxOver(event) {  
        hideDropButton();   
}
window.onclick = function (event) {
    if (!event.target.matches('.drop-items')) {      
        hideDropButton();
    }
}

function redirectHomePage() {
    location.href = '/';
}
