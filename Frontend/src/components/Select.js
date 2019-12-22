import React from 'react';

const Select = ({ id, values, disabled, handleSelect }) => {
    return (
        <select
            className="form-control"
            id={id}
            defaultValue={values[0]}
            disabled={disabled}
            onChange={event => handleSelect(event.target.value)}
        >
            {values.map((item, index) => <option key={index} value={item}>{item}</option>)}
        </select>
    );
};

export default Select;
