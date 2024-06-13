import React, { useContext, useState, useEffect } from 'react';
import minus from '../../../assets/img/Decrease.png';
import arrow from '../../../assets/img/arrow.png';
import {
    handleClickDeleteItem,
    changeFlightData,
    startFlight,
    getFlightStatus,
    endFlight,
    getAirportData,
} from '../../../api-methods/api-methods';
import 'bootstrap/dist/css/bootstrap.css';
import '../FlightItem/FlightItem.css';
import { getData } from '../../../utils/common';
import FlightPoints from '../frames/flight-points-frame/flight-points-frame';
import { PointContext } from '../context/main-context';

const FlightItem = (props) => {
    const [error, setError] = useState(null);
    const [selectedStartAirport, setSelectedStartAirport] = useState(null);
    const [selectedEndAirport, setSelectedEndAirport] = useState(null);
    const [status, setStatus] = useState(null);

    const { airports, currentFlight, setCurrentFlight, getCurrentFlightById, fetchFlights } = useContext(PointContext);
    const isOpen = currentFlight && currentFlight.id === props.id;

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
        let formData = getData(document.getElementById('form-current-flight'));
        const departureRunwayId = selectedStartAirport && selectedStartAirport.runways.find((way) => way.title === formData.departureRunwayId).id;
        const arrivalRunwayId = selectedEndAirport && selectedEndAirport.runways.find((way) => way.title === formData.arrivalRunwayId).id;
        formData = { ...formData, departureRunwayId, arrivalRunwayId };
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
            setStatus({ status: 'In Progress' });
        } catch (err) {
            setError('Failed to start flight simulation.');
        }
    };

    const handleStartAirportChange = async (evt) => {
        const selected = airports.airports.find(el => el.title === evt.target.value);
        await getAirportData(selected, setSelectedStartAirport);
    };

    const handleEndAirportChange = async (evt) => {
        const selected = airports.airports.find(el => el.title === evt.target.value);
        await getAirportData(selected, setSelectedEndAirport);
    };

    useEffect(() => {
        let timer;
        if (status) {
            timer = setInterval(() => {
                getFlightStatus(setStatus);
            }, 500);
        }
        return () => clearInterval(timer);
    }, [status]);

    const isButtonDisabled = status !== null;

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
                                <input className='form-control ms-auto' list="destination-list-1" defaultValue={currentFlight.departureRunway && currentFlight.departureRunway.airport.title} onChange={handleStartAirportChange} />
                                <datalist id="destination-list-1">
                                    {airports.airports && airports.airports.map((el, index) => (
                                        <option key={index} value={el.title}>{`${el.city}, ${el.title}`}</option>
                                    ))}
                                </datalist>
                            </li>
                            {selectedStartAirport && (
                                <li className='start_airport d-flex m-1'>
                                    <p className={`m-0 align-self-center`}>Departure runway:</p>
                                    <input className='form-control ms-auto' list="runway-list-1" defaultValue={currentFlight.departureRunway && currentFlight.departureRunway.title} name={`departureRunwayId`} />
                                    <datalist id="runway-list-1">
                                        {selectedStartAirport.runways && selectedStartAirport.runways.map((el) => (
                                            <option key={el.id} value={el.title}>{el.title}</option>
                                        ))}
                                    </datalist>
                                </li>
                            )}

                            <li className='end_airport d-flex m-1'>
                                <p className={`m-0 align-self-center`}>End airport:</p>
                                <input className='form-control ms-auto' list="destination-list-2" defaultValue={currentFlight.arrivalRunway && currentFlight.arrivalRunway.airport.title} onChange={handleEndAirportChange} />
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
                                    <input className='form-control ms-auto' list="runway-list-2" defaultValue={currentFlight.arrivalRunway && currentFlight.arrivalRunway.title} name={`arrivalRunwayId`} />
                                    <datalist id="runway-list-2">
                                        {selectedEndAirport.runways && selectedEndAirport.runways
                                            .filter(runway => runway.canBeArrival)
                                            .map((el) => (
                                                <option key={el.id} value={el.title}>{`${el.title}`}</option>
                                            ))}
                                    </datalist>
                                </li>
                            )}

                            <li className='d-inline points mt-1'>
                                <p className={`border-bottom border-secondary m-0 mx-auto align-self-center`}>Points:</p>
                                <table className="table mb-0 mt-1">
                                    <thead className={`m-auto`}>
                                        <tr>
                                            <th className="text-center align-middle rounded-3 m-0 p-2">#</th>
                                            <th className="text-center align-middle rounded-3 m-0 p-2">Longitude</th>
                                            <th className="text-center align-middle rounded-3 m-0 p-2">Latitude</th>
                                            <th className="text-center align-middle rounded-3 m-0 p-2">Altitude</th>
                                        </tr>
                                    </thead>
                                </table>
                                <div className='point-container form-control m-1 w-auto'>
                                    <FlightPoints />
                                </div>
                            </li>
                        </ul>
                        <div className={`d-flex block-btns`}>
                            <div>
                                <button className="m-1 btn save-flight btn-primary text-light" type="button" onClick={saveFlight}>Save</button>
                                <button className={`m-1 btn delete-flight btn-primary text-light ${isButtonDisabled ? 'disabled' : ''}`} type="button" onClick={startFlightSimulation}>{status ? `${status.status}...` : 'Start'}</button>
                                {status && <button className={`m-1 btn delete-flight btn-danger text-light`} type="button" onClick={() => endFlight(setStatus)}>Exit</button>}
                                <button className="m-1 btn delete-flight btn-secondary text-light" type="button" onClick={() => { handleClickDeleteItem(props, setCurrentFlight); }}>Delete</button>
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