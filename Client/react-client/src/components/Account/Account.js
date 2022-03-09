import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from "react-router-dom";
import NavBar from "../NavBar/NavBar";
import "./account.css";

function Account(props) {
    let pathName = window.location.pathname;
    
    return (
    <div className="account">
      <NavBar
        userName="Нечаев Илья"
        items={[
          {
            title: "Главная",
            href: `${pathName}/home`,
          },
          {
            title: "Настройки",
            href: `${pathName}/settings`,
          },
        ]}
      />
    </div>
  );
}

export default Account;