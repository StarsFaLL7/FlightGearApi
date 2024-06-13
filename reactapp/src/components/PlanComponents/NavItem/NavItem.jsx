import React, { useEffect, useState, useContext } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import { OverlayTrigger, Button, Popover } from "react-bootstrap";
import "../NavItem/NavItem.css";
import PlanPoints from "../frames/flight-frame/flight-frame";

import plane from '../NavItem/imgs/plane.png';
import analytics from '../NavItem/imgs/analytics.png';
import docs from '../NavItem/imgs/docs.png';
import flightPlans from '../NavItem/imgs/flight-plans.png';
import flightPoints from '../NavItem/imgs/flight-stages.png';

import { getData, handlerAddFlight } from "../../../utils/common";
import { PointContext } from "../context/main-context";

import PopupLoad from "../../AnalyzeComponents/popup/popup";
import { getAirportData } from "../../../api-methods/api-methods";

const NavHeader = () => {
  const [showPopoverPoints, setShowPopoverPoints] = useState(false);
  const [showPopoverPlans, setShowPopoverPlans] = useState(false);
  const [showPopoverCurrentFlight, setShowPopoverCurrentFlight] = useState(false);
  const [selectedStartAirport, setSelectedStartAirport] = useState(null);
  const [selectedEndAirport, setSelectedEndAirport] = useState(null);

  const [sendingFlightData, setSendingFlightData] = useState([]);
  const [isFormVisible, setIsFormVisible] = useState(false);

  const { points, flights, addFlight, airports, currentFlight } = useContext(PointContext);

  const handleClickAddFlight = (evt) => {
    evt.preventDefault();
    let formData = getData(document.getElementById('formFlight'));
    const departureRunwayId = selectedStartAirport && selectedStartAirport.runways.find((way) => way.title === formData.departureRunwayId).id;
    const arrivalRunwayId = selectedStartAirport && selectedStartAirport.runways.find((way) => way.title === formData.arrivalRunwayId).id;
    formData = {...formData, departureRunwayId: departureRunwayId, arrivalRunwayId: arrivalRunwayId}
    addFlight(formData, sendingFlightData, setSendingFlightData);
    setIsFormVisible(!isFormVisible);
  };

  const handleStartAirportChange = (evt) => {
    const selected = airports.airports.find(el => el.title === evt.target.value);
    console.log(selected)
    if (selected) {
      getAirportData(selected, setSelectedStartAirport);
    }
  };

  const handleEndAirportChange = (evt) => {
    const selected = airports.airports.find(el => el.title === evt.target.value);
    if (selected) {
      getAirportData(selected, setSelectedEndAirport);
    }
  };

  const popoverPoints = (
    <Popover id="popover-basic-flights">
      <Popover.Header as="h1">All Flights</Popover.Header>
      <Popover.Body>
        <PlanPoints />
      </Popover.Body>
    </Popover>
  );

  const popoverCurrentFlight = (
    <Popover id="popover-basic-current-flight">
      <Popover.Header as="h1">Current Flight</Popover.Header>
      <Popover.Body>
        {getCurForm()}
      </Popover.Body>
    </Popover>
  );

  function getCurForm() {
    return (
      <main className="container">
        <div className={`row`}>
          <form id="formFlight">
            <ul className="list-unstyled m-0">
              <li className="d-flex align-items-center">
                <p className="fs-5">Flight name:</p>
                <input className="form-control ms-auto" defaultValue={`flight title`} type="text" name="title" required />
              </li>
              <li className='start_airport d-flex'>
                <p className={`fs-5`}>Start airport:</p>
                <input className='form-control ms-auto' list="destination-list-1" onChange={handleStartAirportChange} />
                <datalist id="destination-list-1">
                  {airports.airports && airports.airports.map((el, index) => (
                    <option key={index} value={el.title}>{`${el.city}, ${el.title}`}</option>
                  ))}
                </datalist>
              </li>
              {selectedStartAirport && (
                <li className='start_airport d-flex mt-1'>
                  <p className={`fs-5`}>Departure runway:</p>
                  <input className='form-control ms-auto' list="runway-list-1" name={`departureRunwayId`} />
                  <datalist id="runway-list-1">
                    {selectedStartAirport.runways && selectedStartAirport.runways.map((el) => (
                      <option key={el.id} value={el.title}>{el.title}</option>
                    ))}
                  </datalist>
                </li>
              )}

              <li className='end_airport d-flex mt-1'>
                <p className={`fs-5`}>End airport:</p>
                <input className='form-control ms-auto' list="destination-list-2" onChange={handleEndAirportChange} />
                <datalist id="destination-list-2">
                  {airports.airports && airports.airports.map((el, index) => (
                    <option key={index} value={el.title}>
                      {`${el.city}, ${el.title}`}
                    </option>
                  ))}
                </datalist>
              </li>
              {selectedEndAirport && (
                <li className='start_airport d-flex mt-1'>
                  <p className={`fs-5`}>Arrival runway:</p>
                  <input className='form-control ms-auto' list="runway-list-2" name={`arrivalRunwayId`} />
                  <datalist id="runway-list-2">
                    {selectedEndAirport.runways && selectedEndAirport.runways
                      .filter(runway => runway.canBeArrival)
                      .map((el, index) => (
                        <option key={el.id} value={el.title}>{`${el.title}`}</option>
                      ))}
                  </datalist>
                </li>
              )}
              <li className="d-flex align-items-center mt-1">
                <p className="fs-5">Remarks:</p>
                <textarea className="form-control ms-auto" type="text" name="remarks" />
              </li>
              <li className="mt-3 d-flex align-items-center justify-content-center">
                <button className="btn btn-primary btn-hover" onClick={handleClickAddFlight} type="submit">
                  Save
                </button>
              </li>
            </ul>
          </form>
        </div>
      </main>
    );
  }

  return (
    <header className="container-fluid bg-not-light">
      <div>
        <nav className="d-flex flex-row justify-content-between">
          <ul className="nav nav-pills align-items-center">
            <li className="nav-item mx-2">
              <OverlayTrigger
                trigger="click"
                placement="bottom"
                show={showPopoverCurrentFlight}
                onToggle={(nextShow) => setShowPopoverCurrentFlight(nextShow)}
                overlay={popoverCurrentFlight}
                rootClose={true}
              >
                <Button className="btn flight-plans" variant="transparent">
                  <img src={flightPoints} width="53" alt="Flight Plans" />
                </Button>
              </OverlayTrigger>
            </li>
            <li className="nav-item mx-2">
              <OverlayTrigger
                trigger="click"
                placement="bottom"
                show={showPopoverPoints}
                onToggle={(nextShow) => setShowPopoverPoints(nextShow)}
                overlay={popoverPoints}
                rootClose={true}
              >
                <Button className="btn flight-points" variant="transparent">
                  <img src={flightPlans} width="28" alt="Flight Points" />
                </Button>
              </OverlayTrigger>
            </li>
          </ul>
          <ul className="nav nav-pills">
            <li className="nav-item mx-2">
              <a href="/" className="nav-link" aria-current="page" alt="Planner" title="Planner">
                <img src={plane} width="40" alt="Planner" />
              </a>
            </li>
            <li className="nav-item mx-2">
              <a href="/docs.pdf" target="_blank" className="nav-link" alt="Docs" title="Docs" >
                <img src={docs} width="40" alt="Docs" />
              </a>
            </li>
            <li className="nav-item mx-2">
              <a href="/analyze" className="nav-link" alt="Analytics" title="Analytics">
                <img src={analytics} width="40" alt="Analytics" />
              </a>
            </li>
          </ul>
        </nav>
      </div>
    </header>
  );
};

export default NavHeader;