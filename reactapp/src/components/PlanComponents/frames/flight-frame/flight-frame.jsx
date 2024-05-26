import React, {useState, useEffect} from "react";
import { getPlanData, getFlightData } from "../../../../api-methods/api-methods";
import FlightItem from "../../FlightItem/FlightItem";

const PlanPoints = () => {

  const [flight, setFlight] = useState([]);
  const [openFlightId, setOpenFlightId] = useState(null);

  useEffect(() => { getPlanData(setFlight); }, []);
  const handleFormToggle = (id) => {
    setOpenFlightId(prevId => (prevId === id ? null : id));
  };

  const onRemoveData = async () => { await getPlanData(setFlight); }
  return (
    <>
      <div className={`row`}>
        <div className={`scrollspy-example`}>
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
              openFlightId={openFlightId}
              handleFormToggle={handleFormToggle}/>   
          )}
        </div>
        {/* <div className={`hidden`}>
          <MainMap flight={flight.flightPlans}/>
        </div> */}
      </div>     
    </>
  ) 
}

export default PlanPoints;