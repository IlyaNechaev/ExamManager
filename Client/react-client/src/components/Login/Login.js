import {
  BrowserRouter as Router,
  Route,
  Routes,
  Navigate,
} from "react-router-dom";
import InputForm from "../InputForm/InputForm";
import "./login.css";

function Login(props) {
  let pathName = window.location.pathname;
  console.log(pathName);
  return (
    <div className="login">
      <Routes>
        <Route
          path="login"
          element={
            <InputForm
              formTitle="Авторизуйтесь"
              inputItems={[
                { type: "input", placeholder: "Логин" },
                { type: "password", placeholder: "Пароль" },
              ]}
            />
          }
        />
        <Route
          path="change/:pageId"
          element={
            <InputForm
              formTitle="Измените учетные данные"
              inputItems={[
                { type: "input", placeholder: "Новый логин" },
                { type: "input", placeholder: "Новый пароль" },
                { type: "input", placeholder: "Подтвердите пароль" },
              ]}
            />
          }
        />
      </Routes>
    </div>
  );
}

export default Login;
