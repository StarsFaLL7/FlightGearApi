import React, {useState, useEffect} from "react";
import PlanItem from "../PlanItem/PlanItem"
import styles from '../FlightPlanTable/PlanTable.module.css';
import { getPlanData, getFlightData } from "../../../api-methods/api-methods";
import FlightItem from "../FlightItem/FlightItem";
import { getData } from "../../../utils/common";

const PlanPoints = () => {
  //const [plan, setPlan] = useState([]);
  //const [sendingData, setSendingData] = useState([]);

  const [flight, setFlight] = useState([]);
  //const [curFlight, setCurFlight] = useState([]);

  //useEffect(() => { getPlanData(setPlan); }, []);
  useEffect(() => { getPlanData(setFlight); }, []);
  //useEffect(() => { getFlightData(flight, setFlight); }, []);

  const onRemoveData = async () => { await getPlanData(setFlight); }

  return (
    <>
        <div className={`row`}>
            {/* <table>
              <thead className={`container rounded-4`}>
                <div className={`rounded-4 ${styles.table_head}`}>
                  <h2 className={`display-6 text-center ${styles.title_main}`}>Flight stages</h2>
                  <div className={`container bg-light rounded-4 ${styles.table_head_info}`}>
                    <tr className={`align-items-center ${styles.table_element}`}>
                      <th className={`fs-3 `}>Title</th>
                    </tr>
                  </div>
                </div>
              </thead>
              <tbody> */}
                <div className={styles.scroll}>
                  {flight.flightPlans && flight.flightPlans.map((element, index) =>
                      <FlightItem
                      key={element.key}
                      index={index}
                      id={element.id}
                      title={element.title}
                      remarks={element.remarks}
                      departureRunwayId={element.departureRunwayId}
                      arrivalRunwayId={element.arrivalRunwayId}
                      onRemoveData={onRemoveData}
                    />   
                    )}
                </div>
       {/*        </tbody>
            </table> */}
        </div>     
    </>
  ) 
}

export default PlanPoints;