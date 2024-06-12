import React, { useEffect, useState, createContext } from "react"
import { getFlightData, getPointsData, getPlanData, getAirports, putPointsData, getAnalytics } from "../../../api-methods/api-methods";
import { handlerAddFlight, handlerAddPoint } from "../../../utils/common";

export const PointContext = createContext();

export const PointsContext = ({children}) => {

    const [points, setPoints] = useState([]);
    const [flights, setFlights] = useState([]);
    const [airports, setAirports] = useState([]);
    const [currentFlight, setCurrentFlight] = useState(null);
    const [analytics, setAnalytics] = useState(null)

    const fetchPoints = async () => {
        await getPointsData(setPoints, currentFlight);
        points.routePoints.sort((a, b) => a < b);

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

    const fetchAnalytics = async () => {
        await getAnalytics(setAnalytics);
    };

    useEffect(() => {
        fetchAnalytics();
        fetchFlights();
        fetchAirports();
        points.sort((a, b) => a.order < b.order);
    }, []);

    const addFlight = (formData, sendingPointData, setSendingPointData) => {
        handlerAddFlight(formData, flights, setFlights, sendingPointData, setSendingPointData, airports);
    };

    const addPoint = async (formData, sendingPointData, setSendingPointData, currentFlight) => {
        await handlerAddPoint(formData, points, setPoints, sendingPointData, setSendingPointData, currentFlight);
    };
    const changePointData = async (data) => {
        await putPointsData(currentFlight, data, setPoints);
    };

    return (
        <PointContext.Provider value={{
            points, 
            airports, 
            currentFlight,
            flights,
            analytics, 
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
