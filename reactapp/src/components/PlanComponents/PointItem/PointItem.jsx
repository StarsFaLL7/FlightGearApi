import React, { useEffect, useContext } from 'react';
import styles from './PointItem.css';
import minus from '../../../assets/img/Decrease.png';
import { handleClickDeletePoint } from '../../../api-methods/api-methods';
import "../../../../node_modules/bootstrap/dist/css/bootstrap.css"
import NavHeader from '../NavItem/NavItem';
import { PointContext } from '../context/main-context';


const PointItem = (props) => {
    //console.log(props)
    //const flight
    //useEffect(() => { return () => { }; }, []);
    const {fetchPoints, currentFlight} = useContext(PointContext);
    const handleDelete = async () => {
        await handleClickDeletePoint(currentFlight, props);
        fetchPoints();
    };

    return (
        <>
            <div className={`bg-not-light rounded-3 mb-1`} id={props.id}>
                <tr className={styles.table_element}>
                    <td className={styles.table_element_item_2}>{props.longitude}</td>
                    <td className={styles.table_element_item_2}>{props.latitude}</td>
                    <td className={styles.table_element_item_2}>{props.title}</td>
                    <td className={styles.table_element_item_4}>{props.altitude}</td>
                    <td>
                        <button className='btn' type='button' onClick={handleDelete}>
                            <img src={minus} alt='Decrease' />
                        </button>
                    </td>
                </tr>
            </div>
        </>
    )
}

export default PointItem;