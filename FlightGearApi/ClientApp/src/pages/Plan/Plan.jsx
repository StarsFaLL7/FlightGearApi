import './Plan.css';
import MainApp from '../../components/PlanComponents/PlanTable/PlanTable';
import NavHeader from '../../components/PlanComponents/NavItem/NavItem';
import "../../../node_modules/bootstrap/dist/css/bootstrap.css";
import '../../../node_modules/bootstrap/dist/js/bootstrap.bundle';

function Plan() {
  return (
    <div className={`Plan`}>
      <NavHeader />
      <MainApp />
    </div>
  );
}

export default Plan;
