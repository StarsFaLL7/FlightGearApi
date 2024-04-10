import React, {useState, useEffect} from "react";
import PlanItem from "../PlanItem/PlanItem";
import MainMap from "../Map/Map";
import plus from '../../../assets/img/Union.png';
import styles from './PlanTable.module.css';
import { getPlanData } from "../../../api-methods/api-methods";
import { handlerAddPlan } from "../../../utils/common";
import { getData } from "../../../utils/common";
import maplibregl from 'maplibre-gl';
import 'maplibre-gl/dist/maplibre-gl.css';
import FlightItem from "../PlanItem/FlightItem";

//className={styles.main_info}
//className={styles.plan_table}
//className={`${styles.title}`}
//${styles.speed_label}
//${styles.altitude_label} 
//className={styles.course}
//className={styles.speed}
//className={styles.altitude}
//${styles.table_element_info_2}
const MainApp = () => {
  const [plan, setPlan] = useState([]);
  const [sendingData, setSendingData] = useState([]);
  const [flight, setFlight] = useState([]);

  useEffect(() => { getPlanData(setPlan); }, []);
  
  const onRemoveData = async () => { await getPlanData(setPlan); }

  const handleClickAddFlight = (evt) => {
    evt.preventDefault();
    const formData = getData(document.getElementById('formFlight'));
    
  };

  const handleClickAddPoint = (evt) => {
    evt.preventDefault();
    const formData = getData(document.getElementById('form'));
    handlerAddPlan(formData, plan, setPlan, sendingData, setSendingData);
  };
  return (
    <>
      <main className={`container-fluid`}>
        <div className={`row`}>
          <h1 className={`display-3 text-center ${styles.title_main}`}>Make your own flight simulation</h1>
          <div className={`col`}>
            <div>
              <form className={`bg-light rounded-4 ${styles.add_stage}`} >
                <h1 className={`display-5 text-center ${styles.title_main}`}>Add new flight</h1>
                <ul className={`list-unstyled`}>
                  <li className={`d-flex align-items-center px-2`}>
                    <p className={`fs-3`}>Flight name:</p>
                    <input className={`form-control ms-auto`} type="text" name="flight_name" required/>
                  </li>
                  <li>
                    <button class="btn btn-primary btn-hover" onClick={(evt) => handleClickAddFlight(evt)} type="submit"><img src={plus} alt="Union"/></button>
                  </li>
                </ul> 
              </form>
            </div>
            <div>
              <form className={`bg-light rounded-4 ${styles.add_stage}`} id="form" method='POST' enctype="application/json">
                <ul className={`list-unstyled`}>
                  <li className={`d-flex align-items-center px-2`}>
                    <p className={`fs-3`}>Course:</p>
                    <input className={`form-control ms-auto`} type="text" name="heading" required/>
                  </li>
                  <li className={`d-flex align-items-center px-2`}>
                    <p className={`fs-3`}>Speed(m/s):</p>
                    <input className={`form-control ms-auto`} type="number" step="0.01" name="speed" required/>
                  </li> 
                  <li className={`d-flex align-items-center px-2`}>
                    <p className={`fs-3`}>Altitude(m):</p>
                    <input className={`form-control ms-auto`} type="number" name="altitude" required/>
                  </li>
                  <li className={`d-flex align-items-center px-2`}>
                    <p className={`fs-3`}>Airport:</p>
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
                  <li className={styles.add_stage_element_4}>
                    <button class="btn btn-primary btn-hover" onClick={(evt) => handleClickAddPoint(evt)} type="submit"><img src={plus} alt="Union"/></button>
                  </li>
                </ul> 
              </form>
            </div>
          </div>
          <div className={`col`}>
            <MainMap/>
          </div>
          <div className={`col`}>
            <table>
              <thead className={`container rounded-4`}>
                <div className={`rounded-4 ${styles.table_head}`}>
                  <h2 className={`display-6 text-center ${styles.title_main}`}>Выбранные этапы полёта</h2>
                  <div className={`container bg-light rounded-4 ${styles.table_head_info}`}>
                    <tr className={`align-items-center ${styles.table_element}`}>
                      <th className={`fs-3 `}>Курс</th>
                      <th className={`fs-3 `}>Расчётная скорость</th>
                      <th className={`fs-3 `}>Высота над уровнем моря</th>
                    </tr>
                  </div>
                </div>
              </thead>
              <tbody>
                <div className={styles.scroll}>
                  
                    {plan && plan.map((element, index) =>
                      <PlanItem
                        //key={element.id}
                        //id={element.id}
                        //flight_name={element.flight_name}
                        index={index}
                        heading={element.heading}
                        speed={element.speed}
                        altitude={element.altitude}
                        onRemoveData={onRemoveData}
                      />
                    )}
                     
                </div>
              </tbody>
            </table>
          </div>
        </div>
      </main>     
    </>
  ) 
}

export default MainApp;