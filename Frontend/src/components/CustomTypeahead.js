import React from 'react';
import { Typeahead } from 'react-bootstrap-typeahead';

import 'react-bootstrap-typeahead/css/Typeahead.css';

class CustomTypeahead extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            selected: [],
            input: ''
        };
    }

    render() {
        const { id, options, placeholder, disabled, multiple, handleSelect, handleInputChange } = this.props;
        return (
            <Typeahead
                id={id}
                options={options}
                placeholder={placeholder}
                disabled={disabled}
                multiple={multiple}
                onChange={selected => this.setState({ selected }, () => {
                    if (typeof handleSelect === 'function') {
                        handleSelect(this.state.selected);
                    }
                })}
                onInputChange={input => this.setState({ input }, () => {
                    if (typeof handleInputChange === 'function') {
                        handleInputChange(this.state.input);
                    }
                })}
            />
        );
    }
}

export default CustomTypeahead;
