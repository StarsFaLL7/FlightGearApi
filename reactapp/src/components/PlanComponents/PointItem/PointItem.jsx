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
  const isAirportPointColor = props.isEditable ? 'bg-not-light' : 'bg-airport-point';
  return (
    <div className={`${isAirportPointColor} rounded-3 mb-1 d-flex ${styles.table_element} align-items-center justify-content-between`} id={props.id}>
      <div className={``}>{props.longitude}</div>
      <div className={``}>{props.latitude}</div>
      <div className={``}>{props.title}</div>
      <div className={``}>{props.altitude}</div>
      <div>
        <button className='btn' type='button' onClick={handleDelete}>
          <img src={minus} alt='Decrease' />
        </button>
      </div>
    </div>
  );
};

export default PointItem;