import './Plan.css';
import NavHeader from '../../components/PlanComponents/NavItem/NavItem';
import "../../../node_modules/bootstrap/dist/css/bootstrap.css";
import "../../../node_modules/bootstrap/dist/css/bootstrap.css";
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
