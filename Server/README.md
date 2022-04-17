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
| login    | string     | Логин пользователя  |
| password | string     | Пароль пользователя |



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

<table>
    <tr>
        <th colspan=2>Поле</th>
        <th>Тип данных</th>
        <th>Описание</th>
    </tr>
    <tr>
        <td colspan=2>groupId</td>
        <td>guid</td>
        <td>ID группы, для которой будут созданы студенты</td>
    </tr>
    <tr>
        <td colspan=2>students</td>
        <td>array</td>
        <td></td>
    </tr>
    <tr>
    	<td></td>
        <td>login</td>
        <td>string</td>
        <td>Логин</td>
    </tr>
    <tr>
    	<td></td>
        <td>password</td>
        <td>string</td>
        <td>Пароль</td>
    </tr>
    <tr>
    	<td></td>
        <td>firstName</td>
        <td>string</td>
        <td>Имя</td>
    </tr>
    <tr>
    	<td></td>
        <td>lastName</td>
        <td>string</td>
        <td>Фамилия</td>
    </tr>
</table>



## DeleteStudentsRequest

| Поле     | Тип данных | Описание |
| -------- | ---------- | -------- |
| students |            |          |



# 3. Структура ответов

## Response

| Поле | Тип данных | Описание |
| ---- | ---------- | -------- |
|      |            |          |



## UsersDataResponse

<table>
    <tr>
    	<th colspan=2>Поле</th>
        <th>Тип данных</th>
        <th>Описание</th>
    </tr>
    <tr>
    	<td colspan=2>users</td>
        <td>array</td>
        <td></td>
    </tr>
    <tr>
    	<td></td>
        <td>id</td>
        <td>guid</td>
        <td>ID пользователя</td>
    </tr>
    <tr>
    	<td></td>
        <td>firstName</td>
        <td>string</td>
        <td>Имя</td>
    </tr>
    <tr>
    	<td></td>
        <td>lastName</td>
        <td>string</td>
        <td>Фамилия</td>
    </tr>
    <tr>
    	<td></td>
        <td>groupName</td>
        <td>string</td>
        <td>Название группы</td>
    </tr>
</table>



## TasksDataResponse

<table>
    <tr>
    	<th colspan=2>Поле</th>
        <th>Тип данных</th>
        <th>Описание</th>
    </tr>
    <tr>
    	<td colspan=2>tasks</td>
        <td>array</td>
        <td></td>
    </tr>
    <tr>
    	<td></td>
        <td>id</td>
        <td>guid</td>
        <td>ID задания</td>
    </tr>
    <tr>
    	<td></td>
        <td>title</td>
        <td>string</td>
        <td>Название</td>
    </tr>
    <tr>
    	<td></td>
        <td>description</td>
        <td>string</td>
        <td>Описание</td>
    </tr>
    <tr>
    	<td></td>
        <td>status</td>
        <td>int</td>
        <td>Статус</td>
    </tr>
    <tr>
    	<td></td>
        <td>studentId</td>
        <td>guid</td>
        <td>ID студента, которому принадлежит задание</td>
    </tr>
</table>



## TaskDataResponse

| Поле        | Тип данных | Описание                           |
| ----------- | ---------- | ---------------------------------- |
| id          | guid       | ID задания                         |
| title       | string     | Название                           |
| description | string     | Описание                           |
| status      | int        | Статус                             |
| authorId    | guid       | ID автора задания                  |
| url         | string     | URL для перехода к ресурсу задания |



## GroupDataResponse

| Поле | Тип данных | Описание |
| ---- | ---------- | -------- |
|      |            |          |



## BadResponse

<table>
    <tr>
    	<th colspan=3>Поле</th>
    	<th>Тип данных</th>
    	<th>Описание</th>
    </tr>
    <tr>
    	<td colspan=3>errors</td>
    	<td>dict</td>
    	<td></td>
    </tr>
    <tr>
    	<td></td>
    	<td colspan=2><i>element</i></td>
    	<td></td>
    	<td></td>
    </tr>
    <tr>
    	<td></td>
    	<td></td>
    	<td>key</td>
    	<td>string</td>
    	<td>Название элемента, к которому относится ошибка</td>
    </tr>
    <tr>
    	<td></td>
    	<td></td>
    	<td>value</td>
    	<td>string</td>
    	<td>Описание ошибки</td>
    </tr>
</table>



## ExceptionResponse

| Поле          | Тип данных | Описание                  |
| ------------- | ---------- | ------------------------- |
| exceptionType | string     | Тип вызванного исключения |
| message       | string     | Описание исключения       |
| stackTrace    | string     | Стек вызовов              |

