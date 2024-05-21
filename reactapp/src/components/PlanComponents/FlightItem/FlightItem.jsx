import React, { useEffect, useState } from 'react';
import minus from '../../../assets/img/Decrease.png';
import arrow from '../../../assets/img/arrow.png';
import { handleClickDeleteItem, getFlightData, changeFlightData, getPlanData } from '../../../api-methods/api-methods';
import 'bootstrap/dist/css/bootstrap.css';
import '../FlightItem/FlightItem.css';
import { getData } from '../../../utils/common';

const FlightItem = (props) => {
    const [currentFlight, setCurrentFlight] = useState(null);
    const [showForm, setShowForm] = useState(false);
    const [error, setError] = useState(null);

    let isOpen = props.id ? true : false;

    const handleFlightItemFormChange = (evt) => {
        evt.preventDefault();
        props.handleFormToggle(props.id);
        getFlightData(props.id, setCurrentFlight);
        setShowForm(!showForm);
        console.log(props.id)
    };

    useEffect(() => {getPlanData(setCurrentFlight);}, [])
    useEffect(() => {},[])

    const saveFlight = async (evt) => {
        evt.preventDefault();
        const formData = getData(document.getElementById('form-current-flight'));
        try {
            await changeFlightData(currentFlight.id, formData, setCurrentFlight);
            setShowForm(false);
            setError(null);
        } catch (err) {
            setError('Failed to save flight data.');
        }
        getFlightData(props.id, setCurrentFlight);
        props.handleFormToggle(null);
    };

    return (
        <>
            {error && <div className="alert alert-danger">{error}</div>}
            {currentFlight && (
                <div className={`d-flex bg-not-light rounded-3 form-container m-1`} id={props.id}>
                    {showForm ? (
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
                                <li className='d-inline points m-1'>
                                    <p className={`border-bottom border-secondary m-0 mx-auto align-self-center`}>Points:</p>
                                    <div className='form-control m-1 w-auto'>
                                        {/* Render points data if needed */}
                                    </div>
                                </li>
                            </ul>
                            <div className={`d-flex block-btns`}>
                                <div>
                                    <button className="m-1 btn save-flight btn-primary text-light" type="submit">Save</button>
                                    <button className="m-1 btn delete-flight btn-secondary text-light" type="button" onClick={() => handleClickDeleteItem(props)}>Delete</button>
                                </div>
                                <button className='btn' type='button' onClick={handleFlightItemFormChange}>
                                    <img className='arrow arrow-down' src={arrow} alt='Arrow' />
                                </button>
                            </div>
                        </form>
                    ) : (
                        <>
                            <div className={`title m-0 align-self-center`}>
                                {currentFlight.title !== undefined ? currentFlight.title : props.title}
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
            )}
        </>
    );
};

export default FlightItem;