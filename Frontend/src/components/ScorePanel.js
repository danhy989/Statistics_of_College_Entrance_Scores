import React from 'react';
import Table from 'react-bootstrap/Table';

class ScorePanel extends React.Component {
    renderTable() {
        const { scores } = this.props;

        if (scores && scores.collegeCode) {
            return this.renderMajorScores();
        } else if (scores && scores.majorCode) {
            return this.renderCollegeScores();
        }

        return null;
    }

    renderMajorScores() {
        const { collegeCode, collegeName, majors: years } = this.props.scores;
        return (
            <div>
                <h4>Điểm chuẩn của trường {collegeName} (Mã trường: {collegeCode})</h4>
                {
                    years.map(
                        ({ year, majors }) => (
                            <div className="year-table p-2 mb-2">
                                <h6>Năm {year}</h6>
                                <Table bordered hover>
                                    <thead>
                                        <tr>
                                            <th>STT</th>
                                            <th>Tên ngành</th>
                                            <th>Mã ngành</th>
                                            <th>Tổ hợp môn</th>
                                            <th>Điểm chuẩn</th>
                                            <th>Ghi chú</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {
                                            majors.map(
                                                ({ majorName, majorCode, groupCode, score, info }, index) => (
                                                    <tr>
                                                        <td>{index + 1}</td>
                                                        <td>{majorName}</td>
                                                        <td>{majorCode}</td>
                                                        <td>{groupCode}</td>
                                                        <td>{score}</td>
                                                        <td>{info}</td>
                                                    </tr>
                                                )
                                            )
                                        }
                                    </tbody>
                                </Table>
                            </div>
                        )
                    )
                }
            </div>
        );
    }

    renderCollegeScores() {
        const { majorCode, majorName, colleges: years } = this.props.scores;
        return (
            <div>
                <h4>Điểm chuẩn của ngành/nhóm ngành {majorName} (Mã ngành: {majorCode})</h4>
                {
                    years.map(
                        ({ year, colleges }) => (
                            <div className="year-table p-2 mb-2">
                                <h6>Năm {year}</h6>
                                <Table bordered hover>
                                    <thead>
                                        <tr>
                                            <th>STT</th>
                                            <th>Tên trường</th>
                                            <th>Mã trường</th>
                                            <th>Tổ hợp môn</th>
                                            <th>Điểm chuẩn</th>
                                            <th>Ghi chú</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {
                                            colleges.map(
                                                ({ collegeName, collegeCode, groupCode, score, info }, index) => (
                                                    <tr>
                                                        <td>{index + 1}</td>
                                                        <td>{collegeName}</td>
                                                        <td>{collegeCode}</td>
                                                        <td>{groupCode}</td>
                                                        <td>{score}</td>
                                                        <td>{info}</td>
                                                    </tr>
                                                )
                                            )
                                        }
                                    </tbody>
                                </Table>
                            </div>
                        )
                    )
                }
            </div>
        );
    }

    render() {
        return (
            <div className="score-panel">
                {this.renderTable()}
            </div>
        );
    }
}

export default ScorePanel;
