/* import React, { useState } from "react";
import plus from '../../../assets/img/Union.png';
import styles from '../FlightPlanTable/PlanTable.module.css';
import { getPlanData } from "../../../api-methods/api-methods";
import { handlerAddFlight, getData } from "../../../utils/common";
import FlightPoint from "../flight-points/flight-points";

const CurrentFlight = ({ onChooseCurrentFlight }) => {
  const [flight, setFlight] = useState([]);
  const [sendingFlightData, setSendingFlightData] = useState([]);
  const [isFormVisible, setIsFormVisible] = useState(false);

  const handleCreateNewFlightForm = (evt) => {
    evt.preventDefault();
    setIsFormVisible(!isFormVisible);
  };

  const handleClickAddFlight = (evt) => {
    evt.preventDefault();
    const formData = getData(document.getElementById('formFlight'));
    handlerAddFlight(formData, flight, setFlight, sendingFlightData, setSendingFlightData);
  };
  //${isFormVisible ? '' : 'hidden'}
  function getForm() {
    console.log(isFormVisible)
    return isFormVisible ? (<form className={``} id="formFlight">
    <ul className="list-unstyled m-0">
      <li className="d-flex align-items-center">
        <p className="fs-5">Flight name:</p>
        <input className="form-control ms-auto" type="text" name="title" required />
      </li>
      <li className="d-flex align-items-center">
        <p className="fs-5">Remarks:</p>
        <textarea className="form-control ms-auto" type="text" name="remarks" required />
      </li>
      <li className="d-inline points m-1">
        <p className="border-bottom border-secondary m-0 mx-auto align-self-center">Points:</p>
        <div className="point-container form-control m-1 w-auto">
          <FlightPoint />
        </div>
      </li>
      <li className="d-flex align-items-center">
        <input className="form-control ms-auto" defaultValue={null} name="departureRunwayId" hidden />
      </li>
      <li className="d-flex align-items-center">
        <input className="form-control ms-auto" defaultValue={null} name="arrivalRunwayId" hidden />
      </li>
      <li className="mt-3 d-flex align-items-center justify-content-center">
        <button className="btn btn-primary btn-hover" onClick={handleClickAddFlight} type="submit">
          <img src={plus} alt="Union" />
        </button>
      </li>
    </ul>
  </form>) : ('');
  }

  return (
    <main className="container">
      <div className={`row ${styles.cols}`}>
        <div className="d-flex justify-content-center">
          <button className="btn bg-primary me-auto" onClick={onChooseCurrentFlight}>Choose current flight</button>
          <button className="btn bg-primary" onClick={handleCreateNewFlightForm}>Create New</button>
        </div>
        {getForm()}
      </div>
    </main>
  );
}

export default CurrentFlight; */