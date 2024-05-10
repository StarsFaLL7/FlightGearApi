import React, { useEffect } from 'react';
import styles from '../PlanItem/PlanItem';
import minus from '../../../assets/img/Decrease.png';
import { handleClickDeleteItem } from '../../../api-methods/api-methods';
import "../../../../node_modules/bootstrap/dist/css/bootstrap.css"
import PlanItem from '../PlanItem/PlanItem';

const FlightItem = (props) => {

    useEffect(() => { return () => { }; }, []);

    return (
        <>
            <div className={styles.added_flight} id={props.id}>
                <tr className={styles.table_element}>
                    <td className={styles.table_element_item_2}>{props.Title}</td>
                    <td>
                        <button className={styles.minus} type='button' onClick={() => handleClickDeleteItem(props)}><img src={minus} alt='Decrease' /></button>
                    </td>
                </tr>
            </div>
        </>
    )
}

export default FlightItem;