//говнокод ON

import maplibregl, { Popup } from 'maplibre-gl';
import 'maplibre-gl/dist/maplibre-gl.css';
import MapboxDraw from "@mapbox/mapbox-gl-draw";
import '@mapbox/mapbox-gl-draw/dist/mapbox-gl-draw.css'
import * as turf from '@turf/turf'
//import plane from '../NavItem/imgs/plane.png'
//import MarkerPopover from '../MarkerPopover/marker-popover';

function addControlPanel(map) {
    map.addControl(new maplibregl.NavigationControl(), 'bottom-right');
}

/* function addMarker(map) {
    let markersSet = new Set();
    map.on('contextmenu', (e) => {
        const lngLat = Object.values(e.lngLat);
        markersSet.add(lngLat);
        const marker = createMarker(lngLat, map);
        const popup = createPopup(e, map, marker, markersSet);
        marker.setPopup(popup);
        updateLine(map, Array.from(markersSet).map(coord => [coord[0], coord[1]]));
        console.log(markersSet)
        return markersSet;
    });
    return markersSet;
}

function updateLine(map, coordinates) {
    console.log(coordinates)
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
    //getWay(map, coordinates);
    
}

function getMarkerSet(marks) {
    return marks;
}

function createMarker(lngLat, map) {
    return new maplibregl.Marker({ color: "#0d6efd" })
        .setLngLat(lngLat)
        .addTo(map);
}

function createPopup(data, map, marker, markersSet) {
    let markArr = Array.from(markersSet)
    let popupContent = document.createElement('div');

    popupContent.innerHTML = `
    <div id="form" class="popup-con">
        <p name="longitude" required>Longitude: ${data.lngLat.lng}</p>
        <p name="latitude" required>Latitude: ${data.lngLat.lat}</p>
        <p class="d-flex align-items-center" name="altitude" required>Altitude: <input class="altitude form-control" /></p>
        <p class="d-flex align-items-center" name="speed" required>Speed: <input class="speed form-control" /></p>
        <button class="btn save-popup btn-primary text-light" onClick=${(evt) => handleClickAddPoint(evt)} type="submit">
            save
        </button>
        <button class="btn delete-popup btn-secondary text-light" type="submit">
            delete
        </button>
    </div>
    `;

    console.log(popupContent)

    const deleteButton = popupContent.querySelector('.delete-popup');

    deleteButton.onclick = function() {
        marker.remove();
        for(let i = 0; i < markArr.length; i++) {
            if(markArr[i].toString() === marker.getLngLat().toArray().toString()){
                markersSet.delete(markArr[i]);
            }
        }
        updateLine(map, Array.from(markersSet).map(coord => [coord[0], coord[1]]));
    };

    let popup = new maplibregl.Popup().setDOMContent(popupContent).addTo(map);
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
} */

/* export { addMarker, addControlPanel, mapUtils, getMousePosition, getMarkerSet }; */
//говнокод OFF