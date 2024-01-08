import React, { useState, useEffect } from 'react';
import styles from './PlanItem.module.css';
import minus from '../../../assets/img/Decrease.png';
import axios from "axios";



const PlanItem = (props) => {

    useEffect(() => {
        return () => {

        };
    }, []);

    const handleClick = async () => {
        // await fetch(`https://localhost:7110/api/launch/stages/${props.index}`, {
        //     method: 'DELETE'
        // })
        //     .then((response) => {
        //         if (response.ok) {
        //             setShowComponent(false);
        //             props.onRemoveData();
        //         } else {
        //             console.error('Failed to delete the plan item with id:', props.id);
        //         }
        //     })
        //     .catch((err) => console.error('Network or server error when attempting to delete plan item:', err))
        
        await axios ({
            method: 'delete',
            url: `https://localhost:7110/api/launch/stages/${props.index}`,
        })
            .then((response) => {
                if (response.status === 200) {
                    props.onRemoveData();
                } else {
                    console.error('Failed to delete the plan item with id:', props.id);
                }
            })
            .catch((err) => console.error('Network or server error when attempting to delete plan item:', err))
    };

    return (
        <>
            <div className={styles.added_flight} id={props.id}>
                <tr className={styles.table_element}>
                    <td className={styles.table_element_item_2}>{props.heading}</td>
                    <td className={styles.table_element_item_3}>{props.speed}</td>
                    <td className={styles.table_element_item_4}>{props.altitude}</td>
                    <td>
                        <button className={styles.minus} type='button' onClick={() => handleClick()}><img src={minus} alt='Decrease' /></button>
                    </td>
                </tr>
            </div>
        </>
    )
}

export default PlanItem;