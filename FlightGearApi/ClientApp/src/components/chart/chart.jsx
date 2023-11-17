import React, { useState, useEffect } from 'react';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend } from 'recharts';
import './chart.module.css';

const ChartComponent = () => {
  const [data, setData] = useState([]);

  useEffect(() => {
    fetch('https://api.example.com/data')
      .then(response => response.json())
      .then(data => setData(data));
  }, []);

  const paramsArray = [
    {
        name: 'Тангаж',
        count: '13962',
        value: 123
    },
    {
        name: 'Mангаж',
        count: '13962',
        value: [12,30,20, 61,79,20]
    },
    {
        name: 'Гараж',
        count: '13962',
        value: [12,30,20, 61,79,20]
    }
    ]

  return <>
    <LineChart
      width={500}
      height={300}
      data={paramsArray}
      margin={{ top: 5, right: 30, left: 20, bottom: 5 }}
    >
      <XAxis dataKey="name" />
      <YAxis />
      <CartesianGrid strokeDasharray="3 3" />
      <Tooltip />
      <Legend />
      <Line type="monotone" dataKey={(paramsArray[0].value).toString()} stroke="#8884d8" activeDot={{ r: 8 }} />
      {paramsArray.map((par) => {<Line type="monotone" dataKey={par.value.toString} stroke="#8884d8" activeDot={{ r: 8 }} />})}
      {data}
    </LineChart>
    </>;
}

export default ChartComponent;