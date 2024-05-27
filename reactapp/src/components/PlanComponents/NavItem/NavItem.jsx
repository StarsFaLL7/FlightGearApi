import React, { useEffect, useState } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import { OverlayTrigger, Button, Popover } from "react-bootstrap";
import "../NavItem/NavItem.css"
import PlanPoints from "../frames/flight-frame/flight-frame";

import plane from '../NavItem/imgs/plane.png';
import analytics from '../NavItem/imgs/analytics.png';
import docs from '../NavItem/imgs/docs.png';
import flightPlans from '../NavItem/imgs/flight-plans.png';
import flightPoints from '../NavItem/imgs/flight-stages.png';

import { getData, handlerAddFlight } from "../../../utils/common";
import plus from '../../../assets/img/Union.png';
import FlightPoints from "../frames/flight-points-frame/flight-points-frame";
import PointItem from "../PointItem/PointItem";

const NavHeader = () => {
  const [showPopoverPoints, setShowPopoverPoints] = useState(false);
  const [showPopoverPlans, setShowPopoverPlans] = useState(false);
  const [showPopoverCurrentFlight, setShowPopoverCurrentFlight] = useState(false);

  const [flight, setFlight] = useState([]);
  const [sendingFlightData, setSendingFlightData] = useState([]);
  const [isFormVisible, setIsFormVisible] = useState(false);

  //useEffect(() => {getForm(); })

  const handleCreateNewFlightForm = (evt) => {
    evt.preventDefault();
    setIsFormVisible(!isFormVisible);
  };

  const handleClickAddFlight = (evt) => {
    evt.preventDefault();
    const formData = getData(document.getElementById('formFlight'));
    handlerAddFlight(formData, flight, setFlight, sendingFlightData, setSendingFlightData);
  };

  const handleChooseCurrentFlight = () => {
    setShowPopoverPoints(true);
    setShowPopoverCurrentFlight(false);
  };

  const popoverPoints = (
    <Popover id="popover-basic">
      <Popover.Header as="h1">All Flights</Popover.Header>
      <Popover.Body>
        <PlanPoints />
      </Popover.Body>
    </Popover>
  );

  const popoverPlans = (
    <Popover id="popover-basic">
      <Popover.Header as="h1">All Flights!</Popover.Header>
      <Popover.Body>
        {/* <FlightsPlanTable /> */}
      </Popover.Body>
    </Popover>
  );

  const popoverCurrentFlight = (
    <Popover id="popover-basic">
      <Popover.Header as="h1">Current Flight</Popover.Header>
      <Popover.Body>
        {getMainForm()}
      </Popover.Body>
    </Popover>
  );

  function getForm() {
    return isFormVisible ? (<form className={``} id="formFlight">
      <ul className="list-unstyled m-0">
        <li className="d-flex align-items-center">
          <p className="fs-5">Flight name:</p>
          <input className="form-control ms-auto" type="text" name="title" required />
        </li>
        <li className="d-flex align-items-center">
          <p className="fs-5">Remarks:</p>
          <textarea className="form-control ms-auto" type="text" name="remarks" required />
        </li>
        <li className="d-inline points m-1">
          <p className="border-bottom border-secondary m-0 mx-auto align-self-center">Points:</p>
          <div className="point-container form-control m-1 w-auto">
            <FlightPoints />
          </div>
        </li>
        <li className="d-flex align-items-center">
          <input className="form-control ms-auto" defaultValue={null} name="departureRunwayId" hidden />
        </li>
        <li className="d-flex align-items-center">
          <input className="form-control ms-auto" defaultValue={null} name="arrivalRunwayId" hidden />
        </li>
        <li className="mt-3 d-flex align-items-center justify-content-center">
          <button className="btn btn-primary btn-hover" onClick={handleClickAddFlight} type="submit">
            <img src={plus} alt="Union" />
          </button>
        </li>
      </ul>
    </form>) : ('');
  }

  function getMainForm() {
    return (<main className="container">
      <div className={`row`}>
        <div className="d-flex justify-content-center">
          <button className="btn bg-primary me-auto" onClick={handleChooseCurrentFlight}>Choose current flight</button>
          <button className="btn bg-primary" onClick={handleCreateNewFlightForm}>Create New</button>
        </div>
        {getForm()}
      </div>
    </main>);
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
            <li className="nav-item mx-2 hidden">
              <OverlayTrigger
                trigger="click"
                placement="bottom"
                show={showPopoverPlans}
                onToggle={(nextShow) => setShowPopoverPlans(nextShow)}
                overlay={popoverPlans}
                rootClose={true}
              >
                <Button className="btn flight-plans" variant="transparent">
                  <img src={flightPlans} width="28" alt="Flight Plans" />
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
              <a href="#" className="nav-link" alt="Docs" title="Docs">
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
}

export default NavHeader;