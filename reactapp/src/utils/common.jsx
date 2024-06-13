import { postFlightPointToFlight, sendFlightDataToServer } from "../api-methods/api-methods";

const clearForm = () => {
    const form = document.getElementById('formFlight');
    form.querySelector('input[name=title]').value = '';
    form.querySelector('textarea[name=remarks]').value = '';
    form.querySeletor('input[list=destination-list-1]').value = '';
    form.querySelector('input[list=runway-list-1]').value = '';
    form.querySelector('input[list=destination-list-2]').value = '';
    form.querySelector('input[list=runway-list-2]').value = '';
}

export const handlerAddPoint = async (formData, points, setPoint, sendingPointData, setSendingPointData, currentFlight) => {
  console.log(currentFlight);
  console.log(points);

  if (!formData.longitude || !formData.latitude || !formData.altitude) {
    return;
  }

  let newPoint = { ...formData };

  try {
    return await postFlightPointToFlight(currentFlight, newPoint, setPoint);
  } catch (error) {
    console.error('There was an error sending the data to the server:', error);
  }
};

export const handlerAddFlight = async (formData, flight, setFlight, sendingFlightData, setSendingFlightData, airports) => {
  if (!formData.title) { return; }
  if(formData.departureRunwayId === '') { formData.departureRunwayId = null; }
  if(formData.arrivalRunwayId === '') { formData.arrivalRunwayId = null; }

  const newFlight = { 
    ...formData,
  };
  try {
    setFlight((prevFlight) => {
      const updateFlights = [...(prevFlight.flightPlans ? prevFlight.flightPlans : prevFlight), newFlight];
      return updateFlights;
    });
    await sendFlightDataToServer(newFlight, sendingFlightData, setSendingFlightData);
  } catch (error) {
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
    return data;
};