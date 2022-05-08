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

let onStudentsToAddInfoResponse = function (response) {

}

window.onload = function () {
    let groupId = $(".students-table").attr('value');
    getGroupStudents(groupId, onStudentsInfoResponse);

    const modal = document.querySelector("#add-student-modal");
    const openModal = document.querySelector("#open-modal");
    const closeModal = document.querySelector("#close-modal");

    openModal.addEventListener("click", () => {
        modal.showModal();
    });

    closeModal.addEventListener("click", () => {
        modal.close();
    });
}