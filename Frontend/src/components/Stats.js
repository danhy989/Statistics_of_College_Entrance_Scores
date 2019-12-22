import React from 'react';
import { LineGraph, BarChart } from './Chart';
import CustomTypeahead from './CustomTypeahead';
import Select from './Select';
import api from '../api';

export class MajorScoreOverYears extends React.Component {
    constructor(props) {
        super(props);

        this.controlProps = {
            selectCollege: 'selectCollegeStats',
            selectMajor: 'selectMajorStats'
        };
        this.nameSeparator = ' - ';
        this.inputChangeTimer = 0;
        this.inputChangeTimeout = 800;

        this.handleCollegeInputChange = this.handleCollegeInputChange.bind(this);
        this.handleSelectCollege = this.handleSelectCollege.bind(this);
        this.handleSelectMajor = this.handleSelectMajor.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);

        // Initial state for the component
        this.state = {
            selectDisabled: {
                college: false,
                major: true
            },
            fetchedColleges: [],
            fetchedMajors: [],
            selectedCollege: null,
            selectedMajor: null,
            chartData: {
                collegeName: '',
                majorName: '',
                data: null
            }
        };
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

    handleSelectCollege(collegeArray) {
        if (collegeArray.length) {
            const selectedString = collegeArray[0];
            const separatorPosition = selectedString.indexOf(this.nameSeparator);
            const collegeCode = selectedString.substring(0, separatorPosition);
            const selectedCollege = this.state.fetchedColleges.find(item => item.code === collegeCode);

            //fetch majors here

            this.setState({
                selectDisabled: {
                    college: false,
                    major: false
                },
                selectedCollege
            });
        } else {
            this.setState({
                selectDisabled: {
                    college: false,
                    major: true
                },
                selectedCollege: null
            });
        }
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

    handleSubmit() {
        const majorCollegeDTO = {
            collegeId: 'QSC',
            majorId: '7480103'
        };

        api.getMajorScoreOverYears(majorCollegeDTO)
            .then(response => this.processData(response))
            .catch(error => console.log(error));

        /* const { selectedCollege, selectedMajor } = this.state;
        if (selectedCollege && selectedMajor) {
            const majorCollegeDTO = {
                collegeId: selectedCollege.code,
                majorId: selectedMajor.code
            };
        } */
    }

    processData(raw) {
        const { collegeName, majorName, scores } = raw.body;

        const chartData = {
            collegeName,
            majorName,
            data: {
                labels: scores.map(item => item.year),
                datasets: [
                    {
                        label: 'Điểm chuẩn',
                        backgroundColor: 'rgba(0, 0, 0, 0.1)',
                        borderColor: 'rgba(0, 0, 0, 1)',
                        borderWidth: 1,
                        data: scores.map(item => item.score),
                        lineTension: 0
                    }
                ]
            }
        };

        this.setState({ chartData });
    }

    componentDidMount() {
        api.getAllColleges()
            .then(response => {
                const fetchedColleges = response.body.slice(0, 10);
                this.setState({ fetchedColleges });
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
                                            Chọn trường ĐH-CĐ:
                                        </div>
                                        <div className="form-group col-8 d-flex align-items-center my-0">
                                            <CustomTypeahead
                                                id={this.controlProps.selectCollege}
                                                options={this.state.fetchedColleges.map(({ code, name }) => code + this.nameSeparator + name)}
                                                disabled={this.state.selectDisabled.college}
                                                handleInputChange={this.handleCollegeInputChange}
                                                handleSelect={this.handleSelectCollege}
                                                placeholder="Chọn một trường..."
                                            />
                                        </div>
                                    </div>
                                    <div className="form-row p-2">
                                        <div className="form-group col-4 d-flex align-items-center my-0">
                                            Chọn ngành:
                                        </div>
                                        <div className="form-group col-8 d-flex align-items-center my-0">
                                            <CustomTypeahead
                                                id={this.controlProps.selectMajor}
                                                options={this.state.fetchedMajors.map(({ code, name }) => code + this.nameSeparator + name)}
                                                disabled={this.state.selectDisabled.major}
                                                handleSelect={this.handleSelectMajor}
                                                placeholder="Chọn một ngành..."
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
                            <div>
                                <LineGraph
                                    data={this.state.chartData.data}
                                    options={
                                        {
                                            title: {
                                                display: true,
                                                text: `Điểm chuẩn ngành ${this.state.chartData.majorName} của trường ${this.state.chartData.collegeName} qua các năm`,
                                                fontSize: 18
                                            },
                                            legend: {
                                                display: false
                                            },
                                            scales: {
                                                yAxes: [
                                                    {
                                                        ticks: {
                                                            suggestedMin: 0,
                                                            suggestedMax: 30
                                                        }
                                                    }
                                                ]
                                            }
                                        }
                                    }
                                />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}





























export class CompareScoreBetweenColleges extends React.Component {
    constructor(props) {
        super(props);

        this.controlProps = {
            selectMajor: 'selectMajorCompare',
            selectCollege: 'selectCollegeCompare',
            selectYear: 'selectYearCompare'
        };
        this.nameSeparator = ' - ';
        this.inputChangeTimer = 0;
        this.inputChangeTimeout = 800;

        this.handleMajorInputChange = this.handleMajorInputChange.bind(this);
        this.handleSelectMajor = this.handleSelectMajor.bind(this);
        this.handleSelectYear = this.handleSelectYear.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);

        // Initial state for the component
        this.state = {
            selectDisabled: {
                college: true,
                major: false
            },
            fetchedMajors: [],
            fetchedColleges: [],
            fetchedYears: [],
            selectedMajor: null,
            selectedColleges: [],
            selectedYear: null,
            chartData: {
                majorName: '',
                year: '',
                data: null
            }
        };
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

    handleSelectMajor(majorArray) {
        if (majorArray.length) {
            const selectedString = majorArray[0];
            const separatorPosition = selectedString.indexOf(this.nameSeparator);
            const majorCode = selectedString.substring(0, separatorPosition);
            const selectedMajor = this.state.fetchedMajors.find(item => item.code === majorCode);

            //fetch colleges here

            this.setState({
                selectDisabled: {
                    college: false,
                    major: false
                },
                selectedMajor
            });
        } else {
            this.setState({
                selectDisabled: {
                    college: true,
                    major: false
                },
                selectedMajor: null
            });
        }
    }

    handleSelectCollege(collegeArray) {
        let selectedColleges;

        if (collegeArray.length) {
            selectedColleges = collegeArray.map(selectedString => {
                const separatorPosition = selectedString.indexOf(this.nameSeparator);
                const collegeCode = selectedString.substring(0, separatorPosition);
                return this.state.fetchedColleges.find(item => item.code === collegeCode);
            });
        } else {
            selectedColleges = [];
        }

        this.setState({ selectedColleges });
    }

    handleSelectYear(selectedYear) {
        this.setState({ selectedYear: parseInt(selectedYear) });
    }

    handleSubmit() {
        const compareDTO = {
            majorCode: '7480201',
            collegeCodes: ['QSC', 'QST'],
            years: [2016]
        };

        api.compareMajorScoreBetweenColleges(compareDTO)
            .then(response => console.log(response.body))
            .catch(error => console.log(error));

        /* const { selectedMajor, selectedYear } = this.state;
        if (selectedCollege && selectedMajor) {
            const majorCollegeDTO = {
                collegeId: selectedCollege.code,
                majorId: selectedMajor.code
            };
        } */
    }

    processData(raw) { }

    componentDidMount() {
        api.getAllMajors()
            .then(response => {
                const fetchedMajors = response.body.slice(0, 10);
                this.setState({ fetchedMajors });
            })
            .catch(error => console.log(error));

        api.getYears()
            .then(response => {
                let fetchedYears = response.body;
                this.setState({ fetchedYears, selectedYear: fetchedYears[0] });
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
                                            Chọn ngành:
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
                                            Chọn trường ĐH-CĐ:
                                        </div>
                                        <div className="form-group col-8 d-flex align-items-center my-0">
                                            <CustomTypeahead
                                                id={this.controlProps.selectCollege}
                                                options={this.state.fetchedColleges.map(({ code, name }) => code + this.nameSeparator + name)}
                                                disabled={this.state.selectDisabled.college}
                                                handleSelect={this.handleSelectCollege}
                                                multiple={true}
                                                placeholder="Chọn ít nhất hai trường..."
                                            />
                                        </div>
                                    </div>
                                    <div className="form-row p-2">
                                        <div className="form-group col-4 d-flex align-items-center my-0">
                                            Chọn năm:
                                        </div>
                                        <div className="form-group col-8 d-flex align-items-center my-0">
                                            <Select
                                                id={this.controlProps.selectYear}
                                                values={this.state.fetchedYears.map(item => item.toString())}
                                                handleSelect={this.handleSelectYear}
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
                            <div>
                                <BarChart
                                    data={this.state.chartData.data}
                                    options={
                                        {
                                            title: {
                                                display: true,
                                                text: `Điểm chuẩn ngành ${this.state.chartData.majorName} giữa các trường năm ${this.state.chartData.year}`,
                                                fontSize: 18
                                            },
                                            legend: {
                                                display: false
                                            },
                                            scales: {
                                                yAxes: [
                                                    {
                                                        ticks: {
                                                            suggestedMin: 0,
                                                            suggestedMax: 30
                                                        }
                                                    }
                                                ]
                                            }
                                        }
                                    }
                                />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}
