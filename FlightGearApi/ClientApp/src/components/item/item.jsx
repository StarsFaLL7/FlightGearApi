import React from "react";

import plus from '../../assets/img/plus.png'
import minus from '../../assets/img/minus.png'

const Item = ({name, count}) => {
    const [isMinus, setIsMinus] = React.useState(false);

    const toggleSrc = () => {
        setIsMinus(!isMinus)
    }
    return (<tr><td><button onClick={toggleSrc}>{name}{count}{isMinus ? (<img src={plus} width="20" height="20" alt="Плюс" />) : (<img src={minus} width="20" height="20" alt="Минус" />)}</button></td></tr>)
}

export default Item;