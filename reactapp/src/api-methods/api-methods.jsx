import axios from 'axios';
import { SERVER_URL, ALL_FLIGHTS_URL } from '../const/const';

export const handleClickDeleteItem = async (props) => {  
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

/* export const getPointData = async (flight, setPoint) => {
    try {
      await axios
        .get(`${ALL_FLIGHTS_URL}/${flight}/points`)
        .then((response) => {setPoint(response.data)})
    } catch (err) {
      console.error('There was an error fetching the data:', err)
    } 
}; */

export const getPlanData = async (setPlan) => {
  try {
    await axios
      .get(ALL_FLIGHTS_URL)
      .then((response) => {setPlan(response.data)})
  } catch (err) {
    console.error('There was an error fetching the data:', err)
  } 
};

export const sendFlightDataToServer = async (body, sendingData, setSendingData) => {
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
    throw error; // Выбрасываем ошибку, чтобы вызывающий код мог обработать ее
  }
};

export const postFlightPointToFlight = async (flight, point, sendingData, setSendingData) => {
  try {
    const response = await axios.post(`${ALL_FLIGHTS_URL}/${flight}/points`, point);
    if (response.status === 200) {
      const responseData = response.data;
      setSendingData(responseData);
      return responseData.flightPlan;
    } else {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
  } catch (error) {
    console.error('There was an error sending the data to the server:', error);
    throw error; // Выбрасываем ошибку, чтобы вызывающий код мог обработать ее
  }
};


/* export const getAllflightsData = async (setPlan) => {
  try {
    await axios
      .get(ALL_FLIGHTS_URL)
      .then((response) => {setPlan(response.data); console.log(setPlan(response.data))})
  } catch (err) {
    console.error('There was an error fetching the data:', err)
  }
}; */

export const getFlightData = async (flight, setCurFlight) => {
  try {
    const response = await axios.get(`${ALL_FLIGHTS_URL}/${flight}`);
    setCurFlight(response.data);
  } catch (err) {
    console.error('There was an error fetching the data:', err);
    setCurFlight(null);
  }
};

export const changeFlightData = async (flight, data, setCurFlight) => {
  try {
    const response = await axios.put(`${ALL_FLIGHTS_URL}/${flight}`, data);
    setCurFlight(response.data);
  } catch (err) {
    console.error('There was an error updating the flight data:', err);
    setCurFlight(null);
  }
};


