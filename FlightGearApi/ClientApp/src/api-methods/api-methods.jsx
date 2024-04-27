import axios from 'axios';
import { SERVER_URL } from '../const/const';

export const handleClickDeleteItem = async (props) => {  
    await axios
      .delete(`https://localhost:7110/api/launch/stages/${props.index}`)
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
  console.log(setPlan)
    try {
      await axios
        .get(SERVER_URL)
        .then((response) => {setPlan(response.data); console.log(response.data)})
    } catch (err) {
      console.error('There was an error fetching the data:', err)
    }
};

export const sendDataToServer = async (body, sendingData, setSendingData) => {
    console.log(body)
      await axios
        .post(SERVER_URL, body)
        .then((response) => {
          console.log(response)
          if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
          }
        })
        .then((response) => setSendingData(response.json()));
    return sendingData;
};
