import React, {useState, useEffect} from "react";
import PlanItem from "../PlanItem/PlanItem"
import styles from '../PlanTable/PlanTable.module.css';
import { getPlanData } from "../../../api-methods/api-methods";

const PlanPoints = () => {
  const [plan, setPlan] = useState([]);
  const [sendingData, setSendingData] = useState([]);

  useEffect(() => { getPlanData(setPlan); }, []);
  const onRemoveData = async () => { await getPlanData(setPlan); }

  return (
    <>
        <div className={`row`}>
            <table>
              <thead className={`container rounded-4`}>
                <div className={`rounded-4 ${styles.table_head}`}>
                  <h2 className={`display-6 text-center ${styles.title_main}`}>Flight stages</h2>
                  <div className={`container bg-light rounded-4 ${styles.table_head_info}`}>
                    <tr className={`align-items-center ${styles.table_element}`}>
                      <th className={`fs-3 `}>Longitude</th>
                      <th className={`fs-3 `}>Latitude</th>
                      <th className={`fs-3 `}>Speed(m/s)</th>
                      <th className={`fs-3 `}>Altitude(m)</th>
                    </tr>
                  </div>
                </div>
              </thead>
              <tbody>
              {/* <div className={styles.scroll}>
                <FlightItem
                  onRemoveData={onRemoveData}
                /> */}
                <div className={styles.scroll}>
                  
                    {plan && plan.map((element, index) =>
                      <PlanItem
                        //key={element.id}
                        //id={element.id}
                        //flight_name={element.flight_name}
                        index={index}
                        longitude={element.longitude}
                        latitude={element.latitude}
                        //heading={element.heading}
                        speed={element.speed}
                        altitude={element.altitude}
                        onRemoveData={onRemoveData}
                      />
                    )}
                     
                </div>
              {/* </div> */}
              </tbody>
            </table>
        </div>     
    </>
  ) 
}

export default PlanPoints;