import React from 'react';
import Radio from './Radio';
import CustomTypeahead from './CustomTypeahead';
import ScorePanel from './ScorePanel';
import api from '../api';

class EntranceScore extends React.Component {
    constructor(props) {
        super(props);

        this.controlProps = {
            radioName: 'radioScore',
            radioCollege: 'radioCollege',
            radioMajor: 'radioMajor',
            selectCollege: 'selectCollege',
            selectMajor: 'selectMajor',
            selectYear: 'selectYear'
        };
        this.nameSeparator = ' - ';
        this.inputChangeTimer = 0;
        this.inputChangeTimeout = 800;

        this.handleRadioCollegeClick = this.handleRadioCollegeClick.bind(this);
        this.handleRadioMajorClick = this.handleRadioMajorClick.bind(this);
        this.handleSelectCollege = this.handleSelectCollege.bind(this);
        this.handleSelectMajor = this.handleSelectMajor.bind(this);
        this.handleSelectYear = this.handleSelectYear.bind(this);
        this.handleCollegeInputChange = this.handleCollegeInputChange.bind(this);
        this.handleMajorInputChange = this.handleMajorInputChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);

        // Initial state for the component
        this.state = {
            radioChecked: {
                college: true,
                major: false
            },
            selectDisabled: {
                college: false,
                major: true
            },
            fetchedColleges: [],
            fetchedMajors: [],
            fetchedYears: [],
            selectedCollege: null,
            selectedMajor: null,
            selectedYears: [],
            scores: null
        };
    }

    handleRadioCollegeClick() {
        this.setState({
            radioChecked: {
                college: true,
                major: false
            },
            selectDisabled: {
                college: false,
                major: true
            }
        });
    }

    handleRadioMajorClick() {
        this.setState({
            radioChecked: {
                college: false,
                major: true
            },
            selectDisabled: {
                college: true,
                major: false
            }
        });
    }

    handleSelectCollege(collegeArray) {
        let selectedCollege;

        if (collegeArray.length) {
            const selectedString = collegeArray[0];
            const separatorPosition = selectedString.indexOf(this.nameSeparator);
            const collegeCode = selectedString.substring(0, separatorPosition);
            selectedCollege = this.state.fetchedColleges.find(item => item.code === collegeCode);
        } else {
            selectedCollege = null;
        }

        this.setState({ selectedCollege });
    }

    handleSelectMajor(majorArray) {
        let selectedMajor;

        if (majorArray.length) {
            const selectedString = majorArray[0];
            const separatorPosition = selectedString.indexOf(this.nameSeparator);
            const majorCode = selectedString.substring(0, separatorPosition);
            selectedMajor = this.state.fetchedMajors.find(item => item.code === majorCode);
        } else {
            selectedMajor = null;
        }

        this.setState({ selectedMajor });
    }

    handleSelectYear(selectedYears) {
        this.setState({ selectedYears: selectedYears.map(item => parseInt(item)) });
    }

    handleCollegeInputChange(input) {
        clearTimeout(this.inputChangeTimer);
        this.inputChangeTimer = setTimeout(
            () => {
                if (input.length >= 3) {
                    api.findCollegesByName(input)
                        .then(response => {
                            const fetchedColleges = response.body.slice(0, 15);
                            this.setState({ fetchedColleges });
                        })
                        .catch(error => console.log(error));
                }
            },
            this.inputChangeTimeout
        );
    }

    handleMajorInputChange(input) {
        clearTimeout(this.inputChangeTimer);
        this.inputChangeTimer = setTimeout(
            () => {
                if (input.length >= 3) {
                    api.findMajorsByName(input)
                        .then(response => {
                            const fetchedMajors = response.body.slice(0, 15);
                            this.setState({ fetchedMajors });
                        })
                        .catch(error => console.log(error));
                }
            },
            this.inputChangeTimeout
        );
    }

    handleSubmit() {
        const { radioChecked, selectedCollege, selectedMajor, selectedYears } = this.state;

        if (selectedYears.length) {
            if (radioChecked.college && selectedCollege) {
                const collegeDTO = {
                    collegeCode: selectedCollege.code,
                    years: selectedYears
                };

                api.getMajorScoresFromCollege(collegeDTO)
                    .then(response => this.setState({ scores: response.body }, () => console.log(this.state.scores)))
                    .catch(error => console.log(error));

            } else if (radioChecked.major && selectedMajor) {
                const majorDTO = {
                    majorCode: selectedMajor.code,
                    years: selectedYears
                };

                api.getCollegeScoresByMajor(majorDTO)
                    .then(response => this.setState({ scores: response.body }, () => console.log(this.state.scores)))
                    .catch(error => console.log(error));
            }
        }
    }

    componentDidMount() {
        api.getAllColleges()
            .then(response => {
                const fetchedColleges = response.body.slice(0, 10);
                this.setState({ fetchedColleges });
            })
            .catch(error => console.log(error));

        api.getAllMajors()
            .then(response => {
                const fetchedMajors = response.body.slice(0, 10);
                this.setState({ fetchedMajors });
            })
            .catch(error => console.log(error));

        api.getYears()
            .then(response => {
                let fetchedYears = response.body;
                this.setState({ fetchedYears });
            })
            .catch(error => console.log(error));
    }

    render() {
        return (
            <div>
                <div className="container">
                    <div className="row" >
                        <div className="col-12 p-3">
                            <div style={{ margin: '-8px' }}>
                                <form>
                                    <div className="form-row p-2">
                                        <div className="form-group col-4 d-flex align-items-center my-0">
                                            <Radio
                                                label="Theo trường ĐH-CĐ:"
                                                name={this.controlProps.radioName}
                                                id={this.controlProps.radioCollege}
                                                checked={this.state.radioChecked.college}
                                                handleRadioClick={this.handleRadioCollegeClick}
                                            />
                                        </div>
                                        <div className="form-group col-8 d-flex align-items-center my-0">
                                            <CustomTypeahead
                                                id={this.controlProps.selectCollege}
                                                options={this.state.fetchedColleges.map(({ code, name }) => code + this.nameSeparator + name)}
                                                disabled={this.state.selectDisabled.college}
                                                handleSelect={this.handleSelectCollege}
                                                handleInputChange={this.handleCollegeInputChange}
                                                placeholder="Chọn một trường..."
                                            />
                                        </div>
                                    </div>
                                    <div className="form-row p-2">
                                        <div className="form-group col-4 d-flex align-items-center my-0">
                                            <Radio
                                                label="Theo ngành/nhóm ngành:"
                                                name={this.controlProps.radioName}
                                                id={this.controlProps.radioMajor}
                                                checked={this.state.radioChecked.major}
                                                handleRadioClick={this.handleRadioMajorClick}
                                            />
                                        </div>
                                        <div className="form-group col-8 d-flex align-items-center my-0">
                                            <CustomTypeahead
                                                id={this.controlProps.selectMajor}
                                                options={this.state.fetchedMajors.map(({ code, name }) => code + this.nameSeparator + name)}
                                                disabled={this.state.selectDisabled.major}
                                                handleSelect={this.handleSelectMajor}
                                                handleInputChange={this.handleMajorInputChange}
                                                placeholder="Chọn một ngành..."
                                            />
                                        </div>
                                    </div>
                                    <div className="form-row p-2">
                                        <div className="form-group col-4 d-flex align-items-center my-0">
                                            Chọn năm:
                                        </div>
                                        <div className="form-group col-8 d-flex align-items-center my-0">
                                            <CustomTypeahead
                                                id={this.controlProps.selectYear}
                                                options={this.state.fetchedYears.map(item => item.toString())}
                                                multiple={true}
                                                handleSelect={this.handleSelectYear}
                                                placeholder="Chọn một hoặc nhiều năm..."
                                            />
                                        </div>
                                    </div>
                                    <div className="form-row p-2">
                                        <button
                                            className="btn btn-primary mx-auto"
                                            type="button"
                                            style={{ width: '20%' }}
                                            onClick={this.handleSubmit}
                                        >
                                            Tra cứu
                                        </button>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-12">
                            <ScorePanel scores={this.state.scores} />
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

export default EntranceScore;
