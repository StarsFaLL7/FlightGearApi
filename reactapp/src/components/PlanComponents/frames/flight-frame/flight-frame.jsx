import React, {useState, useEffect, useContext} from "react";
import { getPlanData, getFlightData } from "../../../../api-methods/api-methods";
import FlightItem from "../../FlightItem/FlightItem";
import { PointContext } from "../../context/main-context";

const PlanPoints = () => {

  //const [flights, setFlights] = useState({});
  //const [openFlightId, setOpenFlightId] = useState(null);
  const {flights, setFlights, setCurrentFlight, currentFlight, fetchFlights} = useContext(PointContext);

  useEffect(() => { fetchFlights(); },[]);
  /* const handleFormToggle = (id) => {
    setOpenFlightId(prevId => (prevId === id ? null : id));
    console.log(id)
  }; */

  const onRemoveData = async () => { await fetchFlights(); }
  return (
    <>
      <div className={`row`}>
        <div className={`scrollspy-example`}>
          {flights.flightPlans && flights.flightPlans.map((element, index) =>
            <FlightItem
              key={element.key}
              index={index}
              id={element.id}
              title={element.title}
              remarks={element.remarks}
              routePoints={element.routePoints}
              departureRunwayId={element.departureRunwayId}
              arrivalRunwayId={element.arrivalRunwayId}
              onRemoveData={onRemoveData}
              //openFlightId={openFlightId}
              //handleFormToggle={handleFormToggle}
              />   
          )}
        </div>
      </div>     
    </>
  ) 
}

export default PlanPoints;