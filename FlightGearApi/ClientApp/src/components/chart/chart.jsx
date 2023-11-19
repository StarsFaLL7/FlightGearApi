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

  const test = [{
    name: 'dsasha',
    uv: 400, pv: 2400, amt: 1200
  },
  {
    name: 'dsasDE',
    uv: 600, pv: 2367, amt: 1240
  },
  {
    name: 'dsDEDha',
    uv: 800, pv: 3421, amt: 1320
  }]

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
      data={test}
      margin={{ top: 5, right: 30, left: 20, bottom: 5 }}
    >
      <XAxis dataKey="name" />
      <YAxis />
      <CartesianGrid strokeDasharray="3 3" />
      <Tooltip />
      <Legend />
      <Line type="monotone" dataKey="pv" stroke="#8884d8" activeDot={{ r: 8 }} />
      <Line type="monotone" dataKey="uv" stroke="#8884d8" activeDot={{ r: 8 }} />
      {data}
    </LineChart>
    </>;
}

export default ChartComponent;