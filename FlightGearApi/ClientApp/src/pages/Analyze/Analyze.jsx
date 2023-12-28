import './Analyze.css';
import Table from '../../components/AnalyzeComponents/table/Table';
import ChartComponent from '../../components/AnalyzeComponents/chart/chart';
import PopupLoad from '../../components/AnalyzeComponents/popup/popup';
import RefreshButtonComponent from '../../components/AnalyzeComponents/refreshButton/refreshButton';
import ChangeSession from '../../components/AnalyzeComponents/changeSession/changeSession';
import NavToPlan from '../../components/AnalyzeComponents/navToPlan/navToPlan';

function Analyze() {
  return <>
  <div className="Analyze">
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
      <NavToPlan />
    </div>
  </>;
}

export default Analyze;
