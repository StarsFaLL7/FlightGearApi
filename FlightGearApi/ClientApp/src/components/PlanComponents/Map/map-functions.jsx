import maplibregl from 'maplibre-gl';
import 'maplibre-gl/dist/maplibre-gl.css';

function showHowerPopupInfo(map){
    /* map.on('click', (e) => {
        new maplibregl.Popup().setHTML(`${Object.values(e.lngLat)}`).setLngLat(Object.values(e.lngLat)).addTo(map);
    }); */
};

function addMarker(map) {
    let markersSet = new Set();
    map.current.on('click', (e) => {
        markersSet.add(Object.values(e.lngLat));
        new maplibregl.Marker({color: "#0d6efd"})
          .setLngLat(Object.values(e.lngLat))
          .addTo(map.current)
          .on('click', (e) => {
            new maplibregl.Popup().setHTML(`${Object.values(e.lngLat)}`).setLngLat(Object.values(e.lngLat)).addTo(map);
          });
        console.log(markersSet); 
    });
};

function addControlPanel(map) {
    map.current.addControl(new maplibregl.NavigationControl(), 'top-left');
}

function lineConnect(map) {

}

export {showHowerPopupInfo, addMarker, addControlPanel};