import React, { useEffect, useContext } from 'react';
import styles from './PointItem.css';
import minus from '../../../assets/img/Decrease.png';
import { handleClickDeletePoint } from '../../../api-methods/api-methods';
import "../../../../node_modules/bootstrap/dist/css/bootstrap.css";
import { PointContext } from '../context/main-context';

const PointItem = (props) => {
  const { fetchPoints, currentFlight } = useContext(PointContext);

  const handleDelete = async () => {
    await handleClickDeletePoint(currentFlight, props);
    fetchPoints();
  };

  return (
    <div className={`bg-not-light rounded-3 mb-1 d-flex ${styles.table_element} align-items-center justify-content-between`} id={props.id}>
      <div className={``}>{props.longitude}</div>
      <div className={styles.table_element_item_2}>{props.latitude}</div>
      <div className={styles.table_element_item_2}>{props.title}</div>
      <div className={styles.table_element_item_4}>{props.altitude}</div>
      <div>
        <button className='btn' type='button' onClick={handleDelete}>
          <img src={minus} alt='Decrease' />
        </button>
      </div>
    </div>
  );
};

export default PointItem;