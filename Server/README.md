# 1. API серверной части ExamManager

|          Метод          |          Описание          |          Запрос          |          Ответ          |
|--------|--------|--------|--------|
|  **POST** */login*  |  Проверить зарегистрирован ли пользователь и сгенерировать для него JWT-токен  |  LoginEditModel  |  Response  |
|  **GET** */user/{id}*  |  Получить информацию о пользователе по его ID  |  id  |  UserDataResponse  |
| **GET** */user/{id}/tasks* | Получить информацию о заданиях, которые имеются у пользователя ID | id | TasksDataResponse |
| **GET** */task/{id}* | Получить информацию о задании по его ID | id | TaskDataResponse |
| **POST** */task/create* | Создать задание | CreateTaskRequest | TaskDataResponse |
| **POST** */task/delete* | Удалить задание | DeleteTaskRequet | Response |
| **POST** */task/modify* | Изменить задание | ModifyTaskRequest | TaskDataResponse |
| **GET** */group/{id}* | Получить информацию о группе по ее ID | id | GroupDataResponse |
| **GET** */group/{id}/students* | Получить информацию о студентах, которые состоят в группе ID | id | UsersDataResponse |
| **POST** */group/create* |  Создать группу студентов  |  CreateGroupRequest  |  GroupDataResponse  |
| **POST** */group/students/add* | Добавить студентов к группе | AddStudentsRequest | GroupDataResponse |
| **POST** */group/students/remove* | Удалить студентов из группы | RemoveStudentsRequest | GroupDataResponse |
| **POST** */students* | Получить список информации о студентах | GetStudentsRequest | UsersDataResponse |
| **POST** */students/create* | Зарегистрировать студентов | CreateStudentsRequest | UsersDataResponse |
| **POST** */students/delete* | Удалить студентов | DeleteStudentsRequest | Response |

# 2. Структура запросов

## LoginEditModel

| Поле     | Тип данных | Описание            |
| -------- | ---------- | ------------------- |
| Login    | string     | Логин пользователя  |
| Password | string     | Пароль пользователя |



## CreateTaskRequest

| Поле | Тип данных | Описание |
| ---- | ---------- | -------- |
|      |            |          |



## DeleteTaskRequest

| Поле | Тип данных | Описание |
| ---- | ---------- | -------- |
|      |            |          |



## ModifyTaskRequest

| Поле | Тип данных | Описание |
| ---- | ---------- | -------- |
|      |            |          |



## CreateGroupRequest

| Поле | Тип данных | Описание |
| ---- | ---------- | -------- |
|      |            |          |



## AddStudentsRequest

| Поле | Тип данных | Описание |
| ---- | ---------- | -------- |
|      |            |          |



## RemoveStudentsRequest

| Поле | Тип данных | Описание |
| ---- | ---------- | -------- |
|      |            |          |



## GetStudentsRequest

| Поле | Тип данных | Описание |
| ---- | ---------- | -------- |
|      |            |          |



## CreateStudentsRequest

| Поле    | Тип данных | Описание                                      |
| ------- | ---------- | --------------------------------------------- |
| GroupID | guid       | ID группы, для которой будут созданы студенты |



## DeleteStudentsRequest

| Поле | Тип данных | Описание |
| ---- | ---------- | -------- |
|      |            |          |



# 3. Структура ответов

## 3.1. Response

