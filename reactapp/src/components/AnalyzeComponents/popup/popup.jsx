import { useDispatch } from 'react-redux';
import styles from './popup.module.css';
import minus from '../../../assets/img/Decrease.png';
import React, { useEffect, useState } from "react";
import { setSession } from '../../../redux/ChartSlice/ChartSlice';
import axios from "axios";
import rubishBin from '../imgs/rubbish-bin.png';

const PopupLoad = () => {
    const [sessionList, setSessionList] = useState([]);
    console.log(sessionList);
    const dispatch = useDispatch();
    const ROUTE = "https://localhost:7229/api/analytics/sessions/"

    
    useEffect(() => {
        fetchData();
    }, []);

    async function fetchData() {
        const result =  await axios(ROUTE);
        console.log("Result.Data = ", result.data);
        setSessionList(result.data.sessions);
    }

    const onClickHandler = (id) => {
        dispatch(setSession(id))

        document.querySelector('.dark__content').classList.add('hidden');
    };

    const onClickHandlerClose = () => {
        document.querySelector('.dark__content').classList.add('hidden');
    }
    const onClickHandlerDelete = async (id, e) => {
        e.stopPropagation();
        await axios.delete(`${ROUTE}${id}`);
        fetchData();
    }

    return <div className={`${styles.popup}`}>
        <div className={`d-flex`}>
            <div className={``}><h1 className={styles.popup__title}>Выберите сессию</h1></div>
            <div className={`ms-auto`}><button className={styles.minus} type='button' onClick={() => onClickHandlerClose()}><img src={minus} alt='Decrease'></img></button></div>
        </div>
        <div className={`${styles.sessionList}`}>
            {sessionList.map((s) => 
                <button className={`d-flex w-auto align-items-center`} key={s.id} onClick={() => onClickHandler(s.id)}>Сессия {s.title}
                    <div className={`w-auto h-auto ms-auto `}>
                        <button className={`m-0 d-flex w-auto ms-auto btn btn-danger align-items-center`} type='button' onClick={(e) => onClickHandlerDelete(s.id, e)}>
                            <img className={`${styles.rubish_bin} ms-auto`} src={rubishBin} alt="delete" />
                        </button>
                    </div>
                </button>)}
        </div>
    </div>
};

export default PopupLoad;