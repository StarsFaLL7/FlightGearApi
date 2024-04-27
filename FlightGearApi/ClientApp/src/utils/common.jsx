import { sendDataToServer } from "../api-methods/api-methods";

const clearForm = () => {
    const form = document.getElementById('form');

    form.querySelector('input[name=longitude]').value = '';
    form.querySelector('input[name=latitude]').value = '';
    form.querySelector('input[name=speed]').value = '';
    form.querySelector('input[name=altitude]').value = '';
}

export const handlerAddFlight = async (formData, flight, setFlight, sendingData, setSendingData) => {
  if (!formData.longitude || !formData.latitude || !formData.speed || !formData.altitude) { return; }

  const newFlight = { id: flight.length, ...formData };

  setFlight((prevFlight) => {
    const updatedFlight = [...prevFlight, newFlight];
    //logPlanItemData(updatedPlan); Log the updated plan items
    return updatedFlight;
  });

  /* try {
    // Asynchronously send new plan to server
    await sendDataToServer(newPlan, sendingData, setSendingData);
    // Here you might want to do something with the response
  } catch (error) {
    // Handle any errors that occur during the fetch
    console.error('There was an error sending the data to the server:', error);
  } */
  clearForm();
};

export const handlerAddPlan = async (formData, plan, setPlan, sendingData, setSendingData) => {
    if (!formData.longitude || !formData.latitude || !formData.speed || !formData.altitude) { return; }

    const newPlan = { id: plan.length, ...formData };

    setPlan((prevPlan) => {
      const updatedPlan = [...prevPlan, newPlan];
      //logPlanItemData(updatedPlan); Log the updated plan items
      return updatedPlan;
    });

    try {
      // Asynchronously send new plan to server
      await sendDataToServer(newPlan, sendingData, setSendingData);
      // Here you might want to do something with the response
    } catch (error) {
      // Handle any errors that occur during the fetch
      console.error('There was an error sending the data to the server:', error);
    }
    clearForm();
};

export const getData = (form) => {
    const dataForm = new FormData(form);
    const data = {};
    for (let [name, value] of dataForm){
      data[name] = value;
    }
    console.log(data);
    return data;
};