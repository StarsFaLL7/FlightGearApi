import plus from './img/plus.png'
import minus from './img/minus.png'
import React, { useState } from 'react';

function imageChanger(name){
    // eslint-disable-next-line react-hooks/rules-of-hooks
    const [bool, setBool] = useState(true);
    return <button onClick={()=>{bool?setBool(false):setBool(true)}}>{name}<img src={bool ? plus:minus} width="20" height="20" alt={bool ? "Плюс":"Минус"} /></button>
};

export {imageChanger};