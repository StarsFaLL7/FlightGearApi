import axios from 'axios';
import { SERVER_URL } from '../const/const';

export const handleClickDeleteItem = async (props) => {  
    await axios ({
        method: 'delete',
        url: `https://localhost:7110/api/launch/stages/${props.index}`,
    })
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
    // await fetch(SERVER_URL, {
    //   method: 'GET',
    //   headers: {
    //     'Content-Type': 'application/json',
    //   },
    // })
    //   .then((response) => {
    //     if (!response.ok) {
    //       throw new Error(`HTTP error! status: ${response.status}`);
    //     }
    //   })
    //   .then((response) => setPlan(response.json()))
    //   .catch((err) => console.error('There was an error fetching the data:', err));
    try {
      const result = await axios.get(SERVER_URL);
      console.log('Plan result.data = ', result.data)
      setPlan(result.data)
    } catch (err) {
      console.error('There was an error fetching the data:', err)
    }
};

export const sendDataToServer = async (body, sendingData, setSendingData) => {
    // Параметр функции - data
    // await fetch(SERVER_URL, {
    //   method: 'POST',
    //   headers: {
    //     'Content-Type': 'application/json',
    //   },
    //   body: JSON.stringify(data)
    // })
    //   .then((response) => {
    //     if (!response.ok) {
    //       throw new Error(`HTTP error! status: ${response.status}`);
    //     }
    //   })
    //   .then((response) => setSendingData(response.json()));

      await axios({
        method: 'post',
        url: SERVER_URL,
        data: body
      })
        .then((response) => {
          if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
          }
        })
        .then((response) => setSendingData(response.json()));

    return sendingData;
};