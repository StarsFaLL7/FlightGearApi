import React, { useEffect, useState, createContext } from "react"
import { getFlightData, getPointsData, getPlanData, getAirports, putPointsData } from "../../../api-methods/api-methods";
import { handlerAddFlight, handlerAddPoint } from "../../../utils/common";

export const PointContext = createContext();

export const PointsContext = ({children}) => {

    const [points, setPoints] = useState([]);
    const [flights, setFlights] = useState([]);
    const [airports, setAirports] = useState([]);
    const [currentFlight, setCurrentFlight] = useState(null);

    const fetchPoints = async () => {
        //console.log(currentFlight)
        await getPointsData(setPoints, currentFlight);
    };

    const fetchFlights = async () => {
        await getPlanData(setFlights);
    };

    const fetchAirports = async () => {
        await getAirports(setAirports);
    };

    const fetchCurrentFlight = async (id) => {
        //console.log(id)
        await getFlightData(id, setCurrentFlight);
    };

    const getCurrentFlightById = async (id) => {
        await fetchCurrentFlight(id);
        //setCurrentFlight(id);
    };

    useEffect(() => {
        fetchFlights();
        fetchAirports();
    }, []);

    const addFlight = (formData, sendingPointData, setSendingPointData) => {
        handlerAddFlight(formData, flights, setFlights, sendingPointData, setSendingPointData, airports);
    };

    const addPoint = async (formData, sendingPointData, setSendingPointData, currentFlight) => {
        await handlerAddPoint(formData, points, setPoints, sendingPointData, setSendingPointData, currentFlight);
    };
    const changePointData = async(data) => {
        await putPointsData(currentFlight, data, setPoints);
    };

    return (
        <PointContext.Provider value={{
            points, 
            airports, 
            currentFlight,
            flights, 
            setCurrentFlight, 
            addPoint, 
            fetchPoints, 
            addFlight,
            getCurrentFlightById,
            fetchFlights,
            fetchAirports,
            setPoints,
            changePointData,
        }}>
            {children}
        </PointContext.Provider>
    );
};
