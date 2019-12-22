import React from 'react';
export class RadioGroup extends React.Component {
    constructor(props) {
        super(props);

        this.state = {

        }
    }
    render() {
        return (
            <div className="custom-control custom-radio">
                <input type="radio" className="custom-control-input" id="defaultChecked2" name="defaultExample2" defaultChecked />
                <label className="custom-control-label" htmlFor="defaultChecked2">Default checked</label>
            </div>
        );
    }
}