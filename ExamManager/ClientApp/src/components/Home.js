import React from 'react';
import InputForm from './InputForm/InputForm'

function Home(props) {
    return (
        <InputForm formTitle="�����������" inputItems={
            [
                {
                    type: "input",
                    placeholder: "���"
                }
            ]} />
    )
}

export default Home;