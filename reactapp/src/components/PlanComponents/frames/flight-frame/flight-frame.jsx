import React, { useEffect, useContext } from "react";
import FlightItem from "../../FlightItem/FlightItem";
import { PointContext } from "../../context/main-context";

const PlanPoints = () => {

  const { flights, fetchFlights } = useContext(PointContext);

  useEffect(() => { fetchFlights(); },[]);

  const onRemoveData = async () => { await fetchFlights(); }
  
  return (
    <>
      <div className={`row`}>
        <div className={`scrollspy-example`}>
          {flights.flightPlans && flights.flightPlans.map((element, index) =>
            <FlightItem
              key={element.id}
              index={index}
              id={element.id}
              title={element.title}
              remarks={element.remarks}
              routePoints={element.routePoints}
              departureRunwayId={element.departureRunwayId}
              arrivalRunwayId={element.arrivalRunwayId}
              onRemoveData={onRemoveData}
              />   
          )}
        </div>
      </div>     
    </>
  ) 
}

export default PlanPoints;