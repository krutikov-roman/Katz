/*
    By: Roman Krutikov

    Description: This javascript file includes methods for logging in and logging
                 out of an account (which will primarily be used by admins)

*/

function tryLogin() {
    let email = document.getElementById("loginEmail").value;
    let password = document.getElementById("loginPassword").value;
    if (email == "") {
      alert("Email must be filled out");
      return;
    }
    if (password == "") {
        alert("Password must be filled out");
        return;
    }
    let loginData = {
        Email: email,
        Password: password
    }
    setBearer(loginData);
}

function trySignOut() {
    let bearer = localStorage.getItem("bear")
    if (bearer == null || bearer == ""){
        alert("You are not signed in!")
        return;
    }
    let token = {
        Token: bearer
    }
    fetch(apiUrl + "api/admin/signout", {
        method: "POST",
        headers: {
            "Content-Type": "application/json; charset=utf-8"
        },
        body: JSON.stringify(token)
    })
    .then(response => response.json())
    .then(response => {
        if (response.status == 200){
            localStorage.removeItem("bear")
        }
        alert(response.message);
    })
}

function setBearer(loginData) {
    fetch(apiUrl + "api/admin/login", {
        method: "POST",
        headers: {
            "Content-Type": "application/json; charset=utf-8"
        },
        body: JSON.stringify(loginData)
    })
    .then(response => response.json())
    .then(response => {
        alert(response.message);
        if (response.status == 200){
            localStorage.setItem("bear", response.data)
        }
    })
}
