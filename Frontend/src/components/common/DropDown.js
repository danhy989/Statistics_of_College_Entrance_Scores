import React from 'react';
import { Dropdown} from 'react-bootstrap';

const options = ["Option 1", "Option 2", "Option 3", "Option 4", "Option 5"];
export class DropDown extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            selectedOption: options[0] // default selected value
        };
    }

    handleSelect(eventKey, event) {
        console.log(eventKey);
        console.log('sds');
        //this.setState({ selectedOption: options[eventKey] });
    }

    render() {
        return (
            <Dropdown title={this.state.selectedOption} onSelect={this.handleSelect(this)}>
                <Dropdown.Toggle variant="success" id="dropdown-basic">
                    Dropdown Button
            </Dropdown.Toggle>

            {options.map((opt, i) => (
          <Dropdown.Item key={i} eventKey={i}>
            {opt}
          </Dropdown.Item>
        ))}
            </Dropdown>
        );
    }
}