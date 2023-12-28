import React from "react";
import './item.module.css'

import plus from '../../../assets/img/plus.png'
import minus from '../../../assets/img/minus.png'

import { useDispatch, useSelector } from "react-redux";
import { delData, setData} from "../../../redux/ChartSlice/ChartSlice";

const Item = ({name, count}) => {
    const [isMinus, setIsMinus] = React.useState(false);
    const dataChart = useSelector((state) => state.chart.data);

    const dispatch = useDispatch()

    const toggleSrc = () => {
        setIsMinus(!isMinus)
    }
    const handleClick = (name) => {
        toggleSrc();
        const index = dataChart.indexOf(name);
        if(!isMinus) {
            dispatch(setData(name));
        } else if (index !== -1) {
            dispatch(delData(index));
        }
        
    }
    return (
        <tr>
            <td>
                <button onClick={() => handleClick(name)}>
                    {name} ({count}){isMinus ? (<img src={minus} width="14" height="14" alt="Минус" />) : (<img src={plus} width="14" height="14" alt="Плюс" />)}
                </button>
            </td>
        </tr>
    )
}

export default Item;