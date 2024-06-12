import React, { useContext, useState, useEffect } from 'react';
import minus from '../../../assets/img/Decrease.png';
import arrow from '../../../assets/img/arrow.png';
import { handleClickDeleteItem, changeFlightData, startFlight, getAnalytics, getFlightAnalytics, getAirportData } from '../../../api-methods/api-methods';
import 'bootstrap/dist/css/bootstrap.css';
import '../FlightItem/FlightItem.css';
import { getData } from '../../../utils/common';
import FlightPoints from '../frames/flight-points-frame/flight-points-frame';
import { PointContext } from '../context/main-context';
import PopupLoad from '../../AnalyzeComponents/popup/popup';

const FlightItem = (props) => {
    const [error, setError] = useState(null);
    const [selectedStartAirport, setSelectedStartAirport] = useState(null);
    const [selectedEndAirport, setSelectedEndAirport] = useState(null);

    const { airports, currentFlight, setCurrentFlight, getCurrentFlightById, fetchFlights, analytics } = useContext(PointContext);
    const isOpen = currentFlight && currentFlight.id === props.id;
    console.log(currentFlight);

    useEffect(() => {
        if (isOpen && currentFlight) {
            if (currentFlight.departureRunway && currentFlight.departureRunway.airport) {
                handleStartAirportChange({ target: { value: currentFlight.departureRunway.airport.title } });
            }
            if (currentFlight.arrivalRunway && currentFlight.arrivalRunway.airport) {
                handleEndAirportChange({ target: { value: currentFlight.arrivalRunway.airport.title } });
            }
        }
    }, [isOpen, currentFlight]);

    const handleFlightItemFormChange = async (evt) => {
        evt.preventDefault();
        if (!isOpen) {
            try {
                await getCurrentFlightById(props.id);
                setError(null);
            } catch (err) {
                setError('Failed to fetch flight data.');
            }
        } else {
            setCurrentFlight(null);
        }
    };

    const saveFlight = async (evt) => {
        evt.preventDefault();
        const formData = getData(document.getElementById('form-current-flight'));
        try {
            await changeFlightData(currentFlight.id, formData, setCurrentFlight);
            fetchFlights();
            setError(null);
        } catch (err) {
            setError('Failed to save flight data.');
        }
    };

    const startFlightSimulation = async (evt) => {
        evt.preventDefault();
        try {
            await startFlight(currentFlight);
            setError(null);
        } catch (err) {
            setError('Failed to start flight simulation.');
        }
    };

    const handleStartAirportChange = (evt) => {
        const selected = airports.airports.find(el => el.title === evt.target.value);
        getAirportData(selected, setSelectedStartAirport);
    };

    const handleEndAirportChange = (evt) => {
        const selected = airports.airports.find(el => el.title === evt.target.value);
        getAirportData(selected, setSelectedEndAirport);
    };

    return (
        <>
            {error && <div className="alert alert-danger">{error}</div>}
            <div className={`d-flex bg-not-light rounded-3 form-container m-1`} id={props.id}>
                {isOpen ? (
                    <form className="form-container" id="form-current-flight">
                        <ul className={`list-unstyled`}>
                            <li className='title d-flex m-1'>
                                <p className={`m-0 align-self-center`}>Title:</p>
                                <input className='form-control ms-auto' defaultValue={currentFlight.title} name="title" required />
                            </li>
                            <li className='remarks d-flex m-1'>
                                <p className={`m-0 align-self-center`}>Remarks:</p>
                                <input className='form-control ms-auto' defaultValue={currentFlight.remarks} name="remarks" />
                            </li>

                            <li className='start_airport d-flex m-1'>
                                <p className={`m-0 align-self-center`}>Start airport:</p>
                                <input className='form-control ms-auto' list="destination-list-1" defaultValue={currentFlight.departureRunway && currentFlight.departureRunway.airport.title} onChange={handleStartAirportChange}/>
                                <datalist id="destination-list-1">
                                    {airports.airports && airports.airports.map((el, index) => (
                                        <option key={index} value={el.title}>{`${el.city}, ${el.title}`}</option>
                                    ))}
                                </datalist>
                            </li>
                            {selectedStartAirport && (
                                <li className='start_airport d-flex m-1'>
                                    <p className={`m-0 align-self-center`}>Departure runway:</p>
                                    <input className='form-control ms-auto' list="runway-list-1" defaultValue={currentFlight.departureRunway && currentFlight.departureRunway.title} name={`departureRunwayId`}/>
                                    <datalist id="runway-list-1">
                                        {selectedStartAirport.runways && selectedStartAirport.runways.map((el, index) => (
                                            <option key={index} value={el.id}>{el.title}</option>
                                        ))}
                                    </datalist>
                                </li> 
                            )}

                            <li className='end_airport d-flex m-1'>
                                <p className={`m-0 align-self-center`}>End airport:</p>
                                <input className='form-control ms-auto' list="destination-list-2" defaultValue={currentFlight.arrivalRunway && currentFlight.arrivalRunway.airport.title} onChange={handleEndAirportChange}/>
                                <datalist id="destination-list-2">
                                    {airports.airports && airports.airports.map((el, index) => (
                                        <option key={index} value={el.title}>
                                            {`${el.city}, ${el.title}`}
                                        </option>
                                    ))}
                                </datalist>
                            </li>
                            {selectedEndAirport && (
                                <li className='start_airport d-flex m-1'>
                                    <p className={`m-0 align-self-center`}>Arrival runway:</p>
                                    <input className='form-control ms-auto' list="runway-list-2" defaultValue={currentFlight.arrivalRunway && currentFlight.arrivalRunway.title} name={`arrivalRunwayId`}/>
                                    <datalist id="runway-list-2">
                                        {selectedEndAirport.runways && selectedEndAirport.runways
                                            .filter(runway => runway.canBeArrival)
                                            .map((el, index) => (
                                                <option key={index} value={el.id}>{`${el.title}`}</option>
                                            ))}
                                    </datalist>
                                </li>
                            )}

                            <li className='d-inline points m-1'>
                                <p className={`border-bottom border-secondary m-0 mx-auto align-self-center`}>Points:</p>
                                <div className='point-container form-control m-1 w-auto'>
                                    <FlightPoints />
                                </div>
                            </li>
                        </ul>
                        <div className={`d-flex block-btns`}>
                            <div>
                                <button className="m-1 btn save-flight btn-primary text-light" type="button" onClick={saveFlight}>Save</button>
                                <button className="m-1 btn delete-flight btn-primary text-light" type="button" onClick={startFlightSimulation}>Start</button>
                                {/* <button className="m-1 btn delete-flight btn-primary text-light" type="button" onClick={handleGetAnalytics}>Get analytics</button> */}
                                <button className="m-1 btn delete-flight btn-secondary text-light" type="button" onClick={() =>{handleClickDeleteItem(props, setCurrentFlight);}}>Delete</button>
                            </div>
                            <button className='btn' type='button' onClick={() => setCurrentFlight(null)}>
                                <img className='arrow arrow-down' src={arrow} alt='Arrow' />
                            </button>
                        </div>
                    </form>
                ) : (
                    <>
                        <div className={`title m-0 align-self-center`}>
                            {props.title}
                        </div>
                        <div className={`btns ms-auto`}>
                            <button className='btn' type='button' onClick={() => handleClickDeleteItem(props)}>
                                <img src={minus} alt='Decrease' />
                            </button>
                            <button className='btn' type='button' onClick={handleFlightItemFormChange}>
                                <img className='arrow' src={arrow} alt='Arrow' />
                            </button>
                        </div>
                    </>
                )}
            </div>
        </>
    );
};

export default FlightItem;