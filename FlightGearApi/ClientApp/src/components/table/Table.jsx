import plus from '../../assets/img/plus.png'
import minus from '../../assets/img/minus.png'
import './Table.module.css';
import React, { useState } from 'react';

function Table() {
    const [dataParams, setDataParams] = useState([]);
    const [isMinus, setIsMinus] = useState(false)

    const toggleSrc = () => {
        setIsMinus(!isMinus)
    }

    React.useEffect(() => {
        fetch('').then(data => console.log(data)).then(d => setDataParams(d))
    }, [])

    const paramsArray = [
        {
            name: 'Тангаж',
            count: '13862',
        },
        {
            name: 'Мангаж',
            count: '1323262',
        },
        {
            name: 'Гараж',
            count: '138262',
        }
    ]

    const params = {
        name: 'Тангаж',
        count: '13862'

    }
    
    return <table>
        <thead>
                <tr>
                    <th>Параметры</th>  
                </tr>
            </thead>
            <tbody>
            {paramsArray.map((par) => <tr><td><button onClick={toggleSrc}>{par.name}{par.count}{isMinus ? (<img src={plus} width="20" height="20" alt="Плюс" />) : <></>}</button></td></tr>)
            }
                {/* <tr><td onClick={()=>{bool?setBool(false):setBool(true)}}><button>Скорость севера (94533)<img src={bool ? plus:minus} width="20" height="20" alt={bool ? "Плюс":"Минус"} /></button></td></tr>
                <tr><td onClick={()=>{bool?setBool(false):setBool(true)}}><button>Скорость ветра (32326)<img src={bool ? plus:minus} width="20" height="20" alt={bool ? "Плюс":"Минус"} /></button></td></tr>
                <tr><td onClick={()=>{bool?setBool(false):setBool(true)}}><button>Скорость Восток (11036)<img src={bool ? plus:minus} width="20" height="20" alt={bool ? "Плюс":"Минус"} /></button></td></tr>
                <tr><td onClick={()=>{bool?setBool(false):setBool(true)}}><button>Крен (15196)<img src={bool ? plus:minus} width="20" height="20" alt={bool ? "Плюс":"Минус"} /></button></td></tr>
                <tr><td><button>Тангаж (13862)<img src="./img/plus.png" width="20" height="20" alt="Плюс" /></button></td></tr>
                <tr><td><button>Курс ист (13096)<img src="./img/plus.png" width="20" height="20" alt="Плюс" /></button></td></tr>
                <tr><td><button>Курс магн(11019)<img src="./img/plus.png" width="20" height="20" alt="Плюс" /></button></td></tr>
                <tr><td><button>Долгота (34195)<img src="./img/plus.png" width="20" height="20" alt="Плюс" /></button></td></tr>
                <tr><td><button>Широта (53994)<img src="./img/plus.png" width="20" height="20" alt="Плюс" /></button></td></tr>
                <tr><td><button>Угл сокрость X (77092)<img src="./img/plus.png" width="20" height="20" alt="Плюс" /></button></td></tr>
                <tr><td><button>Угл скорость Y (27059)<img src="./img/plus.png" width="20" height="20" alt="Плюс" /></button></td></tr>
                <tr><td><button>Угл скорость Z (20838)<img src="./img/plus.png" width="20" height="20" alt="Плюс" /></button></td></tr>
                <tr><td><button>Угол атаки (19638)<img src="./img/plus.png" width="20" height="20" alt="Плюс" /></button></td></tr>
                <tr><td><button>Угол скольжения (19235)<img src="./img/plus.png" width="20" height="20" alt="Плюс" /></button></td></tr>
                <tr><td><button>Прод перегрузка (18908)<img src="./img/plus.png" width="20" height="20" alt="Плюс" /></button></td></tr>
                <tr><td><button>Скорость севера (94533)<img src="./img/plus.png" width="20" height="20" alt="Плюс" /></button></td></tr>
                <tr><td><button>Скорость ветра (32326)<img src="./img/plus.png" width="20" height="20" alt="Плюс" /></button></td></tr>
                <tr><td><button>Скорость Восток (11036)<img src="./img/plus.png" width="20" height="20" alt="Плюс" /></button></td></tr>
                <tr><td><button>Крен (15196)<img src="./img/plus.png" width="20" height="20" alt="Плюс" /></button></td></tr>
                <tr><td><button>Тангаж (13862)<img src="./img/plus.png" width="20" height="20" alt="Плюс" /></button></td></tr>
                <tr><td><button>Курс ист (13096)<img src="./img/plus.png" width="20" height="20" alt="Плюс" /></button></td></tr>
                <tr><td><button>Курс магн(11019)<img src="./img/plus.png" width="20" height="20" alt="Плюс" /></button></td></tr>
                <tr><td><button>Долгота (34195)<img src="./img/plus.png" width="20" height="20" alt="Плюс" /></button></td></tr>
                <tr><td><button>Широта (53994)<img src="./img/plus.png" width="20" height="20" alt="Плюс" /></button></td></tr>
                <tr><td><button>Угл сокрость X (77092)<img src="./img/plus.png" width="20" height="20" alt="Плюс" /></button></td></tr>
                <tr><td><button>Угл скорость Y (27059)<img src="./img/plus.png" width="20" height="20" alt="Плюс" /></button></td></tr>
                <tr><td><button>Угл скорость Z (20838)<img src="./img/plus.png" width="20" height="20" alt="Плюс" /></button></td></tr>
                <tr><td><button>Угол атаки (19638)<img src="./img/plus.png" width="20" height="20" alt="Плюс" /></button></td></tr>
                <tr><td><button>Угол скольжения (19235)<img src="./img/plus.png" width="20" height="20" alt="Плюс" /></button></td></tr>
                <tr><td><button>Прод перегрузка (18908)<img src="./img/plus.png" width="20" height="20" alt="Плюс" /></button></td></tr> */}
            </tbody>
    </table>
};

export default Table;