import { useDispatch } from 'react-redux';
import styles from './popup.module.css'
import React, { useEffect, useState } from "react";
import { setSession } from '../../../redux/ChartSlice/ChartSlice';
import axios from "axios";

const PopupLoad = () => {
    const [sessionList, setSessionList] = useState([]);
    console.log(sessionList);
    const dispatch = useDispatch();
    const ROUTE = "https://localhost:7110/api/analytics/sessions/"

    
    useEffect(() => {
        async function fetchData() {
            // You can await here
            const result =  await axios(ROUTE);
            console.log("Result.Data = ", result.data);
            setSessionList(result.data);
        }
        fetchData();
    }, []);

    const onClickHandler = (id) => {
        dispatch(setSession(id))

        document.querySelector('.dark__content').classList.add('hidden')
    };

    return <div className={styles.popup}>
        <h1 className={styles.popup__title}>Выберите сессию</h1>
        {/* <button onClick={() => onClickHandler(1)}>Сессия 1</button>
        <button onClick={() => onClickHandler(2)}>Сессия 2</button> */}
        {sessionList.map((s) => <button key={s.id} onClick={() => onClickHandler(s.id)}>Сессия {s.id}</button>)};
        {/* {sessionList} */}
    </div>
};

export default PopupLoad;