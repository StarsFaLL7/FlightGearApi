import React, { useRef, useEffect, useState } from 'react';
import maplibregl, { LngLat } from 'maplibre-gl';
import 'maplibre-gl/dist/maplibre-gl.css';
import '../Map/Map.css';
import MapboxDraw from "@mapbox/mapbox-gl-draw";
import '@mapbox/mapbox-gl-draw/dist/mapbox-gl-draw.css'
import * as turf from '@turf/turf'
import { getPlanData } from "../../../api-methods/api-methods";
import { handlerAddPlan } from "../../../utils/common";
import { getData } from "../../../utils/common";
import { handleClickDeleteItem } from '../../../api-methods/api-methods';

//import {addMarker, mapUtils, addControlPanel, getMousePosition} from './map-functions';
//shadow-lg
//rounded-4
const MainMap = () => {
  //const [point, setPoint] = useState([])
  const [plan, setPlan] = useState([]);
  const [sendingData, setSendingData] = useState([]);

  useEffect(() => { getPlanData(setPlan); }, []);
  const onRemoveData = async () => { await getPlanData(setPlan); }

  const mapContainer = useRef(null);
  const map = useRef(null);
  const [lng] = useState(0);
  const [lat] = useState(0);
  const [zoom] = useState(0);
  //const [API_KEY] = useState('YOUR_MAPTILER_API_KEY_HERE');
  let markersArr = [];

  function addControlPanel(map) {
    map.addControl(new maplibregl.NavigationControl(), 'bottom-right');
  }

  function addMarker(map) {

    map.on('contextmenu', (e) => {
        let lngLat = Object.values(e.lngLat);
        markersArr.push(lngLat);
        const marker = createMarker(lngLat, map);
        let popup = createPopup(e, map, marker, markersArr);
        marker.setPopup(popup);
        marker.on('dragend', () => {
            let newLngLat = Object.values(marker.getLngLat());
            e.lngLat = marker.getLngLat();
            let index = markersArr.indexOf(lngLat);
            markersArr[index] = newLngLat;
            lngLat = newLngLat;
            updateLine(map, markersArr);
            popup = createPopup(e, map, marker, markersArr);
            marker.setPopup(popup);
        });
        updateLine(map, markersArr);
        const formData = getData(marker.getPopup().getElement().querySelector('form'));
        handlerAddPlan(formData, plan, setPlan, sendingData, setSendingData);
    });
}

  function updateLine(map, coordinates) {
      if (map.getSource('line')) {
          map.getSource('line').setData({
              type: 'Feature',
              geometry: {
                  type: 'LineString',
                  coordinates: coordinates
              }
          });
      } else {
          // Если источник данных линии не существует, добавляем его
          map.addSource('line', {
              type: 'geojson',
              data: {
                  type: 'Feature',
                  geometry: {
                      type: 'LineString',
                      coordinates: coordinates
                  }
              }
          });

          // Добавляем слой линии на карту
          map.addLayer({
              id: 'line',
              type: 'line',
              source: 'line',
              layout: {
                  'line-join': 'round',
                  'line-cap': 'round',
              },
              paint: {
                  'line-color': '#0000FF',
                  'line-width': 5,
                  'line-opacity': 0.5,
              }
          });

      }   
  }

  function createMarker(lngLat, map) {
      return new maplibregl.Marker({draggable: true, color: "#0d6efd" })
          .setLngLat(lngLat)
          .addTo(map);
  }

  function createPopup(data, map, marker, markersArr) {

    let markArr = markersArr;
    let popupContent = document.createElement('div');

    popupContent.innerHTML = `
      <form class='bg-light rounded-4' id="form" method='POST' enctype="application/json">
        <ul class='list-unstyled'>
          <li class='d-flex align-items-center px-2'>
            <p class='fs-6 form-control ms-auto'>Longitude: ${data.lngLat.lng}</p>
            <input class='hidden form-control ms-auto' value='${data.lngLat.lng}' type="number" name="longitude" required/>
          </li>
          <li class='d-flex align-items-center px-2'>
            <p class='fs-6 form-control ms-auto'>Latitude: ${data.lngLat.lat}</p>
            <input class='hidden form-control ms-auto' value='${data.lngLat.lat}' type="number" name="latitude" required/>
          </li>
          <li class='form-control d-flex align-items-center mb-3'>
            <p class='fs-6 pb-0 mb-0'>Speed(m/s):</p>
            <input class='form-control ms-auto' type="number" step="0.01" name="speed" value="0" required/>
          </li> 
          <li class='form-control d-flex align-items-center mb-3'>
            <p class='fs-6 pb-0 mb-0'>Altitude(m):</p>
            <input class='form-control ms-auto' type="number" name="altitude" value="0" required/>
          </li>
          <li>
            <button class="btn save-popup btn-primary text-light" type="submit">
              save
            </button>
            <button class="btn delete-popup btn-secondary text-light" type="submit">
              delete
            </button>
          </li>
        </ul> 
      </form>`;

      const saveButton = popupContent.querySelector('.save-popup');
      const deleteButton = popupContent.querySelector('.delete-popup');

      deleteButton.onclick = function() {
          marker.remove();
          for(let i = 0; i < markArr.length; i++) {
              if(markArr[i].toString() === marker.getLngLat().toArray().toString()){
                markersArr.splice(i, 1);;
              }
          }
          updateLine(map, markersArr);
      };

      saveButton.onclick = function(evt) {
        evt.preventDefault();
        const formData = getData(document.getElementById('form'));
        console.log(document.getElementById('form'))
        handlerAddPlan(formData, plan, setPlan, sendingData, setSendingData);  
      };

      let popup = new maplibregl.Popup().setLngLat([data.lngLat.lng, data.lngLat.lat]).setDOMContent(popupContent).addTo(map);
      return popup;
  }

  function getMousePosition(map) {
      map.on('mousemove', (e) => {
          document.getElementById('info').innerHTML = 
          `<p>Lng: ${e.lngLat.lng}</p>
          <p>Lat: ${e.lngLat.lat}</p>`;
      });
  }    

  function mapUtils(map) {
      MapboxDraw.constants.classes.CONTROL_BASE  = 'maplibregl-ctrl';
      MapboxDraw.constants.classes.CONTROL_PREFIX = 'maplibregl-ctrl-';
      MapboxDraw.constants.classes.CONTROL_GROUP = 'maplibregl-ctrl-group';

      const draw = new MapboxDraw({
          displayControlsDefault: true,
          controls: {
              polygon: true,
              trash: true
          }
      });

      map.addControl(draw);

      map.on('draw.create', updateArea);
      map.on('draw.delete', updateArea);
      map.on('draw.update', updateArea);

      function updateArea(e) {
          const data = draw.getAll();
          const answer = document.getElementById('calculated-area');
          if (data.features.length > 0) {
              const area = turf.area(data);
              // restrict to area to 2 decimal points
              const roundedArea = Math.round(area * 100) / 100;
              answer.innerHTML =
                  `<p><strong>${
                      roundedArea
                  }</strong></p><p>square meters</p>`;
          } else {
              answer.innerHTML = '';
              if (e.type !== 'draw.delete')
                  alert('Use the draw tools to draw a polygon!');
          }
      }
  }

  useEffect(() => {
    if (map.current) return;

    map.current = new maplibregl.Map({
      container: mapContainer.current,
      style: 'https://api.maptiler.com/maps/basic-v2/style.json?key=R2mBAn4LWE1EZYkPEhdD',
      center: [lng, lat],
      zoom: zoom,
      attributionControl: false,
    });

    //mapUtils(map.current)
    addControlPanel(map.current);
    getMousePosition(map.current)
    addMarker(map.current);
    
  }, [ lng, lat, zoom]);

  return (
    <div className={`map-wrap bg-light`}>
      <div ref={mapContainer} className={`map bg-light`}/>
      <div id={`info`}></div>
      <div id={`calculated-area`}></div>
    </div>
  
  );
}

export default MainMap;