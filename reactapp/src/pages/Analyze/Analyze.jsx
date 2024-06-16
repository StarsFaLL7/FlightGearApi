import './Analyze.css';
import Table from '../../components/AnalyzeComponents/table/Table';
import ChartComponent from '../../components/AnalyzeComponents/chart/chart';
import PopupLoad from '../../components/AnalyzeComponents/popup/popup';
import RefreshButtonComponent from '../../components/AnalyzeComponents/refreshButton/refreshButton';
import ChangeSession from '../../components/AnalyzeComponents/changeSession/changeSession';
import NavHeader from '../../components/PlanComponents/NavItem/NavItem';
import "../../../node_modules/bootstrap/dist/css/bootstrap.css";
import { PointsContext } from '../../components/PlanComponents/context/main-context';

function Analyze() {
  return <>
  <PointsContext>
    <div className={`header`}>
      <NavHeader/>
    </div>
    <div className="Analyze ">
      <div className='body'>
        <div className='dark__content'>
          <PopupLoad />
        </div>
        <aside>
          <Table />
        </aside>
        <main className='main__analyze'>
          <div className='buttons'>
            <ChangeSession />
            <RefreshButtonComponent />
          </div>
          <ChartComponent />
          </main>
        </div>
      </div>
    </PointsContext>
  </>;
}

export default Analyze;
