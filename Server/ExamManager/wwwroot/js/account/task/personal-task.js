let taskId = $('.page-header').attr('value');
let userId = "";

let connection = new signalR.HubConnectionBuilder()
    .withUrl('/notification')
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.on("Notify", function (user, message) {
    if (user != userId) {
        return;
    }
    let result = JSON.parse(message);

    if (result.Method === "start") {
        if (result.Status == 1) {
            console.log(result.VMachine);
            turnOnVM(result.VMachineImage, result.VMachine);
        }
        else if (result.Status == 2) {
            console.log(result.VMachineImage);
            turnOffVM("start", result.VMachineImage);
        }
    }
    else if (result.Method == "stop") {
        if (result.Status == 1) {
            console.log(result.VMachine);
            turnOnVM(result.VMachineImage, result.VMachine);
        }
        else if (result.Status == 2) {
            console.log(result.VMachineImage);
            turnOffVM("stop", result.VMachineImage, result.VMachine);
        }
    }
    else if (result.Method == "status" && result.Status == 2) {
        turnOffVM("status", result.VMachineImage);
    }
});

let turnOnVM = function (imageId, vmId) {
    let description = $(`#${imageId} > .description`);
    let actions = $(`#${imageId} > .actions`);

    description.html("Включена");
    description.removeClass();
    description.addClass('description success');

    actions.empty();
    let connectButton = $(`<input type="button" class="btn-action connect" value="Подключиться" />`);
    let turnOffButton = $(`<input type="button" class="btn-cancel turn-off" value="Отключить" />`);
    actions.append(connectButton);
    actions.append(turnOffButton);

    let vMachine = $(`#${imageId}`);
    vMachine.attr('vmId', vmId);

    assignListeners();
}

let turnOffVM = function (method, imageId, vmId) {
    let description = $(`#${imageId} > .description`);
    let actions = $(`#${imageId} > .actions`);
    let connectionInfo = $(`#${imageId} > .connection-info`);

    if (method === "stop" && description.parent().attr('vmid') != vmId) {
        return;
    }

    description.html("Отключена");
    description.removeClass();
    description.addClass('description danger');

    actions.empty();
    let turnOnButton = $(`<input type="button" class="btn-info turn-on" value="Включить" />`);
    actions.append(turnOnButton);

    if (connectionInfo != null) {
        connectionInfo.remove();
    }

    assignListeners();
}

let assignListeners = function () {
    $(".connect").on('click', function () {
        let vmId = $(this).parent().parent().attr('vmid');
        console.log(vmId);

        let onResponse = function (response) {
            console.log(response);

            if (response.type === "BadResponse") {
                return;
            }
            let actions = $(`[vmid="${vmId}"] > .actions`);

            let connectionInfo = $('<div class="connection-info"></div>');
            let parts = response.text.split(";\r\n");

            for (let part of parts) {
                if (part === "") {
                    continue;
                }
                let row = $('<div class="row"></div>');
                row.append($(`<div class="section">${part.split('=')[0]}</div>`));
                row.append($(`<div class="section">${part.split('=')[1]}</div>`));

                connectionInfo.append(row);
            }

            actions.after(connectionInfo);
        }

        connectVMachine(vmId, onResponse);
    });

    $(".check").on('click', function () {
        let taskId = (new URLSearchParams(window.location.search)).get('id');
        let btn = $(this);
        btn.addClass('disabled');
        btn.attr('disabled', 'disabled');

        let onResponse = function (response) {
            console.log(response);
        }

        checkTask(taskId, onResponse);

        setTimeout(function () {
            btn.removeClass('disabled');
            btn.removeAttr('disabled');
        }, 2000);
    });

    // Включение ВМ
    $(".turn-on").on('click', function () {
        $(this).addClass('disabled');
        $(this).attr('disabled', 'disabled');
        let id = $(this).parent().parent().attr('id');
        let description = $(`#${id} > .description`);

        description.removeClass();
        description.addClass('description warning');
        description.html('Включение');

        let onResponse = function (response) {
            console.log(response);
        }

        startTask(taskId, id, onResponse);
    });

    $(".turn-off").on('click', function () {
        let id = $(this).parent().parent().attr('vmid');
        if (id === "") {
            return;
        }

        $(this).toggleClass('disabled');
        $(this).attr('disabled', 'disabled');

        let onResponse = function (response) {
            console.log(response);
        }

        stopTask(id, onResponse);
    });
}


$(document).ready(function () {
    assignListeners();

    userId = decoded["Claim.Key.Id"];
    connection.start();
})