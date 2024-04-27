import { useDispatch } from 'react-redux';
import styles from './popup.module.css';
import minus from '../../../assets/img/Decrease.png';
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

        document.querySelector('.dark__content').classList.add('hidden');
    };

    const onClickHandlerClose = () => {
        document.querySelector('.dark__content').classList.add('hidden');
    }

    return <div className={`${styles.popup}`}>
        <div className={`d-flex`}>
            <div className={``}><h1 className={styles.popup__title}>Выберите сессию</h1></div>
            <div className={`ms-auto`}><button className={styles.minus} type='button' onClick={() => onClickHandlerClose()}><img src={minus} alt='Decrease'></img></button></div>
        </div>
        <div className={`${styles.sessionList}`}>
            {sessionList.map((s) => <button key={s.id} onClick={() => onClickHandler(s.id)}>Сессия {s.title} ({s.id})</button>)}
        </div>
    </div>
};

export default PopupLoad;