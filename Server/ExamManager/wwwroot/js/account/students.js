// Открытие/закрытие модального окна создания группы
const modal = document.querySelector("#create-student-modal");
const openModal = document.querySelector("#open-button");
const closeModal = document.querySelector("#close-button");
const createButton = document.querySelector("#create-student-button");
const reloadButton = document.querySelector("#reload-button");

// Поиск студента (выпадающий список)
const searchInput = $("#student-name");
searchInput.on("input", updateStudents);
searchInput.val("").trigger("input");

// При вводе имени студента
function updateStudents(e) {
    let studentName = e.target.value;

    let data = {
        firstName: studentName,
        lastName: studentName
    };

    // Если строка пустая, то возвращаем всех студентов
    if (studentName === "") {
        data.firstName = null;
        data.lastName = null;
    }

    let onResponse = function (response) {
        console.log(response.responseText);
        fillStudents(JSON.parse(response.responseText));
    };

    getUsers(JSON.stringify(data), onResponse);
}

// Заполнение таблицы студентов
function fillStudents(data) {
    let oldTable = $(".students-table>.body");
    if (oldTable) {
        oldTable.empty();
    }

    let studentsTableBody = $(".students-table>.body");

    let index = 1;
    for (user of data.users) {

        if (decoded["Claim.Key.Id"] == user.id) {
            continue;
        }
        console.log(user);
        let tableRow = $(`<div class="row" student="${user.id}">` +
            `<div>${index}</div>` +
            `<div class="student-name">${user.lastName} ${user.firstName}</div>` +
            `<div class="description">${user.groupName == null ? "-" : user.groupName}</div >` +
            `<div class="description">${user.tasks.length}</div >` +
            '</div> ');


        let actionsColumn = $('<div class="actions"> ' +
            `<a class="edit" href="/pages/user/${user.id}">` +
            '<i class="fa fa-solid fa-pen"></i>' +
            '</a>' +
            '</div>');

        let deleteButton = $(`<a class="delete">` +
            '<i class="fa fa-solid fa-trash"></i>' +
            '</a>');

        deleteButton.on("click", function (e) {
            deleteUser(user.id, function (reponse) {
                window.location.reload();
            });
        });

        actionsColumn.append(deleteButton);
        tableRow.append(actionsColumn);

        studentsTableBody.append(tableRow);
        index += 1;
    }
}

let deleteUser = function (id) {
    let data = {
        users: [
            {
                id: id,
                onlyLogin: false
            }
        ]
    }

    let onResponse = function (response) {
        window.location.reload();
    }

    deleteUsers(JSON.stringify(data), onResponse);
}

// Добавить нового студента
let createNewUser = function () {
    let firstName = $("#firstname").val();
    let lastName = $("#lastname").val();
    let login = $("#login").val();
    let password = $("#password").val();

    let errors = {};
    if (firstName === "") {
        errors["firstname"] = ["Введите имя"];
    }
    if (lastName === "") {
        errors["lastname"] = ["Введите фамилию"];
    }
    if (login === "") {
        errors["login"] = ["Введите логин"];
    }
    if (password === "") {
        errors["password"] = ["Введите пароль"];
    }

    if (Object.keys(errors).length > 0) {
        handleErrors(errors);
        return;
    }

    let data = {
        users: [
            {
                login: login,
                password: password,
                firstName: firstName,
                lastName: lastName,
                role: 1,
                groupId: null
            }
        ]
    }

    let onResponse = function (response) {
        window.location.reload();
    }

    createUsers(JSON.stringify(data), onResponse);
}

let handleErrors = function (errors) {
    $(".field-validation-error").each(function (index) {
        $(this).remove();
    });
    // Вывести ошибки
    for (let error in errors) {
        let inputElement = $(`#${error.toLowerCase()}`);
        for (let errorText of errors[error]) {
            let errorMessage = $(`<span class="field-validation-error">${errorText}</span>`);
            inputElement.after(errorMessage);
            inputElement.addClass("input-validation-error");
        }
    }
}

openModal.addEventListener("click", () => {
    modal.showModal();

});

closeModal.addEventListener("click", () => {
    modal.close();
});

reloadButton.addEventListener("click", () => {
    let value = $("#student-name").val();
    updateStudents(
        {
            target: {
                value: value
            }
        });
});

createButton.addEventListener("click", createNewUser);

updateStudents(
    {
        target: {
            value: ""
        }
    });