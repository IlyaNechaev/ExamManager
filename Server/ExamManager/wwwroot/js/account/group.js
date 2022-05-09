let studentsToAdd = [];

// Заполнение информации о студенте
let fillUserInfo = function (userInfo) {
    let studentInfo = $("#student-info");
    studentInfo.empty();

    if (userInfo == null) {
        return;
    }
    let studentName = $(`<div class="name"><span>${userInfo['lastName']} ${userInfo['firstName']}</span><a id="delete-user"><i class="fa fa-solid fa-trash-can"></i></a></div>`);
    let tasks = $('<div class="tasks"></div>')
    for (let task of userInfo.tasks) {
        let taskElement = $(`<div class="task"><a href="/pages/task?id=${task.id}&student=${userInfo['id']}" class="title">${task.title}</a><div class="status">Выполнено</div></div>`);
        tasks.append(taskElement);
    }

    studentInfo.attr("value", userInfo['id']);
    studentInfo.append(studentName);
    studentInfo.append(tasks);

    $("#delete-user").on("click", function (e) {
        let data = {
            students: [
                {
                    id: userInfo['id']
                }
            ]
        }

        removeGroupStudents(JSON.stringify(data), (response) => {
            fillUserInfo(null);
            let groupId = $(".students-table").attr('value');
            getGroupStudents(groupId, onStudentsInfoResponse);
        });
    });
}

// При получении списка студентов
let onStudentsInfoResponse = function (response) {
    let studentsList = $("#students");

    let students = JSON.parse(response.responseText).users;
    studentsList.empty();

    for (let student of students) {
        if (student.id === decoded["Claim.Key.Id"]) {
            continue;
        }
        let studentElement = $(`<div class="student" value="${student['id']}">${student['lastName']} ${student['firstName']}</div>`);

        let onSelected = function () {
            $(".student").each(function (index) {
                $(this).removeClass("hl");
            });
            studentElement.addClass("hl");

            getUser(student['id'], function (response) {
                // Передаем ответ в функцию заполнения информаци
                fillUserInfo(JSON.parse(response.responseText));
            });
        }

        studentElement.on("click", onSelected);
        studentsList.append(studentElement);
    }
}

// При вводе имени студента
function updateStudents(e) {
    let studentName = e.target.value;
    let groupId = $(".students-table").attr('value');

    let data = {
        name: studentName,
        groupIds: [groupId]
    };

    // Если строка пустая, то возвращаем всех студентов
    if (studentName === "") {
        data.name = null;
        data.firstName = null;
        data.lastName = null;
    }

    getUsers(JSON.stringify(data), onStudentsInfoResponse);
}

let updateStudentsToAdd = function (e) {
    let studentName = e.target.value;
    let groupId = $(".students-table").attr('value');

    let data = {
        name: studentName,
        withoutGroup: true
    };

    // Если строка пустая, то возвращаем всех студентов
    if (studentName === "") {
        data.name = null;
        data.firstName = null;
        data.lastName = null;
    }

    getUsers(JSON.stringify(data), onStudentsToAddInfoResponse);
}

let onStudentsToAddInfoResponse = function (response) {
    let studentsList = $(".add-student-modal .students-list");

    let students = JSON.parse(response.responseText).users;
    studentsList.empty();

    for (let student of students) {
        if (student.id === decoded["Claim.Key.Id"]) {
            continue;
        }
        let studentElement = $(`<div class="student" value="${student['id']}"><i class="fa fa-solid fa-user"></i><div class="name">${student['lastName']} ${student['firstName']}</div></div>`);

        if (studentsToAdd.includes(student['id'])) {
            studentElement.addClass('hl');
        }

        let onSelected = function () {
            studentElement.toggleClass("hl");
            let studentId = studentElement.attr('value');
            // Если студент был в списке на добавление
            if (studentsToAdd.includes(studentId)) {
                for (var i = 0; i < studentsToAdd.length; i++) {
                    if (studentsToAdd[i] === studentId) {
                        studentsToAdd.splice(i, 1);
                    }
                }
            }
            else {
                studentsToAdd.push(studentId);
            }
            console.log(studentsToAdd);
        }

        studentElement.on("click", onSelected);
        studentsList.append(studentElement);
    }
}

window.onload = function () {
    let groupId = $(".students-table").attr('value');
    getGroupStudents(groupId, onStudentsInfoResponse);

    const modal = document.querySelector("#add-student-modal");
    const openModal = document.querySelector("#open-modal");
    const closeModal = document.querySelector("#close-modal")
    openModal.addEventListener("click", () => {
        modal.showModal();
        searchInput.val("").trigger("input");
    });

    closeModal.addEventListener("click", () => {
        modal.close();
        let studentsList = $(".add-student-modal .students-list");
        studentsList.empty();
        studentsList.append($('<div class="loader"></div>'));
        studentsToAdd = [];
    });

    let searchInput = $("#search-student-name");
    searchInput.on("input", updateStudents);
    searchInput.val("").trigger("input");

    searchInput = $("#search-add-students");
    searchInput.on("input", updateStudentsToAdd);
}