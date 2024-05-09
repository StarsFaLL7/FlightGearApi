import React, { useState } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import { OverlayTrigger, Button, Popover } from "react-bootstrap";
import "../NavItem/NavItem.css"
import MainApp from '../PlanTable/PlanTable';
import PlanPoints from "../PlanPoints/plan-points";

import plane from '../NavItem/imgs/plane.png';
import analytics from '../NavItem/imgs/analytics.png';
import docs from '../NavItem/imgs/docs.png';
import flightPlans from '../NavItem/imgs/flight-plans.png';
import flightPoints from '../NavItem/imgs/flight-stages.png';

const NavHeader = () => {
  const [showPopoverPoints, setShowPopoverPoints] = useState(false);
  const [showPopoverPlans, setShowPopoverPlans] = useState(false);

  const popoverPoints = (
    <Popover id="popover-basic">
      <Popover.Header as="h3">Points</Popover.Header>
      <Popover.Body>
        <PlanPoints/>
      </Popover.Body>
    </Popover>
  );

  const popoverPlans = (
    <Popover id="popover-basic">
      <Popover.Header as="h3">All Flights</Popover.Header>
      <Popover.Body>
        <MainApp/>
      </Popover.Body>
    </Popover>
  );

  return (
    <header className="container-fluid bg-not-light">
      <div>
        <nav className="d-flex flex-row justify-content-between">
        <ul className="nav nav-pills align-items-center">
            <li className="nav-item mx-2">
              <OverlayTrigger
                trigger="click"
                placement="bottom"
                show={showPopoverPoints}
                onToggle={(nextShow) => setShowPopoverPoints(nextShow)}
                overlay={popoverPoints}
                rootClose={true}
              >
                <Button className="btn flight-points" variant="transparent" >
                  <img src={flightPoints} width="50" alt="Flight Points"/>
                </Button>
              </OverlayTrigger>
            </li>
            <li className="nav-item mx-2">
            <OverlayTrigger
                trigger="click"
                placement="bottom"
                show={showPopoverPlans}
                onToggle={(nextShow) => setShowPopoverPlans(nextShow)}
                overlay={popoverPlans}
                rootClose={true}
              >
                <Button className="btn flight-plans" variant="transparent">
                  <img src={flightPlans} width="28" alt="Flight Plans"/>
                </Button>
              </OverlayTrigger>
            </li>
          </ul>
          <ul className="nav nav-pills">
            <li className="nav-item mx-2">
              <a href="/" className="nav-link" aria-current="page" alt="Planner" title="Planner">
                <img src={plane} width="40" alt="Planner"/>
              </a>
            </li>
            <li className="nav-item mx-2">
              <a href="#" className="nav-link" alt="Docs" title="Docs">
                <img src={docs} width="40" alt="Docs"/>
              </a>
            </li>
            <li className="nav-item mx-2">
              <a href="/analyze" className="nav-link" alt="Analytics" title="Analytics">
                <img src={analytics} width="40" alt="Analytics"/>
              </a>
            </li>
          </ul>
        </nav>
      </div>
    </header>
  );
}

export default NavHeader;