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
            if (response.status != 200) {
                alert(response.message)
            }
            else {
                document.getElementById("catSelection").innerHTML = "";
                addSelection(response.data)
            }
        })
}

function createCatForAdoption() {
    let name = document.getElementById("ownerName").value
    let email = document.getElementById("ownerEmail").value
    let catName = document.getElementById("name").value
    let catDescription = document.getElementById("description").value
    if (name == "") {
        alert("Your Name must be filled out");
        return;
    }
    if (email == "") {
        alert("Your Email must be filled out");
        return;
    }
    if (catName == "") {
        alert("Cat Name must be filled out");
        return;
    }
    if (catDescription == "") {
        alert("Cat Description must be filled out");
        return;
    }

    let catForAdoption = {
        Name: catName,
        Description: catDescription
    }

    let catForAdoptionRequest = {
        OwnerName: name,
        OwnerEmail: email,
        Cat: catForAdoption
    }

    fetch(apiUrl + "api/forms/requestCatForAdoption", {
        method: "POST",
        headers: {
            "Content-Type": "application/json; charset=utf-8"
        },
        body: JSON.stringify(catForAdoptionRequest)
    })
        .then(response => response.json())
        .then(response => {
            if (response.status != 200) {
                alert(response.message)
            }
        })
}

function adoptCat() {
    let name = document.getElementById("ownerName").value
    let email = document.getElementById("ownerEmail").value
    let catId = document.getElementById("catSelection")
    catId = catId.options[catId.selectedIndex].value
    if (name == "") {
        alert("Your Name must be filled out");
        return;
    }
    if (email == "") {
        alert("Your Email must be filled out");
        return;
    }
    if (catId == "") {
        alert("You need to select a cat");
        return;
    }

    let adoptCatRequest = {
        OwnerName: name,
        OwnerEmail: email,
        CatId: catId
    }

    console.log(adoptCatRequest);
    
    fetch(apiUrl + "api/forms/requestToAdoptCat", {
        method: "POST",
        headers: {
            "Content-Type": "application/json; charset=utf-8"
        },
        body: JSON.stringify(adoptCatRequest)
    })
        .then(response => response.json())
        .then(response => {
            console.log(response);
            if (response.status != 200) {
                alert(response.message)
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
        option.setAttribute("value", item.id)
    }

    document.getElementById("catSelection").appendChild(option)
}