import React, {useState, useEffect} from "react";
import plus from '../../../assets/img/Union.png';
import styles from './PlanTable.module.css';
import { getPlanData } from "../../../api-methods/api-methods";
import { handlerAddPlan, handlerAddFlight, getData } from "../../../utils/common";

const FlightsPlanTable = () => {
  //const [plan, setPlan] = useState([]);
  //const [sendingData, setSendingData] = useState([]);

  const [flight, setFlight] = useState([]);
  const [sendingFlightData, setSendingFlightData] = useState([]);

  //useEffect(() => { getPlanData(setPlan); }, []);
  //useEffect(() => { getPlanData(setFlight); }, []);
  //const onRemoveData = async () => { await getPlanData(setPlan); }

  const handleClickAddFlight = (evt) => {
    evt.preventDefault();
    const formData = getData(document.getElementById('formFlight'));
    let item = handlerAddFlight(formData, flight, setFlight, sendingFlightData, setSendingFlightData);
    //console.log(item)
    return item;
  };

  return (
    <>
      {/* <main className={`container`}>
        <div className={`row ${styles.cols}`}>
          <div className={`col ${styles.cols}`}>
            <div className={`shadow-lg bg-light rounded-4`}>
              <form id="formFlight" className={`bg-light rounded-4 ${styles.add_stage}`}>
                <h1 className={`display-5 text-center ${styles.title_main}`}>Add new flight</h1>
                <ul className={`list-unstyled`}>
                  <li className={`d-flex align-items-center px-2`}>
                    <p className={`fs-3`}>Flight name:</p>
                    <input className={`form-control ms-auto`} type="text" name="title" required />
                  </li>
                  <li className={`d-flex align-items-center px-2`}>
                    <p className={`fs-3`}>Remarks:</p>
                    <textarea className={`form-control ms-auto`} type="text" name="remarks" required />
                  </li>
                  <li className={`d-flex align-items-center px-2`}>
                    <input className={`form-control ms-auto`} defaultValue={null} name="departureRunwayId" hidden />
                  </li>
                  <li className={`d-flex align-items-center px-2`}>
                    <input className={`form-control ms-auto`} defaultValue={null} name="arrivalRunwayId" hidden />
                  </li>
                  <li className={`d-flex align-items-center justify-content-center`}>
                    <button className={`btn btn-primary btn-hover`} onClick={(evt) => handleClickAddFlight(evt)} type="submit">
                      <img src={plus} alt="Union" />
                    </button>
                  </li>
                </ul>
              </form>
            </div>
          </div>
        </div>
      </main> */}
    </>
  );
}

export default FlightsPlanTable;