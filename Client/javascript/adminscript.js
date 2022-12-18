/*
    By: Roman Krutikov

    Description: This javascript file includes methods for fetching a variety of admin
                 info from API endpoints, and then filling that data in on the pages
                 that the data was requested on

*/

function getAcceptedAdoptableCatForms() {
    let bearer = localStorage.getItem("bear")
    if (bearer == null || bearer == ""){
        alert("No login stored, please log in first!")
        return;
    }
    fetch(apiUrl + "api/admin/getAcceptedAdoptableCatForms", {
        method: "GET",
        headers: {
            "Authorization": "Bearer " + bearer,
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

function getAcceptedCatsUpForAdoptionForms() {
    let bearer = localStorage.getItem("bear")
    if (bearer == null || bearer == ""){
        alert("No login stored, please log in first!")
        return;
    }
    fetch(apiUrl + "api/admin/getAcceptedCatsUpForAdoptionForms", {
        method: "GET",
        headers: {
            "Authorization": "Bearer " + bearer,
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

function getDeniedAdoptableCatForms() {
    let bearer = localStorage.getItem("bear")
    if (bearer == null || bearer == ""){
        alert("No login stored, please log in first!")
        return;
    }
    fetch(apiUrl + "api/admin/getDeniedAdoptableCatForms", {
        method: "GET",
        headers: {
            "Authorization": "Bearer " + bearer,
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

function getDeniedCatsUpForAdoptionForms(){
    let bearer = localStorage.getItem("bear")
    if (bearer == null || bearer == ""){
        alert("No login stored, please log in first!")
        return;
    }
    fetch(apiUrl + "api/admin/getDeniedCatsUpForAdoptionForms", {
        method: "GET",
        headers: {
            "Authorization": "Bearer " + bearer,
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

function getNewAdoptableCatForms(){
    let bearer = localStorage.getItem("bear")
    if (bearer == null || bearer == ""){
        alert("No login stored, please log in first!")
        return;
    }
    fetch(apiUrl + "api/admin/getNewAdoptableCatForms", {
        method: "GET",
        headers: {
            "Authorization": "Bearer " + bearer,
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
            populateTableWithActions(response.data, "api/admin/acceptAdoptableCatForm", "api/admin/denyAdoptableCatForm")
        }
    })

}

function getNewCatsUpForAdoptionForms() {
    let bearer = localStorage.getItem("bear")
    if (bearer == null || bearer == ""){
        alert("No login stored, please log in first!")
        return;
    }
    fetch(apiUrl + "api/admin/getNewCatsUpForAdoptionForms", {
        method: "GET",
        headers: {
            "Authorization": "Bearer " + bearer,
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
            populateTableWithActions(response.data, "api/admin/acceptCatUpForAdoptionForm", "api/admin/denyCatUpForAdoptionForm")
        }
    })
}

function populateTable(data){
    data.forEach(item => {
        addItem(item)
    });
}

function populateTableWithActions(data, acceptUrl, denyUrl){
    data.forEach(item => {
        addItemWithActions(item, acceptUrl, denyUrl)
    });
}

function addItem(item) {
    const td0 = document.createElement("td");
    td0.innerHTML = item.id;

    const td1 = document.createElement("td");
    td1.innerHTML = item.ownerName;

    const td2 = document.createElement("td");
    td2.innerHTML = item.ownerEmail;

    const td3 = document.createElement("td");
    td3.innerHTML = item.formStatus;

    const td4 = document.createElement("td");
    if (item.cat == null){
        td4.innerHTML = 'Null' ;
    }
    else {
        td4.innerHTML = item.cat.id ;
    }
    
    const tr = document.createElement("tr");

    tr.appendChild(td0);
    tr.appendChild(td1);
    tr.appendChild(td2);
    tr.appendChild(td3);
    tr.appendChild(td4);

    document.getElementById("tableBody").appendChild(tr);
}

function addItemWithActions(item, acceptUrl, denyUrl) {
    const td0 = document.createElement("td");
    td0.innerHTML = item.id;

    const td1 = document.createElement("td");
    td1.innerHTML = item.ownerName;

    const td2 = document.createElement("td");
    td2.innerHTML = item.ownerEmail;

    const td3 = document.createElement("td");
    td3.innerHTML = item.formStatus;

    const td4 = document.createElement("td");
    if (item.cat == null){
        td4.innerHTML = 'Null' ;
    }
    else {
        td4.innerHTML = item.cat.id ;
    }

    const td5 = document.createElement("td");
    td5.innerHTML = `
        <button class='btn-success' onclick='acceptOrDenyForm("${acceptUrl}","${item.id}")'>Accept</button> 
        <button class='btn-danger' onclick='acceptOrDenyForm("${denyUrl}","${item.id}")'>Deny</button>
    `
    
    const tr = document.createElement("tr");

    tr.appendChild(td0);
    tr.appendChild(td1);
    tr.appendChild(td2);
    tr.appendChild(td3);
    tr.appendChild(td4);
    tr.appendChild(td5);

    document.getElementById("tableBody").appendChild(tr);
}

function acceptOrDenyForm(url, formId){
    let bearer = localStorage.getItem("bear")
    let form = {
        Id: formId
    }
    if (bearer == null || bearer == ""){
        alert("No login stored, please log in first!")
        return;
    }
    fetch(apiUrl + url, {
        method: "POST",
        headers: {
            "Authorization": "Bearer " + bearer,
            "Content-Type": "application/json; charset=utf-8",
        },
        body: JSON.stringify(form)
    })
    .then(response => response.json())
    .then(response => {
        alert(response.message)
        document.getElementById("tableBody").innerHTML = ""
    })
}