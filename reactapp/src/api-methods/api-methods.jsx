import axios from 'axios';
import { SERVER_URL, ALL_FLIGHTS_URL, AIRPORTS_URL, START_FLIGHT, ANALYTICS, STATUS, EXIT_FLIGHT } from '../const/const';

export const startFlight = async (flight) => {
  flight = {title: flight.title, flightPlanId: flight.id, readsPerSecond: 10}
  console.log(flight)
  try {
    const response = await axios.post(`${START_FLIGHT}`, flight);
    return response;
  } catch (err) {
    console.error('There was an error updating the flight data:', err);
  }
};

export const endFlight = async (setStatus, setStartButtonText) => {
  try {
    const response = await axios.post(`${EXIT_FLIGHT}`);
    setStatus(response.data)
    setStartButtonText('Start')
  } catch (err) {
    console.error('There is no any started session:', err);
  }
};

export const getFlightStatus = async (setStatus) => {
  try{
    const response = await axios.get(STATUS);
    setStatus(response.data);
    console.log(response.data);
  } catch {
    setStatus(null);
  }
}



export const getAnalytics = async (setAnalytics) => {
  try {
    const response = await axios.get(`${ANALYTICS}`);
    setAnalytics(response.data);
  } catch (err) {
    console.error('There was an error getting analytics:', err);
  }
};
export const getFlightAnalytics = async (analytics) => {
  try {
    const response = await axios.get(`${ANALYTICS}/${analytics.id}`);
    console.log(response.data)
  } catch (err) {
    console.error('There was an error getting the flight data analytics:', err);
  }
};

export const handleClickDeleteItem = async (props, setCurrentFlight) => {  
  await axios
    .delete(`${ALL_FLIGHTS_URL}/${props.id}`)
    .then((response) => {
      if (response.status === 200) {
        props.onRemoveData();
      } else {
        console.error('Failed to delete the plan item with id:');
      }
    })
    .catch((err) => console.error('Network or server error when attempting to delete plan item:', err))
};

export const handleClickDeletePoint = async (flight, props) => {
  await axios
    .delete(`${ALL_FLIGHTS_URL}/${flight.id}/points/${props.id}`)
    .then((response) => {
      if (response.status === 200) {
        props.onRemoveData();
      } else {
        console.error('Failed to delete the plan item with id:');
      }
    })
    .catch((err) => console.error('Network or server error when attempting to delete plan item:', err))
};

export const putPointsData = async (flight, formData, point, setPoints, setCurrentFlight) => {
  try {
    const response = await axios.put(`${ALL_FLIGHTS_URL}/${flight.id}/points/${point.id}`, formData);
    console.log({routePoints: response.data.flightPlan.routePoints});
    point.onRemoveData();
    setCurrentFlight(response.data.flightPlan);
    setPoints({routePoints: response.data.flightPlan.routePoints});
  } catch (err) {
    console.error('There was an error updating the flight data:', err);
  }
};

export const getPointsData = async (setPoints, flight) => {
    try {
      await axios
        .get(`${ALL_FLIGHTS_URL}/${flight.id}/points`)
        .then((response) => {setPoints({routePoints: response.data.routePoints})})
    } catch (err) {
      console.error('There was an error fetching the data:', err)
    } 
};

export const getPlanData = async (setFlights) => {
  try {
    const response = await axios.get(ALL_FLIGHTS_URL);
    setFlights(response.data);
    return response.data;
  } catch (err) {
    console.error('There was an error fetching the data:', err)
  } 
};

export const sendFlightDataToServer = async (body, sendingData, setSendingData) => {
  console.log(body)
  try {
    const response = await axios.post(ALL_FLIGHTS_URL, body);
    if (response.status === 200) {
      const responseData = response.data;
      setSendingData(responseData);
      return responseData.flightPlan;
    } else {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
  } catch (error) {
    console.error('There was an error sending the data to the server:', error);
    throw error;
  }
};

export const postFlightPointToFlight = async (flight, point, setPoint) => {
  try {
    const response = await axios.post(`${ALL_FLIGHTS_URL}/${flight.id}/points`, point);

    if (response.status === 200) {
      const responseData = response.data;
      console.log(responseData)
      setPoint({routePoints: responseData.flightPlan.routePoints.sort((a, b) => a < b)});
      return responseData.flightPlan;
    } else {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
  } catch (error) {
    console.error('There was an error sending the data to the server:', error);
    throw error;
  }
};

export const getFlightData = async (flight, setCurFlight) => {
  try {
    const response = await axios.get(`${ALL_FLIGHTS_URL}/${flight}`);
    return setCurFlight(response.data);
  } catch (err) {
    console.error('There was an error fetching the data:', err);
    setCurFlight(null);
  }
};

export const changeFlightData = async (flight, formData, setCurFlight) => {
  if(formData.departureRunwayId === '') { formData.departureRunwayId = null; }
  if(formData.arrivalRunwayId === '') { formData.arrivalRunwayId = null; }
  try {
    const response = await axios.put(`${ALL_FLIGHTS_URL}/${flight}`, formData);
    console.log(response)
    setCurFlight(response.data);
  } catch (err) {
    console.error('There was an error updating the flight data:', err);
    setCurFlight(null);
  }
};

export const getAirports = async (setAirports) => {
  try {
    const response = await axios.get(`${AIRPORTS_URL}`);
    setAirports(response.data);
  } catch (err) {
    console.error('There was an error updating the flight data:', err);
    setAirports(null);
  }
};

export const getAirportData = async (currentAirport, setCurrentAirport) => {
  try {
    const response = await axios.get(`${AIRPORTS_URL}/${currentAirport.id}`);
    console.log(response.data)
    setCurrentAirport(response.data);
  } catch (err) {
    console.error('There was an error setting current airport:', err);
    setCurrentAirport(null);
  }
};




