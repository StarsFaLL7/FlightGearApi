import React, { useState, useEffect } from 'react';
import { ResponsiveContainer, LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend } from 'recharts';
// import dataChart1 from '../../../assets/response11.json';
// import dataChart2 from '../../../assets/response22.json';
import { useSelector } from 'react-redux';

// Компонент-график
const ChartComponent = () => {
  // Состояния для данных и статуса загрузки
  const [data, setData] = useState([]);
  const datasChart = useSelector((state) => state.chart.data);
  const isReloading = useSelector((state) => state.chart.isReloading);
  const currentSession = useSelector((state) => state.chart.currentSession);
  
  //Тест
  // const dataChart = currentSession === 1 ? dataChart1: dataChart2;

  const filteredDataChart = data.filter((s) => datasChart.includes(s.name));
  const route = `https://localhost:7110/api/analytics/sessions/${currentSession}/values`;
  // Загрузка данных из API
  useEffect(() => {
    fetch(route)
       .then(response => response.json())
       .then(data => setData(data));
  }, [isReloading, route]);
 
  // Массив цветов
  const COLORS = [
    '#00008B', 
    '#000000', 
    '#123456', 
    '#654321',
    '#ccc000', 
    '#FF0000', 
    '#FFD700', 
    '#8B008B', 
    '#4B0082', 
    '#2F4F4F', 
    '#006400', 
    '#FF1493', 
    '#FF8C00', 
    '#483D8B', 
    '#808080'
  ];
 
  // Функция генерации случайного цвета
  function getRandomInt(min, max) {
     let rand = min + Math.random() * (max + 1 - min);
     return Math.floor(rand);
  }

  // console.log('datachart', filteredDataChart)
 
  // Отрисовка
  return <>
       <ResponsiveContainer width="100%" height="100%">
         <LineChart
           data={data}
           margin={{ top: 10, right: 30, left: 20, bottom: 55 }}
         >
           {/* Ось X */}
           <XAxis dataKey="id" type='category' stroke='white' allowDuplicatedCategory={false} />
 
           {/* Ось Y */}
           <YAxis dataKey="value" stroke='white' />
 
           {/* Сетка */}
           <CartesianGrid strokeDasharray="0 0" />
 
           {/* Подсказка при наведении */}
           <Tooltip />
 
           {/* Легенда графика */}
           <Legend />
 
           {/* Элементы линий */}
           {filteredDataChart.map((s) => {
          
              return ( <Line
               type="monotone"
               isAnimationActive={false}
               dot={false}
               dataKey="value"
               data={s.data}
               name={s.name}
               key={s.name}
               stroke={COLORS[getRandomInt(0, COLORS.length - 1)]}
               strokeWidth={5}
             />)
            
           })
            
           }
 
           {/* Отрисовка дополнительных данных */}
         </LineChart>
       </ResponsiveContainer>
     </>
 }
 
 // Экспорт компонента
 export default ChartComponent;