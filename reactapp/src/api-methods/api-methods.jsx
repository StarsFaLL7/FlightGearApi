import axios from 'axios';
import { SERVER_URL, ALL_FLIGHTS_URL } from '../const/const';

export const handleClickDeleteItem = async (props) => {  
    await axios
      .delete(`https://localhost:7229/api/launch/stages/${props.index}`)
      .then((response) => {
        if (response.status === 200) {
          props.onRemoveData();
        } else {
          console.error('Failed to delete the plan item with id:', props.id);
        }
      })
      .catch((err) => console.error('Network or server error when attempting to delete plan item:', err))
};

export const getPlanData = async (setPlan) => {
    try {
      await axios
        .get(ALL_FLIGHTS_URL)
        .then((response) => {console.log(response); setPlan(response.data)})
    } catch (err) {
      console.error('There was an error fetching the data:', err)
    }
};

/* export const sendDataToServer = async (body, sendingData, setSendingData) => {
      await axios
        .post(ALL_FLIGHTS_URL, body)
        .then((response) => {
          if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
          }
        })
        .then((response) => setSendingData(response.json()));
    return sendingData;
}; */

export const sendFlightDataToServer = async (body, sendingData, setSendingData) => {
  await axios
    .post(ALL_FLIGHTS_URL, body)
    .then((response) => {
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
    })
    .then((response) => setSendingData(response.json()));
return sendingData;
};

export const getAllflightsData = async (setPlan) => {
  try {
    await axios
      .get(ALL_FLIGHTS_URL)
      .then((response) => {setPlan(response.data)})
  } catch (err) {
    console.error('There was an error fetching the data:', err)
  }
};

export const getflightData = async (props, setPlan) => {
  try {
    await axios
      .get(`${ALL_FLIGHTS_URL}${props.index}`)
      .then((response) => {setPlan(response.data)})
  } catch (err) {
    console.error('There was an error fetching the data:', err)
  }
};

export const deleteflightData = async (props) => {
  await axios
    .delete(`${ALL_FLIGHTS_URL}${props.index}`)
    .then((response) => {
      if (response.status === 200) {
        props.onRemoveData();
      } else {
        console.error('Failed to delete the flight item with id:', props.id);
      }
    })
    .catch((err) => console.error('Network or server error when attempting to delete flight item:', err))
};

export const postFlightData = async (body, sendingData, setSendingData) => {
  await axios
    .post(ALL_FLIGHTS_URL, body)
    .then((response) => {
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
    })
    .then((response) => setSendingData(response.json()));
return sendingData;
};

export const updateFlightData = async (props, body, sendingData, setSendingData) => {
  await axios
    .put(`${ALL_FLIGHTS_URL}${props.index}`, body)
    .then((response) => {
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
    })
    .then((response) => setSendingData(response.json()));
return sendingData;
};
