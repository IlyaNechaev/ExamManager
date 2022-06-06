let taskId = $('.page-header').attr('value');

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

let turnOffVM = function (imageId) {
    let description = $(`#${imageId} > .description`);
    let actions = $(`#${imageId} > .actions`);

    description.html("Отключена");
    description.removeClass();
    description.addClass('description danger');

    actions.empty();
    let turnOnButton = $(`<input type="button" class="btn-info turn-on" value="Включить" />`);
    actions.append(turnOnButton);

    assignListeners();
}

let assignListeners = function () {
    $(".connect").on('click', function () {
        let vmId = $(this).parent().parent().attr('vmid');
        console.log(vmId);

        let onResponse = function (response) {
            console.log(response);
            let footer = $('.footer');
            let parts = response.text.split(";\r\n");
            for (let part of parts) {
                footer.append($(`<p>${part}</p>`));
            }
        }

        connectVMachine(vmId, onResponse);
    });

    // Включение ВМ
    $(".turn-on").on('click', function () {
        $(this).toggleClass('disabled');
        $(this).attr('disabled', 'disabled');
        let id = $(this).parent().parent().attr('id');
        let description = $(`#${id} > .description`);

        description.removeClass();
        description.addClass('description warning');
        description.html('Включение');

        let onResponse = function (response) {
            console.log(response);
            if (response.type === "BadResponse") {
                turnOffVM(id);
            }
            else {
                turnOnVM(id, response.vmid);
            }
        }

        startTask(taskId, id, onResponse);
    })
}


$(document).ready(function () {
    assignListeners();    
})