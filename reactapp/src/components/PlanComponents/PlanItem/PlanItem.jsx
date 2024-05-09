import React, { useEffect } from 'react';
import styles from './PlanItem.module.css';
import minus from '../../../assets/img/Decrease.png';
import { handleClickDeleteItem } from '../../../api-methods/api-methods';
import "../../../../node_modules/bootstrap/dist/css/bootstrap.css"

const PlanItem = (props) => {

    useEffect(() => { return () => { }; }, []);

    return (
        <>
            <div className={styles.added_flight} id={props.id}>
                <tr className={styles.table_element}>
                    <td className={styles.table_element_item_2}>{props.longitude}</td>
                    <td className={styles.table_element_item_2}>{props.latitude}</td>
                    {/* <td className={styles.table_element_item_2}>{props.heading}</td> */}
                    <td className={styles.table_element_item_3}>{props.speed}</td>
                    <td className={styles.table_element_item_4}>{props.altitude}</td>
                    <td>
                        <button className={styles.minus} type='button' onClick={() => handleClickDeleteItem(props)}><img src={minus} alt='Decrease' /></button>
                    </td>
                </tr>
            </div>
        </>
    )
}

export default PlanItem;