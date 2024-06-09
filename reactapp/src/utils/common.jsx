import { useState, useContext } from "react";
import { getPlanData, postFlightPointToFlight, sendFlightDataToServer } from "../api-methods/api-methods";
import { PointContext } from "../components/PlanComponents/context/main-context";

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
export const handlerAddPoint = async (formData, points, setPoint, sendingPointData, setSendingPointData, currentFlight) => {
  console.log(currentFlight)
    if (!formData.longitude || !formData.latitude || !formData.altitude) { return; }
    const newPoint = { order: points.routePoints.length, ...formData };
    console.log(newPoint)
    /* setPoint((prevPoints) => {
      console.log(prevPoints) 
      const updatePoints = [...prevPoints.routePoints, newPoint];
      return updatePoints;
    }); */

    try {
      return await postFlightPointToFlight(currentFlight, newPoint, setPoint);
    } catch (error) {
      // Handle any errors that occur during the fetch
      console.error('There was an error sending the data to the server:', error);
    }
    //clearForm();
};

export const handlerSetCurrentFlight = async (formData, flight, setFlight, sendingFlightData, setSendingFlightData) => {
  /* console.log(formData)
  if (!formData.title) { return; }
  if(formData.departureRunwayId === "") { formData.departureRunwayId = null; }
  if(formData.arrivalRunwayId === "") { formData.arrivalRunwayId = null; }
  const newFlight = { ...formData };

  try {
    const response = await postFlightPointToFlight(newFlight, sendingFlightData, setSendingFlightData);

    setFlight((prevPoint) => { 
      prevPoint = response.data;
      console.log(prevPoint)
    });
    return response;
  } catch (error) {
    console.error('There was an error sending the data to the server:', error);
  }
  clearForm(); */
};

export const handlerAddFlight = async (formData, flight, setFlight, sendingFlightData, setSendingFlightData, airports) => {
  if (!formData.title) { return; }
  if(formData.departureRunwayId === '') { formData.departureRunwayId = null; }
  if(formData.arrivalRunwayId === '') { formData.arrivalRunwayId = null; }
  //formData.arrivalRunwayId = airports.airports.filter((el) => formData.arrivalRunwayId === `${el.city}, ${el.title}`)[0].id;
  //formData.departureRunwayId = airports.airports.filter((el) => formData.departureRunwayId === `${el.city}, ${el.title}`)[0].id;
  const newFlight = { 
    ...formData,
    //arrivalRunwayId: formData.arrivalRunwayId === '' ? null : airports.airports.filter((el) => formData.arrivalRunwayId === `${el.city}, ${el.title}`)[0].id,
    //departureRunwayId: formData.departureRunwayId === '' ? null : airports.airports.filter((el) => formData.departureRunwayId === `${el.city}, ${el.title}`)[0].id,
  };
  try {
    setFlight((prevFlight) => {
      console.log(prevFlight)
      const updateFlights = [...(prevFlight.flightPlans ? prevFlight.flightPlans : prevFlight), newFlight];
      return updateFlights;
    });
    return await sendFlightDataToServer(newFlight, sendingFlightData, setSendingFlightData);
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
    console.log(data)
    return data;
};