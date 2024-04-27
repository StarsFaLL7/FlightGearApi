//говнокод ON

import maplibregl from 'maplibre-gl';
import 'maplibre-gl/dist/maplibre-gl.css';
import MapboxDraw from "@mapbox/mapbox-gl-draw";
import '@mapbox/mapbox-gl-draw/dist/mapbox-gl-draw.css'
import * as turf from '@turf/turf'

function addControlPanel(map) {
    map.addControl(new maplibregl.NavigationControl(), 'bottom-left');
}

function addMarker(map) {
    let markersSet = new Set();

    map.on('contextmenu', (e) => {
        const lngLat = Object.values(e.lngLat);
        markersSet.add(lngLat);
        const marker = createMarker(lngLat, map);
        const popup = createPopup(e, map, marker, markersSet);
        marker.setPopup(popup);
        return markersSet;
    });
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
        <div class="popup-con">
            <p>Longitude: ${data.lngLat.lng}</p>
            <p>Latitude: ${data.lngLat.lat}</p>
            <button class="btn delete-popup bg-primary" type="submit">
                delete
            </button>
        </div>
    `;

    const deleteButton = popupContent.querySelector('.delete-popup');

    deleteButton.onclick = function() {
        marker.remove();
        for(let i = 0; i < markArr.length; i++) {
            if(markArr[i].toString() === marker.getLngLat().toArray().toString()){
                markersSet.delete(markArr[i]);
            }
        }
    };

    let popup = new maplibregl.Popup().setDOMContent(popupContent).addTo(map);
    return popup;
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

export { addMarker, addControlPanel, mapUtils };
//говнокод OFF