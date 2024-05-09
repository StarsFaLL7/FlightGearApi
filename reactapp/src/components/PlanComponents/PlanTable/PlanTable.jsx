import React, {useState, useEffect} from "react";
import plus from '../../../assets/img/Union.png';
import styles from './PlanTable.module.css';
import { getPlanData } from "../../../api-methods/api-methods";
import { handlerAddPlan } from "../../../utils/common";
import { getData } from "../../../utils/common";

const MainApp = () => {
  const [plan, setPlan] = useState([]);
  const [sendingData, setSendingData] = useState([]);

  useEffect(() => { getPlanData(setPlan); }, []);
  const onRemoveData = async () => { await getPlanData(setPlan); }

  /* const handleClickAddFlight = (evt) => {
    evt.preventDefault();
    const formData = getData(document.getElementById('formFlight'));
    handlerAddPlan(formData, plan, setPlan, sendingData, setSendingData);
  }; */

  const handleClickAddPoint = (evt) => {
    console.log(evt)
    evt.preventDefault();
    const formData = getData(document.getElementById('form'));
    handlerAddPlan(formData, plan, setPlan, sendingData, setSendingData);
  };
  return (
    <>
      <main className={`container`}>
        <div className={`row ${styles.cols}`}>
          <div className={`col ${styles.cols}`}>
            {/* <div className={`shadow-lg bg-light rounded-4`}>
              <form className={`bg-light rounded-4 ${styles.add_stage}`} >
                <h1 className={`display-5 text-center ${styles.title_main}`}>Add new flight</h1>
                <ul className={`list-unstyled`}>
                  <li className={`d-flex align-items-center px-2`}>
                    <p className={`fs-3`}>Flight name:</p>
                    <input className={`form-control ms-auto`} type="text" name="flight_name" required/>
                  </li>
                  <li className={`d-flex align-items-center px-2`}>
                    <p className={`fs-3`}>Start airport:</p>
                    <input className={`form-control ms-auto `} type="text" name="" list="airport-list" required/>
                    <datalist className={`${styles.datalist}`} id="airport-list">
                      <option value="Amsterdam"></option>
                      <option value="Geneva"></option>
                      <option value="Chamonix"></option>
                      <option value="Amsterdam"></option>
                      <option value="Geneva"></option>
                      <option value="Chamonix"></option>
                      <option value="Amsterdam"></option>
                      <option value="Geneva"></option>
                      <option value="Chamonix"></option>
                    </datalist>
                  </li>
                  <li>
                    <button class="btn btn-primary btn-hover" onClick={(evt) => handleClickAddFlight(evt)} type="submit"><img src={plus} alt="Union"/></button>
                  </li>
                </ul> 
              </form>
            </div> */}
            <div className={`shadow-lg bg-light rounded-4`}>
              <form className={`bg-light rounded-4 ${styles.add_stage}`} id="form" method='POST' enctype="application/json">
                <h1 className={`display-5 text-center ${styles.title_main}`}>Add new point</h1>
                <ul className={`list-unstyled`}>
                  <li className={`d-flex align-items-center px-2`}>
                    <p className={`fs-3`}>Longitude:</p>
                    <input className={`form-control ms-auto`} type="number" name="longitude"  required/>
                  </li>
                  <li className={`d-flex align-items-center px-2`}>
                    <p className={`fs-3`}>Latitude:</p>
                    <input className={`form-control ms-auto`} type="number" name="latitude" required/>
                  </li>
                  {/* <li className={`d-flex align-items-center px-2`}>
                    <p className={`fs-3`}>Course:</p>
                    <input className={`form-control ms-auto`} type="number" name="heading" required/>
                  </li> */}
                  <li className={`d-flex align-items-center px-2`}>
                    <p className={`fs-3`}>Speed(m/s):</p>
                    <input className={`form-control ms-auto`} type="number" step="0.01" name="speed" required/>
                  </li> 
                  <li className={`d-flex align-items-center px-2`}>
                    <p className={`fs-3`}>Altitude(m):</p>
                    <input className={`form-control ms-auto`} type="number" name="altitude" required/>
                  </li>
                  <li className={styles.add_stage_element_4}>
                    <button class="btn btn-primary btn-hover" onClick={(evt) => handleClickAddPoint(evt)} type="submit"><img src={plus} alt="Union"/></button>
                  </li>
                </ul> 
              </form>
            </div>
          </div>
        </div>
      </main>     
    </>
  ) 
}

export default MainApp;