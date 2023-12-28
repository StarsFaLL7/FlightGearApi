import React, { useState, useEffect } from 'react';
import styles from './PlanItem.module.css';
import minus from '../../../assets/img/Decrease.png';



const PlanItem = (props) => {
    const [showComponent, setShowComponent] = useState(true);

    useEffect(() => {
        return () => {

        };
    }, []);

    const handleClick = async () => {
        await fetch(`https://localhost:7110/api/launch/stages/${props.index}`, {
            method: 'DELETE'
        })
            .then((response) => {
                if (response.ok) {
                    setShowComponent(false);
                    props.onRemoveData();
                } else {
                    console.error('Failed to delete the plan item with id:', props.id);
                }
            })
            .catch((err) => console.error('Network or server error when attempting to delete plan item:', err))
    };

    return (
        <>
        {   showComponent &&
            <div className={styles.added_flight} id={props.id}>
                <tr className={styles.table_element}>
                    <td className={styles.table_element_item_2}>{props.heading}</td>
                    <td className={styles.table_element_item_3}>{props.speed}</td>
                    <td className={styles.table_element_item_4}>{props.altitude}</td>
                    <td>
                        {showComponent && <button className={styles.minus} type='button' onClick={() => handleClick()}><img src={minus} alt='Decrease' /></button>}
                    </td>
                </tr>
            </div>
        }
        </>
    )
}

export default PlanItem;