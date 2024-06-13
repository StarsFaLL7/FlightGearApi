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
  const isAirportPoint = props.isEditable;
  const isAirportPointColor = props.isEditable ? 'bg-not-light' : 'bg-airport-point';
  return (
    <div className={`point-item d-flex`} id={props.id}>
    <div className={`${isAirportPointColor} rounded-3 m-1 d-flex ${styles.table_element} align-items-center justify-content-between`}>
      <div className={`align-items-center`}>{props.order}</div>
    </div>
    <div className={`${isAirportPointColor} w-100 rounded-3 m-1 d-flex ${styles.table_element} align-items-center`}>
      <div className={`longitude text-center`}>{props.longitude}</div>
      <div className={`latitude text-center`}>{props.latitude}</div>
      <div className={`altitude text-center`}>{props.altitude}</div>
      {isAirportPoint && <div><button className='btn' type='button' onClick={handleDelete}>
        <img src={minus} alt='Decrease' />
      </button></div>}
    </div>
    </div>
  );
};

export default PointItem;

