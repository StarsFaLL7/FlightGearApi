import './Plan.css';
import NavHeader from '../../components/PlanComponents/NavItem/NavItem';
import "../../../node_modules/bootstrap/dist/css/bootstrap.css";
import "../../../node_modules/bootstrap/dist/css/bootstrap.css";
import MainMap from '../../components/PlanComponents/Map/Map';
import { PointsContext } from '../../components/PlanComponents/context/main-context';
/* import '../../../node_modules/bootstrap/dist/js/bootstrap.bundle'; */
function Plan() {
  return (
    <PointsContext>
      <div className={`Plan`}>
        <NavHeader/>
        <MainMap/>
      </div>
    </PointsContext>
  );
}

export default Plan;
