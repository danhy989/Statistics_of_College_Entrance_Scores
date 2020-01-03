import React from 'react';
import { LineGraph, BarChart } from './Chart';
import CustomTypeahead from './CustomTypeahead';
import Select from './Select';
import MessageBox from './MessageBox';
import api from '../api';
import { CURRENT_YEAR, MAX_FETCH_ITEMS } from '../values';
import { isArrayTruthy } from '../helpers';

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
        this.handleMessageBoxClose = this.handleMessageBoxClose.bind(this);

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
            },
            errorMessageBox: {
                show: false,
                message: ''
            }
        };
    }

    processData(raw) {
        const { collegeName, majorName, scores } = raw.body;
        return {
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
                        lineTension: 0.1
                    }
                ]
            }
        };
    }

    toggleMessageBox(show, message) {
        this.setState({
            errorMessageBox: { show, message }
        });
    }

    handleCollegeInputChange(input) {
        clearTimeout(this.inputChangeTimer);
        this.inputChangeTimer = setTimeout(
            () => {
                if (input.length >= 3) {
                    api.findCollegesByName(input)
                        .then(response => {
                            const fetchedColleges = response.body.slice(0, MAX_FETCH_ITEMS);
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

            //fetch latest majors of the selected college
            api.getMajorScoresFromCollege({
                collegeCode: selectedCollege.code,
                years: [CURRENT_YEAR]
            })
                .then(response => {
                    const fetchedMajors = response.body.majors[0].majors.map(
                        ({ majorCode: code, majorName: name, groupCode }) => ({ code, name, groupCode })
                    );

                    this.setState({
                        selectDisabled: {
                            college: false,
                            major: false
                        },
                        selectedCollege,
                        fetchedMajors
                    });
                })
                .catch(error => console.log(error));
        } else {
            this.setState({
                selectDisabled: {
                    college: false,
                    major: true
                },
                selectedCollege: null,
                fetchedMajors: [],
                selectedMajor: null
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
        const { selectedCollege, selectedMajor } = this.state;

        if (!selectedCollege) {
            this.toggleMessageBox(true, 'Vui lòng chọn một trường.');
            return;
        }

        if (!selectedMajor) {
            this.toggleMessageBox(true, 'Vui lòng chọn một ngành của trường đã chọn.');
            return;
        }

        const majorCollegeDTO = {
            collegeId: selectedCollege.code,
            majorId: selectedMajor.code
        };

        api.getMajorScoreOverYears(majorCollegeDTO)
            .then(response => {
                const chartData = this.processData(response);
                this.setState({ chartData });
            })
            .catch(error => console.log(error));

    }

    handleMessageBoxClose() {
        this.toggleMessageBox(false, '');
    }

    componentDidMount() {
        api.getAllColleges()
            .then(response => {
                const fetchedColleges = response.body.slice(0, MAX_FETCH_ITEMS);
                this.setState({ fetchedColleges });
            })
            .catch(error => console.log(error));
    }

    render() {
        return (
            <div>
                <div className="container">
                    <div className="row">
                        <div className="col-12 p-4">
                            <h3 className="m-0">Điểm chuẩn ngành qua các năm</h3>
                        </div>
                    </div>
                    <div className="row" >
                        <div className="col-12 p-4">
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
                        <div className="col-12 p-4">
                            <div>
                                <LineGraph
                                    data={this.state.chartData.data}
                                    options={{
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
                                        },
                                        plugins: {
                                            datalabels: {
                                                display: true,
                                                color: 'rgba(0, 0, 0, 1)',
                                                anchor: 'end',
                                                align: '-45',
                                                offset: '-1'
                                            }
                                        }
                                    }}
                                />
                            </div>
                        </div>
                    </div>
                </div>
                <MessageBox
                    show={this.state.errorMessageBox.show}
                    message={this.state.errorMessageBox.message}
                    handleClose={this.handleMessageBoxClose}
                />
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
        this.handleSelectCollege = this.handleSelectCollege.bind(this);
        this.handleSelectYear = this.handleSelectYear.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleMessageBoxClose = this.handleMessageBoxClose.bind(this);

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
            },
            errorMessageBox: {
                show: false,
                message: ''
            }
        };
    }

    processData(raw) {
        const { majorName, year, colleges: scores } = raw.body;
        return {
            majorName,
            year,
            data: {
                labels: scores.map(item => item.collegeName),
                datasets: [
                    {
                        label: 'Điểm chuẩn',
                        backgroundColor: 'rgba(75, 192, 192, 1)',
                        borderColor: 'rgba(0, 0, 0, 1)',
                        borderWidth: 1,
                        data: scores.map(item => item.score)
                    }
                ]
            }
        };
    }

    toggleMessageBox(show, message) {
        this.setState({
            errorMessageBox: { show, message }
        });
    }

    handleMajorInputChange(input) {
        clearTimeout(this.inputChangeTimer);
        this.inputChangeTimer = setTimeout(
            () => {
                if (input.length >= 3) {
                    api.findMajorsByName(input)
                        .then(response => {
                            const fetchedMajors = response.body.slice(0, MAX_FETCH_ITEMS);
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

            //fetch colleges that have the selected major
            api.getCollegeScoresByMajor({
                majorCode: selectedMajor.code,
                years: [CURRENT_YEAR]
            })
                .then(response => {
                    const fetchedColleges = response.body.colleges[0].colleges.map(
                        ({ collegeCode: code, collegeName: name, groupCode }) => ({ code, name, groupCode })
                    );

                    this.setState({
                        selectDisabled: {
                            college: false,
                            major: false
                        },
                        selectedMajor,
                        fetchedColleges
                    });
                })
                .catch(error => console.log(error));
        } else {
            this.setState({
                selectDisabled: {
                    college: true,
                    major: false
                },
                selectedMajor: null,
                fetchedColleges: [],
                selectedColleges: []
            });
        }
    }

    handleSelectCollege(collegeArray) {
        let selectedColleges;

        if (collegeArray.length) {
            const mappedArray = collegeArray.map(selectedString => {
                const separatorPosition = selectedString.indexOf(this.nameSeparator);
                const collegeCode = selectedString.substring(0, separatorPosition);
                return this.state.fetchedColleges.find(item => item.code === collegeCode);
            });
            selectedColleges = isArrayTruthy(mappedArray) ? mappedArray : [];
        } else {
            selectedColleges = [];
        }

        this.setState({ selectedColleges });
    }

    handleSelectYear(selectedYear) {
        this.setState({ selectedYear: parseInt(selectedYear) });
    }

    handleSubmit() {
        const { selectedMajor, selectedColleges, selectedYear } = this.state;

        if (!selectedMajor) {
            this.toggleMessageBox(true, 'Vui lòng chọn một ngành.');
            return;
        }

        if (selectedColleges.length < 2) {
            this.toggleMessageBox(
                true,
                'Vui lòng chọn ít nhất hai trường có đào tạo ngành đã chọn và loại bỏ những trường không đào tạo ngành này (nếu có).'
            );
            return;
        }

        if (!selectedYear) {
            this.toggleMessageBox(true, 'Vui lòng chọn một năm.');
            return;
        }

        const compareDTO = {
            majorCode: selectedMajor.code,
            collegeCodes: selectedColleges.map(item => item.code),
            year: selectedYear
        };

        api.compareMajorScoreBetweenColleges(compareDTO)
            .then(response => {
                const chartData = this.processData(response);
                this.setState({ chartData });
            })
            .catch(error => console.log(error));
    }

    handleMessageBoxClose() {
        this.toggleMessageBox(false, '');
    }

    componentDidMount() {
        api.getAllMajors()
            .then(response => {
                const fetchedMajors = response.body.slice(0, MAX_FETCH_ITEMS);
                this.setState({ fetchedMajors });
            })
            .catch(error => console.log(error));

        api.getYears()
            .then(response => {
                const fetchedYears = response.body;
                this.setState({ fetchedYears, selectedYear: fetchedYears[0] });
            })
            .catch(error => console.log(error));
    }

    render() {
        return (
            <div>
                <div className="container">
                    <div className="row">
                        <div className="col-12 p-4">
                            <h3 className="m-0">So sánh điểm chuẩn giữa các trường</h3>
                        </div>
                    </div>
                    <div className="row" >
                        <div className="col-12 p-4">
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
                                        <div className="form-group col-8 d-flex align-items-center my-0" id="multiline-custom-typeahead">
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
                        <div className="col-12 p-4">
                            <div>
                                <BarChart
                                    data={this.state.chartData.data}
                                    options={{
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
                                        },
                                        plugins: {
                                            datalabels: {
                                                display: true,
                                                color: 'rgba(0, 0, 0, 1)',
                                                anchor: 'end',
                                                align: 'top',
                                                offset: '0'
                                            }
                                        }
                                    }}
                                />
                            </div>
                        </div>
                    </div>
                </div>
                <MessageBox
                    show={this.state.errorMessageBox.show}
                    message={this.state.errorMessageBox.message}
                    handleClose={this.handleMessageBoxClose}
                />
            </div>
        );
    }
}
