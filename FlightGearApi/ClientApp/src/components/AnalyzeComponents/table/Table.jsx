import './Table.module.css';
import React, { useState, useEffect } from 'react';
import Item from '../tableItem/item';
import dataTable1 from '../../../assets/response1';
import dataTable2 from '../../../assets/response2';
import { useSelector } from 'react-redux';

function Table() {
    const [dataParams, setDataParams] = useState([]);
    const currentSession = useSelector((state) => state.chart.currentSession);
    const route = `https://localhost:7110/api/analytics/sessions/${currentSession}`;
    console.log(route);
    useEffect(() => {
        fetch(route).then(data => console.log(data)).then(d => setDataParams(d))
    }, [route]);

    //Test
    const dataTable = currentSession===1 ? dataTable1: dataTable2;
    
    return <table>
        <thead>
            <tr>
                <th>Параметры</th>  
            </tr>
        </thead>
        <tbody>
        {dataTable.map((par) => <Item name={par.name} count={par.count}/>)
        }
        {dataParams}
        </tbody>
    </table>
};

export default Table;