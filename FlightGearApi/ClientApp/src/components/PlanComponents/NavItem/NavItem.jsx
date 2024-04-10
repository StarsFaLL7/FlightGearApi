import React from "react";
import "../../../../node_modules/bootstrap/dist/css/bootstrap.css";
import "../NavItem/NavItem.css";

import plane from '../NavItem/imgs/plane.png';
import analytics from '../NavItem/imgs/analytics.png';
import docs from '../NavItem/imgs/docs.png';
import flights from '../NavItem/imgs/flights.png';

//<button class="btn btn-primary btn-hover" onClick={(evt) => handleClick(evt)} type="submit"><img src={plus} alt="Union"/></button>
const NavHeader = () => {

    return (
      <>
        <header className={`container-fluid bg-light`} >
          <div>
            <nav class="d-flex justify-content-center">
              <ul class="nav nav-pills justify-content-center">
                <li class="nav-item">
                  <a href="" class="nav-link" aria-current="page">
                    <img src={plane} width="45"/>
                    <figcaption>Planner</figcaption>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="#" class="nav-link">
                    <img src={docs} width="45"/>
                    <figcaption>Docs</figcaption>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="/analyze" class="nav-link">
                    <img src={analytics} width="45"/>
                    <figcaption>Analytics</figcaption>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="/analyze" class="nav-link">
                    <img src={flights} width="45"/>
                    <figcaption>Flights</figcaption>
                  </a>
                </li>
              </ul>
            </nav>
          </div>
        </header>
      </>
    ) 
  }
  
  export default NavHeader;