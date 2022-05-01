let submitButton = $("#submitButton");
let form = $("#form");

let handleLogin = function (event) {
    console.log(this.password.value);
    event.preventDefault();
    submitButton.disabled = true;

    let data = JSON.stringify(
        {
            login: this.login.value,
            password: this.password.value
        });
    console.log(data);

    const xmlhttp = new XMLHttpRequest();
    xmlhttp.open("POST", "/login");
    xmlhttp.setRequestHeader("Accept", "application/json");
    xmlhttp.setRequestHeader("Content-Type", "application/json");

    xmlhttp.onload = function () {
        onSuccess(JSON.parse(this.responseText));
    };
    xmlhttp.send(data);
}

let onSuccess = function (data) {
    console.log(data);
}

form.submit(handleLogin);
console.log(form);