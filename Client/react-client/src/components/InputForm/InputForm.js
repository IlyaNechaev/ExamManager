import './input-form.css';
import { useParams} from "react-router-dom";

function InputForm(props) {
    
    let inputItems = [];
    for (let item of props.inputItems) {
        inputItems.push(<input key="unique1" className="normal" type={`${item.type}`} placeholder={`${item.placeholder}`} />);
        inputItems.push(<span key="unique2" className="danger" style={{ fontWeight: "bold" }}></span>);
    }
    return (
        <form className="input-form" method="POST" action="login" autoComplete="off">
            <div className="header">{props.formTitle}</div>
            <div className="body">
                {inputItems}
            </div>
            <div className="footer">
                <input type="submit" name="sumbitButton" value="Войти" onClick="this.form.submit(); this.disabled=true;" />
            </div>
        </form>
    )
}

export default InputForm;