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

| Поле     | Тип данных | Обязательно | Описание            |
| -------- | ---------- | ----------- | ------------------- |
| login    | string     | Да          | Логин пользователя  |
| password | string     | Да          | Пароль пользователя |



## CreateTaskRequest

| Поле        | Тип данных | Обязательно | Описание                                                     |
| ----------- | ---------- | ----------- | ------------------------------------------------------------ |
| title       | string     | Да          | Название задания                                             |
| description | string     |             | Описание задания                                             |
| url         | string     | Да          | URL для перехода к ресурсу задания                           |
| studentId   | guid       | Да          | ID студента, для которого будет создано задание              |
| authorId    | guid       |             | ID автора задания. Если значение отсутствует,<br />то будет записан ID текущего пользователя |



## DeleteTaskRequest

| Поле   | Тип данных | Обязательно | Описание                               |
| ------ | ---------- | ----------- | -------------------------------------- |
| taskId | guid       | Да          | ID задания, которое необходимо удалить |



## ModifyTaskRequest

В теле данного запроса значения всех полей кроме *taskId* заменят текущие значения полей задания 

| Поле        | Тип данных | Обязательно | Описание                                  |
| ----------- | ---------- | ----------- | ----------------------------------------- |
| taskId      | guid       | Да          | ID задания, которое необходимо изменить   |
| title       | string     |             | Название задания                          |
| description | string     |             | Описание задания                          |
| studentId   | guid       |             | ID студента, для которого создано задание |
| authorId    | guid       |             | ID автора задания                         |
| status      | int        |             | Статус задания                            |



## CreateGroupRequest

| Поле | Тип данных | Обязательно | Описание        |
| ---- | ---------- | ----------- | --------------- |
| name | string     | Да          | Название группы |



## AddStudentsRequest

<table>
    <tr>
    	<th colspan=2>Поле</th>
        <th>Тип данных</th>
        <th>Обязательно</th>
        <th>Описание</th>
    </tr>
    <tr>
    	<td colspan=2>groupId</td>
        <td>guid</td>
        <td>Да</td>
        <td>ID группы, в которую будут добавлены студенты</td>
    </tr>
    <tr>
    	<td colspan=2>students</td>
        <td>array</td>
        <td></td>
        <td></td>
    </tr>
    <tr>
        <td></td>
        <td>id</td>
        <td>guid</td>
        <td>Да</td>
        <td>ID студента, которого необходимо добавить в группу</td>    	
    </tr>
</table>



## RemoveStudentsRequest

<table>
    <tr>
    	<th colspan=2>Поле</th>
    	<th>Тип данных</th>
    	<th>Обязательно</th>
    	<th>Описание</th>
    </tr>
    <tr>
    	<td colspan=2>groupId</td>
        <td>guid</td>
        <td></td>
        <td>ID группы, из которой необходимо убрать студентов</td>
    </tr>
    <tr>
    	<td colspan=2>students</td>
        <td>array</td>
        <td></td>
        <td></td>
    </tr>    
    <tr>
    	<td></td>
        <td>studentId</td>
    	<td>guid</td>
        <td>Да</td>
        <td>ID студента, которого необходимо убрать из группы</td>
    </tr> 
    <tr>
    	<td></td>
        <td>groupId</td>
    	<td>guid</td>
        <td>Да</td>
        <td>ID группы, из которой необходимо убрать данного студента</td>
    </tr>
</table>



## GetStudentsRequest

| Поле       | Тип данных | Обязательно | Описание                                                     |
| ---------- | ---------- | ----------- | ------------------------------------------------------------ |
| groupId    | guid       |             | ID группы, студенты которой будут добавлены в выборку        |
| taskStatus | int        |             | Статус задания, при наличии которого (хотя бы одного задания<br />с таким статусом) студент будет добавлен в выборку |



## CreateStudentsRequest

<table>
    <tr>
        <th colspan=2>Поле</th>
        <th>Тип данных</th>
        <th>Обязательно</th>
        <th>Описание</th>
    </tr>
    <tr>
        <td colspan=2>groupId</td>
        <td>guid</td>
        <td>Да</td>
        <td>ID группы, для которой будут созданы студенты</td>
    </tr>
    <tr>
        <td colspan=2>students</td>
        <td>array</td>
        <td></td>
        <td></td>
    </tr>
    <tr>
    	<td></td>
        <td>login</td>
        <td>string</td>
        <td>Да</td>
        <td>Логин</td>
    </tr>
    <tr>
    	<td></td>
        <td>password</td>
        <td>string</td>
        <td>Да</td>
        <td>Пароль</td>
    </tr>
    <tr>
    	<td></td>
        <td>firstName</td>
        <td>string</td>
        <td>Да</td>
        <td>Имя</td>
    </tr>
    <tr>
    	<td></td>
        <td>lastName</td>
        <td>string</td>
        <td>Да</td>
        <td>Фамилия</td>
    </tr>
</table>



## DeleteStudentsRequest

<table>
    <tr>
    	<th colspan=2>Поле</th>
        <th>Тип данных</th>
        <th>Обязательно</th>
        <th>Описание</th>
    </tr>
    <tr>
    	<td colspan=2>students</td>
        <td>array</td>
        <td></td>
        <td></td>
    </tr>
    <tr>
    	<td></td>
        <td>id</td>
    	<td>guid</td>
    	<td>Да</td>
    	<td>ID студента, которого необходимо удалить</td>
    </tr>
    <tr>
    	<td></td>
        <td>onlyLogin</td>
    	<td>boolean</td>
    	<td>Да</td>
    	<td>
            В обоих случаях студент лишится возможности авторизоваться в системе<br/>
            <b>true</b> - информация о студенте сохранится<br/>
            <b>false</b> - информацию о студенте будет удалена
        </td>
    </tr>
</table>

# 3. Структура ответов

Каждый ответ имеет в своем теле поле *type*, в котором указан тип ответа

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

| Поле          | Тип данных | Описание                                 |
| ------------- | ---------- | ---------------------------------------- |
| id            | guid       | ID группы                                |
| name          | string     | Название                                 |
| studentsCount | int        | Количество студентов, состоящих в группе |



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

