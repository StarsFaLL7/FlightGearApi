import './Plan.css';
import MainApp from '../../components/PlanComponents/PlanTable/PlanTable';
import NavToAnalyze from '../../components/PlanComponents/navToAnalyze/navToAnalyze';

function Plan() {
  return (
    <div className="Plan">
      <MainApp />
      <NavToAnalyze />
    </div>
  );
}

export default Plan;
