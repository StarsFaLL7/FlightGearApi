import React from "react";
import styles from './refreshButton.module.css';
import { useDispatch, useSelector } from "react-redux";
import { setReload } from '../../../redux/ChartSlice/ChartSlice';
import strelka from '../../../assets/img/strelka.png';

const RefreshButtonComponent = () => {
    const isReloading = useSelector((state)=>state.chart.isReloading)
    const dispatch = useDispatch();

    function debounce (cb, delay) {
        let timeout;
        return () => {
            const context = this;
            const args = arguments;
            clearTimeout(timeout);
            timeout = setTimeout(() => cb.apply(context, args), delay);
        };
    };

    const onClickHandler = () => isReloading===1 ? dispatch(setReload(0)) : dispatch(setReload(1));
    const debouncedOnClickHandler = debounce(onClickHandler, 250);

    return <button className={styles.reload__btn} onClick={() => debouncedOnClickHandler()}><img src={strelka} alt="123" width={30} height={30} /></button>
};

export default RefreshButtonComponent;
