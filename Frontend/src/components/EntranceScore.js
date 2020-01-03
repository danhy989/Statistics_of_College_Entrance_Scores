import React from 'react';
import Radio from './Radio';
import CustomTypeahead from './CustomTypeahead';
import ScorePanel from './ScorePanel';
import MessageBox from './MessageBox';
import api from '../api';
import { MAX_FETCH_ITEMS } from '../values';

class EntranceScore extends React.Component {
    constructor(props) {
        super(props);

        this.controlProps = {
            radioName: 'radioScore',
            radioCollege: 'radioCollege',
            radioMajor: 'radioMajor',
            selectCollege: 'selectCollege',
            selectMajor: 'selectMajor',
            selectYear: 'selectYear',
            selectProvince: 'selectProvince',
            selectGroupCode: 'selectGroupCode'
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
        this.handleMessageBoxClose = this.handleMessageBoxClose.bind(this);
        this.handleSelectProvince = this.handleSelectProvince.bind(this);
        this.handleSelectGroupCode = this.handleSelectGroupCode.bind(this);

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
            fetchedProvinces: [],
            fetchedGroupCodes: [],
            selectedCollege: null,
            selectedMajor: null,
            selectedYears: [],
            selectedProvince: null,
            selectedGroupCode: null,
            scores: null,
            errorMessageBox: {
                show: false,
                message: ''
            }
        };
    }

    toggleMessageBox(show, message) {
        this.setState({
            errorMessageBox: { show, message }
        });
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

    handleSelectProvince(provinceArray) {
        if (provinceArray.length) {
            const selectedString = provinceArray[0];
            const separatorPosition = selectedString.indexOf(this.nameSeparator);
            const provinceId = parseInt(selectedString.substring(0, separatorPosition));
            const selectedProvince = this.state.fetchedProvinces.find(item => item.province_id === provinceId);

            api.getCollegesByProvince(provinceId)
                .then(response => {
                    this.setState({
                        selectedProvince,
                        fetchedColleges: response.body
                    });
                })
                .catch(error => console.log(error));
        } else {
            this.setState({
                selectedProvince: null,
                fetchedColleges: []
            });
        }
    }

    handleSelectGroupCode(groupCodeArray) {
        if (groupCodeArray.length) {
            const selectedGroupCode = groupCodeArray[0];

            api.getMajorsByGroupCode(selectedGroupCode)
                .then(response => {
                    this.setState({
                        selectedGroupCode,
                        fetchedMajors: response.body
                    });
                })
                .catch(error => console.log(error));
        } else {
            this.setState({
                selectedGroupCode: null,
                fetchedMajors: []
            });
        }
    }

    handleCollegeInputChange(input) {
        if (input.length >= 3 && !this.state.selectedProvince) {
            clearTimeout(this.inputChangeTimer);
            this.inputChangeTimer = setTimeout(
                () => api.findCollegesByName(input)
                    .then(response => {
                        const fetchedColleges = response.body.slice(0, MAX_FETCH_ITEMS);
                        this.setState({ fetchedColleges });
                    })
                    .catch(error => console.log(error)),
                this.inputChangeTimeout
            );
        }
    }

    handleMajorInputChange(input) {
        if (input.length >= 3 && !this.state.selectedGroupCode) {
            clearTimeout(this.inputChangeTimer);
            this.inputChangeTimer = setTimeout(
                () => api.findMajorsByName(input)
                    .then(response => {
                        const fetchedMajors = response.body.slice(0, MAX_FETCH_ITEMS);
                        this.setState({ fetchedMajors });
                    })
                    .catch(error => console.log(error)),
                this.inputChangeTimeout
            );
        }
    }

    handleSubmit() {
        const { radioChecked, selectedCollege, selectedMajor, selectedYears } = this.state;

        if (radioChecked.college && !selectedCollege) {
            this.toggleMessageBox(true, 'Vui lòng chọn một trường.');
            return;
        }

        if (radioChecked.major && !selectedMajor) {
            this.toggleMessageBox(true, 'Vui lòng chọn một ngành.');
            return;
        }

        if (!selectedYears.length) {
            this.toggleMessageBox(true, 'Vui lòng chọn ít nhất một năm.');
            return;
        }

        if (radioChecked.college) {
            const collegeDTO = {
                collegeCode: selectedCollege.code,
                years: selectedYears
            };

            api.getMajorScoresFromCollege(collegeDTO)
                .then(response => this.setState({ scores: response.body }))
                .catch(error => console.log(error));

        } else if (radioChecked.major) {
            const majorDTO = {
                majorCode: selectedMajor.code,
                years: selectedYears
            };

            api.getCollegeScoresByMajor(majorDTO)
                .then(response => this.setState({ scores: response.body }))
                .catch(error => console.log(error));
        }
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

        api.getAllMajors()
            .then(response => {
                const fetchedMajors = response.body.slice(0, MAX_FETCH_ITEMS);
                this.setState({ fetchedMajors });
            })
            .catch(error => console.log(error));

        api.getYears()
            .then(response => {
                const fetchedYears = response.body;
                this.setState({ fetchedYears });
            })
            .catch(error => console.log(error));

        api.getAllProvinces()
            .then(response => {
                const fetchedProvinces = response.body;
                this.setState({ fetchedProvinces });
            })
            .catch(error => console.log(error));

        api.getAllGroupCodes()
            .then(response => {
                const fetchedGroupCodes = response.body;
                this.setState({ fetchedGroupCodes });
            })
            .catch(error => console.log(error));
    }

    render() {
        return (
            <div>
                <div className="container">
                    <div className="row">
                        <div className="col-12 p-4">
                            <h3 className="m-0">Tra cứu điểm</h3>
                        </div>
                    </div>
                    <div className="row" >
                        <div className="col-12 p-4">
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
                                        <div className="form-group col-4 d-flex align-items-center my-0"></div>
                                        <div className="form-group col-8 d-flex align-items-center my-0">
                                            <div className="pr-3">Tìm trường theo tỉnh thành:</div>
                                            <CustomTypeahead
                                                id={this.controlProps.selectProvince}
                                                options={this.state.fetchedProvinces.map(({ province_id: id, name }) => id + this.nameSeparator + name)}
                                                disabled={this.state.selectDisabled.college}
                                                handleSelect={this.handleSelectProvince}
                                                placeholder="Chọn một tỉnh..."
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
                                        <div className="form-group col-4 d-flex align-items-center my-0"></div>
                                        <div className="form-group col-8 d-flex align-items-center my-0">
                                            <div className="pr-3">Tìm ngành theo tổ hợp môn:</div>
                                            <CustomTypeahead
                                                id={this.controlProps.selectGroupCode}
                                                options={this.state.fetchedGroupCodes}
                                                disabled={this.state.selectDisabled.major}
                                                handleSelect={this.handleSelectGroupCode}
                                                placeholder="Chọn một tổ hợp môn..."
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
                        <div className="col-12 p-4">
                            <ScorePanel scores={this.state.scores} />
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

export default EntranceScore;
