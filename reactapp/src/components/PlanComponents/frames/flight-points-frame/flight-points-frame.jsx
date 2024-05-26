import React, { useEffect, useState } from 'react';
import { getPointsData } from '../../../../api-methods/api-methods';
import 'bootstrap/dist/css/bootstrap.css';
import PointItem from '../../PointItem/PointItem';


const FlightPoints = () => {
    const [points, setPoint] = useState([]);
    //const flight = '1ab253b4-d038-4073-8451-7bb2ccdc6384'
    /* useEffect(() => { getPointsData(setPoint); }, []);
    console.log(points)


    const onRemoveData = async () => { await getPointsData(setPoint); } */
    useEffect(() => {
        fetchPoints();
      }, []);
    
      const fetchPoints = async () => {
        await getPointsData(setPoint);
      };
    return (
        <>
            <div className={`row`}>
            <div className={``}>
                {points.routePoints && points.routePoints.map((element, index) =>
                    <PointItem
                        key={element.key}
                        index={index}
                        id={element.id}
                        order={element.order}
                        isEditable={element.isEditable}
                        latitude={element.latitude}
                        longitude={element.longitude}
                        remarks={element.remarks}
                        altitude={element.altitude}
                        onRemoveData={fetchPoints}/> 
                )}
            </div>
            {/* <div className={`hidden`}>
                {points.routePoints && points.routePoints.map((element, index) =>
                    <MainMap
                        key={element.key}
                        index={index}
                        id={element.id}
                        order={element.order}
                        isEditable={element.isEditable}
                        latitude={element.latitude}
                        longitude={element.longitude}
                        remarks={element.remarks}
                        altitude={element.altitude}
                        onRemoveData={onRemoveData}/> 
                )}
            </div> */}
            {/* <div className={`hidden`}>
                <MainMap flightPoints={points.routePoints}/>
            </div> */}
        </div>     
        </>
    );    
};

export default FlightPoints;