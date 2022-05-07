﻿
$(document).ready(function () {
    const saveButton = $("#save-button");
    const changePassword = $("#change-password-button");

    let saveChanges = function (e) {
        saveButton.attr("disabled", "disabled");

        const firstname = $("#firstname-input").val();
        const lastname = $("#lastname-input").val();
        const login = $("#login-input").val();
        const userId = decoded["Claim.Key.Id"];

        let data = {
            id: userId,
            firstName: firstname,
            lastName: lastname,
            login: login
        };

        let onResponse = function (response) {
            window.location.reload();
        }

        modifyUser(JSON.stringify(data), onResponse);
    }

    saveButton.on("click", saveChanges);
})