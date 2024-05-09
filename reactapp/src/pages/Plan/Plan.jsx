import './Plan.css';
import MainApp from '../../components/PlanComponents/PlanTable/PlanTable';
import NavHeader from '../../components/PlanComponents/NavItem/NavItem';
import "../../../node_modules/bootstrap/dist/css/bootstrap.css";
import "../../../node_modules/bootstrap/dist/css/bootstrap.css";
import Analyze from '../Analyze/Analyze';
import Footer from '../../components/PlanComponents/Footer/footer';
import MainMap from '../../components/PlanComponents/Map/Map';
/* import '../../../node_modules/bootstrap/dist/js/bootstrap.bundle'; */
function Plan() {
  return (
    <div className={`Plan`}>
      <NavHeader/>
      <MainMap/>
      {/* <Footer/> */}
    </div>
  );
}

export default Plan;
