import './App.css';
import Table from './components/table/Table';
import ChartComponent from './components/chart/chart';

function App() {
  return <>
  <div className="App">
      
      <aside>
        <Table />
      </aside>
      <main>
        <ChartComponent />
      </main>
    </div>
  </>;
}

export default App;
