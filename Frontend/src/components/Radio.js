import React from 'react';

const Radio = ({ name, id, label, checked, handleRadioClick }) => {
    return (
        <div className="form-check">
            <input className="form-check-input" type="radio" name={name} id={id} checked={checked} onChange={() => { }}
                onClick={() => handleRadioClick(id)} />
            <label className="form-check-label" htmlFor={id}>
                {label}
            </label>
        </div>
    );
};

export default Radio;
