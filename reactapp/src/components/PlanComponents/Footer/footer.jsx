import fgLogo from '../../../../src/imgs/FlightGear_logo.ico'

const Footer = () => {
    return (<div class="container-fluid px-0">
    <footer class="d-flex flex-wrap justify-content-between align-items-center border-top bg-light">
      <p class="col-md-4 mb-0 text-muted">© 2021 Company, Inc</p>
  
      <a href="/" class="col-md-4 d-flex align-items-center justify-content-center mb-3 mb-md-0 me-md-auto link-dark text-decoration-none">
        <img src={fgLogo} className={`col-1`}></img>
      </a>
  
      <ul class="nav col-md-4 justify-content-end">
        <li class="nav-item"><a href="#" class="nav-link px-2 text-muted">Home</a></li>
        <li class="nav-item"><a href="#" class="nav-link px-2 text-muted">Features</a></li>
        <li class="nav-item"><a href="#" class="nav-link px-2 text-muted">FAQs</a></li>
        <li class="nav-item"><a href="#" class="nav-link px-2 text-muted">About</a></li>
      </ul>
    </footer>
  </div>)
}

export default Footer;