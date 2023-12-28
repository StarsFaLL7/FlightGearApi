import styles from './changeSession.module.css';
import React from 'react';

const ChangeSession = () => {
    const onClickHandler = () => {
        document.querySelector('.dark__content').classList.remove('hidden');
    };

    return <button className={styles.change__btn} onClick={() => onClickHandler()}>Сменить сессию</button>
};

export default ChangeSession;