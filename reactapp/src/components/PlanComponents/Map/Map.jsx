import React, { useRef, useEffect, useState, useContext } from 'react';
import maplibregl from 'maplibre-gl';
import 'maplibre-gl/dist/maplibre-gl.css';
import '../Map/Map.css';
import '@mapbox/mapbox-gl-draw/dist/mapbox-gl-draw.css';
import * as turf from '@turf/turf';
import { getPlanData } from "../../../api-methods/api-methods";
import { handlerAddPoint } from "../../../utils/common";
import { getData } from "../../../utils/common";
import { handleClickDeleteItem } from '../../../api-methods/api-methods';
import { PointContext } from '../context/main-context';
import { putPointsData } from '../../../api-methods/api-methods';
import PopupLoad from '../../AnalyzeComponents/popup/popup'

const MainMap = () => {
  const mapContainer = useRef(null);
  const map = useRef(null);
  const [lng] = useState(0);
  const [lat] = useState(0);
  const [zoom] = useState(0);

  const [sendingPointData, setSendingPointData] = useState([]);
  const { points, addPoint, setPoints, currentFlight, changePointData, fetchPoints } = useContext(PointContext);

  useEffect(() => {
    if (map.current) return;

    map.current = new maplibregl.Map({
      container: mapContainer.current,
      style: 'https://api.maptiler.com/maps/basic-v2/style.json?key=R2mBAn4LWE1EZYkPEhdD',
      center: [lng, lat],
      zoom: zoom,
      attributionControl: false,
    });

    addControlPanel(map.current);
    getMousePosition(map.current);

  }, [lng, lat, zoom]);

  useEffect(() => {
    if (!map.current || !currentFlight) return;

    clearMarkersAndLines(map.current);
    loadMarkersToMap(map.current);

    // Ensure the event listener is added only once
    const handleContextMenu = (e) => {
      if (!currentFlight) return;

      let lngLat = Object.values(e.lngLat);
      const marker = createMarker(lngLat, map.current);
      let popup = createPopup(e, map.current, marker);
      marker.setPopup(popup);

      let formData = getData(marker.getPopup().addTo(map.current).getElement().querySelector('form'));
      formData = {...formData, altitude: Number(formData.altitude), longitude: Number(formData.longitude), latitude: Number(formData.latitude)}
      marker.getPopup().remove();
      addPoint(formData, sendingPointData, setSendingPointData, currentFlight);
    };

    map.current.on('contextmenu', handleContextMenu);

    return () => {
      map.current.off('contextmenu', handleContextMenu);
    };

  }, [currentFlight, points]);

  function clearMarkersAndLines(map) {
    const markers = document.getElementsByClassName('maplibregl-marker');
    while (markers.length) {
      markers[0].remove();
    }

    if (map.getLayer('line')) {
      map.removeLayer('line');
    }
    if (map.getSource('line')) {
      map.removeSource('line');
    }
  }

  function loadMarkersToMap(map) {
    if (points && points.routePoints) {
      points.routePoints.forEach(point => {
        const markerOptions = {
          color: "#0d6efd",
          draggable: point.isEditable || false
        };

        const marker = new maplibregl.Marker(markerOptions)
          .setLngLat([point.longitude, point.latitude])
          .addTo(map);
        let popup = createPopup(point, map, marker);
        marker.setPopup(popup);

        marker.on('dragend', () => {
          let newLngLat = Object.values(marker.getLngLat());
          const index = points.routePoints.findIndex(coord => coord.longitude === point.longitude && coord.latitude === point.latitude);
          if (index !== -1) {
            points.routePoints[index] = { ...points.routePoints[index], longitude: newLngLat[0], latitude: newLngLat[1] };
            setPoints({ routePoints: [...points.routePoints] });
          }
          updateLine(map, points.routePoints);
        });
      });
      updateLine(map, points.routePoints);
    }
  }

  function addControlPanel(map) {
    map.addControl(new maplibregl.NavigationControl(), 'bottom-right');
  }

  function updateLine(map, coordinates) {
    const coords = coordinates.map(coord => [coord.longitude, coord.latitude]);
    if (map.getSource('line')) {
      map.getSource('line').setData({
        type: 'Feature',
        geometry: {
          type: 'LineString',
          coordinates: coords,
        },
      });
    } else {
      map.addSource('line', {
        type: 'geojson',
        data: {
          type: 'Feature',
          geometry: {
            type: 'LineString',
            coordinates: coords,
          },
        },
      });

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
        },
      });
    }
  }

  function createMarker(lngLat, map) {
    return new maplibregl.Marker({ draggable: true, color: "#0d6efd" })
      .setLngLat(lngLat)
      .addTo(map);
  }

  function createPopup(data, map, marker) {
    let popupContent = document.createElement('div');

    popupContent.innerHTML = `
    <form class='w-100 bg-light rounded-4' id="form" method='POST' enctype="application/json">
      <ul class='w-100 list-unstyled justify-items-center'>
        <li class='popover-item justify-self-center px-2'>
          <p class='fs-6 form-control'>Longitude: ${data.lngLat !== undefined ? data.lngLat.lng : data.longitude}</p>
          <input class='hidden form-control ms-auto' value='${data.lngLat !== undefined ? data.lngLat.lng : data.longitude}' type="number" name="longitude" required/>
        </li>
        <li class='popover-item d-flex align-items-center px-2'>
          <p class='fs-6 form-control'>Latitude: ${data.lngLat !== undefined ? data.lngLat.lat : data.latitude}</p>
          <input class='hidden form-control ms-auto' value='${data.lngLat !== undefined ? data.lngLat.lat : data.latitude}' type="number" name="latitude" required/>
        </li>
        <li class='popover-item form-control d-flex align-items-center mb-3'>
          <p class='fs-6 pb-0 mb-0'>Altitude(m):</p>
          <input class='form-control ms-auto' type="number" name="altitude" ${data.isEditable ? '' : 'readonly'} value="${data.altitude !== undefined ? Number(data.altitude) : 500}" required/>
        </li>
        <li class='popover-item form-control d-flex align-items-center mb-3'>
          <p class='fs-6 pb-0 mb-0'>Remarks:</p>
          <textarea class='form-control ms-auto' type="text" ${data.isEditable ? '' : 'readonly'} name="remarks">${data.remarks !== undefined ? data.remarks : ""}</textarea>
        </li> 
        <li>
          <button class="btn save-popup btn-primary text-light" type="submit">
            Save
          </button>
          <button class="btn delete-popup btn-secondary text-light" type="button">
            Delete
          </button>
        </li>
      </ul> 
    </form>`;

    const saveButton = popupContent.querySelector('.save-popup');
    const deleteButton = popupContent.querySelector('.delete-popup');

    deleteButton.onclick = function (evt) {
      evt.preventDefault();
      marker.remove();
      setPoints(prevPoints => {
        const updatedPoints = { ...prevPoints };
        const index = updatedPoints.routePoints.findIndex(coord => coord.longitude === (data.lngLat !== undefined ? data.lngLat.lng : data.longitude) && coord.latitude === (data.lngLat !== undefined ? data.lngLat.lat : data.latitude));
        if (index !== -1) {
          updatedPoints.routePoints.splice(index, 1);
        }
        updateLine(map, updatedPoints.routePoints);
        return updatedPoints;
      });
    };

    saveButton.onclick = function (evt) {
      evt.preventDefault();
      console.log(evt)
      const formData = getData(popupContent.querySelector('form'));
      console.log(formData);
      changePointData()
      // handlerAddPoint(formData, points, setPoints, sendingPointData, setSendingPointData);
    };

    let popup = new maplibregl.Popup().setLngLat([data.lngLat !== undefined ? data.lngLat.lng : data.longitude, data.lngLat !== undefined ? data.lngLat.lat : data.latitude]).setDOMContent(popupContent);
    return popup;
  }

  function getMousePosition(map) {
    map.on('mousemove', (e) => {
      document.getElementById('info').innerHTML = 
        `<p>Lng: ${e.lngLat.lng}</p>
         <p>Lat: ${e.lngLat.lat}</p>`;
    });
  }

  return (
    <div className={`map-wrap bg-light`}>
      <div ref={mapContainer} className={`map bg-light`} />
      <div id={`info`}></div>
      <div id={`calculated-area`}></div>
    </div>
  );
}

export default MainMap;