/*
    By: Karen James

    Description: This file is used to get available cats for adoption and display them in a table for user to view

*/


function getAvailableCats() {
    fetch(apiUrl + "api/GetAcceptedCatsUpForAdoptionForms", {
        method: "GET",
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
            document.getElementById("tableBody").innerHTML = ""
            populateTable(response.data)
        }
    })
}

function populateTable(data){
    data.forEach(item => {
        addItem(item)
    });
}


function addItem(item) {
    const td0 = document.createElement("td");
    td0.innerHTML = item.name;

    const td1 = document.createElement("td");
    td1.innerHTML = item.description;
    
    const tr = document.createElement("tr");

    tr.appendChild(td0);
    tr.appendChild(td1);

    document.getElementById("tableBody").appendChild(tr);
}
