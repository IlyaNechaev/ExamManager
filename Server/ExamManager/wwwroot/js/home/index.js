let submitButton = $("#submitButton");
let form = $("#form");

const jwt_token = Cookies.get("token");

// Если пользователь авторизован
if (jwt_token) {
    let decoded = jwt_decode(jwt_token);

    let onResponse = function (response) {

        let write = function (response) {
            console.log(response);
        }
        handleRequest("/pages/home", "GET", null, write);
    }

    handleRequest(`/user/${decoded["Claim.Key.Id"]}`, "GET", null, onResponse);
}

// Запрос при авторизации
let handleLogin = function (event) {
    // Очистить сообщения об ошибках
    $(".field-validation-error").remove();

    event.preventDefault();
    submitButton.disabled = true;

    let data =
    {
        login: this.login.value,
        password: this.password.value
    };

    let onResponse = function (response) {
        onSuccess(response);
    };
    handleRequest("/login", "POST", data, onResponse);
}

let onSuccess = function (response) {
    if (response.type === "ErrorsResponse") {
        handleBadResponse(response);
    }
    else if (response.type === "JWTResponse") {
        handleJWTResponse(response);
    }
}

let handleBadResponse = function (response) {
    $("#password").val("");

    // Вывести ошибки
    for (let error in response.errors) {
        let inputElement = $(`#${error.toLowerCase()}`);
        for (let errorText of response.errors[error]) {
            let errorMessage = $(`<span class="field-validation-error">${errorText}</span>`);
            inputElement.after(errorMessage);
            inputElement.addClass("input-validation-error");
        }
    }
}

let handleJWTResponse = function (response) {
    // Устанавливаем токен
    let jwtToken = response.token;
    Cookies.set("token", jwtToken);

    // Если пользователь авторизовался в первый раз
    window.location.replace("/pages/home");
}

let handleDefault = function () {

    // Очистить сообщения об ошибках
    $(".field-validation-error").remove();

    event.preventDefault();
    submitButton.disabled = true;

    if (this.new.value !== this.confirm.value) {
        let errors = {
            "confirm": ["Пароли должны совпадать"]
        };
        handleBadResponse({ errors: errors });
        return;
    }

    let data =
    {
        id: jwt_decode(Cookies.get("token"))["Claim.Key.Id"],
        login: this.login.value,
        password: this.new.value,
        isDefault: false
    };

    let onResponse = function (response) {
        let data = response;
        if (data.type === "ErrorsResponse") {
            handleBadResponse(data);
        }
        else if (data.type === "UserDataResponse") {
            window.location.replace("/pages/home");
        }
    };
    handleRequest("/user/modify", "POST", data, onResponse);

    //let onResponse = function (response) {

    //}

    //handleRequest("/user/modify")
}


form.on('submit', handleLogin);