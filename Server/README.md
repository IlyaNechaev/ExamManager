# API серверной части ExamManager

|          Метод          |          Описание          |          Запрос          |          Ответ          |
|--------|--------|--------|--------|
|  **POST** */login*  |  Проверяет зарегистрирован ли пользователь и генерирует для него JWT-токен  |  LoginEditModel  |  Response  |
|  **GET** */user/{id}*  |  Возвращает информацию о пользователе по его ID  |  id  |  UserDataResponse  |
| **POST** */admin/group/create* |  Создает группу студентов  |  CreateGroupRequest  |  GroupDataResponse  |
| **POST** */admin/group/add* | Добавляет студента к группе | AddStudentRequest | GroupDataResponse |
| **POST** */admin/group/remove* | Удаляет студента из группы | RemoveStudentRequest | GroupDataResponse |
| **POST** */admin/students* | Возвращает список информации о студентах | GetStudentsRequest | StudentsDataResponse |
| **POST** */admin/students/create* | Регистрирует студента | CreateStudentRequest | UserDataResponse |
| **POST** */admin/students/delete* | Удаляет студента | DeleteStudentRequest | Response |
