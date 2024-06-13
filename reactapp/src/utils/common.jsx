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
  console.log(currentFlight);
  console.log(points);

  if (!formData.longitude || !formData.latitude || !formData.altitude) {
    return;
  }

  let newPoint = { order: points.routePoints.length, ...formData };
  if(currentFlight.departureRunway && !currentFlight.arrivalRunway) {
    newPoint = { order: points.routePoints.length, ...formData }
  } else if(currentFlight.arrivalRunway && !currentFlight.departureRunway){
    let order = points.routePoints.filter((point) => !point.isEditable).length;
    newPoint = { order: order, ...formData };
  } else if (points.routePoints.length >= 3 && points.routePoints.slice(0, 3).every((point) => point.isEditable === false)) {
    newPoint = { order: points.routePoints.length, ...formData };
  } else {
    newPoint = { order: points.routePoints.length, ...formData };
  }

  console.log(newPoint);

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
  //formData.arrivalRunwayId = airports.airports.filter((el) => formData.arrivalRunwayId === `${el.city}, ${el.title}`)[0].id;
  //formData.departureRunwayId = airports.airports.filter((el) => formData.departureRunwayId === `${el.city}, ${el.title}`)[0].id;
  const newFlight = { 
    ...formData,
    //arrivalRunwayId: formData.arrivalRunwayId === '' ? null : airports.airports.filter((el) => formData.arrivalRunwayId === `${el.city}, ${el.title}`)[0].id,
    //departureRunwayId: formData.departureRunwayId === '' ? null : airports.airports.filter((el) => formData.departureRunwayId === `${el.city}, ${el.title}`)[0].id,
  };
  try {
    setFlight((prevFlight) => {
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
    return data;
};