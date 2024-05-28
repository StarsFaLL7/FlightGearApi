import React, { useEffect, useState } from 'react';
import { getPointsData } from '../../../../api-methods/api-methods';
import 'bootstrap/dist/css/bootstrap.css';
import PointItem from '../../PointItem/PointItem';

const FlightPoints = () => {
    const [points, setPoints] = useState({});

    const fetchPoints = async () => {
        await getPointsData(setPoints);
    };

    //const onAddPoint = async () => { await getPointsData(setPoints); }

    useEffect(() => {
        fetchPoints();
    }, []);


    return (
        <>
            <div>
                {points.routePoints && points.routePoints.map((element, index) => (
                    <PointItem
                        key={element.id}
                        index={index}
                        id={element.id}
                        order={element.order}
                        isEditable={element.isEditable}
                        latitude={element.latitude}
                        longitude={element.longitude}
                        remarks={element.remarks}
                        altitude={element.altitude}
                        onRemoveData={fetchPoints}
                    />
                ))}
            </div>
        </>
    );
};

export default FlightPoints;