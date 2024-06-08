import React, { useContext, useEffect, useState } from 'react';
import minus from '../../../assets/img/Decrease.png';
import arrow from '../../../assets/img/arrow.png';
import { handleClickDeleteItem, changeFlightData, startFlight } from '../../../api-methods/api-methods';
import 'bootstrap/dist/css/bootstrap.css';
import '../FlightItem/FlightItem.css';
import { getData } from '../../../utils/common';
import FlightPoints from '../frames/flight-points-frame/flight-points-frame';
import { PointContext } from '../context/main-context';

const FlightItem = (props) => {
    const [error, setError] = useState(null);

    const { airports, currentFlight, setCurrentFlight, getCurrentFlightById, fetchFlights } = useContext(PointContext);
    const isOpen = currentFlight && currentFlight.id === props.id;

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
        console.log(currentFlight)
        try {
            await startFlight(currentFlight);
            setError(null);
        } catch (err) {
            setError('Failed to start flight simulation.');
        }
    };

    return (
        <>
            {error && <div className="alert alert-danger">{error}</div>}
            <div className={`d-flex bg-not-light rounded-3 form-container m-1`} id={props.id}>
                {isOpen ? (
                    <form className="form-container" id="form-current-flight" onSubmit={saveFlight}>
                        <ul className={`list-unstyled`}>
                            <li className='title d-flex m-1'>
                                <p className={`m-0 align-self-center`}>Title:</p>
                                <input className='form-control ms-auto' defaultValue={currentFlight.title} name="title" required />
                            </li>
                            <li className='remarks d-flex m-1'>
                                <p className={`m-0 align-self-center`}>Remarks:</p>
                                <input className='form-control ms-auto' defaultValue={currentFlight.remarks} name="remarks" required />
                            </li>
                            <li className='start_airport d-flex m-1'>
                                <p className={`m-0 align-self-center`}>Start airport:</p>
                                <input className='form-control ms-auto' defaultValue={currentFlight.departureRunwayId} list="destination-list-1" name="departureRunwayId" />
                                <datalist id="destination-list-1">
                                    {airports.airports && airports.airports.map((el, index) => (
                                        <option key={index} value={el.title}>{`${el.city}, ${el.title}`}</option>
                                    ))}
                                </datalist>
                            </li>
                            <li className='end_airport d-flex m-1'>
                                <p className={`m-0 align-self-center`}>End airport:</p>
                                <input className='form-control ms-auto' defaultValue={currentFlight.arrivalRunwayId} list="destination-list-2" name="arrivalRunwayId" />
                                <datalist id="destination-list-2">
                                    {airports.airports && airports.airports.map((el, index) => (
                                        <option key={index} value={el.title}>{`${el.city}, ${el.title}`}</option>
                                    ))}
                                </datalist>
                            </li>
                            <li className='d-inline points m-1'>
                                <p className={`border-bottom border-secondary m-0 mx-auto align-self-center`}>Points:</p>
                                <div className='point-container form-control m-1 w-auto'>
                                    <FlightPoints />
                                </div>
                            </li>
                        </ul>
                        <div className={`d-flex block-btns`}>
                            <div>
                                <button className="m-1 btn save-flight btn-primary text-light" type="submit">Save</button>
                                <button className="m-1 btn delete-flight btn-primary text-light" type="button" onClick={(e) => {startFlightSimulation(e) }}>Start</button>
                                <button className="m-1 btn delete-flight btn-primary text-light disabled" type="button" onClick={() => { }}>Get analytics</button>
                                <button className="m-1 btn delete-flight btn-secondary text-light" type="button" onClick={() => handleClickDeleteItem(props)}>Delete</button>
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