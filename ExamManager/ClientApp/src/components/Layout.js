import React from 'react';
import NavBar from './NavBar/NavBar';

function Layout(props) {

    return (
        <div>
            <NavBar userName="����" items={
                [
                    {
                        href: "/home",
                        title: "�������"
                    }
                ]
            } />
            <div className="container">
                {props.children}
            </div>
        </div>
    );
}

function fetchUserData() {

}

export default Layout;