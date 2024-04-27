import React, { useRef, useEffect, useState } from 'react';
import maplibregl from 'maplibre-gl';
import 'maplibre-gl/dist/maplibre-gl.css';
import '../Map/Map.css';
import {addMarker, addControlPanel} from './map-functions';
import { mapUtils } from './map-functions';
//shadow-lg
//rounded-4
export default function MainMap() {
  const mapContainer = useRef(null);
  const map = useRef(null);
  const [lng] = useState(0);
  const [lat] = useState(0);
  const [zoom] = useState(0);
  //const [API_KEY] = useState('YOUR_MAPTILER_API_KEY_HERE');

  useEffect(() => {
    if (map.current) return;

    map.current = new maplibregl.Map({
      container: mapContainer.current,
      style: 'https://api.maptiler.com/maps/basic-v2/style.json?key=R2mBAn4LWE1EZYkPEhdD',
      center: [lng, lat],
      zoom: zoom,
    });

    mapUtils(map.current)
    addControlPanel(map.current);
    addMarker(map.current);

  }, [ lng, lat, zoom]);

  return (
    <div className={`map-wrap bg-light`}>
      <div ref={mapContainer} className={`map bg-light`}/>
      <div className={`calculated-box`}>
        <div id={`calculated-area`}></div>
      </div>
    </div>
  
  );
}