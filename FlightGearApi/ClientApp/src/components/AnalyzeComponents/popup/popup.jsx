import { useDispatch } from 'react-redux';
import styles from './popup.module.css'
import React, { useEffect, useState } from "react";
import { setSession } from '../../../redux/ChartSlice/ChartSlice';

const PopupLoad = () => {
    const [sessionList, setSessionList] = useState([]);
    console.log(sessionList);
    const dispatch = useDispatch();
    const ROUTE = "https://localhost:7110/api/analytics/sessions/"

    useEffect(() => {
        const fetchData = async() => {
            await fetch(ROUTE)
                .then(data => console.log(data))
                .then(d => d.json())
        }
        const getData = fetchData()
        getData().then((data) => setSessionList(data))
    }, []);

    // Здесь должен получаться json в формате:
    // [
    //     {
    //         name: '*Номер сессии* сессия',
    //         id: "Порядковый номер сессии, цифрой"
    //     }
    // ]

    const onClickHandler = (id) => {
        dispatch(setSession(id))

        document.querySelector('.dark__content').classList.add('hidden')
    };

    return <div className={styles.popup}>
        <h1 className={styles.popup__title}>Выберите сессию</h1>
        {/* <button onClick={() => onClickHandler(1)}>Сессия 1</button>
        <button onClick={() => onClickHandler(2)}>Сессия 2</button> */}
        {sessionList.map((s) => <button onClick={() => onClickHandler(s.id)}>ессия {s.id}</button>)};
        {/* {sessionList} */}
    </div>
};

export default PopupLoad;