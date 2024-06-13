import './Table.module.css';
import React, { useState, useEffect } from 'react';
import Item from '../tableItem/item';
import { useSelector } from 'react-redux';
import axios from "axios";

function Table() {
    const [dataParams, setDataParams] = useState([]);
    const currentSession = useSelector((state) => state.chart.currentSession);
    const route = `https://localhost:7229/api/analytics/sessions/${currentSession}`;
    console.log(route);
    
    useEffect(() => {
        async function fetchData() {
            // You can await here
            const result =  await axios(route);
            console.log("Table result.Data = ", result.data);
            setDataParams(result.data);
        }
        fetchData();
    }, [route, currentSession]);

    console.log(dataParams.properties)
    return <table>
        <thead>
            <tr>
                <th>Параметры</th>  
            </tr>
        </thead>
        <tbody>
            {dataParams.properties && dataParams.properties.map((par) => <Item name={par.name} count={par.data.length}/>)}
        </tbody>
    </table>
};

export default Table;