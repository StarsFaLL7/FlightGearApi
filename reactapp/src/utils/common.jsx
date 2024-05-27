import { useState } from "react";
import { getPlanData, postFlightPointToFlight, sendFlightDataToServer } from "../api-methods/api-methods";

const clearForm = () => {
    const form = document.getElementById('formFlight');

    /* form.querySelector('input[name=longitude]').value = '';
    form.querySelector('input[name=latitude]').value = '';
    form.querySelector('input[name=speed]').value = '';
    form.querySelector('input[name=altitude]').value = ''; */
    form.querySelector('input[name=title]').value = '';
    form.querySelector('textarea[name=remarks]').value = '';
}
// sendingData, setSendingData
export const handlerAddPoint = async (formData, point, setPoint, sendingPointData, setSendingPointData) => {
    if (!formData.longitude || !formData.latitude || !formData.speed || !formData.altitude) { return; }
    const newPoint = { order: point.length, ...formData };

    setPoint((prevPoints) => { 
      const updatePoints = [...prevPoints, newPoint]; 
      return updatePoints;
    });

    try {
      return await postFlightPointToFlight(newPoint, sendingPointData, setSendingPointData);
    } catch (error) {
      // Handle any errors that occur during the fetch
      console.error('There was an error sending the data to the server:', error);
    }
    //clearForm();
};

export const handlerAddFlight = async (formData, flight, setFlight, sendingFlightData, setSendingFlightData) => {
  if (!formData.title) { return; }
  if(formData.departureRunwayId === "") { formData.departureRunwayId = null; }
  if(formData.arrivalRunwayId === "") { formData.arrivalRunwayId = null; }
  const newFlight = { ...formData };

  setFlight((prevFlight) => {
    console.log(prevFlight)
    const updateFlights = [...prevFlight, newFlight];
    return updateFlights;
    //return [...prevFlight.flightPlans, newFlight];
  });

  try {
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