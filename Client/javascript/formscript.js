/*
    By: Andreea Popa
    Description: This javascript file includes methods for fetching a variety of form
                 info from API endpoints, and then filling that data in on the pages
                 that the data was requested on

*/

function getAdoptableCats() {
    fetch(apiUrl + "api/forms/getAdoptableCats", {
        method: 'GET',
        headers: {
            "Content-Type": "application/json; charset=utf-8"
        }
    })
    .then(response => response.json())
    .then(response => {
        if (response.status != 200){
            alert(response.message)
        }
        else {
            document.getElementById("catSelection").innerHTML = "";
            addSelection(response.data)
        }
    })
}

function addSelection(data) {
    data.forEach(item => {
        addItem(item)
    });
}

function addItem(item) {
    const option = document.createElement("option")
    if (item == null) {
        option.innerHTML = 'Null'
    } else {
        option.innerHTML = item.name
    }
    
    document.getElementById("catSelection").appendChild(option)
}