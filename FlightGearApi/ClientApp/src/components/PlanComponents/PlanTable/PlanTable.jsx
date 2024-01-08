import React, {useState, useEffect} from "react";
import PlanItem from "../PlanItem/PlanItem";
import plus from '../../../assets/img/Union.png'
import styles from './PlanTable.module.css';
import axios from "axios";


const MainApp = () => {
  const [plan, setPlan] = useState([]);
  const [sendingData, setSendingData] = useState([]);
  const SERVER_URL = "https://localhost:7110/api/launch/stages";

  const handleClick = (evt) => {
    evt.preventDefault();
    const formData = getData(document.getElementById('form'));
    handlerAddPlan(formData);
  };

  useEffect(() => {
    fetchPlanData();
  }, []);

  const getData = (form) => {
    const dataForm = new FormData(form);
    const data = {};
    for (let [name, value] of dataForm){
      data[name] = value;
    }
    console.log(data);
    return data;
  }
  const clearForm = () => {
    const form = document.getElementById('form');

    form.querySelector('input[name=heading]').value = '';
    form.querySelector('input[name=speed]').value = '';
    form.querySelector('input[name=altitude]').value = '';
  }

  const handlerAddPlan = async (formData) => {
    if (!formData.heading || !formData.speed || !formData.altitude) { return; }

    // Construct new plan item
    const newPlan = { id: plan.length, ...formData };

    // Update the state and log the plan items
    setPlan((prevPlan) => {
      const updatedPlan = [...prevPlan, newPlan];
      logPlanItemData(updatedPlan); // Log the updated plan items
      return updatedPlan;
    });

    try {
      // Asynchronously send new plan to server
      await sendDataToServer(newPlan);
      // Here you might want to do something with the response
    } catch (error) {
      // Handle any errors that occur during the fetch
      console.error('There was an error sending the data to the server:', error);
    }
    clearForm();
  };

  const logPlanItemData = (updatedPlan) => {
    console.log(updatedPlan);
  };

  const sendDataToServer = async (body) => {
    // Параметр функции - data
    // await fetch(SERVER_URL, {
    //   method: 'POST',
    //   headers: {
    //     'Content-Type': 'application/json',
    //   },
    //   body: JSON.stringify(data)
    // })
    //   .then((response) => {
    //     if (!response.ok) {
    //       throw new Error(`HTTP error! status: ${response.status}`);
    //     }
    //   })
    //   .then((response) => setSendingData(response.json()));

      await axios({
        method: 'post',
        url: SERVER_URL,
        data: body
      })
        .then((response) => {
          if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
          }
        })
        .then((response) => setSendingData(response.json()));

    return sendingData;
  };

  const fetchPlanData = async () => {
    // await fetch(SERVER_URL, {
    //   method: 'GET',
    //   headers: {
    //     'Content-Type': 'application/json',
    //   },
    // })
    //   .then((response) => {
    //     if (!response.ok) {
    //       throw new Error(`HTTP error! status: ${response.status}`);
    //     }
    //   })
    //   .then((response) => setPlan(response.json()))
    //   .catch((err) => console.error('There was an error fetching the data:', err));
    try {
      const result = await axios.get(SERVER_URL);
      console.log('Plan result.data = ', result.data)
      setPlan(result.data)
    } catch (err) {
      console.error('There was an error fetching the data:', err)
    }
  };
  const onRemoveData = async () => {
    await fetchPlanData();
  }
  return (
    <>
      <header>
      <h1 className={styles.title}>Создайте свою симуляцию полета</h1>
        <form className={styles.add_stage} id="form" method='POST' enctype="application/json"> 
          <div className={styles.add_stage_element_1}>
            <p>Курс:</p>
            <input type="text" className={styles.course} name="heading" required/>
          </div>
          <div className={styles.add_stage_element}>
            <p className={styles.speed_label}>Расчётная скорость полета (м/с):</p>
            <input type="number" className={styles.speed} step="0.01" name="speed" required/>
          </div>
          <div className={styles.add_stage_element_3}>
            <p className={styles.altitude_label}>Высота над уровнем моря:</p>
            <input type="number" className={styles.altitude} name="altitude" required/>
          </div>
          <div className={styles.add_stage_element_4}>
            <button onClick={(evt) => handleClick(evt)} type="submit" className={styles.plus}><img src={plus} alt="Union"/></button>
          </div>
        </form>
    </header>
    <main className={styles.main_info}>
      <iframe className={styles.map} src="https://www.google.com/maps/embed?pb=!1m14!1m12!1m3!1d69934.32592146858!2d60.723338649999995!3d56.78678595!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!5e0!3m2!1sru!2sru!4v1700587609013!5m2!1sru!2sru"  allowfullscreen="" loading="lazy" referrerpolicy="no-referrer-when-downgrade"></iframe>
      <div className={styles.plan_table}>
        <table>
          <thead>
            <div className={styles.table_head}>
              <h2>Выбранные этапы полёта</h2>
              <div className={styles.table_head_info}>
                <tr className={styles.table_element}>
                  <th className={styles.table_element_info_2}>Курс</th>
                  <th className={styles.table_element_info_3}>Расчётная скорость</th>
                  <th className={styles.table_element_info_4}>Высота над уровнем моря</th>
                </tr>
              </div>
            </div>
          </thead>
          <tbody>
            <div className={styles.scroll}>
            {plan && plan.map((element, index) =>
              <PlanItem
                //key={element.id}
                //id={element.id}

                index={index}
                heading={element.heading}
                speed={element.speed}
                altitude={element.altitude}
                onRemoveData={onRemoveData}
              />
            )} 
            </div>
          </tbody>
        </table>
      </div>
    </main>     
    </>
  ) 
}

export default MainApp;